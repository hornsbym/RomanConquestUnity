using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A "Place" is defined as somewhere that can contain units.
/// </summary>
public abstract class Place: MonoBehaviour
{
    public string placeName { get; set; }
    public List<Unit> occupyingUnits { get; protected set; }
    public Allegiance allegiance { get; set; }

    /// <summary>
    /// Tells whether or not a unit can be added to a place.
    /// </summary>
    public bool CanPlace(Unit unit)
    {
        if (this.allegiance == Allegiance.NONE) return true;
        else return unit.allegiance == this.allegiance;
    }

    /// <summary>
    /// Adds the provided unit to the place's list of units.
    /// </summary>
    public abstract void  AddOccupyingUnits<T>(List<T> units) where T : Unit; 

    /// <summary>
    /// Remove the list of units from the place's friendly units.
    /// </summary>
    public abstract void RemoveOccupyingUnits (List<Unit> units) ;
}
