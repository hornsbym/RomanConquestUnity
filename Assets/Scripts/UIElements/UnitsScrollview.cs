using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// General purpose class for displaying a list of game Units.
/// </summary> 
public class UnitsScrollview : MonoBehaviour
{   
    public Text titleText;
    public GameObject content;
    public GameObject unitTile;

    public delegate void TileCallback(Unit unit);
    private List<GameObject> unitTiles;
    private List<Unit> units;

    void Awake()
    {
        unitTiles = new List<GameObject>();
        units = new List<Unit>();
    }

    /// <summary>
    /// Sets the text that should appear above the scrollview.
    /// </summary>
    public void AddTitle(string title)
    {
        this.titleText.text = title;
    }
    
    /// <summary>
    /// Accepts a list of units to add to the scrollview.
    /// For each unit, creates a unit tile to represent the unit.
    /// Accepts an optional function parameter that can be attached to each 
    /// tile so that the tiles can be made clickable.
    /// </summary>
    public void SetContent(List<Unit> units, TileCallback callback = null)
    {
        /// Removes any existing content
        foreach(GameObject tile in unitTiles) 
        {
            Destroy(tile);
        }
        unitTiles.Clear();

        this.units = units;

        /// Defaults the callback to an empty one if no
        /// callback is provided.
        TileCallback unitCallback;
        if (callback == null) {
            unitCallback = (Unit u) => {};
        } else {
            unitCallback = callback;
        }

        /// Create and add a new label for each unit.
        foreach (Unit unit in units)
        {
            GameObject unitTile = Instantiate(this.unitTile, content.GetComponent<RectTransform>(), false);
            unitTiles.Add(unitTile);
            unitTile.GetComponentInChildren<Text>().text = unit.unitName;
            unitTile.GetComponent<Clickable>().SetCallbackDelegate(() => {unitCallback(unit);});
            UnitListing listing = unitTile.AddComponent<UnitListing>();
            listing.unit = unit;
            unitTile.transform.SetParent(content.GetComponent<RectTransform>());
        }
    }

    public List<Unit> GetUnits() 
    {
        return new List<Unit>(this.units);
    }
}