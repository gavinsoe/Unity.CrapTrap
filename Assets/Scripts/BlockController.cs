using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour {

    // Block types
    public enum BlockType
    {
        Normal,
        Unmovable,
        Unhangable,
        Ice,
        Fire,
        Spiky,
        Crumbling,
        Explosive,
        Sticky,
        Switch,
        Invinsible,
        Gate
    }
    public BlockType blockType;

    // Block states
    public bool pulledOut = true;  // Determines whether the block is pulled out or not.
    public bool explode = false; // Block explodes when set to true
	private bool isMoving = false; // Toggles on when block is moving
    private bool isActive = true;  // Toggles on when block is nearby character

	// Components
	protected Animator animator; // The box animator

	// Falling speed
	private float moveSpeed = 6f;
	private float gridSize = 1f;
	
	// Use this for initialization
	void Start () {
        // Retrieve the animator and the collider component
		animator = GetComponent<Animator>();

        // If block is of type 'Gate' always set block to start at a pushed in position
		if ( blockType == BlockType.Gate ) pulledOut = false;

        // Check the pulledOut state and initialise block accordingly
        if ( pulledOut ) PullOut(); else PushIn();
	}

    // Activates the block when it collides with the "Block Activator" collider, which is attached to the character
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Block Activator")
        {
            isActive = true;
        } 
        if (col.gameObject.tag == "Player")
        {
            if (blockType == BlockType.Fire)
            {
                col.gameObject.GetComponent<CharacterController>().isBurning = true;
            }
            else
            {
                col.gameObject.GetComponent<CharacterController>().isBurning = false;
            }
        }
    }

	// Update is called once per frame
	void Update () {
		if(!isMoving && isActive) {
            Collider2D col = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y), 1 << LayerMask.NameToLayer("Character"), -0.9f, 0.9f);
            if (col != null && col.gameObject.name == "Character" && blockType == BlockType.Gate)
            {
				pulledOut = true;
			}

            if (Movable())
            {
				StartCoroutine(FallDown(transform));
			}
		}

        // Check the pulledOut state and initialise block accordingly
        if (pulledOut) PullOut(); else PushIn();

        // Check if block should explode
        if (blockType == BlockType.Explosive && explode) StartCoroutine(Explode());
	}

	// Actions to take when block is pulled out
    protected void PullOut()
    {
		// Change the color of the block
		animator.SetBool("Out", true);	
		// Set the z-depth
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
	}
	
	// Actions to take when block is pushed in
    protected void PushIn()
    {
		// Change the color of the block
		animator.SetBool("Out", false);	
		// Set the z-depth
		transform.position = new Vector3(transform.position.x, transform.position.y, 1);
	}

    // Actions to take when block explodes
    protected IEnumerator Explode()
    {
        if (Debug.isDebugBuild) Debug.Log("Explode");
        Collider2D neighbouringBox;
        // Check right block
        if ( (neighbouringBox = Physics2D.OverlapPoint(new Vector2(transform.position.x + gridSize, transform.position.y), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f)) != null){
            StartCoroutine(neighbouringBox.gameObject.GetComponent<BlockController>().HitByExplosion());
        }
        // Check left block
        if ( (neighbouringBox = Physics2D.OverlapPoint(new Vector2(transform.position.x - gridSize, transform.position.y), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f)) != null){
            StartCoroutine(neighbouringBox.gameObject.GetComponent<BlockController>().HitByExplosion());
        }
        // Check top block
        if ( (neighbouringBox = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f)) != null){
            StartCoroutine(neighbouringBox.gameObject.GetComponent<BlockController>().HitByExplosion());
        }
        // Check bottom block
        if ( (neighbouringBox = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f)) != null){
            StartCoroutine(neighbouringBox.gameObject.GetComponent<BlockController>().HitByExplosion());
        }
        // Check top right block
        if ( (neighbouringBox = Physics2D.OverlapPoint(new Vector2(transform.position.x + gridSize, transform.position.y + gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f)) != null){
            StartCoroutine(neighbouringBox.gameObject.GetComponent<BlockController>().HitByExplosion());
        }
        // Check bottom right block
        if ( (neighbouringBox = Physics2D.OverlapPoint(new Vector2(transform.position.x + gridSize, transform.position.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f)) != null){
            StartCoroutine(neighbouringBox.gameObject.GetComponent<BlockController>().HitByExplosion());
        }
        // Check top left block
        if ( (neighbouringBox = Physics2D.OverlapPoint(new Vector2(transform.position.x - gridSize, transform.position.y + gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f)) != null){
            StartCoroutine(neighbouringBox.gameObject.GetComponent<BlockController>().HitByExplosion());
        }
        // Check bottom left block
        if ( (neighbouringBox = Physics2D.OverlapPoint(new Vector2(transform.position.x - gridSize, transform.position.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f)) != null){
            StartCoroutine(neighbouringBox.gameObject.GetComponent<BlockController>().HitByExplosion());
        }
        yield return new WaitForSeconds(1.0F);
        explode = false;
        Destroy(gameObject);
    }

    // Actions to take when block is affected by an explosion
    protected IEnumerator HitByExplosion()
    {

        if (Debug.isDebugBuild) Debug.Log("Hit!! " + transform.name);
        if (blockType == BlockType.Ice)
        {
            animator.SetTrigger("Evaporate");
            // Evaporate
            Destroy(gameObject);
        }
        else if (blockType == BlockType.Explosive)
        {
            yield return new WaitForSeconds(1.0F);
            StartCoroutine(Explode());
        }
        else if (blockType == BlockType.Invinsible ||
                 blockType == BlockType.Unmovable ||
                 blockType == BlockType.Gate)
        {
            // DO nothing
        }
        else
        {
            // Turn into fire block,
            animator.SetTrigger("Burn");

            blockType = BlockType.Fire;
        }

    }

    // Check if block us supposed to fall
	public IEnumerator FallDown(Transform transform) {
		Vector3 startPosition = transform.position;
		isMoving = true;

        while (Physics2D.OverlapPoint(new Vector2(startPosition.x + (gridSize), startPosition.y), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null && //right
              Physics2D.OverlapPoint(new Vector2(startPosition.x - gridSize, startPosition.y), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null && //left
              Physics2D.OverlapPoint(new Vector2(startPosition.x, startPosition.y + gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null && //up
              Physics2D.OverlapPoint(new Vector2(startPosition.x, startPosition.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null && //down
              Physics2D.OverlapPoint(new Vector2(startPosition.x + gridSize, startPosition.y + gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null && //right up
              Physics2D.OverlapPoint(new Vector2(startPosition.x + gridSize, startPosition.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null && //right down
              Physics2D.OverlapPoint(new Vector2(startPosition.x - gridSize, startPosition.y + gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null && //left up
              Physics2D.OverlapPoint(new Vector2(startPosition.x - gridSize, startPosition.y - gridSize), 1 << LayerMask.NameToLayer("Terrain"), -0.9f, 0.9f) == null && //left down
		      transform.position.z == 0) { 
			float t = 0;
			Vector3 endPosition = startPosition;
			endPosition.y -= gridSize;
			while (t < 1f) {

                t += Time.deltaTime * (moveSpeed / gridSize);
				transform.position = Vector3.Lerp(startPosition, endPosition, t);
				yield return null;
			}
			startPosition = transform.position;
		}
		isMoving = false;
        isActive = false;

		yield return 0;
	}

	public void Moving() {
		isMoving = true;
	}

	public void NotMoving() {
		isMoving = false;
	}

	public void Pull() {
		pulledOut = true;
	}

	public void Push() {
		pulledOut = false;
	}

    public bool Movable()
    {
        if (blockType == BlockType.Normal ||
            blockType == BlockType.Unhangable ||
            blockType == BlockType.Ice ||
            blockType == BlockType.Fire ||
            blockType == BlockType.Spiky ||
            blockType == BlockType.Crumbling ||
            blockType == BlockType.Explosive ||
            blockType == BlockType.Sticky)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Hangable()
    {
        if (blockType == BlockType.Normal ||
            blockType == BlockType.Unmovable ||
            blockType == BlockType.Crumbling ||
            blockType == BlockType.Explosive ||
            blockType == BlockType.Sticky ||
            blockType == BlockType.Switch ||
            blockType == BlockType.Invinsible ||
            blockType == BlockType.Gate)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Slippery()
    {
        if (blockType == BlockType.Ice)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Fiery()
    {
        if (blockType == BlockType.Fire)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /*
	public bool GetUnMovable() {
		return unmovable;
	}

	public bool GetHangable() {
		return hangable;
	}

	public bool GetCrumbling() {
		return crumbling;
	}

	public bool GetSticky() {
		return sticky;
	}

	public bool GetSlippery() {
		return slippery;
	}*/
}