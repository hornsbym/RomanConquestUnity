using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UnitDetailsWidget : MonoBehaviour
{       
    public Text nameLabel;
    public Text levelLabel;
    public Text expLabel;
    public Text meleeLabel;
    public Text defenseLabel;
    public Text rangedLabel;
    public Text movementLabel;

    /// <summary>
    /// Adds a unit's stats to the appropriate labels.
    /// </summary>
    public void FillInLabels(Unit unit) 
    {
        nameLabel.text = unit.unitName;
        levelLabel.text = $"Level {unit.level}";
        expLabel.text = $"{unit.xp} / {unit.xpToNextLevel} Exp";
        meleeLabel.text = unit.melee.ToString();
        rangedLabel.text = unit.ranged.ToString();
        defenseLabel.text = unit.defense.ToString();
        movementLabel.text = unit.movement.ToString();
    }

}