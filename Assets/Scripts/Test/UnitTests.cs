using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will run all unit tests at the start of the game.
/// It should be removed in production builds.
/// TODO: Consider moving this to its own scene.
/// </summary>
public class UnitTests : MonoBehaviour
{
    void Start()
    {
        gameObject.AddComponent<BattleManagerTests>();
        RunTests();
    }

    void RunTests()
    {
        print("xxxxxxxxxx Executing tests xxxxxxxxxx");
        BattleManagerTests.instance.Execute();
        print("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
    }
}
