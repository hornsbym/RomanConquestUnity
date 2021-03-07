using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public interface IUnit
{
    string unitName {get; set;}
    int health { get; set; }
    int melee { get; set; }
    int ranged { get; set; }
    int movement { get; set; }
    int defense { get; set; }
    int cost { get; set; }
    int xp { get; set; }
    int xpToNextLevel { get; set; }
    int level { get; set; }

    /// <summary>
    /// Subtracts the damage from the unit's current health. 
    /// </summary>
    void TakeDamage(int damage);

}