using UnityEngine;
using System.Collections;

public class DestinationController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Collider2D col = Physics2D.OverlapPoint (new Vector2 (transform.position.x, transform.position.y), 1 << LayerMask.NameToLayer("Character"),-0.9f, 0.9f);
		if( col != null && col.gameObject.name == "Character") {
			Time.timeScale = 0;
			Camera.main.GetComponent<GameCompletedGUI>().enabled = true;
		}
	}

}
