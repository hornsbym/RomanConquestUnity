using System;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    public static Utilities instance;

    void Start() {
        instance = this;
    }
}
