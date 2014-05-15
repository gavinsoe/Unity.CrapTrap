using UnityEngine;
using System.Collections;

public class MainGameGUI : MonoBehaviour
{
    #region Currency boxes variables
    public int ntp = 0; // Keeps track of the number of normal toilet papers collected
    public int gtp = 0; // Keeps track of the number of golden toilet papers collected

    public GUIStyle gtpStyle; // The style for the 'golden toilet paper' currency
    public GUIStyle ntpStyle; // The style for the 'normal toilet paper' currency

    private float currencyFontScale = 0.04f; // Variable used to scale font for different window sizes
    private float currencyXOffset = 0.03f; // Variable used to control the x offset of the text in the box
    private float currencyYOffset = -0.055f; // Variable used to control the y offset of the text in the box

    private float currencyBoxHeight; // The calculated height of the currency box
    private float currencyBoxWidth; // The calculated width of the currency box
    #endregion

    #region Minimap variables
    private Camera minimap; // the object holding the minimap
    private Rect mapCanvas; // the size of the minimap

    public GUIStyle mapCanvasStyle; // Styling for the minimap (border and such)
    #endregion

    #region Timer border variables
    private Rect timerBorder; // Timer border (around edges of screen
    public GUIStyle timerBorderStyle; // The styling for the border
    public float timerPulseRate; // The rate at which the timer border pulses
    #endregion
    
    void Start()
    {
        // set the timer border
        timerBorder = new Rect(0, 0, Screen.width, Screen.height);

        // retrieve the minimap
        minimap = GameObject.FindGameObjectWithTag("Minimap").GetComponent<Camera>();
        mapCanvas = minimap.pixelRect;
        mapCanvas.y = Screen.height - mapCanvas.y - mapCanvas.height;

        // Calculate currency box dimensions.
        currencyBoxHeight = Screen.height / 12;
        currencyBoxWidth = Screen.width / 8;

        ntpStyle.fontSize = (int)(Screen.height * currencyFontScale);
        ntpStyle.contentOffset = new Vector2(currencyBoxWidth * currencyXOffset, currencyBoxHeight * currencyYOffset);

        gtpStyle.fontSize = (int)(Screen.height * currencyFontScale);
        gtpStyle.contentOffset = new Vector2(currencyBoxWidth * currencyXOffset, currencyBoxHeight * currencyYOffset);
    }

    // Draw the GUI
    void OnGUI(){
        // Draw the timer border
        TimerPulseBorder(timerPulseRate);

        // Draw the minimap border
        MinimapBorder();

        // Draw the currency boxes.
        NTPScore(currencyBoxWidth, currencyBoxHeight);
        GTPScore(currencyBoxWidth, currencyBoxHeight);
    }

    void TimerPulseBorder(float pulse_rate)
    {
        float half_pulse_rate = pulse_rate / 2;
        Color color_alpha1 = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1);
        Color color_alpha0 = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 0);

        if (pulse_rate < 0.5)
        {
            timerBorderStyle.border = new RectOffset(5, 5, 5, 5);
        }
        else
        {
            timerBorderStyle.border = new RectOffset(10, 10, 10, 10);
        }

        float timeState = Time.time % pulse_rate;
        if (timeState % pulse_rate < half_pulse_rate)
        {
            GUI.color = Color.Lerp(color_alpha1, color_alpha0, timeState / half_pulse_rate);
            GUI.Box(timerBorder, "", timerBorderStyle);
            //Set the color back for other GUI elements
            GUI.color = color_alpha1;
        }
        else
        {
            GUI.color = Color.Lerp(color_alpha0, color_alpha1, (timeState - half_pulse_rate) / half_pulse_rate);
            GUI.Box(timerBorder, "", timerBorderStyle);
            //Set the color back for other GUI elements
            GUI.color = color_alpha1;
        }
    }

    void MinimapBorder()
    {
        GUI.Box(mapCanvas, "", mapCanvasStyle);
    }

    void NTPScore(float width, float height)
    {
        GUI.Label(new Rect(Screen.width - 2 * width, height/4, width, height), ntp.ToString() , ntpStyle);
    }

    void GTPScore(float width, float height)
    {
        GUI.Label(new Rect(Screen.width - width, height/4, width, height), gtp.ToString(), gtpStyle);
    }
}
