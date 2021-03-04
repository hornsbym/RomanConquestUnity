using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

abstract public class Unit : MonoBehaviour
{
    public string unitName {get; set;}
    public int health { get; set; }
    public int melee { get; set; }
    public int ranged { get; set; }
    public int movement { get; set; }
    public int defense { get; set; }
    public int cost { get; set; }
    public int xp { get; set; }
    public int xpToNextLevel { get; set; }
    public int level { get; set; }

    /// <summary>
    /// Subtracts the damage from the unit's current health. 
    /// </summary>
    public virtual void TakeDamage(int damage){
        health -= damage;
    }

}
