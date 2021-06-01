using System;
using System.Collections.Generic;

abstract public class CombinedUnit<T>: Unit where T : Unit 
{
    public float cohesion { get; private set; }

    /// Automatically calculate stats anytime the units are set.
    private List<T> _units;
    public List<T> units { 
        get => _units; 
        set {
            this._units = value;
            this.CalculateStats();
        }
    }

    void Awake() {
        cohesion = 1f;
    }

    /// <summary>
    /// Makes all stats 0.
    /// This will be useful for recalculating totals whenever
    /// the unit composition changes.
    /// </summary>
    private void ZeroStats()
    {
        this.health = 0;
        this.melee = 0;
        this.ranged = 0;
        this.defense = 0;
        this.movement = 0;
    }

    /// <summary>
    /// Calculates the combined stats for the combined unit.
    /// </summary>
    protected virtual void CalculateStats() 
    {
        // Reset the stats for the combined unit.
        ZeroStats();

        if (units.Count > 0) {
            CalculateHealth();
            CalculateMelee();
            CalculateRanged();
            CalculateDefense();
            CalculateMovement();
        }
    }

    /// <summary>
    /// Combines the units' health into one value.
    /// Applies a modifier to slightly decrease it.
    /// </summary>
    private void CalculateHealth() 
    {
        foreach (Unit unit in units) 
        {
            this.health += unit.health;
        }

        this.health = (int) (this.health * CombinedUnitConstants.COMBINED_UNIT_HEALTH_MULTIPLIER);
    }

    /// <summary>
    /// Calculates the melee stat.
    /// Applies cohesion bonus.
    /// </summary>
    private void CalculateMelee() 
    {
        foreach (Unit unit in units) 
        {
            this.melee += unit.melee;
        }

        this.melee = (int) (this.melee * cohesion);
    }

    /// <summary>
    /// Calculates the ranged stat.
    /// Applies cohesion bonus.
    /// </summary>
    private void CalculateRanged()
    {
        foreach (Unit unit in units)
        {
            this.ranged += unit.ranged;
        }

        this.ranged = (int) (this.ranged * cohesion);
    }

    /// <summary>
    /// Calculates the defense stat.
    /// Applies cohesion bonus.
    /// </summary>
    private void CalculateDefense()
    {
        foreach (Unit unit in units)
        {
            this.defense += unit.defense;
        }

        this.defense = (int) (this.defense * cohesion);
    }

    /// <summary>
    /// Calculates the movement stat.
    /// </summary>
    private void CalculateMovement()
    {
        foreach (Unit unit in units)
        {
            this.movement += unit.movement;
        }

        this.movement = this.movement/units.Count;
    }
    
    /// <summary>
    /// Overrides the "TakeDamage" method of the Unit base class.
    /// Disperses the damage taken evenly over all units that compose
    /// the combined unit.
    /// </summary>
    override public void TakeDamage(int damage)
    {
        // Calculate damage each unit should take.
        float exactDamagePerUnit = (float) damage / units.Count;

        // Separate the integer and decimal components of the damage each 
        // unit should take.
        int integerDamage = (int) Math.Truncate(exactDamagePerUnit);
        float decimalDamage =  exactDamagePerUnit - integerDamage;

        // Give each unit its fair share of damage
        foreach (Unit unit in units) 
        {
            unit.TakeDamage(integerDamage);
        }

        // Randomly applies the leftover damage.
        int leftoverDamage = (int) (decimalDamage * units.Count);
        for(int i = 0; i < leftoverDamage; i++) 
        {
            System.Random random = new System.Random();
           units[random.Next(units.Count)].TakeDamage(1);
        }
    }

    /// <summary>
    /// Combined units can't die, they're disbanded instead.
    /// Prefer using Disband() method for clarity.
    /// </summary>
    public override void Die() {
        this.Disband();
    }

    /// <summary>
    /// Fires a disband event using this combined unit as a payload. 
    /// <summary>
    public abstract void Disband();
}