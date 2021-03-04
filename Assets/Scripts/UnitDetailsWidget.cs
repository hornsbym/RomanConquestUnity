using UnityEngine.UI;
using UnityEngine;

public class UnitDetailsWidget : MonoBehaviour
{       
    // The unit being represented
    private Unit unit;


    public Text nameLabel;
    public Text levelLabel;
    public Text expLabel;
    public Text meleeLabel;
    public Text defenseLabel;
    public Text rangedLabel;
    public Text movementLabel;

    void Start()
    {  
        this.unit = GameManager.instance.unitSelection;
        FillInLabels();
    }

    private void FillInLabels() 
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