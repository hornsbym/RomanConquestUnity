using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

/// <summary>
/// Tests pertaining to battle logic.
/// </summary>
public class BattleManagerTests : MonoBehaviour
{
    public static BattleManagerTests instance;

    public void Awake() 
    {
        instance = this;
    }

    public void Execute()
    {
        TestDamageUnit();
    } 

    private void TestDamageUnit() 
    {
        int damage = 10;

        Troop troop = UnitFactory.instance.GenerateTroop(TroopClassification.INFANTRY, Allegiance.ROMAN)[0];

        troop.TakeDamage(10);

        int expectedResult = TroopStats.statLedger[TroopClassification.INFANTRY][Stat.HEALTH] - damage;

        Assert.AreEqual(expectedResult, troop.health);
    }
}
