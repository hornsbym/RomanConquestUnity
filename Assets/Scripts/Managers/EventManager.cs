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

    public static event voidRoadDelegate OnSelectedRoadUpdatedEvent;

    public static event voidCityDelegate OnSelectedCityUpdated;
    public static event voidCityDelegate OnUnitsChanged;
    public static event voidCityDelegate OnCombineSelected;
    public static event voidCityDelegate OnMoveUnitsSelected;
    public static event voidCityDelegate OnCityActionConsumed;

    void Start()
    {
        instance = this;
    }

    public void fireSelectedCityUpdatedEvent(City city) {
        Utilities.instance.Debug("Selected city updated.");
        OnSelectedCityUpdated?.Invoke(city);
    }

    public void fireSelectedRoadUpdatedEvent(Road road) {
        Utilities.instance.Debug("Selected road updated.");
        OnSelectedRoadUpdatedEvent?.Invoke(road);
    }

    public void fireTurnEndEvent()
    {
        Utilities.instance.Debug("Turn ended.");
        OnTurnEnd?.Invoke();
    }

    public void fireTurnBeginEvent()
    {
        Utilities.instance.Debug("Turn began.");
        OnTurnBegin?.Invoke();
    }

    public void fireDefaultSelectedEvent() {
        Utilities.instance.Debug("Default selected.");
        OnDefaultSelected?.Invoke();
    }

    public void fireCombineSelectedEvent(City city)
    {
        Utilities.instance.Debug("Combine troops selected.");
        OnCombineSelected?.Invoke(city);
    }

    public void fireMoveSelectedEvent(City city)
    {
        Utilities.instance.Debug("Move troops selected.");
        OnMoveUnitsSelected?.Invoke(city);
    }

    public void fireUnitsChangedEvent(City city)
    {
        Utilities.instance.Debug("Units changed in " + city.placeName);
        OnUnitsChanged?.Invoke(city);
    }

    public void fireCityActionConsumedEvent(City city)
    {
        Utilities.instance.Debug(city.placeName + " action used");
        OnCityActionConsumed?.Invoke(city);
    }
}
