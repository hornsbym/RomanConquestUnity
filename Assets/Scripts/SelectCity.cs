using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCity : MonoBehaviour
{
    void OnMouseDown() {
        GameManager.instance.SelectCity(MapManager.instance.GetCityFromDict(gameObject));
    }
}