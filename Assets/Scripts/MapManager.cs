using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    /// Makes it a universally accessible singleton
    public static MapManager instance;

    public GameObject cityMarkerPrefab;
    public GameObject roadMarkerPrefab;

    // Links a city to a city marker via a dictionary
    private Dictionary<GameObject, City> markerToCityDict;

    // TODO: Do I need to keep a reference to these?
    public List<City> cities;
    public List<Road> roads;

    // Start is called before the first frame update
    void Start()
    {   
        instance = this;
        cities = new List<City>();
        roads = new List<Road>();
        markerToCityDict = new Dictionary<GameObject, City>();
        InitializeCities();
    }

    // Update is called once per frame
    void Update()
    {
        // Handles city deselection
        if (Input.GetMouseButtonDown(1))
        {
            GameManager.instance.DeselectCity();
        }
    }

    /// <summary>
    /// Responsible for putting all cities on the map.
    /// Also determines which cities should be connected with a road
    /// by setting the citys neighbors.
    /// </summary>
    private void InitializeCities()
    {
        /// Create the cities here.
        /// They will call the "SpawnCityMarker" method themselves.
        City rome = new City("Rome", new Vector3(.91f, -4.15f, 0f), new TroopClassifications[]{TroopClassifications.INFANTRY, TroopClassifications.RANGED});
        City florence = new City("Florence", new Vector3(.55f, -3.65f, 0f), new TroopClassifications[] {TroopClassifications.INFANTRY, TroopClassifications.CAVALRY});
        City naples = new City("Naples", new Vector3(1.39f, -4.45f, 0f), new TroopClassifications[] {TroopClassifications.RANGED});

        /// Connect the cities by declaring neighbors.
        /// Road objects will be build automatically based on this data.
        /// TODO: How do we keep from getting duplicate roads?
        rome.neighbors = new List<City> {florence, naples};
        florence.neighbors = new List<City> {rome};
        naples.neighbors = new List<City> {rome};

        cities.Add(rome);
        cities.Add(florence);
        cities.Add(naples);

        // Connect the cities with roads
        foreach(City city in cities){
            city.ConnectWithNeighbors();
        }
    }

    /// <summary>
    /// Returns the city object associated with a particular
    /// city marker.
    /// </summary>
    public City GetCityFromDict(GameObject marker) {
        // Placeholder for trying to get a city object.
        City city = null;

        // Try to get the city object.
        // Will return null if there isn't one (should never happen).
        markerToCityDict.TryGetValue(marker, out city);

        return city;
    }

    /// <summary>
    /// Instantiates a city marker and returns a reference to it.
    /// </summary>
    public GameObject SpawnCityMarker(City city)
    {
        GameObject marker = (GameObject) Instantiate(cityMarkerPrefab, city.cityPosition, Quaternion.identity);
        markerToCityDict.Add(marker, city);
        return marker;
    }

    /// <summary>
    /// Instantiates a road marker between two cities and returns a reference to it.
    /// </summary>
    public GameObject SpawnRoadMarker(Road road)
    {   
        // Necessary for calculating how much scaling is necessary
        float roadMarkerLength = roadMarkerPrefab.GetComponent<MeshRenderer>().bounds.size.y;

        // Instantiate object
        GameObject marker = (GameObject) Instantiate(roadMarkerPrefab, road.position, road.rotation);

        // Scale the road 
        // TODO:  WHY DO WE NEED TO DIVIDE LENGTH BY 2??
        marker.transform.localScale = new Vector3(
            marker.transform.localScale.x,
            road.length/2,
            marker.transform.localScale.z
        );

        return marker;
    }

    /// <summary>
    /// Checks whether a road exists between the two cities.
    /// </summary>
    private bool DoesRoadExist(City city1, City city2) {
        foreach(Road road in roads) {
            if (road.city1 == city1 && road.city2 == city2)
            {
                return true;
            }
            else if (road.city2 == city1 && road.city1 == city2)
            {
                return true;
            }
        }

        return false;
    }

    // TODO: Make the road creation process more efficient.
    // Currently operating at O(n^2) operations at least.

    /// <summary>
    /// If no road exists between the two provided cities, create one.
    /// If a road alredy exists, do nothing.
    /// </summary>
    public void TryToCreateRoad(City city1, City city2) {
        if (!MapManager.instance.DoesRoadExist(city1, city2))
        {
            // If we get here, the road doesn't exist so we should make one and 
            // add it to the MapManager's roads list.
            MapManager.instance.roads.Add(new Road(city1, city2));
        }
    }
}
