﻿using UnityEngine;
using System.Collections;

public class FailedByFallingGUI : MonoBehaviour
{

    /* GUI Skin
     * Custom Styles [0] = Background
     * Custom Styles [1] = Splattered Poop
     * Custom Styles [2] = Header
     * Custom Styles [3] = Retry Button
     * Custom Styles [4] = Home Button
     */
    public GUISkin activeSkin;
    private MainGameController mainController;
    private GameObject backgroundObject;

    // Triggers
    public bool show = false;
    public bool hide = false;

    #region GUI related
    private float color_alpha = 0; // GUI transparency

    private Rect containerRect;  // The Rect object that encapsulates the whole page

    #region header

    private Rect headerRect;
    private float headerYOffset = 0.19f;
    private float headerFontScale = 0.19f;

    #endregion
    #region Navigation Buttons

    private Rect navContainerRect; // Navigation Container
    private float navContainerYOffset = 0.67f;

    private float navButtonScale = 0.1739365f;
    private float navButtonWidth;
    private float navButtonHeight;
    private float navButtonSpacingScale = -0.09f;

    private Rect retryBtnRect;
    private Rect homeBtnRect;
    #endregion
    #endregion

    // Use this for initialization
    void Start()
    {
        // Retrieve the main game controller
        mainController = Camera.main.GetComponentInChildren<MainGameController>();

        // Set the container position
        containerRect = new Rect(0, 0, Screen.width, Screen.height);

        // Hide on start
        iTween.FadeTo(gameObject, 0, 0);

        #region temp

        // Initialise the header stuff
        activeSkin.customStyles[2].fontSize = (int)(Screen.height * headerFontScale);
        headerRect = new Rect(0, containerRect.height * headerYOffset, containerRect.width, activeSkin.customStyles[2].fontSize);

        #region Navigation

        navButtonHeight = containerRect.height * navButtonScale;
        navButtonWidth = navButtonHeight * ((float)activeSkin.customStyles[3].normal.background.width /
                                            (float)activeSkin.customStyles[3].normal.background.height);
        var navButtonSpacing = navButtonWidth * navButtonSpacingScale;

        float navWidth = navButtonWidth * 2 + navButtonSpacing * 1;
        navContainerRect = new Rect((containerRect.width - navWidth) / 2, containerRect.height * navContainerYOffset, navWidth, navButtonHeight);

        retryBtnRect = new Rect(0, 0, navButtonWidth, navButtonHeight);
        homeBtnRect = new Rect(navButtonWidth + navButtonSpacing, 0, navButtonWidth, navButtonHeight);

        #endregion
        #endregion

        this.enabled = false;
    }

    void OnGUI()
    {
        if (show)
        {
            iTween.FadeTo(gameObject, 1, 0.2f);
            iTween.ValueTo(gameObject, iTween.Hash("from", color_alpha, "to", 1, "onupdate", "AnimateFailedMenu", "easetype", iTween.EaseType.easeOutQuart));
            show = false;
        }
        else if (hide)
        {
            iTween.FadeTo(gameObject, 0, 0.2f);
            iTween.ValueTo(gameObject, iTween.Hash("from", color_alpha, "to", 0, "onupdate", "AnimateFailedMenu", "easetype", iTween.EaseType.easeInQuart));
            hide = false;
        }

        //Set the active skin
        GUI.skin = activeSkin;
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, color_alpha);

        // The container
        GUI.BeginGroup(containerRect);
        // Draw the header
        GUI.Label(headerRect, "YOU DIED", activeSkin.customStyles[2]);
        #region navigation
        GUI.BeginGroup(navContainerRect);

        if (GUI.Button(retryBtnRect, "", activeSkin.customStyles[3]))
        {
            mainController.RetryLevel();
        }
        if (GUI.Button(homeBtnRect, "", activeSkin.customStyles[4]))
        {
            mainController.ReturnToTitle();
        }

        GUI.EndGroup();
        #endregion
        GUI.EndGroup();
    }

    void AnimateFailedMenu(float alpha)
    {
        color_alpha = alpha;
    }

    public void StageFailed()
    {
        show = true;
    }
}