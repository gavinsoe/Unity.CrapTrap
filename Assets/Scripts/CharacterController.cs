﻿using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {
	
	private float moveSpeed = 2f;
	private float gridSize = 1f;
	private enum Orientation {
		Horizontal,
		Vertical
	};
	private float input;
	private bool isMoving = false;
	private Vector3 startPosition;
	private Vector3 endPosition;
	private float t;
	
	private Transform rightDetector;
	private bool rightBlock = false;
	private Transform leftDetector;
	private bool leftBlock = false;
	private bool hanging = false;
	
	protected Animator animator;
	
	void Awake() {
		// Setting up references
		rightDetector = transform.Find ("Right Detector");
		leftDetector = transform.Find ("Left Detector");
	}
	
	void Start(){
		animator = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		// Checks whether the block to the right is climbable
		rightBlock = Physics2D.Linecast(transform.position, rightDetector.position, 1 << LayerMask.NameToLayer("Terrain"));
		leftBlock  = Physics2D.Linecast(transform.position, leftDetector.position, 1 << LayerMask.NameToLayer("Terrain"));
		
		if (!isMoving) {
			input = Input.GetAxis("Horizontal");
			
			if (input != 0){
				StartCoroutine(move(transform));
			}
		}
	}
	
	public IEnumerator move (Transform transform){
		isMoving = true;
		bool stepUp = false;
		// Set the running animation
		animator.SetBool("Running", true);	
		
		startPosition = transform.position;
		t = 0;
		var sign = System.Math.Sign(input);
		/*
		if (hanging){
			if (sign > 0 && rightBlock || sign < 0 && leftBlock){
				endPosition = new Vector3(startPosition.x + sign * (gridSize/2), startPosition.y + (gridSize/2), startPosition.z);
				hanging = false;
			} else {
				endPosition = new Vector3(startPosition.x + sign * (gridSize/2), startPosition.y - (gridSize/2), startPosition.z);
				hanging = false;
			}
		} else {
			if (sign > 0 && rightBlock || sign < 0 && leftBlock){
				endPosition = new Vector3(startPosition.x + sign * (gridSize/2), startPosition.y + (gridSize/2), startPosition.z);
	          	hanging = true;
			} else {
				endPosition = new Vector3(startPosition.x + sign * gridSize, startPosition.y, startPosition.z);
			}
		}
		*/
		
		Collider2D rightCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x + gridSize, startPosition.y));
		Collider2D rightUpCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x + gridSize, startPosition.y + gridSize));
		Collider2D rightDownCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x + gridSize, startPosition.y - gridSize));
		Collider2D leftCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x - gridSize, startPosition.y));
		Collider2D leftUpCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x - gridSize, startPosition.y + gridSize));
		Collider2D leftDownCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x - gridSize, startPosition.y - gridSize));
		Collider2D upCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x, startPosition.y + gridSize));
		Collider2D downCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x, startPosition.y - gridSize));

		if (((rightCollider != null && rightUpCollider == null && sign > 0) || (leftCollider != null && leftUpCollider == null && sign < 0)) && upCollider == null) {
			endPosition = new Vector3(startPosition.x + sign * gridSize, startPosition.y + gridSize, startPosition.z);
			stepUp = true;
		} else if((rightCollider == null && sign > 0) || (leftCollider == null && sign < 0)){
			endPosition = new Vector3(startPosition.x + sign * gridSize, startPosition.y, startPosition.z);
		} else {
			endPosition = startPosition;
		}

		if(Physics2D.OverlapPoint (new Vector2 (startPosition.x + sign * gridSize, startPosition.y - gridSize * 2)) != null && startPosition != endPosition) {
			endPosition.y -= gridSize;
		}

		if(Input.GetKey("o") && leftCollider != null) {
			Vector3 boxStart = leftCollider.transform.position;
			Vector3 boxEnd = boxStart;
			if(sign > 0) {
				if(rightCollider == null) {
					boxEnd.x += gridSize;
				} else {
					endPosition = startPosition;
				}
			} else {
				if(Physics2D.OverlapPoint(new Vector2(boxStart.x - gridSize, boxStart.y)) == null) {
					boxEnd.x -= gridSize;
					if(stepUp) {
						endPosition.y -= gridSize;
					}
				} else {
					endPosition = startPosition;
				}
			}
			while(t < 1f) {
				t += Time.deltaTime * (moveSpeed/gridSize);
				transform.position = Vector3.Lerp(startPosition, endPosition, t);
				leftCollider.transform.gameObject.transform.position = Vector3.Lerp(boxStart, boxEnd, t);
				yield return null;
			}
		} else if(Input.GetKey("p") && rightCollider != null) {
			Vector3 boxStart = rightCollider.transform.position;
			Vector3 boxEnd = boxStart;
			if(sign > 0) {
				if(Physics2D.OverlapPoint(new Vector2(boxStart.x + gridSize, boxStart.y)) == null) {
					boxEnd.x += gridSize;
					if(stepUp) {
						endPosition.y -= gridSize;
					}
					endPosition.x += gridSize;
				} else {
					endPosition = startPosition;
				}
			} else {
				if(leftCollider == null) {
					boxEnd.x -= gridSize;
				} else {
					endPosition = startPosition;
				}
			}
			while(t < 1f) {
				t += Time.deltaTime * (moveSpeed/gridSize);
				transform.position = Vector3.Lerp(startPosition, endPosition, t);
				rightCollider.transform.gameObject.transform.position = Vector3.Lerp(boxStart, boxEnd, t);
				yield return null;
			}
		} else {
			while (t < 1f) {
			
				t += Time.deltaTime * (moveSpeed/gridSize);
				transform.position = Vector3.Lerp(startPosition, endPosition, t);
				yield return null;
			}
		}

		while(Physics2D.OverlapPoint (new Vector2 (transform.position.x, transform.position.y - gridSize)) == null) {
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
		stepUp = false;
		animator.SetBool("Running", false);	
		
		yield return 0;
	}
	
}
