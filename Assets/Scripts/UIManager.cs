using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour 
{
    public static UIManager instance {get; set;}
    
    public GameObject selectedCityLabel;

    public GameObject purchasePanel;
    public GameObject purchaseButtonAnchor;
    public Button purchaseTroopButtonPrefab;
    private List<Button> activePurchaseButtons;

    public GameObject friendlyUnitsPanel;
    public GameObject friendlyUnitPrefab;
    public GameObject friendlyUnitContent;
    private List<GameObject> friendlyUnitInstances;


    void Start()
    {
        instance = this;
        activePurchaseButtons = new List<Button>();
        friendlyUnitInstances = new List<GameObject>();
    }

    void Update() 
    {
        DisplaySelectedCityInformation();
    }

    /// <summary>
    /// Changes the UI to fit the needs of the selected city.
    /// If there isn't a selected city, displays nothing at the UI level.
    /// </summary>
    public void DisplaySelectedCityInformation() 
    {
        City selectedCity = GameManager.instance.selectedCity;
        
        if (selectedCity != null) {
            selectedCityLabel.GetComponent<Text>().text = selectedCity.cityName;
            selectedCityLabel.SetActive(true);
            purchasePanel.SetActive(true);
            friendlyUnitsPanel.SetActive(true);
            DisplayFriendlyTroops();
        } else {
            selectedCityLabel.SetActive(false);
            purchasePanel.SetActive(false);
            friendlyUnitsPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Populates the friendly units section with the units within 
    /// the selected city.
    /// </summary>
    public void DisplayFriendlyTroops()
    {
        List<Unit> friendlyUnits = GameManager.instance.selectedCity.friendlyUnits;

        RectTransform contentRectTransform = friendlyUnitContent.GetComponent<RectTransform>();


        foreach(GameObject instance in friendlyUnitInstances) {
            Destroy(instance);
        }

        friendlyUnitInstances.Clear();

        for(int i = 0;  i < friendlyUnits.Count; i++)
        {
            GameObject friendlyUnitCard = Instantiate(friendlyUnitPrefab, contentRectTransform, false);
            friendlyUnitCard.GetComponentInChildren<Text>().text = friendlyUnits[i].unitName;
            friendlyUnitCard.transform.SetParent(contentRectTransform);
            friendlyUnitInstances.Add(friendlyUnitCard);
        }


    }

    /// <summary>
    /// Creates the purchase buttons per selected city (based on which troop
    /// types that city can provide).
    /// Adds onclick functionality to the purchase buttons.
    /// </summary>
    public void SetupPurchaseButtons() 
    {
        City selectedCity = GameManager.instance.selectedCity;

        if (selectedCity != null) {
            Vector3 buttonAnchorPos = purchaseButtonAnchor.transform.position;
            
            for(int i = 0; i < selectedCity.unitsForSale.Length; i++ ) {
                Vector3 newButtonPos = buttonAnchorPos;
                newButtonPos.y -= i * 35;

                Button newButton = Instantiate(purchaseTroopButtonPrefab, newButtonPos, Quaternion.identity);
                newButton.transform.SetParent(transform);
                AddPurchaseTroopFunctionAndText(ref newButton, selectedCity.unitsForSale[i]);
                activePurchaseButtons.Add(newButton);
            }
        }
    }

    /// <summary>
    /// Adds on-click functionality to the button.
    /// Also adds text to the button.
    /// </summary>
    public void AddPurchaseTroopFunctionAndText(ref Button button, TroopClassifications classification) 
    {
        City selectedCity = GameManager.instance.selectedCity;

        switch(classification) {
            case TroopClassifications.INFANTRY:
                button.onClick.AddListener(() => selectedCity.AddUnit(UnitFactory.GenerateInfantry()));
                button.GetComponentInChildren<Text>().text = "Infantry";
                break;
            case TroopClassifications.RANGED:
                button.onClick.AddListener(() => selectedCity.AddUnit(UnitFactory.GenerateRanged()));
                button.GetComponentInChildren<Text>().text = "Ranged";
                break;
            case TroopClassifications.CAVALRY:
                button.onClick.AddListener(() => selectedCity.AddUnit(UnitFactory.GenerateCavalry()));
                button.GetComponentInChildren<Text>().text = "Cavalry";
                break;
        }
    }

    /// <summary>
    /// Removes all purchase button listeners and destroys the button
    /// game objects.
    /// </summary>
    public void DestroyPurchaseButtons() 
    {
        foreach(Button button in activePurchaseButtons) {
            button.onClick.RemoveAllListeners();
            Destroy(button.gameObject);
        }        

        activePurchaseButtons.Clear();
    }
}