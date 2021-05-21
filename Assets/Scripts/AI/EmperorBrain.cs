using System.Collections.Generic;
using UnityEngine;

public class EmperorBrain : Brain
{
    private Emperor emperor;

    public EmperorBrain(Emperor leader) 
    {
        this.emperor = leader;
    }

    /// <summary>
    /// Creates a list of all possible actions for the next turn.
    /// </summary>
    override protected List<Action> CreatePossibleActionsList() 
    {
        List<Action> actions = new List<Action>();
        foreach (City city in MapManager.instance.cities){
            if (city.allegiance == emperor.allegiance) {
                actions.AddRange(CreatePossibleCityActions(city));
            }
        };

        return actions;
    }

    /// <summary>
    /// Creates all possible actions a leader can take.
    /// Whenever a new action is described, it needs to be included here.
    /// </summary>
    private List<Action> CreatePossibleCityActions(City city) 
    {   
        List<Action> actions = new List<Action>();

        actions.AddRange(CreatePurchaseTroopsActions(city));
        actions.Add(CreateTaxRateAction(city));
        actions.Add(CreatePurchaseBuildingAction(city));

        return actions;
    }

    /// <summary>
    /// Randomly selects one of the buildings for purchase within the city and 
    /// creates a purchase building out of it.
    /// </summary>
    private Action CreatePurchaseBuildingAction(City city)
    {
        /// TODO: Make this not random.
        Building randBuilding = city.buildingsForPurchase[Random.Range(0, city.buildingsForPurchase.Count)];
        return new PurchaseBuildingAction(city, emperor, randBuilding);
    }


    /// <summary>
    /// Creates the possible purchases a leader can make.
    /// </summary>
    private List<Action> CreatePurchaseTroopsActions (City city) 
    {
        List<Action> actions = new List<Action>();

        foreach (TroopClassification troopClass in city.unitsForSale) {
            actions.Add(new PurchaseTroopAction(city, emperor, troopClass));
        }

        return actions;
    } 

    /// <summary>
    /// Contains logic to calculate/recalculate a city's tax rates.
    /// </summary>
    private Action CreateTaxRateAction(City city) 
    {
        return new TaxRateAction(city, .2f);
    }

    /// <summary>
    /// Sorts the actions by value.
    /// TODO: Take the AI's state (or strategy) into consideration when valuing actions. 
    /// </summary>
    override protected List<Action> ConsiderPossibleActionsValue(List<Action> actions) 
    {
        actions.Sort((a, b) => {
            return a.value.CompareTo(b.value);
        });
        
        return actions;
    }
}