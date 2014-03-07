using UnityEngine;
using System.Collections;

public class StoneTuteController : MonoBehaviour {

	private bool seen;
	private int phase;

	// Use this for initialization
	void Start () {
		phase = 1;
		seen = false;
	}
	
	// Update is called once per frame
	void Update () {
		Collider2D col = Physics2D.OverlapPoint (new Vector2 (transform.position.x, transform.position.y), 1 << LayerMask.NameToLayer("Character"),-0.9f, 0.9f);
		if( col != null && col.gameObject.name == "Character" && !seen) {
			Time.timeScale = 0;
			Camera.main.GetComponent<StoneTutorialGUI>().enabled = true;
			Camera.main.GetComponent<StoneTutorialGUI>().SetScript(this);
			/*
			if(phase > 3) {
				Camera.main.GetComponent<StoneTutorialGUI>().enabled = false;
				Time.timeScale = 1;
				seen = true;
			}
			*/
		}
	}

	public void Done() {
		Time.timeScale = 1;
		Camera.main.GetComponent<StoneTutorialGUI>().enabled = false;
		Destroy (gameObject);
	}
}
