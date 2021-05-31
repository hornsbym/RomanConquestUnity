using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// General purpose class for displaying a list of game Units.
/// </summary> 
public class TravellingUnitsScrollview : MonoBehaviour
{   
    [SerializeField] private Text titleText;
    [SerializeField] private GameObject content;
    [SerializeField] private TravellingUnitTile unitTile;

    public delegate void TileCallback (Unit unit);
    private List<TravellingUnitTile> tUnitTiles;

    void Awake()
    {
        tUnitTiles = new List<TravellingUnitTile>();
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
    public void SetContent(List<TravellingUnit> tUnits, TileCallback callback = null)
    {
        /// Removes any existing content
        foreach(TravellingUnitTile tile in tUnitTiles) {
            Destroy(tile.gameObject);
        }
        tUnitTiles.Clear();

        /// Defaults the callback to an empty one if no
        /// callback is provided.
        TileCallback unitCallback;
        if (callback == null) {
            unitCallback = (Unit u) => {};
        } else {
            unitCallback = callback;
        }

        /// Create and add a new label for each unit.
        foreach (TravellingUnit tUnit in tUnits)
        {
            TravellingUnitTile tUnitTile = Instantiate(this.unitTile, content.GetComponent<RectTransform>(), false);
            tUnitTiles.Add(tUnitTile);
            tUnitTile.Initialize(tUnit);

            tUnitTile.transform.SetParent(content.GetComponent<RectTransform>());
        }
    }
}