using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitFactory : MonoBehaviour
{

    public static UnitFactory instance;

    [SerializeField] private GameObject emptyUnitPrefab;

    /// Organize counts by clasification and allegiance so they can be 
    /// easily accessed later
    private Dictionary<Allegiance, Dictionary<TroopClassification, int>> troopCounts;

    void Start()
    {
        instance = this;
        InitializeTroopCounts();
    }

    private void InitializeTroopCounts()
    {
        troopCounts = new Dictionary<Allegiance, Dictionary<TroopClassification, int>>();

        foreach (Allegiance allegiance in Enum.GetValues(typeof(Allegiance)))
        {
            Dictionary<TroopClassification, int> allegianceTroops = new Dictionary<TroopClassification, int>();

            foreach(TroopClassification troopClass in Enum.GetValues(typeof(TroopClassification))) {
                allegianceTroops.Add(troopClass, 0);
            }

            troopCounts.Add(allegiance, allegianceTroops);
        }
    }

    /// <summary>
    /// Instantiates a new game object with an attached Troop script.
    /// Returns the Troop object in that script.
    /// </summary>
    public List<Troop> GenerateTroop(TroopClassification classification, Allegiance allegiance)
    {
        /// Increment the Allegiance's troop count 
        troopCounts[allegiance][classification] = troopCounts[allegiance][classification] + 1;

        // Create an empty troop game object
        Troop troop = GameObject.Instantiate(emptyUnitPrefab).AddComponent<Troop>();
        troop.unitName = $"{Utilities.instance.CountToOrdinal(troopCounts[allegiance][classification])}" + 
            $" {Utilities.instance.UppercaseFirstLetter(allegiance.ToString())}" + 
            $" {Utilities.instance.UppercaseFirstLetter(classification.ToString())}";

        troop.InitializeTroop(classification, allegiance);

        return new List<Troop>(){ troop };
    }
}
