using System.Collections.Generic;
using UnityEngine;

public class UnitCombiner : MonoBehaviour 
{
    public static UnitCombiner instance;

    public GameObject emptyUnitPrefab; 

    private int centuryCount;
    private int cohortCount;
    private int legionCount;

    void Awake() 
    {
        instance = this;  
        centuryCount = 0;
        cohortCount = 0;
        legionCount = 0;  
    }  

    /// <summary>
    /// Creates and returns a new century.
    /// Instantiates a new game object.
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

            return century;
        } else {
            return null;
        }
    }
}