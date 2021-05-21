using UnityEngine.UI;
using UnityEngine;

public class SelectedRoadUI : MonoBehaviour
{
    public Text roadTitle;
    public GameObject friendlyUnitsList;
    public GameObject friendlyUnitDetails;

    void Awake() 
    {
        EventManager.OnSelectedRoadUpdatedEvent += PopulateComponents;
    }

    private void PopulateComponents(Road selectedRoad){
        /// Initialize city title
        roadTitle.text = selectedRoad.placeName;

        /// Initialize UI components
        // InitializeFriendlyUnitList(selectedRoad);
    }

    // private void InitializeFriendlyUnitList(Road selectedRoad)
    // {
    //     /// Instantiate a new UnitScrollview widget 
    //     UnitsScrollview scrollview = friendlyUnitsList.GetComponent<UnitsScrollview>();

    //     /// Set the unit scrollview title.
    //     scrollview.AddTitle("Friendly Units");

    //     /// Add the units and provide logic for what should be done whenever the 
    //     /// individual unit is clicked.
    //     scrollview.SetContent(selectedRoad, (Unit u) =>
    //     {
    //         GameManager.instance.SelectUnit(u);
    //     });
    // }
}
