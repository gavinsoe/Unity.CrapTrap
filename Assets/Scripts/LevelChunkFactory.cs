using UnityEngine;
using System.Collections;

public class LevelChunkFactory : MonoBehaviour {

	public GameObject[] levelChunks;
	private bool triggered = false; // A flag to make sure that the generating of levels is only triggered once.
	
	void OnTriggerEnter2D(Collider2D col){
		// Check if player goes past this collider
		
		if (!triggered && col.gameObject.tag == "Player"){
			// Get location of the next generated chunk
			// Retrieve height of the levelChunk
			var height = transform.GetComponentInChildren<BlockGenerator>().height;
			
			// Generate the next section of the stage
			Vector3 next_location = new Vector3(transform.position.x,transform.position.y + height,0);
			
			var chunk = Instantiate (GenerateLevelChunk(), next_location, transform.rotation) as GameObject;
			RemoveExistingBlocks(chunk);
			triggered = true;
		} else if ( col.gameObject.tag == "Collider" ){
			RemoveThisChunk();
		}
	}
	
	GameObject GenerateLevelChunk(){
		GameObject chunk = levelChunks[0];
		return chunk;
	}
	
	void RemoveExistingBlocks(GameObject chunk){
		// Remove any existing blocks
		foreach(Transform child in chunk.transform) {
			if (child.name != "Generator"){
				Destroy(child.gameObject);
			}
		}
	}
	
	void RemoveThisChunk(){
		Destroy(gameObject);
	}
}
