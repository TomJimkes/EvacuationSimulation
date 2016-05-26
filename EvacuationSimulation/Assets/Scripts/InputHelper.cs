using System;
using UnityEngine;

/// <summary>
/// A class containing helper functions for user input.
/// Use this instead of the regular Input class.
/// Example: use InputHelper.Instance.mousePosition to get the current mouse/cursor position.
/// </summary>
public class InputHelper : MonoBehaviour
{
    #region Public variables
    /// <summary>
    /// The current instance of the InputHelper.
    /// </summary>
    public static InputHelper Instance = null;

    /// <summary>
    /// The distance dragged in x- and y-directions in this frame.
    /// </summary>
    public Vector2 dragDistance;

    /// <summary>
    /// The current position of the mouse/cursor.
    /// </summary>
    public Vector2 mousePosition;

    /// <summary>
    /// The previous position of the mouse/cursor.
    /// </summary>
    public Vector2 prevMousePosition;

    /// <summary>
    /// Whether or not the player is currently dragging the mouse while holding the left mouse button.
    /// </summary>
    public bool dragging;

    /// <summary>
    /// Whether or not the player was dragging the mouse up until now.
    /// </summary>
    public bool dragged;

    /// <summary>
    /// Whether or not the button was pressed in this frame.
    /// </summary>
    public bool leftClicked, rightClicked;

    /// <summary>
    /// Whether or not the button is being held down in this frame.
    /// </summary>
    public bool leftDown, rightDown;

    /// <summary>
    /// Whether or not the button was released in this frame.
    /// </summary>
    public bool leftReleased, rightReleased;

    /// <summary>
    /// Whether or not the user in pinching using two fingers.
    /// </summary>
    public bool pinching;

    /// <summary>
    /// The pinch distance compared to the last update.
    /// </summary>
    public float zoomDistance;
    #endregion

    #region Private variables
    /// <summary>
    /// The position where the mouse was clicked last.
    /// </summary>
    private Vector2 lastMouseClick;

    /// <summary>
    /// Whether or not the button was down during the last frame.
    /// </summary>
    private bool prevLeftDown, prevRightDown;

    /// <summary>
    /// The previous total pinch distance.
    /// </summary>
    private float prevPinchDistance;

    /// <summary>
    /// The distance that was scrolled; used to check scroll distance for touchpads
    /// </summary>
    private float scrollDistance;
    #endregion

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void OnGUI()
    {
        scrollDistance = 0;
        if (Event.current.type == EventType.ScrollWheel)
        {
            scrollDistance = -Event.current.delta.y;
        }
    }

    private void Update()
    {

        //Get information from the Input class concerning the mouse
        mousePosition = Input.mousePosition;
        leftDown = Input.GetMouseButton(0);
        rightDown = Input.GetMouseButton(1);

        //Set whether or not the respective buttons are pressed or released during this frame
        leftClicked = !prevLeftDown && leftDown;
        rightClicked = !prevRightDown && rightDown;
        leftReleased = !leftDown && prevLeftDown;
        rightReleased = !rightDown && prevRightDown;

        //Reset the dragged variable and the last mouse click location upon clicking with the left button
        if (leftClicked)
        {
            lastMouseClick = mousePosition;
            dragged = false;
        }

        //Set the drag variables to true if the user moves far enough away from the click location 
        //while holding the left mouse button down
        if (leftDown && Vector2.Distance(lastMouseClick, mousePosition) > 20)
        {
            dragging = true;
            dragged = true;
        }

        //Reset the dragging variable if the user stops pressing the left mouse button
        if (!leftDown)
            dragging = false;

        //Set the dragging distance of this frame if the user is dragging the mouse
        if (dragging)
            dragDistance = mousePosition - prevMousePosition;
        else
            dragDistance = Vector2.zero;


        if (Input.touchCount >= 2)
        {
            pinching = true;

            Vector2 touch0, touch1;
            float distance;
            touch0 = Input.GetTouch(0).position;
            touch1 = Input.GetTouch(1).position;

            distance = Vector2.Distance(touch0, touch1);

            zoomDistance = -Mathf.Min(distance - prevPinchDistance, prevPinchDistance);

            prevPinchDistance = distance;
        }
        else
        {
            if (Math.Abs(scrollDistance) < float.Epsilon)
            {
                scrollDistance = -Input.mouseScrollDelta.y * 8;
            }
            pinching = false;
            prevPinchDistance = 0;
            zoomDistance = -scrollDistance;
        }

        //Save the data of this frame in the 'previous' variables
        prevMousePosition = mousePosition;
        prevLeftDown = leftDown;
        prevRightDown = rightDown;
    }

}
