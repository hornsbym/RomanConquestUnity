using System.Collections.Generic;
using UnityEngine;

public class UnitCombiner : MonoBehaviour 
{
    public static UnitCombiner instance;

    public GameObject emptyUnitPrefab; 

    private int centuryCount;
    private int cohortCount;
    private int legionCount;

    public City currentCity;

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
            century.unitName = UnitFactory.countToOrdinal(centuryCount) + " Century";
            century.SetUnits(troops);

            foreach (Troop troop in troops) 
            {
                currentCity.friendlyUnits.Remove(troop);
            }

            currentCity.AddUnit(century);
            
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
            cohort.unitName = UnitFactory.countToOrdinal(cohortCount) + " Cohort";
            cohort.SetUnits(centuries);

            foreach (Century century in centuries)
            {
                currentCity.friendlyUnits.Remove(century);
            }

            currentCity.AddUnit(cohort);

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
            legion.unitName = UnitFactory.countToOrdinal(legionCount) + " Legion";
            legion.SetUnits(cohorts);

            foreach (Cohort cohort in cohorts)
            {
                currentCity.friendlyUnits.Remove(cohort);
            }

            currentCity.AddUnit(legion);

            return legion;
        }
        else
        {
            return null;
        }
    }
}