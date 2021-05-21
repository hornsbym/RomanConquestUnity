using UnityEngine;

public class HighlightRoad : MonoBehaviour
{
    private Material unselectedMaterial;
    public Material selectedMaterial;

    void Start() {
        unselectedMaterial = GetComponent<Renderer>().material;
    }

    void OnMouseEnter()
    {
        GetComponent<Renderer>().material = selectedMaterial;
    }

    void OnMouseExit() 
    {
        GetComponent<Renderer>().material = unselectedMaterial;
    }
}
