using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains information about a city and its contents.
/// Also contains a reference to its neighbors and the
/// GameObject that represents it.
/// </summary>
public class City
{
    public string cityName;

    // The coordinates where the city marker will be drawn
    public Vector3 cityPosition;

    // Maintains a record to the city marker
    private GameObject cityMarker {get; set;}

    // Tracks adjacent cities
    private List<City> neighbors;

    // Tracks units within the city
    private List<Unit> hostileUnits;
    public List<Unit> friendlyUnits;

    // Tracks which units the city can sell
    public TroopClassifications[] unitsForSale;

    /// <summary>
    /// Accepts an optional parameter for which troops the city will be able to sell.
    /// </summary>
    public City(string name, Vector3 position, TroopClassifications[] troopsForSale = null){
        cityName = name;
        cityPosition = position;
        cityMarker = MapManager.instance.SpawnCityMarker(this);

        hostileUnits = new List<Unit>();
        friendlyUnits = new List<Unit>();

        // If "troopsForSale" isn't null, then "unitsForSale" is set to its value.
        // If "troopsForSale" is null, then "unitsForSale" is set to an empty array.
        unitsForSale = troopsForSale ?? new TroopClassifications[0];
    }

    /// <summary>
    /// Adds the provided unit to the city's list of units.
    /// </summary>
    public void AddUnit(Troop troop) {
        friendlyUnits.Add(troop);
    }
}
