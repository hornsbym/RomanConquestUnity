using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance {get; set;}

    public delegate void SelectCity(City city);
    public static event SelectCity OnCitySelected;

    public delegate void SelectRoad(Road road);
    public static event SelectRoad OnRoadSelected;

    public delegate void UnitAdded(City city);
    public static event UnitAdded OnUnitAdded;

    public delegate void TurnEnd();
    public static event TurnEnd OnTurnEnd;

    public delegate void SelectDefault();
    public static event SelectDefault OnDefaultSelected;

    public delegate void SelectCombine(City city);
    public static event SelectCombine OnCombineSelected;

    public delegate void SelectMoveUnits(City city);
    public static event SelectMoveUnits OnMoveUnitsSelected;

    void Start()
    {
        instance = this;
    }

    public void fireSelectCityEvent(City city) {
        print("City selection event fired.");
        OnCitySelected?.Invoke(city);
    }

    public void fireSelectRoadEvent(Road road) {
        print("Road selection event fired.");
        OnRoadSelected?.Invoke(road);
    }

    public void fireUnitAddedEvent(City city)
    {
        print("Unit added event fired.");
        OnUnitAdded?.Invoke(city);
    }

    public void fireTurnEndEvent()
    {
        print("Turn ended.");
        OnTurnEnd?.Invoke();
    }

    public void fireDefaultSelectedEvent() {
        print("Default selected.");
        OnDefaultSelected?.Invoke();
    }

    public void fireCombineSelectedEvent(City city)
    {
        print("Combine troops selected.");
        OnCombineSelected?.Invoke(city);
    }

    public void fireMoveSelectedEvent(City city)
    {
        print("Move troops selected.");
        OnMoveUnitsSelected?.Invoke(city);
    }
}
