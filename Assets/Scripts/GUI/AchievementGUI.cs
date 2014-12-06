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
    ///*
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
     //*/

    /*
    private Rect achievementTitleRect;
    private Rect achievementTitleRect1;
    private Rect achievementTitleRect2;
    private Rect achievementTitleRect3;
    private Rect achievementTitleRect4;
    private Rect achievementDescRect;
    private Rect achievementDescRect1;
    private Rect achievementDescRect2;
    private Rect achievementDescRect3;
    private Rect achievementDescRect4;
    private Rect iconsRect;
    private Rect iconsRect1;
    private Rect iconsRect2;
    private Rect iconsRect3;
    private Rect iconsRect4;
    private GUIStyle achievementTitleStyle;
    private GUIStyle achievementDescStyle;
    private float achievementFontScale = 0.032f;
    private string achievementTitleStr = "The Brown Knight";
    private string achievementDesctStr = "Complete 140 different stages";
    */

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
        /*

        achievementTitleStyle = activeSkin.customStyles[7];
        achievementDescStyle = activeSkin.customStyles[8];
        achievementTitleStyle.fontSize = (int)(Screen.height * achievementFontScale);
        achievementDescStyle.fontSize = (int)(Screen.height * achievementFontScale);
        Vector2 achTSize = activeSkin.customStyles[7].CalcSize(new GUIContent(achievementTitleStr));
        Vector2 achDSize = activeSkin.customStyles[8].CalcSize(new GUIContent(achievementDesctStr));
        float iconHeight = achTSize.y + achDSize.y;
        float iconWidth = iconHeight * ((float)iconText.width / (float)iconText.height);
        float iconXOffset = frameXOffset + iconWidth * 2;
        float iconXOffset1 = Screen.width * 0.5f + screenPadding;
        float iconYOffset = subtitleYOffset + subsize.y + screenPadding * 2;
        float iconYOffset1 = subtitleYOffset + subsize.y + screenPadding * 2 + iconHeight + screenPadding * 2;
        float iconYOffset2 = subtitleYOffset + subsize.y + screenPadding * 2 + iconHeight * 2 + screenPadding * 4;
        float iconYOffset3 = subtitleYOffset + subsize.y + screenPadding * 2 + iconHeight * 3 + screenPadding * 6;
        float iconYOffset4 = subtitleYOffset + subsize.y + screenPadding * 2 + iconHeight * 4 + screenPadding * 8;
        iconsRect = new Rect(iconXOffset, iconYOffset, iconWidth, iconHeight);
        iconsRect1 = new Rect(iconXOffset, iconYOffset1, iconWidth, iconHeight);
        iconsRect2 = new Rect(iconXOffset, iconYOffset2, iconWidth, iconHeight);
        iconsRect3 = new Rect(iconXOffset, iconYOffset3, iconWidth, iconHeight);
        iconsRect4 = new Rect(iconXOffset1, iconYOffset, iconWidth, iconHeight);
        float achTXOffset = iconXOffset + iconWidth + screenPadding;
        float achTXOffset1 = iconXOffset1 + iconWidth + screenPadding;
        float achTYOffset = iconYOffset;
        float achDYOffset = achTYOffset + achTSize.y;
        float achDYOffset1 = iconYOffset1 + achTSize.y;
        float achDYOffset2 = iconYOffset2 + achTSize.y;
        float achDYOffset3 = iconYOffset3 + achTSize.y;
        float achDYOffset4 = iconYOffset4 + achTSize.y;
        achievementTitleRect = new Rect(achTXOffset, iconYOffset, achTSize.x, achTSize.y);
        achievementTitleRect1 = new Rect(achTXOffset, iconYOffset1, achTSize.x, achTSize.y);
        achievementTitleRect2 = new Rect(achTXOffset, iconYOffset2, achTSize.x, achTSize.y);
        achievementTitleRect3 = new Rect(achTXOffset, iconYOffset3, achTSize.x, achTSize.y);
        achievementTitleRect4 = new Rect(achTXOffset1, iconYOffset, achTSize.x, achTSize.y);
        achievementDescRect = new Rect(achTXOffset, achDYOffset, achDSize.x, achDSize.y);
        achievementDescRect1 = new Rect(achTXOffset, achDYOffset1, achDSize.x, achDSize.y);
        achievementDescRect2 = new Rect(achTXOffset, achDYOffset2, achDSize.x, achDSize.y);
        achievementDescRect3 = new Rect(achTXOffset, achDYOffset3, achDSize.x, achDSize.y);
        achievementDescRect4 = new Rect(achTXOffset1, achDYOffset, achDSize.x, achDSize.y);

        */

        ///*
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
        float achievementTitleHeight = activeSkin.customStyles[7].CalcSize(new GUIContent(Game.instance.achievements[0].title)).y;
        float iconHeight =  achievementTitleHeight * 2;
        //float iconWidth = iconHeight * ((float)iconsTexture[0].width / (float)iconsTexture[0].height);
        float iconWidth = iconHeight * ((float)iconText.width / (float)iconText.height);
        float iconXOffset1 = frameXOffset + iconWidth * 2;
        float iconXOffset2 = Screen.width * 0.5f + screenPadding;
        float iconYOffset;
        float achievementTitleXOffset1 = iconXOffset1 + iconWidth + screenPadding;
        float achievementTitleXOffset2 = iconXOffset2 + iconWidth + screenPadding;
        float achievementTitleYOffset;
        float achievementDescYOffset;
        Debug.Log(numOfAchievements);
        int j = 0;
        for (int i = 0; i < numOfAchievements; i++)
        {
            if (!Game.instance.achievements[i].hidden)
            {
                Debug.Log("TEST " + i);
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
                }
                else if ((j % 10) == 1)
                {
                    iconYOffset = subtitleYOffset + subsize.y + screenPadding * 2;
                    iconsRect[j] = new Rect(iconXOffset2, iconYOffset, iconWidth, iconHeight);
                    achievementTitleYOffset = iconYOffset;
                    achievementDescYOffset = achievementTitleYOffset + achievementTitleHeight;
                    achievementTitleRect[j] = new Rect(achievementTitleXOffset2, achievementTitleYOffset, achievementTitleSize.x, achievementTitleSize.y);
                    achievementDescRect[j] = new Rect(achievementTitleXOffset2, achievementDescYOffset, achievementDescSize.x, achievementDescSize.y);
                }
                else if ((j % 10) == 2)
                {
                    iconYOffset = subtitleYOffset + subsize.y + screenPadding * 2 + iconHeight + screenPadding * 2;
                    iconsRect[j] = new Rect(iconXOffset1, iconYOffset, iconWidth, iconHeight);
                    achievementTitleYOffset = iconYOffset;
                    achievementDescYOffset = achievementTitleYOffset + achievementTitleHeight;
                    achievementTitleRect[j] = new Rect(achievementTitleXOffset1, achievementTitleYOffset, achievementTitleSize.x, achievementTitleSize.y);
                    achievementDescRect[j] = new Rect(achievementTitleXOffset1, achievementDescYOffset, achievementDescSize.x, achievementDescSize.y);
                }
                else if ((j % 10) == 3)
                {
                    iconYOffset = subtitleYOffset + subsize.y + screenPadding * 2 + iconHeight + screenPadding * 2;
                    iconsRect[j] = new Rect(iconXOffset2, iconYOffset, iconWidth, iconHeight);
                    achievementTitleYOffset = iconYOffset;
                    achievementDescYOffset = achievementTitleYOffset + achievementTitleHeight;
                    achievementTitleRect[j] = new Rect(achievementTitleXOffset2, achievementTitleYOffset, achievementTitleSize.x, achievementTitleSize.y);
                    achievementDescRect[j] = new Rect(achievementTitleXOffset2, achievementDescYOffset, achievementDescSize.x, achievementDescSize.y);
                }
                else if ((j % 10) == 4)
                {
                    iconYOffset = subtitleYOffset + subsize.y + screenPadding * 2 + iconHeight * 2 + screenPadding * 4;
                    iconsRect[j] = new Rect(iconXOffset1, iconYOffset, iconWidth, iconHeight);
                    achievementTitleYOffset = iconYOffset;
                    achievementDescYOffset = achievementTitleYOffset + achievementTitleHeight;
                    achievementTitleRect[j] = new Rect(achievementTitleXOffset1, achievementTitleYOffset, achievementTitleSize.x, achievementTitleSize.y);
                    achievementDescRect[j] = new Rect(achievementTitleXOffset1, achievementDescYOffset, achievementDescSize.x, achievementDescSize.y);
                }
                else if ((j % 10) == 5)
                {
                    iconYOffset = subtitleYOffset + subsize.y + screenPadding * 2 + iconHeight * 2 + screenPadding * 4;
                    iconsRect[j] = new Rect(iconXOffset2, iconYOffset, iconWidth, iconHeight);
                    achievementTitleYOffset = iconYOffset;
                    achievementDescYOffset = achievementTitleYOffset + achievementTitleHeight;
                    achievementTitleRect[j] = new Rect(achievementTitleXOffset2, achievementTitleYOffset, achievementTitleSize.x, achievementTitleSize.y);
                    achievementDescRect[j] = new Rect(achievementTitleXOffset2, achievementDescYOffset, achievementDescSize.x, achievementDescSize.y);
                }
                else if ((j % 10) == 6)
                {
                    iconYOffset = subtitleYOffset + subsize.y + screenPadding * 2 + iconHeight * 3 + screenPadding * 6;
                    iconsRect[j] = new Rect(iconXOffset1, iconYOffset, iconWidth, iconHeight);
                    achievementTitleYOffset = iconYOffset;
                    achievementDescYOffset = achievementTitleYOffset + achievementTitleHeight;
                    achievementTitleRect[j] = new Rect(achievementTitleXOffset1, achievementTitleYOffset, achievementTitleSize.x, achievementTitleSize.y);
                    achievementDescRect[j] = new Rect(achievementTitleXOffset1, achievementDescYOffset, achievementDescSize.x, achievementDescSize.y);
                }
                else if ((j % 10) == 7)
                {
                    iconYOffset = subtitleYOffset + subsize.y + screenPadding * 2 + iconHeight * 3 + screenPadding * 6;
                    iconsRect[j] = new Rect(iconXOffset2, iconYOffset, iconWidth, iconHeight);
                    achievementTitleYOffset = iconYOffset;
                    achievementDescYOffset = achievementTitleYOffset + achievementTitleHeight;
                    achievementTitleRect[j] = new Rect(achievementTitleXOffset2, achievementTitleYOffset, achievementTitleSize.x, achievementTitleSize.y);
                    achievementDescRect[j] = new Rect(achievementTitleXOffset2, achievementDescYOffset, achievementDescSize.x, achievementDescSize.y);
                }
                else if ((j % 10) == 8)
                {
                    iconYOffset = subtitleYOffset + subsize.y + screenPadding * 2 + iconHeight * 4 + screenPadding * 8;
                    iconsRect[j] = new Rect(iconXOffset1, iconYOffset, iconWidth, iconHeight);
                    achievementTitleYOffset = iconYOffset;
                    achievementDescYOffset = achievementTitleYOffset + achievementTitleHeight;
                    achievementTitleRect[j] = new Rect(achievementTitleXOffset1, achievementTitleYOffset, achievementTitleSize.x, achievementTitleSize.y);
                    achievementDescRect[j] = new Rect(achievementTitleXOffset1, achievementDescYOffset, achievementDescSize.x, achievementDescSize.y);
                }
                else if ((j % 10) == 9)
                {
                    iconYOffset = subtitleYOffset + subsize.y + screenPadding * 2 + iconHeight * 4 + screenPadding * 8;
                    iconsRect[j] = new Rect(iconXOffset2, iconYOffset, iconWidth, iconHeight);
                    achievementTitleYOffset = iconYOffset;
                    achievementDescYOffset = achievementTitleYOffset + achievementTitleHeight;
                    achievementTitleRect[j] = new Rect(achievementTitleXOffset2, achievementTitleYOffset, achievementTitleSize.x, achievementTitleSize.y);
                    achievementDescRect[j] = new Rect(achievementTitleXOffset2, achievementDescYOffset, achievementDescSize.x, achievementDescSize.y);
                }
                j++;
            }
            maxPage = j / 10 - 1;
        }
        //*/

        /*

        // Banner
        bannerTexture = activeSkin.customStyles[0].normal.background;
        float bannerHeight = Screen.height * 0.76f;
        float bannerWidth = bannerHeight * ((float)bannerTexture.width / (float)bannerTexture.height);
        float bannerXOffset = (Screen.width - bannerWidth) * 0.5f;
        float bannerYOffset = -bannerHeight * 0.3f;
        bannerRect = new Rect(bannerXOffset, bannerYOffset, bannerWidth, bannerHeight);

        // Achievement Paper
        paperTexture = activeSkin.customStyles[2].normal.background;
        float paperHeight = Screen.height * 0.79f;
        float paperWidth = paperHeight * ((float)paperTexture.width / (float)paperTexture.height);
        float paperXOffset = (Screen.width - paperWidth) * 0.5f;
        float paperYOffset = paperHeight * 0.25f;
        paperRect = new Rect(paperXOffset, paperYOffset, paperWidth, paperHeight);

        // Title
        titleStyle = activeSkin.customStyles[10];
        titleStyle.fontSize = (int)(Screen.height * titleFontScale);
        Vector2 size = activeSkin.customStyles[10].CalcSize(new GUIContent(title));
        float titleXOffset = paperXOffset + paperWidth * 0.51f - size.x * 0.5f;
        float titleYOffset = paperYOffset + paperHeight * 0.11f;
        titleRect = new Rect(titleXOffset, titleYOffset, size.x, size.y);

        // Reward
        rewardStyle = activeSkin.customStyles[11];
        rewardStyle.fontSize = (int)(Screen.height * rewardFontScale);
        Vector2 rewardSize = activeSkin.customStyles[11].CalcSize(new GUIContent(achievementController.rewardAmount[0]));
        rewardTexture = achievementController.rewards[0];
        float rewardHeight = rewardSize.y;
        float rewardWidth = rewardHeight * ((float)rewardTexture.width / (float)rewardTexture.height);
        float rewardXOffset1 = paperXOffset + paperWidth * 0.51f - rewardWidth - screenPadding;
        float rewardXOffset2 = paperXOffset + paperWidth * 0.51f + screenPadding;
        float rewardYOffset = titleYOffset + screenPadding + size.y;
        rewardRect = new Rect(rewardXOffset1, rewardYOffset, rewardWidth, rewardHeight);
        rewardNumberRect = new Rect(rewardXOffset2, rewardYOffset, rewardSize.x, rewardSize.y);
        rewardNumber = achievementController.rewardAmount[0];

        // Decoration
        decorTexture = activeSkin.customStyles[5].normal.background;
        float decorHeight = size.y * 0.7f;
        float decorWidth = decorHeight * ((float)decorTexture.width / (float)decorTexture.height);
        float leftDecorXOffset = titleXOffset - screenPadding;
        float rightDecorXOffset = titleXOffset + size.x + screenPadding;
        float decorYOffset = titleYOffset + size.y * 0.15f;
        leftDecorRect = new Rect(leftDecorXOffset, decorYOffset, -decorWidth, decorHeight);
        rightDecorRect = new Rect(rightDecorXOffset, decorYOffset, decorWidth, decorHeight);

        // Tick Boxes
        tickBoxTexture = activeSkin.customStyles[3].normal.background;
        float tickBoxWidth = paperWidth * 0.07f;
        float tickBoxHeight = tickBoxWidth * ((float)tickBoxTexture.height / (float)tickBoxTexture.width);
        float tickBoxXOffset = paperXOffset + paperWidth * 0.25f;
        float tickBoxYOffset = paperYOffset;
        tickBoxRect1 = new Rect(tickBoxXOffset, tickBoxYOffset + paperHeight * 0.35f, tickBoxWidth, tickBoxHeight);
        tickBoxRect2 = new Rect(tickBoxXOffset, tickBoxYOffset + paperHeight * 0.47f, tickBoxWidth, tickBoxHeight);
        tickBoxRect3 = new Rect(tickBoxXOffset, tickBoxYOffset + paperHeight * 0.59f, tickBoxWidth, tickBoxHeight);
        tickBoxRect4 = new Rect(tickBoxXOffset, tickBoxYOffset + paperHeight * 0.71f, tickBoxWidth, tickBoxHeight);

        // Ticks
        tickTexture = activeSkin.customStyles[4].normal.background;
        tickRect1 = new Rect(tickBoxXOffset, tickBoxYOffset + paperHeight * 0.35f, tickBoxWidth, tickBoxHeight);
        tickRect2 = new Rect(tickBoxXOffset, tickBoxYOffset + paperHeight * 0.47f, tickBoxWidth, tickBoxHeight);
        tickRect3 = new Rect(tickBoxXOffset, tickBoxYOffset + paperHeight * 0.59f, tickBoxWidth, tickBoxHeight);
        tickRect4 = new Rect(tickBoxXOffset, tickBoxYOffset + paperHeight * 0.71f, tickBoxWidth, tickBoxHeight);

        // Achievement Text
        achievementStyle = activeSkin.customStyles[9];
        titleStyle.fontSize = (int)(Screen.height * titleFontScale);
        achievement1 = achievementController.achievements[0].rows[0].ToString();
        achievement2 = achievementController.achievements[0].rows[1].ToString();
        achievement3 = achievementController.achievements[0].rows[2].ToString();
        achievement4 = achievementController.achievements[0].rows[3].ToString();
        Vector2 achievementSize1 = activeSkin.customStyles[9].CalcSize(new GUIContent(achievement1));
        Vector2 achievementSize2 = activeSkin.customStyles[9].CalcSize(new GUIContent(achievement2));
        Vector2 achievementSize3 = activeSkin.customStyles[9].CalcSize(new GUIContent(achievement3));
        Vector2 achievementSize4 = activeSkin.customStyles[9].CalcSize(new GUIContent(achievement4));
        float achievementXOffset = tickBoxXOffset + tickBoxWidth;
        float achievementYOffset1 = tickBoxYOffset + paperHeight * 0.35f + tickBoxHeight * 0.55f;
        float achievementYOffset2 = tickBoxYOffset + paperHeight * 0.47f + tickBoxHeight * 0.55f;
        float achievementYOffset3 = tickBoxYOffset + paperHeight * 0.59f + tickBoxHeight * 0.55f;
        float achievementYOffset4 = tickBoxYOffset + paperHeight * 0.71f + tickBoxHeight * 0.55f;
        achievementRect1 = new Rect(achievementXOffset, achievementYOffset1, achievementSize1.x, achievementSize1.y);
        achievementRect2 = new Rect(achievementXOffset, achievementYOffset2, achievementSize2.x, achievementSize2.y);
        achievementRect3 = new Rect(achievementXOffset, achievementYOffset3, achievementSize3.x, achievementSize3.y);
        achievementRect4 = new Rect(achievementXOffset, achievementYOffset4, achievementSize4.x, achievementSize4.y);

        // Thumbs
        thumbsTexture = activeSkin.customStyles[6].normal.background;
        float thumbsHeight = Screen.height * 0.4f;
        float thumbsWidht = thumbsHeight * ((float)thumbsTexture.width / (float)thumbsTexture.height);
        float thumbsXOffset = paperXOffset + paperWidth - thumbsWidht * 1.3f;
        float thumbsYOffset = paperYOffset + paperHeight - thumbsHeight;
        thumbsRect = new Rect(thumbsXOffset, thumbsYOffset, thumbsWidht, thumbsHeight);
         */

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

        /*

        GUI.DrawTexture(iconsRect, iconText);
        GUI.Label(achievementTitleRect, achievementTitleStr, achievementTitleStyle);
        GUI.Label(achievementDescRect, achievementDesctStr, achievementDescStyle);

        GUI.DrawTexture(iconsRect1, iconText);
        GUI.Label(achievementTitleRect1, achievementTitleStr, achievementTitleStyle);
        GUI.Label(achievementDescRect1, achievementDesctStr, achievementDescStyle);

        GUI.DrawTexture(iconsRect2, iconText);
        GUI.Label(achievementTitleRect2, achievementTitleStr, achievementTitleStyle);
        GUI.Label(achievementDescRect2, achievementDesctStr, achievementDescStyle);

        GUI.DrawTexture(iconsRect3, iconText);
        GUI.Label(achievementTitleRect3, achievementTitleStr, achievementTitleStyle);
        GUI.Label(achievementDescRect3, achievementDesctStr, achievementDescStyle);

        GUI.DrawTexture(iconsRect4, iconText);
        GUI.Label(achievementTitleRect4, achievementTitleStr, achievementTitleStyle);
        GUI.Label(achievementDescRect4, achievementDesctStr, achievementDescStyle);

        */

        //*
        for (int i = 0; i < 10; i++)
        {
            if ((i + page * 10) < iconsRect.Length)
            {
                GUI.DrawTexture(iconsRect[i + page * 10], iconText);
                GUI.Label(achievementTitleRect[i + page * 10], achievementTitleStr[i + page * 10], achievementTitleStyle);
                GUI.Label(achievementDescRect[i + page * 10], achievementDescStr[i + page * 10], achievementDescStyle);
            }
        }
         //*/

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
