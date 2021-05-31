using UnityEngine;

public class Cohort: CombinedUnit<Century> 
{
    public override void Disband()
    {
        EventManager.instance.fireCohortDisbandedEvent(this);
    }
}