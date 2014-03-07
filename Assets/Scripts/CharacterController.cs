﻿using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

    // Environment variables
    public float moveSpeed = 5f;
    private float gridSize = 1f;

    // Character States
    [HideInInspector]
    public bool isMoving = false;
    [HideInInspector]
    public bool isHanging = false;
    [HideInInspector]
    public bool isBurning = false;
    private bool wasBurning = false;
    //[HideInInspector]
    public bool reachedDestination = false;


    private MainGameController game;

    void Start()
    {
        game = Camera.main.GetComponent<MainGameController>();
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
    }

    // Function to move the character
    public IEnumerator move(Transform transform, int sign, bool grabLeft, bool grabRight)
    {
        // disable movement if destination has been reached
        if (reachedDestination) yield return null;

        // Set the movement flag on
        isMoving = true;
        bool stepUp = false;
        bool hMove = false;
        bool vMove = false;
        // Set the running animation
        //animator.SetBool("Running", true);	

        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition;
        float t = 0;

        Collider2D rightCollider = Physics2D.OverlapPoint(new Vector2(startPosition.x + gridSize, startPosition.y), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);
        Collider2D rightUpCollider = Physics2D.OverlapPoint(new Vector2(startPosition.x + gridSize, startPosition.y + gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);
        //Collider2D rightDownCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x + gridSize, startPosition.y - gridSize), 1 << 8, -0.9f, 0.9f);
        Collider2D leftCollider = Physics2D.OverlapPoint(new Vector2(startPosition.x - gridSize, startPosition.y), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);
        Collider2D leftUpCollider = Physics2D.OverlapPoint(new Vector2(startPosition.x - gridSize, startPosition.y + gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);
        //Collider2D leftDownCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x - gridSize, startPosition.y - gridSize), 1 << 8, -0.9f, 0.9f);
        Collider2D upCollider = Physics2D.OverlapPoint(new Vector2(startPosition.x, startPosition.y + gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);


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

    #region Old Code
    /*
    public IEnumerator jump(Transform transform) {
		isMoving = true;
		t = 0;
		Collider2D box;
		startPosition = transform.position;
		endPosition = startPosition;
		if((box = Physics2D.OverlapPoint (new Vector2 (startPosition.x, startPosition.y + gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.1f, 0.9f)) != null) {
			if(box.transform.gameObject.GetComponent<BlockController>().Hangable()) {
				endPosition.y += gridSize * 1.5f;
				while(t < 1f) {
					t += Time.deltaTime * (moveSpeed/gridSize);
					transform.position = Vector3.Lerp(startPosition, endPosition, t);
					yield return null;
				}
				hanging = true;
			}
		}
		isMoving = false;
		yield return 0;
	}

	// push is called when the push button is pressed
	public IEnumerator push(Transform transform) {
		isMoving = true;
		Collider2D box;
		t = 0;
		startPosition = transform.position;
		endPosition = startPosition;
		
		// if there is a block behind the character that can be pulled
		if((box = Physics2D.OverlapPoint (new Vector2 (startPosition.x, startPosition.y + gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.1f, 0.9f)) != null && !hanging) {
			if(box.transform.gameObject.GetComponent<BlockController>().Movable()) {
				endPosition.y += gridSize/2;
				while(t < 1f) {
					t += Time.deltaTime * (moveSpeed/gridSize);
					transform.position = Vector3.Lerp(startPosition, endPosition, t);
					yield return null;
				}
				Destroy(box.transform.gameObject);
				//box.transform.gameObject.GetComponent<BlockController>().Push();
				//endPosition.y += gridSize/2;
				//hanging = false;
				
				t = 0f;
				// move to hanging on the block below
				while(t < 1f) {
					t += Time.deltaTime * (moveSpeed/gridSize);
					transform.position = Vector3.Lerp(endPosition, startPosition, t);
					yield return null;
				}
			}
		}
		isMoving = false;
		yield return 0;
	}

	// pull is called when the pull button is pressed
	public IEnumerator pull(Transform transform) {
		isMoving = true;
		Collider2D box;
		t = 0;
		startPosition = transform.position;
		endPosition = startPosition;

		// if there is a block behind the character that can be pulled
		if((box = Physics2D.OverlapPoint (new Vector2 (startPosition.x, startPosition.y + gridSize / 2), 1 << LayerMask.NameToLayer("Terrain"), 0.1f, 1.9f)) != null && !hanging) {
			Collider2D boxDown = Physics2D.OverlapPoint (new Vector2 (startPosition.x, startPosition.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.1f, 0.9f);
			if(box.transform.gameObject.GetComponent<BlockController>().Movable() && 
			   			boxDown.transform.gameObject.GetComponent<BlockController>().Hangable()) {
				box.transform.gameObject.GetComponent<BlockController>().Pull();
				endPosition.y -= gridSize/2;
				hanging = true;

				// move to hanging on the block below
				while(t < 1f) {
					t += Time.deltaTime * (moveSpeed/gridSize);
					transform.position = Vector3.Lerp(startPosition, endPosition, t);
					yield return null;
				}
			}
		}
		isMoving = false;
		yield return 0;
	}

	// hang will be called then the down button is pressed
	public IEnumerator hang(Transform transform) {
		isMoving = true;
		var sign = System.Math.Sign (inputV);
		t = 0;
		startPosition = transform.position;
		endPosition = startPosition;

		Collider2D box = Physics2D.OverlapPoint (new Vector2 (startPosition.x, startPosition.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.1f, 0.9f);

		// if the character is not hanging and the down button is pushed: go to hanging on the block below
		if(!hanging && sign < 0 && box.transform.gameObject.GetComponent<BlockController>().Hangable()) {
			endPosition.y -= gridSize/2;
			hanging = true;
		// if the character is hanging and the up button is pushed: climb up if there are no blcoks in the way
		} else if(hanging && sign > 0 && Physics2D.OverlapPoint (new Vector2 (startPosition.x, startPosition.y + gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null) {
			endPosition.y += gridSize/2;
			hanging = false;
		}
		while(t < 1f) {
			t += Time.deltaTime * (moveSpeed/gridSize);
			transform.position = Vector3.Lerp(startPosition, endPosition, t);
			yield return null;
		}
		isMoving = false;
		yield return 0;
	}
	
	public IEnumerator move (Transform transform, int sign, bool grabLeft, bool grabRight){
		isMoving = true;
		bool stepUp = false;
		// Set the running animation
		//animator.SetBool("Running", true);	
		
		startPosition = transform.position;
		endPosition = startPosition;
		t = 0;
		
		Collider2D rightCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x + gridSize, startPosition.y), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);
		Collider2D rightUpCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x + gridSize, startPosition.y + gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);
		//Collider2D rightDownCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x + gridSize, startPosition.y - gridSize), 1 << 8, -0.9f, 0.9f);
        Collider2D leftCollider = Physics2D.OverlapPoint(new Vector2(startPosition.x - gridSize, startPosition.y), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);
		Collider2D leftUpCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x - gridSize, startPosition.y + gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);
		//Collider2D leftDownCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x - gridSize, startPosition.y - gridSize), 1 << 8, -0.9f, 0.9f);
		Collider2D upCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x, startPosition.y + gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f);
	
		
		if(!hanging) {
			// If character is not grabbing on to any blocks, proceed to move
			if (!grabLeft && !grabRight){
				// Make the character face the correct direction
				if (sign > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
				else transform.rotation = Quaternion.Euler(0, 180, 0);
				// Set destination depending on surrounding obstacles
				if (((rightCollider != null && rightUpCollider == null && sign > 0) || (leftCollider != null && leftUpCollider == null && sign < 0)) && upCollider == null) {
					endPosition = new Vector3(startPosition.x + sign * gridSize, startPosition.y + gridSize, startPosition.z);
					stepUp = true;
					hMove = true;
				} else if((rightCollider == null && sign > 0) || (leftCollider == null && sign < 0)){
					endPosition = new Vector3(startPosition.x + sign * gridSize, startPosition.y, startPosition.z);
					hMove = true;
				} else {
					endPosition = startPosition;
				}
	
				if(Physics2D.OverlapPoint (new Vector2 (startPosition.x + sign * gridSize, startPosition.y - gridSize * 2), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) != null && startPosition != endPosition &&
				   Physics2D.OverlapPoint(new Vector2(startPosition.x + sign * gridSize, startPosition.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null && stepUp == false) {
					endPosition.y -= gridSize;
					vMove = true;
				}
			}
			
			// If player is grabbing on to left block
			if(grabLeft && leftCollider != null) {
				transform.rotation = Quaternion.Euler(0, 180, 0);
				if(leftCollider.transform.gameObject.GetComponent<BlockController>().Movable()) {
					leftCollider.transform.gameObject.GetComponent<BlockController>().Moving();
					Vector3 boxStart = leftCollider.transform.position;
					Vector3 boxEnd = boxStart;

					if(sign > 0) {
						if(rightCollider == null) {
							boxEnd.x += gridSize;
							if(!hMove) {
								endPosition.x += gridSize;
							}
						} else {
							endPosition = startPosition;
						}

					} else {
						if(Physics2D.OverlapPoint(new Vector2(boxStart.x - gridSize, boxStart.y), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null) {
							boxEnd.x -= gridSize;
							if(stepUp) {
								endPosition.y -= gridSize;
							}
							if(!hMove) {
								endPosition.x -= gridSize;
							}
						} else {
							endPosition = startPosition;
						}

					}
					if(vMove) {
						endPosition.y = startPosition.y;
					}
					while(t < 1f) {
						t += Time.deltaTime * (moveSpeed/gridSize);
						transform.position = Vector3.Lerp(startPosition, endPosition, t);
						leftCollider.transform.gameObject.transform.position = Vector3.Lerp(boxStart, boxEnd, t);
						yield return null;
					}
					leftCollider.transform.gameObject.GetComponent<BlockController>().NotMoving();
				}
			} else if(grabRight && rightCollider != null) { // If player is grabbing on to blocks on the right
				transform.rotation = Quaternion.Euler(0, 0, 0);
				if(rightCollider.transform.gameObject.GetComponent<BlockController>().Movable()) {
					rightCollider.transform.gameObject.GetComponent<BlockController>().Moving();
					Vector3 boxStart = rightCollider.transform.position;
					Vector3 boxEnd = boxStart;
					if(sign > 0) {
						if(Physics2D.OverlapPoint(new Vector2(boxStart.x + gridSize, boxStart.y), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null) {
							boxEnd.x += gridSize;
							if(stepUp) {
								endPosition.y -= gridSize;
							}
							if(!hMove) {
								endPosition.x += gridSize;
							}
						} else {
							endPosition = startPosition;
						}
					} else {
						if(leftCollider == null) {
							boxEnd.x -= gridSize;
							if(!hMove) {
								endPosition.x -= gridSize;
							}
						} else {
							endPosition = startPosition;
						}
					}
					if(vMove) {
						endPosition.y = startPosition.y;
					}
					while(t < 1f) {
						t += Time.deltaTime * (moveSpeed/gridSize);
						transform.position = Vector3.Lerp(startPosition, endPosition, t);
						rightCollider.transform.gameObject.transform.position = Vector3.Lerp(boxStart, boxEnd, t);
						yield return null;
					}
					rightCollider.transform.gameObject.GetComponent<BlockController>().NotMoving();
				}
			} else {
				while (t < 1f) {
				
					t += Time.deltaTime * (moveSpeed/gridSize);
					transform.position = Vector3.Lerp(startPosition, endPosition, t);
					yield return null;
				}
			}
		} else {
			if((rightCollider != null && sign > 0 && rightCollider.transform.gameObject.GetComponent<BlockController>().Hangable()) || 
			   (leftCollider != null && sign < 0 && leftCollider.transform.gameObject.GetComponent<BlockController>().Hangable())) {
				endPosition.x = endPosition.x + sign * gridSize;
			} else if((rightCollider == null && sign > 0) || (leftCollider == null && sign < 0)) {
				endPosition.x = endPosition.x + sign * gridSize;
				endPosition.y -= gridSize/2;
				hanging = false;
			}
			while (t < 1f) {
				
				t += Time.deltaTime * (moveSpeed/gridSize);
				transform.position = Vector3.Lerp(startPosition, endPosition, t);
				yield return null;
			}
		}

		startPosition = transform.position;
		Collider2D downCollider;
		while((downCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x, startPosition.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f)) != null && 
		      		downCollider.transform.gameObject.GetComponent<BlockController>().Slippery() &&
		      		Physics2D.OverlapPoint(new Vector2(startPosition.x + gridSize * sign, startPosition.y), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null && 
		      		!hanging) {
			Debug.Log("Slide");
			endPosition = startPosition;
			endPosition.x += gridSize * sign;
			t = 0;
			while (t < 1f) {
				
				t += Time.deltaTime * (moveSpeed/gridSize);
				transform.position = Vector3.Lerp(startPosition, endPosition, t);
				yield return null;
			}
			startPosition = transform.position;
		}

		while(Physics2D.OverlapPoint (new Vector2 (transform.position.x, transform.position.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null && !hanging) {
			t = 0;
			startPosition = transform.position;
			endPosition = transform.position;
			endPosition.y -= gridSize;
			while (t < 1f) {
				
				t += Time.deltaTime * (moveSpeed/gridSize);
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
    */
    #endregion
}
