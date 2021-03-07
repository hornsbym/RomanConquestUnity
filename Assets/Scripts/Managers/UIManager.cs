using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour 
{
    public static UIManager instance {get; set;}
    
    // Shows name of city or road
    public GameObject selectedCityLabel;

    // Maintains a reference between the
    // friendly unit UI elements and their Unit objects
    private Dictionary<GameObject, Unit> friendlyUnitInstances;

    // Fields for purchasing troops
    private GameObject purchasePanelInstance;
    public GameObject purchasePanelPrefab;
    public GameObject purchasePanelAnchor;

    // Fields for displaying existing units
    private GameObject friendlyUnitsPanelInstance;
    public GameObject friendlyUnitsPanelPrefab;

    // For displaying a single friendly unit
    public GameObject friendlyUnitPanelAnchor;

    // For showing unit details (both friendly and not)
    public GameObject unitDetailsWidgetPrefab;

    // For displaying friendly unit details
    public GameObject friendlyUnitDetailsWidgetAnchor;
    private GameObject friendlyUnitDetailsInstance;

    void Start()
    {
        instance = this;
        friendlyUnitInstances = new Dictionary<GameObject, Unit>();
    }

    void Update() 
    {
        DisplaySelectionInformation();
    }

    /// <summary>
    /// Changes the UI to fit the needs of the selected city.
    /// If there isn't a selected city, displays nothing at the UI level.
    /// </summary>
    public void DisplaySelectionInformation() 
    {
        Place selection = GameManager.instance.placeSelection;
        
        if (selection != null ) {
            if (selection is Road) {
                BuildRoadUI((Road) selection);
            } else if (selection is City) {
                BuildCityUI((City) selection);
            }
        } else {
            selectedCityLabel.SetActive(false);
        }
    }

    /// <summary>
    /// Adds road-specific components to the UI.
    /// </summary>
    private void BuildRoadUI(Road selection)
    {
        selectedCityLabel.GetComponent<Text>().text = selection.placeName;
        selectedCityLabel.SetActive(true);
    }

    /// <summary>
    /// Adds city-specific components to the UI.
    /// </summary>
    private void BuildCityUI(City selection)
    {
        selectedCityLabel.GetComponent<Text>().text = selection.placeName;
        selectedCityLabel.SetActive(true);
    }

    /// <summary>
    /// Creates and tracks the friendly troop cards that need to be
    /// displayed whenever a city is selected.
    /// Should be called by the MapManager whenever a new City is selected.
    /// </summary>
    public void CreateFriendlyUnitsSection(City selectedCity) 
    {
        friendlyUnitsPanelInstance = Instantiate(friendlyUnitsPanelPrefab, 
            friendlyUnitPanelAnchor.transform.position, Quaternion.identity);
        friendlyUnitsPanelInstance.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
    }

    /// <summary>
    /// Creates a friendly unit detials widget.
    /// </summary>
    public void CreateFriendlyUnitDetailsWidget()
    {
        friendlyUnitDetailsInstance = Instantiate(unitDetailsWidgetPrefab, 
            friendlyUnitDetailsWidgetAnchor.transform.position, Quaternion.identity);
        friendlyUnitDetailsInstance.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
    }

    /// <summary>
    /// Destroys the friendly unity details section and 
    /// removes the reference to it.
    /// </summary>
    public void DestroyFriendlyUnitDetailsWidget() 
    {
        Destroy(friendlyUnitDetailsInstance);
        friendlyUnitDetailsInstance = null;
    }

    /// <summary>
    /// Removes the friendly units panel from the UI.
    /// </summary>
    public void DestroyFriendlyUnitsPanel() 
    {
        Destroy(friendlyUnitsPanelInstance);
        friendlyUnitsPanelInstance = null;
    }

    /// <summary>
    /// Creates the purchase buttons per selected city (based on which troop
    /// types that city can provide).
    /// Adds onclick functionality to the purchase buttons.
    /// </summary>
    public void CreatePurchaseButtonsSection(City selectedCity)
    {
        purchasePanelInstance = Instantiate(purchasePanelPrefab,
            purchasePanelAnchor.transform.position, Quaternion.identity);
        purchasePanelInstance.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
    }

    /// <summary>
    /// Removes all purchase button listeners and destroys the button
    /// game objects.
    /// </summary>
    public void DestroyPurchaseButtons()
    {
        Destroy(purchasePanelInstance);
        purchasePanelInstance = null;
    }
}