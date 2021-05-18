using System.Collections.Generic;

public abstract class Brain
{
    /// <summary>
    /// Executes all game actions the AI has chosen.
    /// </summary>
    public void TakeTurn() 
    {
        // Create all possible actions
        List<Action> actions = CreatePossibleActionsList();

        // Pick the best ones
        actions = ConsiderPossibleActionsValue(new List<Action>(actions));

        // Execute the best actions
        foreach(Action action in actions) {
            // Checks if the action is still possible, and if so execute it.
            // This is necessary for actions that can each other, such as buying troops.
            if (action.IsPossible()) {
                action.Execute();
            }
        }
    }

    /// <summary>
    /// Creates a list of all possible actions for the next turn.
    /// </summary>
    protected abstract List<Action> CreatePossibleActionsList();


    /// <summary>
    /// Sorts the actions by value.
    /// TODO: Take the AI's state (or strategy) into consideration when valuing actions. 
    /// </summary>
    protected abstract List<Action> ConsiderPossibleActionsValue(List<Action> actions);
}