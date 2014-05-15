using UnityEngine;
using System;
using System.Collections;

public class TitleScreenGUI : MonoBehaviour {

    // GUI Skin
    public GUISkin activeSkin;
    enum Pages { TitleScreen, ContactUs }

    // Page variables
    private Vector2 scrollPosition = new Vector2(0, 0);

    void Start()
    {
    }

    void OnGUI()
    {
        GUI.skin = activeSkin;
        DemoTitleScreen();
    }

    void DemoTitleScreen()
    {
        var formOffset_X = Screen.width * 0.2f;
        var formOffset_Y = Screen.height * 0.05f;
        var formWidth = Screen.width * 0.6f;
        var formHeight = Screen.height * 0.9f;

        GUILayout.BeginArea(new Rect(formOffset_X, formOffset_Y, formWidth, formHeight));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.BeginVertical();

        GUILayout.Label("Crap Trap! [DEMO]", activeSkin.customStyles[0]);

        // Tutorial Stage
        if (GUILayout.Button("Tutorial Stage"))
        {
            // Redirect to Tutorial Stage
            // Application.LoadLevel("TutorialStage");
        }

        // Settings Button
        if (GUILayout.Button("Stage 1"))
        {
            // Redirect to Stage 1
            // Application.LoadLevel("DemoStage1");
        }

        // Settings Button
        if (GUILayout.Button("Stage 2"))
        {
            // Redirect to Stage 2
            // Application.LoadLevel("DemoStage2");
        }

        // Credits Button
        if (GUILayout.Button("Let us know what you think!"))
        {
            // Open Contact us modal
            Application.LoadLevel("GUI_Review");
        }

        // Credits Button
        if (GUILayout.Button("Contact Us!"))
        {
            // Open Contact us modal
            Application.LoadLevel("GUI_ContactUs");
        }

        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
}


