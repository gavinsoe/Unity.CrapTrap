﻿using UnityEngine;
using System.Collections;

public class ChapterSelectGUI : MonoBehaviour 
{

    /* GUI Skin
     * Custom Styles [0] = Background
     * Custom Styles [1] = Poop
     * Custom Styles [2] = Achievements Button
     * Custom Styles [3] = Character Button
     * Custom Styles [4] = Item Shop Button
     * Custom Styles [5] = Sound Button
     * Custom Styles [6] = NTP Box
     * Custom Styles [7] = GTP Box
     * Custom Styles [8] = Chapter 1 Box
     * Custom Styles [9] = Chapter 2 Box
     * Custom Styles [10] = Chapter 3 Box
     */
    public GUISkin activeSkin;
    private MainGameController mainController;
    private NavigationManager navManager;

    #region GUI Related

    private Rect containerRect;  // The Rect object that encapsulates the whole page

    #region background

    private Rect bgRect; // Rect that holds the background
    private Texture bgTexture; // The background texture

    private Rect poopRect; // Poop
    private Texture poopTexture; // The poop texture
    private float poopScale = 0.2f; // Scale of the poop
    private float poopXOffset = 0.13f; // poop X Offset
    private float poopYOffset = 0.83f; // poop Y Offset

    #endregion
    #region buttons

    // Navigation Buttons
    private Rect navContainerRect;
    private float navContainerXOffset = 0f;
    private float navContainerYOffset = 0f;
    private float navButtonScale = 0.13f;
    private float navButtonSpacingScale = -0.08f;

    private Rect achievementsBtnRect;
    private GUIStyle achievementsBtnStyle;
    private ButtonHandler achievementsHandler;
    private Rect characterBtnRect;
    private GUIStyle characterBtnStyle;
    private ButtonHandler characterHandler;
    private Rect itemShopBtnRect;
    private GUIStyle itemShopBtnStyle;
    private ButtonHandler itemShopHandler;

    // Sound Buttons
    private bool sound = false;
    private Rect soundBtnRect;
    private GUIStyle soundBtnStyle;
    private ButtonHandler soundBtnHandler;
    private float soundBtnScale = 0.09f;
    private float soundBtnXOffset = 0.01f;
    private float soundBtnYOffset = 0.9f;

    #endregion
    #region currency boxes

    public int ntp = 0; // Keeps track of the number of ntp
    public int gtp = 0; // Keeps track of the number of gtp

    private Rect currencyNTPRect;
    private Rect currencyGTPRect;
    private GUIStyle NTPStyle;
    private GUIStyle GTPStyle;

    private float currencyBoxScale = 0.125f; // Variable used to scale the box for different window sizes
    private float currencyFontScale = 0.031f; // Variable used to scale font for different window sizes
    private float currencyXOffset = -0.135f; // Variable used to control the x offset of the text in the box
    private float currencyYOffset = 0.009f; // Variable used to control the y offset of the text in the box
    private float currencySpacing = 1.10f; // Spacing between the two currency boxes;
    private float currencyEdgeSpacing = 0.95f; // Space between the currency box and the edge of the screen.

    #endregion
    #region chapters

    private Rect chaptersContainerRect;

    private Rect chapter1Rect;
    private Texture chapter1Texture;
    private Rect chapter2Rect;
    private Texture chapter2Texture;
    private Texture chapter2LockedTexture;
    private Rect chapter3Rect;
    private Texture chapter3Texture;
    private Texture chapter3LockedTexture;

    private Rect chapterBgRect;
    private Rect chapterLabelRect;
    private GUIStyle chapterFontStyle;

    private float chapterSpacingScale = 0f;
    private float chapterBoxScale = 0.67f;
    private float chapterFontScale = 0.09f;
    private float chapterLabelYOffset = 0.425f;
    
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

        // Background texture
        bgRect = new Rect(0, 0, Screen.width, Screen.height);
        bgTexture = activeSkin.customStyles[0].normal.background;

        // Poop Texture
        poopTexture = activeSkin.customStyles[1].normal.background;
        float poopHeight = Screen.height * poopScale;
        float poopWidth = poopHeight * ((float)poopTexture.width / (float)poopTexture.height);
        poopRect = new Rect(Screen.width * poopXOffset, Screen.height * poopYOffset, poopWidth, poopHeight);

        #endregion
        #region Navigation buttons

        achievementsBtnStyle = activeSkin.customStyles[2];
        characterBtnStyle = activeSkin.customStyles[3];
        itemShopBtnStyle = activeSkin.customStyles[4];
        Texture navBtntexture = achievementsBtnStyle.normal.background;
        float navButtonHeight = Screen.height * navButtonScale;
        float navButtonWidth = navButtonHeight * ((float)navBtntexture.width / (float)navBtntexture.height);
        float navButtonSpacing = navButtonWidth * navButtonSpacingScale;

        float navWidth = navButtonWidth * 3 + navButtonSpacing * 2;
        float navXOffset = Screen.width * navContainerXOffset;
        float navYOffset = Screen.height * navContainerYOffset;
        navContainerRect = new Rect(navXOffset, navYOffset, navWidth, navButtonHeight);

        achievementsBtnRect = new Rect(0, 0, navButtonWidth, navButtonHeight);
        characterBtnRect = new Rect(navButtonWidth + navButtonSpacing, 0, navButtonWidth, navButtonHeight);
        itemShopBtnRect = new Rect(2 * (navButtonWidth + navButtonSpacing), 0, navButtonWidth, navButtonHeight);

        achievementsHandler = new ButtonHandler(achievementsBtnRect, gameObject, 0.9f, "ScaleAchievementsButton");
        characterHandler = new ButtonHandler(characterBtnRect, gameObject, 0.9f, "ScaleCharacterButton");
        itemShopHandler = new ButtonHandler(itemShopBtnRect, gameObject, 0.9f, "ScaleItemShopButton");

        #endregion
        #region Sound button

        soundBtnStyle = activeSkin.customStyles[5];
        Texture soundBtnTexture = soundBtnStyle.normal.background;
        float soundBtnHeight = Screen.height * soundBtnScale;
        float soundBtnWidth = soundBtnHeight * ((float)soundBtnTexture.width / (float)soundBtnTexture.height);
        float soundXOffset = Screen.width * soundBtnXOffset;
        float soundYOffset = Screen.height * soundBtnYOffset;
        soundBtnRect = new Rect(soundXOffset, soundYOffset, soundBtnWidth, soundBtnHeight);
        soundBtnHandler = new ButtonHandler(soundBtnRect, gameObject, 0.9f, "ScaleSoundButton");

        #endregion
        #region currency boxes

        NTPStyle = new GUIStyle(activeSkin.customStyles[6]);
        GTPStyle = new GUIStyle(activeSkin.customStyles[7]);
        Texture currencyTexture = NTPStyle.normal.background;
        float currencyBoxHeight = Screen.height * currencyBoxScale;
        float currencyBoxWidth = currencyBoxHeight * ((float)currencyTexture.width / (float)currencyTexture.height);

        // Font Scaling
        NTPStyle.fontSize = (int)(Screen.height * currencyFontScale);
        NTPStyle.contentOffset = new Vector2(currencyBoxWidth * currencyXOffset, currencyBoxHeight * currencyYOffset);
        GTPStyle.fontSize = (int)(Screen.height * currencyFontScale);
        GTPStyle.contentOffset = new Vector2(currencyBoxWidth * currencyXOffset, currencyBoxHeight * currencyYOffset);

        // Container Rect
        currencyNTPRect = new Rect(Screen.width - (currencySpacing * currencyBoxWidth), currencyBoxHeight * 0.1f, currencyBoxWidth, currencyBoxHeight);
        currencyGTPRect = new Rect(Screen.width - ((currencyEdgeSpacing + currencySpacing) * currencyBoxWidth), currencyBoxHeight * 0.1f, currencyBoxWidth, currencyBoxHeight);

        #endregion
        #region chapters

        chapterFontStyle = new GUIStyle(activeSkin.label);
        chapter1Texture = activeSkin.customStyles[8].onNormal.background;
        chapter2Texture = activeSkin.customStyles[9].onNormal.background;
        chapter2LockedTexture = activeSkin.customStyles[9].normal.background;
        chapter3Texture = activeSkin.customStyles[10].onNormal.background;
        chapter3LockedTexture = activeSkin.customStyles[10].normal.background;

        float chapterBoxHeight = Screen.height * chapterBoxScale;
        float chapterBoxWidth = chapterBoxHeight * ((float)chapter1Texture.width / (float)chapter1Texture.height);
        float chapterSpacing = chapterBoxWidth * chapterSpacingScale;
        float chaptersContainerWidth = chapterBoxWidth * 3 + chapterSpacing * 2;

        // Check if container surpasses the screen width and adjust accordingly
        if (chaptersContainerWidth > Screen.width)
        {
            chapterBoxWidth = Screen.width / (3 + chapterSpacingScale);
            chapterBoxHeight = chapterBoxWidth * ((float)chapter1Texture.height / (float)chapter1Texture.width);
            chapterSpacing = chapterBoxWidth * chapterSpacingScale;
            chaptersContainerWidth = chapterBoxWidth * 3 + chapterSpacing * 2;
        }

        chaptersContainerRect = new Rect((Screen.width - chaptersContainerWidth) * 0.5f,
                                         (Screen.height - chapterBoxHeight) * 0.5f,
                                         chaptersContainerWidth, chapterBoxHeight);
        chapter1Rect = new Rect(0, 0, chapterBoxWidth, chapterBoxHeight);
        chapter2Rect = new Rect(chapterBoxWidth + chapterSpacing, 0, chapterBoxWidth, chapterBoxHeight);
        chapter3Rect = new Rect(2 * (chapterBoxWidth + chapterSpacing), 0, chapterBoxWidth, chapterBoxHeight);
        chapterBgRect = new Rect(0, 0, chapterBoxWidth, chapterBoxHeight);

        float labelYOffset = chapterBoxHeight * chapterLabelYOffset;
        chapterLabelRect = new Rect(0, labelYOffset, chapterBoxWidth, chapterBoxHeight - labelYOffset);
        chapterFontStyle.fontSize = (int)(chapterFontScale * chapterBoxHeight);

        #endregion
    }

    void OnGUI()
    {
        #region temp


        #endregion

        // Set the active skin
        GUI.skin = activeSkin;
        // The container
        GUI.BeginGroup(containerRect);
        {
            GUI.DrawTexture(bgRect, bgTexture, ScaleMode.ScaleAndCrop);
            GUI.DrawTexture(poopRect, poopTexture);
            // Draw the buttons
            Buttons();
            // Currency box
            Currency();
            // Chapters
            Chapters();
        }
        GUI.EndGroup();
    }

    #region GUI Sections

    void Buttons()
    {
        // Navigation buttons
        GUI.BeginGroup(navContainerRect);
        {
            if (GUI.Button(achievementsBtnRect, "", achievementsBtnStyle))
            {
                navManager.NavToAchievements();
            }
            achievementsHandler.OnMouseOver(achievementsBtnRect);

            if (GUI.Button(characterBtnRect, "", characterBtnStyle))
            {
                navManager.NavToCharacterPage();
            }
            characterHandler.OnMouseOver(characterBtnRect);

            if (GUI.Button(itemShopBtnRect, "", itemShopBtnStyle))
            {
                navManager.NavToItemShop();
            }
            itemShopHandler.OnMouseOver(itemShopBtnRect);
        } 
        GUI.EndGroup();

        // Sound Button
        sound = GUI.Toggle(soundBtnRect, sound, "", soundBtnStyle);
        //mainController.ToggleSound(!sound);
        soundBtnHandler.OnMouseOver(soundBtnRect);
    }

    void Currency()
    {
        // NTP
        GUI.Label(currencyNTPRect, ntp.ToString(), NTPStyle);
        // GTP
        GUI.Label(currencyGTPRect, gtp.ToString(), GTPStyle);
    }

    void Chapters()
    {
        GUI.BeginGroup(chaptersContainerRect);
        {
            GUI.BeginGroup(chapter1Rect);
            {
                GUI.DrawTexture(chapterBgRect, chapter1Texture);
                GUI.Label(chapterLabelRect, "Chapter 1\n 3/10", chapterFontStyle);
                if (GUI.Button(chapterBgRect, ""))
                {
                    Application.LoadLevel("GUI_Chapter1StageSelect");
                }
            }
            GUI.EndGroup();

            GUI.BeginGroup(chapter2Rect);
            {
                GUI.DrawTexture(chapterBgRect, chapter2LockedTexture);
            }
            GUI.EndGroup();

            GUI.BeginGroup(chapter3Rect);
            {
                GUI.DrawTexture(chapterBgRect, chapter2LockedTexture);
            }
            GUI.EndGroup();
        }
        GUI.EndGroup();
    }

    #endregion
    #region Button Animators

    // animate the achievements button
    void ScaleAchievementsButton(Rect size)
    {
        achievementsBtnRect = size;
    }

    // animate the character button
    void ScaleCharacterButton(Rect size)
    {
        characterBtnRect = size;
    }

    // animate the item shop button
    void ScaleItemShopButton(Rect size)
    {
        itemShopBtnRect = size;
    }

    // animate the sound button
    void ScaleSoundButton(Rect size)
    {
        soundBtnRect = size;
    }

    #endregion
}