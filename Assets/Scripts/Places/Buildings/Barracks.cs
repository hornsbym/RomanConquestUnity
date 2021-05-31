public class Barracks : Building
{
    public Barracks()
    {
        this.cost = BuildingCost.BARRACKS;
        this.buildingName = "Barracks";
    }
    public override void ApplyEffect(City city)
    {
        city.unitsForSale.Add(TroopClassification.INFANTRY);
    }

    public override void RemoveEffect(City city)
    {
        city.unitsForSale.Remove(TroopClassification.INFANTRY);
    }
}
