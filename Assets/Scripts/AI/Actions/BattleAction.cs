using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Chooses troops and type of attack.
/// All attackers don't have to be used, but defenders will always defend together.
/// </summary>
public class BattleAction : Action
{
    private List<Unit> eligibleAttackers { get; set; }
    private List<Unit> defenders { get; set; }
    private enum AttackType { RANGED, FULL }

    public BattleAction(List<Unit> eligibleAttackers, List<Unit> defenders) 
    {
        this.eligibleAttackers = eligibleAttackers;
        this.defenders = defenders;
    }

    public override void Execute()
    {
        List<Unit> attackers = ChooseAttackers();
        AttackType attackType = ChooseAttackType(attackers);

        switch (attackType) {
            case (AttackType.RANGED):
                // TODO: Get the city being assaulted if necessary
                BattleManager.instance.RangedOnlyAttack(attackers, defenders, null);
                break;
            case (AttackType.FULL):
                // TODO: Get the city being assaulted if necessary
                BattleManager.instance.FullAttack(attackers, defenders);
                break;
            default:
                break;
        }
    }

    public override bool IsPossible()
    {
        /// We won't check whether or not the attackers are adjacent to the defenders here.
        /// That check will need to happen higher (probably in the "Place" object).
        
        // TODO: What are the limitations on when attacks are possible?
        return true;
    }

    public override void Evaluate()
    {
        /// TODO: Fill out evaluation logic
        this.value = 5;
    }

    /// <summary>
    /// Chooses which of the eligible attackers should be used to attack.
    /// </summary>
    private List<Unit> ChooseAttackers()
    {
        // TODO: Fill in attacker selection logic.
        // TODO: This should balance the current health of the attackers and the current health of the defenders,
        // TODO: along with the Leader's current strategy.
        return eligibleAttackers;
    }

    /// <summary>
    /// Chooses which tpe of attack should be performed given
    /// which troops are selected for attack.
    /// </summary>
    private AttackType ChooseAttackType(List<Unit> selectedAttackers)
    {
        // TODO: Fill in selection logic
        // TODO: This logic should balance attack damage taken with damage given. 
        return AttackType.FULL;
    }



}
