using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFactory: MonoBehaviour
{   

    public static UnitFactory instance;

    public GameObject emptyUnitPrefab;

    private int infantryCount = 0;
    private int rangedCount = 0;
    private int cavalryCount = 0;

    void Start() {
        instance = this;
    }

    /// <summary>
    /// Instantiates a new game object with an attached Troop script.
    /// Returns the Troop object in that script.
    /// </summary>
    public Troop GenerateInfantry() {
        infantryCount++;
        Troop troop = GameObject.Instantiate(emptyUnitPrefab).AddComponent<Troop>();
        troop.unitName = $"{countToOrdinal(infantryCount)} Infantry";
        troop.InitializeTroop(TroopClassifications.INFANTRY);
        return troop;
    }

    /// <summary>
    /// Instantiates a new game object with an attached Ranged script.
    /// Returns the Ranged object in that script.
    /// </summary>
    public Troop GenerateRanged()
    {
        rangedCount++;
        Troop troop = GameObject.Instantiate(emptyUnitPrefab).AddComponent<Troop>();
        troop.unitName = $"{countToOrdinal(rangedCount)} Ranged";
        troop.InitializeTroop(TroopClassifications.RANGED);
        return troop;
    }

    /// <summary>
    /// Instantiates a new game object with an attached Cavalry script.
    /// Returns the Cavalry object in that script.
    /// </summary>
    public Troop GenerateCavalry()
    {
        cavalryCount++;
        Troop troop = GameObject.Instantiate(emptyUnitPrefab).AddComponent<Troop>();
        troop.unitName = $"{countToOrdinal(cavalryCount)} Cavalry";
        troop.InitializeTroop(TroopClassifications.CAVALRY);
        return troop;
    }

    /// <summary>
    /// Creates an ordinal number from an integer.
    /// Used for naming units.
    /// </summary>
    public static string countToOrdinal(int count) {
        string stringCount = count.ToString();
        char lastLetter = stringCount[stringCount.Length - 1];

        // Handles any x11, x12, x13
        if (stringCount.Length > 1) {
            char secondToLastLetter = stringCount[stringCount.Length - 2];
            if (secondToLastLetter == '1'){
                stringCount += "th";
                return stringCount;
            }
        }

        // Handles anything else
        switch(lastLetter){
            case '1':
                stringCount += "st";
                break;
            case '2':
                stringCount += "nd";
                break;
            case '3':
                stringCount += "rd";
                break;
            default:
                stringCount += "th";
                break;
        }

        return stringCount;
    }
}
