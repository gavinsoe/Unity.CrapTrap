using UnityEngine;
using System.Collections;

public class PlayerTouchControls : MonoBehaviour {

     // Variables related to the touch controls
    private int maxTouches = 2;	// up to 5 (iOS only supports 5 apparently)
    private float minDragDistance = 25f; // Swipe distance before touch is regarded as 'touch and drag'
    private float minHoldTime = 0.15f; // Time before touch is regarded as 'touch and hold'

    enum InputState { TouchLeft, TouchRight, Left_DragLeft, Left_DragRight, Right_DragRight, Right_DragLeft, SwipeDown, MovingRight, MovingLeft, Done };
    enum Commands { None, MoveLeft, MoveRight, Left_DragLeft, Left_DragRight, Right_DragRight, Right_DragLeft, HangDown, ClimbUp, PullOut };
    private InputState[] touchState;
    private Vector2[] touchStartPosition;
    private float[] touchStartTime;
    private Commands nextCommand;

    // The component that handles all the character movement.
    private CharacterController character;

    // Use this for initialization
	void Start () {

        // Retrieve the characterController
        character = GetComponent<CharacterController>();

        // inititialise the arrays used for manipulating the touch controls
        touchStartPosition = new Vector2[maxTouches];
        touchStartTime = new float[maxTouches];
        touchState = new InputState[maxTouches];
	}
	
	// Update is called once per frame
	void Update () {

        // Detect Touch input
        foreach (Touch touch in Input.touches)
        {
            //if (Debug.isDebugBuild) Debug.Log("[" + touch.fingerId + "] NextCommand : " + nextCommand.ToString());
            //if (Debug.isDebugBuild) Debug.Log("[" + touch.fingerId + "] Phase : " + touch.phase.ToString() + " | Time : " + (Time.time - touchStartTime[touch.fingerId]));
            // When a finger touches the screen...
            if (touch.phase == TouchPhase.Began)
            {
                //if (Debug.isDebugBuild) Debug.Log("[" + touch.fingerId + "] Touch detected at : " + touch.position.ToString());

                // Store data on where the user touched the screen, and the time of when it is pressed
                touchStartPosition[touch.fingerId] = touch.position;
                touchStartTime[touch.fingerId] = Time.time;

                if (touch.position.x < Screen.width / 2)
                {
                    touchState[touch.fingerId] = InputState.TouchLeft;
                }
                else if (touch.position.x > Screen.width / 2)
                {
                    touchState[touch.fingerId] = InputState.TouchRight;
                }

                nextCommand = Commands.None;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                var deltaPosition = touch.position - touchStartPosition[touch.fingerId];

                if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y))
                {
                    if (touchState[touch.fingerId] == InputState.TouchLeft)
                    {
                        if (deltaPosition.x > minDragDistance)
                        {
                            nextCommand = Commands.Left_DragRight;
                        }
                        else if (deltaPosition.x < -minDragDistance)
                        {
                            nextCommand = Commands.Left_DragLeft;
                        }
                    }
                    else if (touchState[touch.fingerId] == InputState.TouchRight)
                    {
                        if (deltaPosition.x > minDragDistance)
                        {
                            nextCommand = Commands.Right_DragRight;
                        }
                        else if (deltaPosition.x < -minDragDistance)
                        {
                            nextCommand = Commands.Right_DragLeft;
                        }
                    }
                }
                else
                {
                    if (deltaPosition.y > minDragDistance)
                    {
                        nextCommand = Commands.ClimbUp;
                    }
                    else if (deltaPosition.y < -minDragDistance)
                    {
                        if (Input.touches.Length == 1)
                        {
                            nextCommand = Commands.HangDown;
                        }
                        else
                        {
                            nextCommand = Commands.PullOut;
                        }
                    }
                }
            }

            if (touch.phase == TouchPhase.Stationary)
            {
                // check how long user has been touching the screen
                if (Time.time - touchStartTime[touch.fingerId] > minHoldTime)
                {
                    // check on which side of the screen the tap occured
                    if (touchState[touch.fingerId] == InputState.TouchLeft ||
                        touchState[touch.fingerId] == InputState.MovingLeft)
                    {
                        nextCommand = Commands.MoveLeft;
                    }
                    else if (touchState[touch.fingerId] == InputState.TouchRight ||
                             touchState[touch.fingerId] == InputState.MovingRight)
                    {
                        nextCommand = Commands.MoveRight;
                    }
                    else if (touchState[touch.fingerId] == InputState.Left_DragRight)
                    {
                        nextCommand = Commands.Left_DragRight;
                    }
                    else if (touchState[touch.fingerId] == InputState.Left_DragLeft)
                    {
                        nextCommand = Commands.Left_DragLeft;
                    }
                    else if (touchState[touch.fingerId] == InputState.Right_DragRight)
                    {
                        nextCommand = Commands.Right_DragRight;
                    }
                    else if (touchState[touch.fingerId] == InputState.Right_DragLeft)
                    {
                        nextCommand = Commands.Right_DragLeft;
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                // check on which side of the screen the tap occured
                if (touchState[touch.fingerId] == InputState.TouchLeft ||
                    touchState[touch.fingerId] == InputState.MovingLeft)
                {
                    nextCommand = Commands.MoveLeft;
                }
                else if (touchState[touch.fingerId] == InputState.TouchRight ||
                            touchState[touch.fingerId] == InputState.MovingRight)
                {
                    nextCommand = Commands.MoveRight;
                }
            }

            // Check for any queued commands and execute when possible
            if (nextCommand != Commands.None && !character.isMoving)
            {
                if (nextCommand == Commands.MoveRight)
                {
                    StartCoroutine(character.move(transform, 1, false, false));
                    touchState[touch.fingerId] = InputState.MovingRight;
                }
                else if (nextCommand == Commands.MoveLeft)
                {
                    StartCoroutine(character.move(transform, -1, false, false));
                    touchState[touch.fingerId] = InputState.MovingLeft;
                }
                else if (nextCommand == Commands.ClimbUp)
                {
                    StartCoroutine(character.hang(transform, 1));
                    touchState[touch.fingerId] = InputState.Done;
                }
                else if (nextCommand == Commands.HangDown)
                {
                    StartCoroutine(character.hang(transform, -1));
                    touchState[touch.fingerId] = InputState.Done;
                }
                else if (nextCommand == Commands.Left_DragLeft)
                {
                    StartCoroutine(character.move(transform, -1, true, false));
                    touchState[touch.fingerId] = InputState.Left_DragLeft;
                }
                else if (nextCommand == Commands.Left_DragRight)
                {
                    StartCoroutine(character.move(transform, 1, true, false));
                    touchState[touch.fingerId] = InputState.Left_DragRight;
                }
                else if (nextCommand == Commands.Right_DragLeft)
                {
                    StartCoroutine(character.move(transform, -1, false, true));
                    touchState[touch.fingerId] = InputState.Right_DragLeft;
                }
                else if (nextCommand == Commands.Right_DragRight)
                {
                    StartCoroutine(character.move(transform, 1, false, true));
                    touchState[touch.fingerId] = InputState.Right_DragRight;
                }
                else if (nextCommand == Commands.PullOut)
                {
                    StartCoroutine(character.pull(transform));
                    touchState[touch.fingerId] = InputState.Done;
                }

                nextCommand = Commands.None;
            }
        }
	}

}
