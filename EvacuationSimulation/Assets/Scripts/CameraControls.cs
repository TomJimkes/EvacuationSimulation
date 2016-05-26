using System;
using UnityEngine;

/// <summary>
/// Allows the user to move the camera around.
/// </summary>
public class CameraControls : MonoBehaviour
{
    //Publicly settable variables
    public float moveSpeed = 0.2f;
    public float acceleration = 0.2f;

    public float zoomSpeed = 1f;
    public float maxZoomDistance = 5;
    public float minZoomDistance = 2;

    //Target position to move towards
    private Vector3 destination;
    private float destinationZoom;

    // Use this for initialization
    void Start()
    {
        //Set the destination to the current position
        destination = gameObject.transform.position;
        destinationZoom = gameObject.GetComponent<Camera>().orthographicSize;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Update the zoom distance
        gameObject.GetComponent<Camera>().orthographicSize = destinationZoom;

        //Move the camera towards its destination
        Vector3 pos = gameObject.transform.position;
        pos = Vector3.Lerp(pos, destination, Math.Min(acceleration, 1));

        gameObject.transform.position = pos;

    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        //Zoom by either pinching on a tablet or scrolling with a mouse
        if (Math.Abs(InputHelper.Instance.zoomDistance) > 0.01f)
        {
            float prevZoom = gameObject.GetComponent<Camera>().orthographicSize;
            //Use multiplication in order to generate a nice zooming speed curve
            destinationZoom = prevZoom * (1 + (InputHelper.Instance.zoomDistance * zoomSpeed) / 200);
            destinationZoom = Mathf.Clamp(destinationZoom, minZoomDistance, maxZoomDistance);

            return;
        }

        //Check if the user is dragging with the mouse
        if (InputHelper.Instance.dragging)
        {
            //Move according to the dragging distance
            destination += Camera.main.ScreenToWorldPoint(Vector3.zero) -
                Camera.main.ScreenToWorldPoint(InputHelper.Instance.dragDistance);
        }

        //Scrolling by using the arrow keys
        if (Input.GetKey(KeyCode.RightArrow))
            destination.x += moveSpeed;
        if (Input.GetKey(KeyCode.UpArrow))
            destination.y += moveSpeed;
        if (Input.GetKey(KeyCode.LeftArrow))
            destination.x -= moveSpeed;
        if (Input.GetKey(KeyCode.DownArrow))
            destination.y -= moveSpeed;

    }
}
