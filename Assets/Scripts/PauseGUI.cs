using UnityEngine;
using System.Collections;

public class PauseGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		
		// Calculate the menu rect
		Rect r = new Rect (Screen.width * (1f - 0.3f) / 2,
		              Screen.height * (1f - 0.3f) / 2,
		              Screen.width * 0.3f,
		              Screen.height * 0.3f);
		
		// Draw menu
		GUILayout.BeginArea (r);
		GUILayout.BeginVertical ("box");
		
		GUILayout.Label ("Game Paused");
		
		// Make the buttons.
		if(GUILayout.Button("Resume Game")) {
			enabled = false;
			Time.timeScale = 1f;
		}
		if(GUILayout.Button ("Quit to Main Menu")) {
			Application.LoadLevel("test");
		}
		
		GUILayout.EndVertical ();
		GUILayout.EndArea ();
	}
}
