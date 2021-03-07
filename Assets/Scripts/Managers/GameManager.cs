using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; set;}
    public Place placeSelection {get; set;}
    public Unit unitSelection {get; set;}

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        placeSelection = null;
        unitSelection = null;
    }

    /// <summary>
    /// Select a Unit.
    /// </summary>
    public void SelectUnit(Unit unit) {
        print($"Selected unit: {unit}");
        unitSelection = unit;
        UIManager.instance.DestroyFriendlyUnitDetailsWidget();
        UIManager.instance.CreateFriendlyUnitDetailsWidget();
    }

    /// <summary>
    /// Sets the selection to a city.
    /// </summary>
    public void SelectCity(City city) 
    {
        DeselectAll();
        placeSelection = city;
        UIManager.instance.CreatePurchaseButtonsSection(city);
        UIManager.instance.CreateFriendlyUnitsSection(city);
    }

    /// <summary>
    /// Sets the selection to a road.
    /// </summary>
    public void SelectRoad(Road road) 
    {
        DeselectAll();
        placeSelection = road;
    }

    /// <summary>
    /// Sets the selected city to null.
    /// </summary>
    public void DeselectAll()
    {
        placeSelection = null;
        unitSelection = null;
        UIManager.instance.DestroyPurchaseButtons();
        UIManager.instance.DestroyFriendlyUnitsPanel();
        UIManager.instance.DestroyFriendlyUnitDetailsWidget();
    }
}
