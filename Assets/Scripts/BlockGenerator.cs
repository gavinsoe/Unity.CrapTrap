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
			for (int j = 1; j <= height; j++){
				transform.position = new Vector3(i,j,0);
				Instantiate (Randomizer(), transform.position, transform.rotation);
			}
		}
	}
	
	GameObject Randomizer(){
		GameObject block = blocks[0];
		block.GetComponent<BlockController>().pulledOut = (Random.value > 0.5f);
		return block;
	}
	
	
}