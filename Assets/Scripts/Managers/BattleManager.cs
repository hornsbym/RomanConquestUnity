using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour 
{
    public static BattleManager instance { get; private set; }

    void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Performs a cycle of ranged attacks and counter attacks.
    /// Attackers hit first, and defenders get to counter attack with any 
    /// surviving units.
    /// If the city parameter is provided, we are assuming the attack is an assault on a city.
    /// </summary>
    public void RangedOnlyAttack(List<Unit> attackers, List<Unit>  defenders, City city) 
    {
        LaunchRangedAttack(attackers, defenders, city);
        LaunchRangedAttack(defenders, attackers, city);
    }

    /// <summary>
    /// Performs a full cycle of attacks and counter attacks, includes ranged and melee.
    /// Attackers hit first, and defenders get to counter attack with any 
    /// surviving units.
    /// If the city parameter is provided, we are assuming the attack is an assault on a city.
    /// </summary>
    public void FullAttack(List<Unit> attackers, List<Unit> defenders, City city = null)
    {
        RangedOnlyAttack(attackers, defenders, city);

        // Melee attack and counter attack
        LaunchMeleeAttack(attackers, defenders, city);
        LaunchMeleeAttack(defenders, attackers, city);
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

        RangedOnlyAttack(attackersCopy, defendersCopy, null);

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

        FullAttack(attackersCopy, defendersCopy, null);

        return new BattlePreview(attackersCopy, defendersCopy);
    }

    /// <summary>
    /// Performs ranged attack logic between two sets of units.
    /// Ranged attacks will not result in a city capture
    /// </summary>
    private void LaunchRangedAttack(List<Unit> attackers, List<Unit> defenders, City city = null)
    {
        if (defenders.Count > 0){
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
    }

    /// <summary>
    /// Performs melee attack logic between two sets of units.
    /// If a city is provided, we will assume this is an assault on a city.
    /// In that case, if all enemy units fall the city allegiance will change and
    /// the attacking troops will be moved into the city. 
    /// </summary>
    private void LaunchMeleeAttack(List<Unit> attackers, List<Unit> defenders, City city = null)
    {
        if (defenders.Count > 0) {
            /// Calculate combined strength of the attacking units
            int combinedMeleeAttack = 0;
            int defenderDeathCount = 0;

            foreach (Unit attacker in attackers) {
                combinedMeleeAttack += attacker.melee;
            }

            /// Calculate how much damage should be disbursed to each defender
            //! This is imperfect, since it is a float (or double) being truncated into an integer
            //! The randomness is okay for now, but might need to be addressed later
            int damagePerDefender = combinedMeleeAttack / defenders.Count;

            foreach (Unit defender in defenders) {
                defender.TakeDamage(damagePerDefender);
                if (defender.health <= 0) {
                    defenderDeathCount++;
                }
            }

            // If the death count matches the length of the defenders list,
            // perform a city capture.
            // TODO: Come up with a better way to figure out if all defenders died.
            // TODO: Right now, we are just counting the dead and comparing that count to the length of the list.
            // TODO: It would be better if we could just remove them directly upon unit death.
            if ((defenderDeathCount == defenders.Count) && city != null) {
                CaptureCity(attackers, city);
            }
        } else if ((attackers.Count > 0 && defenders.Count == 0) && city != null) {
            CaptureCity(attackers, city);
        }
    }

    /// <summary>
    /// Performs all logic necessary to capture a city.
    /// Changes the city to the allegiance provided.
    /// </summary>
    void CaptureCity(List<Unit> capturingUnits, City capturedCity)
    {
        Allegiance capturingUnitsAllegiance = capturingUnits[0].allegiance;
        Leader leader = GameManager.instance.allegianceToLeaderMapping[capturingUnitsAllegiance];
        capturedCity.ChangeAllegiance(capturingUnitsAllegiance);
        capturedCity.AddOccupyingUnits(capturingUnits);
    }
}
