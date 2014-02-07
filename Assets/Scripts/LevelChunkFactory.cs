using UnityEngine;
using System.Collections;

public class LevelChunkFactory : MonoBehaviour {

	public GameObject[] levelChunks;

	void OnTriggerEnter2D(Collider2D col){
		// Check if player goes past this collider
		if (col.gameObject.tag == "Player"){
			// Get location of current chunk
			Vector3 cur_location = transform.parent.position;
			
			// Get location of the next generated chunk
			// Retrieve height of the levelChunk
			var height = transform.parent.GetComponentInChildren<BlockGenerator>().height;
			
			// Generate the next section of the stage
			transform.position = new Vector3(cur_location.x,cur_location.y + height,0);
			
			var block = Instantiate (GenerateLevelChunk(), transform.position, transform.rotation) as GameObject;
		}
	}
	
	GameObject GenerateLevelChunk(){
		GameObject chunk = levelChunks[0];
		return chunk;
	}
}
