using UnityEngine;
using System.Collections;

public class MainMenuGUI : MonoBehaviour {

	private int menuNum;
	Rect r;
    public GUISkin mainSkin;
	public GUISkin levelSkin;

	// Use this for initialization
	void Start () {
		menuNum = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {



		// Make a background box
		//GUI.Box (new Rect (20, 20, 100, 90), "Main Menu");

		if(menuNum == 0 ) {
			// Calculate the menu rect
			r = new Rect (Screen.width/2 - 104,
			              Screen.height/2 - 100,
			              208,
			              200);
			GUI.skin = mainSkin;
			MainMenu ();
		} else if(menuNum == 1) {
			// Calculate the menu rect
			r = new Rect (Screen.width / 2 - 164,
			              Screen.height / 2 - 150,
			              328,
			              300);
			GUI.skin = levelSkin;
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

		// First row of buttons
		GUILayout.BeginHorizontal ();
		// Make the buttons.
		if(GUILayout.Button("B1")) {
			Application.LoadLevel("stage_B.1");
		}
		if(GUILayout.Button("B2")) {
			Application.LoadLevel("stage_B.2");
		}
		if(GUILayout.Button("B3")) {
			Application.LoadLevel("stage_B.3");
		}
		if(GUILayout.Button("B4")) {
			Application.LoadLevel("stage_B.4");
		}
		if(GUILayout.Button("B5")) {
			Application.LoadLevel("stage_B.5");
		}
		if(GUILayout.Button("B6")) {
			Application.LoadLevel("stage_B.6");
		}
		GUILayout.EndHorizontal ();

		// Second row of buttons
		GUILayout.BeginHorizontal ();
		// Make the buttons.
		if(GUILayout.Button("G1")) {
			Application.LoadLevel("stage_G.1_Climbing");
		}
		if(GUILayout.Button("G2")) {
			Application.LoadLevel("stage_G.2_Climbing+SecretArea");
		}
		GUILayout.EndHorizontal ();
		GUILayout.EndVertical ();
		GUILayout.EndArea ();
		if(GUI.Button (new Rect(Screen.width * 0.3f, Screen.height * 0.8f, 60, 50), "Back")) {
			menuNum = 0;
		}
	}
}
