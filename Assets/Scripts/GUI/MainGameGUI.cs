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

    #region Currency boxes variables
    public int ntp = 0; // Keeps track of the number of normal toilet papers collected
    public int gtp = 0; // Keeps track of the number of golden toilet papers collected

    private float currencyFontScale = 0.04f; // Variable used to scale font for different window sizes
    private float currencyXOffset = -0.06f; // Variable used to control the x offset of the text in the box
    private float currencyYOffset = 0.009f; // Variable used to control the y offset of the text in the box

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
        timerBorder = new Rect(timer_margin/2, timer_margin/2, Screen.width - timer_margin, Screen.height - timer_margin);

        // Calculate currency box dimensions.
        currencyBoxHeight = Screen.height / 8;
        currencyBoxWidth = currencyBoxHeight * 2;

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

    }

    // Draw the GUI
    void OnGUI()
    {
        // Draw the timer border
        TimerPulseBorder(timerPulseRate);

        // Draw the minimap border
        MinimapBorder();

        // Draw the currency boxes.
        NTPScore(currencyBoxWidth, currencyBoxHeight);
        GTPScore(currencyBoxWidth, currencyBoxHeight);
        
        // Draw the Pause Button
        PauseButton();
    }

    void TimerPulseBorder(float pulse_rate)
    {
        float half_pulse_rate = pulse_rate / 2;
        Color color_alpha1 = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1);
        Color color_alpha0 = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 0);

        if (pulse_rate < 0.5)
        {
            activeSkin.customStyles[3].border = new RectOffset(5, 5, 5, 5);
        }
        else
        {
            activeSkin.customStyles[3].border = new RectOffset(10, 10, 10, 10);
        }

        float timeState = Time.time % pulse_rate;
        if (timeState % pulse_rate < half_pulse_rate)
        {
            GUI.color = Color.Lerp(color_alpha1, color_alpha0, timeState / half_pulse_rate);
            GUI.Box(timerBorder, "", activeSkin.customStyles[3]);
            //Set the color back for other GUI elements
            GUI.color = color_alpha1;
        }
        else
        {
            GUI.color = Color.Lerp(color_alpha0, color_alpha1, (timeState - half_pulse_rate) / half_pulse_rate);
            GUI.Box(timerBorder, "", activeSkin.customStyles[3]);
            //Set the color back for other GUI elements
            GUI.color = color_alpha1;
        }
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
        GUI.Label(new Rect(Screen.width - (1.15f * width), height * 0.1f, width, height), ntp.ToString(), activeSkin.customStyles[1]);
    }

    void GTPScore(float width, float height)
    {
        GUI.Label(new Rect(Screen.width - 2 * (1.15f * width), height * 0.1f, width, height), gtp.ToString(), activeSkin.customStyles[0]);
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

}
