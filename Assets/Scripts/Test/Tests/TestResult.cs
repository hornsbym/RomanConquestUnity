using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestResult
{
    public string test { get; private set; }
    public bool result { get; private set; }

    public TestResult ( string test, bool result ) 
    {
        this.test = test;
        this.result = result;
    }
}
