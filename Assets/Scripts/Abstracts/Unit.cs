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
    public int xp { get; set; }
    public int xpToNextLevel { get; set; }
    public int level { get; set; }

    public virtual void TakeDamage(int damage) 
    {
        health -= damage;

        // Kills the unit if health reaches 0
        if (health <= 0) {
            Die();
        }
    }

    /// <summary>
    /// Fires a unit died event using this unit as the payload.
    /// </summary>
    public virtual void Die() {
        EventManager.instance.fireUnitDiedEvent(this);
    }

    /// <summary>
    /// Returns a copy of the unit.
    /// The copy != the original.
    /// Useful for mocking turn actions (especially battle actions).
    /// </summary>
    public Unit GetCopy()
    {
        return (Unit) this.MemberwiseClone();
    }

}
