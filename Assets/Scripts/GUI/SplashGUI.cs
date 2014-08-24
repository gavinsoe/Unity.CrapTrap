using UnityEngine;
using System.Collections;

public class SplashGUI : MonoBehaviour {
    public float splashDuration = 2;
    public Texture splashTexture;

    private Rect screenRect;

	// Use this for initialization
	void Start () {
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
	}

    void Update()
    {
        splashDuration -= Time.deltaTime;
        if (splashDuration < 0)
        {
            Application.LoadLevel("GUI_TitleScreen");
        }
    }

    void OnGUI()
    {
        GUI.DrawTexture(screenRect, splashTexture, ScaleMode.ScaleAndCrop);
    }
}
