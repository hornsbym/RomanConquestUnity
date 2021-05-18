public class PurchaseAction : Action
{
    private City city;
    private Leader leader;
    private TroopClassification troopClass;


    public PurchaseAction(City city, Leader leader, TroopClassification troopClassification) {
        this.city = city;
        this.leader = leader;
        this.troopClass = troopClassification;
    }

    override public void Execute() 
    {
        int price = TroopStats.statLedger[troopClass][Stat.COST];

        if ((leader.gold >= price && city.hasAction))
        {
            city.AddOccupyingUnits(UnitFactory.instance.GenerateTroop(troopClass, leader.allegiance));
            city.UseAction();
            leader.gold -= price;
        }
    }

    override public bool IsPossible()
    {
        // TODO: Rework this, it's ugly. We need to separate Governor vs Emperor scenarios.
            if (city.hasAction == false)
            {
                return false;
            }
            else if (city.allegiance != leader.allegiance)
            {
                return false;
            }
            else if (!city.unitsForSale.Contains(troopClass))
            {
                return false;
            }
            else if (leader.gold < TroopStats.GetTroopStat(troopClass, Stat.COST))
            {
                return false;
            }
            else
            {
                return true;
            }

    }

    override public void Evaluate()
    {
        value = 5;
    }
}
