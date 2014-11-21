using UnityEngine;
using System.Collections;
using System.IO;

public class StageSelectGUI : MonoBehaviour {

    /* GUI Skin
     * Custom Styles [0] = Energy
     * Custom Styles [1] = Back Button
     * Custom Styles [2] = Sound Button
     * Custom Styles [3] = Background Normal
     * Custom Styles [4] = Frame Normal
     * Custom Styles [5] = Button Locked
     * Custom Styles [6] = Button 0 Star
     * Custom Styles [7] = Button 1 Star
     * Custom Styles [8] = Button 2 Star
     * Custom Styles [9] = Button 3 Star
     * Custom Styles [10] = Toggle Challenge Inactive
     * Custom Styles [11] = Toggle Challenge Normal
     * Custom Styles [12] = Background Challenge
     * Custom Styles [13] = Frame Challenge
     * Custom Styles [14] = Button Challenge Locked
     * Custom Styles [15] = Button Challenge 0 Star
     * Custom Styles [16] = Button Challenge 1 Star
     * Custom Styles [17] = Button Challenge 2 Star
     * Custom Styles [18] = Button Challenge 3 Star
     * Custom Styles [19] = Arrow Prev
     * Custom Styles [20] = Arrow Next
     * Custom Styles [21] = Story Toolbar
     */
    public GUISkin activeSkin;
    public Texture storyThumbnail;

    #region Touch Controls
    
    private int maxTouches = 1;	// up to 5 (iOS only supports 5 apparently)
    private float minDragDistance = 50f; // Swipe distance before touch is regarded as 'touch and drag'
    
    private Vector2[] touchStartPosition;

    #endregion

    #region Variables

    public int chapterNumber;
    public string[] nStages = new string[10];
    public string[] cStages = new string[10];
    public string[] story;

    #endregion

    #region GUI related

    private Rect containerRect; // The Rect object that encapsulates the whole page
    private Rect innerNormalRect; // The Rect object that holds the normal stages fram
    private Rect innerChallengeRect; // The Rect object that holds the challenge stages fram

    // Rect animation positions
    private Rect innerPosLeftRect; // holds position value when inner frame is on the left side
    private Rect innerPosMiddleRect; // holds position value when inner frame is on the middle
    private Rect innerPosRightRect; // holds position value when inner frame is on the right side

    #region background

    private Rect bgRect; // Rect that holds the background
    private Texture bgTexture; // The background texture
    private Texture bgChallengeTexture; // The background for the challenge stage

    #endregion
    #region buttons

    // Back Button
    private Rect backBtnRect;
    private GUIStyle backBtnStyle;
    private ButtonHandler backBtnHandler;
    private float backBtnXOffset = 0f;
    private float backBtnYOffset = 0f;
    private float backBtnScale = 0.13f;

    // Sound Button
    private bool sound = false;
    private Rect soundBtnRect;
    private GUIStyle soundBtnStyle;
    private ButtonHandler soundBtnHandler;
    private float soundBtnScale = 0.09f;
    private float soundBtnXOffset = 0.01f;
    private float soundBtnYOffset = 0.9f;

    // Challenge Toggle Button
    private bool wasChallenge = false;
    private bool isChallenge = false;
    private Rect challengeBtnRect;
    private GUIStyle challengeBtnStyle;
    private ButtonHandler challengeBtnHandler;
    private float challengeBtnScale = 0.08f;
    private float challengeBtnXOffset = 0.011f; // offset from left
    private float challengeBtnYOffset = 0.015f; // offset from bottom

    // Energy
    private Rect energyRect;
    private GUIStyle energyStyle;
    private float energyScale = 0.1f;
    private float energyFontScale = 0.3f;
    private float energyXOffset = 0.01f; // Offset from the right
    private float energyYOffset = 0.01f; // Offset from the top
    private float energyLabelXOffset = 0.62f;
    private float energyLabelYOffset = 0.06f;
    
    #endregion
    #region main content

    // Content Header
    private Rect contentHeaderRect;
    private GUIStyle contentHeaderStyle;
    private float contentHeaderScale = 0.07f;
    private float contentHeaderYOffset = 0.04f;

    // Challenge Header
    private Rect challengeHeaderRect;
    private GUIStyle challengeHeaderStyle;
    private float challengeHeaderYOffset = 0.155f;

    // Scary Challenge Text
    private Rect challengeLabelRect;
    private GUIStyle challengeLabelStyle;
    private float challengeLabelScale = 0.09f;
    private float challengeLabelYOffset = 0.35f;

    // Content Container
    private Rect contentContainerRect;
    private Rect contentContainerBgRect;
    private Texture normalContainerBgTexture;
    private Texture challengeContainerBgTexture;
    private float contentContainerScale = 1;

    #region stage buttons

    public enum StageBtnState { Star_3, Star_2, Star_1, Star_0, Locked };
    private GUIStyle stageBtn_Locked;
    private GUIStyle stageBtn_0Star;
    private GUIStyle stageBtn_1Star;
    private GUIStyle stageBtn_2Star;
    private GUIStyle stageBtn_3Star;
    private GUIStyle challengeStageBtn_Locked;
    private GUIStyle challengeStageBtn_0Star;
    private GUIStyle challengeStageBtn_1Star;
    private GUIStyle challengeStageBtn_2Star;
    private GUIStyle challengeStageBtn_3Star;

    /* ╔═══╗ ╔═══╗ ╔═══╗ ╔═══╗ ╔═══╗
     * ║ 0 ║ ║ 1 ║ ║ 2 ║ ║ 3 ║ ║ 4 ║
     * ╚═══╝ ╚═══╝ ╚═══╝ ╚═══╝ ╚═══╝
     * ╔═══╗ ╔═══╗ ╔═══╗ ╔═══╗ ╔═══╗
     * ║ 5 ║ ║ 6 ║ ║ 7 ║ ║ 8 ║ ║ 9 ║
     * ╚═══╝ ╚═══╝ ╚═══╝ ╚═══╝ ╚═══╝
     */
    private Rect stagesContainerRect;
    private float stagesContainerYOffset = 0.57f;
    private float stagesXSpacingScale = 0.01f;
    private float stagesYSpacingScale = -0.01f;
    private float stagesBtnScale = 0.16f;

    private Rect stage1Rect;
    private Rect stage2Rect;
    private Rect stage3Rect;
    private Rect stage4Rect;
    private Rect stage5Rect;
    private Rect stage6Rect;
    private Rect stage7Rect;
    private Rect stage8Rect;
    private Rect stage9Rect;
    private Rect stage10Rect;

    private StageBtnState stage1State;
    private StageBtnState stage2State;
    private StageBtnState stage3State;
    private StageBtnState stage4State;
    private StageBtnState stage5State;
    private StageBtnState stage6State;
    private StageBtnState stage7State;
    private StageBtnState stage8State;
    private StageBtnState stage9State;
    private StageBtnState stage10State;
    private StageBtnState challengeStage1State;
    private StageBtnState challengeStage2State;
    private StageBtnState challengeStage3State;
    private StageBtnState challengeStage4State;
    private StageBtnState challengeStage5State;
    private StageBtnState challengeStage6State;
    private StageBtnState challengeStage7State;
    private StageBtnState challengeStage8State;
    private StageBtnState challengeStage9State;
    private StageBtnState challengeStage10State;

    private GUIStyle stage1Style;
    private GUIStyle stage2Style;
    private GUIStyle stage3Style;
    private GUIStyle stage4Style;
    private GUIStyle stage5Style;
    private GUIStyle stage6Style;
    private GUIStyle stage7Style;
    private GUIStyle stage8Style;
    private GUIStyle stage9Style;
    private GUIStyle stage10Style;

    private GUIStyle challengeStage1Style;
    private GUIStyle challengeStage2Style;
    private GUIStyle challengeStage3Style;
    private GUIStyle challengeStage4Style;
    private GUIStyle challengeStage5Style;
    private GUIStyle challengeStage6Style;
    private GUIStyle challengeStage7Style;
    private GUIStyle challengeStage8Style;
    private GUIStyle challengeStage9Style;
    private GUIStyle challengeStage10Style;

    private ButtonHandler stage1Handler;
    private ButtonHandler stage2Handler;
    private ButtonHandler stage3Handler;
    private ButtonHandler stage4Handler;
    private ButtonHandler stage5Handler;
    private ButtonHandler stage6Handler;
    private ButtonHandler stage7Handler;
    private ButtonHandler stage8Handler;
    private ButtonHandler stage9Handler;
    private ButtonHandler stage10Handler;

    private float stageBtnLabelScale = 0.36f;
    private float stageBtnLabelXOffset = 0f;
    private float stageBtnLabelYOffset = 0.06f;

    #endregion

    #region story carousel

    private bool inTransition = false;
    private int cur_page;
    private int nxt_page;
    private int max_page;
    private Rect storyContainerRect;

    private Rect storyInnerActiveRect;
    private Rect storyInnerTempRect;
    private Rect storyPosLeft;
    private Rect storyPosMid;
    private Rect storyPosRight;

    private float storyContainerXOffset = 0.135f;
    private float storyContainerYOffset = 0.24f;
    private float storyContainerWidth = 0.52f;
    private float storyContainerHeight = 0.31f;
    private float storyContainerFontScale = 0.036f;

    private Rect arrowNextRect;
    private Rect arrowPrevRect;
    private GUIStyle arrowNextStyle;
    private GUIStyle arrowPrevStyle;
    private float arrowScale = 0.06f;

    private Rect storyNavRect;
    private GUIStyle storyNavStyle;
    private float storyNavScale = 0.08f;
    private float storyNavSpacing = 0.55f;

    private string[] storyNavStrings;

    #endregion

    #region thumbnail

    private Rect thumbnailRect;
    public float thumbnailXOffset;
    public float thumbnailYOffset;
    public float thumbnailScale;

    #endregion

    #endregion

    #endregion

    // Use this for initialization
	void Start () 
    {
        #region Touch Controls

        // inititialise the arrays used for manipulating the touch controls
        touchStartPosition = new Vector2[maxTouches];

        #endregion
        #region GUI

        // Set the container rect
        containerRect = new Rect(0, 0, Screen.width, Screen.height);
        
        // Set animation position rects
        innerPosLeftRect = new Rect(-Screen.width, 0, Screen.width, Screen.height);
        innerPosMiddleRect = new Rect(0, 0, Screen.width, Screen.height);
        innerPosRightRect = new Rect(Screen.width, 0, Screen.width, Screen.height);

        // Set default position
        innerNormalRect = innerPosMiddleRect;
        innerChallengeRect = innerPosLeftRect;

        #region background

        // Background
        bgRect = new Rect(0, 0, Screen.width, Screen.height);
        bgTexture = activeSkin.customStyles[3].normal.background;
        bgChallengeTexture = activeSkin.customStyles[12].normal.background;

        #endregion
        
        #region main content

        normalContainerBgTexture = activeSkin.customStyles[4].normal.background;
        challengeContainerBgTexture = activeSkin.customStyles[13].normal.background;
        float contentContainerHeight = Screen.height * contentContainerScale;
        float contentContainerWidth = contentContainerHeight * ((float)normalContainerBgTexture.width / (float)normalContainerBgTexture.height);
        float contentContainerXOffset = (Screen.width - contentContainerWidth) * 0.5f;
        float contentContainerYOffset = (Screen.height - contentContainerHeight) * 0.5f;
        contentContainerRect = new Rect(contentContainerXOffset, contentContainerYOffset, contentContainerWidth, contentContainerHeight);
        contentContainerBgRect = new Rect(0, 0, contentContainerWidth, contentContainerHeight);

        #region headers

        // normal header
        contentHeaderStyle = new GUIStyle(activeSkin.label);
        contentHeaderStyle.fontSize = (int)(contentContainerHeight * contentHeaderScale);
        contentHeaderStyle.alignment = TextAnchor.MiddleCenter;
        float headerWidth = contentContainerWidth;
        float headerHeight = contentHeaderStyle.fontSize * 2;
        float headerXOffset = 0;
        float headerYOffset = contentContainerHeight * contentHeaderYOffset;
        contentHeaderRect = new Rect(headerXOffset, headerYOffset, headerWidth, headerHeight);

        // challenge header
        challengeHeaderStyle = new GUIStyle(activeSkin.label);
        challengeHeaderStyle.fontSize = (int)(contentContainerHeight * contentHeaderScale);
        challengeHeaderStyle.normal.textColor = Color.white;
        challengeHeaderStyle.alignment = TextAnchor.MiddleCenter;
        float cHeaderWidth = contentContainerWidth;
        float cHeaderHeight = challengeHeaderStyle.fontSize * 2;
        float cHeaderXOffset = 0;
        float cHeaderYOffset = contentContainerHeight * challengeHeaderYOffset;
        challengeHeaderRect = new Rect(cHeaderXOffset, cHeaderYOffset, cHeaderWidth, cHeaderHeight);
        
        // challenge label
        challengeLabelStyle = new GUIStyle(activeSkin.label);
        challengeLabelStyle.fontSize = (int)(contentContainerHeight * challengeLabelScale);
        challengeLabelStyle.normal.textColor = Color.red;
        challengeLabelStyle.alignment = TextAnchor.MiddleCenter;
        float cLabelWidth = contentContainerWidth;
        float cLabelHeight = challengeLabelStyle.CalcHeight(new GUIContent("N\nN"), cLabelWidth);
        float cLabelXOffset = 0;
        float cLabelYOffset = contentContainerHeight * challengeLabelYOffset;
        challengeLabelRect = new Rect(cLabelXOffset, cLabelYOffset, cLabelWidth, cLabelHeight);

        #endregion
       
        #region stage buttons

        Texture stageBtnTexture = activeSkin.customStyles[5].normal.background;
        float stageBtnHeight = contentContainerHeight * stagesBtnScale;
        float stageBtnWidth = stageBtnHeight * ((float)stageBtnTexture.width / (float)stageBtnTexture.height);
        float stageBtnXSpacing = contentContainerWidth * stagesXSpacingScale;
        float stageBtnYSpacing = contentContainerWidth * stagesYSpacingScale;

        float stagesWidth = stageBtnWidth * 5 + stageBtnXSpacing * 4;
        float stagesHeight = stageBtnHeight * 2 + stageBtnYSpacing;
        float stagesXOffset = (contentContainerWidth - stagesWidth) * 0.5f;
        float stagesYOffset = contentContainerHeight * stagesContainerYOffset;

        stagesContainerRect = new Rect(stagesXOffset, stagesYOffset, stagesWidth, stagesHeight);

        stage1Rect = new Rect(0, 0, stageBtnWidth, stageBtnHeight);
        stage2Rect = new Rect(stageBtnWidth + stageBtnXSpacing, 0, stageBtnWidth, stageBtnHeight);
        stage3Rect = new Rect(2 * (stageBtnWidth + stageBtnXSpacing), 0, stageBtnWidth, stageBtnHeight);
        stage4Rect = new Rect(3 * (stageBtnWidth + stageBtnXSpacing), 0, stageBtnWidth, stageBtnHeight);
        stage5Rect = new Rect(4 * (stageBtnWidth + stageBtnXSpacing), 0, stageBtnWidth, stageBtnHeight);

        stage6Rect = new Rect(0, stageBtnHeight + stageBtnYSpacing, stageBtnWidth, stageBtnHeight);
        stage7Rect = new Rect(stageBtnWidth + stageBtnXSpacing, stageBtnHeight + stageBtnYSpacing, stageBtnWidth, stageBtnHeight);
        stage8Rect = new Rect(2 * (stageBtnWidth + stageBtnXSpacing), stageBtnHeight + stageBtnYSpacing, stageBtnWidth, stageBtnHeight);
        stage9Rect = new Rect(3 * (stageBtnWidth + stageBtnXSpacing), stageBtnHeight + stageBtnYSpacing, stageBtnWidth, stageBtnHeight);
        stage10Rect = new Rect(4 * (stageBtnWidth + stageBtnXSpacing), stageBtnHeight + stageBtnYSpacing, stageBtnWidth, stageBtnHeight);

        // Set the button font size
        activeSkin.customStyles[6].fontSize = (int)(stageBtnHeight * stageBtnLabelScale);
        activeSkin.customStyles[7].fontSize = (int)(stageBtnHeight * stageBtnLabelScale);
        activeSkin.customStyles[8].fontSize = (int)(stageBtnHeight * stageBtnLabelScale);
        activeSkin.customStyles[9].fontSize = (int)(stageBtnHeight * stageBtnLabelScale);

        activeSkin.customStyles[6].contentOffset = new Vector2(stageBtnWidth * stageBtnLabelXOffset, stageBtnHeight * stageBtnLabelYOffset);
        activeSkin.customStyles[7].contentOffset = new Vector2(stageBtnWidth * stageBtnLabelXOffset, stageBtnHeight * stageBtnLabelYOffset);
        activeSkin.customStyles[8].contentOffset = new Vector2(stageBtnWidth * stageBtnLabelXOffset, stageBtnHeight * stageBtnLabelYOffset);
        activeSkin.customStyles[9].contentOffset = new Vector2(stageBtnWidth * stageBtnLabelXOffset, stageBtnHeight * stageBtnLabelYOffset);

        // Set the styles to put on each stage (need to check database here)
        stageBtn_Locked = new GUIStyle(activeSkin.customStyles[5]);
        stageBtn_1Star  = new GUIStyle(activeSkin.customStyles[7]);
        stageBtn_0Star  = new GUIStyle(activeSkin.customStyles[6]);
        stageBtn_2Star  = new GUIStyle(activeSkin.customStyles[8]);
        stageBtn_3Star  = new GUIStyle(activeSkin.customStyles[9]);
        challengeStageBtn_Locked = new GUIStyle(activeSkin.customStyles[14]);
        challengeStageBtn_0Star  = new GUIStyle(activeSkin.customStyles[15]);
        challengeStageBtn_1Star  = new GUIStyle(activeSkin.customStyles[16]);
        challengeStageBtn_2Star  = new GUIStyle(activeSkin.customStyles[17]);
        challengeStageBtn_3Star  = new GUIStyle(activeSkin.customStyles[18]);
        
        // Get the normal button states
        stage1State = getButtonState(0, false);
        stage2State = getButtonState(1, false);
        stage3State = getButtonState(2, false);
        stage4State = getButtonState(3, false);
        stage5State = getButtonState(4, false);
        stage6State = getButtonState(5, false);
        stage7State = getButtonState(6, false);
        stage8State = getButtonState(7, false);
        stage9State = getButtonState(8, false);
        stage10State = getButtonState(9, false);
        
        // Get the challenge button states
        challengeStage1State = getButtonState(0, true);
        challengeStage2State = getButtonState(1, true);
        challengeStage3State = getButtonState(2, true);
        challengeStage4State = getButtonState(3, true);
        challengeStage5State = getButtonState(4, true);
        challengeStage6State = getButtonState(5, true);
        challengeStage7State = getButtonState(6, true);
        challengeStage8State = getButtonState(7, true);
        challengeStage9State = getButtonState(8, true);
        challengeStage10State = getButtonState(9, true);

        // Set the normal button styles
        stage1Style = getButtonStyle(stage1State, false);
        stage2Style = getButtonStyle(stage2State, false);
        stage3Style = getButtonStyle(stage3State, false);
        stage4Style = getButtonStyle(stage4State, false);
        stage5Style = getButtonStyle(stage5State, false);
        stage6Style = getButtonStyle(stage6State, false);
        stage7Style = getButtonStyle(stage7State, false);
        stage8Style = getButtonStyle(stage8State, false);
        stage9Style = getButtonStyle(stage9State, false);
        stage10Style = getButtonStyle(stage10State, false);

        // Set the challenge button styles
        challengeStage1Style = getButtonStyle(challengeStage1State, true);
        challengeStage2Style = getButtonStyle(challengeStage2State, true);
        challengeStage3Style = getButtonStyle(challengeStage3State, true);
        challengeStage4Style = getButtonStyle(challengeStage4State, true);
        challengeStage5Style = getButtonStyle(challengeStage5State, true);
        challengeStage6Style = getButtonStyle(challengeStage6State, true);
        challengeStage7Style = getButtonStyle(challengeStage7State, true);
        challengeStage8Style = getButtonStyle(challengeStage8State, true);
        challengeStage9Style = getButtonStyle(challengeStage9State, true);
        challengeStage10Style = getButtonStyle(challengeStage10State, true);

        // Set the button handlers
        stage1Handler = new ButtonHandler(stage1Rect, gameObject, 0.9f, "ScaleStage1Button");
        stage2Handler = new ButtonHandler(stage2Rect, gameObject, 0.9f, "ScaleStage2Button");
        stage3Handler = new ButtonHandler(stage3Rect, gameObject, 0.9f, "ScaleStage3Button");
        stage4Handler = new ButtonHandler(stage4Rect, gameObject, 0.9f, "ScaleStage4Button");
        stage5Handler = new ButtonHandler(stage5Rect, gameObject, 0.9f, "ScaleStage5Button");
        stage6Handler = new ButtonHandler(stage6Rect, gameObject, 0.9f, "ScaleStage6Button");
        stage7Handler = new ButtonHandler(stage7Rect, gameObject, 0.9f, "ScaleStage7Button");
        stage8Handler = new ButtonHandler(stage8Rect, gameObject, 0.9f, "ScaleStage8Button");
        stage9Handler = new ButtonHandler(stage9Rect, gameObject, 0.9f, "ScaleStage9Button");
        stage10Handler = new ButtonHandler(stage10Rect, gameObject, 0.9f, "ScaleStage10Button");

        #endregion

        #endregion

        #region buttons

        // Back button
        backBtnStyle = activeSkin.customStyles[1];
        Texture backBtnTexture = backBtnStyle.normal.background;
        float backBtnHeight = Screen.height * backBtnScale;
        float backBtnWidth = backBtnHeight * ((float)backBtnTexture.width / (float)backBtnTexture.height);
        float backXOffset = Screen.width * backBtnXOffset;
        float backYOffset = Screen.height * backBtnYOffset;
        backBtnRect = new Rect(backXOffset, backYOffset, backBtnWidth, backBtnHeight);
        backBtnHandler = new ButtonHandler(backBtnRect, gameObject, 0.9f, "ScaleBackButton");

        // Sound button
        soundBtnStyle = activeSkin.customStyles[2];
        Texture soundBtnTexture = soundBtnStyle.normal.background;
        float soundBtnHeight = Screen.height * soundBtnScale;
        float soundBtnWidth = soundBtnHeight * ((float)soundBtnTexture.width / (float)soundBtnTexture.height);
        float soundXOffset = Screen.width * (1 - soundBtnXOffset) - soundBtnWidth;
        float soundYOffset = Screen.height * soundBtnYOffset;
        soundBtnRect = new Rect(soundXOffset, soundYOffset, soundBtnWidth, soundBtnHeight);
        soundBtnHandler = new ButtonHandler(soundBtnRect, gameObject, 0.9f, "ScaleSoundButton");

        // Challenge button
        //if (challengeStage1State == StageBtnState.Locked)
        if(!Game.instance.challengeChapterUnlocked[chapterNumber-1])
        {
            challengeBtnStyle = activeSkin.customStyles[10];
        }
        else
        {
            challengeBtnStyle = activeSkin.customStyles[11];
        }
        Texture challengeBtnTexture = challengeBtnStyle.normal.background;
        float challengeBtnWidth = Screen.width * challengeBtnScale;
        float challengeBtnHeight = challengeBtnWidth * ((float)challengeBtnTexture.height / (float)challengeBtnTexture.width);
        float challengeXOffset = Screen.width * challengeBtnXOffset;
        float challengeYOffset = Screen.height * (1 - challengeBtnYOffset) - challengeBtnHeight;
        challengeBtnRect = new Rect(challengeXOffset, challengeYOffset, challengeBtnWidth, challengeBtnHeight);
        challengeBtnHandler = new ButtonHandler(challengeBtnRect, gameObject, 0.9f, "ScaleChallengeButton");

        // Energy bar
        energyStyle = activeSkin.customStyles[0];
        Texture energyTexture = energyStyle.normal.background;
        float energyHeight = Screen.height * energyScale;
        float energyWidth = energyHeight * ((float)energyTexture.width / (float)energyTexture.height);
        float eXOffset = Screen.width * (1 - energyXOffset) - energyWidth;
        float eYOffset = Screen.height * energyYOffset;
        energyRect = new Rect(eXOffset, eYOffset, energyWidth, energyHeight);

        energyStyle.fontSize = (int)(energyHeight * energyFontScale);
        energyStyle.contentOffset = new Vector2(energyWidth * energyLabelXOffset, energyHeight * energyLabelYOffset);

        #endregion

        #region story
        
        cur_page = 1;
        nxt_page = 1;
        max_page = story.Length;

        float storyHeight = contentContainerRect.height * storyContainerHeight;
        float storyWidth = contentContainerRect.width * storyContainerWidth;
        float storyXOffset = contentContainerRect.width * storyContainerXOffset;
        float storyYOffset = contentContainerRect.height * storyContainerYOffset;

        storyContainerRect = new Rect(storyXOffset, storyYOffset, storyWidth, storyHeight);
        storyPosMid = new Rect(0, 0, storyWidth, storyHeight);
        storyPosLeft = new Rect(-storyWidth, 0, storyWidth, storyHeight);
        storyPosRight = new Rect(storyWidth, 0, storyWidth, storyHeight);
        storyInnerActiveRect = storyPosMid;
        storyInnerTempRect = storyPosRight;

        activeSkin.label.fontSize = (int)(contentContainerRect.height * storyContainerFontScale);
        
        arrowPrevStyle = activeSkin.customStyles[19];
        arrowNextStyle = activeSkin.customStyles[20];
        Texture arrowTexture = arrowNextStyle.normal.background;
        float arrowHeight = Screen.height * arrowScale;
        float arrowWidth = arrowHeight * ((float)arrowTexture.width / (float)arrowTexture.height);
        arrowNextRect = new Rect(storyXOffset + storyWidth + 0.5f * arrowWidth,
                                 storyYOffset + 0.5f * (storyHeight - arrowHeight),
                                 arrowWidth, arrowHeight);
        arrowPrevRect = new Rect(storyXOffset - 1.5f * (arrowWidth),
                                 storyYOffset + 0.5f * (storyHeight - arrowHeight),
                                 arrowWidth, arrowHeight);

        // Navigation toolbar
        storyNavStrings = new string[max_page];
        storyNavStyle = activeSkin.customStyles[21];
        
        float toolbarWidth = storyWidth;
        float toolbarHeight = storyHeight * storyNavScale;
        float toolbarXOffset = storyXOffset;
        float toolbarYOffset = storyYOffset + storyHeight;

        storyNavRect = new Rect(toolbarXOffset, toolbarYOffset, toolbarWidth, toolbarHeight);
        storyNavStyle.fixedHeight = toolbarHeight;
        storyNavStyle.fixedWidth = toolbarHeight;
        storyNavStyle.margin = new RectOffset((int)(toolbarHeight * storyNavSpacing),
                                              (int)(toolbarHeight * storyNavSpacing), 0, 0);


        #endregion
        #region thumbnail

        float thumbWidth = contentContainerRect.width * thumbnailScale;
        float thumbHeight = thumbWidth * ((float)storyThumbnail.height / (float)storyThumbnail.width);
        float thumbXOffset = contentContainerRect.width * thumbnailXOffset;
        float thumbYOffset = contentContainerRect.height * thumbnailYOffset;

        thumbnailRect = new Rect(thumbXOffset, thumbYOffset, thumbWidth, thumbHeight);

        #endregion
        #endregion
    }

    StageBtnState getButtonState(int stage_num, bool isChallengeStage)
    {
        bool unlocked = false;
        if (isChallengeStage)
        {
            unlocked = Game.instance.challengeLevelsUnlocked[chapterNumber-1][stage_num];
        }
        else
        {
            unlocked = Game.instance.levelsUnlocked[chapterNumber-1][stage_num];
        }

        if (unlocked)
        {
            var stars = 0;
            if (isChallengeStage)
            {
                stars = Game.instance.challengeStars[chapterNumber-1][stage_num];
            }
            else
            {
                stars = Game.instance.stars[chapterNumber-1][stage_num];
            }

            if (stars == 0)
            {
                return StageBtnState.Star_0;
            }
            else if (stars == 1)
            {
                return StageBtnState.Star_1;
            }
            else if (stars == 2)
            {
                return StageBtnState.Star_2;
            }
            else if (stars == 3)
            {
                return StageBtnState.Star_3;
            }
        }

        // default to locked
        return StageBtnState.Locked;
    }

    GUIStyle getButtonStyle(StageBtnState state, bool isChallengeStage)
    {
        if (state == StageBtnState.Star_0)
        {
            if (isChallengeStage) return challengeStageBtn_0Star;
            return stageBtn_0Star;
        }
        else if (state == StageBtnState.Star_1)
        {
            if (isChallengeStage) return challengeStageBtn_1Star;
            return stageBtn_1Star;
        }
        else if (state == StageBtnState.Star_2)
        {
            if (isChallengeStage) return challengeStageBtn_2Star;
            return stageBtn_2Star;
        }
        else if (state == StageBtnState.Star_3)
        {
            if (isChallengeStage) return challengeStageBtn_3Star;
            return stageBtn_3Star;
        }
        else
        {
            if (isChallengeStage) return challengeStageBtn_Locked;
            return stageBtn_Locked;
        }
    }

	// Update is called once per frame
    void Update()
    {
        #region thumbnail

        float thumbHeight = contentContainerRect.height * thumbnailScale;
        float thumbWidth = thumbHeight * ((float)storyThumbnail.width / (float)storyThumbnail.height);
        float thumbXOffset = contentContainerRect.width * thumbnailXOffset;
        float thumbYOffset = contentContainerRect.height * thumbnailYOffset;

        thumbnailRect = new Rect(thumbXOffset, thumbYOffset, thumbWidth, thumbHeight);

        #endregion

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPosition[touch.fingerId] = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                var deltaPosition = touch.position - touchStartPosition[touch.fingerId];

                if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y))
                {
                    if (deltaPosition.x < -minDragDistance && 
                        cur_page < max_page && !inTransition)
                    {
                        inTransition = true;
                        nxt_page = cur_page + 1;
                        NextPage();
                    }
                    else if (deltaPosition.x > minDragDistance &&
                             cur_page > 1 && !inTransition)
                    {
                        inTransition = true;
                        nxt_page = cur_page - 1;
                        PrevPage();
                    }
                }
            }
        }
    }
	void OnGUI ()
    {
        // Set the active skin
        GUI.skin = activeSkin;

        // The container
        GUI.BeginGroup(containerRect);
        {
            // Draw the main frame
            NormalStageFrame();
            // Draw the challenge main frame
            ChallengeStageFrame();
            // Draw the buttons
            Buttons();
        }
        GUI.EndGroup();
    }

    #region GUI Sections

    void Buttons()
    {
        // Back button
        if (GUI.Button(backBtnRect, "", backBtnStyle))
        {
            NavigationManager.instance.NavToChapterSelect();
        }
        backBtnHandler.OnMouseOver(backBtnRect);

        // Sound Button
        Game.instance.audio = GUI.Toggle(soundBtnRect, Game.instance.audio, "", soundBtnStyle);
        MainGameController.instance.ToggleSound(Game.instance.audio);
        soundBtnHandler.OnMouseOver(soundBtnRect);

        // Challenge Button 
        //if (challengeStage1State == StageBtnState.Locked)
        if(!Game.instance.challengeChapterUnlocked[chapterNumber-1])
        {
            GUI.Button(challengeBtnRect, "", challengeBtnStyle);
        }
        else
        {
            isChallenge = GUI.Toggle(challengeBtnRect, isChallenge, "", challengeBtnStyle);
            challengeBtnHandler.OnMouseOver(challengeBtnRect);
        }
        // check if it has been toggled
        if (wasChallenge != isChallenge)
        {
            ToggleChallenge(isChallenge);
            wasChallenge = isChallenge;
        }

        // Energy Box
        GUI.Label(energyRect, "99", energyStyle);
        
    }
    void NormalStageFrame()
    {
        GUI.BeginGroup(innerNormalRect);
        {
            // Draw the background
            GUI.DrawTexture(bgRect, bgTexture, ScaleMode.ScaleAndCrop);
            GUI.BeginGroup(contentContainerRect);
            {
                // Background texture
                GUI.DrawTexture(contentContainerBgRect, normalContainerBgTexture);

                // Header
                GUI.Label(contentHeaderRect, "Chapter " + chapterNumber, contentHeaderStyle);

                // Story
                StoryFrame();

                // Stage buttons
                GUI.BeginGroup(stagesContainerRect);
                {
                    DrawStageButton(stage1State, stage1Style, stage1Rect, "1", nStages[0], stage1Handler);
                    DrawStageButton(stage2State, stage2Style, stage2Rect, "2", nStages[1], stage2Handler);
                    DrawStageButton(stage3State, stage3Style, stage3Rect, "3", nStages[2], stage3Handler);
                    DrawStageButton(stage4State, stage4Style, stage4Rect, "4", nStages[3], stage4Handler);
                    DrawStageButton(stage5State, stage5Style, stage5Rect, "5", nStages[4], stage5Handler);
                    DrawStageButton(stage6State, stage6Style, stage6Rect, "6", nStages[5], stage6Handler);
                    DrawStageButton(stage7State, stage7Style, stage7Rect, "7", nStages[6], stage7Handler);
                    DrawStageButton(stage8State, stage8Style, stage8Rect, "8", nStages[7], stage8Handler);
                    DrawStageButton(stage9State, stage9Style, stage9Rect, "9", nStages[8], stage9Handler);
                    DrawStageButton(stage10State, stage10Style, stage10Rect, "10", nStages[9], stage10Handler);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();
        }
        GUI.EndGroup();
    }
    void ChallengeStageFrame()
    {
        GUI.BeginGroup(innerChallengeRect);
        {
            //Draw the background
            GUI.DrawTexture(bgRect, bgChallengeTexture, ScaleMode.ScaleAndCrop);
            GUI.BeginGroup(contentContainerRect);
            {
                // Background texture
                GUI.DrawTexture(contentContainerBgRect, challengeContainerBgTexture);

                // Header
                GUI.Label(challengeHeaderRect, "Chapter " + chapterNumber, challengeHeaderStyle);

                // Stfuu
                GUI.Label(challengeLabelRect, "PREPARE YOUR \nANUS!", challengeLabelStyle);

                // Stage buttons
                GUI.BeginGroup(stagesContainerRect);
                {
                    DrawStageButton(challengeStage1State, challengeStage1Style, stage1Rect, "1", cStages[0], stage1Handler);
                    DrawStageButton(challengeStage2State, challengeStage2Style, stage2Rect, "2", cStages[1], stage2Handler);
                    DrawStageButton(challengeStage3State, challengeStage3Style, stage3Rect, "3", cStages[2], stage3Handler);
                    DrawStageButton(challengeStage4State, challengeStage4Style, stage4Rect, "4", cStages[3], stage4Handler);
                    DrawStageButton(challengeStage5State, challengeStage5Style, stage5Rect, "5", cStages[4], stage5Handler);
                    DrawStageButton(challengeStage6State, challengeStage6Style, stage6Rect, "6", cStages[5], stage6Handler);
                    DrawStageButton(challengeStage7State, challengeStage7Style, stage7Rect, "7", cStages[6], stage7Handler);
                    DrawStageButton(challengeStage8State, challengeStage8Style, stage8Rect, "8", cStages[7], stage8Handler);
                    DrawStageButton(challengeStage9State, challengeStage9Style, stage9Rect, "9", cStages[8], stage9Handler);
                    DrawStageButton(challengeStage10State, challengeStage10Style, stage10Rect, "10", cStages[9], stage10Handler);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();
        }
        GUI.EndGroup();
    }
    void StoryFrame()
    {
        GUI.DrawTexture(thumbnailRect, storyThumbnail);
        GUI.BeginGroup(storyContainerRect);
        {
            GUI.Label(storyInnerActiveRect, story[cur_page - 1]);
            GUI.Label(storyInnerTempRect, story[nxt_page - 1]);
        }
        GUI.EndGroup();

        GUI.color = new Color(0.58f, 0.58f, 0.58f);
        if (cur_page > 1)
        {
            if (GUI.Button(arrowPrevRect, "", arrowPrevStyle))
            {
                if (!inTransition)
                {
                    inTransition = true;
                    nxt_page = cur_page - 1;
                    PrevPage();
                }
            }
        }
        if (cur_page < max_page)
        {
            if (GUI.Button(arrowNextRect, "", arrowNextStyle))
            {
                if (!inTransition)
                {
                    inTransition = true;
                    nxt_page = cur_page + 1;
                    NextPage();
                }
            }
        }
        GUI.color = Color.white;

        // the story nav
        if (max_page > 1)
        {
            GUILayout.BeginArea(storyNavRect);
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Toolbar(cur_page - 1, storyNavStrings, storyNavStyle);
                    //GUI.Toolbar(storyNavRect, cur_page - 1, storyNavStrings, storyNavStyle);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }
    }
    void DrawStageButton(StageBtnState state, GUIStyle btnStyle, Rect btnPos, string btnLabel, string stageName, ButtonHandler handler)
    {
        if (state == StageBtnState.Locked)
        {
            GUI.Button(btnPos, "", btnStyle);
        }
        else
        {
            if (GUI.Button(btnPos, btnLabel, btnStyle))
            {
                if (isChallenge)
                {
                    NavigationManager.instance.chapter = chapterNumber - 1 + 7;
                }
                else
                {
                    NavigationManager.instance.chapter = chapterNumber - 1;
                }
                NavigationManager.instance.stage = System.Convert.ToInt32(btnLabel) - 1;
                Application.LoadLevel(stageName);
            }
            handler.OnMouseOver(btnPos);
        }
    }

    #endregion

    void ToggleChallenge(bool transitionToChallenge)
    {
        if (transitionToChallenge)
        {
            iTween.ValueTo(gameObject,
                           iTween.Hash("from", innerChallengeRect,
                                       "to", innerPosMiddleRect,
                                       "onupdate", "AnimateChallengeRect",
                                       "easetype", iTween.EaseType.easeInQuart,
                                       "time", 0.5f));
            iTween.ValueTo(gameObject,
                           iTween.Hash("from", innerNormalRect,
                                       "to", innerPosRightRect,
                                       "onupdate", "AnimateNormalRect",
                                       "easetype", iTween.EaseType.easeInQuart,
                                       "time", 0.5f));
        }
        else
        {
            iTween.ValueTo(gameObject,
                           iTween.Hash("from", innerChallengeRect,
                                       "to", innerPosLeftRect,
                                       "onupdate", "AnimateChallengeRect",
                                       "easetype", iTween.EaseType.easeInQuart,
                                       "time", 0.5f));
            iTween.ValueTo(gameObject,
                           iTween.Hash("from", innerNormalRect,
                                       "to", innerPosMiddleRect,
                                       "onupdate", "AnimateNormalRect",
                                       "easetype", iTween.EaseType.easeInQuart,
                                       "time", 0.5f));
        }
    }

    void NextPage()
    {
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", storyPosMid,
                                   "to", storyPosLeft,
                                   "onupdate", "AnimateActiveStoryRect",
                                   "easetype", iTween.EaseType.easeOutBack,
                                   "time", 0.5f));

        iTween.ValueTo(gameObject,
                       iTween.Hash("from", storyPosRight,
                                   "to", storyPosMid,
                                   "onupdate", "AnimateTempStoryRect",
                                   "oncomplete", "OnStoryAnimationComplete",
                                   "easetype", iTween.EaseType.easeOutBack,
                                   "time", 0.5f));
    }

    void PrevPage()
    {
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", storyPosMid,
                                   "to", storyPosRight,
                                   "onupdate", "AnimateActiveStoryRect",
                                   "easetype", iTween.EaseType.easeOutBack,
                                   "time", 0.5f));

        iTween.ValueTo(gameObject,
                       iTween.Hash("from", storyPosLeft,
                                   "to", storyPosMid,
                                   "onupdate", "AnimateTempStoryRect",
                                   "oncomplete", "OnStoryAnimationComplete",
                                   "easetype", iTween.EaseType.easeOutBack,
                                   "time", 0.5f));
    }

    void OnStoryAnimationComplete()
    {
        cur_page = nxt_page;
        storyInnerActiveRect = storyPosMid;
        storyInnerTempRect = storyPosLeft;
        inTransition = false;
    }

    #region Animators

    // animates the active story Rect
    void AnimateActiveStoryRect(Rect size)
    {
        storyInnerActiveRect = size;
    }

    // animates the transition story Rect
    void AnimateTempStoryRect(Rect size)
    {
        storyInnerTempRect = size;
    }

    // animates the back button
    void ScaleBackButton(Rect size)
    {
        backBtnRect = size;
    }

    // animates the sound button
    void ScaleSoundButton(Rect size)
    {
        soundBtnRect = size;
    }

    // animate stage 1 button
    void ScaleStage1Button(Rect size)
    {
        stage1Rect = size;

        stage1Style.fontSize = (int)(size.height * stageBtnLabelScale);
        stage1Style.contentOffset = new Vector2(size.width * stageBtnLabelXOffset, size.height * stageBtnLabelYOffset);
    }

    // animate stage 2 button
    void ScaleStage2Button(Rect size)
    {
        stage2Rect = size;

        stage2Style.fontSize = (int)(size.height * stageBtnLabelScale);
        stage2Style.contentOffset = new Vector2(size.width * stageBtnLabelXOffset, size.height * stageBtnLabelYOffset);
    }

    // animate stage 3 button
    void ScaleStage3Button(Rect size)
    {
        stage3Rect = size;

        stage3Style.fontSize = (int)(size.height * stageBtnLabelScale);
        stage3Style.contentOffset = new Vector2(size.width * stageBtnLabelXOffset, size.height * stageBtnLabelYOffset);
    }

    // animate stage 4 button
    void ScaleStage4Button(Rect size)
    {
        stage4Rect = size;

        stage4Style.fontSize = (int)(size.height * stageBtnLabelScale);
        stage4Style.contentOffset = new Vector2(size.width * stageBtnLabelXOffset, size.height * stageBtnLabelYOffset);
    }

    // animate stage 5 button
    void ScaleStage5Button(Rect size)
    {
        stage5Rect = size;

        stage5Style.fontSize = (int)(size.height * stageBtnLabelScale);
        stage5Style.contentOffset = new Vector2(size.width * stageBtnLabelXOffset, size.height * stageBtnLabelYOffset);
    }

    // animate stage 6 button
    void ScaleStage6Button(Rect size)
    {
        stage6Rect = size;

        stage6Style.fontSize = (int)(size.height * stageBtnLabelScale);
        stage6Style.contentOffset = new Vector2(size.width * stageBtnLabelXOffset, size.height * stageBtnLabelYOffset);
    }

    // animate stage 7 button
    void ScaleStage7Button(Rect size)
    {
        stage7Rect = size;

        stage7Style.fontSize = (int)(size.height * stageBtnLabelScale);
        stage7Style.contentOffset = new Vector2(size.width * stageBtnLabelXOffset, size.height * stageBtnLabelYOffset);
    }

    // animate stage 8 button
    void ScaleStage8Button(Rect size)
    {
        stage8Rect = size;

        stage8Style.fontSize = (int)(size.height * stageBtnLabelScale);
        stage8Style.contentOffset = new Vector2(size.width * stageBtnLabelXOffset, size.height * stageBtnLabelYOffset);
    }

    // animate stage 9 button
    void ScaleStage9Button(Rect size)
    {
        stage9Rect = size;

        stage9Style.fontSize = (int)(size.height * stageBtnLabelScale);
        stage9Style.contentOffset = new Vector2(size.width * stageBtnLabelXOffset, size.height * stageBtnLabelYOffset);
    }

    // animate stage 10 button
    void ScaleStage10Button(Rect size)
    {
        stage10Rect = size;

        stage10Style.fontSize = (int)(size.height * stageBtnLabelScale);
        stage10Style.contentOffset = new Vector2(size.width * stageBtnLabelXOffset, size.height * stageBtnLabelYOffset);
    }

    // animate the challenge toggle button
    void ScaleChallengeButton(Rect size)
    {
        challengeBtnRect = size;
    }

    // animate the normal frame
    void AnimateNormalRect(Rect size)
    {
        innerNormalRect = size;
    }

    // animate the challenge frame
    void AnimateChallengeRect(Rect size)
    {
        innerChallengeRect = size;
    }

    #endregion
}
