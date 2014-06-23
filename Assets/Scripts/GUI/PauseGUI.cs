using UnityEngine;
using System.Collections;

public class PauseGUI : MonoBehaviour {
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
    private MainGameController mainController;

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
    private float pauseTxtYOffset = 0.05f;
    private float pauseTxtFontScale = 0.15f;

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
    private float menuScale = 1.448f;
    private float menuHeight; // height of the pause menu
    private float menuWidth; // width of the pause menu
    private float menuXOffset; // Variables used to calculate x offsets for the menu
    private float menuYOffset; // Variables used to calculate y offsets for the menu

    #endregion
    #region Pause menu buttons

    private float btnDimensionPercentage = 0.12f;
    private float btnHeight;
    private float btnWidth;
    /* Button Offsets
     * -----------------
     * |       |       |
     * |   A   |   B   |
     * |       |       |
     * -----------------
     * |       |       |
     * |   C   |   D   |
     * |       |       |
     * -----------------
     */

    private float A_BtnXOffset = 0.312f; // X offset for the A button
    private float A_BtnYOffset = 0.251f; // Y offset for the A button
    private float B_BtnXOffset = 0.525f; // X offset for the B button
    private float B_BtnYOffset = 0.251f; // Y offset for the B button
    private float C_BtnXOffset = 0.312f; // X offset for the C button
    private float C_BtnYOffset = 0.384f; // Y offset for the C button
    private float D_BtnXOffset = 0.525f; // X offset for the D button
    private float D_BtnYOffset = 0.384f; // Y offset for the D button
    private Rect A_BtnRect;
    private Rect B_BtnRect;
    private Rect C_BtnRect;
    private Rect D_BtnRect;
    private ButtonHandler A_BtnScale;
    private ButtonHandler B_BtnScale;
    private ButtonHandler C_BtnScale;
    private ButtonHandler D_BtnScale;

    #endregion

    #endregion

    void Start()
    {
        // Retrieve the main game controller
        mainController = gameObject.GetComponentInChildren<MainGameController>();

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

        // Initialise menu variables
        menuTexture = activeSkin.customStyles[2].normal.background;
        menuHeight = Screen.height * menuScale;
        menuWidth = menuHeight * ((float)menuTexture.width / (float)menuTexture.height);

        menuXOffset = (Screen.width - menuWidth) * 0.5f;
        menuYOffset = menuHeight * 0.05f;
        menuContainerRect = new Rect(menuXOffset, menuYOffset, menuWidth, menuHeight);
        menuRect = new Rect(0, 0, menuWidth, menuHeight);

        // Calculate and scale the buttons on the menu
        // Calculate button dimensions (assumes all buttons have same dimensions)
        btnHeight = menuRect.height * btnDimensionPercentage;
        btnWidth = btnHeight * ((float)activeSkin.customStyles[3].normal.background.width /
                                (float)activeSkin.customStyles[3].normal.background.height);

        A_BtnRect = new Rect(menuRect.width * A_BtnXOffset, menuRect.height * A_BtnYOffset, btnWidth, btnHeight);
        B_BtnRect = new Rect(menuRect.width * B_BtnXOffset, menuRect.height * B_BtnYOffset, btnWidth, btnHeight);
        C_BtnRect = new Rect(menuRect.width * C_BtnXOffset, menuRect.height * C_BtnYOffset, btnWidth, btnHeight);
        D_BtnRect = new Rect(menuRect.width * D_BtnXOffset, menuRect.height * D_BtnYOffset, btnWidth, btnHeight);

        // Initialise button scalers
        A_BtnScale = new ButtonHandler(A_BtnRect, gameObject, 0.9f, "A_ScaleButton");
        B_BtnScale = new ButtonHandler(B_BtnRect, gameObject, 0.9f, "B_ScaleButton");
        C_BtnScale = new ButtonHandler(C_BtnRect, gameObject, 0.9f, "C_ScaleButton");
        D_BtnScale = new ButtonHandler(D_BtnRect, gameObject, 0.9f, "D_ScaleButton");

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
            iTween.ValueTo(gameObject, iTween.Hash("from", containerRect, "to", openPosition, "onupdate", "AnimatePauseMenu", "easetype", iTween.EaseType.easeOutQuart));
            show = false;
        }
        else if (hide)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", containerRect, "to", closedPosition, "onupdate", "AnimatePauseMenu", "easetype", iTween.EaseType.easeInQuart));
            hide = false;
        }

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
            mainController.ResumeGame();
        }
    }

    void PauseMenu()
    {
        GUI.BeginGroup(menuContainerRect);

        GUI.DrawTexture(menuRect, menuTexture);

        // Draw the retry button
        if (GUI.Button(A_BtnRect, "", activeSkin.customStyles[4]))
        {
            mainController.RetryLevel();
        }

        // Draw the home button
        if (GUI.Button(B_BtnRect, "", activeSkin.customStyles[3]))
        {
            mainController.ReturnToTitle();
        }

        // Draw the sound button
        sound = GUI.Toggle(C_BtnRect, sound, "", activeSkin.customStyles[5]);
        mainController.ToggleSound(!sound);

        // Draw the retry button
        map = GUI.Toggle(D_BtnRect, map, "", activeSkin.customStyles[6]);
        mainController.ToggleMap(!map);

        A_BtnScale.OnMouseOver(A_BtnRect);
        B_BtnScale.OnMouseOver(B_BtnRect);
        C_BtnScale.OnMouseOver(C_BtnRect);
        D_BtnScale.OnMouseOver(D_BtnRect);
        GUI.EndGroup();
    }

    #endregion

    void AnimatePauseMenu(Rect newCoordinates)
    {
        containerRect = newCoordinates;
    }

    public void PauseGame()
    {
        show = true;
    }

    public void ResumeGame()
    {
        hide = true;
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

    //applies the values from iTween:
    void D_ScaleButton(Rect size)
    {
        D_BtnRect = size;
    }
}
