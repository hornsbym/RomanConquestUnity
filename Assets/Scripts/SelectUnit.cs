using UnityEngine;
using UnityEngine.EventSystems;

public class SelectUnit : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData data)
    {
        Unit unit = gameObject.GetComponent<Unit>();
        GameManager.instance.SelectUnit(unit);
    }
}
