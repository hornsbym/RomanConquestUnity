using System;

public abstract class Building: IEquatable<Building>
{
    public string buildingName { get; protected set; }
    public int cost { get; protected set; }

    public bool Equals(Building building)
    {
        if (building.buildingName == this.buildingName) {
            return true;
        } else {
            return false;
        }
    }

    public static bool operator == (Building buildingA, Building buildingB) 
    {
        return buildingA.Equals(buildingB);
    }

    public static bool operator !=(Building buildingA, Building buildingB)
    {
        return !(buildingA.Equals(buildingB));
    }

    /// <summary>
    /// Code relating to what a city does should be put here.
    /// Cities will call this method when the building is built.
    /// This should modify the city (might require minor City class modifications to make the effect
    /// possible).
    /// </summary>
    public abstract void ApplyEffect(City city);

    /// <summary>
    /// Code relating to what a city does should be put here.
    /// Cities will call this method when the building is destroyed.
    /// This should modify the city (might require minor City class modifications to make the effect
    /// possible).
    /// This should undo any effects applied previously applied to a city.
    /// </summary>
    public abstract void RemoveEffect(City city);
}
