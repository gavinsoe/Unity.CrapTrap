using UnityEngine;
using System.Collections;

public class FailedGUI : MonoBehaviour {
    
    /* GUI Skin
     * Custom Styles [0] = Background
     * Custom Styles [1] = Splattered Poop
     * Custom Styles [2] = Header
     * Custom Styles [3] = Retry Button
     * Custom Styles [4] = Home Button
     */
    public GUISkin activeSkin;
    private MainGameController mainController;

    // Triggers
    public bool show = false;
    public bool hide = false;

    #region GUI related

    private Rect containerRect;  // The Rect object that encapsulates the whole page

    private Rect openPosition; // Position of the page when it is shown
    private Rect closedPosition; // Position of the page when it is hidden

    #region Background

    private Rect backgroundRect; // The Rect object that holds the background of the page
    private Texture backgroundTexture; // The texture for the pause menu background
    private Texture poopTexture; //

    #endregion
    #region header

    private Rect headerRect;
    private float headerYOffset = 0.29f;
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
        mainController = gameObject.GetComponentInChildren<MainGameController>();

        // Set the page open/closed positions
        openPosition = new Rect(0, 0, Screen.width, Screen.height);
        closedPosition = new Rect(0, Screen.height, Screen.width, Screen.height);

        // Set the page to start as closed.
        containerRect = closedPosition;

        // Initialise menu background variables
        backgroundRect = new Rect(0, 0, Screen.width, Screen.height);
        backgroundTexture = activeSkin.customStyles[0].normal.background;
        poopTexture = activeSkin.customStyles[1].normal.background;

        
	}

	void OnGUI()
    {
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
        if (show)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", containerRect, "to", openPosition, "onupdate", "AnimateFailedMenu", "easetype", iTween.EaseType.easeOutQuart));
            show = false;
        }
        else if (hide)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", containerRect, "to", closedPosition, "onupdate", "AnimateFailedMenu", "easetype", iTween.EaseType.easeInQuart));
            hide = false;
        }

        //Set the active skin
        GUI.skin = activeSkin;

        // The container
        GUI.BeginGroup(containerRect);
        // Draw the background
        GUI.DrawTexture(backgroundRect, backgroundTexture, ScaleMode.ScaleAndCrop);
        // Draw the header
        GUI.Label(headerRect, "YOU\nFAILED",activeSkin.customStyles[2]);
        // Draw the poop
        GUI.DrawTexture(backgroundRect, poopTexture, ScaleMode.ScaleAndCrop);
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

    void AnimateFailedMenu(Rect newCoordinates)
    {
        containerRect = newCoordinates;
    }

    public void StageFailed()
    {
        show = true;
    }
}
