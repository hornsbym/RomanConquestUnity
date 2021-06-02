using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AttackUI : MonoBehaviour
{
    [SerializeField] private UnitsScrollview unitsScrollviewPrefab;
    [SerializeField] private HorizontalLayoutGroup unitsScrollviewLayout;

    [SerializeField] private Button rangedButton;
    [SerializeField] private Button fullBattleButton;
    [SerializeField] private Button cancelButton;

    private List<AttackFromCitySvPair> cityScrollviewPairs { get; set; }

    /// Used for attacks from roads only
    private RoadScrollviewPair city1RoadScrollviewPair { get; set; }
    private RoadScrollviewPair city2RoadScrollviewPair { get; set; }

    private Road selectedRoad { get; set; }
    private City selectedCity { get; set; }

    void Awake()
    {
        cityScrollviewPairs = new List<AttackFromCitySvPair>();
        EventManager.OnSelectedCityUpdated += CreateAttackFromCityScrollviews;
        EventManager.OnSelectedRoadUpdatedEvent += CreateAttackFromRoadScrollviews;
        EventManager.OnSelectedCityUpdated += setSelectedCity;
        EventManager.OnSelectedRoadUpdatedEvent += setSelectedRoad;

        fullBattleButton.onClick.AddListener(ExecuteBattle);
        rangedButton.onClick.AddListener(ExecuteSkirmish);
        cancelButton.onClick.AddListener(CancelSelection);
    }

    private void setSelectedCity(City city)
    {
        this.selectedRoad = null;
        this.selectedCity = city;
    }

    private void setSelectedRoad(Road road)
    {
        this.selectedCity = null;
        this.selectedRoad = road;
    }

    /// <summary>
    /// Used for attacking a city from a road.
    /// </summary>
    private class RoadScrollviewPair {
        public UnitsScrollview attackingUnitsScrollview { get; private set; }
        public UnitsScrollview campScrollview { get; private set; }
        public City cityBeingAttacked { get; private set; }

        public delegate void UnitInputFunction(Unit unit);

        public RoadScrollviewPair(UnitsScrollview attackingUnitsScrollview, UnitsScrollview campScrollview, City cityBeingAttacked) 
        {
            this.attackingUnitsScrollview = attackingUnitsScrollview;
            this.campScrollview = campScrollview;
            this.cityBeingAttacked = cityBeingAttacked;
        }

        // Returns true if some scrollview in this pair contains the provided unit
        public bool HasUnit(Unit u) {
            return attackingUnitsScrollview.GetUnits().Contains(u) || campScrollview.GetUnits().Contains(u);
        }

        public void MoveUnit(Unit u, UnitsScrollview.TileCallback func) {
            if (attackingUnitsScrollview.GetUnits().Contains(u)) {
                List<Unit> newAttackingUnits = attackingUnitsScrollview.GetUnits();
                List<Unit> newCampedUnits = campScrollview.GetUnits();

                newAttackingUnits.Remove(u);
                newCampedUnits.Add(u);

                attackingUnitsScrollview.SetContent(newAttackingUnits, func);
                campScrollview.SetContent(newCampedUnits, func);
            } else {
                List<Unit> newAttackingUnits = attackingUnitsScrollview.GetUnits();
                List<Unit> newCampedUnits = campScrollview.GetUnits();

                newAttackingUnits.Add(u);
                newCampedUnits.Remove(u);

                attackingUnitsScrollview.SetContent(newAttackingUnits, func);
                campScrollview.SetContent(newCampedUnits, func);
            }
        }
    }


    /// <summary>
    /// Abstract concept that pairs a scrollview with a city.
    /// This will make it easier to move the units when the selection is finalized.
    /// </summary>
    private struct AttackFromCitySvPair 
    {
        public Place place { get; private set; }
        public UnitsScrollview scrollview { get; private set; }

        public AttackFromCitySvPair(Place place, UnitsScrollview scrollview) {
            this.place = place;
            this.scrollview = scrollview;
        }
    }

    /// <summary>
    /// Creates one scrollview for the selected city and one for each of the selected 
    /// city's neighbors. Players will then be able to assign the units from the selected city
    /// to a neighboring city.
    /// </summary>
    void CreateAttackFromCityScrollviews(City selectedCity) 
    {
        DestroyAllScrollviews();

        // Get all the roads into the city
        List<Road> connectedRoads = selectedCity.GetConnectedRoads();

        // Create a scrollview of the units in the selected city
        UnitsScrollview selectedCitySv = UnitsScrollview.Instantiate<UnitsScrollview>(unitsScrollviewPrefab);
        selectedCitySv.AddTitle(selectedCity.placeName);
        selectedCitySv.transform.SetParent(unitsScrollviewLayout.transform);

        // Populate the selected city scrollview with the current units
        selectedCitySv.SetContent(new List<Unit>(selectedCity.occupyingUnits), CycleUnit);
        cityScrollviewPairs.Add(new AttackFromCitySvPair(selectedCity, selectedCitySv));

        // Create scrollviews from the neighboring roads that have
        // enemy units camped within them
        foreach (Road road in connectedRoads) {
            UnitsScrollview unitsScrollviewCopy = UnitsScrollview.Instantiate<UnitsScrollview>(unitsScrollviewPrefab);
            unitsScrollviewCopy.AddTitle("Camp on Via " + road.placeName);
            unitsScrollviewCopy.transform.SetParent(unitsScrollviewLayout.transform);
            unitsScrollviewCopy.SetContent(new List<Unit>(), CycleUnit);
            cityScrollviewPairs.Add(new AttackFromCitySvPair(road, unitsScrollviewCopy));
        }
    }

    /// <summary>
    /// Creates two scrollviews, one for each camp on the city.
    /// Creates two other scrollviews for the cities the road connects.
    /// </summary>
    void CreateAttackFromRoadScrollviews(Road selectedRoad)
    {
        /// Clear any existing scrollviews
        DestroyAllScrollviews();

        /// Creates a scrollview for the units camped near the first city

        UnitsScrollview city1CampSv  = UnitsScrollview.Instantiate<UnitsScrollview>(unitsScrollviewPrefab);
        city1CampSv.AddTitle("Camp near " + selectedRoad.city1.placeName);
        city1CampSv.transform.SetParent(unitsScrollviewLayout.transform);

        // Fill the city 1 camp scrollview
        // Copy units to a new list to prevent overriding values
        city1CampSv.SetContent(new List<Unit>(selectedRoad.GetEncampedUnits(selectedRoad.city1)), CycleRoadUnit);
        cityScrollviewPairs.Add(new AttackFromCitySvPair(selectedRoad.city1, city1CampSv));

        // Create empty scrollviews for the first city
        // Units chosen to attack will populate the scrollview
        UnitsScrollview city1AttackingUnitsSv = UnitsScrollview.Instantiate<UnitsScrollview>(unitsScrollviewPrefab);
        city1AttackingUnitsSv.AddTitle("Attack " + selectedRoad.city1.placeName);
        city1AttackingUnitsSv.transform.SetParent(unitsScrollviewLayout.transform);
        city1AttackingUnitsSv.SetContent(new List<Unit>(), CycleRoadUnit);
        cityScrollviewPairs.Add(new AttackFromCitySvPair(selectedRoad.city1, city1AttackingUnitsSv));

        // Repeat process for the second city
        // This ensures the "camp" and "attacking" scrollviews will be next to each other
        UnitsScrollview city2CampSv = UnitsScrollview.Instantiate<UnitsScrollview>(unitsScrollviewPrefab);
        city2CampSv.AddTitle("Camp near " + selectedRoad.city2.placeName);
        city2CampSv.transform.SetParent(unitsScrollviewLayout.transform);
        city2CampSv.SetContent(new List<Unit>(selectedRoad.GetEncampedUnits(selectedRoad.city2)), CycleRoadUnit);
        cityScrollviewPairs.Add(new AttackFromCitySvPair(selectedRoad.city2, city2CampSv));

        UnitsScrollview city2AttackingUnitsSv = UnitsScrollview.Instantiate<UnitsScrollview>(unitsScrollviewPrefab);
        city2AttackingUnitsSv.AddTitle("Attack " + selectedRoad.city2.placeName);
        city2AttackingUnitsSv.transform.SetParent(unitsScrollviewLayout.transform);
        city2AttackingUnitsSv.SetContent(new List<Unit>(), CycleRoadUnit);
        cityScrollviewPairs.Add(new AttackFromCitySvPair(selectedRoad.city2, city2AttackingUnitsSv));

        // Pair the scrollviews
        city1RoadScrollviewPair = new RoadScrollviewPair(city1AttackingUnitsSv, city1CampSv, selectedRoad.city1);
        city2RoadScrollviewPair = new RoadScrollviewPair(city2AttackingUnitsSv, city2CampSv, selectedRoad.city2);
    }

    /// <summary>
    /// Moves a unit from one scrollview to the next.
    /// </summary>
    private void CycleUnit(Unit unit) 
    {
        for (int i = 0; i < cityScrollviewPairs.Count; i++) {
            UnitsScrollview currentScrollview = cityScrollviewPairs[i].scrollview;
            UnitsScrollview nextScrollview = null;

            // Wraps to front of list if we're dealing with the last scrollview

            if (i + 1 == cityScrollviewPairs.Count) {
                nextScrollview = cityScrollviewPairs[0].scrollview;
            } else {
                nextScrollview = cityScrollviewPairs[i+1].scrollview;
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

    /// <summary>
    /// Allows units to be moved between a road camp scrollview and an 
    /// "attacking city" scrollview.
    /// </summary>
    private void CycleRoadUnit(Unit unit) {

        if (city1RoadScrollviewPair.HasUnit(unit)) {
            city1RoadScrollviewPair.MoveUnit(unit, CycleRoadUnit);
        } else {
            city2RoadScrollviewPair.MoveUnit(unit, CycleRoadUnit);
        }
    }

    void DestroyAllScrollviews()
    {
        foreach (AttackFromCitySvPair pair in cityScrollviewPairs)
        {
            Destroy(pair.scrollview.gameObject);
        }

        cityScrollviewPairs.Clear();
    }

    void ExecuteBattle() 
    {
        if (this.selectedCity != null) {
            ExecuteBattleFromCity(this.selectedCity);
        } else {
            ExecuteBattleFromRoad(this.selectedRoad);
        }
    }

    void ExecuteSkirmish() 
    {
        if (this.selectedCity != null)
        {
            ExecuteSkirmishFromCity(this.selectedCity);
        }
        else
        {
            ExecuteSkirmishFromRoad(this.selectedRoad);
        }
    }

    void ExecuteBattleFromCity(City city) 
    {
        foreach (AttackFromCitySvPair pair in cityScrollviewPairs) {
            if (pair.place != city) {
                BattleManager.instance.FullAttack(city.occupyingUnits, pair.scrollview.GetUnits());
            }
        }
    }

    void ExecuteSkirmishFromCity(City city)
    {
        foreach (AttackFromCitySvPair pair in cityScrollviewPairs)
        {
            if (pair.place != city)
            {
                BattleManager.instance.RangedOnlyAttack(city.occupyingUnits, pair.scrollview.GetUnits());
            }
        }
    }

    void ExecuteBattleFromRoad(Road road)
    {
        BattleManager.instance.FullAttack(city1RoadScrollviewPair.attackingUnitsScrollview.GetUnits(), city1RoadScrollviewPair.cityBeingAttacked.occupyingUnits);
        BattleManager.instance.FullAttack(city2RoadScrollviewPair.attackingUnitsScrollview.GetUnits(), city2RoadScrollviewPair.cityBeingAttacked.occupyingUnits);
    }

    void ExecuteSkirmishFromRoad(Road road)
    {
        BattleManager.instance.RangedOnlyAttack(city1RoadScrollviewPair.attackingUnitsScrollview.GetUnits(), city1RoadScrollviewPair.cityBeingAttacked.occupyingUnits);
        BattleManager.instance.RangedOnlyAttack(city2RoadScrollviewPair.attackingUnitsScrollview.GetUnits(), city2RoadScrollviewPair.cityBeingAttacked.occupyingUnits);
    }

    void CancelSelection()
    {
        if (selectedCity != null) {
            EventManager.instance.fireSelectedCityUpdatedEvent(selectedCity);
        } else {
            EventManager.instance.fireSelectedRoadUpdatedEvent(selectedRoad);
        }
    }
}
