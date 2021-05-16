using System.Collections.Generic;
using UnityEngine;

public class TroopStats : MonoBehaviour
{
    public static Dictionary<TroopClassification, Dictionary<Stat, int>> statLedger;

    void Awake() 
    {
        /// Establish troop statistics here
        Dictionary<Stat, int> cavalry = new Dictionary<Stat, int>(){
            { Stat.HEALTH, 80 },
            { Stat.COST, 10 },
            { Stat.MELEE, 12 },
            { Stat.RANGED, 0 },
            { Stat.MOVEMENT, 3 },
            { Stat.DEFENSE, 2 }
        };
        Dictionary<Stat, int> infantry = new Dictionary<Stat, int>(){
            { Stat.HEALTH, 80 },
            { Stat.COST, 5 },
            { Stat.MELEE, 8 },
            { Stat.RANGED, 2 },
            { Stat.MOVEMENT, 1 },
            { Stat.DEFENSE, 4 }
        };
        Dictionary<Stat, int> ranged = new Dictionary<Stat, int>(){
            { Stat.HEALTH, 80 },
            { Stat.COST, 5 },
            { Stat.MELEE, 2 },
            { Stat.RANGED, 6 },
            { Stat.MOVEMENT, 1 },
            { Stat.DEFENSE, 1 }
        };

        /// Add troop statistics to the ledger here
        TroopStats.statLedger = new Dictionary<TroopClassification, Dictionary<Stat, int>>() {
            { TroopClassification.CAVALRY, cavalry },
            { TroopClassification.INFANTRY, infantry },
            { TroopClassification.RANGED, ranged }
        };
    }
}