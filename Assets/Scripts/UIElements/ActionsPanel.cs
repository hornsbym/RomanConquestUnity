using UnityEngine.UI;
using UnityEngine;

public class ActionsPanel : MonoBehaviour
{
    [SerializeField] private Button combineButton;
    [SerializeField] private Button moveUnitsButton;
    [SerializeField] private Button attackButton;
    void Start() 
    {
        this.combineButton.onClick.AddListener(() => {
            EventManager.instance.fireCombineSelectedEvent(GameManager.instance.currentCity);
        });

        this.moveUnitsButton.onClick.AddListener(() =>
        {
            EventManager.instance.fireMoveSelectedEvent(GameManager.instance.currentCity);
        });

        this.attackButton.onClick.AddListener(() => {
            EventManager.instance.fireAttackScreenSelectedEvent(GameManager.instance.currentCity);
        });
    }
    
}
