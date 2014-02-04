using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour {

	// Block states
	public bool unmovable;   // Determines whether the block is unmovable
	public bool hangable;    // Determines whether players can hang on to the block
	public bool crumbling;   // Determines whether the block will dissapear once stepped on.
	public bool sticky;      // Determines whether players will get slowed while in contact with the block.
	public bool pulledOut;  // Determines whether the block is pulled out or not.

	// Components
	protected Animator animator;
	protected BoxCollider2D boxCollider;
	
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		boxCollider = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		// Check whether the block is pulled out or not and set accordingly.
		if (pulledOut){
			PullOut();
		} else {
			PushIn();
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
}