using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StageCompleteGUI : MonoBehaviour 
{
    public static StageCompleteGUI instance;

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
     * Custom Styles [11] = Star
     * Custom Styles [12] = Star Shine
     * Custom Styles [13] = Tick Box
     * Custom Styles [14] = Tick
     * Custom Styles [15] = Arrow Next
     * Custom Styles [16] = Transparent Button
     * Custom Styles [17] = Capsule Closed
     * Custom Styles [18] = Capsule Top
     * Custom Styles [19] = Capsule Bot
     * Custom Styles [20] = Capsule Shine
     */
    public GUISkin activeSkin;
    private int screen = 0;
    private bool allowTransition;

    #region GUI related

    private Rect containerRect;  // The Rect object that encapsulates the whole page

    private Rect openPosition; // Position of the page when it is shown
    private Rect closedPosition; // Position of the page when it is hidden

    #region Background

    private Rect backgroundRect; // The Rect object that holds the background of the page
    private Texture backgroundTexture; // The texture for the pause menu background

    #endregion
    #region Menu Frame

    private Rect menuRect;
    private Rect menuContainerRect;
    private Texture menuTexture;
    private float menuScale = 1.21f;
    private float menuHeight; // height of the pause menu
    private float menuWidth; // width of the pause menu
    private float menuXOffset; // Variables used to calculate x offsets for the menu
    private float menuYOffset; // Variables used to calculate y offsets for the menu

    /* Star ratings
     * ╔═══════════╦═══════════╦═══════════╗
     * ║   Star1   ║   Star2   ║   Star3   ║
     * ╚═══════════╩═══════════╩═══════════╝
     */
    private Rect starsContainerRect; // Box that contains all 3 stars

    private Texture starTexture; // Texture of the star
    private Rect star1Rect; // Position and size of star 1
    private Rect star2Rect; // Position and size of star 2
    private Rect star3Rect; // Position and size of star 3
    private Rect star1ShowRect; // Position and size of star 1 when shown
    private Rect star2ShowRect; // Position and size of star 1 when shown
    private Rect star3ShowRect; // Position and size of star 1 when shown

    private Texture starShineTexture; // Texture of the shine
    private Rect starShine1Rect; // Position and size of shine 1
    private Rect starShine2Rect; // Position and size of shine 2
    private Rect starShine3Rect; // Position and size of shine 3

    private float starScale = 0.0997f;
    private float starContainerXOffset = 0.331f;
    private float starContainerYOffset = 0.035f;
    private float starsSpacingScale = 0.02f;

    private Vector2 star1Pivot; // The pivot point of star 1 for rotation
    private Vector2 star2Pivot; // The pivot point of star 2 for rotation
    private Vector2 star3Pivot; // The pivot point of star 3 for rotation
    private int shine1Rotation; // used to animate the rotation of shine 1
    private int shine2Rotation; // used to animate the rotation of shine 2
    private int shine3Rotation; // used to animate the rotation of shine 3
    private float shine1Alpha; // used to animate the alpha value of shine 1
    private float shine2Alpha; // used to animate the alpha value of shine 2
    private float shine3Alpha; // used to animate the alpha value of shine 3

    private bool showStar1;
    private bool showStar2;
    private bool showStar3;
    #endregion

    private Rect contentContainerRect;
    private float contentContainerXOffset = 0.17f;
    private float contentContainerYOffset = 0.195f;
    private float contentContainerWidth = 0.665f;
    private float contentContainerHeight = 0.45f;

    private Rect innerContentContainerRect;
    private Rect innerContentTransitionRect;
    private Rect innerContainerPosLeft;
    private Rect innerContainerPosMid;
    private Rect innerContainerPosRight;

    private int transitionTo; // Screen to transition to

    #region header

    private Rect headerRect;
    private float headerYOffset = 0.04f;
    private float headerFontScale = 0.08f;

    #endregion
    #region Objectives

    private Texture tickboxTexture;
    private Texture tickTexture;

    private float tickboxScale = 0.18f;

    private Rect objectivesRect;
    private float objectivesXOffset = 0.09f;
    private float objectivesYOffset = 0.23f;
    private float objectivesVerticalSpacing;

    private Rect tickbox1Rect;
    private Rect tickbox2Rect;
    private Rect tickbox3Rect;

    private Rect tick1ContainerRect;
    private Rect tick2ContainerRect;
    private Rect tick3ContainerRect;

    private Rect tickRect;

    private Rect objective1Rect;
    private Rect objective2Rect;
    private Rect objective3Rect;

    private string objective1;
    private string objective2;
    private string objective3;
    private Objective[] objectives;
    private int objCompleteCount;

    private GUIStyle objectiveLabelStyle;
    private float objectiveFontScale = 0.4f;

    private Texture arrowNextTexture;
    private float arrowScale = 0.16f;
    private Rect arrowNextRect;
    private float arrowXOffset = 0.93f;
    private float arrowYOffset = 0.39f;

    #endregion
    #region Treasure

    private int capsulesMax;
    private int capsulesCollected;
    private Texture loot1 = null;
    private Texture loot2 = null;
    private Texture loot3 = null;
    private Rect treasureRect;

    private Rect A_treasureItemContainerRect;
    private Rect B_treasureItemContainerRect;
    private Rect C_treasureItemContainerRect;
    private Rect D_treasureItemContainerRect;
    private Rect E_treasureItemContainerRect;

    private Rect capsuleTopRect;
    private Rect capsuleBotRect;
    private Rect capsuleEmptyRect;
    private Texture capsuleTopTexture;
    private Texture capsuleBotTexture;
    private Texture capsuleEmptyTexture;

    private Rect capsuleTopOpenRect;
    private Rect capsuleBotOpenRect;
    private float openOffset = 0.2f;
    private Rect capsuleItemRect;
    private float capsuleItemScale = 0.8f;
    private Rect capsuleShineRect;
    private Texture capsuleShineTexture;
    private Rect capsuleShineOpenRect;
    private float capsuleShineAlpha;
    private float shineScale = 1.2f;

    private float treasuresXOffset = 0f;
    private float treasuresYOffset = 0f;
    private float capsuleSpacing = 0.05f;

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

    private Rect statsRect; //  Stats container
    private float statsXOffset = 0.3f;
    private float statsYOffset = 0.27f;
    
    private Texture A_iconTexture;
    private Texture B_iconTexture;
    private Texture C_iconTexture;

    // All icons have same width
    private float iconScale = 0.05f;
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

    private Rect navContainerRect; // Navigation Container
    private float navContainerYOffset = 0.689f;
    private float navAlpha; // used to animate the alpha value of navigation

    private float navButtonScale = 0.13f;
    private float navButtonWidth;
    private float navButtonHeight;
    private float navButtonSpacingScale = -0.09f;

    private Rect retryBtnRect;
    private Rect homeBtnRect;
    private Rect nextBtnRect;

    #endregion

    #endregion

    void Awake()
    {
        // set the static variable so that other classes can easily use this class
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
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
        menuHeight = Screen.height * menuScale;
        menuWidth = menuHeight * ((float)menuTexture.width / (float)menuTexture.height);

        menuXOffset = (Screen.width - menuWidth) * 0.5f;
        menuYOffset = menuHeight * 0f;
        menuContainerRect = new Rect(menuXOffset, menuYOffset, menuWidth, menuHeight);
        menuRect = new Rect(0, 0, menuWidth, menuHeight);

        #region stars

        starTexture = activeSkin.customStyles[11].normal.background;
        float starHeight = menuHeight * starScale;
        float starWidth = starHeight * ((float)starTexture.width / (float)starTexture.height);
        float starIconSpacing = starWidth * starsSpacingScale;

        starShineTexture = activeSkin.customStyles[12].normal.background;
        float starShineHeight = starHeight * 1.5f;
        float starShineWidth = starShineHeight * ((float)starShineTexture.width / (float)starShineTexture.height);

        float starContainerWidth = starWidth * 3 + starIconSpacing * 2 + (starShineWidth - starWidth);
        float starContainerHeight = starShineHeight;

        starsContainerRect = new Rect(menuWidth * starContainerXOffset, menuWidth * starContainerYOffset, starContainerWidth, starContainerHeight);

        star1ShowRect = new Rect((starShineWidth - starWidth) * 0.5f, (starShineHeight - starHeight) * 0.5f, starWidth, starHeight);
        star2ShowRect = new Rect((starShineWidth - starWidth) * 0.5f + starWidth + starIconSpacing, (starShineHeight - starHeight) * 0.5f, starWidth, starHeight);
        star3ShowRect = new Rect((starShineWidth - starWidth) * 0.5f + 2 * (starWidth + starIconSpacing), (starShineHeight - starHeight) * 0.5f, starWidth, starHeight);

        starShine1Rect = new Rect(0, 0, starShineWidth, starShineHeight);
        starShine2Rect = new Rect(starWidth + starIconSpacing, 0, starShineWidth, starShineHeight);
        starShine3Rect = new Rect(2 * (starWidth + starIconSpacing), 0, starShineWidth, starShineHeight);

        star1Pivot = new Vector2(starShine1Rect.x + starShine1Rect.width * 0.5f, starShine1Rect.height * 0.5f);
        star2Pivot = new Vector2(starShine2Rect.x + starShine2Rect.width * 0.5f, starShine2Rect.height * 0.5f);
        star3Pivot = new Vector2(starShine3Rect.x + starShine3Rect.width * 0.5f, starShine3Rect.height * 0.5f);

        star1Rect = new Rect(star1Pivot.x, star1Pivot.y, 0, 0);
        star2Rect = new Rect(star2Pivot.x, star2Pivot.y, 0, 0);
        star3Rect = new Rect(star3Pivot.x, star3Pivot.y, 0, 0);

        #endregion

        // the content container
        contentContainerRect = new Rect(menuWidth * contentContainerXOffset,
                                        menuHeight * contentContainerYOffset,
                                        menuWidth * contentContainerWidth,
                                        menuHeight * contentContainerHeight);

        // Initialise the header stuff
        activeSkin.customStyles[2].fontSize = (int)(Screen.height * headerFontScale);
        headerRect = new Rect(0, contentContainerRect.height * headerYOffset, contentContainerRect.width, activeSkin.customStyles[2].fontSize);

        #region objectives

        tickboxTexture = activeSkin.customStyles[13].normal.background;
        tickTexture = activeSkin.customStyles[14].normal.background;

        float tickboxHeight = contentContainerRect.height * tickboxScale;
        float tickboxWidth = tickboxHeight * ((float)tickboxTexture.width / (float)tickboxTexture.height);

        // Initialise inner container Rects
        innerContainerPosLeft = new Rect(-contentContainerRect.width, 0, contentContainerRect.width, contentContainerRect.height);
        innerContainerPosMid = new Rect(0, 0, contentContainerRect.width, contentContainerRect.height);
        innerContainerPosRight = new Rect(contentContainerRect.width, 0, contentContainerRect.width, contentContainerRect.height);
        innerContentContainerRect = innerContainerPosMid;
        innerContentTransitionRect = innerContainerPosRight;

        // Initialise the objectives rect
        objectivesRect = new Rect(contentContainerRect.width * objectivesXOffset, 
                                  contentContainerRect.height * objectivesYOffset, 
                                  contentContainerRect.width * (1 - objectivesXOffset * 2), 
                                  contentContainerRect.height * (1 - objectivesYOffset));
        objectivesVerticalSpacing = tickboxHeight;

        // set the objectives dimensions
        tickbox1Rect = new Rect(0, 0, tickboxWidth, tickboxHeight);
        tickbox2Rect = new Rect(0, objectivesVerticalSpacing, tickboxWidth, tickboxHeight);
        tickbox3Rect = new Rect(0, objectivesVerticalSpacing * 2, tickboxWidth, tickboxHeight);

        tick1ContainerRect = new Rect(0, 0, 0, tickboxHeight);
        tick2ContainerRect = new Rect(0, objectivesVerticalSpacing, 0, tickboxHeight);
        tick3ContainerRect = new Rect(0, objectivesVerticalSpacing * 2, 0, tickboxHeight);

        tickRect = new Rect(0, 0, tickboxWidth, tickboxHeight);

        objective1Rect = new Rect(tickboxWidth * 0.75f, 0, objectivesRect.width - tickboxWidth, tickboxHeight);
        objective2Rect = new Rect(tickboxWidth * 0.75f, objectivesVerticalSpacing, objectivesRect.width - tickboxWidth, tickboxHeight);
        objective3Rect = new Rect(tickboxWidth * 0.75f, objectivesVerticalSpacing * 2, objectivesRect.width - tickboxWidth, tickboxHeight);

        objectiveLabelStyle = activeSkin.label;
        objectiveLabelStyle.fontSize = (int)(objectiveFontScale * tickboxHeight);
        objectiveLabelStyle.alignment = TextAnchor.LowerLeft;

        arrowNextTexture = activeSkin.customStyles[15].normal.background;
        float arrowHeight = contentContainerRect.height * arrowScale;
        float arrowWidth = arrowHeight * ((float)arrowNextTexture.width / (float)arrowNextTexture.height);
        arrowNextRect = new Rect(contentContainerRect.width * arrowXOffset, contentContainerRect.height * arrowYOffset, arrowWidth, arrowHeight);

        #endregion
        #region treasures

        capsuleEmptyTexture = activeSkin.customStyles[17].normal.background;
        capsuleTopTexture = activeSkin.customStyles[18].normal.background;
        capsuleBotTexture = activeSkin.customStyles[19].normal.background;

        // Initialise the treasure rect
        treasureRect = new Rect(0, 0, contentContainerRect.width, contentContainerRect.height);
        treasureRect = new Rect(contentContainerRect.width * treasuresXOffset,
                                contentContainerRect.height * treasuresYOffset,
                                contentContainerRect.width * (1 - treasuresXOffset * 2),
                                contentContainerRect.height * (1 - treasuresYOffset));

        float spacing = treasureRect.width * capsuleSpacing;
        float capsuleWidth = (treasureRect.width - (spacing * 4)) / 3;
        float capsuleHeight = capsuleWidth * ((float)capsuleEmptyTexture.height / (float)capsuleEmptyTexture.width);

        capsuleEmptyRect = new Rect(0, 0.5f * (treasureRect.height - capsuleHeight), capsuleWidth, capsuleHeight);
        capsuleTopRect = capsuleEmptyRect;
        capsuleBotRect = capsuleEmptyRect;
        capsuleTopOpenRect = new Rect(0, capsuleEmptyRect.y - (treasureRect.height * openOffset), capsuleWidth, capsuleHeight);
        capsuleBotOpenRect = new Rect(0, capsuleEmptyRect.y + (treasureRect.height * openOffset), capsuleWidth, capsuleHeight);

        float capsuleItemDimension = Mathf.Min(capsuleWidth,capsuleHeight) * capsuleItemScale;
        float capsuleItemXOffset = (capsuleHeight - capsuleItemDimension) * 0.5f;
        float capsuleItemYOffset = (capsuleWidth - capsuleItemDimension) * 0.5f;
        capsuleItemRect = new Rect(capsuleItemXOffset, capsuleEmptyRect.y + capsuleItemYOffset, capsuleItemDimension, capsuleItemDimension);

        capsuleShineTexture = activeSkin.customStyles[20].normal.background;
        float capsuleShineWidth = capsuleEmptyRect.width * shineScale;
        float capsuleShineHeight = capsuleShineWidth * ((float)capsuleShineTexture.height / (float)capsuleShineTexture.width);
        capsuleShineRect = new Rect(capsuleShineWidth * 0.5f, capsuleItemRect.y, 0, 0);
        capsuleShineAlpha = 0;
        capsuleShineOpenRect = new Rect(0, capsuleBotOpenRect.y, capsuleShineWidth, capsuleShineHeight);

        A_treasureItemContainerRect = new Rect(spacing, 0, capsuleWidth, treasureRect.height);
        B_treasureItemContainerRect = new Rect(0.5f * (treasureRect.width - spacing) - capsuleWidth, 0, capsuleWidth, treasureRect.height);
        C_treasureItemContainerRect = new Rect(capsuleWidth + 2 * spacing, 0, capsuleWidth, treasureRect.height);
        D_treasureItemContainerRect = new Rect(0.5f * (treasureRect.width + spacing), 0, capsuleWidth, treasureRect.height);
        E_treasureItemContainerRect = new Rect(2 * capsuleWidth + 3 * spacing, 0, capsuleWidth, treasureRect.height);

        #endregion
        #region Stats

        // Initialise the stats rect
        statsRect = new Rect(contentContainerRect.width * statsXOffset, 
                             contentContainerRect.height * statsYOffset, 
                             contentContainerRect.width * (1 - statsXOffset * 2), 
                             contentContainerRect.height * (1 - statsYOffset));

        // Get the textures
        A_iconTexture = activeSkin.customStyles[3].normal.background;
        B_iconTexture = activeSkin.customStyles[4].normal.background;
        C_iconTexture = activeSkin.customStyles[5].normal.background;

        // Get the icon dimensions
        iconWidth = menuRect.width * iconScale;
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

        navButtonHeight = menuRect.height * navButtonScale;
        navButtonWidth = navButtonHeight * ((float)activeSkin.customStyles[7].normal.background.width /
                                            (float)activeSkin.customStyles[7].normal.background.height);
        float navButtonSpacing = navButtonWidth * navButtonSpacingScale;

        float navWidth = navButtonWidth * 3 + navButtonSpacing * 2;
        navContainerRect = new Rect((menuWidth - navWidth) / 2, menuHeight * navContainerYOffset, navWidth, navButtonHeight);

        retryBtnRect = new Rect(0, 0, navButtonWidth, navButtonHeight);
        homeBtnRect = new Rect(navButtonWidth + navButtonSpacing, 0, navButtonWidth, navButtonHeight);
        nextBtnRect = new Rect(2 * (navButtonWidth + navButtonSpacing), 0, navButtonWidth, navButtonHeight);

        #endregion

	}

    void Update()
    {
    }
	
	void OnGUI()
    {
        // Sets the GUI depth
        GUI.depth = 10;
        // Set the active skin
        GUI.skin = activeSkin;
        // The container
        GUI.BeginGroup(containerRect);
        // Draw the background
        GUI.DrawTexture(backgroundRect, backgroundTexture, ScaleMode.ScaleAndCrop);
        // Draw the main display
        MainDisplay();

        GUI.EndGroup();
    }

    #region GUI sections

    void MainDisplay()
    {
        GUI.BeginGroup(menuContainerRect);
        {
            GUI.DrawTexture(menuRect, menuTexture);

            Stars();

            GUI.BeginGroup(contentContainerRect);
            {
                GUI.BeginGroup(innerContentContainerRect);
                {
                    if (screen == 1)
                    {
                        Objectives();
                    }
                    else if (screen == 2)
                    {
                        Treasures();
                    }
                    else if (screen == 3)
                    {
                        Stats();
                    }
                    if (allowTransition)
                    {
                        // Next arrow
                        var original_color = GUI.color;
                        GUI.color = (new Color(0.843f, 0.29f, 0.094f));
                        GUI.DrawTexture(arrowNextRect, arrowNextTexture);
                        GUI.color = original_color;
                    }
                }
                GUI.EndGroup();
                GUI.BeginGroup(innerContentTransitionRect);
                {
                    if (transitionTo == 2)
                    {
                        Treasures();
                    }
                    else if (transitionTo == 3)
                    {
                        Stats();
                    }
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            if (screen == 3)
            {
                Navigation();
            }
        }
        GUI.EndGroup();

        if (allowTransition)
        {
            if (GUI.Button(new Rect(0, 0, Screen.width, Screen.height), "", activeSkin.customStyles[16]))
            {
                TransitionToNext();
            }
        }
    }

    void Objectives()
    {
        GUI.Label(headerRect, "Objectives", activeSkin.customStyles[2]);

        GUI.BeginGroup(objectivesRect);
        {
            // Draw the first tickbox
            GUI.DrawTexture(tickbox1Rect, tickboxTexture);
            GUI.BeginGroup(tick1ContainerRect);
            {
                GUI.DrawTexture(tickRect, tickTexture);
            }
            GUI.EndGroup();
            // The objective text
            GUI.Label(objective1Rect, objective1, objectiveLabelStyle);

            // Draw the second tickbox
            GUI.DrawTexture(tickbox2Rect, tickboxTexture);
            GUI.BeginGroup(tick2ContainerRect);
            {
                GUI.DrawTexture(tickRect, tickTexture);
            }
            GUI.EndGroup();
            // The objective text
            GUI.Label(objective2Rect, objective2, objectiveLabelStyle);

            // Draw the third tickbox
            GUI.DrawTexture(tickbox3Rect, tickboxTexture);
            GUI.BeginGroup(tick3ContainerRect);
            {
                GUI.DrawTexture(tickRect, tickTexture);
            }
            GUI.EndGroup();
            // The objective text
            GUI.Label(objective3Rect, objective3, objectiveLabelStyle);
        }
        GUI.EndGroup();
    }

    void Treasures()
    {
        GUI.BeginGroup(treasureRect);
        {
            if (capsulesMax == 1)
            {
                Capsule(C_treasureItemContainerRect, loot1);
            }
            else if (capsulesMax == 2)
            {
                Capsule(B_treasureItemContainerRect, loot1);
                Capsule(D_treasureItemContainerRect, loot2);
            }
            else if (capsulesMax == 3)
            {
                Capsule(A_treasureItemContainerRect, loot1);
                Capsule(C_treasureItemContainerRect, loot2);
                Capsule(E_treasureItemContainerRect, loot3);
            }
        }
        GUI.EndGroup();
    }

    void Capsule(Rect container, Texture item)
    {
        GUI.BeginGroup(container);
        {
            if (item != null)
            {
                GUI.DrawTexture(capsuleItemRect, item);

                // Shine
                var color = GUI.color;
                GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, capsuleShineAlpha);
                GUI.DrawTexture(capsuleShineRect, capsuleShineTexture);
                GUI.color = color;

                GUI.DrawTexture(capsuleTopRect, capsuleTopTexture);
                GUI.DrawTexture(capsuleBotRect, capsuleBotTexture);
            }
            else
            {
                GUI.DrawTexture(capsuleEmptyRect, capsuleEmptyTexture);
            }
        }
        GUI.EndGroup();
    }

    void Stats()
    {
        GUI.Label(headerRect, "Summary", activeSkin.customStyles[2]);

        GUI.BeginGroup(statsRect);
        {
            GUI.DrawTexture(A_iconRect, A_iconTexture);
            GUI.DrawTexture(B_iconRect, B_iconTexture);
            GUI.DrawTexture(C_iconRect, C_iconTexture);

            GUI.Label(A_statLabelRect, time, activeSkin.customStyles[6]);
            GUI.Label(B_statLabelRect, ntpCollected + "/" + ntpAvailable, activeSkin.customStyles[6]);
            GUI.Label(C_statLabelRect, capsulesCollected + "/" + capsulesMax, activeSkin.customStyles[6]);
        }
        GUI.EndGroup();
    }

    void Navigation()
    {

        var color = GUI.color; // save previous color
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, navAlpha);

        GUI.BeginGroup(navContainerRect);
        {
            if (GUI.Button(retryBtnRect, "", activeSkin.customStyles[7]))
            {
                NavigationManager.instance.RetryLevel();
            }
            if (GUI.Button(homeBtnRect, "", activeSkin.customStyles[8]))
            {
                NavigationManager.instance.NavToTitle();
            }
            if (GUI.Button(nextBtnRect, "", activeSkin.customStyles[9]))
            {
                NavigationManager.instance.NextStage();
            }
        }
        GUI.EndGroup();

        GUI.color = color;
    }

    void Stars()
    {
        GUI.BeginGroup(starsContainerRect);

        var color = GUI.color;

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, shine1Alpha);
        GUIUtility.RotateAroundPivot(shine1Rotation, star1Pivot);
        GUI.DrawTexture(starShine1Rect, starShineTexture);
        GUIUtility.RotateAroundPivot(-shine1Rotation, star1Pivot);
        GUI.color = color;
        GUI.DrawTexture(star1Rect, starTexture);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, shine2Alpha);
        GUIUtility.RotateAroundPivot(shine2Rotation, star2Pivot);
        GUI.DrawTexture(starShine2Rect, starShineTexture);
        GUIUtility.RotateAroundPivot(-shine2Rotation, star2Pivot);
        GUI.color = color;
        GUI.DrawTexture(star2Rect, starTexture);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, shine3Alpha);
        GUIUtility.RotateAroundPivot(shine3Rotation, star3Pivot);
        GUI.DrawTexture(starShine3Rect, starShineTexture);
        GUIUtility.RotateAroundPivot(-shine3Rotation, star3Pivot);
        GUI.color = color;
        GUI.DrawTexture(star3Rect, starTexture);

        GUI.EndGroup();
    }

    #endregion

    void AnimateGameCompletedMenu(Rect newCoordinates)
    {
        containerRect = newCoordinates;
    }
    void AnimateStar1(Rect size)
    {
        star1Rect = size;
    }
    void AnimateStar2(Rect size)
    {
        star2Rect = size;
    }
    void AnimateStar3(Rect size)
    {
        star3Rect = size;
    }
    void AnimateShine1Alpha(float alpha)
    {
        shine1Alpha = alpha;
    }
    void AnimateShine2Alpha(float alpha)
    {
        shine2Alpha = alpha;
    }
    void AnimateShine3Alpha(float alpha)
    {
        shine3Alpha = alpha;
    }
    void AnimateShine1Rotation(int rotation)
    {
        shine1Rotation = rotation * 2;
    }
    void AnimateShine2Rotation(int rotation)
    {
        shine2Rotation = rotation * 2;
    }
    void AnimateShine3Rotation(int rotation)
    {
        shine3Rotation = rotation * 2;
    }
    void AnimateTick1(Rect size)
    {
        tick1ContainerRect = size;
    }
    void AnimateTick2(Rect size)
    {
        tick2ContainerRect = size;
    }
    void AnimateTick3(Rect size)
    {
        tick3ContainerRect = size;
    }
    void AnimateCapsuleTop(Rect size)
    {
        capsuleTopRect = size;
    }
    void AnimateCapsuleBot(Rect size)
    {
        capsuleBotRect = size;
    }
    void AnimateCapsuleShine(Rect size)
    {
        capsuleShineRect = size;
    }
    void AnimateCapsuleShineAlpha(float alpha)
    {
        capsuleShineAlpha = alpha;
    }
    void AnimateInnerRect(Rect size)
    {
        innerContentContainerRect = size;
    }
    void AnimateTransitionRect(Rect size)
    {
        innerContentTransitionRect = size;
    }
    void OnTransitionComplete()
    {
        innerContentContainerRect = innerContainerPosMid;
        innerContentTransitionRect = innerContainerPosRight;

        screen = transitionTo;
        if (screen == 2)
        {
            OpenCapsules();
        }
    }
    void AnimateNavAlpha(float alpha)
    {
        navAlpha = alpha;
    }

    public void StageComplete(string _time, int _ntpCollected, int _ntpAvailable,
                              int _capAvailable, List<Capsule> capsules, Objective[] objs)
    {
        // Objectives
        objectives = objs;
        objective1 = objectives[0].ToString();
        objective2 = objectives[1].ToString();
        objective3 = objectives[2].ToString();
        // Count number of completed objectives
        objCompleteCount = 0;
        if (objectives[0].completed) objCompleteCount += 1;
        if (objectives[1].completed) objCompleteCount += 1;
        if (objectives[2].completed) objCompleteCount += 1;

        // Stage stats
        time = _time;
        ntpCollected = _ntpCollected;
        ntpAvailable = _ntpAvailable;
        capsulesCollected = capsules.Count;
        capsulesMax = _capAvailable;

        if (capsulesCollected >= 1)
        {
            // Update Inventory
            loot1 = InventoryManager.instance.lootItem(capsules[0].obtainLoot());
        }
        if (capsulesCollected >= 2)
        {
            // Update Inventory
            loot2 = InventoryManager.instance.lootItem(capsules[1].obtainLoot());
        }
        if (capsulesCollected >= 3)
        {
            // Update Inventory
            loot3 = InventoryManager.instance.lootItem(capsules[2].obtainLoot());
        }

        allowTransition = false;
        screen = 1;

        // Animate
        iTween.ValueTo(gameObject,
                        iTween.Hash("from", containerRect,
                                    "to", openPosition,
                                    "onupdate", "AnimateGameCompletedMenu",
                                    "oncomplete", "ShowStar1",
                                    "easetype", iTween.EaseType.easeOutQuart));
    }
    void ShowStar1()
    {
        if (objCompleteCount > 0)
        {
            // Animate Star and shine
            iTween.ValueTo(gameObject,
                                iTween.Hash("from", star1Rect,
                                            "to", star1ShowRect,
                                            "onupdate", "AnimateStar1",
                                            "oncomplete", "ShowStar2",
                                            "easetype", iTween.EaseType.easeOutQuart,
                                            "time", 0.5));
            iTween.ValueTo(gameObject,
                                iTween.Hash("from", 1,
                                            "to", 0,
                                            "onupdate", "AnimateShine1Alpha",
                                            "easetype", iTween.EaseType.easeOutQuart,
                                            "time", 8));
            iTween.ValueTo(gameObject,
                                iTween.Hash("from", 0,
                                            "to", 360,
                                            "onupdate", "AnimateShine1Rotation",
                                            "easetype", iTween.EaseType.easeOutQuart,
                                            "time", 8));

            if (objectives[0].completed)
            {
                // Animate the tick
                iTween.ValueTo(gameObject,
                                    iTween.Hash("from", tick1ContainerRect,
                                                "to", tickbox1Rect,
                                                "onupdate", "AnimateTick1",
                                                "easetype", iTween.EaseType.easeOutQuart,
                                                "time", 0.5));
            }
            else if (objectives[1].completed)
            {
                // Animate the tick
                iTween.ValueTo(gameObject,
                                iTween.Hash("from", tick2ContainerRect,
                                            "to", tickbox2Rect,
                                            "onupdate", "AnimateTick2",
                                            "easetype", iTween.EaseType.easeOutQuart,
                                            "time", 0.5));
            }
            else if (objectives[2].completed)
            {
                // Animate the tick
                iTween.ValueTo(gameObject,
                                iTween.Hash("from", tick3ContainerRect,
                                            "to", tickbox3Rect,
                                            "onupdate", "AnimateTick3",
                                            "easetype", iTween.EaseType.easeOutQuart,
                                            "time", 0.5));
            }
        }
        else
        {
            allowTransition = true;
        }
    }
    void ShowStar2()
    {
        if (objCompleteCount > 1)
        {
            // Animate Shine
            iTween.ValueTo(gameObject,
                                iTween.Hash("from", star2Rect,
                                            "to", star2ShowRect,
                                            "onupdate", "AnimateStar2",
                                            "oncomplete", "ShowStar3",
                                            "easetype", iTween.EaseType.easeOutQuart,
                                            "time", 0.5));
            iTween.ValueTo(gameObject,
                                iTween.Hash("from", 1,
                                            "to", 0,
                                            "onupdate", "AnimateShine2Alpha",
                                            "easetype", iTween.EaseType.easeOutQuart,
                                            "time", 8));
            iTween.ValueTo(gameObject,
                                iTween.Hash("from", 0,
                                            "to", 360,
                                            "onupdate", "AnimateShine2Rotation",
                                            "easetype", iTween.EaseType.easeOutQuart,
                                            "time", 8));

            if (objectives[0].completed)
            {
                if (objectives[1].completed)
                {
                    iTween.ValueTo(gameObject,
                                iTween.Hash("from", tick2ContainerRect,
                                            "to", tickbox2Rect,
                                            "onupdate", "AnimateTick2",
                                            "easetype", iTween.EaseType.easeOutQuart,
                                            "time", 0.5));
                }
                else if (objectives[2].completed)
                {
                    iTween.ValueTo(gameObject,
                                iTween.Hash("from", tick3ContainerRect,
                                            "to", tickbox3Rect,
                                            "onupdate", "AnimateTick3",
                                            "easetype", iTween.EaseType.easeOutQuart,
                                            "time", 0.5));
                }
            }
            else if (objectives[1].completed)
            {
                iTween.ValueTo(gameObject,
                                iTween.Hash("from", tick3ContainerRect,
                                            "to", tickbox3Rect,
                                            "onupdate", "AnimateTick3",
                                            "easetype", iTween.EaseType.easeOutQuart,
                                            "time", 0.5));
            }
        }
        else
        {
            allowTransition = true;
        }
    }
    void ShowStar3()
    {
        if (objCompleteCount > 2)
        {
            // Animate Shine
            iTween.ValueTo(gameObject,
                                iTween.Hash("from", star3Rect,
                                            "to", star3ShowRect,
                                            "onupdate", "AnimateStar3",
                                            "oncomplete", "AllowTransition",
                                            "easetype", iTween.EaseType.easeOutQuart,
                                            "time", 0.5));
            iTween.ValueTo(gameObject,
                                iTween.Hash("from", 1,
                                            "to", 0,
                                            "onupdate", "AnimateShine3Alpha",
                                            "easetype", iTween.EaseType.easeOutQuart,
                                            "time", 8));
            iTween.ValueTo(gameObject,
                                iTween.Hash("from", 0,
                                            "to", 360,
                                            "onupdate", "AnimateShine3Rotation",
                                            "easetype", iTween.EaseType.easeOutQuart,
                                            "time", 8));
            // Animate the tick
            iTween.ValueTo(gameObject,
                                iTween.Hash("from", tick3ContainerRect,
                                            "to", tickbox3Rect,
                                            "onupdate", "AnimateTick3",
                                            "easetype", iTween.EaseType.easeOutQuart,
                                            "time", 0.5));
        }
        else
        {
            allowTransition = true;
        }
    }
    void OpenCapsules()
    {
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", capsuleTopRect,
                                   "to", capsuleTopOpenRect,
                                   "onupdate", "AnimateCapsuleTop",
                                   "easetype", iTween.EaseType.easeOutQuart,
                                   "time", 0.5));
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", capsuleBotRect,
                                   "to", capsuleBotOpenRect,
                                   "onupdate", "AnimateCapsuleBot",
                                   "easetype", iTween.EaseType.easeOutQuart,
                                   "time", 0.5));
        
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", capsuleShineRect,
                                   "to", capsuleShineOpenRect,
                                   "onupdate", "AnimateCapsuleShine",
                                   "easetype", iTween.EaseType.easeOutQuart,
                                   "time", 0.5f));
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", capsuleShineAlpha,
                                   "to", 1,
                                   "onupdate", "AnimateCapsuleShineAlpha",
                                   "easetype", iTween.EaseType.easeOutQuart,
                                   "time", 0.5f));
         
    }

    void AllowTransition()
    {
        allowTransition = true;
    }

    void TransitionToNext()
    {
        if (screen == 1 && capsulesMax == 0)
        {
            transitionTo = 3;
        }
        else
        {
            transitionTo = screen + 1;
        }

        if (transitionTo == 3)
        {
            allowTransition = false;

            iTween.ValueTo(gameObject,
                                iTween.Hash("from", 0,
                                            "to", 1,
                                            "onupdate", "AnimateNavAlpha",
                                            "easetype", iTween.EaseType.easeOutQuart,
                                            "time", 0.5));
        }

        // Animate the transition
        iTween.ValueTo(gameObject,
                            iTween.Hash("from", innerContentContainerRect,
                                        "to", innerContainerPosLeft,
                                        "onupdate", "AnimateInnerRect",
                                        "easetype", iTween.EaseType.easeOutQuart,
                                        "time", 0.5));
        iTween.ValueTo(gameObject,
                            iTween.Hash("from", innerContentTransitionRect,
                                        "to", innerContainerPosMid,
                                        "onupdate", "AnimateTransitionRect",
                                        "oncomplete", "OnTransitionComplete",
                                        "easetype", iTween.EaseType.easeOutQuart,
                                        "time", 0.5));
    }
}
