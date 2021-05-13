using UnityEngine;

/// <summary>
/// Represents the leader of a civilization.
/// Tracks data relevant to managing the civilization, like the amount of gold.
/// The player will be one of these (or, at least it will contain a reference to one).
/// </summary>
public class Leader
{
    public Allegiance allegiance { get; set; }
    public int gold { get; set; }
    public string leaderName { get; set; }

    public Leader(string leaderName, Allegiance allegiance) {
        this.allegiance = allegiance;
        this.leaderName = leaderName;
    }
}
