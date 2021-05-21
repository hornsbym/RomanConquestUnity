public class Range : Building
{
    public Range()
    {
        this.cost = BuildingCost.RANGE;
        this.buildingName = "Range";
    }
    public override void ApplyEffect(City city)
    {
        city.unitsForSale.Add(TroopClassification.RANGED);
    }

    public override void RemoveEffect(City city)
    {
        city.unitsForSale.Remove(TroopClassification.RANGED);
    }
}
