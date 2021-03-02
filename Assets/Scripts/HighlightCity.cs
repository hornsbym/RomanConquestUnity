using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightCity : MonoBehaviour
{
    public Material unselectedMaterial;
    public Material selectedMaterial;

    void OnMouseEnter()
    {
        GetComponent<Renderer>().material = selectedMaterial;
    }

    void OnMouseExit() 
    {
        GetComponent<Renderer>().material = unselectedMaterial;
    }
}
