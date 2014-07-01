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

    public float color_alpha = 0; // transparency
    public float alpha_to = 0; // the target alpha value when dissapearing (the border)

    #region Currency boxes variables
    public int ntp = 0; // Keeps track of the number of normal toilet papers collected
    public int gtp = 0; // Keeps track of the number of golden toilet papers collected

    private float currencyFontScale = 0.031f; // Variable used to scale font for different window sizes
    private float currencyXOffset = -0.135f; // Variable used to control the x offset of the text in the box
    private float currencyYOffset = 0.009f; // Variable used to control the y offset of the text in the box
    private float currencySpacing = 1.10f; // Spacing between the two currency boxes;
    private float currencyEdgeSpacing = 0.95f; // Space between the currency box and the edge of the screen.

    private float currencyBoxHeight; // The calculated height of the currency box
    private float currencyBoxWidth; // The calculated width of the currency box
    #endregion

    #region Minimap variables
    private Camera minimap; // the object holding the minimap
    private float minimapXOffset = 24f;
    private float minimapYOffset = 20f;
    private Rect mapCanvas; // the size of the minimap
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

        // set the minimap dimensions and positions.
        minimap = GameObject.FindGameObjectWithTag("Minimap").GetComponent<Camera>();

        mapCanvas = minimap.pixelRect;
        mapCanvas.x = minimapXOffset;
        mapCanvas.y = minimapYOffset + ppButtonTextureDimensions;
        mapCanvas.y = Screen.height - mapCanvas.y - mapCanvas.height;
        minimap.pixelRect = mapCanvas;
        mapCanvas.y = Screen.height - mapCanvas.y - mapCanvas.height;

        // Start the pu;se
        onStartPulse();
    }

    // Draw the GUI
    void OnGUI()
    {
        // Draw the minimap border
        MinimapBorder();

        // Draw the currency boxes.
        NTPScore(currencyBoxWidth, currencyBoxHeight);
        GTPScore(currencyBoxWidth, currencyBoxHeight);
        
        // Draw the Pause Button
        PauseButton();

        // Draw the timer border
        TimerPulseBorder(timerPulseRate);
    }

    void TimerPulseBorder(float pulse_rate)
    {
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, color_alpha);

        GUI.Box(timerBorder, "", activeSkin.customStyles[3]);
    }

    void MinimapBorder()
    {
        if (mainController.mapEnabled)
        {
            GUI.Box(mapCanvas, "", activeSkin.customStyles[2]);
        }
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
        // Texture is smaller than 'pressable' area.  Hence a texture and button
        //GUI.DrawTexture(ppButtonTexturePos, pauseButton);
        if (GUI.Button(ppButtonPos, "", activeSkin.customStyles[4]))
        {
            mainController.PauseGame();
        }
    }

    // animate border pulse
    void AnimateBorder(float alpha)
    {
        color_alpha = alpha;
    }

    void onStartPulse()
    {
        timerPulseRate = (1 - mainController.timeElapsed / mainController.maxTime) * 1.5f;

        if (timerPulseRate < 0.5)
        {
            alpha_to = (0.5f - timerPulseRate) * 1.5f;
            activeSkin.customStyles[3].border = new RectOffset(5, 5, 5, 5);

            //timerPulseRate = 0.45f;
        }
        else
        {
            alpha_to = 0f;
            activeSkin.customStyles[3].border = new RectOffset(10, 10, 10, 10);
        }
        iTween.ValueTo(gameObject, 
                       iTween.Hash("from", color_alpha, 
                                   "to", 1,
                                   "onupdate", "AnimateBorder",
                                   "oncomplete", "onEndPulse",
                                   "easetype", iTween.EaseType.easeInCirc,
                                   "time", timerPulseRate));
    }

    void onEndPulse()
    {
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", color_alpha,
                                   "to", alpha_to,
                                   "onupdate", "AnimateBorder",
                                   "oncomplete", "onStartPulse",
                                   "easetype", iTween.EaseType.easeOutCirc,
                                   "time", timerPulseRate));
    }

}
