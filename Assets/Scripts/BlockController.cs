using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour {

	// Block states
	public bool unmovable;   // Determines whether the block is unmovable
	public bool hangable;    // Determines whether players can hang on to the block
	public bool crumbling;   // Determines whether the block will dissapear once stepped on.
	public bool sticky;      // Determines whether players will get slowed while in contact with the block.
	public bool pulledOut;  // Determines whether the block is pulled out or not.
	public bool isMoving = false;

	// Components
	protected Animator animator;
	protected BoxCollider2D boxCollider;

	// Falling speed
	private float moveSpeed = 2f;
	private float gridSize = 1f;
	
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		boxCollider = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		// Check whether the block is pulled out or not and set accordingly.
		if(!isMoving) {
			if (pulledOut){
				PullOut();
			} else {
				PushIn();
			}

			Vector3 startPosition = transform.position;

			Collider2D rightCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x + (gridSize/2f + 0.1f), startPosition.y));
			Collider2D rightUpCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x + gridSize/2f + 0.1f, startPosition.y + gridSize/2f + 0.1f));
			Collider2D rightDownCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x + gridSize/2f + 0.1f, startPosition.y - gridSize/2f - 0.1f));
			Collider2D leftCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x - gridSize/2f - 0.1f, startPosition.y));
			Collider2D leftUpCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x - gridSize/2f - 0.1f, startPosition.y + gridSize/2f + 0.1f));
			Collider2D leftDownCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x - gridSize/2f - 0.1f, startPosition.y - gridSize/2f - 0.1f));
			Collider2D upCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x, startPosition.y + gridSize/2f + 0.1f));
			Collider2D downCollider = Physics2D.OverlapPoint (new Vector2 (startPosition.x, startPosition.y - gridSize/2f - 0.1f));

			if(rightCollider == null && leftCollider == null && upCollider == null && downCollider == null &&
			      rightUpCollider == null && rightDownCollider == null && leftUpCollider == null && leftDownCollider == null) {
				StartCoroutine(FallDown(transform));
			}
		}
	}
	
	// Actions to take when block is pulled out
	public void PullOut(){
		// Change the color of the block
		animator.SetBool("Out", true);	
		// Enable the collider
		boxCollider.enabled = true;
		// Set the z-depth
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
	}
	
	// Actions to take when block is pushed in
	public void PushIn(){
		// Change the color of the block
		animator.SetBool("Out", false);	
		// Enable the collider
		boxCollider.enabled = false;
		// Set the z-depth
		transform.position = new Vector3(transform.position.x, transform.position.y, 1);
	}

	public IEnumerator FallDown(Transform transform) {
		Vector3 startPosition = transform.position;
		isMoving = true;
		
		while(Physics2D.OverlapPoint (new Vector2 (startPosition.x + (gridSize/2f + 0.1f), startPosition.y)) == null && //right
		      Physics2D.OverlapPoint (new Vector2 (startPosition.x - gridSize/2f - 0.1f, startPosition.y)) == null && //left
		      Physics2D.OverlapPoint (new Vector2 (startPosition.x, startPosition.y + gridSize/2f + 0.1f)) == null && //up
		      Physics2D.OverlapPoint (new Vector2 (startPosition.x, startPosition.y - gridSize/2f - 0.1f)) == null && //down
		      Physics2D.OverlapPoint (new Vector2 (startPosition.x + gridSize/2f + 0.1f, startPosition.y + gridSize/2f + 0.1f)) == null && //right up
		      Physics2D.OverlapPoint (new Vector2 (startPosition.x + gridSize/2f + 0.1f, startPosition.y - gridSize/2f - 0.1f)) == null && //right down
		      Physics2D.OverlapPoint (new Vector2 (startPosition.x - gridSize/2f - 0.1f, startPosition.y + gridSize/2f + 0.1f)) == null && //left up
		      Physics2D.OverlapPoint (new Vector2 (startPosition.x - gridSize/2f - 0.1f, startPosition.y - gridSize/2f - 0.1f)) == null) { //left down
			float t = 0;
			startPosition = transform.position;
			Vector3 endPosition = startPosition;
			endPosition.y -= gridSize;
			while (t < 1f) {
				
				t += Time.deltaTime * (moveSpeed/gridSize);
				transform.position = Vector3.Lerp(startPosition, endPosition, t);
				yield return null;
			}
		}
		isMoving = false;

		yield return 0;
	}

	public void Moving() {
		isMoving = true;
	}

	public void NotMoving() {
		isMoving = false;
	}
}