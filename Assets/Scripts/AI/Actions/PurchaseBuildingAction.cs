public class PurchaseBuildingAction : Action
{
    private City city { get; set; }
    private Leader leader { get; set; }
    private Building building { get; set; }


    public PurchaseBuildingAction(City city, Leader leader, Building building) {
        this.city = city;
        this.leader = leader;
        this.building = building;
    }

    override public void Execute() 
    {
        int price = building.cost;

        if ((leader.gold >= price && city.hasAction))
        {
            leader.PurchaseBuilding(building, city);
        }
    }

    override public bool IsPossible()
    {
        // TODO: Rework this, it's ugly. We need to separate Governor vs Emperor scenarios.
        if (city.hasAction == false)
        {
            return false;
        }
        else if (city.allegiance != leader.allegiance && city.allegiance != Allegiance.NONE)
        {
            return false;
        }
        else if (leader.gold < building.cost)
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
