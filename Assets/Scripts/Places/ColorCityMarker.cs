using UnityEngine;

public class ColorCityMarker : MonoBehaviour
{   
    [SerializeField] private Material roman;
    [SerializeField] private Material independent;
    [SerializeField] private Material gallic;
    [SerializeField] private Material noAllegiance;

    [SerializeField] private Material highlightedMaterial;

    private City city;

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
                case Allegiance.NONE:
                    GetComponent<Renderer>().material = noAllegiance;
                    break;
                case Allegiance.GALLIC:
                    GetComponent<Renderer>().material = gallic;
                    break;
            }
        }
    }
}
