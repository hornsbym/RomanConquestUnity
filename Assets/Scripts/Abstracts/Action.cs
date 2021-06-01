/// <summary>
/// Represents something that can be done in the game.
/// Should contain all of the information necessary to perform the action.
/// </summary>
public abstract class Action  
{
    public int value { get; protected set; }

    /// <summary>
    /// Performs all necessary game logic to take the action.
    /// </summary>
    public abstract void Execute();


    /// <summary>
    /// Contains logic  to checks if a move can be made.
    /// </summary>
    public abstract bool IsPossible();


    /// <summary>
    /// Contains logic to set a move's value.
    /// The higher the value, the better a move is considered to be.
    /// TODO: Create ENUM constants to parameterize how a move should be evaluated.
    /// TODO: Consider moving this logic one step up into the "Brain" class.
    /// </summary>
    public abstract void Evaluate();
}