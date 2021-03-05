﻿using System.Collections.Generic;

/// <summary>
/// Contains information about a city and its contents.
/// Also contains a reference to its neighbors and the
/// GameObject that represents it.
/// </summary>
public class City : Place
{
    // Tracks adjacent cities
    private List<Neighbor> neighbors {get; set;}

    // Tracks units within the city
    private List<Unit> hostileUnits;
    public List<Unit> friendlyUnits;

    // Tracks which units the city can sell
    public TroopClassifications[] unitsForSale;

    /// Called whenever the component is added to an object.
    void Awake() {
        neighbors = new List<Neighbor>();
        hostileUnits = new List<Unit>();
        friendlyUnits = new List<Unit>();

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
    /// If no road exists between the city and its neighbors, 
    /// creates the road.
    /// </summary>
    public void ConnectWithNeighbors() 
    {
        // Validates the road doesn't exist first
        foreach(Neighbor neighbor in neighbors) {
            // If we get here, the road doesn't exist so we should make one and 
            // add it to the MapManager's roads list.
            MapManager.instance.TryToCreateRoad(this, neighbor.city, neighbor.travelLength);
        }
    }

    /// <summary>
    /// Adds a neighbor to the list of neighboring cities.
    /// </summary>
    public void AddNeighbor(City neighboringCity, int distance) 
    {
        neighbors.Add(new Neighbor(neighboringCity, distance));
    }


    /// <summary>
    /// Adds the provided unit to the city's list of units.
    /// </summary>
    public void AddUnit(Unit unit) 
    {   
        friendlyUnits.Add(unit);
        UIManager.instance.DestroyFriendlyUnitsPanel();
        UIManager.instance.CreateFriendlyUnitsSection(this);
    }
}
