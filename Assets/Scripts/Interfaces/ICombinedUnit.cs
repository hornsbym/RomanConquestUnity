using System.Collections.Generic;

interface ICombinedUnit : IUnit 
{
    List<Unit> units {get; set;}
}