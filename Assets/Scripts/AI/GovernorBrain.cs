﻿using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A more limited version of the standard Brain class.
/// TODO: Consider moving shared functionality to an abstract class.
/// </summary>
public class GovernorBrain : Brain
{
    private Governor governor { get; set; }

    public GovernorBrain(Governor gov)
    {
        this.governor = gov;
    }

    /// <summary>
    /// Creates a list of all possible actions for the next turn.
    /// </summary>
    override protected List<Action> CreatePossibleActionsList()
    {
        List<Action> actions = new List<Action>();

        actions.AddRange(CreatePurchaseTroopsActions());
        actions.Add(CreateTaxRateAction());
        actions.Add(CreatePurchaseBuildingAction());

        return actions;
    }
    
    /// <summary>
    /// Creates the possible purchases a leader can make.
    /// </summary>
    private List<Action> CreatePurchaseTroopsActions()
    {
        List<Action> actions = new List<Action>();

        foreach (TroopClassification troopClass in governor.city.unitsForSale)
        {
            actions.Add(new PurchaseTroopAction(governor.city, governor, troopClass));
        }

        return actions;
    }

    /// <summary>
    /// Contains logic to calculate/recalculate a city's tax rates.
    /// </summary>
    private Action CreateTaxRateAction()
    {
        return new TaxRateAction(this.governor.city, .2f);
    }

    /// <summary>
    /// Randomly selects one of the buildings for purchase within the city and 
    /// creates a purchase building out of it.
    /// </summary>
    private Action CreatePurchaseBuildingAction() 
    {   
        /// TODO: Make this not random.
        Building randBuilding = governor.city.buildingsForPurchase[Random.Range(0, governor.city.buildingsForPurchase.Count)];
        return new PurchaseBuildingAction(governor.city, governor, randBuilding);
    }

    /// <summary>
    /// Sorts the actions by value.
    /// TODO: Take the AI's state (or strategy) into consideration when valuing actions. 
    /// </summary>
    override protected List<Action> ConsiderPossibleActionsValue(List<Action> actions)
    {
        actions.Sort((a, b) =>
        {
            return a.value.CompareTo(b.value);
        });

        return actions;
    }

}
