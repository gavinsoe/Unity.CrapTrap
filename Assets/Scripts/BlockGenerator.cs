using UnityEngine;
using System.Collections;

public class BlockGenerator : MonoBehaviour {

	public int width, i;
	public int height, j;
	public GameObject[] blocks;

	// Use this for initialization
	void Start () {
		if (transform.parent.name == "Bottom Floor"){
			// Create the blocks (bottom 2 levels)
			for (i = 1; i <= width; i++){
				for (j = 1; j <= height; j++){
					
					GameObject block = blocks[0]; // Retrieve the normal block
					if ( j == 1 ){
						// Set all blocks to be 'pulled out' when it is at the bottom most level.
						block.GetComponent<BlockController>().pulledOut = true;
					} else {
						// Randomize the rest of the blocks
						block.GetComponent<BlockController>().pulledOut = (Random.value > 0.5f);
					}
					
					transform.position = new Vector3(i,j,0);
					var new_block = Instantiate (block, transform.position, transform.rotation) as GameObject;
					new_block.transform.parent = this.transform.parent;
					new_block.transform.localPosition = transform.position;
					
					// checks if block collides with the character, if it does, make sure block is pulled in
					if (Physics2D.OverlapPoint(new_block.transform.position, 1 << LayerMask.NameToLayer("Character"))){
						new_block.GetComponent<BlockController>().pulledOut = false;
					}
					
				}
			}
		} else {
			// Create the blocks.
			for (i = 1; i <= width; i++){
				for (j = -3; j <= (height-3); j++){
					transform.position = new Vector3(i,j,0);
					var new_block = Instantiate (GenerateRandomBlock(), transform.position, transform.rotation) as GameObject;
					new_block.transform.parent = this.transform.parent;
					new_block.transform.localPosition = transform.position;
				}
			}
		}
	}
	
	GameObject GenerateRandomBlock(){
		GameObject block = blocks[0];
		block.GetComponent<BlockController>().pulledOut = (Random.value > 0.5f);
		return block;
	}
	
}