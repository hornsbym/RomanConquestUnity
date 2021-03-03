using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road 
{
    // The cities connected by the road.
    public City city1;
    public City city2;

    // Instructions for positioning the road marker on the map.
    public Vector3 position;
    public Quaternion rotation; 
    public float length;
    private GameObject marker;

    // Tracks which units are traversing the road and which city 
    // they're destined for.
    public List<Unit> toCity1;
    public List<Unit> toCity2;

    public Road(City city1, City city2) {
        // Set the cities.
        this.city1 = city1;
        this.city2 = city2;

        // Finds the midpoint between the two cities.
        position = new Vector3(
            city2.cityPosition.x + ((city1.cityPosition.x - city2.cityPosition.x) / 2),
            city2.cityPosition.y + ((city1.cityPosition.y - city2.cityPosition.y) / 2),
            0
        );

        // Finds the rotation necessary to point the road between the two cities
        rotation = Quaternion.LookRotation(city2.cityPosition - city1.cityPosition) * Quaternion.Euler(90, 0, 0);
       
        // Determines how long the road needs to be to connect the cities
        length = Vector3.Distance(city1.cityPosition, city2.cityPosition);

        // Spawn the corresponding road marker on the map.
        marker = MapManager.instance.SpawnRoadMarker(this);
    }
}
