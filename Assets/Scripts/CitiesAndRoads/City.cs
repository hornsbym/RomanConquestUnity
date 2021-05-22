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
    public List<Unit> occupyingUnits { get; private set; }

    // Governor in charge of controlling independent cities
    public Governor governor { get; set; }

    // Tracks which units the city can sell
    public List<TroopClassification> unitsForSale;

    // Keeps track of who the city is allied with
    public Allegiance allegiance { get; set; }

    // This is the maximum amount of money a city will produce per turn
    public int wealth { get; set; }

    // This is the percentage of wealth the leader will take from this city per turn.
    // Taking a larger portion increases the chance that a city will enter a state of revolt.
    // This should always be between 0 and 1
    private float _taxRate;
    public float taxRate { 
        get => _taxRate; 
        set {
            _taxRate = Mathf.Clamp(value, 0f, 1f);
        }
    }

    // This is the chance per turn that a city will revolt per turn.
    // This should always be between 0 and 1.
    private float _publicUnrest;
    public float publicUnrest { 
        get => _publicUnrest; 
        set {
            _publicUnrest = Mathf.Clamp(value, 0f, 1f);
        }
    }

    // Any time a city does something, set this to false.
    // It will be reset to true at the beginning of each turn.
    public bool hasAction { get; private set; }

    // Buildings built in the city. 
    // Each building has a unique effect on the city.
    // Only a limited number of buildings can be build in any one city at a time.
    public int buildingLimit { get; set; }
    public List<Building> buildings { get; set; }
    public List<Building> buildingsForPurchase { get; set; }

    /// Called whenever the component is added to an object.
    void Awake() {
        neighbors = new List<Neighbor>();
        occupyingUnits = new List<Unit>();
        buildings = new List<Building>();
        unitsForSale = new List<TroopClassification>();
        buildingsForPurchase = new List<Building>() { new Barracks(), new Range(), new Stables() };

        buildingLimit = 3;

        publicUnrest = 0f;
        taxRate = 0f;
        hasAction = true; 
    }

    public override bool CanPlace(Unit unit)
    {
        return unit.allegiance == this.allegiance;
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
        int tax = (int)(wealth * taxRate);

        /// Independent cities give their taxes directly to the governor
        /// And don't calculate public unrest or revolt
        if (this.allegiance != Allegiance.INDEPENDENT) {
            /// Check if the city should revolt
            float chance = UnityEngine.Random.Range(0f, 1f);
            if (publicUnrest > chance) {
                Revolt();
            } else {
                GameManager.instance.allegianceToLeaderMapping[allegiance].gold += tax;
            }

            CalculatePublicUnrest();
        } else {
            governor.gold += tax;
        }
    }

    /// <summary>
    /// Performs calculation logic to figure the amount of public unrest in a city.
    /// </summary>
    private void CalculatePublicUnrest() 
    {
        if (taxRate >= 0f && taxRate <= .1f) {
            publicUnrest = publicUnrest + CityManagementConstants.TIER_1_UNREST_DIFFERENCE;
        } else if (taxRate > .1f && taxRate <= .25f) {
            publicUnrest = publicUnrest + CityManagementConstants.TIER_2_UNREST_DIFFERENCE;
        } else if (taxRate > .25f && taxRate <= .4f) {
            publicUnrest = publicUnrest + CityManagementConstants.TIER_3_UNREST_DIFFERENCE;
        } else if (taxRate > .4f) {
            publicUnrest = publicUnrest + CityManagementConstants.TIER_4_UNREST_DIFFERENCE;
        }
    }

    /// <summary>
    /// Consumes the city's per-turn action.
    /// </summary>
    public void UseAction()
    {
        this.hasAction = false;
    }

    /// <summary>
    /// Resets the city's per-turn action.
    /// </summary>
    public void ResetAction()
    {
        this.hasAction = true;
    }

    /// <summary>
    /// Performs revolt logic, primarily setting the allegiance to independent.
    /// Also removes any friendly units in the city and 
    /// will eventually spawn independently aligned units.
    /// </summary>
    private void Revolt() 
    {
        ClearOccupyingUnits();
        AddOccupyingUnits(UnitFactory.instance.GenerateTroop(TroopClassification.INFANTRY, Allegiance.INDEPENDENT));
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

        if (units.Count > 0) {
            if (roadToNeighbor.CanPlace(units[0])) {
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

        }        
    }

    /// <summary>
    /// Adds a neighbor to the list of neighboring cities.
    /// Automatically adds this city to the neighboring city's neighbors.
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
        EventManager.instance.fireSelectedCityUpdatedEvent(this);
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
            ChangeAllegiance(occupyingUnits[0].allegiance);
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

    /// <summary>
    /// Returns whether or not a building can be built.
    /// </summary>
    public bool CanBuild<T>(T building) where T : Building
    {   
        /// Can't build past the city's building limit
        if (this.buildings.Count >= this.buildingLimit) {
            return false;
        } 
        
        /// Can't build two of the same building
        else if (this.buildings.Contains(building)) {
            return false;
        } else {
            return true;
        }
    }

    /// <summary>
    /// Adds the building to the list of buildings, if able.
    /// Applies the building's affect and consumes the city's action.
    /// </summary>
    public void AddBuilding<T>(T building) where T : Building
    {
        if (CanBuild(building)) {
            this.buildings.Add(building);
            building.ApplyEffect(this);
            this.UseAction();
        }
    }

    /// <summary>
    /// Removes the building from the list of buildings, if able.
    /// Removes the building's affect and consumes the city's action.
    /// </summary>
    public void RemoveBuilding<T>(T building) where T : Building
    {
        if (this.buildings.Contains(building))
        {
            this.buildings.Add(building);
            building.RemoveEffect(this); 
            this.UseAction();
        }
    }
 }
