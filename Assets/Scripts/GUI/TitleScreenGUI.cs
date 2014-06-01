using UnityEngine;
using System;
using System.Collections;

public class TitleScreenGUI : MonoBehaviour {

    // GUI Skin
    public GUISkin activeSkin;
    enum Pages { TitleScreen, ContactUs }

    // Page variables
    private Vector2 scrollPosition = new Vector2(0, 0);
    public Rect logoContainerRect;
    public float logoContainerWidth;
    public float logoContainerHeight;

    public Rect logoRect;
    public Texture logoTexture;
    public float logoWidth;
    public float logoHeight;
    public float logoXOffset;
    public float logoYOffset;

    public Rect demoTextRect;
    private float demoTextFontScale = 0.13f;

    public Rect navContainerRect;
    public float navContainerWidth;
    public float navContainerHeight;
    public float navContainerXOffset;
    public float navContainerYOffset;
    private float navFontScaling = 0.06f;
    private float navTopPaddingScaling = 0.5f;
    private float navBottomPaddingScaling = 0.65f;


    void Start()
    {
    }

    void OnGUI()
    {
        #region temp

        logoContainerWidth = Screen.width * 0.6f;
        logoContainerHeight = Screen.height;
        logoContainerRect = new Rect(0, 0, logoContainerWidth, logoContainerHeight);

        logoTexture = activeSkin.customStyles[0].normal.background;
        logoWidth = logoContainerWidth;
        logoHeight = logoWidth * ((float)logoTexture.height / (float)logoTexture.width);
        logoXOffset = 0;
        logoYOffset = 0;
        logoRect = new Rect(logoXOffset, logoYOffset, logoWidth, logoHeight);

        // Font auto scaling
        activeSkin.label.fontSize = (int)(Screen.height * demoTextFontScale);
        demoTextRect = new Rect(0, logoHeight*0.8f, logoContainerWidth, activeSkin.label.fontSize * 2);

        // Nav button font scaling
        activeSkin.button.fontSize = (int)(Screen.height * navFontScaling);
        // Padding Scaling
        activeSkin.button.padding.top = (int) (activeSkin.button.fontSize * navTopPaddingScaling);
        activeSkin.button.padding.bottom = (int) (activeSkin.button.fontSize * navBottomPaddingScaling);

        navContainerHeight = Screen.height * 0.7f;
        navContainerWidth = Screen.width * 0.3f;
        navContainerXOffset = logoContainerWidth;
        navContainerYOffset = Screen.height * 0.15f;
        navContainerRect = new Rect(navContainerXOffset, navContainerYOffset, navContainerWidth, navContainerHeight);

        #endregion

        GUI.skin = activeSkin;
        TitleScreen();
    }

    private void TitleScreen()
    {
        GUI.BeginGroup(logoContainerRect);
        
        GUI.DrawTexture(logoRect, logoTexture);
        GUI.Label(demoTextRect,"demo");

        GUI.EndGroup();

        GUILayout.BeginArea(navContainerRect);
        //GUILayout.BeginArea(new Rect(formOffset_X, formOffset_Y, formWidth, formHeight));
        //scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.BeginVertical();


        // Tutorial Stage
        if (GUILayout.Button("tutorial"))
        {
            // Redirect to Tutorial Stage
             Application.LoadLevel("stage_tutorial");
        }

        // Settings Button
        if (GUILayout.Button("stage 1"))
        {
            // Redirect to Stage 1
             Application.LoadLevel("stage_Demo_1");
        }

        // Settings Button
        if (GUILayout.Button("stage 2"))
        {
            // Redirect to Stage 2
            Application.LoadLevel("stage_Demo_2");
        }

        // Credits Button
        if (GUILayout.Button("feedback"))
        {
            // Open Contact us modal
            Application.LoadLevel("GUI_Review");
        }

        // Credits Button
        if (GUILayout.Button("contact us!"))
        {
            // Open Contact us modal
            Application.LoadLevel("GUI_ContactUs");
        }

        GUILayout.EndVertical();
        //GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

}


