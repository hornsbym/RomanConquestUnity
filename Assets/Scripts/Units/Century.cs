public class Century : CombinedUnit<Troop>
{
    public override void Disband()
    {
        EventManager.instance.fireCenturyDisbandedEvent(this);
    }
}