/// <summary>
/// A class for tracking a Unit's travelling progress.
/// Basically pairs a Unit with an integer and a destination city.
/// </summary>
public class TravellingUnit
{
    public int turnsToArrival { get; private set; }
    public City destination { get; private set; }
    public Unit unit { get; private set; }

    public TravellingUnit(Unit unit, City destination, int turnCount)
    {
        this.unit = unit;
        this.destination = destination;
        this.turnsToArrival = turnCount;
    }

    /// <summary>
    /// Decreases the number of turns by 1.
    /// If the remaining turns to arrival gets to 0 or lower, returns the unit.
    /// A unit might get to 0 or lower if they are not able be put into the destination
    /// because it's occupied by a different civ.
    /// If there are remaining turns, return null.
    /// </summary>
    public Unit Progress()
    {
        turnsToArrival--;

        if (turnsToArrival <= 0)
        {
            return unit;
        }

        return null;
    }
}
