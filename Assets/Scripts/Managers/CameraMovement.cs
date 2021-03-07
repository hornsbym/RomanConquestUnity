using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{   
    // Get a reference to the map sprite so we can figure out
    // the movement bounds.
    public GameObject mapSprite;

    // Represents the edges of the map sprite.
    private float topBound;
    private float bottomBound;
    private float rightBound;
    private float leftBound;

    [SerializeField] 
    public float horizontalPanRate;
    [SerializeField]
    public float verticalPanRate;

    // Start is called before the first frame update
    void Start()
    {
        InitializeMapBounds();
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalPan();
        VerticalPan();

    }

    /// <summary>
    /// Moves the camera position left or right.
    /// Won't extend past horizontal Map boundaries.
    ///</summary>
    private void HorizontalPan()
    {
        float hInput = Input.GetAxis("Horizontal");

        float xChange = hInput * horizontalPanRate;

        Vector3 newPosition = new Vector3(
            Mathf.Clamp(this.gameObject.transform.position.x + xChange, leftBound, rightBound),
            this.gameObject.transform.position.y,
            this.gameObject.transform.position.z
        );

        this.gameObject.transform.position = newPosition;
    }

    /// <summary>
    /// Moves the camera position up or down.
    /// Won't extend past vertical Map boundaries.
    ///</summary>
    private void VerticalPan()
    {
        float vInput = Input.GetAxis("Vertical");
        
        float yChange = vInput * verticalPanRate;

        Vector3 newPosition = new Vector3(
            this.gameObject.transform.position.x,
            Mathf.Clamp(this.gameObject.transform.position.y + yChange, bottomBound, topBound),
            this.gameObject.transform.position.z
        );

        this.gameObject.transform.position = newPosition;
    }

    /// <summary>
    /// Calculates the map edge boundaries based the provided map sprite.
    /// </summary>
    private void InitializeMapBounds()
    {
        Vector3 mapOrigin = mapSprite.GetComponent<Renderer>().bounds.center;
        Vector3 mapExtents = mapSprite.GetComponent<Renderer>().bounds.extents;

        topBound = mapOrigin.y + mapExtents.y;
        bottomBound = mapOrigin.y - mapExtents.y;
        rightBound = mapOrigin.x + mapExtents.x;
        leftBound = mapOrigin.x - mapExtents.x;
    }
}
