using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class SelectedRoadUI : MonoBehaviour
{
    [SerializeField] private Text roadTitle;
    [SerializeField] private TravellingUnitsScrollview tUnitScrollview;
    [SerializeField] private UnitsScrollview city1CampedUnitsScrollview;
    [SerializeField] private UnitsScrollview city2CampedUnitsScrollview;


    void Awake() 
    {
        EventManager.OnSelectedRoadUpdatedEvent += PopulateComponents;
    }

    private void PopulateComponents(Road selectedRoad){
        /// Initialize city title
        roadTitle.text = selectedRoad.placeName;
        InitializeTravellingUnitScrollview(selectedRoad.travellingUnits);
        InitializeCampedUnitsScrollviews(selectedRoad);
    }

    private void InitializeTravellingUnitScrollview (List<TravellingUnit> tUnits) 
    {
        tUnitScrollview.AddTitle("Units in transit");
        tUnitScrollview.SetContent(tUnits);
    }
    
    private void InitializeCampedUnitsScrollviews(Road selectedRoad)
    {
        city1CampedUnitsScrollview.AddTitle("Camped outside " + selectedRoad.city1.placeName);
        city1CampedUnitsScrollview.SetContent(selectedRoad.campedOutsideCity1);

        city2CampedUnitsScrollview.AddTitle("Camped outside " + selectedRoad.city2.placeName);
        city2CampedUnitsScrollview.SetContent(selectedRoad.campedOutsideCity2);
    }
}
