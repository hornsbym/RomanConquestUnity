using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCityMarker : MonoBehaviour
{   
    public Material roman;
    public Material independent;
    public Material gallic;

    private City city;

    public Material highlightedMaterial;

    private bool isHighlighted {get; set;}

    void Start()
    {
        city = GetComponent<City>();
    }

    void OnMouseEnter()
    {
        isHighlighted = true;
    }

    void OnMouseExit()
    {
        isHighlighted = false;
    }

    void Update()
    {   
        if (isHighlighted) {
            GetComponent<Renderer>().material = highlightedMaterial;
        } else {
            switch (city.allegiance)
            {
                case Allegiance.ROMAN:
                    GetComponent<Renderer>().material = roman;
                    break;
                case Allegiance.INDEPENDENT:
                    GetComponent<Renderer>().material = independent;
                    break;
                case Allegiance.GALLIC:
                    GetComponent<Renderer>().material = gallic;
                    break;
            }
        }
    }
}
