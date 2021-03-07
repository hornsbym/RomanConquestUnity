using UnityEngine.UI;
using UnityEngine;

public class PurchasesSection : MonoBehaviour
{
    private City selectedCity;
    public GameObject buttonAnchor;
    public Button purchaseButtonPrefab;

    void Start()
    {
        selectedCity = (City) GameManager.instance.placeSelection;
        CreateButtons();
    }

    /// <summary>
    /// Creaets the buttons and adds them to the UI widget.
    /// </summary>
    private void CreateButtons() 
    {
        Vector3 buttonAnchorPos = buttonAnchor.transform.position;

        for (int i = 0; i < selectedCity.unitsForSale.Length; i++)
        {
            Vector3 newButtonPos = buttonAnchorPos;
            newButtonPos.y -= i * 35;

            Button newButton = Instantiate(purchaseButtonPrefab, newButtonPos, Quaternion.identity);
            newButton.transform.SetParent(transform);
            AddPurchaseTroopFunctionAndText(ref newButton, selectedCity.unitsForSale[i]);
        }
    }

    /// <summary>
    /// Adds on-click functionality to the button.
    /// Also adds text to the button.
    /// </summary>
    private void AddPurchaseTroopFunctionAndText(ref Button button, TroopClassifications classification)
    {
        City selectedCity = (City) GameManager.instance.placeSelection;

        switch (classification)
        {
            case TroopClassifications.INFANTRY:
                button.onClick.AddListener(() => selectedCity.AddUnit(UnitFactory.instance.GenerateInfantry()));
                button.GetComponentInChildren<Text>().text = "Infantry";
                break;
            case TroopClassifications.RANGED:
                button.onClick.AddListener(() => selectedCity.AddUnit(UnitFactory.instance.GenerateRanged()));
                button.GetComponentInChildren<Text>().text = "Ranged";
                break;
            case TroopClassifications.CAVALRY:
                button.onClick.AddListener(() => selectedCity.AddUnit(UnitFactory.instance.GenerateCavalry()));
                button.GetComponentInChildren<Text>().text = "Cavalry";
                break;
        }
    }

}
