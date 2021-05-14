using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveTroopsUI : MonoBehaviour
{
    public UnitsScrollview unitsScrollviewPrefab;
    public GameObject unitsScrollviewLayout;

    public Button confirmButton;
    public Button cancelButton;

    private List<ScrollviewCityPair> scrollviewCityPairs;

    private City currentCity;

    void Awake()
    {
        scrollviewCityPairs = new List<ScrollviewCityPair>();
        EventManager.OnSelectedCityUpdated += AddNeighboringCityScrollviews;
        EventManager.OnUnitsChanged += AddNeighboringCityScrollviews;
        EventManager.OnMoveUnitsSelected += setSelectedCity;

        confirmButton.onClick.AddListener(ConfirmSelection);
        cancelButton.onClick.AddListener(CancelSelection);
    }

    private void setSelectedCity(City city)
    {
        this.currentCity = city;
    }

    /// <summary>
    /// Abstract concept that pairs a scrollview with a city.
    /// This will make it easier to move the units when the selection is finalized.
    /// </summary>
    private class ScrollviewCityPair 
    {
        public City city;
        public UnitsScrollview scrollview;

        public ScrollviewCityPair(City city, UnitsScrollview scrollview) {
            this.city = city;
            this.scrollview = scrollview;
        }
    }

    /// <summary>
    /// Creates one scrollview for the selected city and one for each of the selected 
    /// city's neighbors. Players will then be able to assign the units from the selected city
    /// to a neighboring city.
    /// </summary>
    void AddNeighboringCityScrollviews(City selectedCity) 
    {
        DestroyAllScrollviews();

        // City selectedCity = (City) GameManager.instance.placeSelection;

        List<City> neighboringCities = selectedCity.GetNeighbors();

        UnitsScrollview selectedCitySv = UnitsScrollview.Instantiate<UnitsScrollview>(unitsScrollviewPrefab);
        selectedCitySv.AddTitle(selectedCity.placeName);
        selectedCitySv.transform.SetParent(unitsScrollviewLayout.transform);

        // Create a new list here to prevent overriding values
        selectedCitySv.SetContent(new List<Unit>(selectedCity.occupyingUnits), CycleUnit);

        scrollviewCityPairs.Add(new ScrollviewCityPair(selectedCity, selectedCitySv));

        foreach (City neighboringCity in neighboringCities) {
            UnitsScrollview unitsScrollviewCopy = UnitsScrollview.Instantiate<UnitsScrollview>(unitsScrollviewPrefab);
            unitsScrollviewCopy.AddTitle("To " + neighboringCity.placeName);
            unitsScrollviewCopy.transform.SetParent(unitsScrollviewLayout.transform);
            unitsScrollviewCopy.SetContent(new List<Unit>(), CycleUnit);
            scrollviewCityPairs.Add(new ScrollviewCityPair(neighboringCity, unitsScrollviewCopy));
        }
    }

    void DestroyAllScrollviews() 
    {
        foreach (ScrollviewCityPair pair in scrollviewCityPairs) {
            Destroy(pair.scrollview.gameObject);
        }

        scrollviewCityPairs.Clear();
    }

    /// <summary>
    /// Moves a unit from one scrollview to the next.
    /// </summary>
    private void CycleUnit(Unit unit) 
    {
        for (int i = 0; i < scrollviewCityPairs.Count; i++) {
            UnitsScrollview currentScrollview = scrollviewCityPairs[i].scrollview;
            UnitsScrollview nextScrollview = null;

            // Wraps to front of list if we're dealing with the last scrollview

            if (i + 1 == scrollviewCityPairs.Count) {
                nextScrollview = scrollviewCityPairs[0].scrollview;
            } else {
                nextScrollview = scrollviewCityPairs[i+1].scrollview;
            }

            if (currentScrollview.GetUnits().Contains(unit)) {
                // Remove the unit from the current scrollview
                List<Unit> currentSvUnits = new List<Unit>(currentScrollview.GetUnits());
                currentSvUnits.Remove(unit);
                currentScrollview.SetContent(currentSvUnits, CycleUnit);

                // Add the unit to the next scrollview
                List<Unit> nextSvUnits = new List<Unit>(nextScrollview.GetUnits());
                nextSvUnits.Add(unit);
                nextScrollview.SetContent(nextSvUnits, CycleUnit);

                break;
            }
        }
    }

    void ConfirmSelection() 
    {
        foreach(ScrollviewCityPair pair in scrollviewCityPairs) {
            if (pair.city != currentCity) {
                currentCity.SendFriendlyUnitsToNeighbor(pair.scrollview.GetUnits(), pair.city);
            }
        }
        EventManager.instance.fireSelectedCityUpdatedEvent(currentCity);
    }

    void CancelSelection()
    {
        EventManager.instance.fireSelectedCityUpdatedEvent(currentCity);
    }
}
