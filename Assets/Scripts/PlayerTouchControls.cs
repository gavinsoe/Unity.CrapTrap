﻿using UnityEngine;
using System.Collections;

public class PlayerTouchControls : MonoBehaviour {

     // Variables related to the touch controls
    private int maxTouches = 2;	// up to 5 (iOS only supports 5 apparently)
    private float minDragDistance = 50f; // Swipe distance before touch is regarded as 'touch and drag'
    private float minHoldTime = 0.1f; // Time before touch is regarded as 'touch and hold'

    public enum InputState { TouchLeft, TouchRight, DragLeft, DragRight, SwipeDown, MovingRight, MovingLeft, Done };
    public enum Commands { None, MoveLeft, MoveRight, DragLeft, DragRight, HangDown, ClimbUp, PullOut, ZoomOut, ZoomIn };
    public InputState[] touchState;
    private Vector2[] touchStartPosition;
    private float[] touchStartTime;
    public Commands nextCommand;

    // Enables / Disables some type of movements
    public bool enableHanging = true;
    public bool enablePushPull = true;

    // The component that handles all the character movement.
    private CharacterController character;
    private MainGameController main;

    // Use this for initialization
	void Start () {

        // Retrieve the characterController
        character = GetComponent<CharacterController>();
        main = Camera.main.GetComponent<MainGameController>();

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainGameController.instance.PauseGame();
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

            else if (touch.phase == TouchPhase.Moved)
            {
                var deltaPosition = touch.position - touchStartPosition[touch.fingerId];

                if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y))
                {
                    if (deltaPosition.x > minDragDistance)
                    {
                        touchState[touch.fingerId] = InputState.DragRight;
                        if (Input.touches.Length == 1)
                        {
                            nextCommand = Commands.DragRight;
                        }
                        else if (Input.touches.Length == 2)
                        {
                            if (touch.fingerId == 0)
                            {
                                if (touchState[1] == InputState.DragLeft)
                                {
                                    if (touchStartPosition[0].x > touchStartPosition[1].x)
                                    {
                                        nextCommand = Commands.ZoomIn;
                                    }
                                    else if (touchStartPosition[0].x < touchStartPosition[1].x)
                                    {
                                        nextCommand = Commands.ZoomOut;
                                    }
                                }
                            }
                            else if (touch.fingerId == 1)
                            {
                                if (touchState[0] == InputState.DragLeft)
                                {
                                    if (touchStartPosition[0].x > touchStartPosition[1].x)
                                    {
                                        nextCommand = Commands.ZoomOut;
                                    }
                                    else if (touchStartPosition[0].x < touchStartPosition[1].x)
                                    {
                                        nextCommand = Commands.ZoomIn;
                                    }
                                }
                            }
                        }
                    }
                    else if (deltaPosition.x < -minDragDistance)
                    {
                        touchState[touch.fingerId] = InputState.DragLeft;
                        if (Input.touches.Length == 1)
                        {
                            nextCommand = Commands.DragLeft;
                        }
                        else if (Input.touches.Length == 2)
                        {
                            if (touch.fingerId == 0)
                            {
                                if (touchState[1] == InputState.DragRight)
                                {
                                    if (touchStartPosition[0].x > touchStartPosition[1].x)
                                    {
                                        nextCommand = Commands.ZoomOut;
                                    }
                                    else if (touchStartPosition[0].x < touchStartPosition[1].x)
                                    {
                                        nextCommand = Commands.ZoomIn;
                                    }
                                }
                            }
                            else if (touch.fingerId == 1)
                            {
                                if (touchState[0] == InputState.DragRight)
                                {
                                    if (touchStartPosition[0].x > touchStartPosition[1].x)
                                    {
                                        nextCommand = Commands.ZoomIn;
                                    }
                                    else if (touchStartPosition[0].x < touchStartPosition[1].x)
                                    {
                                        nextCommand = Commands.ZoomOut;
                                    }
                                }
                            }
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

            else if (touch.phase == TouchPhase.Stationary)
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
                        if (touchState[touch.fingerId] == InputState.TouchLeft ||
                            touchState[touch.fingerId] == InputState.TouchRight ||
                            touchState[touch.fingerId] == InputState.MovingLeft ||
                            touchState[touch.fingerId] == InputState.MovingRight)
                        {
                            if (touch.position.x < Screen.width / 2)
                            {
                                nextCommand = Commands.MoveLeft;
                                touchState[touch.fingerId] = InputState.TouchLeft;
                            }
                            else if (touch.position.x > Screen.width / 2)
                            {
                                nextCommand = Commands.MoveRight;
                                touchState[touch.fingerId] = InputState.TouchRight;
                            }
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
            }

            else if (touch.phase == TouchPhase.Ended)
            {
                if (Input.touches.Length == 1)
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
            }

            // Check for any queued commands and execute when possible
            if (!character.isMoving)
            {
                if (nextCommand == Commands.ZoomIn)
                {
                    main.ZoomIn();
                }
                else if (nextCommand == Commands.ZoomOut)
                {
                    main.ZoomOut();
                }
                else if (nextCommand == Commands.MoveRight)
                {
                    StartCoroutine(character.move(transform, 1, false));
                    touchState[touch.fingerId] = InputState.MovingRight;
                }
                else if (nextCommand == Commands.MoveLeft)
                {
                    StartCoroutine(character.move(transform, -1, false));
                    touchState[touch.fingerId] = InputState.MovingLeft;
                }
                else if (nextCommand == Commands.ClimbUp && enableHanging)
                {
                    StartCoroutine(character.hang(transform, 1));
                    touchState[touch.fingerId] = InputState.Done;
                }
                else if (nextCommand == Commands.HangDown && enableHanging)
                {
                    StartCoroutine(character.hang(transform, -1));
                    touchState[touch.fingerId] = InputState.Done;
                }
                else if (nextCommand == Commands.DragLeft && enablePushPull)
                {
                    StartCoroutine(character.move(transform, -1, true));
                    touchState[touch.fingerId] = InputState.DragLeft;
                }
                else if (nextCommand == Commands.DragRight && enablePushPull)
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
