public class Stables : Building
{
    public Stables() { 
        this.cost = BuildingCost.STABLES;
        this.buildingName = "Stables";
    }

    public override void ApplyEffect(City city)
    {
        city.unitsForSale.Add(TroopClassification.CAVALRY);
    }

    public override void RemoveEffect(City city)
    {
        city.unitsForSale.Remove(TroopClassification.CAVALRY);
    } 
}

