using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerDownHandler
{
    public delegate void Callback();
    private Callback callback;

    public void OnPointerDown(PointerEventData data)
    {        
        callback();
    }

    public void SetCallbackDelegate(Callback callback)
    {
        this.callback = callback;
    }   
}
