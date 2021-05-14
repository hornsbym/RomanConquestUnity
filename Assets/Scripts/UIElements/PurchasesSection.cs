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

        for (int i = 0; i < selectedCity.unitsForSale.Length; i++)
        {
            GameObject row = Instantiate(troopPurchaseRowPrefab, purchaseButtonRowContent.GetComponent<RectTransform>(), false);
            row.transform.SetParent(purchaseButtonRowContent.GetComponent<RectTransform>());

            row.GetComponent<TroopPurchaseRow>().Initialize(selectedCity.unitsForSale[i]);
            existingPurchaseRows.Add(row);
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
