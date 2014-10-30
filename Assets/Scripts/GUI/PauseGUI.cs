using UnityEngine;
using System.Collections;

public class PauseGUI : MonoBehaviour 
{
    public static PauseGUI instance;

    /* GUI Skin
     * Custom Styles [0] = Pause Menu Background
     * Custom Styles [1] = Resume Button
     * Custom Styles [2] = Pause Menu
     * Custom Styles [3] = Home Button
     * Custom Styles [4] = Retry Button
     * Custom Styles [5] = Sound Button
     * Custom Styles [6] = Map Button
     * Custom Styles [7] = Label
     */
    public GUISkin activeSkin;

    // Triggers
    public bool show = false;
    public bool hide = false;
    private bool sound = false;
    private bool map = false;

    #region GUI related

    private Rect containerRect;  // The Rect object that contains the whole pause menu
    private Rect openPosition; // Position of the pause menu when game is paused
    private Rect closedPosition; // Position of the pause menu when game is running

    #region Background

    private Rect backgroundRect; // The Rect object that holds the background of the pause menu
    private Texture backgroundTexture; // The texture for the pause menu background

    #endregion
    #region Pause text

    private Rect pauseTextRect;
    private float pauseTxtYOffset = 0.34f;
    private float pauseTxtFontScale = 0.21f;

    #endregion
    #region Pause/Play button

    private GUIStyle ppButtonStyle; // The button style
    private Rect ppButtonPos; // The location of the Play/Pause Button
    private float ppButtonXOffset = 15f;
    private float ppButtonYOffset = 15f;
    private float ppButtonDimensions;  // The height and width of the play/pause button

    #endregion
    #region Pause menu

    Rect menuRect;
    Rect menuContainerRect;
    Texture menuTexture;
    private float menuScale = 1.21f;
    private float menuHeight; // height of the pause menu
    private float menuWidth; // width of the pause menu
    private float menuXOffset = 0f; // Variables used to calculate x offsets for the menu
    private float menuYOffset = 0.09f; // Variables used to calculate y offsets for the menu

    #endregion
    #region Pause menu buttons

    private float btnScale = 0.125f;
    private float btnHeight;
    private float btnWidth;
    /* Button Offsets
     * ---------
     * |       |
     * |   A   |
     * |       |
     * ---------
     * |       |
     * |   B   |
     * |       |
     * ---------
     * |       |
     * |   C   |
     * |       |
     * ---------
     */

    private float A_BtnXOffset = 0.298f; // X offset for the A button
    private float A_BtnYOffset = 0.205f; // Y offset for the A button
    private float B_BtnXOffset = 0.298f; // X offset for the B button
    private float B_BtnYOffset = 0.3385f; // Y offset for the B button
    private float C_BtnXOffset = 0.298f; // X offset for the C button
    private float C_BtnYOffset = 0.47f; // Y offset for the C button
    private Rect A_BtnRect;
    private Rect B_BtnRect;
    private Rect C_BtnRect;
    private ButtonHandler A_BtnScale;
    private ButtonHandler B_BtnScale;
    private ButtonHandler C_BtnScale;

    #endregion

    #endregion

    void Awake()
    {
        // set the static variable so that other classes can easily use this class
        instance = this;
    }

    void Start()
    {
        // Set the pause menu open/closed positions
        openPosition = new Rect(0, 0, Screen.width, Screen.height);
        closedPosition = new Rect(0, Screen.height, Screen.width, Screen.height);

        // Set the pause menu to start as closed.
        containerRect = closedPosition;

        // Initialise menu background variables
        backgroundRect = new Rect(0, 0, Screen.width, Screen.height);
        backgroundTexture = activeSkin.customStyles[0].normal.background;

        // Set the play button texture
        ppButtonStyle = activeSkin.customStyles[1];
        ppButtonDimensions = Screen.height / 8;
        ppButtonPos = new Rect(ppButtonXOffset, ppButtonYOffset, ppButtonDimensions, ppButtonDimensions);

        #region menuContainer

        // Initialise menu variables
        menuTexture = activeSkin.customStyles[2].normal.background;
        menuHeight = Screen.height * menuScale;
        menuWidth = menuHeight * ((float)menuTexture.width / (float)menuTexture.height);

        //menuXOffset = (Screen.width - menuWidth) * 0.5f;
        //menuYOffset = menuHeight * 0.05f;
        menuContainerRect = new Rect(menuXOffset * Screen.width, menuYOffset * Screen.height, menuWidth, menuHeight);
        menuRect = new Rect(0, 0, menuWidth, menuHeight);

        #endregion

        #region menu Buttons

        // Calculate and scale the buttons on the menu
        // Calculate button dimensions (assumes all buttons have same dimensions)
        btnHeight = menuRect.height * btnScale;
        btnWidth = btnHeight * ((float)activeSkin.customStyles[3].normal.background.width /
                                (float)activeSkin.customStyles[3].normal.background.height);

        A_BtnRect = new Rect(menuRect.width * A_BtnXOffset, menuRect.height * A_BtnYOffset, btnWidth, btnHeight);
        B_BtnRect = new Rect(menuRect.width * B_BtnXOffset, menuRect.height * B_BtnYOffset, btnWidth, btnHeight);
        C_BtnRect = new Rect(menuRect.width * C_BtnXOffset, menuRect.height * C_BtnYOffset, btnWidth, btnHeight);

        // Initialise button scalers
        A_BtnScale = new ButtonHandler(A_BtnRect, gameObject, 0.9f, "A_ScaleButton");
        B_BtnScale = new ButtonHandler(B_BtnRect, gameObject, 0.9f, "B_ScaleButton");
        C_BtnScale = new ButtonHandler(C_BtnRect, gameObject, 0.9f, "C_ScaleButton");

        #endregion

        // Scale and position the 'pause' text
        activeSkin.customStyles[7].fontSize = (int)(Screen.height * pauseTxtFontScale);
        pauseTextRect = new Rect(0, Screen.height * pauseTxtYOffset, Screen.width, activeSkin.customStyles[7].fontSize);   
	}

    void Update()
    {
    }

    void OnGUI()
    {
        if (show)
        {
            PauseGame();
            show = false;
        }
        else if (hide)
        {
            ResumeGame();
            hide = false;
        }

        GUI.depth = 1;
        // Set the active skin
        GUI.skin = activeSkin;

        // The container
        GUI.BeginGroup(containerRect);
        // Draw the background
        GUI.DrawTexture(backgroundRect, backgroundTexture, ScaleMode.ScaleAndCrop);
        // Draw the pause menu
        PauseMenu();
        // Draw the resume button
        ResumeButton();
        // Draw the Paused label
        GUI.Label(pauseTextRect, "paused",activeSkin.customStyles[7]);
        
        GUI.EndGroup();

    }

    #region GUI

    void ResumeButton()
    {
        // Texture is smaller than 'pressable' area.  Hence a texture and button
        //GUI.DrawTexture(ppButtonTexturePos, ppButtonStyle.normal.background);
        if (GUI.Button(ppButtonPos, "", ppButtonStyle))
        {
            MainGameController.instance.ResumeGame();
        }
    }

    void PauseMenu()
    {
        GUI.BeginGroup(menuContainerRect);

        GUI.DrawTexture(menuRect, menuTexture);

        // Draw the retry button
        if (GUI.Button(A_BtnRect, "", activeSkin.customStyles[4]))
        {
            NavigationManager.instance.RetryLevel();
        }

        // Draw the home button
        if (GUI.Button(B_BtnRect, "", activeSkin.customStyles[3]))
        {
            NavigationManager.instance.NavToChapterSelect();
        }

        // Draw the sound button
        Game.instance.audio = GUI.Toggle(C_BtnRect, Game.instance.audio, "", activeSkin.customStyles[5]);
        MainGameController.instance.ToggleSound(Game.instance.audio);

        A_BtnScale.OnMouseOver(A_BtnRect);
        B_BtnScale.OnMouseOver(B_BtnRect);
        C_BtnScale.OnMouseOver(C_BtnRect);
        GUI.EndGroup();
    }

    #endregion

    void AnimatePauseMenu(Rect newCoordinates)
    {
        containerRect = newCoordinates;
    }

    public void PauseGame()
    {
        iTween.ValueTo(gameObject, 
                       iTween.Hash("from", containerRect, 
                                   "to", openPosition, 
                                   "onupdate", "AnimatePauseMenu", 
                                   "easetype", iTween.EaseType.easeOutQuart));
        
    }

    public void ResumeGame()
    {
        iTween.ValueTo(gameObject, 
                       iTween.Hash("from", containerRect, 
                                   "to", closedPosition, 
                                   "onupdate", "AnimatePauseMenu", 
                                   "easetype", iTween.EaseType.easeInQuart));
    }

    //applies the values from iTween:
    void A_ScaleButton(Rect size)
    {
        A_BtnRect = size;

    }

    //applies the values from iTween:
    void B_ScaleButton(Rect size)
    {
        B_BtnRect = size;
    }

    //applies the values from iTween:
    void C_ScaleButton(Rect size)
    {
        C_BtnRect = size;

    }
}
