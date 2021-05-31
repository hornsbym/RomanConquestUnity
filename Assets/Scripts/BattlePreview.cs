using System.Collections.Generic;

public struct BattlePreview
{
    public List<Unit> remainingAttackers { get; private set; }
    public List<Unit> remainingDefenders { get; private set; }

    public BattlePreview (List<Unit> attackers, List<Unit> defenders) {
        this.remainingAttackers = attackers;
        this.remainingDefenders = defenders;
    }
}
