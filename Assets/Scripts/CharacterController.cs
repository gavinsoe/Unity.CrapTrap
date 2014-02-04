using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {
	
	private float moveSpeed = 2f;
	private float gridSize = 0.51f;
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
