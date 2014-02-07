using UnityEngine;
using System.Collections;

public class BlockGenerator : MonoBehaviour {

	public int width;
	public int height;
	public GameObject[] blocks;

	// Use this for initialization
	void Start () {
		// Create the blocks.
		for (int i = 1; i <= width; i++){
			for (int j = -3; j <= (height-3); j++){
				transform.position = new Vector3(i,j,0);
				var block = Instantiate (GenerateBlock(), transform.position, transform.rotation) as GameObject;
				block.transform.parent = this.transform.parent;
				block.transform.localPosition = transform.position;
			}
		}
	}
	
	GameObject GenerateBlock(){
		GameObject block = blocks[0];
		block.GetComponent<BlockController>().pulledOut = (Random.value > 0.5f);
		return block;
	}
	
}