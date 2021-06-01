using UnityEngine;

public class Legion: CombinedUnit<Cohort>
{
    public override void Disband()
    {
        EventManager.instance.fireLegionDisbandedEvent(this);
    }
}