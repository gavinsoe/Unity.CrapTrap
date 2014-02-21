using UnityEngine;
using System.Collections;

public class PlayerTouchController : MonoBehaviour {

    // Environment variables
    public float moveSpeed = 5f;
    private float gridSize = 1f;
	
    private bool isMoving = false;
    private bool isHanging = false;

    // Variables related to the touch controls
    private int maxTouches = 2;	// up to 5 (iOS only supports 5 apparently)
    private float minDragDistance = 25f; // Swipe distance before touch is regarded as 'touch and drag'
    private float minHoldTime = 0.15f; // Time before touch is regarded as 'touch and hold'

    enum InputState { TouchLeft, TouchRight, Left_DragLeft, Left_DragRight, Right_DragRight, Right_DragLeft, MovingRight, MovingLeft, Done };
    enum Commands { None, MoveLeft, MoveRight, Left_DragLeft, Left_DragRight, Right_DragRight, Right_DragLeft, HangDown, ClimbUp };
    private InputState[] touchState;
    private Vector2[] touchStartPosition;
    private float[] touchStartTime;
    private Commands nextCommand;

    // The component that handles the character animation.
    private Animator animator;

    // Use this for initialization
	void Start () {
        // Retrieve the animator component
        animator = GetComponentInChildren<Animator>();

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
            if (Debug.isDebugBuild) Debug.Log("[" + touch.fingerId + "] NextCommand : " + nextCommand.ToString());
            if (Debug.isDebugBuild) Debug.Log("[" + touch.fingerId + "] Phase : " + touch.phase.ToString() + " | Time : " + (Time.time - touchStartTime[touch.fingerId]));
            // When a finger touches the screen...
            if (touch.phase == TouchPhase.Began)
            {
                if (Debug.isDebugBuild) Debug.Log("[" + touch.fingerId + "] Touch detected at : " + touch.position.ToString());

                // Store data on where the user touched the screen, and the time of when it is pressed
                touchStartPosition[touch.fingerId] = touch.position;
                touchStartTime[touch.fingerId] = Time.time;

                if (touch.position.x < Screen.width / 2)
                {
                    touchState[touch.fingerId] = InputState.TouchLeft;
                } else if (touch.position.x > Screen.width / 2)
                {
                    touchState[touch.fingerId] = InputState.TouchRight;
                }


                nextCommand = Commands.None;

                /*
                // Get blocks on left and right of character (if any)
                Collider2D rightCollider = Physics2D.OverlapPoint(new Vector2(transform.position.x + gridSize, transform.position.y), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);
                Collider2D leftCollider = Physics2D.OverlapPoint(new Vector2(transform.position.x - gridSize, transform.position.y), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);

                // Cast a ray from the touch point to the world, and check if it collides with any boxes
                var ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                if (hit)
                {
                    if (hit.collider == rightCollider)
                    {
                        touchState[touch.fingerId] = InputState.DragRight;
                    }
                    else if (hit.collider == leftCollider)
                    {
                        touchState[touch.fingerId] = InputState.DragLeft;
                    }
                }*/
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
                        nextCommand = Commands.HangDown;
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
            if (nextCommand != Commands.None && !isMoving)
            {
                if (nextCommand == Commands.MoveRight)
                {
                    StartCoroutine(move(transform, 1, false, false));
                    touchState[touch.fingerId] = InputState.MovingRight;
                }
                else if (nextCommand == Commands.MoveLeft)
                {
                    StartCoroutine(move(transform, -1, false, false));
                    touchState[touch.fingerId] = InputState.MovingLeft;
                }
                else if (nextCommand == Commands.ClimbUp)
                {
                    StartCoroutine(hang(transform, 1));
                    touchState[touch.fingerId] = InputState.Done;
                }
                else if (nextCommand == Commands.HangDown)
                {
                    StartCoroutine(hang(transform, -1));
                    touchState[touch.fingerId] = InputState.Done;
                }
                else if (nextCommand == Commands.Left_DragLeft)
                {
                    StartCoroutine(move(transform, -1, true, false));
                    touchState[touch.fingerId] = InputState.Left_DragLeft;
                }
                else if (nextCommand == Commands.Left_DragRight)
                {
                    StartCoroutine(move(transform, 1, true, false));
                    touchState[touch.fingerId] = InputState.Left_DragRight;
                }
                else if (nextCommand == Commands.Right_DragLeft)
                {
                    StartCoroutine(move(transform, -1, false, true));
                    touchState[touch.fingerId] = InputState.Right_DragLeft;
                }
                else if (nextCommand == Commands.Right_DragRight)
                {
                    StartCoroutine(move(transform, 1, false, true));
                    touchState[touch.fingerId] = InputState.Right_DragRight;
                }

                nextCommand = Commands.None;
            }
        }
	}

    // Function to move the character
    public IEnumerator move(Transform transform, int sign, bool grabLeft, bool grabRight)
    {
        isMoving = true;
        bool stepUp = false;
	    bool hMove = false;
	    bool vMove = false;
        // Set the running animation
        //animator.SetBool("Running", true);	

        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition;
        float t = 0;

        Collider2D rightCollider = Physics2D.OverlapPoint(new Vector2(startPosition.x + gridSize, startPosition.y), 1 << 8, -0.9f, 0.9f);
        Collider2D rightUpCollider = Physics2D.OverlapPoint(new Vector2(startPosition.x + gridSize, startPosition.y + gridSize), 1 << 8, -0.9f, 0.9f);
        //Collider2D rightDownCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x + gridSize, startPosition.y - gridSize), 1 << 8, -0.9f, 0.9f);
        Collider2D leftCollider = Physics2D.OverlapPoint(new Vector2(startPosition.x - gridSize, startPosition.y), 1 << 8, -0.9f, 0.9f);
        Collider2D leftUpCollider = Physics2D.OverlapPoint(new Vector2(startPosition.x - gridSize, startPosition.y + gridSize), 1 << 8, -0.9f, 0.9f);
        //Collider2D leftDownCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x - gridSize, startPosition.y - gridSize), 1 << 8, -0.9f, 0.9f);
        Collider2D upCollider = Physics2D.OverlapPoint(new Vector2(startPosition.x, startPosition.y + gridSize), 1 << 8, -0.9f, 0.9f);


        if (!isHanging)
        {
            // If character is not grabbing on to any blocks, proceed to move
            if (!grabLeft && !grabRight)
            {
                // Make the character face the correct direction
                if (sign > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
                else transform.rotation = Quaternion.Euler(0, 180, 0);
                // Set destination depending on surrounding obstacles
                if (((rightCollider != null && rightUpCollider == null && sign > 0) || (leftCollider != null && leftUpCollider == null && sign < 0)) && upCollider == null)
                {
                    endPosition = new Vector3(startPosition.x + sign * gridSize, startPosition.y + gridSize, startPosition.z);
                    stepUp = true;
                    hMove = true;
                }
                else if ((rightCollider == null && sign > 0) || (leftCollider == null && sign < 0))
                {
                    endPosition = new Vector3(startPosition.x + sign * gridSize, startPosition.y, startPosition.z);
                    hMove = true;
                }
                else
                {
                    endPosition = startPosition;
                }

                if (Physics2D.OverlapPoint(new Vector2(startPosition.x + sign * gridSize, startPosition.y - gridSize * 2), 1 << 8, -0.9f, 0.9f) != null && startPosition != endPosition &&
                   Physics2D.OverlapPoint(new Vector2(startPosition.x + sign * gridSize, startPosition.y - gridSize), 1 << 8, -0.9f, 0.9f) == null && stepUp == false)
                {
                    endPosition.y -= gridSize;
                    vMove = true;
                }
            }

            // If player is grabbing on to left block
            if (grabLeft && leftCollider != null)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                if (!leftCollider.transform.gameObject.GetComponent<BlockController>().GetUnMovable())
                {
                    leftCollider.transform.gameObject.GetComponent<BlockController>().Moving();
                    Vector3 boxStart = leftCollider.transform.position;
                    Vector3 boxEnd = boxStart;

                    if (sign > 0)
                    {
                        if (rightCollider == null)
                        {
                            boxEnd.x += gridSize;
                            if (!hMove)
                            {
                                endPosition.x += gridSize;
                            }
                        }
                        else
                        {
                            endPosition = startPosition;
                        }

                    }
                    else
                    {
                        if (Physics2D.OverlapPoint(new Vector2(boxStart.x - gridSize, boxStart.y), 1 << 8, -0.9f, 0.9f) == null)
                        {
                            boxEnd.x -= gridSize;
                            if (stepUp)
                            {
                                endPosition.y -= gridSize;
                            }
                            if (!hMove)
                            {
                                endPosition.x -= gridSize;
                            }
                        }
                        else
                        {
                            endPosition = startPosition;
                        }

                    }
                    if (vMove)
                    {
                        endPosition.y = startPosition.y;
                    }
                    while (t < 1f)
                    {
                        t += Time.deltaTime * (moveSpeed / gridSize);
                        transform.position = Vector3.Lerp(startPosition, endPosition, t);
                        leftCollider.transform.gameObject.transform.position = Vector3.Lerp(boxStart, boxEnd, t);
                        yield return null;
                    }
                    leftCollider.transform.gameObject.GetComponent<BlockController>().NotMoving();
                }
            }
            else if (grabRight && rightCollider != null)
            { // If player is grabbing on to blocks on the right
                transform.rotation = Quaternion.Euler(0, 0, 0);
                if (!rightCollider.transform.gameObject.GetComponent<BlockController>().GetUnMovable())
                {
                    rightCollider.transform.gameObject.GetComponent<BlockController>().Moving();
                    Vector3 boxStart = rightCollider.transform.position;
                    Vector3 boxEnd = boxStart;
                    if (sign > 0)
                    {
                        if (Physics2D.OverlapPoint(new Vector2(boxStart.x + gridSize, boxStart.y), 1 << 8, -0.9f, 0.9f) == null)
                        {
                            boxEnd.x += gridSize;
                            if (stepUp)
                            {
                                endPosition.y -= gridSize;
                            }
                            if (!hMove)
                            {
                                endPosition.x += gridSize;
                            }
                        }
                        else
                        {
                            endPosition = startPosition;
                        }
                    }
                    else
                    {
                        if (leftCollider == null)
                        {
                            boxEnd.x -= gridSize;
                            if (!hMove)
                            {
                                endPosition.x -= gridSize;
                            }
                        }
                        else
                        {
                            endPosition = startPosition;
                        }
                    }
                    if (vMove)
                    {
                        endPosition.y = startPosition.y;
                    }
                    while (t < 1f)
                    {
                        t += Time.deltaTime * (moveSpeed / gridSize);
                        transform.position = Vector3.Lerp(startPosition, endPosition, t);
                        rightCollider.transform.gameObject.transform.position = Vector3.Lerp(boxStart, boxEnd, t);
                        yield return null;
                    }
                    rightCollider.transform.gameObject.GetComponent<BlockController>().NotMoving();
                }
            }
            else
            {
                while (t < 1f)
                {
                    
                    t += Time.deltaTime * (moveSpeed / gridSize);
                    transform.position = Vector3.Lerp(startPosition, endPosition, t);

                    yield return null;
                }
            }
        }
        else
        {
            if ((rightCollider != null && sign > 0 && rightCollider.transform.gameObject.GetComponent<BlockController>().GetHangable()) ||
               (leftCollider != null && sign < 0 && leftCollider.transform.gameObject.GetComponent<BlockController>().GetHangable()))
            {
                endPosition.x = endPosition.x + sign * gridSize;
            }
            else if ((rightCollider == null && sign > 0) || (leftCollider == null && sign < 0))
            {
                endPosition.x = endPosition.x + sign * gridSize;
                endPosition.y -= gridSize / 2;
                isHanging = false;
            }
            while (t < 1f)
            {

                t += Time.deltaTime * (moveSpeed / gridSize);
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }
        }

        startPosition = transform.position;
        Collider2D downCollider;
        while ((downCollider = Physics2D.OverlapPoint(new Vector2(startPosition.x, startPosition.y - gridSize), 1 << 8, -0.9f, 0.9f)) != null &&
                    downCollider.transform.gameObject.GetComponent<BlockController>().GetSlippery() &&
                    Physics2D.OverlapPoint(new Vector2(startPosition.x + gridSize * sign, startPosition.y), 1 << 8, -0.9f, 0.9f) == null &&
                    !isHanging)
        {
            Debug.Log("Slide");
            endPosition = startPosition;
            endPosition.x += gridSize * sign;
            t = 0;
            while (t < 1f)
            {

                t += Time.deltaTime * (moveSpeed / gridSize);
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }
            startPosition = transform.position;
        }

        while (Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - gridSize), 1 << 8, -0.9f, 0.9f) == null && !isHanging)
        {
            t = 0;
            startPosition = transform.position;
            endPosition = transform.position;
            endPosition.y -= gridSize;
            while (t < 1f)
            {

                t += Time.deltaTime * (moveSpeed / gridSize);
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }
        }

        isMoving = false;
        hMove = false;
        vMove = false;
        stepUp = false;
        //animator.SetBool("Running", false);	

        yield return 0;
    }

    // hang will be called then the down button is pressed
    public IEnumerator hang(Transform transform, int sign)
    {
        isMoving = true;
        
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition;
        float t = 0;

        Collider2D box = Physics2D.OverlapPoint(new Vector2(startPosition.x, startPosition.y - gridSize), 1 << 8, -0.1f, 0.9f);

        // if the character is not hanging and the down button is pushed: go to hanging on the block below
        if (!isHanging && sign < 0 && box.transform.gameObject.GetComponent<BlockController>().GetHangable())
        {
            endPosition.y -= gridSize / 2;
            isHanging = true;
            // if the character is hanging and the up button is pushed: climb up if there are no blocks in the way
        }
        else if (isHanging && sign > 0 && Physics2D.OverlapPoint(new Vector2(startPosition.x, startPosition.y + gridSize), 1 << 8, -0.9f, 0.9f) == null)
        {
            endPosition.y += gridSize / 2;
            isHanging = false;
        }
        while (t < 1f)
        {
            t += Time.deltaTime * (moveSpeed / gridSize);
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }
        isMoving = false;
        yield return 0;
    }
}
