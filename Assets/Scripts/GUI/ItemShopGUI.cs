using Soomla;
using Soomla.Store;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ItemShopGUI : MonoBehaviour
{
    /* GUI Skin
     * Custom Styles [0] = Background
     * Custom Styles [1] = Shopkeeper
     * Custom Styles [2] = Back Button
     * Custom Styles [3] = Head Button
     * Custom Styles [4] = Upper Body Button
     * Custom Styles [5] = Lower Body Button
     * Custom Styles [6] = Item Button
     * Custom Styles [7] = GTP
     * Custom Styles [8] = NTP
     * Custom Styles [9] = Item Container
     * Custom Styles [10] = Arrow Next
     * Custom Styles [11] = Arrow Prev
     * Custom Styles [12] = NTP Purchase Button
     * Custom Styles [13] = GTP Purchase Button
     * Custom Styles [14] = $ Purchase Button
     * Custom Styles [15] = Sold out
     * Custom Styles [16] = Item Highlight
     * Custom Styles [17] = Speech Bubble
     * Custom Styles [18] = Popup box
     * Custom Styles [19] = Cancel Button
     * Custom Styles [20] = Shading base
     */
    public GUISkin activeSkin;
    public Texture tempIcon;

    private ItemType activeWindow; // the active item shop window
    private int cur_page; // active page number
    private int max_page; //number of pages
    private int selected_item = 0; // index of the selected item
    private bool inTransition = false; // whether the page is transitioning
    private bool initialized = false;
    private bool show_popup = false;

    private Item[] slotItems = new Item[9];
    private Item[] transitionItems = new Item[9];
    private Item itemSlot1;
    private Item itemSlot2;
    private Item itemSlot3;
    private Item itemSlot4;
    private Item itemSlot5;
    private Item itemSlot6;
    private Item itemSlot7;
    private Item itemSlot8;
    private Item itemSlot9;

    private Item itemSlotTransition1;
    private Item itemSlotTransition2;
    private Item itemSlotTransition3;
    private Item itemSlotTransition4;
    private Item itemSlotTransition5;
    private Item itemSlotTransition6;
    private Item itemSlotTransition7;
    private Item itemSlotTransition8;
    private Item itemSlotTransition9;

    #region GUI Related

    private Rect containerRect; // The Rect object that encapsulates the whole page

    #region background

    private Rect bgRect; // background Rect
    private Texture bgTexture; // The background texture

    private Rect shopkeeperRect; // shopkeeper Rect
    private Texture shopkeeperTexture; // shopkeeper texture
    private float shopkeeperScale = 0.72f;
    private float shopkeeperXOffset = 0.73f;
    private float shopkeeperYOffset = 0.54f;

    #endregion
    #region navigation

    private Rect navContainerRect; // Navigation container

    private Rect btnBackRect;
    private GUIStyle btnBackStyle;

    private Rect btnHeadRect;
    private Texture2D btnHeadInactiveTexture;
    private Texture2D btnHeadActiveTexture;
    private GUIStyle btnHeadStyle;

    private Rect btnBodyRect;
    private Texture2D btnBodyInactiveTexture;
    private Texture2D btnBodyActiveTexture;
    private GUIStyle btnBodyStyle;

    private Rect btnLegsBodyRect;
    private Texture2D btnLegsInactiveTexture;
    private Texture2D btnLegsActiveTexture;
    private GUIStyle btnLegsStyle;

    private Rect btnItemRect;
    private Texture2D btnItemInactiveTexture;
    private Texture2D btnItemActiveTexture;
    private GUIStyle btnItemStyle;

    #endregion
    #region currency boxes

    private Rect currencyNTPRect;
    private Rect currencyGTPRect;
    private GUIStyle NTPStyle;
    private GUIStyle GTPStyle;

    private float currencyBoxScale = 0.125f; // Variable used to scale the box for different window sizes
    private float currencyFontScale = 0.031f; // Variable used to scale font for different window sizes
    private float currencyXOffset = -0.135f; // Variable used to control the x offset of the text in the box
    private float currencyYOffset = 0.009f; // Variable used to control the y offset of the text in the box
    private float currencySpacing = 1.10f; // Spacing between the two currency boxes;
    private float currencyEdgeSpacing = 0.95f; // Space between the currency box and the edge of the screen.

    #endregion
    #region Items

    /* ╔═══╗ ╔═══╗ ╔═══╗
     * ║ 1 ║ ║ 2 ║ ║ 3 ║
     * ╚═══╝ ╚═══╝ ╚═══╝
     * ╔═══╗ ╔═══╗ ╔═══╗
     * ║ 4 ║ ║ 5 ║ ║ 6 ║
     * ╚═══╝ ╚═══╝ ╚═══╝
     * ╔═══╗ ╔═══╗ ╔═══╗
     * ║ 7 ║ ║ 8 ║ ║ 9 ║
     * ╚═══╝ ╚═══╝ ╚═══╝
     */
    private Texture itemBoxTexture;
    private Texture soldOutTexture;
    private Rect itemsContainerRect;
    private Rect item1Rect;
    private Rect item2Rect;
    private Rect item3Rect;
    private Rect item4Rect;
    private Rect item5Rect;
    private Rect item6Rect;
    private Rect item7Rect;
    private Rect item8Rect;
    private Rect item9Rect;
    private Rect itemBgRect;

    private Rect itemInnerContainerRect;
    private Rect itemTransitionContainerRect;
    private Rect itemPosCenter;
    private Rect itemPosLeft;
    private Rect itemPosRight;
    private Rect itemPosTop;
    private Rect itemPosBottom;

    private float itemIconScale = 0.5f;
    private Rect itemIconRect;
    private float itemIconYOffset = -0.1f;

    private Texture highlightTexture;

    private GUIStyle ntpPurchaseBtnStyle;
    private GUIStyle gtpPurchaseBtnStyle;
    private GUIStyle dollarPurchaseBtnStyle;
    private float itemBtnScale = 0.22f;
    private float itemBtnYOffset = 0.68f;
    private float itemBtnLabelXOffset = 0.13f;
    private float itemBtnLabelYOffset = -0.02f;
    private float itemBtnLabelScale = 0.4f;
    private Rect itemBtnRect;

    private GUIStyle arrowNextStyle;
    private Rect arrowNextRect;
    private GUIStyle arrowPrevStyle;
    private Rect arrowPrevRect;
    private float arrowScale = 0.08f;
    
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
    #region Popup Confirmation

    private Rect popupRect;
    private Rect popupBgRect;
    private Rect popupPictureRect;
    private Rect popupLabelRect;
    private Rect popupCancelButtonRect;
    private Rect popupConfirmButtonRect;

    private GUIStyle shadingStyle;
    private GUIStyle popupBgStyle;
    private GUIStyle popupCancelStyle;
    private float popupXScale = 0.9f; // ratio of the popup box, based on the 'shelves' (3x3 boxes)
    private float popupRatio = 0.4f; // y:x ratio 2:5
    private float popupPadding = 20f;

    private float popupConfirmBtnScale = 0.25f;

    private float popupCancelBtnScale = 0.28f;
    private float popupCancelXOffset = 0.7f;
    private float popupCancelYOffset = 0.27f;

    private GUIStyle popupLabelStyle;
    private float popupLabelScale;
    #endregion
    #endregion
    #region Touch Controls

    private int maxTouches = 1;	// up to 5 (iOS only supports 5 apparently)
    private float minDragDistance = 50f; // Swipe distance before touch is regarded as 'touch and drag'

    private Vector2[] touchStartPosition;

    #endregion

    // Use this for initialization
    void Start()
    {
        #region Touch Controls

        // inititialise the arrays used for manipulating the touch controls
        touchStartPosition = new Vector2[maxTouches];

        #endregion

        // Initialize soomla store
        SoomlaStore.Initialize(new CrapTrapAssets());

        // Set the container rect
        containerRect = new Rect(0, 0, Screen.width, Screen.height);

        #region background

        // Background texture
        bgRect = new Rect(0, 0, Screen.width, Screen.height);
        bgTexture = activeSkin.customStyles[0].normal.background;

        // Fox Lady
        shopkeeperTexture = activeSkin.customStyles[1].normal.background;
        float shopkeeperHeight = Screen.height * shopkeeperScale;
        float shopkeeperWidth = shopkeeperHeight * ((float)shopkeeperTexture.width / (float)shopkeeperTexture.height);
        shopkeeperRect = new Rect(Screen.width - shopkeeperWidth,
                                    Screen.height * shopkeeperYOffset,
                                    shopkeeperWidth,
                                    shopkeeperHeight);

        #endregion
        #region navigation

        btnBackStyle = activeSkin.customStyles[2];

        btnHeadActiveTexture = activeSkin.customStyles[3].onNormal.background;
        btnHeadInactiveTexture = activeSkin.customStyles[3].normal.background;
        btnHeadStyle = new GUIStyle(activeSkin.customStyles[3]);

        btnBodyActiveTexture = activeSkin.customStyles[4].onNormal.background;
        btnBodyInactiveTexture = activeSkin.customStyles[4].normal.background;
        btnBodyStyle = new GUIStyle(activeSkin.customStyles[4]);

        btnLegsActiveTexture = activeSkin.customStyles[5].onNormal.background;
        btnLegsInactiveTexture = activeSkin.customStyles[5].normal.background;
        btnLegsStyle = new GUIStyle(activeSkin.customStyles[5]);

        btnItemActiveTexture = activeSkin.customStyles[6].onNormal.background;
        btnItemInactiveTexture = activeSkin.customStyles[6].normal.background;
        btnItemStyle = new GUIStyle(activeSkin.customStyles[6]);

        float btnHeight = Screen.height / 7;
        Texture backBtnTexture = btnBackStyle.normal.background;
        float backBtnWidth = btnHeight * ((float)backBtnTexture.width / (float)backBtnTexture.height);
        // Assumes all equipment buttons are of the same size
        Texture eqBtnTexture = btnHeadStyle.normal.background;
        float eqBtnWidth = btnHeight * ((float)eqBtnTexture.width / (float)eqBtnTexture.height);

        float navContainerWidth = Mathf.Max(backBtnWidth, eqBtnWidth);
        navContainerRect = new Rect(0, 0, navContainerWidth, Screen.height);
        btnBackRect = new Rect(0, 0, backBtnWidth, btnHeight);
        btnHeadRect = new Rect(0.5f * (navContainerWidth - eqBtnWidth), btnHeight, eqBtnWidth, btnHeight);
        btnBodyRect = new Rect(0.5f * (navContainerWidth - eqBtnWidth), btnHeight * 2, eqBtnWidth, btnHeight);
        btnLegsBodyRect = new Rect(0.5f * (navContainerWidth - eqBtnWidth), btnHeight * 3, eqBtnWidth, btnHeight);
        btnItemRect = new Rect(0.5f * (navContainerWidth - eqBtnWidth), btnHeight * 4, eqBtnWidth, btnHeight);

        #endregion
        #region currency boxes

        NTPStyle = new GUIStyle(activeSkin.customStyles[8]);
        GTPStyle = new GUIStyle(activeSkin.customStyles[7]);
        Texture currencyTexture = NTPStyle.normal.background;
        float currencyBoxHeight = Screen.height * currencyBoxScale;
        float currencyBoxWidth = currencyBoxHeight * ((float)currencyTexture.width / (float)currencyTexture.height);

        // Font Scaling
        NTPStyle.fontSize = (int)(Screen.height * currencyFontScale);
        NTPStyle.contentOffset = new Vector2(currencyBoxWidth * currencyXOffset, currencyBoxHeight * currencyYOffset);
        GTPStyle.fontSize = (int)(Screen.height * currencyFontScale);
        GTPStyle.contentOffset = new Vector2(currencyBoxWidth * currencyXOffset, currencyBoxHeight * currencyYOffset);

        // Container Rect
        currencyNTPRect = new Rect(Screen.width - (currencySpacing * currencyBoxWidth), currencyBoxHeight * 0.1f, currencyBoxWidth, currencyBoxHeight);
        currencyGTPRect = new Rect(Screen.width - ((currencyEdgeSpacing + currencySpacing) * currencyBoxWidth), currencyBoxHeight * 0.1f, currencyBoxWidth, currencyBoxHeight);

        #endregion
        #region item container

        itemBoxTexture = activeSkin.customStyles[9].normal.background;
        arrowNextStyle = activeSkin.customStyles[10];
        arrowPrevStyle = activeSkin.customStyles[11];
        soldOutTexture = activeSkin.customStyles[15].normal.background;

        Texture arrowTexture = arrowNextStyle.normal.background;
        float arrowHeight = Screen.height * arrowScale;
        float arrowWidth = arrowHeight * ((float)arrowTexture.width / (float)arrowTexture.height);

        float itemsContainerWidth = currencyGTPRect.x - navContainerRect.width - 2 * arrowWidth;
        float itemBoxWidth = itemsContainerWidth / 3;
        float itemBoxHeight = itemBoxWidth * ((float)itemBoxTexture.height / (float)itemBoxTexture.width);
        float itemsContainerHeight = itemBoxHeight * 3;

        // Make sure it doesn't exceed screen height
        if (Screen.height < itemsContainerHeight)
        {
            itemsContainerHeight = Screen.height;
            itemBoxHeight = itemsContainerHeight / 3;
            itemBoxWidth = itemBoxHeight * ((float)itemBoxTexture.width / (float)itemBoxTexture.height);
            itemsContainerWidth = itemBoxWidth * 3;
        }

        // Calculate item Icon dimensions and offset
        float IconDimension = itemBoxHeight * itemIconScale;
        float IconXOffset = (itemBoxWidth - IconDimension) * 0.5f;
        float IconYOffset = (itemBoxHeight - IconDimension) * 0.5f + itemBoxHeight * itemIconYOffset;

        // Get the texture for the highlighted item
        highlightTexture = activeSkin.customStyles[16].normal.background;

        // Calculate button location and dimension
        ntpPurchaseBtnStyle = activeSkin.customStyles[12];
        gtpPurchaseBtnStyle = activeSkin.customStyles[13];
        dollarPurchaseBtnStyle = activeSkin.customStyles[14];

        float itemBtnHeight = itemBoxHeight * itemBtnScale;
        float itemBtnWidth = itemBtnHeight * ((float)ntpPurchaseBtnStyle.normal.background.width /
                                              (float)ntpPurchaseBtnStyle.normal.background.height);
        float btnXOffset = (itemBoxWidth - itemBtnWidth) * 0.5f;
        float btnYOffset = itemBoxHeight * itemBtnYOffset;
        itemBtnRect = new Rect(btnXOffset, btnYOffset, itemBtnWidth, itemBtnHeight);

        int btnLabelFontSize = (int)(itemBtnHeight * itemBtnLabelScale);
        Vector2 btnLabelOffset = new Vector2(itemBtnWidth * itemBtnLabelXOffset, itemBtnHeight * itemBtnLabelYOffset);
        ntpPurchaseBtnStyle.contentOffset = btnLabelOffset;
        ntpPurchaseBtnStyle.fontSize = btnLabelFontSize;
        gtpPurchaseBtnStyle.contentOffset = btnLabelOffset;
        gtpPurchaseBtnStyle.fontSize = btnLabelFontSize;
        dollarPurchaseBtnStyle.contentOffset = btnLabelOffset;
        dollarPurchaseBtnStyle.fontSize = btnLabelFontSize;

        // Initialise the containers
        itemsContainerRect = new Rect(navContainerRect.width + arrowWidth, 0.5f * (Screen.height - itemsContainerHeight), itemsContainerWidth, itemsContainerHeight);
        itemBgRect = new Rect(0, 0, itemBoxWidth, itemBoxHeight);
        itemIconRect = new Rect(IconXOffset, IconYOffset, IconDimension, IconDimension);
        item1Rect = new Rect(0, 0, itemBoxWidth, itemBoxHeight);
        item2Rect = new Rect(itemBoxWidth, 0, itemBoxWidth, itemBoxHeight);
        item3Rect = new Rect(2 * itemBoxWidth, 0, itemBoxWidth, itemBoxHeight);
        item4Rect = new Rect(0, itemBoxHeight, itemBoxWidth, itemBoxHeight);
        item5Rect = new Rect(itemBoxWidth, itemBoxHeight, itemBoxWidth, itemBoxHeight);
        item6Rect = new Rect(2 * itemBoxWidth, itemBoxHeight, itemBoxWidth, itemBoxHeight);
        item7Rect = new Rect(0, 2 * itemBoxHeight, itemBoxWidth, itemBoxHeight);
        item8Rect = new Rect(itemBoxWidth, 2 * itemBoxHeight, itemBoxWidth, itemBoxHeight);
        item9Rect = new Rect(2 * itemBoxWidth, 2 * itemBoxHeight, itemBoxWidth, itemBoxHeight);
        arrowNextRect = new Rect(itemsContainerRect.x + itemsContainerRect.width, 0.5f * (Screen.height - arrowHeight), arrowWidth, arrowHeight);
        arrowPrevRect = new Rect(itemsContainerRect.x - arrowWidth, 0.5f * (Screen.height - arrowHeight), arrowWidth, arrowHeight);

        itemPosCenter = itemBgRect;
        itemPosLeft = new Rect(-itemBgRect.width, 0, itemBgRect.width, itemBgRect.height);
        itemPosRight = new Rect(itemBgRect.width, 0, itemBgRect.width, itemBgRect.height);
        itemPosTop = new Rect(0, -itemBgRect.height, itemBgRect.width, itemBgRect.height);
        itemPosBottom = new Rect(0, itemBgRect.height, itemBgRect.width, itemBgRect.height);
        itemInnerContainerRect = itemPosCenter;
        itemTransitionContainerRect = itemPosLeft;

        // Initialise empty item objects
        int count = 0;
        while (count < 9)
        {
            slotItems[count] = new Item();
            transitionItems[count] = new Item();
            count++;
        }
        #endregion
        #region speech bubble

        // Speech Bubble
        bubbleTexture = activeSkin.customStyles[17].normal.background;
        float bubbleHeight = Screen.height * bubbleScale;
        float bubbleWidth = bubbleHeight * ((float)bubbleTexture.width / (float)bubbleTexture.height);
        bubbleRect = new Rect(0,0,bubbleWidth,bubbleHeight);

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
        #region popup confirmation

        // Shading base
        shadingStyle = activeSkin.customStyles[20];

        // Container
        float popupXDimension = itemsContainerRect.width * popupXScale;
        float popupYDimension = popupXDimension * popupRatio;
        float popupXOffset = itemsContainerRect.x + (itemsContainerRect.width - popupXDimension) * 0.5f;
        float popupYOffset = itemsContainerRect.y + (itemsContainerRect.height - popupYDimension) * 0.5f;
        popupBgStyle = activeSkin.customStyles[18];

        // Preview Pic
        float popupPicDimension = popupYDimension - 2 * popupPadding;
        float popupPicXOffset = popupPadding;
        float popupPicYOffset = popupPadding;

        // Cancel button
        popupCancelStyle = activeSkin.customStyles[19];
        float cancelBtnHeight = popupYDimension * popupCancelBtnScale;
        float cancelBtnWidth = cancelBtnHeight * ((float)popupCancelStyle.normal.background.width /
                                                  (float)popupCancelStyle.normal.background.height);
        float cancelBtnXOffset = popupXDimension + popupXOffset - (cancelBtnWidth * popupCancelXOffset);
        float cancelBtnYOffset = popupYOffset - (cancelBtnHeight * popupCancelYOffset);

        // Confirm Button
        float confirmBtnHeight = popupYDimension * popupConfirmBtnScale;
        float confirmBtnWidth = confirmBtnHeight * ((float)ntpPurchaseBtnStyle.normal.background.width /
                                                    (float)ntpPurchaseBtnStyle.normal.background.height);
        float confirmBtnXOffset = (popupXDimension - popupPicDimension - confirmBtnWidth - 2 * popupPadding) * 0.5f + popupPicDimension + popupPadding;
        float confirmBtnYOffset = (popupYDimension - confirmBtnHeight - popupPadding);

        // Label
        popupLabelStyle = new GUIStyle(activeSkin.label);
        popupLabelStyle.fontSize = (int)(popupLabelScale * popupYDimension);

        float popupLabelHeight = confirmBtnYOffset;
        float popupLabelWidth = popupXDimension - popupPicDimension - popupPadding;
        float popupLabelXOffset = popupPicDimension + popupPadding;
        float popupLabelYOffset = popupPadding;

        popupRect = new Rect(popupXOffset, popupYOffset, popupXDimension, popupYDimension);
        popupBgRect = new Rect(0, 0, popupXDimension, popupYDimension);
        popupPictureRect = new Rect(popupPicXOffset, popupPicYOffset, popupPicDimension, popupPicDimension);
        popupCancelButtonRect = new Rect(cancelBtnXOffset, cancelBtnYOffset, cancelBtnWidth, cancelBtnHeight);
        popupConfirmButtonRect = new Rect(confirmBtnXOffset, confirmBtnYOffset, confirmBtnWidth, confirmBtnHeight);
        popupLabelRect = new Rect(popupLabelXOffset, popupLabelYOffset, popupLabelWidth, popupLabelHeight);

        #endregion
    }

    void Update()
    {
        #region Touch Controls

        // Enable touch controls when popup is not up
        if (!show_popup)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    touchStartPosition[touch.fingerId] = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    var deltaPosition = touch.position - touchStartPosition[touch.fingerId];

                    // Horizontal Movement
                    if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y))
                    {
                        if (deltaPosition.x < -minDragDistance &&
                            cur_page < max_page && !inTransition)
                        {
                            cur_page++;
                            if (activeWindow == ItemType.eq_head)
                            {
                                TransitionItems(InventoryManager.instance.GetOwnedEquipment(ItemType.eq_head));
                            }
                            else if (activeWindow == ItemType.eq_body)
                            {
                                TransitionItems(InventoryManager.instance.GetOwnedEquipment(ItemType.eq_body));
                            }
                            else if (activeWindow == ItemType.eq_legs)
                            {
                                TransitionItems(InventoryManager.instance.GetOwnedEquipment(ItemType.eq_legs));
                            }
                            else if (activeWindow == ItemType.item_consumable)
                            {
                                TransitionItems(InventoryManager.instance.GetOwnedEquipment(ItemType.item_consumable));
                            }
                            NextPage();
                        }
                        else if (deltaPosition.x > minDragDistance &&
                                 cur_page > 1 && !inTransition)
                        {
                            cur_page--;
                            if (activeWindow == ItemType.eq_head)
                            {
                                TransitionItems(InventoryManager.instance.GetOwnedEquipment(ItemType.eq_head));
                            }
                            else if (activeWindow == ItemType.eq_body)
                            {
                                TransitionItems(InventoryManager.instance.GetOwnedEquipment(ItemType.eq_body));
                            }
                            else if (activeWindow == ItemType.eq_legs)
                            {
                                TransitionItems(InventoryManager.instance.GetOwnedEquipment(ItemType.eq_legs));
                            }
                            else if (activeWindow == ItemType.item_consumable)
                            {
                                TransitionItems(InventoryManager.instance.GetOwnedEquipment(ItemType.item_consumable));
                            }
                            PrevPage();
                        }
                    }
                    // Vertical Movement
                    else if (Mathf.Abs(deltaPosition.y) > Mathf.Abs(deltaPosition.x))
                    {
                        if (deltaPosition.y < -minDragDistance && !inTransition)
                        {
                            if (activeWindow == ItemType.eq_head)
                            {
                                activeWindow = ItemType.item_consumable;

                                cur_page = 1;
                                TransitionItems(InventoryManager.instance.GetOwnedEquipment(ItemType.item_consumable));

                                ChangeCategory(false);
                            }
                            else if (activeWindow == ItemType.eq_body)
                            {
                                activeWindow = ItemType.eq_head;

                                cur_page = 1;
                                TransitionItems(InventoryManager.instance.GetOwnedEquipment(ItemType.eq_head));

                                ChangeCategory(false);
                            }
                            else if (activeWindow == ItemType.eq_legs)
                            {
                                activeWindow = ItemType.eq_body;

                                cur_page = 1;
                                TransitionItems(InventoryManager.instance.GetOwnedEquipment(ItemType.eq_body));

                                ChangeCategory(false);
                            }
                            else if (activeWindow == ItemType.item_consumable)
                            {
                                activeWindow = ItemType.eq_legs;

                                cur_page = 1;
                                TransitionItems(InventoryManager.instance.GetOwnedEquipment(ItemType.eq_legs));

                                ChangeCategory(false);
                            }
                        }
                        else if (deltaPosition.y > minDragDistance && !inTransition)
                        {
                            if (activeWindow == ItemType.eq_head)
                            {
                                activeWindow = ItemType.eq_body;

                                cur_page = 1;
                                TransitionItems(InventoryManager.instance.GetOwnedEquipment(ItemType.eq_body));

                                ChangeCategory(true);
                            }
                            else if (activeWindow == ItemType.eq_body)
                            {
                                activeWindow = ItemType.eq_legs;

                                cur_page = 1;
                                TransitionItems(InventoryManager.instance.GetOwnedEquipment(ItemType.eq_legs));

                                ChangeCategory(true);
                            }
                            else if (activeWindow == ItemType.eq_legs)
                            {
                                activeWindow = ItemType.item_consumable;

                                cur_page = 1;
                                TransitionItems(InventoryManager.instance.GetOwnedEquipment(ItemType.item_consumable));

                                ChangeCategory(true);
                            }
                            else if (activeWindow == ItemType.item_consumable)
                            {
                                activeWindow = ItemType.eq_head;

                                cur_page = 1;
                                TransitionItems(InventoryManager.instance.GetOwnedEquipment(ItemType.eq_head));

                                ChangeCategory(true);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        // initialise items
        if (!initialized)
        {
            activeWindow = ItemType.eq_head;

            cur_page = 1;
            TransitionItems(InventoryManager.instance.equipmentsHead.Values.ToList());

            ChangeCategory(true);

            initialized = true;
        }

        if (activeWindow == ItemType.eq_head)
        {
            btnHeadStyle.normal.background = btnHeadActiveTexture;
            btnBodyStyle.normal.background = btnBodyInactiveTexture;
            btnLegsStyle.normal.background = btnLegsInactiveTexture;
            btnItemStyle.normal.background = btnItemInactiveTexture;
        }
        else if (activeWindow == ItemType.eq_body)
        {
            btnHeadStyle.normal.background = btnHeadInactiveTexture;
            btnBodyStyle.normal.background = btnBodyActiveTexture;
            btnLegsStyle.normal.background = btnLegsInactiveTexture;
            btnItemStyle.normal.background = btnItemInactiveTexture;
        }
        else if (activeWindow == ItemType.eq_legs)
        {
            btnHeadStyle.normal.background = btnHeadInactiveTexture;
            btnBodyStyle.normal.background = btnBodyInactiveTexture;
            btnLegsStyle.normal.background = btnLegsActiveTexture;
            btnItemStyle.normal.background = btnItemInactiveTexture;
        }
        else if (activeWindow == ItemType.item_consumable)
        {
            btnHeadStyle.normal.background = btnHeadInactiveTexture;
            btnBodyStyle.normal.background = btnBodyInactiveTexture;
            btnLegsStyle.normal.background = btnLegsInactiveTexture;
            btnItemStyle.normal.background = btnItemActiveTexture;
        }

        // Update speech bubble
        if (selected_item != 0)
        {
            bubbleText = slotItems[selected_item-1].description;
        }
        else
        {
            bubbleText = "Skelly (get it?) The souls of the dead will now help you transform that pesky block to wood.";
        }
    }

    // Drawing the GUI
    void OnGUI()
    {
        // Sets the GUI depth
        GUI.depth = 10;

        // Set the active skin
        GUI.skin = activeSkin;
        // The container
        GUI.BeginGroup(containerRect);
        {
            GUI.DrawTexture(bgRect, bgTexture, ScaleMode.ScaleAndCrop);

            Navigation();
            Currency();
            Items();

            // Popup
            if (show_popup)
            {
                if (selected_item == 0)
                {
                    show_popup = false;
                }
                else
                {
                    PopupConfirmation(slotItems[selected_item - 1]);
                }
            }

            Shopkeeper();
        }
        GUI.EndGroup();
    }

    #region GUI Parts

    void Navigation()
    {
        GUI.BeginGroup(navContainerRect);
        {
            if (goodButton(btnBackRect, "", btnBackStyle) && !show_popup)
            {
                NavigationManager.instance.NavToChapterSelect();
            }

            if (goodButton(btnHeadRect, "", btnHeadStyle) && !show_popup)
            {
                activeWindow = ItemType.eq_head;

                cur_page = 1;
                TransitionItems(InventoryManager.instance.equipmentsHead.Values.ToList());
                
                ChangeCategory(true);
            }

            if (goodButton(btnBodyRect, "", btnBodyStyle) && !show_popup)
            {
                activeWindow = ItemType.eq_body;

                cur_page = 1;
                TransitionItems(InventoryManager.instance.equipmentsBody.Values.ToList());

                ChangeCategory(true);
            }

            if (goodButton(btnLegsBodyRect, "", btnLegsStyle) && !show_popup)
            {
                activeWindow = ItemType.eq_legs;

                cur_page = 1;
                TransitionItems(InventoryManager.instance.equipmentsLegs.Values.ToList());

                ChangeCategory(true);
            }

            if (goodButton(btnItemRect, "", btnItemStyle) && !show_popup)
            {
                activeWindow = ItemType.item_consumable;

                cur_page = 1;
                TransitionItems(InventoryManager.instance.itemsConsumable.Values.ToList());

                ChangeCategory(true);
            }
        }
        GUI.EndGroup();
    }

    void Currency()
    {
        // NTP
        GUI.Label(currencyNTPRect, InventoryManager.instance.ntp.ToString(), NTPStyle);
        // GTP
        GUI.Label(currencyGTPRect, InventoryManager.instance.gtp.ToString(), GTPStyle);
    }

    void Items()
    {
        GUI.BeginGroup(itemsContainerRect);
        {
            GUI.BeginGroup(item1Rect);
            {
                GUI.DrawTexture(itemBgRect, itemBoxTexture);
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[0],1);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[0],0);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item2Rect);
            {
                GUI.DrawTexture(itemBgRect, itemBoxTexture);
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[1],2);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[1],0);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item3Rect);
            {
                GUI.DrawTexture(itemBgRect, itemBoxTexture);
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[2],3);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[2],0);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item4Rect);
            {
                GUI.DrawTexture(itemBgRect, itemBoxTexture);
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[3],4);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[3],0);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item5Rect);
            {
                GUI.DrawTexture(itemBgRect, itemBoxTexture);
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[4],5);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[4],0);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item6Rect);
            {
                GUI.DrawTexture(itemBgRect, itemBoxTexture);
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[5],6);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[5],0);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item7Rect);
            {
                GUI.DrawTexture(itemBgRect, itemBoxTexture);
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[6],7);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[6],0);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item8Rect);
            {
                GUI.DrawTexture(itemBgRect, itemBoxTexture);
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[7],8);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[7],0);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item9Rect);
            {
                GUI.DrawTexture(itemBgRect, itemBoxTexture);
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[8],9);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[8],0);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();
        }
        GUI.EndGroup();

        if (cur_page > 1)
        {
            if (goodButton(arrowPrevRect, "", arrowPrevStyle))
            {
                cur_page--;
                if (activeWindow == ItemType.eq_head)
                {
                    TransitionItems(InventoryManager.instance.equipmentsHead.Values.ToList());
                }
                else if (activeWindow == ItemType.eq_body)
                {
                    TransitionItems(InventoryManager.instance.equipmentsBody.Values.ToList());
                }
                else if (activeWindow == ItemType.eq_legs)
                {
                    TransitionItems(InventoryManager.instance.equipmentsLegs.Values.ToList());
                }
                else if (activeWindow == ItemType.item_consumable)
                {
                    TransitionItems(InventoryManager.instance.itemsConsumable.Values.ToList());
                }

                PrevPage();
            }
        }
        if (cur_page < max_page)
        {
            if (goodButton(arrowNextRect, "", arrowNextStyle))
            {
                cur_page++;
                if (activeWindow == ItemType.eq_head)
                {
                    TransitionItems(InventoryManager.instance.equipmentsHead.Values.ToList());
                }
                else if (activeWindow == ItemType.eq_body)
                {
                    TransitionItems(InventoryManager.instance.equipmentsBody.Values.ToList());
                }
                else if (activeWindow == ItemType.eq_legs)
                {
                    TransitionItems(InventoryManager.instance.equipmentsLegs.Values.ToList());
                }
                else if (activeWindow == ItemType.item_consumable)
                {
                    TransitionItems(InventoryManager.instance.itemsConsumable.Values.ToList());
                }

                NextPage();
            }
        }
    }

    void ItemInner(Item item, int index)
    {
        if (item.itemId != "empty"){
            if (item.icon != null)
            {
                GUI.DrawTexture(itemIconRect, item.icon);
            }
            else
            {
                GUI.DrawTexture(itemIconRect, tempIcon);
            }
            
            if (goodButton(itemBgRect, "",activeSkin.button) && !show_popup)
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
            if (selected_item == index && selected_item != 0 && !show_popup)
            {
                GUI.DrawTexture(itemIconRect, highlightTexture);
            }

            if (item.currency == CurrencyType.Dollar)
            {
                if (goodButton(itemBtnRect, item.dollarPrice.ToString(), dollarPurchaseBtnStyle) && !show_popup)
                {
                    selected_item = index;
                    show_popup = true;
                    ShowBubble();
                }
            }
            else if (item.currency == CurrencyType.GTP)
            {
                if (goodButton(itemBtnRect, item.price.ToString(), gtpPurchaseBtnStyle) && !show_popup)
                {
                    selected_item = index;
                    show_popup = true;
                    ShowBubble();
                }
            }
            else if (item.currency == CurrencyType.NTP)
            {
                if (goodButton(itemBtnRect, item.price.ToString(), ntpPurchaseBtnStyle) && !show_popup)
                {
                    selected_item = index;
                    show_popup = true;
                    ShowBubble();
                }
            }

            if (item.type != ItemType.item_consumable &&
                item.type != ItemType.item_instant &&
                item.balance == 1)
            {
                GUI.DrawTexture(itemBgRect, soldOutTexture, ScaleMode.ScaleToFit);
            }
        }
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

    void PopupConfirmation(Item item)
    {
        GUI.color = new Color(0f, 0f, 0f, 0.8f);
        GUI.DrawTexture(containerRect, shadingStyle.normal.background);
        GUI.color = Color.white;

        GUI.BeginGroup(popupRect);
        {
            if (goodButton(popupBgRect, "", activeSkin.button));
            GUI.Box(popupBgRect, "", popupBgStyle);
            if (item.icon != null)
            {
                GUI.DrawTexture(popupPictureRect, item.icon);
            }
            else
            {
                GUI.DrawTexture(popupPictureRect, tempIcon);
            }
            GUI.Label(popupLabelRect, "Purchase " + item.name + "?", popupLabelStyle);
            if (item.currency == CurrencyType.Dollar)
            {
                if (goodButton(popupConfirmButtonRect, item.dollarPrice.ToString(), dollarPurchaseBtnStyle))
                {
                    StoreInventory.BuyItem(item.itemId);
                    InventoryManager.instance.UpdateCurrency();
                }
            }
            else if (item.currency == CurrencyType.GTP)
            {
                if (goodButton(popupConfirmButtonRect, item.price.ToString(), gtpPurchaseBtnStyle))
                {
                    StoreInventory.BuyItem(item.itemId);
                    InventoryManager.instance.UpdateCurrency();
                }
            }
            else if (item.currency == CurrencyType.NTP)
            {
                if (goodButton(popupConfirmButtonRect, item.price.ToString(), ntpPurchaseBtnStyle))
                {
                    StoreInventory.BuyItem(item.itemId);
                    InventoryManager.instance.UpdateCurrency();
                }
            }
        }
        GUI.EndGroup();
        if (goodButton(popupCancelButtonRect, "", popupCancelStyle))
        {
            show_popup = false;
            selected_item = 0;
            HideBubble();
        }
    }
    
    #endregion
    #region Item data

    void TransitionItems(List<Item> items)
    {
        int numberOfItems = items.Count;
        max_page = (numberOfItems - 1) / 9 + 1;
        int offset = 9 * (cur_page - 1);

        for (int i = 0; i < transitionItems.Length; i++)
        {
            if ((i + offset) < numberOfItems)
            {
                transitionItems[i] = items.ElementAt(i + offset);
            }
            else
            {
                transitionItems[i] = new Item();
            }
        }
    }

    #endregion
    #region Animations

    void AnimateItemInnerRect(Rect pos)
    {
        itemInnerContainerRect = pos;
    }

    void AnimateItemTransitionRect(Rect pos)
    {
        itemTransitionContainerRect = pos;
    }

    void AnimateBubble(Rect pos)
    {
        bubbleContainerRect = pos;
    }

    #endregion

    void NextPage()
    {
        inTransition = true;

        selected_item = 0;
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", itemPosCenter,
                                   "to", itemPosLeft,
                                   "onupdate", "AnimateItemInnerRect",
                                   "easetype", iTween.EaseType.easeOutBack,
                                   "time", 0.5f));

        iTween.ValueTo(gameObject,
                       iTween.Hash("from", itemPosRight,
                                   "to", itemPosCenter,
                                   "onupdate", "AnimateItemTransitionRect",
                                   "oncomplete", "OnAnimationComplete",
                                   "easetype", iTween.EaseType.easeOutBack,
                                   "time", 0.5f));
    }

    void PrevPage()
    {
        inTransition = true;

        selected_item = 0;
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", itemPosCenter,
                                   "to", itemPosRight,
                                   "onupdate", "AnimateItemInnerRect",
                                   "easetype", iTween.EaseType.easeOutBack,
                                   "time", 0.5f));

        iTween.ValueTo(gameObject,
                       iTween.Hash("from", itemPosLeft,
                                   "to", itemPosCenter,
                                   "onupdate", "AnimateItemTransitionRect",
                                   "oncomplete", "OnAnimationComplete",
                                   "easetype", iTween.EaseType.easeOutBack,
                                   "time", 0.5f));
    }

    void ChangeCategory(bool up)
    {
        inTransition = true;

        if (up)
        {
            selected_item = 0;
            iTween.ValueTo(gameObject,
                           iTween.Hash("from", itemPosCenter,
                                       "to", itemPosTop,
                                       "onupdate", "AnimateItemInnerRect",
                                       "easetype", iTween.EaseType.easeOutBack,
                                       "time", 0.5f));

            iTween.ValueTo(gameObject,
                           iTween.Hash("from", itemPosBottom,
                                       "to", itemPosCenter,
                                       "onupdate", "AnimateItemTransitionRect",
                                       "oncomplete", "OnAnimationComplete",
                                       "easetype", iTween.EaseType.easeOutBack,
                                       "time", 0.5f));
        }
        else
        {
            selected_item = 0;
            iTween.ValueTo(gameObject,
                           iTween.Hash("from", itemPosCenter,
                                       "to", itemPosBottom,
                                       "onupdate", "AnimateItemInnerRect",
                                       "easetype", iTween.EaseType.easeOutBack,
                                       "time", 0.5f));

            iTween.ValueTo(gameObject,
                           iTween.Hash("from", itemPosTop,
                                       "to", itemPosCenter,
                                       "onupdate", "AnimateItemTransitionRect",
                                       "oncomplete", "OnAnimationComplete",
                                       "easetype", iTween.EaseType.easeOutBack,
                                       "time", 0.5f));
        }

        HideBubble();
    }

    void OnAnimationComplete()
    {
        itemInnerContainerRect = itemPosCenter;
        itemTransitionContainerRect = itemPosBottom;

        for (int i = 0; i < slotItems.Length; i++)
        {
            slotItems[i] = transitionItems[i];
        }

        inTransition = false;
    }

    void ShowBubble()
    {
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", bubbleContainerRect,
                                   "to", openBubbleRect,
                                   "onupdate", "AnimateBubble",
                                   "easetype", iTween.EaseType.easeOutQuart,
                                   "time", 0.1f));
    }

    void HideBubble()
    {
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", bubbleContainerRect,
                                   "to", closedBubbleRect,
                                   "onupdate", "AnimateBubble",
                                   "easetype", iTween.EaseType.easeInQuart,
                                   "time", 0.1f));
    }

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