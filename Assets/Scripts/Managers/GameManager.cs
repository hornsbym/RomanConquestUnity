using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }
    
    public Dictionary<Allegiance, Leader> leaderToCivMapping;

    public Unit unitSelection { get; set; }
    public City currentCity { get; set; }
    public int turnCount { get; set; }
    
    void Awake() {
        EventManager.OnCitySelected += SelectCity;
        EventManager.OnRoadSelected += SelectRoad;
        EventManager.OnDefaultSelected += DeselectAll;
        EventManager.OnTurnBegin += BeginTurn;
        EventManager.OnTurnEnd += EndTurn;
    }

    // Start is called before the first frame update
    void Start()
    {   
        instance = this;
        unitSelection = null;
        turnCount = 0;

        InitializeCivLeaders();


        /// Start the first turn manually
        BeginTurn();
    }

    /// <summary>
    /// Creates new civilization leaders and adds them to the dictionary tracking it.
    /// </summary>
    private void InitializeCivLeaders() 
    {   
        leaderToCivMapping = new Dictionary<Allegiance, Leader>();

        leaderToCivMapping.Add(Allegiance.ROMAN, new Leader("Cesar", Allegiance.ROMAN));
        leaderToCivMapping.Add(Allegiance.GALLIC, new Leader("Vercingetorix", Allegiance.GALLIC));
        leaderToCivMapping.Add(Allegiance.INDEPENDENT, new Leader("Revolter", Allegiance.INDEPENDENT));
    }

    /// <summary>
    /// Initiates begin-of-turn logic.
    /// </summary>
    private void BeginTurn()
    {
        turnCount++;
        CollectAllTaxes();

        ///// TODO: Remove once UI has been updated
        print(
            "Turn " + turnCount + ", " +
            leaderToCivMapping[Allegiance.ROMAN].gold + " Gold");
    }

    /// <summary>
    /// Initiates end-of-turn logic.
    /// </summary>
    private void EndTurn()
    {
        ///// PUT ALL TURN-END LOGIC ABOVE THIS POINT /////
        /// The last thing that should be done at the turn end is initiate the next turn's beginning.
        EventManager.instance.fireTurnBeginEvent();
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
    }

    /// <summary>
    /// Sets the selection to a road.
    /// </summary>
    private void SelectRoad(Road road) 
    {
        DeselectAll();
    }

    /// <summary>
    /// Sets the selected city to null.
    /// </summary>
    private void DeselectAll()
    {
        unitSelection = null;
    }

    /// <summary>
    /// Automatically gives the leaders of each empire their tax money.
    /// </summary>
    private void CollectAllTaxes() 
    {
        foreach (City city in MapManager.instance.cities) {
            city.CollectTaxes();
        }
    }
}
