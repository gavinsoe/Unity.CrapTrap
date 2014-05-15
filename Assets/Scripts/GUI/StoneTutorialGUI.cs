using UnityEngine;
using System.Collections;

public class StoneTutorialGUI : MonoBehaviour {
	
	private StoneTuteController parentScr;
	private int phase;

	// Use this for initialization
	void Start () {
		phase = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("space")) {
			phase += 1;
		}
	}

	void OnGUI() {
		//GUI.skin = mainSkin;
		
		// Calculate the menu rect
		Rect r = new Rect (Screen.width * 0.25f,
		              Screen.height * 0.25f,
		              200,
		              100);
		
		// Draw menu
		r = GUILayout.Window (0, r, DoStone, "Tutorial #1");
	}

	public void DoStone(int windowID) {
		string str;
		GUILayout.BeginVertical("box");
		switch(phase) {
			case 1:
				str = GUILayout.TextArea ("To the right is the exotic stone block");
				break;
			case 2:
				str = GUILayout.TextArea ("The stone block is not movable at all");
				break;
			case 3:
				str = GUILayout.TextArea ("It cannot be destroyed but it is hangable");
				break;
			case 4:
				parentScr.Done();
				break;
			default:
				parentScr.Done();
				break;
		}
		GUILayout.Label ("press space");
		GUILayout.EndVertical ();
	}

	public void SetScript(StoneTuteController obj) {
		parentScr = obj;
	}
}
