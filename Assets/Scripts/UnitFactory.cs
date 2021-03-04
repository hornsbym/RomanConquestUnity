using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFactory: MonoBehaviour
{   

    public static UnitFactory instance;

    public GameObject troopPrefab;

    private int infantryCount = 0;
    private int rangedCount = 0;
    private int cavalryCount = 0;

    void Start() {
        instance = this;
    }

    public Troop GenerateInfantry() {
        infantryCount++;
        GameObject.Instantiate(troopPrefab);
        Troop troop = troopPrefab.AddComponent<Troop>();
        troop.unitName = $"{countToOrdinal(infantryCount)} Infantry";
        troop.InitializeTroop(TroopClassifications.INFANTRY);
        return troop;
    }

    public Troop GenerateRanged()
    {
        rangedCount++;
        GameObject.Instantiate(troopPrefab);
        Troop troop = troopPrefab.AddComponent<Troop>();
        troop.unitName = $"{countToOrdinal(rangedCount)} Ranged";
        troop.InitializeTroop(TroopClassifications.RANGED);
        return troop;
    }
    
    public Troop GenerateCavalry()
    {
        cavalryCount++;
        GameObject.Instantiate(troopPrefab);
        Troop troop = troopPrefab.AddComponent<Troop>();
        troop.unitName = $"{countToOrdinal(cavalryCount)} Cavalry";
        troop.InitializeTroop(TroopClassifications.CAVALRY);
        return troop;
    }

    /// <summary>
    /// Creates an ordinal number from an integer.
    /// Used for naming units.
    /// </summary>
    private string countToOrdinal(int count) {
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
