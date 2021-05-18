/// <summary>
/// Base class for Emperors and Governors
///</summary>
public abstract class Leader
{
    public Allegiance allegiance { get; set; }
    public int gold { get; set; }
    public string leaderName { get; set; }

    public Brain brain { get; protected set; }

    /// <summary>
    /// Performs the leader's game actions.
    /// </summary>
    public void TakeTurn()
    {
        this.brain.TakeTurn();
    }
}
