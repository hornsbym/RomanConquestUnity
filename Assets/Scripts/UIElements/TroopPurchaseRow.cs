using UnityEngine.UI;
using UnityEngine;

public class TroopPurchaseRow : MonoBehaviour
{
    public Button purchaseButton;
    public InputField quantityInput;

    TroopClassifications classification;

    private City currentCity;

    void Awake() {
        EventManager.OnCitySelected += setCurrentCity;
    }

    private void setCurrentCity(City city) 
    {
        this.currentCity = city;
    }

    /// <summary>
    /// Adds text to the button.
    /// Adds on-click functionality to the purchase button.
    /// </summary>
    public void Initialize(TroopClassifications spawnClass)
    {
        classification = spawnClass;
        CreateButtonFromSpawnClass();
    }

    private void CreateButtonFromSpawnClass()
    {
        switch (classification)
        {
            case TroopClassifications.INFANTRY:
                purchaseButton.onClick.AddListener(() => {
                    int numTroops = int.Parse(quantityInput.GetComponentInChildren<Text>().text);
                    for(int i = 0; i < numTroops; i++) {
                        GameManager.instance.currentCity.AddUnit(UnitFactory.instance.GenerateInfantry());
                    }
                    quantityInput.text = "1";
                });
                purchaseButton.GetComponentInChildren<Text>().text = "Infantry";
                break;
            case TroopClassifications.RANGED:
                purchaseButton.onClick.AddListener(() => {
                    int numTroops = int.Parse(quantityInput.GetComponentInChildren<Text>().text);
                    for (int i = 0; i < numTroops; i++)
                    {
                        GameManager.instance.currentCity.AddUnit(UnitFactory.instance.GenerateRanged());
                    }
                    quantityInput.text = "1";
                });
                purchaseButton.GetComponentInChildren<Text>().text = "Ranged";
                break;
            case TroopClassifications.CAVALRY:
                purchaseButton.onClick.AddListener(() => {
                    int numTroops = int.Parse(quantityInput.GetComponentInChildren<Text>().text);
                    for (int i = 0; i < numTroops; i++)
                    {
                        GameManager.instance.currentCity.AddUnit(UnitFactory.instance.GenerateCavalry());
                    }
                    quantityInput.text = "1";
                });
                purchaseButton.GetComponentInChildren<Text>().text = "Cavalry";
                break;
        }
    }
}
