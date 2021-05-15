using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class SelectedCityUI : MonoBehaviour
{
    public Text cityTitle;
    public GameObject friendlyUnitsList;
    public GameObject friendlyUnitDetails;
    public GameObject purchasePanel;
    public GameObject cityOptions;

    void Awake() 
    {
        friendlyUnitDetails.SetActive(false);

        // Subscribe methods to events
        EventManager.OnUnitsChanged += InitializeFriendlyUnitList;
        EventManager.OnSelectedCityUpdated += PopulateComponents;
    }

    void Update() {
        DisplaySelectedTroopDetails();
    }

    private void DisplaySelectedTroopDetails()
    {
        if (GameManager.instance.unitSelection != null) {
            friendlyUnitDetails.GetComponent<UnitDetailsWidget>().FillInLabels(GameManager.instance.unitSelection);
            friendlyUnitDetails.SetActive(true);
        } else {
            friendlyUnitDetails.SetActive(false);
        }
    }

    private void InitializeFriendlyUnitList(City city)
    {
        /// Instantiate a new UnitScrollview widget 
        UnitsScrollview scrollview = friendlyUnitsList.GetComponent<UnitsScrollview>();

        /// Set the unit scrollview title.
        scrollview.AddTitle("Occupying Units");

        /// Add the units and provide logic for what should be done whenever the 
        /// individual unit is clicked.
        scrollview.SetContent(city.occupyingUnits, (Unit u) =>
        {
            GameManager.instance.SelectUnit(u);
        });
    }

    private void PopulateComponents(City city){
        /// Initialize city title
        cityTitle.text = city.placeName;

        /// Initialize UI components
        InitializeFriendlyUnitList(city);
    }
}
