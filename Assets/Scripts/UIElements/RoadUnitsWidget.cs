using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadUnitsWidget : MonoBehaviour
{
    [SerializeField] private UnitsScrollview occupiedUnitsScrollview;

    void Awake() {
        EventManager.OnSelectedRoadUpdatedEvent += populateScrollviews;
    }

    private void populateScrollviews(Road selectedRoad) 
    {
        occupiedUnitsScrollview.AddTitle("Units");
    }
}
