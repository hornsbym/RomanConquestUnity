using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : Place
{
    // The cities connected by the road.
    public City city1 { get; set; }
    public City city2 { get; set; }

    // Holding zone for troops that have arrived at an occupied city.
    public Camp city1Camp { get; private set; }
    public Camp city2Camp { get; private set; }

    // The number of turns it takes to traverse the road
    public int defaultTurnCount { get; set; }

    // Instructions for positioning the road marker on the map.
    // public Vector3 position;
    public Quaternion rotation { get; set; }

    // Tracks which units are traversing the road and which city 
    // they're destined for.
    // Should always be synchronized with the "occupyingUnits" field
    // inherited from Parent class.
    public List<TravellingUnit> travellingUnits { get; private set; }

    public override List<Unit> occupyingUnits 
    { 
        /// <summary>
        /// Derives the units on the road from the travelling units
        /// and both city camps.
        /// </summary>
        get {
            List<Unit> u = new List<Unit>();
            foreach (TravellingUnit tu in travellingUnits) {
                u.Add(tu.unit);
            }
            u.AddRange(city1Camp.occupyingUnits);
            u.AddRange(city2Camp.occupyingUnits);
            return u;
        }

        /// <summary>
        /// Setting occupying units on a road doesn't make much sense 
        /// because we're deriving the value from both camps and the 
        /// travelling units list.
        /// </summary>
        protected set => base.occupyingUnits = value; 
    }

    void Awake() 
    {
        EventManager.OnTurnEnd += ProgressAllUnits;
        EventManager.OnUnitDiedEvent += RemoveDeadUnit;
        this.travellingUnits = new List<TravellingUnit>();
        this.occupyingUnits = new List<Unit>();
        this.allegiance = Allegiance.NONE;
    }

    /// Units will remain as an "OccupyingUnit" of the roas as long as they are
    /// travelling along the road or encamped at a city.
    public override void AddOccupyingUnits<T>(List<T> units)
    {
        // /// Add the units to the occupying units list
        // occupyingUnits.AddRange(units);

        // /// Handles changing allegiances if friendly units are the only units in the city.
        // if (this.occupyingUnits.Count > 0)
        // {
        //     this.allegiance = occupyingUnits[0].allegiance;
        // }    
    }

    override public void RemoveOccupyingUnits(List<Unit> units)
    {
        // TODO: Delete dead units from the camps too (verify this works)
        // TODO: Can this be simplified to use "remove all" insead of a for-each loop?

        // TODO: Is it necessary to destroy the game objects too?
        // List<Unit> revisedUnitsList = new List<Unit>(this.occupyingUnits);
        // foreach (Unit unit in this.occupyingUnits)
        // {
        //     if (units.Contains(unit))
        //     {
        //         revisedUnitsList.Remove(unit);
        //     }
        // }

        // this.occupyingUnits = revisedUnitsList;

        // If there are no units encamped along the road, change the road
        // allegiance to NONE
        if (occupyingUnits.Count == 0) {
            this.allegiance = Allegiance.NONE;
        }
    }

    public override void RemoveDeadUnit (Unit deadUnit)
    {
        RemoveOccupyingUnits(new List<Unit>() { deadUnit });
    }

    public void InitializeRoad() {
        // Create name
        placeName = $"{city1.placeName} - {city2.placeName}";

        // Create new camps outside of cities
        this.city1Camp = new Camp(this, city1);
        this.city2Camp = new Camp(this, city2);

        // Finds the midpoint between the two cities.
        Vector3 city1Position = city1.gameObject.transform.position;
        Vector3 city2Position = city2.gameObject.transform.position;

        // Finds the rotation necessary to point the road between the two cities
        float length = Vector3.Distance(city1Position, city2Position);

        // Position the road between the two cities
        gameObject.transform.position = new Vector3(
            city2Position.x + ((city1Position.x - city2Position.x) / 2),
            city2Position.y + ((city1Position.y - city2Position.y) / 2),
            0
        );

        // Scale the road 
        // TODO:  WHY DO WE NEED TO DIVIDE LENGTH BY 2??
        gameObject.transform.localScale = new Vector3(
            gameObject.transform.localScale.x,
            length / 2,
            gameObject.transform.localScale.z
        );

        // Rotate the road
        gameObject.transform.rotation = Quaternion.LookRotation(city2Position - city1Position) * Quaternion.Euler(90, 0, 0); ;
    }

    /// <summary>
    /// Initiates moving a unit from one city to another.
    /// </summary>
    public void PutUnitsOnRoad(List<Unit> units, City currentCity) 
    {
        AddOccupyingUnits(units);

        // Convert the list of units into corresponding travelling units
        foreach (Unit unit in units) {
            /// Take whichever city doesn't match the current city
            City destination = (city1 == currentCity) ? city2 : city1;
            travellingUnits.Add(new TravellingUnit(unit, destination, defaultTurnCount));
        }
    }

    /// <summary>
    /// Moves all units closer to their destination.
    /// If any of them arrive at their destination, move them into that city.
    /// </summary>
    public void ProgressAllUnits()
    {   
        // We will add the units still travelling to this list as we iterate.
        List<TravellingUnit> travellingUnitsBackupList = new List<TravellingUnit>();

        // Loop over all units going towards the first city.
        foreach(TravellingUnit tUnit in travellingUnits){
            Unit arrivedUnit = tUnit.Progress();
            if (arrivedUnit != null && tUnit.destination.CanPlace(arrivedUnit)) {
                tUnit.destination.AddOccupyingUnits(new List<Unit>(){ arrivedUnit });
            } else {
                // TODO: Fix this so that we track which units need to be put in each camp
                // TODO: and then put them all in at once.
                EncampUnit(tUnit.unit, tUnit.destination);
            }
        }

        // Replace the units en route list with the units that are still 
        // en route.
        travellingUnits = travellingUnitsBackupList;
        
        // If the city has no units in it, move the units into the city
        if (city1.occupyingUnits.Count == 0 && city1Camp.occupyingUnits.Count > 0) {
            city1Camp.MoveUnitsIntoCity();
        }

        if (city2.occupyingUnits.Count == 0 && city2Camp.occupyingUnits.Count > 0)
        {
            city2Camp.MoveUnitsIntoCity();
        }
    }

    /// <summary>
    /// Get the camp on the road outside of a particular city.
    /// </summary>
    public Camp GetCityCamp(City city) 
    {
        return (city == city1) ? this.city1Camp : this.city2Camp ;
    }

    /// <summary>
    /// Puts the unit in the camp outside of the city.
    /// </summary>
    private void EncampUnit(Unit unit, City city) {
        if (city == city1) {
            this.city1Camp.EncampUnits(new List<Unit>() { unit });
        } else {
            this.city2Camp.EncampUnits(new List<Unit>() { unit });
        }
    }
}
