using UnityEngine;
using System.Collections;

public class DestinationController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Physics2D.OverlapPoint (new Vector2 (transform.position.x, transform.position.y), 1 << LayerMask.NameToLayer("Character"), 3.5f, 12.5f) != null) {
			Time.timeScale = 0;
			Camera.main.GetComponent<GameCompletedGUI>().enabled = true;
		}
	}
}
