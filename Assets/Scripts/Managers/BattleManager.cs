using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour 
{
    public static BattleManager instance { get; set; }

    void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Performs a cycle of ranged attacks and counter attacks.
    /// Attackers hit first, and defenders get to counter attack with any 
    /// surviving units.
    /// </summary>
    public void RangedOnlyAttack(List<Unit> attackers, List<Unit>  defenders) 
    {
        LaunchRangedAttack(attackers, defenders);
        LaunchRangedAttack(defenders, attackers);
    }

    /// <summary>
    /// Performs a full cycle of attacks and counter attacks, includes ranged and melee.
    /// Attackers hit first, and defenders get to counter attack with any 
    /// surviving units.
    /// </summary>
    public void FullAttack(List<Unit> attackers, List<Unit> defenders)
    {
        RangedOnlyAttack(attackers, defenders);

        // Melee attack and counter attack
        LaunchMeleeAttack(attackers, defenders);
        LaunchMeleeAttack(defenders, attackers);
    }

    /// <summary>
    /// Copies the provided attackers and defenders and performs a mock ranged battle.
    /// Returns a struct with the results.
    /// </summary>
    public BattlePreview PreviewRangedBattle(List<Unit> attackers, List<Unit> defenders) 
    {
        List<Unit> attackersCopy = new List<Unit>();
        List<Unit> defendersCopy = new List<Unit>();

        // Create a copy of the attackers
        foreach (Unit attacker in attackers) {
            attackersCopy.Add(attacker.GetCopy());
        }

        // Create a copy of the attackers
        foreach (Unit defender in defenders)
        {
            defendersCopy.Add(defender.GetCopy());
        }

        RangedOnlyAttack(attackersCopy, defendersCopy);

        return new BattlePreview(attackersCopy, defendersCopy);
    }

    /// <summary>
    /// Copies the provided attackers and defenders and performs a mock battle.
    /// Returns a struct with the results.
    /// </summary>
    public BattlePreview PreviewFullBattle(List<Unit> attackers, List<Unit> defenders)
    {
        List<Unit> attackersCopy = new List<Unit>();
        List<Unit> defendersCopy = new List<Unit>();

        // Create a copy of the attackers
        foreach (Unit attacker in attackers)
        {
            attackersCopy.Add(attacker.GetCopy());
        }

        // Create a copy of the attackers
        foreach (Unit defender in defenders)
        {
            defendersCopy.Add(defender.GetCopy());
        }

        FullAttack(attackersCopy, defendersCopy);

        return new BattlePreview(attackersCopy, defendersCopy);
    }

    /// <summary>
    /// Performs ranged attack logic between two sets of units.
    /// </summary>
    private void LaunchRangedAttack(List<Unit> attackers, List<Unit> defenders)
    {
        /// Calculate combined strength of the attacking units
        int combinedRangedAttack = 0;
        foreach (Unit attacker in attackers)
        {
            combinedRangedAttack += attacker.ranged;
        }

        /// Calculate how much damage should be disbursed to each defender
        //! This is imperfect, since it is a float (or double) being truncated into an integer
        //! The randomness is okay for now, but might need to be addressed later
        int damagePerDefender = combinedRangedAttack / defenders.Count;

        foreach (Unit defender in defenders)
        {
            defender.TakeDamage(damagePerDefender);
        }
    }

    /// <summary>
    /// Performs melee attack logic between two sets of units.
    /// </summary>
    private void LaunchMeleeAttack(List<Unit> attackers, List<Unit> defenders)
    {
        /// Calculate combined strength of the attacking units
        int combinedMeleeAttack = 0;
        foreach (Unit attacker in attackers)
        {
            combinedMeleeAttack += attacker.melee;
        }

        /// Calculate how much damage should be disbursed to each defender
        //! This is imperfect, since it is a float (or double) being truncated into an integer
        //! The randomness is okay for now, but might need to be addressed later
        int damagePerDefender = combinedMeleeAttack / defenders.Count;

        foreach (Unit defender in defenders)
        {
            defender.TakeDamage(damagePerDefender);
        }
    }
}
