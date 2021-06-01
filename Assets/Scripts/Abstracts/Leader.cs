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

    /// <summary>
    /// If a building can be built in a particular city and a 
    /// leader has enough money to buy it, perform the purchase.
    /// </summary>
    public void PurchaseBuilding(Building building, City city)
    {
        if ((city.CanBuild(building)) && (gold > building.cost))
        {
            city.AddBuilding(building);
            this.gold -= building.cost;
        }
    }
}
