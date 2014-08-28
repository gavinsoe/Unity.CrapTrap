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
    private NavigationManager navManager;

    // Triggers
    public bool show = false;
    public bool hide = false;

    #region GUI related

    private Rect containerRect;  // The Rect object that encapsulates the whole page

    #region Background
    private Rect bgContainerRect; 
    private Rect bgRect; // The Rect object that holds the background of the page
    private Rect bgRectOpen; // The position of the background when open
    private Rect bgRectClose; // The position of the background when closed
    private float bgAlpha; // The transparency of the background
    private Texture poopTexture; // The poop texture

    #endregion

    private float guiAlpha;
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

    // audio clips
    public AudioClip fart;

    // Use this for initialization
    void Awake()
    {
        // Retrieve the main game controller
        mainController = gameObject.GetComponentInChildren<MainGameController>();

        // Retrieve the nav manager
        navManager = gameObject.GetComponentInChildren<NavigationManager>();

        // Set the size of the frame
        containerRect = new Rect(0, 0, Screen.width, Screen.height);

        #region background

        bgRectOpen = new Rect(0, 0, Screen.width, Screen.height);
        bgRectClose = new Rect(0, Screen.height, 0, Screen.height);
        bgContainerRect = bgRectClose;

        // Set the page open/closed positions

        // Initialise menu background variables
        bgRect = new Rect(0, 0, Screen.width, Screen.height); ; // Set initial background position to closed
        bgAlpha = 0; // Set initial background to transparency to 0
        poopTexture = activeSkin.customStyles[1].normal.background; // grab the background texture

        #endregion
        #region Text and navigation

        guiAlpha = 0; // Transparency of every other GUI stuff

        // header stuff
        activeSkin.customStyles[2].fontSize = (int)(Screen.height * headerFontScale);
        headerRect = new Rect(0, containerRect.height * headerYOffset, containerRect.width, activeSkin.customStyles[2].fontSize);
        
        // navigation buttons
        navButtonHeight = containerRect.height * navButtonScale;
        navButtonWidth = navButtonHeight * ((float)activeSkin.customStyles[3].normal.background.width /
                                            (float)activeSkin.customStyles[3].normal.background.height);
        var navButtonSpacing = navButtonWidth * navButtonSpacingScale;

        float navWidth = navButtonWidth * 2 + navButtonSpacing * 1;
        navContainerRect = new Rect((containerRect.width - navWidth) / 2, containerRect.height * navContainerYOffset, navWidth, navButtonHeight);

        retryBtnRect = new Rect(0, 0, navButtonWidth, navButtonHeight);
        homeBtnRect = new Rect(navButtonWidth + navButtonSpacing, 0, navButtonWidth, navButtonHeight);

        #endregion
	}

    void Start()
    {
        this.enabled = false;
    }

	void OnGUI()
    {
        //Set the active skin
        GUI.skin = activeSkin;

        // The container
        GUI.BeginGroup(containerRect);
        {
            // Draw the poop
            GUI.BeginGroup(bgContainerRect);
            {
                //GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, bgAlpha);
                GUI.DrawTexture(bgRect, poopTexture, ScaleMode.ScaleAndCrop);
            }
            GUI.EndGroup();

            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, guiAlpha);
            // Draw the header
            GUI.Label(headerRect, "YOU\nFAILED", activeSkin.customStyles[2]);

            #region navigation

            GUI.BeginGroup(navContainerRect);

            if (GUI.Button(retryBtnRect, "", activeSkin.customStyles[3]))
            {
                navManager.RetryLevel();
            }
            if (GUI.Button(homeBtnRect, "", activeSkin.customStyles[4]))
            {
                navManager.NavToTitle();
            }

            GUI.EndGroup();

            #endregion
        }
        GUI.EndGroup();
    }

    void AnimatePoopTransparency(float alpha)
    {
        bgAlpha = alpha;
    }

    void AnimateGUITransparency(float alpha)
    {
        guiAlpha = alpha;
    }

    void AnimateFailedMenu(Rect size)
    {
        bgContainerRect = size;
        bgRect.y = -size.y;
    }

    // Show the menu
    public void Show()
    {
        this.enabled = true;
        audio.PlayOneShot(fart, 1f);
        iTween.ValueTo(gameObject,
                          iTween.Hash("from", bgContainerRect,
                                      "to", bgRectOpen,
                                      "onupdate", "AnimateFailedMenu",
                                      "easetype", iTween.EaseType.easeOutCirc,
                                      "time", 0.4f));
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", bgAlpha,
                                   "to", 1,
                                   "onupdate", "AnimatePoopTransparency",
                                   "easetype", iTween.EaseType.easeInQuart,
                                   "time", 0.5f));

        iTween.ValueTo(gameObject,
                       iTween.Hash("from", guiAlpha,
                                   "to", 1,
                                   "onupdate", "AnimateGUITransparency",
                                   "easetype", iTween.EaseType.easeInQuart,
                                   "time", 1f));
    }
}
