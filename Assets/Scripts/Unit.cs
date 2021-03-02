using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Unit
{
    public string unitName {get; set;}
    protected int health { get; set; }
    protected int melee { get; set; }
    protected int ranged { get; set; }
    protected int movement { get; set; }
    protected int defense { get; set; }
    protected int cost { get; set; }
    protected int xp { get; set; }
    public int level { get; set; }

    /// <summary>
    /// Subtracts the damage from the unit's current health. 
    /// </summary>
    public virtual void TakeDamage(int damage){
        health -= damage;
    }
}
