﻿using UnityEngine;
using System.Collections;

public class PlayerTouchControlsv2 : MonoBehaviour {
    
    /* GUI Skin
     * Custom Styles [0] = Push/Pull Button
     */
    public GUISkin activeSkin;

    #region GUI stuff

    private float dragBtnScale = 0.15f;
    private float dragBtnMargin = 0.04f;
    private Rect leftDragBtnRect;
    public Rect leftTouchArea;
    private Rect rightDragBtnRect;
    public Rect rightTouchArea;
    private GUIStyle leftDragBtnStyle;
    private GUIStyle rightDragBtnStyle;

    #endregion

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

        #region GUI

        leftDragBtnStyle = activeSkin.customStyles[0];
        rightDragBtnStyle = activeSkin.customStyles[0];

        float btnHeight = Screen.height * dragBtnScale;
        float btnWidth = btnHeight * ((float)leftDragBtnStyle.normal.background.width /
                                      (float)leftDragBtnStyle.normal.background.height);
        float btnMargin = Screen.height * dragBtnMargin;

        float touchAreaWidth = btnWidth * 2;
        float touchAreaHeight = btnHeight * 1.25f;

        leftDragBtnRect = new Rect(btnMargin, Screen.height - btnHeight - btnMargin,
                                   btnWidth, btnHeight);
        rightDragBtnRect = new Rect(Screen.width - btnWidth - btnMargin,
                                    Screen.height - btnHeight - btnMargin,
                                    btnWidth, btnHeight);
        leftTouchArea = new Rect(0, 0, touchAreaWidth, touchAreaHeight);
        rightTouchArea = new Rect(Screen.width - touchAreaWidth, 0, touchAreaWidth, touchAreaHeight);

        #endregion

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
                if (Debug.isDebugBuild) Debug.Log("[" + touch.fingerId + "] Touch detected at : " + touch.position.ToString());

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

                // check if buttons are pressed
                if (leftTouchArea.Contains(touch.position))
                {
                    nextCommand = Commands.DragLeft;
                }
                else if (rightTouchArea.Contains(touch.position))
                {
                    nextCommand = Commands.DragRight;
                }
                else
                {
                    // When a new touch comes in, reset the queued commands to none.
                    nextCommand = Commands.None;
                }
                character.wasMoving = false;

            }
            else if (touch.phase == TouchPhase.Moved)
            {
                var deltaPosition = touch.position - touchStartPosition[touch.fingerId];

                if (Mathf.Abs(deltaPosition.y) > Mathf.Abs(deltaPosition.x))
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
                else
                {
                    if (deltaPosition.x > minDragDistance)
                    {
                        touchState[touch.fingerId] = InputState.DragRight;
                        if (Input.touches.Length == 1)
                        {
                            nextCommand = Commands.MoveRight;
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
                            nextCommand = Commands.MoveLeft;
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

                        // check if buttons are pressed
                        if (leftTouchArea.Contains(touch.position))
                        {
                            nextCommand = Commands.DragLeft;
                        }
                        else if (rightTouchArea.Contains(touch.position))
                        {
                            nextCommand = Commands.DragRight;
                        }
                        else if (touchState[touch.fingerId] == InputState.TouchLeft ||
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
                    nextCommand = Commands.None;
                }
                else if (nextCommand == Commands.ZoomOut)
                {
                    main.ZoomOut();
                    nextCommand = Commands.None;
                }
                else if (nextCommand == Commands.MoveRight)
                {
                    StartCoroutine(character.move(transform, 1, false));
                    touchState[touch.fingerId] = InputState.MovingRight;
                    nextCommand = Commands.None;
                }
                else if (nextCommand == Commands.MoveLeft)
                {
                    StartCoroutine(character.move(transform, -1, false));
                    touchState[touch.fingerId] = InputState.MovingLeft;
                    nextCommand = Commands.None;
                }
                else if (nextCommand == Commands.ClimbUp && enableHanging)
                {
                    StartCoroutine(character.hang(transform, 1));
                    touchState[touch.fingerId] = InputState.Done;
                    nextCommand = Commands.None;
                }
                else if (nextCommand == Commands.HangDown && enableHanging)
                {
                    StartCoroutine(character.hang(transform, -1));
                    touchState[touch.fingerId] = InputState.Done;
                    nextCommand = Commands.None;
                }
                else if (nextCommand == Commands.DragLeft && enablePushPull)
                {
                    StartCoroutine(character.move(transform, -1, true));
                    touchState[touch.fingerId] = InputState.DragLeft;
                    nextCommand = Commands.None;
                }
                else if (nextCommand == Commands.DragRight && enablePushPull)
                {
                    StartCoroutine(character.move(transform, 1, true));
                    touchState[touch.fingerId] = InputState.DragRight;
                    nextCommand = Commands.None;
                }
                else if (nextCommand == Commands.PullOut)
                {
                    StartCoroutine(character.pull(transform));
                    touchState[touch.fingerId] = InputState.Done;
                    nextCommand = Commands.None;
                }
                
            }
        }
	}

    // GUI buttons
    void OnGUI()
    {
        GUI.depth = 0;

        GUI.skin = activeSkin;

        GUI.DrawTexture(leftDragBtnRect, leftDragBtnStyle.normal.background);
        GUI.DrawTexture(rightDragBtnRect, leftDragBtnStyle.normal.background);
    }
}
