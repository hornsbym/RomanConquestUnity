using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance {get; set;}

    // Delegate definitions
    public delegate void voidEmptyDelegate();
    public delegate void voidCityDelegate(City city);
    public delegate void voidRoadDelegate(Road road);

    // Event declarations
    public static event voidEmptyDelegate OnTurnEnd;
    public static event voidEmptyDelegate OnTurnBegin;
    public static event voidEmptyDelegate OnDefaultSelected;

    public static event voidRoadDelegate OnRoadSelected;

    public static event voidCityDelegate OnCitySelected;
    public static event voidCityDelegate OnUnitsChanged;
    public static event voidCityDelegate OnCombineSelected;
    public static event voidCityDelegate OnMoveUnitsSelected;

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

    public void fireUnitsChangedEvent(City city)
    {
        print("Unit added event fired.");
        OnUnitsChanged?.Invoke(city);
    }

    public void fireTurnEndEvent()
    {
        print("Turn ended.");
        OnTurnEnd?.Invoke();
    }

    public void fireTurnBeginEvent()
    {
        print("Turn began.");
        OnTurnBegin?.Invoke();
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
