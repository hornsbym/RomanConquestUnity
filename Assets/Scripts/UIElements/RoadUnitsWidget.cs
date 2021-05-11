using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadUnitsWidget : MonoBehaviour
{
    public UnitsScrollview city1BoundUnitsScrollview;
    public UnitsScrollview city2BoundUnitsScrollview;

    void Awake() {
        EventManager.OnRoadSelected += populateScrollviews;
    }

    private void populateScrollviews(Road selectedRoad) 
    {
        city1BoundUnitsScrollview.AddTitle("To " + selectedRoad.city1.placeName);
        city1BoundUnitsScrollview.SetContent(selectedRoad.GetUnitsEnRouteTo(selectedRoad.city1));

        city2BoundUnitsScrollview.AddTitle("To " + selectedRoad.city2.placeName);
        city2BoundUnitsScrollview.SetContent(selectedRoad.GetUnitsEnRouteTo(selectedRoad.city2));
    }
}
