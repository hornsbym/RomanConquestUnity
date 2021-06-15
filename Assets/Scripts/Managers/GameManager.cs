using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }
    
    public bool debugMode;
    
    public Dictionary<Allegiance, Emperor> allegianceToLeaderMapping { get; set; }
    public Allegiance playerAllegiance;
    public List<Emperor> emperors;

    public Unit unitSelection { get; set; }
    public City currentCity { get; set; }
    public int turnCount { get; set; }
    
    void Awake() {
        debugMode = true;

        EventManager.OnSelectedCityUpdated += SelectCity;
        EventManager.OnSelectedRoadUpdatedEvent += SelectRoad;
        EventManager.OnDefaultSelected += DeselectAll;
        EventManager.OnTurnBegin += BeginTurn;
        EventManager.OnTurnEnd += EndTurn;

        playerAllegiance = Allegiance.ROMAN;

        InitializeCivLeaders();
    }

    void Start()
    {   
        instance = this;
        unitSelection = null;
        turnCount = 0;

        /// Start the first turn manually
        BeginTurn();
    }


    /// <summary>
    /// Returns the player's leader object based on the allegiance set in the Start() method.
    /// </summary>
    public Emperor GetPlayerLeader() 
    {
        return allegianceToLeaderMapping[playerAllegiance];
    }

    /// <summary>
    /// Creates new civilization leaders and adds them to the dictionary tracking it.
    /// </summary>
    private void InitializeCivLeaders() 
    {   
        allegianceToLeaderMapping = new Dictionary<Allegiance, Emperor>();
        emperors = new List<Emperor>();

        Emperor roman = new Emperor("Cesar", Allegiance.ROMAN);
        Emperor gallic = new Emperor("Vercingetorix", Allegiance.GALLIC);

        allegianceToLeaderMapping.Add(Allegiance.ROMAN, roman);
        allegianceToLeaderMapping.Add(Allegiance.GALLIC, gallic);
        allegianceToLeaderMapping.Add(Allegiance.INDEPENDENT, null);
        allegianceToLeaderMapping.Add(Allegiance.NONE, null);

        emperors.Add(roman);
        emperors.Add(gallic);
    }

    /// <summary>
    /// Initiates begin-of-turn logic.
    /// </summary>
    private void BeginTurn()
    {
        turnCount++;
        CollectAllTaxes();
        ResetCityActions(playerAllegiance);

        // Debug mode only
        PrintDebugInformation();
    }

    /// <summary>
    /// Prints a bunch of useful information to the console.
    /// Only used for debugging without relying on the UI.
    /// </summary>
    private void PrintDebugInformation() {
        void PrintCityInformation(City city)
        {
            Utilities.instance.Debug(city.placeName + ": " + city.allegiance.ToString() + ", " + city.wealth + " wealth, " + city.taxRate + " tax rate, " + city.publicUnrest + " public unrest");
        }

        void PrintRoadInformation(Road road) 
        {
            Utilities.instance.Debug(road.placeName + ": " + road.travellingUnits.Count + " tUnits, " + road.occupyingUnits.Count + " units");
        }

        Utilities.instance.Debug("***** Turn " + this.turnCount + " *****");
        Utilities.instance.Debug(allegianceToLeaderMapping[Allegiance.ROMAN].gold + " Gold");

        City rome = null;
        City neapolis = null;

        foreach(City city in MapManager.instance.cities) 
        {
            PrintCityInformation(city);

            if (city.placeName == "Rome") {
                rome = city;
            } else if (city.placeName == "Neapolis") {
                neapolis = city;
            }

        }

        if (rome != null && neapolis != null) {
            PrintRoadInformation(MapManager.instance.GetRoad(rome, neapolis));
        }

        Utilities.instance.Debug("********************");
    }

    /// <summary>
    /// Takes turns for all non-player, non-independent leaders.
    /// </summary>
    private void TakeOpponentTurns() 
    {
        foreach (Emperor leader in emperors)
        {
            if ((leader.allegiance != playerAllegiance) && (leader.allegiance != Allegiance.INDEPENDENT && leader.allegiance != Allegiance.NONE)) {
                leader.TakeTurn();
            }
        }
    }

    /// <summary>
    /// Lets the independent city governors do stuff each turn.
    /// </summary>
    private void TakeGovernorTurns()
    {
        foreach (City city in MapManager.instance.cities) {
            if (city.allegiance == Allegiance.INDEPENDENT || city.allegiance == Allegiance.NONE) {
                city.governor.TakeTurn();
            }
        }
    }

    /// <summary>
    /// Initiates end-of-turn logic.
    /// </summary>
    private void EndTurn()
    {
        TakeOpponentTurns();
        TakeGovernorTurns();

        if (debugMode) 
        {
            Debug.ClearDeveloperConsole();
        }

        /// PUT ALL TURN-END LOGIC ABOVE THIS POINT ///
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

    /// <summary>
    /// Enables the allegiance's cities to perform an action.
    /// </summary>
    private void ResetCityActions(Allegiance allegiance) {
        foreach (City city in MapManager.instance.cities) {
            city.ResetAction();
            
        }
    }
}
