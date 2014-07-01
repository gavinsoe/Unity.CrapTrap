using UnityEngine;
using System.Collections;

public class PlayerTouchControlsv3 : MonoBehaviour {

     // Variables related to the touch controls
    private int maxTouches = 2;	// up to 5 (iOS only supports 5 apparently)
    private float minDragDistance = 50f; // Swipe distance before touch is regarded as 'touch and drag'
    private float minHoldTime = 0.1f; // Time before touch is regarded as 'touch and hold'

    public enum InputState { TouchLeft, TouchRight, DragLeft, DragRight, SwipeDown, MovingRight, MovingLeft, KeepMovingRight, KeepMovingLeft, Done};
    public enum Commands { None, MoveLeft, MoveRight, DragLeft, DragRight, HangDown, ClimbUp, PullOut };
    public InputState[] touchState;
    public Vector2[] touchStartPosition;
    public float[] touchStartTime;
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
            //if (Debug.isDebugBuild) Debug.Log("[" + touch.fingerId + "] NextCommand : " + nextCommand.ToString());
            //if (Debug.isDebugBuild) Debug.Log("[" + touch.fingerId + "] Phase : " + touch.phase.ToString() + " | Time : " + (Time.time - touchStartTime[touch.fingerId]));
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
                    if (touchState[touch.fingerId] == InputState.MovingRight ||
                        touchState[touch.fingerId] == InputState.MovingLeft)
                    {
                        touchState[touch.fingerId] = InputState.KeepMovingLeft;
                    }
                    else
                    {
                        touchState[touch.fingerId] = InputState.TouchLeft;
                    }
                }
                else if (touch.position.x > Screen.width / 2)
                {
                    if (touchState[touch.fingerId] == InputState.MovingRight ||
                        touchState[touch.fingerId] == InputState.MovingLeft)
                    {
                        touchState[touch.fingerId] = InputState.KeepMovingRight;
                    }
                    else
                    {
                        touchState[touch.fingerId] = InputState.TouchRight;
                    }
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
                if (Time.time - touchStartTime[touch.fingerId] > minHoldTime)
                {
                    if (touchState[touch.fingerId] == InputState.KeepMovingRight)
                    {
                        nextCommand = Commands.MoveRight;
                    }
                    else if (touchState[touch.fingerId] == InputState.KeepMovingLeft)
                    {
                        nextCommand = Commands.MoveLeft;
                    }
                    else if (touchState[touch.fingerId] == InputState.DragRight)
                    {
                        nextCommand = Commands.DragRight;
                    }
                    else if (touchState[touch.fingerId] == InputState.DragLeft)
                    {
                        nextCommand = Commands.DragLeft;
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                var deltaPosition = touch.position - touchStartPosition[touch.fingerId];
                // Check if touch moved above the 'touch and drag' treshold
                if (Mathf.Abs(deltaPosition.x) > minDragDistance ||
                    Mathf.Abs(deltaPosition.y) > minDragDistance)
                {
                    if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y))
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
                else
                {
                    // check on which side of the screen the tap occured
                    if (touchState[touch.fingerId] == InputState.TouchLeft ||
                        touchState[touch.fingerId] == InputState.KeepMovingLeft)
                    {
                        nextCommand = Commands.MoveLeft;
                        touchState[touch.fingerId] = InputState.MovingLeft;
                        StartCoroutine(doubleTapDelay(touch.fingerId, touchState[touch.fingerId]));
                    }
                    else if (touchState[touch.fingerId] == InputState.TouchRight ||
                             touchState[touch.fingerId] == InputState.KeepMovingRight)
                    {
                        nextCommand = Commands.MoveRight;
                        touchState[touch.fingerId] = InputState.MovingRight;
                        StartCoroutine(doubleTapDelay(touch.fingerId, touchState[touch.fingerId]));
                    }
                }
            }
            // Check for any queued commands and execute when possible
            if (!character.isMoving)
            {
                if (nextCommand == Commands.MoveRight)
                {
                    StartCoroutine(character.move(transform, 1, false));
                }
                else if (nextCommand == Commands.MoveLeft)
                {
                    StartCoroutine(character.move(transform, -1, false));
                }
                else if (nextCommand == Commands.ClimbUp)
                {
                    StartCoroutine(character.hang(transform, 1));
                }
                else if (nextCommand == Commands.HangDown)
                {
                    StartCoroutine(character.hang(transform, -1));
                }
                else if (nextCommand == Commands.DragLeft)
                {
                    StartCoroutine(character.move(transform, -1, true));
                }
                else if (nextCommand == Commands.DragRight)
                {
                    StartCoroutine(character.move(transform, 1, true));
                }
                else if (nextCommand == Commands.PullOut)
                {
                    StartCoroutine(character.pull(transform));
                }
                
                nextCommand = Commands.None;
            }
        }
	}

    IEnumerator doubleTapDelay(int i, InputState prevState)
    {
        yield return new WaitForSeconds(0.2F);

        if (touchState[i] == prevState)
        {
            touchState[i] = InputState.Done;
        }
    }
}
