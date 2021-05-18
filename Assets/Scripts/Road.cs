using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : Place
{
    // The cities connected by the road.
    public City city1;
    public City city2;

    // The number of turns it takes to traverse the road
    public int defaultTurnCount;

    // Instructions for positioning the road marker on the map.
    // public Vector3 position;
    public Quaternion rotation;

    // Tracks which units are traversing the road and which city 
    // they're destined for.
    private Dictionary<City, List<TravellingUnit>> unitsEnRouteTo;

    void Awake() 
    {
        EventManager.OnTurnEnd += ProgressAllUnits;
    }

    /// <summary>
    /// A class for tracking a Unit's travelling progress.
    /// Basically pairs a Unit with an integer.
    /// </summary>
    private class TravellingUnit 
    {
        public int turnsToArrival;
        public Unit unit;

        public TravellingUnit(Unit unit, int turnCount) {
            this.unit = unit;
            this.turnsToArrival = turnCount;
        }

        /// <summary>
        /// Decreases the number of turns by 1.
        /// If the remaining turns to arrival gets to 0, returns the unit.
        /// If there are remaining turns, return null.
        /// </summary>
        public Unit progress(){
            turnsToArrival--;

            if (turnsToArrival == 0){
                return unit;
            }

            return null;
        }
    }


    public void InitializeRoad() {
        // Create name
        placeName = $"{city1.placeName} - {city2.placeName}";

        // Initializes unit tracking dictionary
        unitsEnRouteTo = new Dictionary<City, List<TravellingUnit>>();
        unitsEnRouteTo.Add(city1, new List<TravellingUnit>());
        unitsEnRouteTo.Add(city2, new List<TravellingUnit>());

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
    public void PutUnitOnRoad(Unit unit, City currentCity) 
    {
        // If the unit is currently in city1, send the unit to city2
        if (currentCity == city1) {
            unitsEnRouteTo[city2].Add(new TravellingUnit(unit, defaultTurnCount));
        } else if (currentCity == city2) {
            unitsEnRouteTo[city1].Add(new TravellingUnit(unit, defaultTurnCount));
        }
    }

    /// <summary>
    /// Moves all units closer to their destination.
    /// If any of them arrive at their destination, move them into that city.
    /// </summary>
    public void ProgressAllUnits()
    {   
        // We will add the units still travelling to this list as we iterate.
        List<TravellingUnit> toCity1BackupUnitList = new List<TravellingUnit>();

        // Loop over all units going towards the first city.
        foreach(TravellingUnit tUnit in unitsEnRouteTo[city1]){
            Unit arrivedUnit = tUnit.progress();
            if (arrivedUnit != null) {
                city1.AddOccupyingUnits(new List<Unit>(){ arrivedUnit });
            } else {
                toCity1BackupUnitList.Add(tUnit);
            }
        }

        // Replace the units en route list with the units that are still 
        // en route.
        unitsEnRouteTo[city1] = toCity1BackupUnitList;

        // We will add the units still travelling towards
        // the second city to this list as we iterate.
        List<TravellingUnit> toCity2BackupUnitList = new List<TravellingUnit>();

        // Loop over all units going towards the second city.
        foreach (TravellingUnit tUnit in unitsEnRouteTo[city2])
        {
            Unit arrivedUnit = tUnit.progress();
            if (arrivedUnit != null)
            {
                city2.AddOccupyingUnits(new List<Unit>() { arrivedUnit });
            }
            else
            {
                toCity2BackupUnitList.Add(tUnit);
            }
        }

        // Replace the units en route list with the units that are still 
        // en route.
        unitsEnRouteTo[city2] = toCity2BackupUnitList;
    }

    /// <summary>
    /// Returns the units headed towards a particular city.
    /// </summary>
    public List<Unit> GetUnitsEnRouteTo(City destination) {
        List<Unit> units = new List<Unit>();
        foreach (TravellingUnit tUnit in unitsEnRouteTo[destination]) {
            units.Add(tUnit.unit);
        }
        return units;
    }
}
