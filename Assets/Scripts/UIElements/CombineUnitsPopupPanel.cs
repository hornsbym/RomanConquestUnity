using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CombineUnitsPopupPanel : MonoBehaviour
{
    public Button cancelCombineButton;
    public Button confirmCombineButton;

    public GameObject availableTroopsScrollviewPrefab;
    public GameObject selectedTroopsScrollviewPrefab;

    private List<Unit> selectedUnits;
    private List<Unit> availableUnits;
    
    void Awake() 
    {
        EventManager.OnUnitAdded += Initialize;
        EventManager.OnCitySelected += Initialize;

    }

    private void Initialize(City selectedCity)
    {
        selectedUnits = new List<Unit>();
        availableUnits = new List<Unit>(selectedCity.friendlyUnits);

        InitializeButtons();
        InitializeAvailableTroopsScrollview();
        InitializeSelectedTroopsScrollview();
    }

    private void InitializeButtons()
    {
        cancelCombineButton.onClick.AddListener(() => {
            EventManager.instance.fireDefaultSelectedEvent();
        });

        confirmCombineButton.onClick.AddListener(() => {
            
            if (UnitListIsValid()) {
                switch(selectedUnits[0]) {
                    case Troop t:
                        Century century = UnitCombiner.instance.FormCentury(new SelectedUnits<Troop>(selectedUnits).units);
                        if (century != null) {
                            selectedUnits.Clear();
                        } else {
                            this.DeselectAllUnits();
                        }
                        break;
                    case Century c:
                        Cohort cohort = UnitCombiner.instance.FormCohort(new SelectedUnits<Century>(selectedUnits).units);
                        if (cohort != null)
                        {
                            selectedUnits.Clear();
                        }
                        else
                        {
                            this.DeselectAllUnits();
                        }
                        break;
                    case Cohort c:
                        Legion legion = UnitCombiner.instance.FormLegion(new SelectedUnits<Cohort>(selectedUnits).units);
                        if (legion != null)
                        {
                            selectedUnits.Clear();
                        }
                        else
                        {
                            this.DeselectAllUnits();
                        }
                        break;
                    default:
                        break;
                }

                InitializeAvailableTroopsScrollview();
                InitializeSelectedTroopsScrollview();
            } else {
                this.DeselectAllUnits();
                InitializeAvailableTroopsScrollview();
                InitializeSelectedTroopsScrollview();
            };
        });
    }

    /// <summary>
    /// Private inner class responsible to help combine units.
    /// </summary>
    private class SelectedUnits<T> where T : Unit 
    {
        public List<T> units {get;}

        public SelectedUnits(List<Unit> units) {
            this.units = new List<T>();
            
            foreach(Unit unit in units) {
                this.units.Add((T) unit);
            }
        }
    }

    /// <summary>
    /// Makes sure all units in the list are of the same type
    /// </summary>
    private bool UnitListIsValid() 
    {
        for(int i = 0; i < this.selectedUnits.Count - 1; i++)
        {
            if (selectedUnits[i].GetType() != selectedUnits[i+1].GetType()) {
                return false;
            }
        }
        return true;
    }


    private void InitializeAvailableTroopsScrollview()
    {
        UnitsScrollview sv = availableTroopsScrollviewPrefab.GetComponent<UnitsScrollview>();
        sv.AddTitle("Available units");
        sv.SetContent(this.availableUnits, (Unit u) => { SelectUnit(u); UpdateScrollviews();});
    }

    private void InitializeSelectedTroopsScrollview()
    {
        UnitsScrollview sv = selectedTroopsScrollviewPrefab.GetComponent<UnitsScrollview>();
        sv.AddTitle("Selected units");
        sv.SetContent(this.selectedUnits, (Unit u) => {DeselectUnit(u); UpdateScrollviews();});
    }
    
    /// <summary>
    /// Moves a unit from the available units list 
    /// to the selected units list.
    /// </summary>
    private void SelectUnit(Unit u) 
    {
        this.availableUnits.Remove(u);
        this.selectedUnits.Add(u);
    }

    /// <summary>
    /// Moves a unit from the selected units list 
    /// to the available units list.
    /// </summary>
    private void DeselectUnit(Unit u)
    {
        this.selectedUnits.Remove(u);
        this.availableUnits.Add(u);
    }

    /// <summary>
    /// Moves a group of units from the selected list to the 
    /// available units list.
    /// </summary>
    private void DeselectAllUnits() 
    {
        this.availableUnits.AddRange(selectedUnits);
        this.selectedUnits.Clear();
    }

    private void UpdateScrollviews()
    {
        InitializeAvailableTroopsScrollview();
        InitializeSelectedTroopsScrollview();
    }

}
