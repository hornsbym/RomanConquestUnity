using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    /// Makes it a universally accessible singleton
    public static MapManager instance;

    public GameObject cityMarkerPrefab;

    // Links a city to a city marker via a dictionary
    private Dictionary<GameObject, City> markerToCityDict;

    // TODO: Do I need to keep a reference to this?
    public List<City> cities;

    // Start is called before the first frame update
    void Start()
    {   
        instance = this;
        cities = new List<City>();
        markerToCityDict = new Dictionary<GameObject, City>();
        InitializeCities();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Responsible for putting all cities on the map.
    /// </summary>
    private void InitializeCities()
    {
        /// Create the cities here.
        /// They will call the "SpawnCityMarker" method themselves.
        cities.Add(new City("Rome", new Vector3(.91f, -4.15f, 0f), new TroopClassifications[]{TroopClassifications.INFANTRY, TroopClassifications.RANGED}));
        cities.Add(new City("Florence", new Vector3(.55f, -3.65f, 0f), new TroopClassifications[] { TroopClassifications.CAVALRY}));
        cities.Add(new City("Naples", new Vector3(1.39f, -4.45f, 0f)));
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
    /// Handles what happens when just the map is clicked.
    /// </summary>
    // void OnMouseDown(){
    //     GameManager.instance.DeselectCity();
    // }
}
