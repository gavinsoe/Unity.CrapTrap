using UnityEngine;
using System.Collections;

public class FailedGUI : MonoBehaviour {

	public GUISkin mainSkin;
	Rect r;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		GUI.skin = mainSkin;
		
		// Calculate the menu rect
		r = new Rect (Screen.width/2 - 104,
		              Screen.height/2 - 100,
		              208,
		              200);
		
		// Draw menu
		GUILayout.BeginArea (r);
		GUILayout.BeginVertical ("box");
		
		GUILayout.Label ("FAILED");
		
		// Make the buttons.
		if(GUILayout.Button("Retry")) {
			enabled = false;
			Time.timeScale = 1f;
			Application.LoadLevel(Application.loadedLevel);
		}
		if(GUILayout.Button ("Quit to Main Menu")) {
			Application.LoadLevel("test");
		}
		
		GUILayout.EndVertical ();
		GUILayout.EndArea ();
	}
}
