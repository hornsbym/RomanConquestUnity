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
        this.gold = 10;
    }

    /// <summary>
    /// Checks whether or not the leader has enough gold to purchase a given troop.
    /// If the leader can purchase the troops, remove the gold and generate the troops.
    /// </summary>
    public void PurchaseTroop(TroopClassification troopClass, City city)
    {
        int price = TroopStats.statLedger[troopClass][Stat.COST];

        if ((gold >= price && city.hasAction)) 
        {
            city.AddOccupyingUnits(UnitFactory.instance.GenerateTroop(troopClass, allegiance));
            city.UseAction();
            gold -= price;
        }
    }
}
