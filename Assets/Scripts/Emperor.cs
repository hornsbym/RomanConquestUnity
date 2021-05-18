/// <summary>
/// Represents the leader of a civilization.
/// Tracks data relevant to managing the civilization, like the amount of gold.
/// The player will be one of these (or, at least it will contain a reference to one).
/// </summary>
public class Emperor : Leader
{
    public Emperor(string leaderName, Allegiance allegiance) {
        this.allegiance = allegiance;
        this.leaderName = leaderName;
        this.gold = 100;

        this.brain = new EmperorBrain(this);
    }

    /// <summary>
    /// Checks whether or not the Emperor has enough gold to purchase a given troop.
    /// If the Emperor can purchase the troops, remove the gold and generate the troops.
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
