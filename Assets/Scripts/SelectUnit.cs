using UnityEngine;
using UnityEngine.EventSystems;

public class SelectUnit : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData data)
    {        
        UnitListing unitListing = gameObject.GetComponent<UnitListing>();
        GameManager.instance.SelectUnit(unitListing.unit);
    }
}
