using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class PurchasesSection : MonoBehaviour
{
    public GameObject purchaseButtonRowContent;
    public GameObject troopPurchaseRowPrefab;

    private List<GameObject> existingPurchaseRows;

    void Awake() {
        EventManager.OnSelectedCityUpdated += CreatePurchaseRows;
        existingPurchaseRows = new List<GameObject>();
    }

    /// <summary>
    /// Creates the buttons and adds them to the UI widget.
    /// </summary>
    private void CreatePurchaseRows(City selectedCity) 
    {
        DeletePurchaseRows();

        if (selectedCity.allegiance == GameManager.instance.playerAllegiance) {
            foreach (TroopClassification troopClass in selectedCity.unitsForSale)
            {
                GameObject row = Instantiate(troopPurchaseRowPrefab, purchaseButtonRowContent.GetComponent<RectTransform>(), false);
                row.transform.SetParent(purchaseButtonRowContent.GetComponent<RectTransform>());

                row.GetComponent<TroopPurchaseRow>().Initialize(troopClass, selectedCity);
                existingPurchaseRows.Add(row);
            }
        }
    }

    /// <summary>
    /// Clears out the purchase panel.
    /// </summary>
    private void DeletePurchaseRows()
    {
        foreach (GameObject row in existingPurchaseRows) {
            Destroy(row);
        }

        existingPurchaseRows.Clear();
    }
}
