using UnityEngine;
using UnityEngine.UI;

public class NextTurnButton : MonoBehaviour
{
    public Button nextTurnButton;

    // Start is called before the first frame update
    void Start()
    {
        nextTurnButton.onClick.AddListener(GoToNextTurn);
    }

    private void GoToNextTurn() {
        EventManager.instance.fireTurnEndEvent();
    }
}
