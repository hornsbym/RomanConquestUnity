using UnityEngine;

public class SelectCity : MonoBehaviour
{
    void OnMouseDown() {
        EventManager.instance.fireSelectedCityUpdatedEvent(gameObject.GetComponent<City>());
    }
}
