using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievementGUI : MonoBehaviour
{
    /* GUI Skin
     * Custom Styles [0] = Back Button
     * Custom Styles [1] = Background
     * Custom Styles [2] = Frame
     * Custom Styles [3] = Title Text
     * Custom Styles [4] = Sub-title text
     * Custom Styles [5] = Arrow Next
     * Custom Styles [6] = Arrow Prev
     * Custom Styles [7] = Blue Icon
     * Custom Styles [8] = Green Icon
     * Custom Styles [9] = Red Icon
     * Custom Styles [10] = Achievement Title Text
     * Custom Styles [11] = Achievement Description Text
     */

    #region Global Variables
    // GUI Skin
    public GUISkin activeSkin;
    private AchievementController achievementController;
    public Texture iconText;

    // Triggers
    private bool sound = false;

    // Page variables
    public Rect bgRect;
    public Texture bgTexture;
    private float screenPaddingPercentage = 0.01f; // percentage of screen size;

    // Achievement Banner
    private Rect bannerRect;
    private Texture bannerTexture;

    // Frame
    private Rect frameRect;
    private Texture frameTexture;

    // Achievement Paper
    private Rect paperRect;
    private Texture paperTexture;

    // Buttons
    private Rect btnBackRect;
    private Rect btnPreviousRect;
    private Rect btnNextRect;

    private ButtonHandler btnBackScale;
    private ButtonHandler btnPreviousScale;
    private ButtonHandler btnNextScale;

    private float btnTopPaddingScaling = 0.39f;
    private float btnBottomPaddingScaling = 0f;

    // Achievement Text
    private Rect achievementRect1;
    private Rect achievementRect2;
    private Rect achievementRect3;
    private Rect achievementRect4;
    private GUIStyle achievementStyle;
    //private float achievementFontScale = 0.04f;
    private string achievement1;
    private string achievement2;
    private string achievement3;
    private string achievement4;

    // Tick boxes
    private Rect tickBoxRect1;
    private Rect tickBoxRect2;
    private Rect tickBoxRect3;
    private Rect tickBoxRect4;
    private Texture tickBoxTexture;

    // Ticks
    private Rect tickRect1;
    private Rect tickRect2;
    private Rect tickRect3;
    private Rect tickRect4;
    private Texture tickTexture;

    // Title Text
    private Rect titleRect;
    private GUIStyle titleStyle;
    private float titleFontScale = 0.12f;
    private string title = "Achievements";

    // Sub-Title Text
    private Rect subtitleRect;
    private GUIStyle subtitleStyle;
    private float subtitleFontScale = 0.04f;
    private string subtitle = "Are you game enough?";

    // Achievements
    private Rect[] achievementTitleRect;
    private Rect[] achievementDescRect;
    private Rect[] iconsRect;
    private GUIStyle achievementTitleStyle;
    private GUIStyle achievementDescStyle;
    private Texture[] iconsTexture;
    private float achievementFontScale = 0.032f;
    private float achievementDFontScale = 0.025f;
    private string[] achievementTitleStr;
    private string[] achievementDescStr;
    private Rect[] capsulesRect;
    private Texture[] capsulesTexture;

    // Decoration
    private Rect rightDecorRect;
    private Rect leftDecorRect;
    private Texture decorTexture;

    // Thumbs
    private Rect thumbsRect;
    private Texture thumbsTexture;

    // Rewards
    private Rect rewardRect;
    private Rect rewardNumberRect;
    private Texture rewardTexture;
    private string rewardNumber;
    private float rewardFontScale = 0.07f;
    private GUIStyle rewardStyle;

    private int page;
    private int maxPage;

    #endregion

    // Use this for initialization
	void Start ()
    {
        #region GUI

        bgRect = new Rect(0, 0, Screen.width, Screen.height);
        bgTexture = activeSkin.customStyles[1].normal.background;
        achievementController = gameObject.GetComponent<AchievementController>();
        page = 0;

        // Calculate screen padding
        float screenPadding = Screen.height * screenPaddingPercentage;

        // Frame
        frameTexture = activeSkin.customStyles[2].normal.background;
        float frameHeight = Screen.height - screenPadding * 2;
        float frameWidth = frameHeight * ((float)frameTexture.width / (float)frameTexture.height);
        float frameXOffset = (Screen.width - frameWidth) * 0.5f;
        float frameYOffset = screenPadding;
        frameRect = new Rect(frameXOffset, frameYOffset, frameWidth, frameHeight);

        // Title
        titleStyle = activeSkin.customStyles[3];
        titleStyle.fontSize = (int)(Screen.height * titleFontScale);
        Vector2 size = activeSkin.customStyles[3].CalcSize(new GUIContent(title));
        float titleXOffset = frameXOffset + frameWidth * 0.51f - size.x * 0.5f;
        float titleYOffset = frameYOffset + frameHeight * 0.13f;
        titleRect = new Rect(titleXOffset, titleYOffset, size.x, size.y);

        // Sub-Title
        subtitleStyle = activeSkin.customStyles[4];
        subtitleStyle.fontSize = (int)(Screen.height * subtitleFontScale);
        Vector2 subsize = activeSkin.customStyles[4].CalcSize(new GUIContent(subtitle));
        float subtitleXOffset = frameXOffset + frameWidth * 0.51f - subsize.x * 0.5f;
        float subtitleYOffset = titleYOffset + size.y;
        subtitleRect = new Rect(subtitleXOffset, subtitleYOffset, subsize.x, subsize.y);

        // Achievements
        achievementTitleStyle = activeSkin.customStyles[7];
        achievementDescStyle = activeSkin.customStyles[8];
        achievementTitleStyle.fontSize = (int)(Screen.height * achievementFontScale);
        achievementDescStyle.fontSize = (int)(Screen.height * achievementDFontScale);
        int numOfAchievements = Game.instance.achievements.Length;
        achievementTitleRect = new Rect[numOfAchievements];
        achievementDescRect = new Rect[numOfAchievements];
        achievementTitleStr = new string[numOfAchievements];
        achievementDescStr = new string[numOfAchievements];
        iconsTexture = new Texture[numOfAchievements];
        iconsRect = new Rect[numOfAchievements];
        capsulesRect = new Rect[numOfAchievements];
        capsulesTexture = new Texture[numOfAchievements];
        float achievementTitleHeight = activeSkin.customStyles[7].CalcSize(new GUIContent(Game.instance.achievements[0].title)).y;
        float iconHeight =  achievementTitleHeight * 2;
        float iconWidth = iconHeight * ((float)iconText.width / (float)iconText.height);
        float iconXOffset1 = frameXOffset + iconWidth * 2;
        float iconXOffset2 = Screen.width * 0.5f + screenPadding;
        float iconYOffset;
        float achievementTitleXOffset1 = iconXOffset1 + iconWidth + screenPadding;
        float achievementTitleXOffset2 = iconXOffset2 + iconWidth + screenPadding;
        float achievementTitleYOffset;
        float achievementDescYOffset;
        float capsuleHeight = iconHeight;
        float capsuleWidth = capsuleHeight * ((float)activeSkin.customStyles[9].normal.background.width / (float)activeSkin.customStyles[9].normal.background.height);
        float capsuleXOffset = 0f;
        float capsuleYOffset = 0f;
        int j = 0;
        for (int i = 0; i < numOfAchievements; i++)
        {
            if (!Game.instance.achievements[i].hidden)
            {
                // choose which icon to output
                iconsTexture[j] = Game.instance.achievements[i].icon;
                Vector2 achievementTitleSize = activeSkin.customStyles[7].CalcSize(new GUIContent(Game.instance.achievements[i].title));
                Vector2 achievementDescSize = activeSkin.customStyles[7].CalcSize(new GUIContent(Game.instance.achievements[i].ToString()));
                achievementTitleStr[j] = Game.instance.achievements[i].title;
                achievementDescStr[j] = Game.instance.achievements[i].ToString();
                int test = i % 10;
                if ((j % 10) == 0)
                {
                    iconYOffset = subtitleYOffset + subsize.y + screenPadding * 2;
                    iconsRect[j] = new Rect(iconXOffset1, iconYOffset, iconWidth, iconHeight);
                    achievementTitleYOffset = iconYOffset;
                    achievementDescYOffset = achievementTitleYOffset + achievementTitleHeight;
                    achievementTitleRect[j] = new Rect(achievementTitleXOffset1, achievementTitleYOffset, achievementTitleSize.x, achievementTitleSize.y);
                    achievementDescRect[j] = new Rect(achievementTitleXOffset1, achievementDescYOffset, achievementDescSize.x, achievementDescSize.y);
                    capsuleXOffset = achievementTitleXOffset1 + achievementTitleSize.x;
                    capsuleYOffset = iconYOffset - achievementTitleSize.y;
                }
                else if ((j % 10) == 1)
                {
                    iconYOffset = subtitleYOffset + subsize.y + screenPadding * 2;
                    iconsRect[j] = new Rect(iconXOffset2, iconYOffset, iconWidth, iconHeight);
                    achievementTitleYOffset = iconYOffset;
                    achievementDescYOffset = achievementTitleYOffset + achievementTitleHeight;
                    achievementTitleRect[j] = new Rect(achievementTitleXOffset2, achievementTitleYOffset, achievementTitleSize.x, achievementTitleSize.y);
                    achievementDescRect[j] = new Rect(achievementTitleXOffset2, achievementDescYOffset, achievementDescSize.x, achievementDescSize.y);
                    capsuleXOffset = achievementTitleXOffset2 + achievementTitleSize.x;
                    capsuleYOffset = iconYOffset - achievementTitleSize.y;                
                }
                else if ((j % 10) == 2)
                {
                    iconYOffset = subtitleYOffset + subsize.y + screenPadding * 2 + iconHeight + screenPadding * 2;
                    iconsRect[j] = new Rect(iconXOffset1, iconYOffset, iconWidth, iconHeight);
                    achievementTitleYOffset = iconYOffset;
                    achievementDescYOffset = achievementTitleYOffset + achievementTitleHeight;
                    achievementTitleRect[j] = new Rect(achievementTitleXOffset1, achievementTitleYOffset, achievementTitleSize.x, achievementTitleSize.y);
                    achievementDescRect[j] = new Rect(achievementTitleXOffset1, achievementDescYOffset, achievementDescSize.x, achievementDescSize.y);
                    capsuleXOffset = achievementTitleXOffset1 + achievementTitleSize.x;
                    capsuleYOffset = iconYOffset - achievementTitleSize.y;
                }
                else if ((j % 10) == 3)
                {
                    iconYOffset = subtitleYOffset + subsize.y + screenPadding * 2 + iconHeight + screenPadding * 2;
                    iconsRect[j] = new Rect(iconXOffset2, iconYOffset, iconWidth, iconHeight);
                    achievementTitleYOffset = iconYOffset;
                    achievementDescYOffset = achievementTitleYOffset + achievementTitleHeight;
                    achievementTitleRect[j] = new Rect(achievementTitleXOffset2, achievementTitleYOffset, achievementTitleSize.x, achievementTitleSize.y);
                    achievementDescRect[j] = new Rect(achievementTitleXOffset2, achievementDescYOffset, achievementDescSize.x, achievementDescSize.y);
                    capsuleXOffset = achievementTitleXOffset2 + achievementTitleSize.x;
                    capsuleYOffset = iconYOffset - achievementTitleSize.y;
                }
                else if ((j % 10) == 4)
                {
                    iconYOffset = subtitleYOffset + subsize.y + screenPadding * 2 + iconHeight * 2 + screenPadding * 4;
                    iconsRect[j] = new Rect(iconXOffset1, iconYOffset, iconWidth, iconHeight);
                    achievementTitleYOffset = iconYOffset;
                    achievementDescYOffset = achievementTitleYOffset + achievementTitleHeight;
                    achievementTitleRect[j] = new Rect(achievementTitleXOffset1, achievementTitleYOffset, achievementTitleSize.x, achievementTitleSize.y);
                    achievementDescRect[j] = new Rect(achievementTitleXOffset1, achievementDescYOffset, achievementDescSize.x, achievementDescSize.y);
                    capsuleXOffset = achievementTitleXOffset1 + achievementTitleSize.x;
                    capsuleYOffset = iconYOffset - achievementTitleSize.y;
                }
                else if ((j % 10) == 5)
                {
                    iconYOffset = subtitleYOffset + subsize.y + screenPadding * 2 + iconHeight * 2 + screenPadding * 4;
                    iconsRect[j] = new Rect(iconXOffset2, iconYOffset, iconWidth, iconHeight);
                    achievementTitleYOffset = iconYOffset;
                    achievementDescYOffset = achievementTitleYOffset + achievementTitleHeight;
                    achievementTitleRect[j] = new Rect(achievementTitleXOffset2, achievementTitleYOffset, achievementTitleSize.x, achievementTitleSize.y);
                    achievementDescRect[j] = new Rect(achievementTitleXOffset2, achievementDescYOffset, achievementDescSize.x, achievementDescSize.y);
                    capsuleXOffset = achievementTitleXOffset2 + achievementTitleSize.x;
                    capsuleYOffset = iconYOffset - achievementTitleSize.y;
                }
                else if ((j % 10) == 6)
                {
                    iconYOffset = subtitleYOffset + subsize.y + screenPadding * 2 + iconHeight * 3 + screenPadding * 6;
                    iconsRect[j] = new Rect(iconXOffset1, iconYOffset, iconWidth, iconHeight);
                    achievementTitleYOffset = iconYOffset;
                    achievementDescYOffset = achievementTitleYOffset + achievementTitleHeight;
                    achievementTitleRect[j] = new Rect(achievementTitleXOffset1, achievementTitleYOffset, achievementTitleSize.x, achievementTitleSize.y);
                    achievementDescRect[j] = new Rect(achievementTitleXOffset1, achievementDescYOffset, achievementDescSize.x, achievementDescSize.y);
                    capsuleXOffset = achievementTitleXOffset1 + achievementTitleSize.x;
                    capsuleYOffset = iconYOffset - achievementTitleSize.y;
                }
                else if ((j % 10) == 7)
                {
                    iconYOffset = subtitleYOffset + subsize.y + screenPadding * 2 + iconHeight * 3 + screenPadding * 6;
                    iconsRect[j] = new Rect(iconXOffset2, iconYOffset, iconWidth, iconHeight);
                    achievementTitleYOffset = iconYOffset;
                    achievementDescYOffset = achievementTitleYOffset + achievementTitleHeight;
                    achievementTitleRect[j] = new Rect(achievementTitleXOffset2, achievementTitleYOffset, achievementTitleSize.x, achievementTitleSize.y);
                    achievementDescRect[j] = new Rect(achievementTitleXOffset2, achievementDescYOffset, achievementDescSize.x, achievementDescSize.y);
                    capsuleXOffset = achievementTitleXOffset2 + achievementTitleSize.x;
                    capsuleYOffset = iconYOffset - achievementTitleSize.y;
                }
                else if ((j % 10) == 8)
                {
                    iconYOffset = subtitleYOffset + subsize.y + screenPadding * 2 + iconHeight * 4 + screenPadding * 8;
                    iconsRect[j] = new Rect(iconXOffset1, iconYOffset, iconWidth, iconHeight);
                    achievementTitleYOffset = iconYOffset;
                    achievementDescYOffset = achievementTitleYOffset + achievementTitleHeight;
                    achievementTitleRect[j] = new Rect(achievementTitleXOffset1, achievementTitleYOffset, achievementTitleSize.x, achievementTitleSize.y);
                    achievementDescRect[j] = new Rect(achievementTitleXOffset1, achievementDescYOffset, achievementDescSize.x, achievementDescSize.y);
                    capsuleXOffset = achievementTitleXOffset1 + achievementTitleSize.x;
                    capsuleYOffset = iconYOffset - achievementTitleSize.y;
                }
                else if ((j % 10) == 9)
                {
                    iconYOffset = subtitleYOffset + subsize.y + screenPadding * 2 + iconHeight * 4 + screenPadding * 8;
                    iconsRect[j] = new Rect(iconXOffset2, iconYOffset, iconWidth, iconHeight);
                    achievementTitleYOffset = iconYOffset;
                    achievementDescYOffset = achievementTitleYOffset + achievementTitleHeight;
                    achievementTitleRect[j] = new Rect(achievementTitleXOffset2, achievementTitleYOffset, achievementTitleSize.x, achievementTitleSize.y);
                    achievementDescRect[j] = new Rect(achievementTitleXOffset2, achievementDescYOffset, achievementDescSize.x, achievementDescSize.y);
                    capsuleXOffset = achievementTitleXOffset2 + achievementTitleSize.x;
                    capsuleYOffset = iconYOffset - achievementTitleSize.y;
                }

                if (Game.instance.achievements[i].capsuleColor == Rarity.common)
                {
                    capsulesTexture[j] = activeSkin.customStyles[9].normal.background;
                    capsulesRect[j] = new Rect(capsuleXOffset, capsuleYOffset, capsuleWidth, capsuleHeight);
                }
                else if (Game.instance.achievements[i].capsuleColor == Rarity.rare)
                {
                    capsulesTexture[j] = activeSkin.customStyles[10].normal.background;
                    capsulesRect[j] = new Rect(capsuleXOffset, capsuleYOffset, capsuleWidth, capsuleHeight);
                }
                else if (Game.instance.achievements[i].capsuleColor == Rarity.challenge)
                {
                    capsulesTexture[j] = activeSkin.customStyles[11].normal.background;
                    capsulesRect[j] = new Rect(capsuleXOffset, capsuleYOffset, capsuleWidth, capsuleHeight);
                }
                else
                {
                    capsulesTexture[j] = null;
                }

                j++;
            }
            maxPage = j / 10;
        }

        // Buttons
        float btnBackHeight = Screen.height * 0.1f;
        float btnBackWidth = btnBackHeight * ((float)activeSkin.customStyles[0].normal.background.width /
                                          (float)activeSkin.customStyles[0].normal.background.height);
        btnBackRect = new Rect(screenPadding, screenPadding, btnBackWidth, btnBackHeight);

        float btnControlHeight = Screen.height * 0.2f;
        float btnControlWidth = btnControlHeight * ((float)activeSkin.customStyles[5].normal.background.width /
                                                    (float)activeSkin.customStyles[5].normal.background.height);
        btnPreviousRect = new Rect(screenPadding, (Screen.height - btnControlHeight) * 0.5f, btnControlWidth, btnControlHeight);
        btnNextRect = new Rect(Screen.width - screenPadding - btnControlWidth, (Screen.height - btnControlHeight) * 0.5f, btnControlWidth, btnControlHeight);

        // Initialise button scalers
        btnBackScale = new ButtonHandler(btnBackRect, gameObject, 0.9f, "Back_ScaleButton");

        #endregion

    }
	
	// Update is called once per frame
    void OnGUI()
    {
        Debug.Log("SHIT");
        // Sets the GUI depth
        GUI.depth = 10;
        GUI.skin = activeSkin;
        AchievementScreen();
	}

    private void AchievementScreen()
    {
        // Background
        GUI.DrawTexture(bgRect, bgTexture, ScaleMode.ScaleAndCrop);

        // Frame
        GUI.DrawTexture(frameRect, frameTexture);

        // Title
        GUI.Label(titleRect, title, titleStyle);

        // Sub-Title
        GUI.Label(subtitleRect, subtitle, subtitleStyle);

        // Achievements

        for (int i = 0; i < 10; i++)
        {
            if ((i + page * 10) < iconsRect.Length)
            {
                GUI.DrawTexture(iconsRect[i + page * 10], iconText);
                GUI.Label(achievementTitleRect[i + page * 10], achievementTitleStr[i + page * 10], achievementTitleStyle);
                GUI.Label(achievementDescRect[i + page * 10], achievementDescStr[i + page * 10], achievementDescStyle);
                if (capsulesTexture[i] != null)
                {
                    GUI.DrawTexture(capsulesRect[i + page * 10], capsulesTexture[i]);
                }
            }
        }

        // back button
        if (GUI.Button(btnBackRect, "", activeSkin.customStyles[0]))
        {
            NavigationManager.instance.NavToChapterSelect();
        }

        if (page > 0)
        {
            // Page Control Buttons
            if (GUI.Button(btnPreviousRect, "", activeSkin.customStyles[6]))
            {
                page -= 1;
            }
        }
        if (page < maxPage)
        {
            if (GUI.Button(btnNextRect, "", activeSkin.customStyles[5]))
            {
                page += 1;
            }
        }

        btnBackScale.OnMouseOver(btnBackRect);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            NavigationManager.instance.NavToChapterSelect();
        }

        /*

        // Achievement Paper
        GUI.DrawTexture(paperRect, paperTexture);

        // Title
        GUI.Label(titleRect, title, titleStyle);

        // Title Decoration
        GUI.DrawTexture(leftDecorRect, decorTexture);
        GUI.DrawTexture(rightDecorRect, decorTexture);

        // Rewards
        GUI.DrawTexture(rewardRect, rewardTexture);
        GUI.Label(rewardNumberRect, rewardNumber, rewardStyle);

        // Achievement Text
        GUI.Label(achievementRect1, achievement1, achievementStyle);
        GUI.Label(achievementRect2, achievement2, achievementStyle);
        GUI.Label(achievementRect3, achievement3, achievementStyle);
        GUI.Label(achievementRect4, achievement4, achievementStyle);

        // Banner
        GUI.DrawTexture(bannerRect, bannerTexture);

        // Tick Boxes
        GUI.DrawTexture(tickBoxRect1, tickBoxTexture);
        GUI.DrawTexture(tickBoxRect2, tickBoxTexture);
        GUI.DrawTexture(tickBoxRect3, tickBoxTexture);
        GUI.DrawTexture(tickBoxRect4, tickBoxTexture);

        // Ticks
        GUI.DrawTexture(tickRect1, tickTexture);
        GUI.DrawTexture(tickRect2, tickTexture);
        GUI.DrawTexture(tickRect3, tickTexture);
        GUI.DrawTexture(tickRect4, tickTexture);

        // Thumbs
        GUI.DrawTexture(thumbsRect, thumbsTexture);

        
         */
    }

    void Back_ScaleButton(Rect size)
    {
        btnBackRect = size;
    }
}
