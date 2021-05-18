using System.Collections.Generic;

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

        actions.AddRange(CreatePurchaseActions(city));
        actions.Add(CreateTaxRateAction(city));

        return actions;
    }


    /// <summary>
    /// Creates the possible purchases a leader can make.
    /// </summary>
    private List<Action> CreatePurchaseActions (City city) 
    {
        List<Action> actions = new List<Action>();

        foreach (TroopClassification troopClass in city.unitsForSale) {
            actions.Add(new PurchaseAction(city, emperor, troopClass));
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