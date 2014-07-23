using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievementGUI : MonoBehaviour
{

    #region Global Variables
    // GUI Skin
    public GUISkin activeSkin;
    private AchievementController achievementController;

    // Triggers
    private bool sound = false;

    // Page variables
    public Rect bgRect;
    public Texture bgTexture;
    private float screenPaddingPercentage = 0.01f; // percentage of screen size;

    // Achievement Banner
    private Rect bannerRect;
    private Texture bannerTexture;

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
    private float achievementFontScale = 0.04f;
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
    private float titleFontScale = 0.1f;
    private string title = "GG";

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

    #endregion

    // Use this for initialization
	void Start ()
    {

        #region GUI

        bgRect = new Rect(0, 0, Screen.width, Screen.height);
        bgTexture = activeSkin.customStyles[1].normal.background;
        achievementController = gameObject.GetComponent<AchievementController>();

        // Calculate screen padding
        float screenPadding = Screen.height * screenPaddingPercentage;

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

        // Buttons
        float btnBackHeight = Screen.height * 0.1f;
        float btnBackWidth = btnBackHeight * ((float)activeSkin.customStyles[8].normal.background.width /
                                          (float)activeSkin.customStyles[8].normal.background.height);
        btnBackRect = new Rect(screenPadding, screenPadding, btnBackWidth, btnBackHeight);

        float btnControlHeight = Screen.height * 0.2f;
        float btnControlWidth = btnControlHeight * ((float)activeSkin.customStyles[7].normal.background.width /
                                                    (float)activeSkin.customStyles[7].normal.background.height);
        btnPreviousRect = new Rect(screenPadding + btnControlWidth, (Screen.height - btnControlHeight) * 0.5f, -btnControlWidth, btnControlHeight);
        btnNextRect = new Rect(Screen.width - screenPadding - btnControlWidth, (Screen.height - btnControlHeight) * 0.5f, btnControlWidth, btnControlHeight);

        // Initialise button scalers
        btnBackScale = new ButtonHandler(btnBackRect, gameObject, 0.9f, "Back_ScaleButton");

        #endregion

    }
	
	// Update is called once per frame
	void OnGUI () {
        GUI.skin = activeSkin;
        AchievementScreen();
	}

    private void AchievementScreen()
    {
        // Background
        GUI.DrawTexture(bgRect, bgTexture, ScaleMode.ScaleAndCrop);

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

        // back button
        if (GUI.Button(btnBackRect, "", activeSkin.customStyles[8]))
        {

        }

        // Page Control Buttons
        if (GUI.Button(btnPreviousRect, "", activeSkin.customStyles[7]))
        {

        }
        if (GUI.Button(btnNextRect, "", activeSkin.customStyles[7]))
        {

        }

        btnBackScale.OnMouseOver(btnBackRect);
    }

    void Back_ScaleButton(Rect size)
    {
        btnBackRect = size;
    }
}
