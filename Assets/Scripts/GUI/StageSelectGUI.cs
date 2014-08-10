﻿using UnityEngine;
using System.Collections;

public class StageSelectGUI : MonoBehaviour {

    /* GUI Skin
     * Custom Styles [0] = Background
     * Custom Styles [1] = Back Button
     * Custom Styles [2] = Sound Button
     * Custom Styles [3] = Frame
     * Custom Styles [4] = Button Locked
     * Custom Styles [5] = Button 0 Star
     * Custom Styles [6] = Button 1 Star
     * Custom Styles [7] = Button 2 Star
     * Custom Styles [8] = Button 3 Star
     */
    public GUISkin activeSkin;
    private MainGameController mainController;
    private NavigationManager navManager;

    #region GUI related

    private Rect containerRect; // The Rect object that encapsulates the whole page

    #region background

    private Rect bgRect; // Rect that holds the background
    private Texture bgTexture; // The background texture

    #endregion
    #region buttons

    // Back Button
    private Rect backBtnRect;
    private GUIStyle backBtnStyle;
    private ButtonHandler backBtnHandler;
    private float backBtnXOffset = 0f;
    private float backBtnYOffset = 0f;
    private float backBtnScale = 0.13f;

    // Sound Button
    private bool sound = false;
    private Rect soundBtnRect;
    private GUIStyle soundBtnStyle;
    private ButtonHandler soundBtnHandler;
    private float soundBtnScale = 0.09f;
    private float soundBtnXOffset = 0.01f;
    private float soundBtnYOffset = 0.9f;

    #endregion
    #region main content

    private Rect contentHeaderRect;
    private GUIStyle contentHeaderStyle;
    private float contentHeaderScale = 0.07f;
    private float contentHeaderYOffset = 0.04f;

    private Rect contentContainerRect;
    private Rect contentContainerBgRect;
    private Texture contentContainerBgTexture;
    private float contentContainerScale = 1;

    #region stage buttons

    /* ╔═══╗ ╔═══╗ ╔═══╗ ╔═══╗ ╔═══╗
     * ║ 0 ║ ║ 1 ║ ║ 2 ║ ║ 3 ║ ║ 4 ║
     * ╚═══╝ ╚═══╝ ╚═══╝ ╚═══╝ ╚═══╝
     * ╔═══╗ ╔═══╗ ╔═══╗ ╔═══╗ ╔═══╗
     * ║ 5 ║ ║ 6 ║ ║ 7 ║ ║ 8 ║ ║ 9 ║
     * ╚═══╝ ╚═══╝ ╚═══╝ ╚═══╝ ╚═══╝
     */
    private Rect stagesContainerRect;
    private float stagesContainerYOffset = 0.57f;
    private float stagesXSpacingScale = 0.01f;
    private float stagesYSpacingScale = -0.01f;
    private float stagesBtnScale = 0.16f;

    private Rect stage1Rect;
    private Rect stage2Rect;
    private Rect stage3Rect;
    private Rect stage4Rect;
    private Rect stage5Rect;
    private Rect stage6Rect;
    private Rect stage7Rect;
    private Rect stage8Rect;
    private Rect stage9Rect;
    private Rect stage10Rect;

    private GUIStyle stage1Style;
    private GUIStyle stage2Style;
    private GUIStyle stage3Style;
    private GUIStyle stage4Style;
    private GUIStyle stage5Style;
    private GUIStyle stage6Style;
    private GUIStyle stage7Style;
    private GUIStyle stage8Style;
    private GUIStyle stage9Style;
    private GUIStyle stage10Style;

    private ButtonHandler stage1Handler;
    private ButtonHandler stage2Handler;
    private ButtonHandler stage3Handler;
    private ButtonHandler stage4Handler;
    private ButtonHandler stage5Handler;
    private ButtonHandler stage6Handler;
    private ButtonHandler stage7Handler;
    private ButtonHandler stage8Handler;
    private ButtonHandler stage9Handler;
    private ButtonHandler stage10Handler;

    private float stageBtnLabelScale = 0.36f;
    private float stageBtnLabelXOffset = 0f;
    private float stageBtnLabelYOffset = 0.06f;

    #endregion

    #endregion

    #endregion

    // Use this for initialization
	void Start () 
    {
        // Retrieve the main game controller
        mainController = gameObject.GetComponentInChildren<MainGameController>();

        // Retrieve the nav manager
        navManager = gameObject.GetComponentInChildren<NavigationManager>();

        // Set the container rect
        containerRect = new Rect(0, 0, Screen.width, Screen.height);

        #region background

        // Background
        bgRect = new Rect(0, 0, Screen.width, Screen.height);
        bgTexture = activeSkin.customStyles[0].normal.background;

        #endregion
        #region buttons

        // Back button
        backBtnStyle = activeSkin.customStyles[1];
        Texture backBtnTexture = backBtnStyle.normal.background;
        float backBtnHeight = Screen.height * backBtnScale;
        float backBtnWidth = backBtnHeight * ((float)backBtnTexture.width / (float)backBtnTexture.height);
        float backXOffset = Screen.width * backBtnXOffset;
        float backYOffset = Screen.height * backBtnYOffset;
        backBtnRect = new Rect(backXOffset, backYOffset, backBtnWidth, backBtnHeight);
        backBtnHandler = new ButtonHandler(backBtnRect, gameObject, 0.9f, "ScaleBackButton");

        // Sound button
        soundBtnStyle = activeSkin.customStyles[2];
        Texture soundBtnTexture = soundBtnStyle.normal.background;
        float soundBtnHeight = Screen.height * soundBtnScale;
        float soundBtnWidth = soundBtnHeight * ((float)soundBtnTexture.width / (float)soundBtnTexture.height);
        float soundXOffset = Screen.width * soundBtnXOffset;
        float soundYOffset = Screen.height * soundBtnYOffset;
        soundBtnRect = new Rect(soundXOffset, soundYOffset, soundBtnWidth, soundBtnHeight);
        soundBtnHandler = new ButtonHandler(soundBtnRect, gameObject, 0.9f, "ScaleSoundButton");

        #endregion
        #region main content

        contentContainerBgTexture = activeSkin.customStyles[3].normal.background;
        float contentContainerHeight = Screen.height * contentContainerScale;
        float contentContainerWidth = contentContainerHeight * ((float)contentContainerBgTexture.width / (float)contentContainerBgTexture.height);
        float contentContainerXOffset = (Screen.width - contentContainerWidth) * 0.5f;
        float contentContainerYOffset = (Screen.height - contentContainerHeight) * 0.5f;
        contentContainerRect = new Rect(contentContainerXOffset, contentContainerYOffset, contentContainerWidth, contentContainerHeight);
        contentContainerBgRect = new Rect(0, 0, contentContainerWidth, contentContainerHeight);

        contentHeaderStyle = new GUIStyle(activeSkin.label);
        contentHeaderStyle.fontSize = (int)(contentContainerHeight * contentHeaderScale);
        float headerWidth = contentContainerWidth;
        float headerHeight = contentHeaderStyle.fontSize * 2;
        float headerXOffset = 0;
        float headerYOffset = contentContainerHeight * contentHeaderYOffset;
        contentHeaderRect = new Rect(headerXOffset, headerYOffset, headerWidth, headerHeight);

        #region stage buttons

        Texture stageBtnTexture = activeSkin.customStyles[4].normal.background;
        float stageBtnHeight = contentContainerHeight * stagesBtnScale;
        float stageBtnWidth = stageBtnHeight * ((float)stageBtnTexture.width / (float)stageBtnTexture.height);
        float stageBtnXSpacing = contentContainerWidth * stagesXSpacingScale;
        float stageBtnYSpacing = contentContainerWidth * stagesYSpacingScale;

        float stagesWidth = stageBtnWidth * 5 + stageBtnXSpacing * 4;
        float stagesHeight = stageBtnHeight * 2 + stageBtnYSpacing;
        float stagesXOffset = (contentContainerWidth - stagesWidth) * 0.5f;
        float stagesYOffset = contentContainerHeight * stagesContainerYOffset;

        stagesContainerRect = new Rect(stagesXOffset, stagesYOffset, stagesWidth, stagesHeight);

        stage1Rect = new Rect(0, 0, stageBtnWidth, stageBtnHeight);
        stage2Rect = new Rect(stageBtnWidth + stageBtnXSpacing, 0, stageBtnWidth, stageBtnHeight);
        stage3Rect = new Rect(2 * (stageBtnWidth + stageBtnXSpacing), 0, stageBtnWidth, stageBtnHeight);
        stage4Rect = new Rect(3 * (stageBtnWidth + stageBtnXSpacing), 0, stageBtnWidth, stageBtnHeight);
        stage5Rect = new Rect(4 * (stageBtnWidth + stageBtnXSpacing), 0, stageBtnWidth, stageBtnHeight);

        stage6Rect = new Rect(0, stageBtnHeight + stageBtnYSpacing, stageBtnWidth, stageBtnHeight);
        stage7Rect = new Rect(stageBtnWidth + stageBtnXSpacing, stageBtnHeight + stageBtnYSpacing, stageBtnWidth, stageBtnHeight);
        stage8Rect = new Rect(2 * (stageBtnWidth + stageBtnXSpacing), stageBtnHeight + stageBtnYSpacing, stageBtnWidth, stageBtnHeight);
        stage9Rect = new Rect(3 * (stageBtnWidth + stageBtnXSpacing), stageBtnHeight + stageBtnYSpacing, stageBtnWidth, stageBtnHeight);
        stage10Rect = new Rect(4 * (stageBtnWidth + stageBtnXSpacing), stageBtnHeight + stageBtnYSpacing, stageBtnWidth, stageBtnHeight);

        // Set the button font size
        activeSkin.customStyles[5].fontSize = (int)(stageBtnHeight * stageBtnLabelScale);
        activeSkin.customStyles[6].fontSize = (int)(stageBtnHeight * stageBtnLabelScale);
        activeSkin.customStyles[7].fontSize = (int)(stageBtnHeight * stageBtnLabelScale);
        activeSkin.customStyles[8].fontSize = (int)(stageBtnHeight * stageBtnLabelScale);

        activeSkin.customStyles[5].contentOffset = new Vector2(stageBtnWidth * stageBtnLabelXOffset, stageBtnHeight * stageBtnLabelYOffset);
        activeSkin.customStyles[6].contentOffset = new Vector2(stageBtnWidth * stageBtnLabelXOffset, stageBtnHeight * stageBtnLabelYOffset);
        activeSkin.customStyles[7].contentOffset = new Vector2(stageBtnWidth * stageBtnLabelXOffset, stageBtnHeight * stageBtnLabelYOffset);
        activeSkin.customStyles[8].contentOffset = new Vector2(stageBtnWidth * stageBtnLabelXOffset, stageBtnHeight * stageBtnLabelYOffset);

        // Set the styles to put on each stage (need to check database here)
        stage1Style = new GUIStyle(activeSkin.customStyles[8]);
        stage2Style = new GUIStyle(activeSkin.customStyles[7]);
        stage3Style = new GUIStyle(activeSkin.customStyles[6]);
        stage4Style = new GUIStyle(activeSkin.customStyles[5]);
        stage5Style = new GUIStyle(activeSkin.customStyles[4]);
        stage6Style = new GUIStyle(activeSkin.customStyles[4]);
        stage7Style = new GUIStyle(activeSkin.customStyles[4]);
        stage8Style = new GUIStyle(activeSkin.customStyles[4]);
        stage9Style = new GUIStyle(activeSkin.customStyles[4]);
        stage10Style = new GUIStyle(activeSkin.customStyles[4]);

        // Set the button handlers
        stage1Handler = new ButtonHandler(stage1Rect, gameObject, 0.9f, "ScaleStage1Button");
        stage2Handler = new ButtonHandler(stage2Rect, gameObject, 0.9f, "ScaleStage2Button");
        stage3Handler = new ButtonHandler(stage3Rect, gameObject, 0.9f, "ScaleStage3Button");
        stage4Handler = new ButtonHandler(stage4Rect, gameObject, 0.9f, "ScaleStage4Button");
        stage5Handler = new ButtonHandler(stage5Rect, gameObject, 0.9f, "ScaleStage5Button");
        stage6Handler = new ButtonHandler(stage6Rect, gameObject, 0.9f, "ScaleStage6Button");
        stage7Handler = new ButtonHandler(stage7Rect, gameObject, 0.9f, "ScaleStage7Button");
        stage8Handler = new ButtonHandler(stage8Rect, gameObject, 0.9f, "ScaleStage8Button");
        stage9Handler = new ButtonHandler(stage9Rect, gameObject, 0.9f, "ScaleStage9Button");
        stage10Handler = new ButtonHandler(stage10Rect, gameObject, 0.9f, "ScaleStage10Button");

        #endregion

        #endregion
	}
	
	// Update is called once per frame
	void OnGUI ()
    {
        #region temp

        #endregion

        // Set the active skin
        GUI.skin = activeSkin;

        // The container
        GUI.BeginGroup(containerRect);
        {
            GUI.DrawTexture(bgRect, bgTexture, ScaleMode.ScaleAndCrop);
            // Draw the buttons
            Buttons();
            // Draw the main frame
            MainContent();
        }
        GUI.EndGroup();
    }

    #region GUI Sections

    void Buttons()
    {
        // Back button
        if (GUI.Button(backBtnRect, "", backBtnStyle))
        {
            navManager.NavToChapterSelect();
        }
        backBtnHandler.OnMouseOver(backBtnRect);

        // Sound Button
        sound = GUI.Toggle(soundBtnRect, sound, "", soundBtnStyle);
        soundBtnHandler.OnMouseOver(soundBtnRect);
    }
    void MainContent()
    {
        GUI.BeginGroup(contentContainerRect);
        {
            // Background texture
            GUI.DrawTexture(contentContainerBgRect, contentContainerBgTexture);

            // Header
            GUI.Label(contentHeaderRect, "Chapter 1", contentHeaderStyle);

            // Stage buttons
            GUI.BeginGroup(stagesContainerRect);
            {
                if (GUI.Button(stage1Rect, "1", stage1Style))
                {
                    Application.LoadLevel("C1-N1");
                }
                stage1Handler.OnMouseOver(stage1Rect);

                if (GUI.Button(stage2Rect, "2", stage2Style))
                {
                    Application.LoadLevel("C1-N2");
                }
                stage2Handler.OnMouseOver(stage2Rect);

                if (GUI.Button(stage3Rect, "3", stage3Style))
                {
                    Application.LoadLevel("C1-N3");
                }
                stage3Handler.OnMouseOver(stage3Rect);

                if (GUI.Button(stage4Rect, "4", stage4Style))
                {
                    Application.LoadLevel("C1-N4");
                }
                stage4Handler.OnMouseOver(stage4Rect);

                if (GUI.Button(stage5Rect, "5", stage5Style))
                {
                    Application.LoadLevel("C1-N5");
                }
                stage5Handler.OnMouseOver(stage5Rect);

                if (GUI.Button(stage6Rect, "6", stage6Style))
                {
                    Application.LoadLevel("C1-N6");
                }
                stage6Handler.OnMouseOver(stage6Rect);

                if (GUI.Button(stage7Rect, "7", stage7Style))
                {
                    Application.LoadLevel("C1-N7");
                }
                stage7Handler.OnMouseOver(stage7Rect);

                if (GUI.Button(stage8Rect, "8", stage8Style))
                {
                    Application.LoadLevel("C1-N8");
                }
                stage8Handler.OnMouseOver(stage8Rect);

                if (GUI.Button(stage9Rect, "9", stage9Style))
                {
                    Application.LoadLevel("C1-N9");
                }
                stage9Handler.OnMouseOver(stage9Rect);

                if (GUI.Button(stage10Rect, "10", stage10Style))
                {
                    Application.LoadLevel("C1-N10");
                }
                stage10Handler.OnMouseOver(stage10Rect);

            }
            GUI.EndGroup();
        }
        GUI.EndGroup();
    }

    #endregion
    #region Button Animators

    // animates the back button
    void ScaleBackButton(Rect size)
    {
        backBtnRect = size;
    }

    // animates the sound button
    void ScaleSoundButton(Rect size)
    {
        soundBtnRect = size;
    }

    // animate stage 1 button
    void ScaleStage1Button(Rect size)
    {
        stage1Rect = size;

        stage1Style.fontSize = (int)(size.height * stageBtnLabelScale);
        stage1Style.contentOffset = new Vector2(size.width * stageBtnLabelXOffset, size.height * stageBtnLabelYOffset);
    }

    // animate stage 2 button
    void ScaleStage2Button(Rect size)
    {
        stage2Rect = size;

        stage2Style.fontSize = (int)(size.height * stageBtnLabelScale);
        stage2Style.contentOffset = new Vector2(size.width * stageBtnLabelXOffset, size.height * stageBtnLabelYOffset);
    }

    // animate stage 3 button
    void ScaleStage3Button(Rect size)
    {
        stage3Rect = size;

        stage3Style.fontSize = (int)(size.height * stageBtnLabelScale);
        stage3Style.contentOffset = new Vector2(size.width * stageBtnLabelXOffset, size.height * stageBtnLabelYOffset);
    }

    // animate stage 4 button
    void ScaleStage4Button(Rect size)
    {
        stage4Rect = size;

        stage4Style.fontSize = (int)(size.height * stageBtnLabelScale);
        stage4Style.contentOffset = new Vector2(size.width * stageBtnLabelXOffset, size.height * stageBtnLabelYOffset);
    }

    // animate stage 5 button
    void ScaleStage5Button(Rect size)
    {
        stage5Rect = size;

        stage5Style.fontSize = (int)(size.height * stageBtnLabelScale);
        stage5Style.contentOffset = new Vector2(size.width * stageBtnLabelXOffset, size.height * stageBtnLabelYOffset);
    }

    // animate stage 6 button
    void ScaleStage6Button(Rect size)
    {
        stage6Rect = size;

        stage6Style.fontSize = (int)(size.height * stageBtnLabelScale);
        stage6Style.contentOffset = new Vector2(size.width * stageBtnLabelXOffset, size.height * stageBtnLabelYOffset);
    }

    // animate stage 7 button
    void ScaleStage7Button(Rect size)
    {
        stage7Rect = size;

        stage7Style.fontSize = (int)(size.height * stageBtnLabelScale);
        stage7Style.contentOffset = new Vector2(size.width * stageBtnLabelXOffset, size.height * stageBtnLabelYOffset);
    }

    // animate stage 8 button
    void ScaleStage8Button(Rect size)
    {
        stage8Rect = size;

        stage8Style.fontSize = (int)(size.height * stageBtnLabelScale);
        stage8Style.contentOffset = new Vector2(size.width * stageBtnLabelXOffset, size.height * stageBtnLabelYOffset);
    }

    // animate stage 9 button
    void ScaleStage9Button(Rect size)
    {
        stage9Rect = size;

        stage9Style.fontSize = (int)(size.height * stageBtnLabelScale);
        stage9Style.contentOffset = new Vector2(size.width * stageBtnLabelXOffset, size.height * stageBtnLabelYOffset);
    }

    // animate stage 10 button
    void ScaleStage10Button(Rect size)
    {
        stage10Rect = size;

        stage10Style.fontSize = (int)(size.height * stageBtnLabelScale);
        stage10Style.contentOffset = new Vector2(size.width * stageBtnLabelXOffset, size.height * stageBtnLabelYOffset);
    }

    #endregion
}