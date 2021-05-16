using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class TroopPurchaseRow : MonoBehaviour
{
    public Button purchaseButton;

    TroopClassification classification;

    /// <summary>
    /// Adds text to the button.
    /// Adds on-click functionality to the purchase button.
    /// </summary>
    public void Initialize(TroopClassification spawnClass, City city)
    {
        classification = spawnClass;
        CreateButtonFromSpawnClass(city);
    }

    private void CreateButtonFromSpawnClass(City city)
    {
        purchaseButton.GetComponentInChildren<Text>().text = Utilities.instance.UppercaseFirstLetter(classification.ToString());

        // Deactivate purchase button if the player doesn't have enough gold to buy the troop 
        // or if the city has already performed an action
        if (!city.hasAction) {
            purchaseButton.interactable = false;
        } else if (GameManager.instance.GetPlayerLeader().gold < TroopStats.statLedger[classification][Stat.COST]) {
            purchaseButton.interactable = false;
        } else {
            purchaseButton.onClick.AddListener(() =>
            {
                GameManager.instance.allegianceToLeaderMapping[GameManager.instance.playerAllegiance].PurchaseTroop(classification, city);
            });

            purchaseButton.interactable = true;
        }

    }
}
