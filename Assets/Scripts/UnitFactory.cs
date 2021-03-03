using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitFactory
{   
    private static int infantryCount = 0;
    private static int rangedCount = 0;
    private static int cavalryCount = 0;
    
    public static Troop GenerateInfantry() {
        infantryCount++;
        return new Troop($"{UnitFactory.countToOrdinal(infantryCount)} Infantry", TroopClassifications.INFANTRY);
    }

    public static Troop GenerateRanged()
    {
        rangedCount++;
        return new Troop($"{UnitFactory.countToOrdinal(rangedCount)} Ranged", TroopClassifications.RANGED);
    }
    
    public static Troop GenerateCavalry()
    {
        cavalryCount++;
        return new Troop($"{UnitFactory.countToOrdinal(cavalryCount)} Cavalry", TroopClassifications.CAVALRY);
    }

    /// <summary>
    /// Creates an ordinal number from an integer.
    /// Used for naming units.
    /// </summary>
    private static string countToOrdinal(int count) {
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
