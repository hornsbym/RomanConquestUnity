using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; set;}
    public City selectedCity {get; set;}

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        selectedCity = null;
    }

    /// <summary>
    /// Sets the selected city.
    /// </summary>
    public void SelectCity(City city) {
        DeselectCity();
        selectedCity = city;
        UIManager.instance.SetupPurchaseButtons();
    }

    /// <summary>
    /// Sets the selected city to null.
    /// </summary>
    public void DeselectCity(){
        selectedCity = null;
        UIManager.instance.DestroyPurchaseButtons();
    }
}
