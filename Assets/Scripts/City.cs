using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Contains information about a city and its contents.
/// Also contains a reference to its neighbors and the
/// GameObject that represents it.
/// </summary>
public class City : Place
{
    // Tracks adjacent cities
    private List<Neighbor> neighbors { get; set; }

    // Tracks units within the city
    public List<Unit> occupyingUnits { get; set; }

    // Tracks which units the city can sell
    public TroopClassifications[] unitsForSale;

    // Keeps track of who the city is allied with
    public Allegiance allegiance { get; set; }

    // This is the maximum amount of money a city will produce per turn
    public int wealth { get; set; }

    // This is the percentage of wealth the leader will take from this city per turn.
    // Taking a larger portion increases the chance that a city will enter a state of revolt.
    // This should always be between 0 and 1
    public float taxRate { get; set; }

    // This is the chance per turn that a city will revolt per turn.
    // This should always be between 0 and 1.
    public float publicUnrest { get; set; }

    /// Called whenever the component is added to an object.
    void Awake() {
        neighbors = new List<Neighbor>();
        occupyingUnits = new List<Unit>();

        publicUnrest = 0.0f;
        taxRate = .249f;
    }

    /// <summary>
    /// Tracks adjacent cities.
    /// Pairs a City with an integer representing how many days 
    /// of travel it will take to travel the connecting road.
    /// </summary>
    private class Neighbor
    {
        public City city;
        public int travelLength;

        public Neighbor(City city, int travelLength) {
            this.city = city;
            this.travelLength = travelLength;
        }
    }

    /// <summary>
    /// Adds the taxes owed by the city to the leader's gold reserve.
    /// Before that, considers if the city should revolt.
    /// After the taxes are collected, adjusts the public unrest.
    /// </summary>
    public void CollectTaxes() 
    {
        /// Check if the city should revolt
        float chance = UnityEngine.Random.Range(0f, 1f);
        if (publicUnrest > chance) {
            Revolt();
        } else {
            int tax = (int) (wealth * taxRate);
            GameManager.instance.allegianceToLeaderMapping[allegiance].gold += tax;
        }

        CalculatePublicUnrest();
    }

    /// <summary>
    /// Performs calculation logic to figure the amount of public unrest in a city.
    /// </summary>
    private void CalculatePublicUnrest() 
    {
        if (taxRate >= 0f && taxRate <= .1f) {
            publicUnrest = Mathf.Clamp((publicUnrest + CityManagementConstants.TIER_1_UNREST_DIFFERENCE), 0f, 1f);
        } else if (taxRate > .1f && taxRate <= .25f) {
            publicUnrest = Mathf.Clamp((publicUnrest + CityManagementConstants.TIER_2_UNREST_DIFFERENCE), 0f, 1f);
        } else if (taxRate > .25f && taxRate <= .4f) {
            publicUnrest = Mathf.Clamp((publicUnrest + CityManagementConstants.TIER_3_UNREST_DIFFERENCE), 0f, 1f);
        } else if (taxRate > .4f) {
            publicUnrest = Mathf.Clamp((publicUnrest + CityManagementConstants.TIER_4_UNREST_DIFFERENCE), 0f, 1f);
        }
    }

    /// <summary>
    /// Performs revolt logic, primarily setting the allegiance to independent.
    /// Also removes any friendly units in the city and 
    /// will eventually spawn independently aligned units.
    /// </summary>
    private void Revolt() 
    {
        ChangeAllegiance(Allegiance.INDEPENDENT);
        ClearOccupyingUnits();
    }

    /// <summary>
    /// If no road exists between the city and its neighbors, 
    /// creates the road.
    /// </summary>
    public void ConnectWithNeighbors() 
    {
        // Validates the road doesn't exist first
        foreach(Neighbor neighbor in neighbors) {
            // If we get here, the road doesn't exist so we should make one and 
            // add it to the MapManager's roads list.
            Road newRoad = MapManager.instance.TryToCreateRoad(this, neighbor.city, neighbor.travelLength);
        }
    }

    /// <summary>
    /// Sends a group of units on the road to a neighboring city.
    /// </summary>
    public void SendFriendlyUnitsToNeighbor(List<Unit> units, City city) {
        Road roadToNeighbor = MapManager.instance.GetRoad(this, city);

        foreach (Unit unit in units) {
            roadToNeighbor.PutUnitOnRoad(unit, this);
        }

        /// Removes units from the city if they're being put on the road
        List<Unit> revisedUnitsList = new List<Unit>(this.occupyingUnits);
        foreach (Unit unit in this.occupyingUnits) {
            if (units.Contains(unit)) {
                revisedUnitsList.Remove(unit);
            }
        }
        
        this.occupyingUnits = revisedUnitsList;
    }

    /// <summary>
    /// Adds a neighbor to the list of neighboring cities.
    /// </summary>
    public void AddNeighbor(City neighboringCity, int distance) 
    {
        neighbors.Add(new Neighbor(neighboringCity, distance));
    }

    /// <summary>
    /// Returns a list of the cities that are connected to this city.
    /// </summary>
    public List<City> GetNeighbors()
    {
        List<City> neighboringCities = new List<City>();
        foreach (Neighbor neighbor in this.neighbors) {
            neighboringCities.Add(neighbor.city);
        }

        return neighboringCities;
    }

    /// <summary>
    /// Changes the city's allegiance to the provided allegiance.
    /// </summary>
    public void ChangeAllegiance(Allegiance newAllegiance) 
    {
        this.allegiance = newAllegiance;
    }

    /// <summary>
    /// Adds the provided unit to the city's list of units.
    /// </summary>
    public void AddOccupyingUnits<T>(List<T> units) where T : Unit
    {   
        occupyingUnits.AddRange(units);
        EventManager.instance.fireUnitsChangedEvent(this);

        /// Handles changing allegiances if friendly units are the only units in the city.
        if (this.occupyingUnits.Count > 0) {
            this.allegiance = occupyingUnits[0].allegiance;
        }
    }

    /// <summary>
    /// Remove the list of units from the city's friendly units.
    /// </summary>
    public void RemoveOccupyingUnits<T>(List<T> units) where T : Unit
    {        
        /// TODO: Is it necessary to destroy the game objects too?
        List<Unit> revisedUnitsList = new List<Unit>(this.occupyingUnits);
        foreach (Unit unit in this.occupyingUnits)
        {
            if (units.Contains(unit))
            {
                revisedUnitsList.Remove(unit);
            }
        }

        this.occupyingUnits = revisedUnitsList;

        /// Send an event informing the rest of the game that the units have changed.
        EventManager.instance.fireUnitsChangedEvent(this);
    }

    /// <summary>
    /// Clears the occupied units list.
    /// </summary>
    public void ClearOccupyingUnits()
    {
        /// TODO: Is it necessary to destroy the game objects too?
        occupyingUnits.Clear();
        EventManager.instance.fireUnitsChangedEvent(this);
    }
}
