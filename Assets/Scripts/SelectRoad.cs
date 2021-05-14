using UnityEngine;
using UnityEngine.EventSystems;

public class SelectRoad : MonoBehaviour
{
    void OnMouseDown()
    {
        EventManager.instance.fireSelectedRoadUpdatedEvent(gameObject.GetComponent<Road>());
    }
}
