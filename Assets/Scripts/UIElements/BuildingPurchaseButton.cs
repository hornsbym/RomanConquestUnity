using UnityEngine.UI;
using UnityEngine;

public class BuildingPurchaseButton : MonoBehaviour
{
    [SerializeField] private Button purchaseButton;

    private Building building;

    /// <summary>
    /// Adds text to the button.
    /// Adds on-click functionality to the purchase button.
    /// </summary>
    public void Initialize(Building building, City city)
    {
        this.building = building;
        CreateButtonFromSpawnClass(city);
    }

    private void CreateButtonFromSpawnClass(City city)
    {
        purchaseButton.GetComponentInChildren<Text>().text = building.buildingName;

        // Deactivate purchase button if the player doesn't have enough gold to buy the troop 
        // or if the city has already performed an action
        if (GameManager.instance.GetPlayerLeader().gold < building.cost) {
            purchaseButton.interactable = false;
        } else {
            purchaseButton.onClick.AddListener(() =>
            {
                GameManager.instance.GetPlayerLeader().PurchaseBuilding(building, city);
            });

            purchaseButton.interactable = city.hasAction;
        }

    }
}
