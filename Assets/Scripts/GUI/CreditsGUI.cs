using UnityEngine;
using System;
using System.Collections;

public class CreditsGUI : MonoBehaviour {

    // GUI Skin
    public GUISkin activeSkin;

    // Page variables
    private Rect bgRect;
    private Texture bgTexture;

    // Credits Container
    private Rect containerRect;
    private Rect containerBgRect;
    private Rect contentRect;
    private float contentXMargin = 0.125f;
    private float contentYMargin = 0.11f;

    // Le Splat
    private Rect splatRect;
    private Texture splatTexture;
    private float splatSize = 0.24f;
    private float splatXOffset = 0.755f;
    private float splatYOffset = 0.62f;

    // Logo
    private Texture logoTexture;
    private float logoHeight;
    private float logoWidth;
    private float logoScale = 0.2f;

    // Text
    private GUIStyle labelHeader;
    private GUIStyle labelNormal;
    private GUIStyle labelBold;
    private float headerFontScale = 0.05f;
    private float normalFontScale = 0.032f;

    // Buttons
    private Rect btnBackRect;
    private Rect btnWebRect;
    private float btnSize = 0.125f;
    private float btnXMargin = 0.18f;
    private float btnYMargin = 0.12f;
    private ButtonHandler btnBackScale;
    private ButtonHandler btnWebScale;

    void Start()
    {
        #region GUI

        bgRect = new Rect(0, 0, Screen.width, Screen.height);
        bgTexture = activeSkin.customStyles[1].normal.background;

        // Buttons
        float btnHeight = Screen.height * btnSize;
        float btnWidth = btnHeight * ((float)activeSkin.customStyles[5].normal.background.width /
                                      (float)activeSkin.customStyles[5].normal.background.height);
        float xMargin = btnWidth * btnXMargin;
        float yMargin = btnHeight * btnYMargin;
        btnBackRect = new Rect(xMargin, Screen.height - btnHeight - yMargin, btnWidth, btnHeight);
        btnWebRect = new Rect(Screen.width - btnWidth - xMargin, Screen.height - btnHeight - yMargin, btnWidth, btnHeight);

        // Initialise button scalers
        btnBackScale = new ButtonHandler(btnBackRect, gameObject, 0.9f, "Back_ScaleButton");
        btnWebScale = new ButtonHandler(btnWebRect, gameObject, 0.9f, "Web_ScaleButton");

        // Container and container background
        var containerHeight = Screen.height;
        var containerWidth = Screen.width - 2 * btnWidth - 2 * xMargin;
        containerRect = new Rect((Screen.width - containerWidth) * 0.5f, 0, containerWidth, containerHeight);
        containerBgRect = new Rect(0, 0, containerRect.width, containerRect.height);

        xMargin = containerRect.width * contentXMargin;
        yMargin = containerRect.height * contentYMargin;
        contentRect = new Rect(xMargin, yMargin, containerRect.width - 2 * xMargin, containerRect.height - 2 * yMargin);

        // The splat
        splatTexture = activeSkin.customStyles[4].normal.background;
        float splatHeight = Screen.height * splatSize;
        float splatWidth = splatHeight * ((float)splatTexture.width / (float)splatTexture.height);
        splatRect = new Rect(Screen.width * splatXOffset, Screen.height * splatYOffset, splatWidth, splatHeight);

        // Logo
        logoTexture = activeSkin.customStyles[0].normal.background;
        logoHeight = Screen.height * logoScale;
        logoWidth = logoHeight * (logoTexture.width / logoTexture.height);

        // Text
        labelHeader = new GUIStyle(activeSkin.label);
        labelNormal = new GUIStyle(activeSkin.label);
        labelBold = new GUIStyle(activeSkin.customStyles[3]);

        labelHeader.fontSize = (int)(Screen.height * headerFontScale);
        labelNormal.fontSize = (int)(Screen.height * normalFontScale);
        labelBold.fontSize = (int)(Screen.height * normalFontScale);

        #endregion
    }

    void OnGUI()
    {
        // Sets the GUI depth
        GUI.depth = 10;

        GUI.skin = activeSkin;
        Credits();
    }

    private void Credits()
    {
        // Background
        GUI.DrawTexture(bgRect, bgTexture, ScaleMode.ScaleAndCrop);

        // Splat
        GUI.DrawTexture(splatRect, splatTexture, ScaleMode.ScaleAndCrop);

        // Container
        GUILayout.BeginArea(containerRect);
        {
            GUI.Box(containerBgRect, "");
            
            GUILayout.BeginArea(contentRect);
            {
                GUILayout.BeginVertical();
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Label("designed and developed by", labelHeader);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Button(logoTexture, labelHeader, GUILayout.Width(logoWidth), GUILayout.Height(logoHeight));
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(@"Gavin Soebiantoro ",labelBold);
                        GUILayout.Label(@"- Programmer",labelNormal);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(@"Bayoe Otto ", labelBold);
                        GUILayout.Label(@"- Programmer/part time composer", labelNormal);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(@"Andrew Chen ", labelBold);
                        GUILayout.Label(@"- Writer", labelNormal);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(@"Amelia Chandra ", labelBold);
                        GUILayout.Label(@"- Graphic Artist",labelNormal);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.Space(labelNormal.fontSize);
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(@"special thanks to",labelHeader);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();

                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }
        GUILayout.EndArea();

        // Buttons
        if (GUI.Button(btnBackRect, "", activeSkin.customStyles[5]))
        {
            Application.LoadLevel("GUI_TitleScreen");

        }
        /*if (GUI.Button(btnWebRect, "", activeSkin.customStyles[6]))
        {
            Application.OpenURL("http://articonnect.com/");
        }*/
        btnBackScale.OnMouseOver(btnBackRect);
        btnWebScale.OnMouseOver(btnWebRect);
    }

    // applies values from iTween
    void Back_ScaleButton(Rect size)
    {
        btnBackRect = size;
    }

    // applies values from iTween
    void Web_ScaleButton(Rect size)
    {
        btnWebRect = size;
    }
}

