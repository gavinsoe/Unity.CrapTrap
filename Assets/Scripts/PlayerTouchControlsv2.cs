using UnityEngine;
using System.Collections;

public class PlayerTouchControlsv2 : MonoBehaviour {

     // Variables related to the touch controls
    private int maxTouches = 2;	// up to 5 (iOS only supports 5 apparently)
    private float minDragDistance = 50f; // Swipe distance before touch is regarded as 'touch and drag'
    private float minHoldTime = 0.1f; // Time before touch is regarded as 'touch and hold'

    enum InputState { TouchLeft, TouchRight, DragLeft, DragRight, SwipeDown, MovingRight, MovingLeft, Done };
    enum Commands { None, MoveLeft, MoveRight, DragLeft, DragRight, HangDown, ClimbUp, PullOut };
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
	void Update ()
    {
        if (!character.isMoving)
        {
            character.idle();
        }
        // Detect Touch input
        foreach (Touch touch in Input.touches)
        {
            #region old
            if (Debug.isDebugBuild) Debug.Log("[" + touch.fingerId + "] NextCommand : " + nextCommand.ToString());
            if (Debug.isDebugBuild) Debug.Log("[" + touch.fingerId + "] Phase : " + touch.phase.ToString() + " | Time : " + (Time.time - touchStartTime[touch.fingerId]));
            // When a finger touches the screen...
            if (touch.phase == TouchPhase.Began)
            {
                //if (Debug.isDebugBuild) Debug.Log("[" + touch.fingerId + "] Touch detected at : " + touch.position.ToString());

                // Store the location and time of the initial touch position
                touchStartPosition[touch.fingerId] = touch.position;
                touchStartTime[touch.fingerId] = Time.time;

                // Check which side the touch occurs at
                if (touch.position.x < Screen.width / 2)
                {
                    touchState[touch.fingerId] = InputState.TouchLeft;
                }
                else if (touch.position.x > Screen.width / 2)
                {
                    touchState[touch.fingerId] = InputState.TouchRight;
                }

                // When a new touch comes in, reset the queued commands to none.
                nextCommand = Commands.None;
                character.wasMoving = false;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                var deltaPosition = touch.position - touchStartPosition[touch.fingerId];

                if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y))
                {
                    if (Input.touches.Length == 1)
                    {
                        if (deltaPosition.x > minDragDistance)
                        {
                            nextCommand = Commands.MoveRight;
                            touchState[touch.fingerId] = InputState.MovingRight;
                        }
                        else if (deltaPosition.x < -minDragDistance)
                        {
                            nextCommand = Commands.MoveLeft;
                            touchState[touch.fingerId] = InputState.MovingLeft;
                        }
                    }
                    else
                    {
                        if (deltaPosition.x > minDragDistance)
                        {
                            nextCommand = Commands.DragRight;
                            touchState[touch.fingerId] = InputState.DragRight;
                        }
                        else if (deltaPosition.x < -minDragDistance)
                        {
                            nextCommand = Commands.DragLeft;
                            touchState[touch.fingerId] = InputState.DragLeft;
                        }
                    }
                    /*
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
                    {                        if (deltaPosition.x > minDragDistance)
                        {
                            nextCommand = Commands.Right_DragRight;
                        }
                        else if (deltaPosition.x < -minDragDistance)
                        {
                            nextCommand = Commands.Right_DragLeft;
                        }
                    }*/
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
                //if (Time.time - touchStartTime[touch.fingerId] > minHoldTime)
                //{
                    // check on which side of the screen the tap occured
                    /*if (touchState[touch.fingerId] == InputState.TouchLeft ||
                        touchState[touch.fingerId] == InputState.MovingLeft)
                    {
                        nextCommand = Commands.MoveLeft;
                    }
                    else if (touchState[touch.fingerId] == InputState.TouchRight ||
                             touchState[touch.fingerId] == InputState.MovingRight)
                    {
                        nextCommand = Commands.MoveRight;
                    }*/

                if (Time.time - touchStartTime[touch.fingerId] > minHoldTime)
                {
                    if (Input.touches.Length == 1)
                    {
                        if (touchState[touch.fingerId] == InputState.MovingLeft)
                        {
                            nextCommand = Commands.MoveLeft;
                        }
                        else if (touchState[touch.fingerId] == InputState.MovingRight)
                        {
                            nextCommand = Commands.MoveRight;
                        }
                        else
                        {
                            nextCommand = Commands.None;
                        }
                    }
                    else
                    {
                        if (touchState[touch.fingerId] == InputState.DragRight)
                        {
                            nextCommand = Commands.DragRight;
                        }
                        else if (touchState[touch.fingerId] == InputState.DragLeft)
                        {
                            nextCommand = Commands.DragLeft;
                        }
                        else
                        {
                            nextCommand = Commands.None;
                        }
                    }
                }
            }
            #endregion
            // Check for any queued commands and execute when possible
            if (!character.isMoving)
            {
                if (nextCommand == Commands.MoveRight)
                {
                    StartCoroutine(character.move(transform, 1, false));
                    touchState[touch.fingerId] = InputState.MovingRight;
                }
                else if (nextCommand == Commands.MoveLeft)
                {
                    StartCoroutine(character.move(transform, -1, false));
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
                else if (nextCommand == Commands.DragLeft)
                {
                    StartCoroutine(character.move(transform, -1, true));
                    touchState[touch.fingerId] = InputState.DragLeft;
                }
                else if (nextCommand == Commands.DragRight)
                {
                    StartCoroutine(character.move(transform, 1, true));
                    touchState[touch.fingerId] = InputState.DragRight;
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
