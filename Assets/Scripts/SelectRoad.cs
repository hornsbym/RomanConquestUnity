using UnityEngine;
using UnityEngine.EventSystems;

public class SelectRoad : MonoBehaviour
{
    void OnMouseDown()
    {
        GameManager.instance.SelectRoad(gameObject.GetComponent<Road>());
    }
}
