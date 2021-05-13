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
                    GameManager.instance.currentCity.AddOccupyingUnits(UnitFactory.instance.GenerateInfantry(numTroops));
                    quantityInput.text = "1";
                });
                purchaseButton.GetComponentInChildren<Text>().text = "Infantry";
                break;
            case TroopClassifications.RANGED:
                purchaseButton.onClick.AddListener(() => {
                    int numTroops = int.Parse(quantityInput.GetComponentInChildren<Text>().text);
                    GameManager.instance.currentCity.AddOccupyingUnits(UnitFactory.instance.GenerateRanged(numTroops));
                    quantityInput.text = "1";
                });
                purchaseButton.GetComponentInChildren<Text>().text = "Ranged";
                break;
            case TroopClassifications.CAVALRY:
                purchaseButton.onClick.AddListener(() => {
                    int numTroops = int.Parse(quantityInput.GetComponentInChildren<Text>().text);
                    GameManager.instance.currentCity.AddOccupyingUnits(UnitFactory.instance.GenerateCavalry(numTroops));
                    quantityInput.text = "1";
                });
                purchaseButton.GetComponentInChildren<Text>().text = "Cavalry";
                break;
        }
    }
}
