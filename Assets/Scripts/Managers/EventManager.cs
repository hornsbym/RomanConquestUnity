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
    public delegate void voidUnitDelegate(Unit unit);
    public delegate void voidCombinedUnitDelegate<T>(T combinedUnit) where T : Unit;

    // Event declarations
    public static event voidEmptyDelegate OnTurnEnd;
    public static event voidEmptyDelegate OnTurnBegin;
    public static event voidEmptyDelegate OnDefaultSelected;

    public static event voidRoadDelegate OnSelectedRoadUpdatedEvent;

    public static event voidCityDelegate OnSelectedCityUpdated;
    public static event voidCityDelegate OnUnitsChanged;
    public static event voidCityDelegate OnCombineSelected;
    public static event voidCityDelegate OnMoveUnitsSelected;
    public static event voidCityDelegate OnAttackSelected;
    public static event voidCityDelegate OnCityActionConsumed;

    public static event voidUnitDelegate OnUnitDiedEvent;

    public static event voidCombinedUnitDelegate<Century> OnCenturyDisbanded;
    public static event voidCombinedUnitDelegate<Cohort> OnCohortDisbanded;
    public static event voidCombinedUnitDelegate<Legion> OnLegionDisbanded;


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

    public void fireUnitDiedEvent(Unit unit)
    {
        Utilities.instance.Debug(unit.unitName + " died");
        OnUnitDiedEvent?.Invoke(unit);
    }

    public void fireCenturyDisbandedEvent(Century century)
    {
        Utilities.instance.Debug(century.unitName + " disbanded");
        OnCenturyDisbanded?.Invoke(century);
    }

    public void fireCohortDisbandedEvent(Cohort cohort)
    {
        Utilities.instance.Debug(cohort.unitName + " disbanded");
        OnCohortDisbanded?.Invoke(cohort);
    }

    public void fireLegionDisbandedEvent(Legion legion)
    {
        Utilities.instance.Debug(legion.unitName + " disbanded");
        OnLegionDisbanded?.Invoke(legion);
    }

    public void fireAttackScreenSelectedEvent(City city) 
    {
        Utilities.instance.Debug("Attack action selected");
        OnAttackSelected?.Invoke(city);
    }
}
