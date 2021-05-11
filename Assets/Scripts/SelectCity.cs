using UnityEngine;

public class SelectCity : MonoBehaviour
{
    void OnMouseDown() {
        EventManager.instance.fireSelectCityEvent(gameObject.GetComponent<City>());
    }
}
