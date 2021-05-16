using System.Collections.Generic;

public class Troop : Unit 
{
    public void InitializeTroop(TroopClassification classification, Allegiance allegiance) 
    {
        this.allegiance = allegiance; 
        ApplyStats(classification);
    }

    /// <summary>
    /// Gives the troop pre-defined starting stats based on classifications.
    /// </summary>
    public void ApplyStats(TroopClassification classification) 
    {
        Dictionary<Stat, int> stats = TroopStats.statLedger[classification];

        melee = stats[Stat.MELEE];
        ranged = stats[Stat.RANGED];
        movement = stats[Stat.MOVEMENT];
        defense = stats[Stat.DEFENSE];
        health = stats[Stat.HEALTH];
    }
}