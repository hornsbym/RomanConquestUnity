using UnityEngine.UI;
using UnityEngine;

public class CityActionsPanel : MonoBehaviour
{
    public Button combineButton;
    public Button moveUnitsButton;

    void Start() 
    {
        this.combineButton.onClick.AddListener(() => {
            EventManager.instance.fireCombineSelectedEvent(GameManager.instance.currentCity);
        });

        this.moveUnitsButton.onClick.AddListener(() =>
        {
            EventManager.instance.fireMoveSelectedEvent(GameManager.instance.currentCity);
        });
    }
    
}
