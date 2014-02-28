using UnityEngine;
using System.Collections;

public class MainMenuGUI : MonoBehaviour {

	private int menuNum;
	Rect r;
    public GUISkin customSkin;

	// Use this for initialization
	void Start () {
		menuNum = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
        GUI.skin = customSkin;

		// Calculate the menu rect
		r = new Rect (Screen.width * (1f - 0.3f) / 2,
		                  Screen.height * (1f - 0.3f) / 2,
		                  Screen.width * 0.3f,
		                  Screen.height * 1f);

		// Make a background box
		//GUI.Box (new Rect (20, 20, 100, 90), "Main Menu");

		if(menuNum == 0 ) {
			MainMenu ();
		} else if(menuNum == 1) {
			LevelSelect();
		}
	}

	void MainMenu() {
		// Draw menu
		GUILayout.BeginArea (r);
		GUILayout.BeginVertical ("box");

		GUILayout.Label ("Purgatory Lavatory");

		// Make the buttons.
		if(GUILayout.Button("Level Select")) {
			menuNum = 1;
		}
		if(GUILayout.Button ("Quit")) {
			Application.Quit();
		}

		GUILayout.EndVertical ();
		GUILayout.EndArea ();
	}

	void LevelSelect() {
		// Draw menu
		GUILayout.BeginArea (r);
		GUILayout.BeginVertical ("box");
		
		GUILayout.Label ("Level Select");

		// Make the buttons.
		if(GUILayout.Button("Level 1")) {
			Application.LoadLevel("stage_B.1");
		}
		if(GUILayout.Button ("Back")) {
			menuNum = 0;
		}
		
		GUILayout.EndVertical ();
		GUILayout.EndArea ();
	}
}
