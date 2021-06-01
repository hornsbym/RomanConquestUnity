using UnityEngine;
using UnityEngine.UI;

public class DefaultUI : MonoBehaviour
{
    public Text turnCountText;
    public Text goldCountText;
    public Button nextTurnButton;

    void Awake() 
    {
        EventManager.OnTurnBegin += SetTurnCountText;
        EventManager.OnTurnBegin += SetGoldCountText;
        EventManager.OnDefaultSelected += SetGoldCountText;

        nextTurnButton.onClick.AddListener(EventManager.instance.fireTurnEndEvent);
    }

    void Start()
    {
        SetGoldCountText();
        SetTurnCountText();
    }

    private void SetTurnCountText() 
    {
        turnCountText.text = "Turn " + GameManager.instance.turnCount;
    }

    private void SetGoldCountText()
    {
        goldCountText.text = GameManager.instance.GetPlayerLeader().gold.ToString() + " Gold";
    }
}
