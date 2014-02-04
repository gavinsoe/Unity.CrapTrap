using UnityEngine;
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

		Vector2 rightVector = new Vector2 (startPosition.x + gridSize, startPosition.y);
		Collider2D rightCollider = Physics2D.OverlapPoint (rightVector);
		Vector2 rightUpVector = new Vector2 (startPosition.x + gridSize, startPosition.y + gridSize);
		Collider2D rightUpCollider = Physics2D.OverlapPoint (rightUpVector);
		Vector2 leftVector = new Vector2 (startPosition.x - gridSize, startPosition.y);
		Collider2D leftCollider = Physics2D.OverlapPoint (leftVector);
		Vector2 leftUpVector = new Vector2 (startPosition.x - gridSize, startPosition.y + gridSize);
		Collider2D leftUpCollider = Physics2D.OverlapPoint (leftUpVector);

		if ((rightCollider != null && rightUpCollider == null && sign > 0) || (leftCollider != null && leftUpCollider == null && sign < 0)) {
			endPosition = new Vector3(startPosition.x + sign * gridSize, startPosition.y + gridSize, startPosition.z);
			Debug.Log(startPosition);
			Debug.Log(endPosition);
		} else if((rightCollider == null && sign > 0) || (leftCollider == null && sign < 0)){
			endPosition = new Vector3(startPosition.x + sign * gridSize, startPosition.y, startPosition.z);
		} else {
			endPosition = startPosition;
		}
				
		while (t < 1f) {
		
			t += Time.deltaTime * (moveSpeed/gridSize);
			transform.position = Vector3.Lerp(startPosition, endPosition, t);
			yield return null;
		}
		
		isMoving = false;
		animator.SetBool("Running", false);	
		
		yield return 0;
	}
	
}
