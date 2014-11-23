using UnityEngine;
using System.Collections;

public class MainGameGUI : MonoBehaviour
{
    public static MainGameGUI instance;

    /* GUI Skin
     * Custom Styles [0] = Capsule Box
     * Custom Styles [1] = NTP Currency Box
     * Custom Styles [2] = Minimap Canvas
     * Custom Styles [3] = Timer Border
     * Custom Styles [4] = Pause Button
     * Custom Styles [5] = Item Slot Empty
     * Custom Styles [6] = Item Slot
     */
    public GUISkin activeSkin;
    private MainGameController mainController;

    private float guiAlpha = 1;
    private float borderAlpha = 0; // transparency
    private float borderTargetAlpha; // Target transparency for the border

    #region Currency boxes variables

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
    #region Consumables

    private float consumablesYOffset = 0.8f;
    private float consumablesScale = 0.17f;

    private Texture itemEmptyTexture;
    private Texture itemFilledTexture;
    private Rect consumable1Rect;
    private Rect consumable2Rect;
    private Rect consumable3Rect;

    private float consumableIconScale = 0.85f;
    private Rect consumableIconRect;
    private Rect consumableButtonRect;

    private float consumableInnerScale = 0.7f;
    private Rect consumableInnerRect;

    #endregion
    void Awake()
    {
        // set the static variable so that other classes can easily use this class
        instance = this;
    }

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

    void Update()
    {
        #region Consumable Items

        itemEmptyTexture = activeSkin.customStyles[5].normal.background;
        itemFilledTexture = activeSkin.customStyles[6].normal.background;

        float conHeight = Screen.height * consumablesScale;
        float conWidth = conHeight * ((float)itemEmptyTexture.width / (float)itemEmptyTexture.height);
        float conXOffset = (Screen.width - 3 * conWidth) * 0.5f;
        float conYOffset = Screen.height * consumablesYOffset;

        consumable1Rect = new Rect(conXOffset, conYOffset, conWidth, conHeight);
        consumable2Rect = new Rect(conXOffset + conWidth, conYOffset, conWidth, conHeight);
        consumable3Rect = new Rect(conXOffset + 2 * conWidth, conYOffset, conWidth, conHeight);

        // Calculate item Icon dimension and offset
        float conIconHeight = conHeight * consumableIconScale;
        float conIconWidth = conIconHeight * (conWidth / conHeight);
        float conIconXOffset = (conWidth - conIconWidth) * 0.5f;
        float conIconYOffset = (conHeight - conIconHeight) * 0.5f;

        // calculate the equipment icon size
        float conInnerHeight = conIconHeight * consumableInnerScale;
        float conInnerWidth = conInnerHeight;
        float conInnerXOffset = (conIconHeight - conInnerHeight) * 0.5f;
        float conInnerYOffset = (conIconWidth - conInnerWidth) * 0.5f;

        consumableButtonRect = new Rect(0, 0, conWidth, conHeight);
        consumableIconRect = new Rect(conIconXOffset, conIconYOffset, conIconWidth, conIconHeight);
        consumableInnerRect = new Rect(conInnerXOffset, conInnerYOffset, conInnerWidth, conInnerHeight);

        #endregion
    }

    // Draw the GUI
    void OnGUI()
    {
        // Sets the GUI depth
        GUI.depth = 20;

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, guiAlpha);

        // Draw the currency boxes.
        NTPScore(currencyBoxWidth, currencyBoxHeight);
        GTPScore(currencyBoxWidth, currencyBoxHeight);
        
        // Draw the Pause Button
        PauseButton();

        // Consumables
        ConsumableItems();

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
        GUI.Label(new Rect(Screen.width - (currencySpacing * width), 
                           height * 0.1f, width, height), 
                           MainGameController.instance.ntp.ToString(), 
                           activeSkin.customStyles[1]);
    }

    void GTPScore(float width, float height)
    {
        GUI.Label(new Rect(Screen.width - ((currencyEdgeSpacing + currencySpacing) * width), 
                           height * 0.1f, width, height), 
                           MainGameController.instance.capsules.Count.ToString(), 
                           activeSkin.customStyles[0]);
    }

    void PauseButton()
    {
        if (GUI.Button(ppButtonPos, "", activeSkin.customStyles[4]))
        {
            mainController.PauseGame();
        }
    }

    void ConsumableItems()
    {
        GUI.BeginGroup(consumable1Rect);
        {
            if (InventoryManager.instance.equippedConsumables[0].itemId != "empty")
            {
                GUI.DrawTexture(consumableIconRect, itemFilledTexture);
                GUI.DrawTexture(consumableInnerRect, InventoryManager.instance.equippedConsumables[0].icon);
                if (GUI.Button(consumableButtonRect, "", activeSkin.button) && 
                    !MainGameController.instance.timerPaused)
                {
                    InventoryManager.instance.ConsumeItem(0);
                }
            }
            else
            {
                GUI.DrawTexture(consumableIconRect, itemEmptyTexture);
            }
        }
        GUI.EndGroup();

        GUI.BeginGroup(consumable2Rect);
        {
            if (InventoryManager.instance.equippedConsumables[1].itemId != "empty")
            {
                GUI.DrawTexture(consumableIconRect, itemFilledTexture);
                GUI.DrawTexture(consumableInnerRect, InventoryManager.instance.equippedConsumables[1].icon);
                if (GUI.Button(consumableIconRect, "", activeSkin.button) &&
                    !MainGameController.instance.timerPaused)
                {
                    InventoryManager.instance.ConsumeItem(1);
                }
            }
            else
            {
                GUI.DrawTexture(consumableIconRect, itemEmptyTexture);
            }
        }
        GUI.EndGroup();

        GUI.BeginGroup(consumable3Rect);
        {
            if (InventoryManager.instance.equippedConsumables[2].itemId != "empty")
            {
                GUI.DrawTexture(consumableIconRect, itemFilledTexture);
                GUI.DrawTexture(consumableInnerRect, InventoryManager.instance.equippedConsumables[2].icon);
                if (GUI.Button(consumableIconRect, "", activeSkin.button) &&
                    !MainGameController.instance.timerPaused)
                {
                    InventoryManager.instance.ConsumeItem(2);
                }
            }
            else
            {
                GUI.DrawTexture(consumableIconRect, itemEmptyTexture);
            }
        }
        GUI.EndGroup();
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
