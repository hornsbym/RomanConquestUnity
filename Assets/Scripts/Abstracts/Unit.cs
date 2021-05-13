using UnityEngine;

public abstract class Unit : MonoBehaviour, IUnit
{
    public string unitName { get; set; }
    public Allegiance allegiance { get; set; }
    public int health { get; set; }
    public int melee { get; set; }
    public int ranged { get; set; }
    public int movement { get; set; }
    public int defense { get; set; }
    public int cost { get; set; }
    public int xp { get; set; }
    public int xpToNextLevel { get; set; }
    public int level { get; set; }

    public virtual void TakeDamage(int damage) 
    {
        health -= damage;
    }

}
