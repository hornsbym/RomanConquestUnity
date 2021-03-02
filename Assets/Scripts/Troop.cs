using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troop : Unit 
{
    public Troop(string name, TroopClassifications classification) 
    {
        unitName = name;

        switch(classification) {
            case TroopClassifications.INFANTRY:
                applyInfantryStats();
                break;
            case TroopClassifications.RANGED:
                applyRangedStats();
                break;
            case TroopClassifications.CAVALRY:
                applyCavalryStats();
                break;
        }
    }

    /// <summary>
    /// Gives the troop the starting stats of an infantry unit.
    /// </summary>
    private void applyInfantryStats(){
        melee = BaseStats.INFANTRY_MELEE;
        ranged = BaseStats.INFANTRY_RANGED;
        movement = BaseStats.INFANTRY_MOVEMENT;
        defense = BaseStats.INFANTRY_DEFENSE;
    }

    /// <summary>
    /// Gives the troop the starting stats of a ranged unit.
    /// </summary>
    private void applyRangedStats()
    {
        melee = BaseStats.RANGED_MELEE;
        ranged = BaseStats.RANGED_RANGED;
        movement = BaseStats.RANGED_MOVEMENT;
        defense = BaseStats.RANGED_DEFENSE;
    }

    /// <summary>
    /// Gives the troop the starting stats of a cavalry unit.
    /// </summary>
    private void applyCavalryStats()
    {
        melee = BaseStats.CAVALRY_MELEE;
        ranged = BaseStats.CAVALRY_RANGED;
        movement = BaseStats.CAVALRY_MOVEMENT;
        defense = BaseStats.CAVALRY_DEFENSE;
    }
}