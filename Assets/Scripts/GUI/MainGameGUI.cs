using UnityEngine;
using System.Collections;

public class MainGameGUI : MonoBehaviour
{
    /* GUI Skin
     * Custom Styles [0] = GTP Currency Box
     * Custom Styles [1] = NTP Currency Box
     * Custom Styles [2] = Minimap Canvas
     * Custom Styles [3] = Timer Border
     * Custom Styles [4] = Pause Button
     */
    public GUISkin activeSkin;
    private MainGameController mainController;

    private float guiAlpha = 1;
    private float borderAlpha = 0; // transparency
    private float borderTargetAlpha; // Target transparency for the border

    #region Currency boxes variables
    public int ntp = 0; // Keeps track of the number of ntp
    public int gtp = 0; // Keeps track of the number of gtp

    private float currencyFontScale = 0.031f; // Variable used to scale font for different window sizes
    private float currencyXOffset = -0.135f; // Variable used to control the x offset of the text in the box
    private float currencyYOffset = 0.009f; // Variable used to control the y offset of the text in the box
    private float currencySpacing = 1.10f; // Spacing between the two currency boxes;
    private float currencyEdgeSpacing = 0.95f; // Space between the currency box and the edge of the screen.

    private float currencyBoxHeight; // The calculated height of the currency box
    private float currencyBoxWidth; // The calculated width of the currency box
    #endregion

    #region Timer border variables
    private Rect timerBorder; // Timer border (around edges of screen
    public float timerPulseRate; // The rate at which the timer border pulses
    #endregion

    #region Pause/Play button variables
    public Texture pauseButton;
    private Rect ppButtonPos; // The location of the Play/Pause Button
    public float ppButtonXOffset = 15f;
    public float ppButtonYOffset = 15f;
    public float ppButtonTextureDimensions;  // The height and width of the play/pause button
    public float ppButtonDimensions;  // The height and width of the play/pause button
    #endregion

    void Start()
    {
        // Retrieve the main game controller
        mainController = gameObject.GetComponentInChildren<MainGameController>();

        // set the timer border
        var timer_margin = Screen.height * 0.02f;
        timerBorder = new Rect(timer_margin / 2, timer_margin / 2, Screen.width - timer_margin, Screen.height - timer_margin);

        // Calculate currency box dimensions.
        Texture currencyTexture = activeSkin.customStyles[0].normal.background;
        currencyBoxHeight = Screen.height / 8;
        currencyBoxWidth = currencyBoxHeight * ((float)currencyTexture.width / (float)currencyTexture.height);

        // Font auto scaling
        activeSkin.customStyles[0].fontSize = (int)(Screen.height * currencyFontScale);
        activeSkin.customStyles[0].contentOffset = new Vector2(currencyBoxWidth * currencyXOffset, currencyBoxHeight * currencyYOffset);

        activeSkin.customStyles[1].fontSize = (int)(Screen.height * currencyFontScale);
        activeSkin.customStyles[1].contentOffset = new Vector2(currencyBoxWidth * currencyXOffset, currencyBoxHeight * currencyYOffset);

        // Play and pause button position and dimension
        ppButtonDimensions = Screen.height / 8;
        ppButtonTextureDimensions = ppButtonDimensions * 0.625f;
        ppButtonPos = new Rect(ppButtonXOffset, ppButtonYOffset, ppButtonDimensions, ppButtonDimensions);
        
        // Start the pu;se
        onStartPulse();
    }

    // Draw the GUI
    void OnGUI()
    {
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, guiAlpha);

        // Draw the currency boxes.
        NTPScore(currencyBoxWidth, currencyBoxHeight);
        GTPScore(currencyBoxWidth, currencyBoxHeight);
        
        // Draw the Pause Button
        PauseButton();

        // Draw the timer border
        TimerPulseBorder(timerPulseRate);
    }

    #region GUI components
    void TimerPulseBorder(float pulse_rate)
    {
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, borderAlpha);

        GUI.Box(timerBorder, "", activeSkin.customStyles[3]);
    }

    void NTPScore(float width, float height)
    {
        GUI.Label(new Rect(Screen.width - (currencySpacing * width), height * 0.1f, width, height), ntp.ToString(), activeSkin.customStyles[1]);
    }

    void GTPScore(float width, float height)
    {
        GUI.Label(new Rect(Screen.width - ((currencyEdgeSpacing + currencySpacing) * width), height * 0.1f, width, height), gtp.ToString(), activeSkin.customStyles[0]);
    }

    void PauseButton()
    {
        if (GUI.Button(ppButtonPos, "", activeSkin.customStyles[4]))
        {
            mainController.PauseGame();
        }
    }
    #endregion

    #region border pulse animation
    void AnimateBorder(float alpha)
    {
        borderAlpha = alpha;
    }

    void onStartPulse()
    {
        timerPulseRate = (1 - mainController.timeElapsed / mainController.maxTime) * 1.5f;

        if (timerPulseRate < 0.5)
        {
            borderTargetAlpha = (0.5f - timerPulseRate) * 1.5f;
            activeSkin.customStyles[3].border = new RectOffset(5, 5, 5, 5);

            //timerPulseRate = 0.45f;
        }
        else
        {
            borderTargetAlpha = 0f;
            activeSkin.customStyles[3].border = new RectOffset(10, 10, 10, 10);
        }
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", borderAlpha, 
                                   "to", 1,
                                   "onupdate", "AnimateBorder",
                                   "oncomplete", "onEndPulse",
                                   "easetype", iTween.EaseType.easeInCirc,
                                   "time", timerPulseRate));
    }

    void onEndPulse()
    {
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", borderAlpha,
                                   "to", borderTargetAlpha,
                                   "onupdate", "AnimateBorder",
                                   "oncomplete", "onStartPulse",
                                   "easetype", iTween.EaseType.easeOutCirc,
                                   "time", timerPulseRate));
    }
    #endregion

    #region show and/or hide the gui

    void FadeOutGUI(float alpha)
    {
        guiAlpha = alpha;
    }
    // Hide the menu
    public void Hide()
    {
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", guiAlpha,
                                   "to", 0,
                                   "onupdate", "FadeOutGUI",
                                   "easetype", iTween.EaseType.easeInQuart,
                                   "time", 0.5f));
    }
    #endregion
}
