using UnityEngine;
using System.Collections;

public class FailedGUI : MonoBehaviour 
{
    public static FailedGUI instance;

    /* GUI Skin
     * Custom Styles [0] = Solid BlackBackground
     * Custom Styles [1] = Splattered Poop
     * Custom Styles [2] = Header
     * Custom Styles [3] = Retry Button
     * Custom Styles [4] = Home Button
     * Custom Styles [5] = Next Button
     * Custom Styles [6] = Shopkeeper
     * Custom Styles [7] = Speech Bubble
     * Custom Styles [8] = Item Sold Out
     * Custom Styles [9] = Item Highlight
     * Custom Styles [10] = Dollar Button
     */
    public GUISkin activeSkin;
    private int screen = 0;
    public Texture tempIcon;

    // Triggers
    private float masterAlpha;

    #region GUI related

    private Rect containerRect;  // The Rect object that encapsulates the whole page
    private Rect innerContainerRect;
    private Rect innerTransitionRect;

    private Rect innerContainerPosMid;
    private Rect innerContainerPosLeft;
    private Rect innerContainerPosRight;

    private int transitionTo; // Screen to transition to

    #region Background

    private Rect bgContainerRect; 
    private Rect bgRect; // The Rect object that holds the background of the page
    private Rect bgRectOpen; // The position of the background when open
    private Rect bgRectClose; // The position of the background when closed
    private float bgAlpha; // The transparency of the background
    private Texture poopTexture; // The poop texture

    #endregion

    private float guiAlpha;
    #region header

    private Rect headerRect;
    private float headerYOffset = 0.29f;
    private float headerFontScale = 0.19f;

    #endregion
    #region Navigation Buttons

    private Rect navContainerRect; // Navigation Container for the emergency diapers screen
    private float navContainerYOffset = 0.67f; // Offset for the navigation on the emergency diapers screen
    private float nextBtnYOffset = 0.67f;

    private float navButtonScale = 0.1739365f;
    private float navButtonSpacingScale = -0.09f;

    private Rect retryBtnRect;
    private Rect homeBtnRect;
    private Rect nextBtnRect;

    private GUIStyle retryBtnStyle;
    private GUIStyle homeBtnStyle;
    private GUIStyle nextBtnStyle;

    #endregion
    #region Shopkeeper

    private Texture solidBlackTexture; // solid black background

    private Rect shopkeeperRect; // shopkeeper Rect
    private Texture shopkeeperTexture; // shopkeeper texture
    private float shopkeeperScale = 0.72f;
    private float shopkeeperXOffset = 0.73f;
    private float shopkeeperYOffset = 0.54f;

    #endregion
    #region Speech Bubble

    private Rect bubbleRect;
    private Rect bubbleContainerRect;
    private Rect closedBubbleRect;
    private Rect openBubbleRect;
    private Texture bubbleTexture;

    private float bubbleScale = 0.42f;
    private float bubbleYOffset = 0.21f;
    private string bubbleText;

    private Rect bubbleLabelRect;
    private GUIStyle bubbleLabelStyle;
    private float bubbleLabelXPadding = 0.09f;
    private float bubbleLabelYPadding = 0.11f;
    private float bubbleLabelXScale = 0.82f;
    private float bubbleLabelYScale = 0.7f;
    private float bubbleLabelFontScale = 0.12f;

    #endregion
    #region Items

    private int selected_item = 0; // index of the selected item

    /* ╔═══╗ ╔═══╗ ╔═══╗
     * ║ 1 ║ ║ 2 ║ ║ 3 ║
     * ╚═══╝ ╚═══╝ ╚═══╝
     */

    private Texture soldOutTexture;
    private float itemsContainerXPadding = 0.09f;
    private float itemsContainerYOffset = 0.18f;
    private Rect itemsContainerRect;

    private Rect item1Rect;
    private Rect item2Rect;
    private Rect item3Rect;
    private Item[] slotItems = new Item[3];

    private Rect itemInnerContainerRect;
    private float itemIconScale = 0.9f;
    private Rect itemIconRect;
    private float itemIconYOffset = 0f;

    private Texture highlightTexture;

    private GUIStyle dollarPurchaseBtnStyle;
    private float itemBtnScale = 0.22f;
    private float itemBtnYOffset = 0.68f;
    private float itemBtnLabelXOffset = 0.13f;
    private float itemBtnLabelYOffset = -0.02f;
    private float itemBtnLabelScale = 0.4f;
    private Rect itemBtnRect;

    #endregion

    #endregion

    // audio clips
    public AudioClip fart;

    void Awake()
    {
        // set the static variable so that other classes can easily use this class
        instance = this;
    }

    // Initialise GUI Elements
    void Start()
    {
        // Set the size of the frame
        containerRect = new Rect(0, 0, Screen.width, Screen.height);
        innerContainerPosMid = containerRect;
        innerContainerPosLeft = new Rect(-containerRect.width, 0, containerRect.width, containerRect.height);
        innerContainerPosRight = new Rect(containerRect.width, 0, containerRect.width, containerRect.height);

        innerContainerRect = innerContainerPosMid;
        innerTransitionRect = innerContainerPosRight;

        #region background

        bgRectOpen = new Rect(0, 0, Screen.width, Screen.height);
        bgRectClose = new Rect(0, Screen.height, 0, Screen.height);
        bgContainerRect = bgRectClose;

        // Initialise menu background variables
        bgRect = new Rect(0, 0, Screen.width, Screen.height); ; // Set initial background position to closed
        bgAlpha = 0; // Set initial background to transparency to 0
        poopTexture = activeSkin.customStyles[1].normal.background; // grab the background texture
        solidBlackTexture = activeSkin.customStyles[0].normal.background;

        #endregion
        #region Text and navigation

        guiAlpha = 0; // Transparency of every other GUI stuff

        // header stuff
        activeSkin.customStyles[2].fontSize = (int)(Screen.height * headerFontScale);
        headerRect = new Rect(0, containerRect.height * headerYOffset, containerRect.width, activeSkin.customStyles[2].fontSize);
        
        // navigation buttons
        retryBtnStyle = activeSkin.customStyles[3];
        homeBtnStyle = activeSkin.customStyles[4];
        nextBtnStyle = activeSkin.customStyles[5];

        float navButtonHeight = containerRect.height * navButtonScale;
        float navButtonWidth = navButtonHeight * ((float)retryBtnStyle.normal.background.width /
                                                  (float)retryBtnStyle.normal.background.height);
        float navButtonSpacing = navButtonWidth * navButtonSpacingScale;

        // Next button on first screen
        nextBtnRect = new Rect((Screen.width - navButtonWidth) * 0.5f, 
                               containerRect.height * nextBtnYOffset, 
                               navButtonWidth, 
                               navButtonHeight);

        // Navigation on second screen
        float navWidth = navButtonWidth * 2 + navButtonSpacing * 1;
        navContainerRect = new Rect((containerRect.width - navWidth) / 2, 
                                    containerRect.height * navContainerYOffset, 
                                    navWidth, 
                                    navButtonHeight);

        retryBtnRect = new Rect(0, 0, navButtonWidth, navButtonHeight);
        homeBtnRect = new Rect(navButtonWidth + navButtonSpacing, 0, navButtonWidth, navButtonHeight);
        
        #endregion
        #region Fox Lady

        // Fox Lady
        shopkeeperTexture = activeSkin.customStyles[6].normal.background;
        float shopkeeperHeight = Screen.height * shopkeeperScale;
        float shopkeeperWidth = shopkeeperHeight * ((float)shopkeeperTexture.width / (float)shopkeeperTexture.height);
        shopkeeperRect = new Rect(Screen.width - shopkeeperWidth,
                                    Screen.height * shopkeeperYOffset,
                                    shopkeeperWidth,
                                    shopkeeperHeight);

        #endregion
        #region Speech Bubbles

        // Speech Bubble
        bubbleTexture = activeSkin.customStyles[7].normal.background;
        float bubbleHeight = Screen.height * bubbleScale;
        float bubbleWidth = bubbleHeight * ((float)bubbleTexture.width / (float)bubbleTexture.height);
        bubbleRect = new Rect(0, 0, bubbleWidth, bubbleHeight);

        openBubbleRect = new Rect(Screen.width - bubbleWidth,
                              Screen.height * bubbleYOffset,
                              bubbleWidth,
                              bubbleHeight);
        closedBubbleRect = new Rect(openBubbleRect.x + (openBubbleRect.width * 0.5f), openBubbleRect.y + openBubbleRect.height, 0, 0);
        bubbleContainerRect = closedBubbleRect;

        bubbleLabelStyle = new GUIStyle(activeSkin.label);
        float bblXPadding = bubbleWidth * bubbleLabelXPadding;
        float bblYPadding = bubbleHeight * bubbleLabelYPadding;
        float bblWidth = bubbleWidth * bubbleLabelXScale;
        float bblheight = bubbleHeight * bubbleLabelYScale;
        bubbleLabelRect = new Rect(bblXPadding, bblYPadding, bblWidth, bblheight);
        bubbleLabelStyle.fontSize = (int)(bubbleLabelFontScale * bblheight);

        #endregion
        #region Items

        soldOutTexture = activeSkin.customStyles[8].normal.background;
        highlightTexture = activeSkin.customStyles[9].normal.background;
        dollarPurchaseBtnStyle = activeSkin.customStyles[10];

        // Calculate dimension of the box
        float containerPadding = Screen.width * itemsContainerXPadding;
        float itemBoxDimension = (openBubbleRect.x - containerPadding * 2) / 3;

        // Item purchase buttons
        float itemBtnHeight = itemBoxDimension * itemBtnScale;
        float itemBtnWidth = itemBtnHeight * ((float)dollarPurchaseBtnStyle.normal.background.width /
                                              (float)dollarPurchaseBtnStyle.normal.background.height);

        // Dimension of the container
        float itemsWidth = 3 * itemBoxDimension;
        float itemsHeight = itemBtnHeight + itemBoxDimension;
        float itemsXOffset = containerPadding;
        float itemsYOffset = navContainerRect.y - itemBoxDimension - (Screen.height * itemsContainerYOffset);

        itemsContainerRect = new Rect(itemsXOffset, itemsYOffset, itemsWidth, itemsHeight);

        itemInnerContainerRect = new Rect(0, 0, itemBoxDimension, itemsHeight);
        item1Rect = new Rect(0, 0, itemBoxDimension, itemsHeight);
        item2Rect = new Rect(itemBoxDimension, 0, itemBoxDimension, itemsHeight);
        item3Rect = new Rect(2 * itemBoxDimension, 0, itemBoxDimension, itemsHeight);

        // Calculate item Icon dimensions and offset
        float IconDimension = itemBoxDimension * itemIconScale;
        float IconXOffset = (itemBoxDimension - IconDimension) * 0.5f;
        float IconYOffset = (itemBoxDimension - IconDimension) * 0.5f;
        itemIconRect = new Rect(IconXOffset, IconYOffset, IconDimension, IconDimension);

        float btnXOffset = (itemBoxDimension - itemBtnWidth) * 0.5f;
        float btnYOffset = itemBoxDimension;
        itemBtnRect = new Rect(btnXOffset, btnYOffset, itemBtnWidth, itemBtnHeight);

        int btnLabelFontSize = (int)(itemBtnHeight * itemBtnLabelScale);
        Vector2 btnLabelOffset = new Vector2(itemBtnWidth * itemBtnLabelXOffset, itemBtnHeight * itemBtnLabelYOffset);

        // Initialise empty item objects
        int count = 0;
        while (count < 3)
        {
            slotItems[count] = new Item();
            count++;
        }

        #endregion

        // Default alpha to 1
        masterAlpha = 1;

        // Default to disabled
        this.enabled = false;
	}

    void Update()
    {
        // Update speech bubble
        if (selected_item != 0)
        {
            bubbleText = slotItems[selected_item - 1].description;
        }
    }

	void OnGUI()
    {
        // Sets the GUI depth
        GUI.depth = 10;

        //Set the active skin
        GUI.skin = activeSkin;

        // Set the master transparency
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, masterAlpha);

        // The container
        GUI.BeginGroup(containerRect);
        {
            GUI.BeginGroup(innerContainerRect);
            {
                if (screen == 1)
                {
                    FailedScreen();
                }
                else if (screen == 2)
                {
                    EmergencyDiaperOffer();
                }
            }
            GUI.EndGroup();
            GUI.BeginGroup(innerTransitionRect);
            {
                if (transitionTo == 2)
                {
                    EmergencyDiaperOffer();
                }
            }
            GUI.EndGroup();
        }
        GUI.EndGroup();
    }

    #region GUI sections

    void FailedScreen()
    {
        // Draw the poop
        GUI.BeginGroup(bgContainerRect);
        {
            //GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, bgAlpha);
            GUI.DrawTexture(bgRect, poopTexture, ScaleMode.ScaleAndCrop);
        }
        GUI.EndGroup();

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, guiAlpha);
        // Draw the header
        GUI.Label(headerRect, "CRAP!", activeSkin.customStyles[2]);

        if (GUI.Button(nextBtnRect, "", nextBtnStyle))
        {
            TransitionToNext();
        }
    }

    void EmergencyDiaperOffer()
    {
        // Black Background
        GUI.DrawTexture(innerContainerPosMid, solidBlackTexture, ScaleMode.StretchToFill);

        // Items for sale
        Items();

        // The shopkeeper (and her speech bubbles)
        Shopkeeper();

        #region navigation

        GUI.BeginGroup(navContainerRect);
        {
            if (GUI.Button(retryBtnRect, "", retryBtnStyle))
            {
                NavigationManager.instance.RetryLevel();
            }
            if (GUI.Button(homeBtnRect, "", homeBtnStyle))
            {
                NavigationManager.instance.NavToTitle();
            }
        }
        GUI.EndGroup();

        #endregion
    }

    void Shopkeeper()
    {
        GUI.BeginGroup(bubbleContainerRect);
        {
            GUI.DrawTexture(bubbleRect, bubbleTexture);
            GUI.Label(bubbleLabelRect, bubbleText, bubbleLabelStyle);
        }
        GUI.EndGroup();

        GUI.DrawTexture(shopkeeperRect, shopkeeperTexture);
    }

    void Items()
    {
        GUI.BeginGroup(itemsContainerRect);
        {
            GUI.BeginGroup(item1Rect);
            {
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[0], 1);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item2Rect);
            {
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[1], 2);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item3Rect);
            {
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[2], 3);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();
        }
        GUI.EndGroup();
    }

    void ItemInner(Item item, int index)
    {
        if (item.itemId != "empty")
        {
            if (item.icon != null)
            {
                GUI.DrawTexture(itemIconRect, item.icon);
            }
            else
            {
                GUI.DrawTexture(itemIconRect, tempIcon);
            }

            if (goodButton(itemInnerContainerRect, "", activeSkin.button))
            {
                if (selected_item == index)
                {
                    selected_item = 0;
                    HideBubble();
                }
                else
                {
                    selected_item = index;
                    ShowBubble();
                }
            }
            if (selected_item == index && selected_item != 0)
            {
                GUI.DrawTexture(itemIconRect, highlightTexture);
            }

            if (item.currency == CurrencyType.Dollar)
            {
                if (goodButton(itemBtnRect, item.dollarPrice.ToString(), dollarPurchaseBtnStyle))
                {
                    InventoryManager.instance.PurchaseAndUseEmergencyDiapers(item.itemId);
                }
            }

            if (item.type != ItemType.item_consumable &&
                item.type != ItemType.item_instant &&
                item.balance == 1)
            {
                GUI.DrawTexture(itemInnerContainerRect, soldOutTexture, ScaleMode.ScaleToFit);
            }
        }
        else
        {
            GUI.DrawTexture(itemInnerContainerRect, soldOutTexture, ScaleMode.ScaleToFit);
        }
    }

    #endregion

    #region Animations

    void AnimateBubble(Rect pos)
    {
        bubbleContainerRect = pos;
    }

    void AnimatePoopTransparency(float alpha)
    {
        bgAlpha = alpha;
    }

    void AnimateGUITransparency(float alpha)
    {
        guiAlpha = alpha;
    }

    void AnimateMasterTransparency(float alpha)
    {
        masterAlpha = alpha;
    }
    
    void AnimateFailedMenu(Rect size)
    {
        bgContainerRect = size;
        bgRect.y = -size.y;
    }

    void AnimateTransitionRect(Rect size)
    {
        innerTransitionRect = size;
    }

    void OnTransitionComplete()
    {
        innerContainerRect = innerContainerPosMid;
        innerTransitionRect = innerContainerPosRight;

        screen = transitionTo;
    }

    #endregion

    #region Public Methods
    
    // Show the GUI
    public void Show()
    {
        // Set visible 
        masterAlpha = 1;

        // Initialise variables
        if (MainGameController.instance.failedAttempts == 1)
        {
            slotItems[0] = InventoryManager.instance.itemsOther[CrapTrapAssets.EMERGENCY_REVIVE_1_1_ID];
            slotItems[1] = InventoryManager.instance.itemsOther[CrapTrapAssets.EMERGENCY_REVIVE_2_1_ID];
            slotItems[2] = InventoryManager.instance.itemsOther[CrapTrapAssets.EMERGENCY_REVIVE_3_1_ID];
            bubbleText = "[Failed Once]";
            ShowBubble();
        }
        else if (MainGameController.instance.failedAttempts == 2)
        {
            slotItems[0] = InventoryManager.instance.itemsOther[CrapTrapAssets.EMERGENCY_REVIVE_1_2_ID];
            slotItems[1] = InventoryManager.instance.itemsOther[CrapTrapAssets.EMERGENCY_REVIVE_2_2_ID];
            slotItems[2] = InventoryManager.instance.itemsOther[CrapTrapAssets.EMERGENCY_REVIVE_3_2_ID];
            bubbleText = "[Failed Twice]";
            ShowBubble();
        }
        else if (MainGameController.instance.failedAttempts == 3)
        {
            slotItems[0] = InventoryManager.instance.itemsOther[CrapTrapAssets.EMERGENCY_REVIVE_1_3_ID];
            slotItems[1] = InventoryManager.instance.itemsOther[CrapTrapAssets.EMERGENCY_REVIVE_2_3_ID];
            slotItems[2] = InventoryManager.instance.itemsOther[CrapTrapAssets.EMERGENCY_REVIVE_3_3_ID];
            bubbleText = "[Failed Thrice]";
            ShowBubble();
        }
        else
        {
            slotItems[0] = new Item();
            slotItems[1] = new Item();
            slotItems[2] = new Item();
            bubbleText = "Unfortunately we ran out of diapers. Better luck next time!";
            ShowBubble();
        }


        this.enabled = true;
        screen = 1;
        audio.PlayOneShot(fart, 1f);
        iTween.ValueTo(gameObject,
                          iTween.Hash("from", bgContainerRect,
                                      "to", bgRectOpen,
                                      "onupdate", "AnimateFailedMenu",
                                      "easetype", iTween.EaseType.easeOutCirc,
                                      "time", 0.4f));
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", bgAlpha,
                                   "to", 1,
                                   "onupdate", "AnimatePoopTransparency",
                                   "easetype", iTween.EaseType.easeInQuart,
                                   "time", 0.5f));

        iTween.ValueTo(gameObject,
                       iTween.Hash("from", guiAlpha,
                                   "to", 1,
                                   "onupdate", "AnimateGUITransparency",
                                   "easetype", iTween.EaseType.easeInQuart,
                                   "time", 1f));
    }

    // Hide the GUI
    public void Hide()
    {
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", bgAlpha,
                                   "to", 0,
                                   "onupdate", "AnimateMasterTransparency",
                                   "oncomplete", "DisableSelf",
                                   "easetype", iTween.EaseType.easeInQuart,
                                   "time", 1f));
    }

    // Transition to next slide
    void TransitionToNext()
    {
        if (screen == 1)
        {
            transitionTo = 2;
        }
        
        // Animate the transition
        iTween.ValueTo(gameObject,
                            iTween.Hash("from", innerTransitionRect,
                                        "to", innerContainerPosMid,
                                        "onupdate", "AnimateTransitionRect",
                                        "oncomplete", "OnTransitionComplete",
                                        "easetype", iTween.EaseType.easeOutQuart,
                                        "time", 0.5));
    }

    // Show speech bubble
    void ShowBubble()
    {
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", bubbleContainerRect,
                                   "to", openBubbleRect,
                                   "onupdate", "AnimateBubble",
                                   "easetype", iTween.EaseType.easeOutQuart,
                                   "time", 0.1f));
    }

    // Hide speech bubble
    void HideBubble()
    {
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", bubbleContainerRect,
                                   "to", closedBubbleRect,
                                   "onupdate", "AnimateBubble",
                                   "easetype", iTween.EaseType.easeInQuart,
                                   "time", 0.1f));
    }

    void DisableSelf()
    {
        this.enabled = false;
        bgContainerRect = bgRectClose;
        bgAlpha = 0;
        selected_item = 0;
        masterAlpha = 1;
        MainGameController.instance.EnableTimeNMove();
    }

    #endregion

    #region utility

    // Default unity button bugs when they have one button stacked above another.
    // Hence this function
    bool goodButton(Rect bounds, string caption, GUIStyle btnStyle)
    {
        int controlID = GUIUtility.GetControlID(bounds.GetHashCode(), FocusType.Passive);

        bool isMouseOver = bounds.Contains(Event.current.mousePosition);
        bool isDown = GUIUtility.hotControl == controlID;

        if (GUIUtility.hotControl != 0 && !isDown)
        {
            // ignore mouse while some other control has it
            // (this is the key bit that goodButton appears to be missing)
            isMouseOver = false;
        }

        if (Event.current.type == EventType.Repaint)
        {
            btnStyle.Draw(bounds, new GUIContent(caption), isMouseOver, isDown, false, false);
        }
        switch (Event.current.GetTypeForControl(controlID))
        {
            case EventType.mouseDown:
                if (isMouseOver)
                {  // (note: isMouseOver will be false when another control is hot)
                    GUIUtility.hotControl = controlID;
                }
                break;

            case EventType.mouseUp:
                if (GUIUtility.hotControl == controlID) GUIUtility.hotControl = 0;
                if (isMouseOver && bounds.Contains(Event.current.mousePosition)) return true;
                break;
        }

        return false;
    }

    #endregion
}
