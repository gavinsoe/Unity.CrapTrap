using UnityEngine;
using System.Collections;

public class GameCompletedGUI : MonoBehaviour {

    /* GUI Skin
     * Custom Styles [0] = Background
     * Custom Styles [1] = Container Background
     * Custom Styles [2] = Header
     * Custom Styles [3] = Time Icon
     * Custom Styles [4] = NTP Icon
     * Custom Styles [5] = GTP Icon
     * Custom Styles [6] = Stat Label
     * Custom Styles [7] = Retry Button
     * Custom Styles [8] = Home Button
     * Custom Styles [9] = Next Button
     * Custom Styles [10] = Feedback Button
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

    #endregion
    #region Menu Frame

    Rect menuRect;
    Rect menuContainerRect;
    Texture menuTexture;
    private float menuHeight; // height of the pause menu
    private float menuWidth; // width of the pause menu
    private float menuXOffset; // Variables used to calculate x offsets for the menu
    private float menuYOffset; // Variables used to calculate y offsets for the menu

    #endregion
    #region header

    private Rect headerRect;
    private float headerYOffset = 0.12f;
    private float headerFontScale = 0.12f;

    #endregion
    #region Stats

    // Vars
    private string time;
    private int ntpCollected;
    private int ntpAvailable;
    private int gtpCollected;
    private int gtpAvailable;


    /* Button Offsets
     * -------------------------
     * |   IconA   |   stat A  |
     * -------------------------
     * |   IconB   |   stat B  |
     * -------------------------
     * |   IconC   |   stat C  |
     * -------------------------
     */

    Rect statsRect; //  Stats container
    private float statsXOffset = 0.312f;
    private float statsYOffset = 0.245f;

    private Texture A_iconTexture;
    private Texture B_iconTexture;
    private Texture C_iconTexture;

    // All icons have same width
    private float iconScale = 0.123f;
    private float iconWidth;
    private float A_iconHeight;
    private float B_iconHeight;
    private float C_iconHeight;

    private Rect A_iconRect;
    private Rect B_iconRect;
    private Rect C_iconRect;

    // All stats have same dimensions
    private float statLabelXOffset;
    private float statLabelHeight;

    private Rect A_statLabelRect;
    private Rect B_statLabelRect;
    private Rect C_statLabelRect;

    private float statVerticalSpacing;

    #endregion
    #region Navigation Buttons

    public Rect navContainerRect; // Navigation Container
    private float navContainerYOffset = 0.435f;

    private float navButtonScale = 0.34f;
    private float navButtonWidth;
    private float navButtonHeight;
    private float navButtonSpacingScale = -0.09f;

    private Rect retryBtnRect;
    private Rect homeBtnRect;
    private Rect nextBtnRect;
    #endregion
    #region Feedback Button

    private Rect feedbackBtnRect;
    private float feedbackFontScaling = 0.08f;
    private float feedbackBtnHeightScale = 1.9f;
    private float feedbackBtnWidthScale = 6.44f;
    private float feedbackXOffset = 0.64f;
    private float feedbackYOffset = 0.83f;

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

        // Initialise menu container variables
        menuTexture = activeSkin.customStyles[1].normal.background;
        menuWidth = Screen.width * 0.85f;
        menuHeight = menuWidth * ((float)menuTexture.height / (float)menuTexture.width);

        menuXOffset = Screen.width * 0.0725f;
        menuYOffset = menuHeight * 0f;
        menuContainerRect = new Rect(menuXOffset, menuYOffset, menuWidth, menuHeight);
        menuRect = new Rect(0, 0, menuWidth, menuHeight);

        // Initialise the header stuff
        activeSkin.customStyles[2].fontSize = (int)(Screen.height * headerFontScale);
        headerRect = new Rect(0, menuContainerRect.height * headerYOffset, menuContainerRect.width, activeSkin.customStyles[2].fontSize);

        #region Stats

        // Initialise the stats rect
        statsRect = new Rect(menuWidth * statsXOffset, menuHeight * statsYOffset, menuWidth * (1 - statsXOffset * 2), menuHeight * (1 - statsXOffset * 2));

        // Get the textures
        A_iconTexture = activeSkin.customStyles[3].normal.background;
        B_iconTexture = activeSkin.customStyles[4].normal.background;
        C_iconTexture = activeSkin.customStyles[5].normal.background;

        // Get the icon dimensions
        iconWidth = statsRect.width * iconScale;
        A_iconHeight = iconWidth * ((float)A_iconTexture.height / (float)A_iconTexture.width);
        B_iconHeight = iconWidth * ((float)B_iconTexture.height / (float)B_iconTexture.width);
        C_iconHeight = iconWidth * ((float)C_iconTexture.height / (float)C_iconTexture.width);

        // Vertical spacing
        statVerticalSpacing = Mathf.Max(A_iconHeight, B_iconHeight, C_iconHeight) * 1.05f;

        // Set the location of the icons
        A_iconRect = new Rect(0, 0, iconWidth, A_iconHeight);
        B_iconRect = new Rect(0, statVerticalSpacing, iconWidth, B_iconHeight);
        C_iconRect = new Rect(0, 2 * statVerticalSpacing, iconWidth, C_iconHeight);

        // Set the dimensions of the stat label
        statLabelHeight = Mathf.Max(A_iconHeight, B_iconHeight, C_iconHeight);
        activeSkin.customStyles[6].fontSize = (int)statLabelHeight;

        // set the locations of the stats
        A_statLabelRect = new Rect(0, 0, statsRect.width, statLabelHeight);
        B_statLabelRect = new Rect(0, statVerticalSpacing, statsRect.width, statLabelHeight);
        C_statLabelRect = new Rect(0, 2 * statVerticalSpacing, statsRect.width, statLabelHeight);

        #endregion
        #region Navigation

        navButtonHeight = statsRect.height * navButtonScale;
        navButtonWidth = navButtonHeight * ((float)activeSkin.customStyles[7].normal.background.width /
                                            (float)activeSkin.customStyles[7].normal.background.height);
        var navButtonSpacing = navButtonWidth * navButtonSpacingScale;

        float navWidth = navButtonWidth * 3 + navButtonSpacing * 2;
        navContainerRect = new Rect((menuWidth - navWidth) / 2, menuHeight * navContainerYOffset, navWidth, navButtonHeight);

        retryBtnRect = new Rect(0, 0, navButtonWidth, navButtonHeight);
        homeBtnRect = new Rect(navButtonWidth + navButtonSpacing, 0, navButtonWidth, navButtonHeight);
        nextBtnRect = new Rect(2 * (navButtonWidth + navButtonSpacing), 0, navButtonWidth, navButtonHeight);

        #endregion
        #region Feedback Button

        var fontSize = (int)(Screen.height * feedbackFontScaling);
        activeSkin.customStyles[10].fontSize = fontSize;
        var btnHeight = fontSize * feedbackBtnHeightScale;
        var btnWidth = fontSize * feedbackBtnWidthScale;
        feedbackBtnRect = new Rect(backgroundRect.width * feedbackXOffset, backgroundRect.height * feedbackYOffset, btnWidth, btnHeight);

        #endregion
	}
	
	void OnGUI()
    {
        #region temp

        
        #endregion

        if (show)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", containerRect, "to", openPosition, "onupdate", "AnimateGameCompletedMenu", "easetype", "easeOutBounce"));
            show = false;
        }
        else if (hide)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", containerRect, "to", closedPosition, "onupdate", "AnimateGameCompletedMenu", "easetype", "easeOutExpo"));
            hide = false;
        }

        // Set the active skin
        GUI.skin = activeSkin;
        // The container
        GUI.BeginGroup(containerRect);
        // Draw the background
        GUI.DrawTexture(backgroundRect, backgroundTexture, ScaleMode.ScaleAndCrop);
        // Draw the main display
        MainDisplay();

        if (GUI.Button(feedbackBtnRect, "feedback", activeSkin.customStyles[10]))
        {
            // Open Contact us modal
            Application.LoadLevel("GUI_Review");
        }

        GUI.EndGroup();
    }

    #region GUI sections

    void MainDisplay()
    {
        GUI.BeginGroup(menuContainerRect);

        GUI.DrawTexture(menuRect, menuTexture);

        GUI.Label(headerRect, "COMPLETED!",activeSkin.customStyles[2]);

        Stats();

        Navigation();

        GUI.EndGroup();
    }

    void Stats()
    {
        GUI.BeginGroup(statsRect);

        GUI.DrawTexture(A_iconRect, A_iconTexture);
        GUI.DrawTexture(B_iconRect, B_iconTexture);
        GUI.DrawTexture(C_iconRect, C_iconTexture);

        GUI.Label(A_statLabelRect, time, activeSkin.customStyles[6]);
        GUI.Label(B_statLabelRect, ntpCollected+"/"+ntpAvailable, activeSkin.customStyles[6]);
        GUI.Label(C_statLabelRect, gtpCollected+"/"+gtpAvailable, activeSkin.customStyles[6]);
        GUI.EndGroup();
    }

    void Navigation()
    {
        GUI.BeginGroup(navContainerRect);

        if (GUI.Button(retryBtnRect, "", activeSkin.customStyles[7]))
        {
            mainController.RetryLevel();
        }
        if (GUI.Button(homeBtnRect, "", activeSkin.customStyles[8]))
        {
            mainController.ReturnToTitle();
        }
        if (GUI.Button(nextBtnRect, "", activeSkin.customStyles[9]))
        {
            mainController.NextStage();
        }

        GUI.EndGroup();
    }
    #endregion

    void AnimateGameCompletedMenu(Rect newCoordinates)
    {
        containerRect = newCoordinates;
    }

    public void StageComplete(float _time, int _ntpCollected, int _ntpAvailable, int _gtpCollected, int _gtpAvailable)
    {
        int mins = (int)(_time / 60);
        int seconds = (int)(_time % 60);
        time = string.Format("{0:00}:{1:00}",mins,seconds);

        ntpCollected = _ntpCollected;
        ntpAvailable = _ntpAvailable;
        gtpCollected = _gtpCollected;
        gtpAvailable = _gtpAvailable;

        show = true;
    }
}
