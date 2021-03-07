using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FriendlyUnitsSection : MonoBehaviour
{
    private City selectedCity;
    public GameObject friendlyUnitPrefab;
    public GameObject scrollviewContent;
    private List<GameObject> friendlyUnitInstances;

    void Awake()
    {
        friendlyUnitInstances = new List<GameObject>();
    }

    void Start() {
        selectedCity = (City)GameManager.instance.placeSelection;

        SetupFriendlyUnitInstances();
    }

    /// <summary>
    /// Creates and tracks the friendly troop cards that need to be
    /// displayed whenever a city is selected.
    /// Should be called by the MapManager whenever a new City is selected.
    /// </summary>
    public void SetupFriendlyUnitInstances()
    {
        /// Get the friendly units at in the city.
        List<Unit> friendlyUnits = selectedCity.friendlyUnits;

        /// Locate where to display the friendly unit prefabs.
        RectTransform contentRectTransform = scrollviewContent.GetComponent<RectTransform>();

        /// Create and add a new label for each unit.
        foreach (Unit unit in friendlyUnits)
        {
            GameObject friendlyUnitCard = Instantiate(friendlyUnitPrefab, contentRectTransform, false);
            friendlyUnitCard.GetComponentInChildren<Text>().text = unit.unitName;
            UnitListing listing = friendlyUnitCard.AddComponent<UnitListing>();
            listing.unit = unit;
            friendlyUnitCard.transform.SetParent(contentRectTransform);
        }
    }
}
