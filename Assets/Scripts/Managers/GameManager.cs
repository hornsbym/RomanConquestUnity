﻿using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }
    
    public Dictionary<Allegiance, Leader> allegianceToLeaderMapping;
    public Allegiance playerAllegiance;

    public Unit unitSelection { get; set; }
    public City currentCity { get; set; }
    public int turnCount { get; set; }
    
    void Awake() {
        EventManager.OnSelectedCityUpdated += SelectCity;
        EventManager.OnSelectedRoadUpdatedEvent += SelectRoad;
        EventManager.OnDefaultSelected += DeselectAll;
        EventManager.OnTurnBegin += BeginTurn;
        EventManager.OnTurnEnd += EndTurn;
    }

    void Start()
    {   
        instance = this;
        unitSelection = null;
        turnCount = 0;

        playerAllegiance = Allegiance.ROMAN;

        InitializeCivLeaders();

        /// Start the first turn manually
        BeginTurn();
    }

    /// <summary>
    /// Creates new civilization leaders and adds them to the dictionary tracking it.
    /// </summary>
    private void InitializeCivLeaders() 
    {   
        allegianceToLeaderMapping = new Dictionary<Allegiance, Leader>();

        allegianceToLeaderMapping.Add(Allegiance.ROMAN, new Leader("Cesar", Allegiance.ROMAN));
        allegianceToLeaderMapping.Add(Allegiance.GALLIC, new Leader("Vercingetorix", Allegiance.GALLIC));
        allegianceToLeaderMapping.Add(Allegiance.INDEPENDENT, new Leader("Revolter", Allegiance.INDEPENDENT));
    }

    /// <summary>
    /// Initiates begin-of-turn logic.
    /// </summary>
    private void BeginTurn()
    {
        turnCount++;
        CollectAllTaxes();

        // TODO: Debug mode only
        PrintDebugInformation();
    }

    /// <summary>
    /// Prints a bunch of useful information to the console.
    /// Only used for debugging without relying on the UI.
    /// </summary>
    private void PrintDebugInformation() {
        void PrintCityInformation(City city)
        {
            print(city.placeName + ": " + city.allegiance.ToString() + ", " + city.wealth + " wealth, " + city.taxRate + " tax rate, " + city.publicUnrest + " public unrest");
        }

        print("***** Turn " + turnCount + " *****");
        print(allegianceToLeaderMapping[Allegiance.ROMAN].gold + " Gold");
        foreach(City city in MapManager.instance.cities) 
        {
            PrintCityInformation(city);
        }
        print("********************");
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
    /// Returns the player's leader object based on the allegiance set in the Start() method.
    /// </summary>
    public Leader GetPlayerLeaderObject() 
    {
        return allegianceToLeaderMapping[playerAllegiance];
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
