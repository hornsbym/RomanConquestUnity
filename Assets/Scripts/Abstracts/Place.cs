using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Place: MonoBehaviour
{
    public string placeName {get; set;}

    /// <summary>
    /// Tells whether or not a unit can be added to a place.
    /// </summary>
    public abstract bool CanPlace(Unit unit);
}
