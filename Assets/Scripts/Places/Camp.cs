using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Placed along roads.
/// Temporary holding place for units when they can't enter an occupied city.
/// </summary>
public class Camp
{
    public List<Unit> occupyingUnits { get; private set; }
    public Allegiance allegiance { get; private set; } 
    // The road on which the camp is placed
    public Road road  { get; private set; }
    public City city { get; private set; }

    public Camp(Road road, City city) {
        this.road = road;
        this.city = city;
        this.occupyingUnits = new List<Unit>();
        this.allegiance = Allegiance.NONE;

        EventManager.OnUnitDiedEvent += RemoveDeadUnit;
    }

    /// <summary>
    /// Puts the units in the camp.
    /// </summary>
    public void EncampUnits(List<Unit> units) 
    {
        this.occupyingUnits.AddRange(units);
    }

    /// <summary>
    /// Moves the units from the camp into the city.
    /// Clears the list of occupying units in the camp and road.
    /// </summary>
    public void MoveUnitsIntoCity()
    {
        city.AddOccupyingUnits(this.occupyingUnits);
        this.occupyingUnits.Clear();
    }

    public void RemoveDeadUnit(Unit u) {
        this.occupyingUnits.Remove(u);
    }
}
