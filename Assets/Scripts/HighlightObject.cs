using UnityEngine;

public class HighlightObject : MonoBehaviour
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
