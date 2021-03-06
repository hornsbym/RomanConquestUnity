using System.Collections.Generic;
using UnityEngine;

public class UnitCombiner : MonoBehaviour 
{
    public static UnitCombiner instance;

    public GameObject emptyUnitPrefab; 

    private int centuryCount;
    private int cohortCount;
    private int legionCount;

    private City currentCity;

    void Awake() 
    {
        instance = this;  
        centuryCount = 0;
        cohortCount = 0;
        legionCount = 0;  

        EventManager.OnCombineSelected += setCurrentCity;
    }  

    private void setCurrentCity(City city) 
    {
        this.currentCity = city;
    }

    /// <summary>
    /// Creates and returns a new century.
    /// Instantiates a new game object.
    /// If successful, removes the troops from the city's units
    /// and adds the century.
    /// </summary>
    public Century FormCentury(List<Troop> troops) 
    {
        if (troops.Count >= CombinedUnitConstants.CENTURY_SIZE_LOWER_BOUND 
            && troops.Count <= CombinedUnitConstants.CENTURY_SIZE_UPPER_BOUND) 
        {
            centuryCount++;
            Century century = Instantiate(emptyUnitPrefab).AddComponent<Century>();
            century.unitName = Utilities.instance.CountToOrdinal(centuryCount) + " Century";
            century.units = troops;

            foreach (Troop troop in troops) 
            {
                currentCity.occupyingUnits.Remove(troop);
            }

            currentCity.AddOccupyingUnits(new List<Unit>(){century});
            
            return century;
        } else {
            return null;
        }
    }

    /// <summary>
    /// Creates and returns a new cohort.
    /// Instantiates a new game object.
    /// If successful, removes the centuries from the city's units
    /// and adds the cohort.
    /// </summary>
    public Cohort FormCohort(List<Century> centuries)
    {
        if (centuries.Count >= CombinedUnitConstants.COHORT_SIZE_LOWER_BOUND
        && centuries.Count <= CombinedUnitConstants.COHORT_SIZE_UPPER_BOUND){
            cohortCount++;
            Cohort cohort = Instantiate(emptyUnitPrefab).AddComponent<Cohort>();
            cohort.unitName = Utilities.instance.CountToOrdinal(cohortCount) + " Cohort";
            cohort.units = centuries;

            foreach (Century century in centuries)
            {
                currentCity.occupyingUnits.Remove(century);
            }

            currentCity.AddOccupyingUnits(new List<Unit>(){cohort});

            return cohort;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Creates and returns a new legion.
    /// Instantiates a new game object.
    /// If successful, removes the cohorts from the city's units
    /// and adds the legion.
    /// </summary>
    public Legion FormLegion(List<Cohort> cohorts)
    {
        if (cohorts.Count >= CombinedUnitConstants.LEGION_SIZE_LOWER_BOUND
        && cohorts.Count <= CombinedUnitConstants.LEGION_SIZE_UPPER_BOUND)
        {
            legionCount++;
            Legion legion = Instantiate(emptyUnitPrefab).AddComponent<Legion>();
            legion.unitName = Utilities.instance.CountToOrdinal(legionCount) + " Legion";
            legion.units = cohorts;

            foreach (Cohort cohort in cohorts)
            {
                currentCity.occupyingUnits.Remove(cohort);
            }

            currentCity.AddOccupyingUnits(new List<Unit>(){ legion });

            return legion;
        }
        else
        {
            return null;
        }
    }
}