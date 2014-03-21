using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

    // Environment variables
    public float moveSpeed = 5f;
    private float gridSize = 1f;

    // Character States
    //[HideInInspector]
    public bool isMoving = false;
    public bool isSlidingRight = false;
    public bool isSlidingLeft = false;
    //[HideInInspector]
    public bool isHanging = false;
    //[HideInInspector]
    public bool isBurning = false;
    private bool wasBurning = false;
    //[HideInInspector]
    public bool reachedDestination = false;

    private MainGameController game;
    private Animator animator;

    void Start()
    {
        game = Camera.main.GetComponent<MainGameController>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (reachedDestination && !isMoving)
        {
            // End game
            Time.timeScale = 0;
            Camera.main.GetComponent<GameCompletedGUI>().enabled = true;
        }
        if (isBurning && !wasBurning)
        {
            game.setTimerReductionRate(1.25f);
            wasBurning = true;
        }
        if (!isBurning && wasBurning)
        {
            game.setTimerReductionRate(1f);
            wasBurning = false;
        }
        // Check if character is falling
        if (!isMoving)
        {
            Collider2D bottomCollider = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);

            // Check if character is falling
            if (bottomCollider == null) StartCoroutine(freeFalling());

            // Check if character is on slippery ground
            if (isSlidingRight)
            {
                StartCoroutine(slidingRight());
            }
            if (isSlidingLeft)
            {
                StartCoroutine(slidingLeft());
            }
        }
    }

    // Function to move the character
    public IEnumerator move(Transform transform, int sign, bool grabLeft, bool grabRight)
    {
        // disable movement if destination has been reached
        if (reachedDestination) yield return null;

        // Set the running animation
        //animator.SetBool("Running", true);	

        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition;

        Collider2D rightCollider = Physics2D.OverlapPoint(new Vector2(startPosition.x + gridSize, startPosition.y), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);
        Collider2D rightUpCollider = Physics2D.OverlapPoint(new Vector2(startPosition.x + gridSize, startPosition.y + gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);
        Collider2D rightDownCollider = Physics2D.OverlapPoint(new Vector2(startPosition.x + gridSize, startPosition.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);
        Collider2D leftCollider = Physics2D.OverlapPoint(new Vector2(startPosition.x - gridSize, startPosition.y), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);
        Collider2D leftUpCollider = Physics2D.OverlapPoint(new Vector2(startPosition.x - gridSize, startPosition.y + gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);
        Collider2D leftDownCollider = Physics2D.OverlapPoint(new Vector2(startPosition.x - gridSize, startPosition.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);
        Collider2D upCollider = Physics2D.OverlapPoint(new Vector2(startPosition.x, startPosition.y + gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);
        Collider2D bottomCollider = Physics2D.OverlapPoint(new Vector2(startPosition.x, startPosition.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);
        
        if (!isHanging)
        {
            // If character is not grabbing on to any blocks (just moving)
            if (!grabLeft && !grabRight)
            {
                if (rightCollider == null && sign > 0)
                {
                    if (rightDownCollider == null)
                    {
                        StartCoroutine(fallRight());
                    }
                    else
                    {
                        StartCoroutine(moveRight());
                    }
                }
                else if (leftCollider == null && sign < 0)
                {
                    if (leftDownCollider == null)
                    {
                        StartCoroutine(fallLeft());
                    }
                    else
                    {
                        StartCoroutine(moveLeft());
                    }
                }
                else if (rightCollider != null && rightUpCollider == null && upCollider == null && sign > 0)
                {
                    StartCoroutine(climbRight());
                }
                else if (leftCollider != null && leftUpCollider == null && upCollider == null && sign < 0)
                {
                    StartCoroutine(climbLeft());
                }
            }
            // If player is grabbing on to right block
            else if (grabRight && rightCollider != null)
            {
                BlockController block = rightCollider.transform.gameObject.GetComponent<BlockController>();
                if (rightCollider.transform.gameObject.GetComponent<BlockController>().Movable())
                {
                    if (sign < 0)
                    {
                        if (leftCollider == null) StartCoroutine(pullRight(block));
                    }
                    else
                    {
                        if (Physics2D.OverlapPoint(new Vector2(block.transform.position.x + gridSize, block.transform.position.y), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null)
                        {
                            StartCoroutine(pushRight(block));
                        }
                    }
                }
            }
            // If player is grabbing on to left block
            else if (grabLeft && leftCollider != null)
            {
                BlockController block = leftCollider.transform.gameObject.GetComponent<BlockController>();
                if (leftCollider.transform.gameObject.GetComponent<BlockController>().Movable())
                {
                    if (sign < 0)
                    {
                        if (Physics2D.OverlapPoint(new Vector2(block.transform.position.x - gridSize, block.transform.position.y), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null)
                        {
                            StartCoroutine(pushLeft(block));
                        }
                    }
                    else
                    {
                        if (rightCollider == null)
                        {
                            StartCoroutine(pullLeft(block));
                        }
                    }
                }
            }
        }
        else
        {
            if ( rightCollider != null && sign > 0 && rightCollider.transform.gameObject.GetComponent<BlockController>().Hangable() )
            {
                StartCoroutine(hangRight());
            }
            else if ( leftCollider != null && sign < 0 && leftCollider.transform.gameObject.GetComponent<BlockController>().Hangable() )
            {
                StartCoroutine(hangLeft());
            }
            else if (rightCollider == null && sign > 0)
            {
                StartCoroutine(fallRight());
            }
            else if (leftCollider == null && sign < 0)
            {
                StartCoroutine(fallLeft());
            }
        }

        #region Original Code
        /*
        if (!isHanging)
        {
            // If character is not grabbing on to any blocks, proceed to move
            if (!grabLeft && !grabRight)
            {
                // Make the character face the correct direction
                if (sign > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
                else transform.rotation = Quaternion.Euler(0, 180, 0);
                // Set destination depending on surrounding obstacles
                if (((rightCollider != null && rightUpCollider == null && sign > 0) ||
                     (leftCollider != null && leftUpCollider == null && sign < 0)) && upCollider == null)
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

                if (Physics2D.OverlapPoint(new Vector2(startPosition.x + sign * gridSize, startPosition.y - gridSize * 2), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) != null && startPosition != endPosition &&
                   Physics2D.OverlapPoint(new Vector2(startPosition.x + sign * gridSize, startPosition.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null && stepUp == false)
                {
                    endPosition.y -= gridSize;
                    vMove = true;
                }
            }

            // If player is grabbing on to left block
            if (grabLeft && leftCollider != null)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                if (leftCollider.transform.gameObject.GetComponent<BlockController>().Movable())
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
                        if (Physics2D.OverlapPoint(new Vector2(boxStart.x - gridSize, boxStart.y), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null)
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
                if (rightCollider.transform.gameObject.GetComponent<BlockController>().Movable())
                {
                    rightCollider.transform.gameObject.GetComponent<BlockController>().Moving();
                    Vector3 boxStart = rightCollider.transform.position;
                    Vector3 boxEnd = boxStart;
                    if (sign > 0)
                    {
                        if (Physics2D.OverlapPoint(new Vector2(boxStart.x + gridSize, boxStart.y), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null)
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
            if ((rightCollider != null && sign > 0 && rightCollider.transform.gameObject.GetComponent<BlockController>().Hangable()) ||
               (leftCollider != null && sign < 0 && leftCollider.transform.gameObject.GetComponent<BlockController>().Hangable()))
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
        while ((downCollider = Physics2D.OverlapPoint(new Vector2(startPosition.x, startPosition.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f)) != null &&
                downCollider.transform.gameObject.GetComponent<BlockController>().Slippery() &&
                Physics2D.OverlapPoint(new Vector2(startPosition.x + gridSize * sign, startPosition.y), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null &&
                !isHanging)
        {
            if (Debug.isDebugBuild) Debug.Log("Slide");
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

        while (Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null && !isHanging)
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
        */
        #endregion
    }

    // pull out a block
    public IEnumerator pull(Transform transform)
    {
        // disable movement if destination has been reached
        if (reachedDestination) yield return null;

        // Set the movement flag on
        isMoving = true;

        Collider2D box;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition;
        float t = 0;

        // Check for boxes behind the character that can be pulled out.
        if ((box = Physics2D.OverlapPoint(new Vector2(startPosition.x, startPosition.y + gridSize / 2), 1 << LayerMask.NameToLayer("Terrain"), 0.1f, 1.9f)) != null && !isHanging)
        {
            Collider2D boxDown = Physics2D.OverlapPoint(new Vector2(startPosition.x, startPosition.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.1f, 0.9f);
            if (box.transform.gameObject.GetComponent<BlockController>().Movable() &&
                boxDown.transform.gameObject.GetComponent<BlockController>().Hangable())
            {
                box.transform.gameObject.GetComponent<BlockController>().Pull();
                endPosition.y -= gridSize / 2;
                isHanging = true;

                // move to hanging on the block below
                while (t < 1f)
                {
                    t += Time.deltaTime * (moveSpeed / gridSize);
                    transform.position = Vector3.Lerp(startPosition, endPosition, t);
                    yield return null;
                }
            }
        }

        isMoving = false;
        yield return 0;
    }

    // hang will be called then the down button is pressed
    public IEnumerator hang(Transform transform, int sign)
    {
        // disable movement if destination has been reached
        if (reachedDestination) yield return null;

        // Set the movement flag on
        isMoving = true;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition;
        float t = 0;

        Collider2D box = Physics2D.OverlapPoint(new Vector2(startPosition.x, startPosition.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.1f, 0.9f);

        // if the character is not hanging and the down button is pushed: go to hanging on the block below
        if (!isHanging && sign < 0 && box.transform.gameObject.GetComponent<BlockController>().Hangable())
        {
            endPosition.y -= gridSize / 2;
            isHanging = true;
            // if the character is hanging and the up button is pushed: climb up if there are no blocks in the way
        }
        else if (isHanging && sign > 0 && Physics2D.OverlapPoint(new Vector2(startPosition.x, startPosition.y + gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null)
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

    #region Animations

    public IEnumerator moveRight()
    {
        // disable movement if destination has been reached
        if (reachedDestination) yield return null;

        // Set the movement flag on
        isMoving = true;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(transform.position.x + gridSize, transform.position.y, transform.position.z);
        float t = 0;

        // Face the character the correct way
        transform.rotation = Quaternion.Euler(0, 0, 0);
        
        //Set Animator Variables
        animator.SetBool("isRunning", true);
        animator.SetBool("isPushing", false);
        animator.SetBool("isPulling", false);

        Collider2D downCollider;
        while (t < 1f)
        {
            // Check if character is no longer on a slippery block and change animation accordingly
            downCollider = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);
            if (!isSlidingRight && downCollider != null && downCollider.transform.gameObject.GetComponent<BlockController>().Slippery())
            {
                if (Debug.isDebugBuild) Debug.Log("Sliding Right : " + transform.position.ToString());
                animator.SetBool("isSliding", true);
                isSlidingRight = true;
            }

            t += Time.deltaTime * (moveSpeed / gridSize);
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        isMoving = false;   
    }

    public IEnumerator moveLeft()
    {
        // disable movement if destination has been reached
        if (reachedDestination) yield return null;

        // Set the movement flag on
        isMoving = true;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(transform.position.x - gridSize, transform.position.y, transform.position.z);
        float t = 0;

        // Face the character the correct way
        transform.rotation = Quaternion.Euler(0, 180, 0);

        // Animate
        animator.SetBool("isRunning", true);
        animator.SetBool("isPushing", false);
        animator.SetBool("isPulling", false);

        Collider2D downCollider;
        while (t < 1f)
        {
            // Check if character is no longer on a slippery block and change animation accordingly
            downCollider = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);
            if (!isSlidingLeft && downCollider != null && downCollider.transform.gameObject.GetComponent<BlockController>().Slippery())
            {
                if (Debug.isDebugBuild) Debug.Log("Sliding Left : " + transform.position.ToString());
                animator.SetBool("isSliding", true);
                isSlidingLeft = true;
            }

            t += Time.deltaTime * (moveSpeed / gridSize);
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        isMoving = false;
    }

    public IEnumerator climbRight()
    {
        // disable movement if destination has been reached
        if (reachedDestination) yield return null;

        // Set the movement flag on
        isMoving = true;

        Vector3 startPosition = transform.position;
        Vector3 midPosition = new Vector3(transform.position.x + gridSize/2, transform.position.y + gridSize, transform.position.z);
        Vector3 endPosition = new Vector3(transform.position.x + gridSize, transform.position.y + gridSize, transform.position.z);
        float t = 0;

        // Face the character the correct way
        transform.rotation = Quaternion.Euler(0, 0, 0);

        // Animate
        animator.SetBool("isRunning", true);

        while (t < 1f)
        {
            t += Time.deltaTime * (2 * moveSpeed / gridSize);
            transform.position = Vector3.Lerp(startPosition, midPosition, t);
            yield return null;
        }

        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * (2 * moveSpeed / gridSize);
            transform.position = Vector3.Lerp(midPosition, endPosition, t);
            yield return null;
        }

        animator.SetBool("isRunning", false);
        isMoving = false;
    }

    public IEnumerator climbLeft()
    {
        // disable movement if destination has been reached
        if (reachedDestination) yield return null;

        // Set the movement flag on
        isMoving = true;

        Vector3 startPosition = transform.position;
        Vector3 midPosition = new Vector3(transform.position.x - gridSize / 2, transform.position.y + gridSize, transform.position.z);
        Vector3 endPosition = new Vector3(transform.position.x - gridSize, transform.position.y + gridSize, transform.position.z);
        float t = 0;

        // Face the character the correct way
        transform.rotation = Quaternion.Euler(0, 180, 0);

        // Animate
        animator.SetBool("isRunning", true);

        while (t < 1f)
        {
            t += Time.deltaTime * (2 * moveSpeed / gridSize);
            transform.position = Vector3.Lerp(startPosition, midPosition, t);
            yield return null;
        }

        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * (2 * moveSpeed / gridSize);
            transform.position = Vector3.Lerp(midPosition, endPosition, t);
            yield return null;
        }

        animator.SetBool("isRunning", false);
        isMoving = false;
    }

    public IEnumerator hangRight()
    {
        // disable movement if destination has been reached
        if (reachedDestination) yield return null;

        // Set the movement flag on
        isMoving = true;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(transform.position.x + gridSize, transform.position.y, transform.position.z);
        float t = 0;

        // Face the character the correct way
        transform.rotation = Quaternion.Euler(0, 0, 0);

        //Animate
        animator.SetBool("isRunning", true);

        while (t < 1f)
        {
            t += Time.deltaTime * (moveSpeed / gridSize);
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        animator.SetBool("isRunning", false);
        isMoving = false;
    }

    public IEnumerator hangLeft()
    {
        // disable movement if destination has been reached
        if (reachedDestination) yield return null;

        // Set the movement flag on
        isMoving = true;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(transform.position.x - gridSize, transform.position.y, transform.position.z);
        float t = 0;

        // Face the character the correct way
        transform.rotation = Quaternion.Euler(0, 180, 0);

        // Animate
        animator.SetBool("isRunning", true);

        while (t < 1f)
        {
            t += Time.deltaTime * (moveSpeed / gridSize);
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        animator.SetBool("isRunning", false);
        isMoving = false;
    }

    public IEnumerator fallRight()
    {
        // disable movement if destination has been reached
        if (reachedDestination) yield return null;

        // Set the movement flag on
        isMoving = true;

        Vector3 startPosition = transform.position;
        Vector3 midPosition = new Vector3(transform.position.x + gridSize / 2, transform.position.y, transform.position.z);
        Vector3 endPosition = new Vector3(transform.position.x + gridSize, transform.position.y - gridSize, transform.position.z);
        if (isHanging)
        {
            endPosition = new Vector3(transform.position.x + gridSize, transform.position.y - gridSize / 2, transform.position.z);
        }
        float t = 0;

        // Face the character the correct way
        transform.rotation = Quaternion.Euler(0, 0, 0);

        //Animate
        animator.SetBool("isRunning", true);

        while (t < 1f)
        {
            t += Time.deltaTime * (2 * moveSpeed / gridSize);
            transform.position = Vector3.Lerp(startPosition, midPosition, t);
            yield return null;
        }

        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * (2 * moveSpeed / gridSize);
            transform.position = Vector3.Lerp(midPosition, endPosition, t);
            yield return null;
        }

        // Check if character is stepping into slippery block
        var downCollider = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);
        if (downCollider != null && downCollider.transform.gameObject.GetComponent<BlockController>().Slippery())
        {
            if (Debug.isDebugBuild) Debug.Log("Sliding Right : " + transform.position.ToString());
            animator.SetBool("isSliding", true);
            isSlidingRight = true;
        }

        animator.SetBool("isRunning", false);

        isHanging = false;
        isMoving = false;
    }

    public IEnumerator fallLeft()
    {
        // disable movement if destination has been reached
        if (reachedDestination) yield return null;

        // Set the movement flag on
        isMoving = true;

        Vector3 startPosition = transform.position;
        Vector3 midPosition = new Vector3(transform.position.x - gridSize / 2, transform.position.y, transform.position.z);
        Vector3 endPosition = new Vector3(transform.position.x - gridSize, transform.position.y - gridSize, transform.position.z);
        if (isHanging)
        {
            endPosition = new Vector3(transform.position.x - gridSize, transform.position.y - gridSize / 2, transform.position.z);
        }
        float t = 0;

        // Face the character the correct way
        transform.rotation = Quaternion.Euler(0, 180, 0);

        //Animate
        animator.SetBool("isRunning", true);

        while (t < 1f)
        {
            t += Time.deltaTime * (2 * moveSpeed / gridSize);
            transform.position = Vector3.Lerp(startPosition, midPosition, t);
            yield return null;
        }

        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * (2 * moveSpeed / gridSize);
            transform.position = Vector3.Lerp(midPosition, endPosition, t);
            yield return null;
        }

        // Check if character is stepping into slippery block
        var downCollider = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);
        if (downCollider != null && downCollider.transform.gameObject.GetComponent<BlockController>().Slippery())
        {
            if (Debug.isDebugBuild) Debug.Log("Sliding Left : " + transform.position.ToString());
            animator.SetBool("isSliding", true);
            isSlidingLeft = true;
        }

        animator.SetBool("isRunning", false);
        
        isHanging = false;
        isMoving = false;
    }

    public IEnumerator freeFalling()
    {
        // disable movement if destination has been reached
        if (reachedDestination) yield return null;

        // Set the movement flag on
        isMoving = true;

        // Keep on falling if no ground underneath
        while (Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null && !isHanging)
        {
            float t = 0;
            Vector3 startPosition = transform.position;
            Vector3 endPosition = transform.position;
            endPosition.y -= gridSize;
            while (t < 1f)
            {
                t += Time.deltaTime * (moveSpeed / gridSize);
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }
        }

        isMoving = false;
    }

    public IEnumerator slidingRight()
    {
        // disable movement if destination has been reached
        if (reachedDestination) yield return null;

        // Set the movement flag on
        isMoving = true;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(transform.position.x + gridSize, transform.position.y, transform.position.z);
        float t = 0;

        // Face the character the correct way
        transform.rotation = Quaternion.Euler(0, 0, 0);

        // Animate
        // Keep on moving (or sliding) if on slippery block.
        Collider2D downCollider;
        while ((downCollider = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f)) != null &&
               downCollider.gameObject.GetComponent<BlockController>().Slippery() &&
               Physics2D.OverlapPoint(new Vector2(transform.position.x + gridSize, transform.position.y), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null)
        {
            startPosition = transform.position;
            endPosition = new Vector3(transform.position.x + gridSize, transform.position.y, transform.position.z);
            t = 0;
            while (t < 1f)
            {
                t += Time.deltaTime * (moveSpeed / gridSize);
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }
        }

        if (Debug.isDebugBuild) Debug.Log("STOP Sliding Right : " + transform.position.ToString());
        animator.SetBool("isSliding", false);
        isSlidingRight = false;

        isMoving = false;
    }

    public IEnumerator slidingLeft()
    {
        // disable movement if destination has been reached
        if (reachedDestination) yield return null;

        // Set the movement flag on
        isMoving = true;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(transform.position.x + gridSize, transform.position.y, transform.position.z);
        float t = 0;

        // Face the character the correct way
        transform.rotation = Quaternion.Euler(0, 180, 0);

        // Animate
        // Keep on moving (or sliding) if on slippery block.
        Collider2D downCollider;
        while ((downCollider = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f)) != null &&
               downCollider.gameObject.GetComponent<BlockController>().Slippery() &&
               Physics2D.OverlapPoint(new Vector2(transform.position.x - gridSize, transform.position.y), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null)
        {
            startPosition = transform.position;
            endPosition = new Vector3(transform.position.x - gridSize, transform.position.y, transform.position.z);
            t = 0;
            while (t < 1f)
            {
                t += Time.deltaTime * (moveSpeed / gridSize);
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }
        }

        if (Debug.isDebugBuild) Debug.Log("STOP Sliding Left : " + transform.position.ToString());
        animator.SetBool("isSliding", false);
        isSlidingLeft = false;

        isMoving = false;
    }

    public IEnumerator pushRight(BlockController block)
    {
        // disable movement if destination has been reached
        if (reachedDestination) yield return null;

        // Set the movement flag on (for both character and block)
        isMoving = true;
        block.Moving();

        // Calculate where character will be
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(transform.position.x + gridSize, transform.position.y, transform.position.z);
        float t = 0;

        // Calculate where box will be
        Vector3 boxStart = block.transform.position;
        Vector3 boxEnd = new Vector3(boxStart.x + gridSize, boxStart.y, boxStart.z);

        // Face the character the correct way
        transform.rotation = Quaternion.Euler(0, 0, 0);

        //Animate
        animator.SetBool("isPushing", true);
        animator.SetBool("isPulling", false);

        while (t < 1f)
        {
            t += Time.deltaTime * (moveSpeed / gridSize);
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            block.transform.position = Vector3.Lerp(boxStart, boxEnd, t);
            yield return null;
        }

        isMoving = false;
        block.NotMoving();
    }

    public IEnumerator pushLeft(BlockController block)
    {
        // disable movement if destination has been reached
        if (reachedDestination) yield return null;

        // Set the movement flag on (for both character and block)
        isMoving = true;
        block.Moving();

        // Calculate where character will be
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(transform.position.x - gridSize, transform.position.y, transform.position.z);
        float t = 0;

        // Calculate where box will be
        Vector3 boxStart = block.transform.position;
        Vector3 boxEnd = new Vector3(boxStart.x - gridSize, boxStart.y, boxStart.z);

        // Face the character the correct way
        transform.rotation = Quaternion.Euler(0, 180, 0);

        //Animate
        animator.SetBool("isPushing", true);
        animator.SetBool("isPulling", false);

        while (t < 1f)
        {
            t += Time.deltaTime * (moveSpeed / gridSize);
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            block.transform.position = Vector3.Lerp(boxStart, boxEnd, t);
            yield return null;
        }

        isMoving = false;
        block.NotMoving();
    }

    public IEnumerator pullRight(BlockController block)
    {
        // disable movement if destination has been reached
        if (reachedDestination) yield return null;

        // Set the movement flag on (for both character and block)
        isMoving = true;
        block.Moving();

        // Calculate where character will be
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(transform.position.x - gridSize, transform.position.y, transform.position.z);
        float t = 0;

        // Calculate where box will be
        Vector3 boxStart = block.transform.position;
        Vector3 boxEnd = new Vector3(boxStart.x - gridSize, boxStart.y, boxStart.z);

        // Face the character the correct way
        transform.rotation = Quaternion.Euler(0, 0, 0);

        //Animate
        animator.SetBool("isPulling", true);
        animator.SetBool("isPushing", false);

        while (t < 1f)
        {
            t += Time.deltaTime * (moveSpeed / gridSize);
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            block.transform.position = Vector3.Lerp(boxStart, boxEnd, t);
            yield return null;
        }

        isMoving = false;
        block.NotMoving();
    }

    public IEnumerator pullLeft(BlockController block)
    {
        // disable movement if destination has been reached
        if (reachedDestination) yield return null;

        // Set the movement flag on (for both character and block)
        isMoving = true;
        block.Moving();

        // Calculate where character will be
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(transform.position.x + gridSize, transform.position.y, transform.position.z);
        float t = 0;

        // Calculate where box will be
        Vector3 boxStart = block.transform.position;
        Vector3 boxEnd = new Vector3(boxStart.x + gridSize, boxStart.y, boxStart.z);

        // Face the character the correct way
        transform.rotation = Quaternion.Euler(0, 180, 0);

        //Animate
        animator.SetBool("isPulling", true);
        animator.SetBool("isPushing", false);

        while (t < 1f)
        {
            t += Time.deltaTime * (moveSpeed / gridSize);
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            block.transform.position = Vector3.Lerp(boxStart, boxEnd, t);
            yield return null;
        }

        isMoving = false;
        block.NotMoving();
    }

    public void idle()
    {
        animator.SetBool("isRunning", false);
        animator.SetBool("isPushing", false);
        animator.SetBool("isPulling", false);
    }
    #endregion

}
