using UnityEngine;
using System.Collections;

public class FailedByFallingGUI : MonoBehaviour
{
    public static FailedByFallingGUI instance;
    /* GUI Skin
     * Custom Styles [0] = Background
     * Custom Styles [1] = Splattered Poop
     * Custom Styles [2] = Header
     * Custom Styles [3] = Retry Button
     * Custom Styles [4] = Home Button
     */
    public GUISkin activeSkin;
    private GameObject backgroundObject;

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

    void Awake()
    {
        // set the static variable so that other classes can easily use this class
        instance = this;

        // Hide on start
        iTween.FadeTo(gameObject, 0, 0);

        // Set the container position
        containerRect = new Rect(0, 0, Screen.width, Screen.height);

        #region Calculate GUI Components

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

    // Use this for initialization
    void Start()
    {
    }

    void OnGUI()
    {
        // Sets the GUI depth
        GUI.depth = 10;

        //Set the active skin
        GUI.skin = activeSkin;
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, color_alpha);

        // The container
        GUI.BeginGroup(containerRect);
        // Draw the header
        GUI.Label(headerRect, "YOU FELL", activeSkin.customStyles[2]);
        #region navigation
        GUI.BeginGroup(navContainerRect);

        if (GUI.Button(retryBtnRect, "", activeSkin.customStyles[3]))
        {
            NavigationManager.instance.RetryLevel();
        }
        if (GUI.Button(homeBtnRect, "", activeSkin.customStyles[4]))
        {
            NavigationManager.instance.NavToChapterSelect();
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
        this.enabled = true;
        iTween.FadeTo(gameObject, 1, 0.2f);
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", color_alpha,
                                   "to", 1,
                                   "onupdate", "AnimateFailedMenu",
                                   "easetype", iTween.EaseType.easeOutQuart));
        // Alter culling mask to hide player
        Camera.main.cullingMask = ~(1 << LayerMask.NameToLayer("Character") | 1 << LayerMask.NameToLayer("Minimap"));

    }

    public void HideFailedScreen()
    {
        iTween.FadeTo(gameObject, 0, 0.2f);
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", color_alpha,
                                   "to", 0,
                                   "onupdate", "AnimateFailedMenu",
                                   "easetype", iTween.EaseType.easeInQuart));
        this.enabled = false;
    }
}
