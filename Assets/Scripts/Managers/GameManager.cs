using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; set;}

    public Unit unitSelection {get; set;}
    public City currentCity {get; set;}

    void Awake() {
        EventManager.OnCitySelected += SelectCity;
        EventManager.OnRoadSelected += SelectRoad;
        EventManager.OnDefaultSelected += DeselectAll;
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        unitSelection = null;
    }


    /// <summary>
    /// Select a Unit.
    /// </summary>
    public void SelectUnit(Unit unit) {
        unitSelection = unit;
    }

    /// <summary>
    /// Sets the selection to a city.
    /// </summary>
    public void SelectCity(City city) 
    {
        DeselectAll();
        this.currentCity = city;
        print($"{city.placeName} selected");
    }

    /// <summary>
    /// Sets the selection to a road.
    /// </summary>
    public void SelectRoad(Road road) 
    {
        DeselectAll();
        print($"{road.placeName} selected");
    }

    /// <summary>
    /// Sets the selected city to null.
    /// </summary>
    public void DeselectAll()
    {
        unitSelection = null;
    }
}
