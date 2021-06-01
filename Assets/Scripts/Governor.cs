/// <summary>
/// Similar to an Emperor, but more limited in things they can do.
/// Each city will be able to have their own AI strategy.
/// </summary>
public class Governor : Leader
{
    private AIStrategy strategy { get; set; }
    public City city { get; private set; }

    public Governor(string leaderName, City city, AIStrategy strategy)
    {
        this.allegiance = Allegiance.INDEPENDENT;
        this.leaderName = leaderName;
        this.strategy = strategy;
        this.city = city;
        this.gold = 0;

        this.brain = new GovernorBrain(this);
    }

    /// <summary>
    /// Checks whether or not the Governor has enough gold to purchase a given troop.
    /// If the Governer can purchase the troops, remove the gold and generate the troops.
    /// </summary>
    public void PurchaseTroop(TroopClassification troopClass)
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
