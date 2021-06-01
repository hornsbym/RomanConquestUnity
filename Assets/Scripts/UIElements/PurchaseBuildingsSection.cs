using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class PurchaseBuildingsSection : MonoBehaviour
{
    public GameObject purchaseButtonRowContent;
    public GameObject buildingPurchaseButtonPrefab;

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
            foreach (Building building in selectedCity.buildingsForPurchase)
            {
                GameObject btn = Instantiate(buildingPurchaseButtonPrefab, purchaseButtonRowContent.GetComponent<RectTransform>(), false);
                btn.transform.SetParent(purchaseButtonRowContent.GetComponent<RectTransform>());

                btn.GetComponent<BuildingPurchaseButton>().Initialize(building, selectedCity);
                existingPurchaseRows.Add(btn);
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
