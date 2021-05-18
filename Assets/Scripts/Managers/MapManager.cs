using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    /// Makes it a universally accessible singleton
    public static MapManager instance;

    public GameObject cityMarkerPrefab;
    public GameObject roadMarkerPrefab;

    // TODO: Do I need to keep a reference to these?
    public List<City> cities;
    public Dictionary<City, Dictionary<City, Road>> roads;

    // Start is called before the first frame update
    void Start()
    {   
        instance = this;
        cities = new List<City>();
        roads = new Dictionary<City, Dictionary<City, Road>>();
        InitializeCities();
    }

    // Update is called once per frame
    void Update()
    {
        // Handles city deselection
        if (Input.GetMouseButtonDown(1))
        {
            EventManager.instance.fireDefaultSelectedEvent();
        }
    }

    /// <summary>
    /// Responsible for putting all cities on the map.
    /// Also determines which cities should be connected with a road
    /// by setting the citys neighbors.
    /// </summary>
    private void InitializeCities()
    {
        /// Create the cities and city map markers here.
        City rome = SpawnCityMarker("Rome", new Vector3(.91f, -4.15f, 0f), Allegiance.ROMAN, 20, new List<TroopClassification> { TroopClassification.INFANTRY })
            .GetComponent<City>();
        City arretium = SpawnCityMarker("Arretium", new Vector3(.55f, -3.65f, 0f), Allegiance.INDEPENDENT, 10)
            .GetComponent<City>();
        City neapolis = SpawnCityMarker("Neapolis", new Vector3(1.39f, -4.45f, 0f), Allegiance.INDEPENDENT, 10)
            .GetComponent<City>();
        City genua = SpawnCityMarker("Genua", new Vector3(-.1f, -3.23f, 0f), Allegiance.INDEPENDENT, 10)
            .GetComponent<City>();
        City vienne = SpawnCityMarker("Vienne", new Vector3(-.8f, -2.9f, 0f), Allegiance.INDEPENDENT, 10, new List<TroopClassification>() { TroopClassification.INFANTRY })
            .GetComponent<City>();
        City aventicum = SpawnCityMarker("Aventicum", new Vector3(-.51f, -2.24f, 0f), Allegiance.INDEPENDENT, 10)
            .GetComponent<City>();
        City cenabum = SpawnCityMarker("Cenabum", new Vector3(-2.03f, -1.97f, 0f), Allegiance.GALLIC, 20, new List<TroopClassification> { TroopClassification.CAVALRY })
            .GetComponent<City>();
            
        
        /// Connect the cities by declaring neighbors.
        /// Road objects will be build automatically based on this data.
        RecordNeighbors(rome, arretium, 1);
        RecordNeighbors(rome, neapolis, 1);
        RecordNeighbors(arretium, genua, 2);
        RecordNeighbors(genua, arretium, 2);
        RecordNeighbors(genua, vienne, 2);
        RecordNeighbors(vienne, aventicum, 3);
        RecordNeighbors(vienne, cenabum, 5);

        // Connect the cities with roads
        foreach(City city in cities){
            city.ConnectWithNeighbors();
        }
    }

    /// <summary>
    /// Record each city as neighbors to one another.
    /// </summary>
    private void RecordNeighbors(City city1, City city2, int distance) 
    {
        city1.AddNeighbor(city2, distance);
        city2.AddNeighbor(city1, distance);
    }

    /// <summary>
    /// Instantiates a city marker and returns a reference to it.
    /// The "real" data is maintained in the marker's "City" component, which is also created here.
    /// </summary>
    public GameObject SpawnCityMarker(string cityName, Vector3 cityPosition, Allegiance allegiance, int wealth, List<TroopClassification> troopsForSale = null)
    {
        GameObject marker = (GameObject) Instantiate(cityMarkerPrefab, cityPosition, Quaternion.identity);
        City city = marker.AddComponent<City>();
        city.placeName = cityName;
        city.allegiance = allegiance;
        city.wealth = wealth;
        city.governor = new Governor(city.placeName + " Governor", city, AIStrategy.DEFAULT);

        if (troopsForSale != null) {
            city.unitsForSale = troopsForSale;
        } else {
            city.unitsForSale = new List<TroopClassification>();
        }

        /// Keep track of the city in an array.
        /// Why? For connecting the cities with roads.
        cities.Add(city);
        
        return marker;
    }

    /// <summary>
    /// Instantiates a road marker between two cities and returns a reference to it.
    /// </summary>
    public Road SpawnRoadMarker(City city1, City city2, int roadLength)
    {   
        // Instantiate object
        GameObject marker = (GameObject) Instantiate(roadMarkerPrefab);
        Road road = marker.AddComponent<Road>();
        road.city1 = city1;
        road.city2 = city2;
        road.defaultTurnCount = roadLength;
        road.InitializeRoad();

        /// If the roads dictionary already contains a dictionary of roads for a city,
        /// add the road to that dictionary. 
        if (this.roads.ContainsKey(city1)) {
            this.roads[city1].Add(city2, road);
        } 
        /// If not, create a new dictionary and add to it.
        else {
            this.roads[city1] = new Dictionary<City, Road>();
            this.roads[city1].Add(city2, road);
        }

        /// Do the same the second city to the first
        if (this.roads.ContainsKey(city2)) {
            this.roads[city2].Add(city1, road);
        } else {
            this.roads[city2] = new Dictionary<City, Road>();
            this.roads[city2].Add(city1, road);
        }

        return road;
    }

    /// <summary>
    /// Finds a road from the roads dictionary.
    /// </summary>
    public Road GetRoad(City startingCity, City destination) 
    {
        return this.roads[startingCity][destination];
    }

    // TODO: Make the road creation process more efficient.
    // Currently operating at O(n^2) operations at least.

    /// <summary>
    /// If no road exists between the two provided cities, create one.
    /// If a road alredy exists, do nothing.
    /// </summary>
    public Road TryToCreateRoad(City city1, City city2, int length) {
        if (!MapManager.instance.DoesRoadExist(city1, city2))
        {
            // If we get here, the road doesn't exist so we should make one and 
            // add it to the MapManager's roads list.
            return SpawnRoadMarker(city1, city2, length);
        } else {
            return null;
        }
    }

    /// <summary>
    /// Checks whether a road exists between the two cities.
    /// </summary>
    private bool DoesRoadExist(City city1, City city2)
    {
        /// If there's no nested dictionary for the first city, return false
        if (!roads.ContainsKey(city1)) {
            return false;
        } else {
            /// If there is a nested dictionary but it doesn't contain the second city, return false.
            if (!roads[city1].ContainsKey(city2)) {
                return false;
            } 
            
            /// If the dictionary exists for the first city and it contains a road to the second city, return true.
            else {
                return true;
            }
        }
    }
}
