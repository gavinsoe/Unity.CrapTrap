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
     * Custom Styles [4] = Body Button
     * Custom Styles [5] = Hands Button
     * Custom Styles [6] = Legs Button
     * Custom Styles [7] = Feet Button
     * Custom Styles [8] = Item Button
     * Custom Styles [9] = GTP
     * Custom Styles [10] = NTP
     * Custom Styles [11] = Item Container
     * Custom Styles [12] = Arrow Next
     * Custom Styles [13] = Arrow Prev
     * Custom Styles [14] = NTP Purchase Button
     * Custom Styles [15] = GTP Purchase Button
     * Custom Styles [16] = $ Purchase Button
     * Custom Styles [17] = Sold out
     * Custom Styles [17] = Item Highlight
     */
    public GUISkin activeSkin;
    public Texture tempIcon;

    private MainGameController mainController;
    private InventoryManager inventory;
    private ItemType activeWindow; // the active item shop window
    public int cur_page;
    public int max_page; //number of pages

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
    public float shopkeeperScale = 0.72f;
    public float shopkeeperXOffset = 0.73f;
    public float shopkeeperYOffset = 0.54f;

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

    private Rect btnHandsRect;
    private Texture2D btnHandsInactiveTexture;
    private Texture2D btnHandsActiveTexture;
    private GUIStyle btnHandsStyle;

    private Rect btnLegsRect;
    private Texture2D btnLegsInactiveTexture;
    private Texture2D btnLegsActiveTexture;
    private GUIStyle btnLegsStyle;

    private Rect btnFeetRect;
    private Texture2D btnFeetInactiveTexture;
    private Texture2D btnFeetActiveTexture;
    private GUIStyle btnFeetStyle;

    private Rect btnItemRect;
    private Texture2D btnItemInactiveTexture;
    private Texture2D btnItemActiveTexture;
    private GUIStyle btnItemStyle;

    #endregion
    #region currency boxes

    public int ntp = 0; // Keeps track of the number of ntp
    public int gtp = 0; // Keeps track of the number of gtp

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

    public float itemIconScale;
    private Rect itemIconRect;

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
    #endregion

    // Use this for initialization
    void Start() 
    {
        // Initialize soomla store
        SoomlaStore.Initialize(new CrapTrapAssets());

        // Retrieve the main game controller
        mainController = gameObject.GetComponentInChildren<MainGameController>();

        // Inventory
        // TODO:: need to change
        inventory = gameObject.GetComponent<InventoryManager>();

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

        btnHandsActiveTexture = activeSkin.customStyles[5].onNormal.background;
        btnHandsInactiveTexture = activeSkin.customStyles[5].normal.background;
        btnHandsStyle = new GUIStyle(activeSkin.customStyles[5]);

        btnLegsActiveTexture = activeSkin.customStyles[6].onNormal.background;
        btnLegsInactiveTexture = activeSkin.customStyles[6].normal.background;
        btnLegsStyle = new GUIStyle(activeSkin.customStyles[6]);

        btnFeetActiveTexture = activeSkin.customStyles[7].onNormal.background;
        btnFeetInactiveTexture = activeSkin.customStyles[7].normal.background;
        btnFeetStyle = new GUIStyle(activeSkin.customStyles[7]);

        btnItemActiveTexture = activeSkin.customStyles[8].onNormal.background;
        btnItemInactiveTexture = activeSkin.customStyles[8].normal.background;
        btnItemStyle = new GUIStyle(activeSkin.customStyles[8]);

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
        btnHandsRect = new Rect(0.5f * (navContainerWidth - eqBtnWidth), btnHeight * 3, eqBtnWidth, btnHeight);
        btnLegsRect = new Rect(0.5f * (navContainerWidth - eqBtnWidth), btnHeight * 4, eqBtnWidth, btnHeight);
        btnFeetRect = new Rect(0.5f * (navContainerWidth - eqBtnWidth), btnHeight * 5, eqBtnWidth, btnHeight);
        btnItemRect = new Rect(0.5f * (navContainerWidth - eqBtnWidth), btnHeight * 6, eqBtnWidth, btnHeight);

        #endregion
        #region currency boxes

        NTPStyle = new GUIStyle(activeSkin.customStyles[10]);
        GTPStyle = new GUIStyle(activeSkin.customStyles[9]);
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

        itemBoxTexture = activeSkin.customStyles[11].normal.background;
        arrowNextStyle = activeSkin.customStyles[12];
        arrowPrevStyle = activeSkin.customStyles[13];
        soldOutTexture = activeSkin.customStyles[17].normal.background;
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
        float itemIconDimension = itemBoxHeight * itemIconScale;
        float itemIconOffset = (itemBoxWidth - itemIconDimension) * 0.5f;

        // Calculate button location and dimension
        ntpPurchaseBtnStyle = activeSkin.customStyles[14];
        gtpPurchaseBtnStyle = activeSkin.customStyles[15];
        dollarPurchaseBtnStyle = activeSkin.customStyles[16];

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
        itemIconRect = new Rect(itemIconOffset, 0, itemIconDimension, itemIconDimension);
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
    }

    void Update()
    {
        if (activeWindow == ItemType.eq_head)
        {
            btnHeadStyle.normal.background = btnHeadActiveTexture;
            btnBodyStyle.normal.background = btnBodyInactiveTexture;
            btnHandsStyle.normal.background = btnHandsInactiveTexture;
            btnLegsStyle.normal.background = btnLegsInactiveTexture;
            btnFeetStyle.normal.background = btnFeetInactiveTexture;
            btnItemStyle.normal.background = btnItemInactiveTexture;
        }
        else if (activeWindow == ItemType.eq_body)
        {
            btnHeadStyle.normal.background = btnHeadInactiveTexture;
            btnBodyStyle.normal.background = btnBodyActiveTexture;
            btnHandsStyle.normal.background = btnHandsInactiveTexture;
            btnLegsStyle.normal.background = btnLegsInactiveTexture;
            btnFeetStyle.normal.background = btnFeetInactiveTexture;
            btnItemStyle.normal.background = btnItemInactiveTexture;
        }
        else if (activeWindow == ItemType.eq_hands)
        {
            btnHeadStyle.normal.background = btnHeadInactiveTexture;
            btnBodyStyle.normal.background = btnBodyInactiveTexture;
            btnHandsStyle.normal.background = btnHandsActiveTexture;
            btnLegsStyle.normal.background = btnLegsInactiveTexture;
            btnFeetStyle.normal.background = btnFeetInactiveTexture;
            btnItemStyle.normal.background = btnItemInactiveTexture;
        }
        else if (activeWindow == ItemType.eq_legs)
        {
            btnHeadStyle.normal.background = btnHeadInactiveTexture;
            btnBodyStyle.normal.background = btnBodyInactiveTexture;
            btnHandsStyle.normal.background = btnHandsInactiveTexture;
            btnLegsStyle.normal.background = btnLegsActiveTexture;
            btnFeetStyle.normal.background = btnFeetInactiveTexture;
            btnItemStyle.normal.background = btnItemInactiveTexture;
        }
        else if (activeWindow == ItemType.eq_feet)
        {
            btnHeadStyle.normal.background = btnHeadInactiveTexture;
            btnBodyStyle.normal.background = btnBodyInactiveTexture;
            btnHandsStyle.normal.background = btnHandsInactiveTexture;
            btnLegsStyle.normal.background = btnLegsInactiveTexture;
            btnFeetStyle.normal.background = btnFeetActiveTexture;
            btnItemStyle.normal.background = btnItemInactiveTexture;
        }
        else if (activeWindow == ItemType.item_consumable)
        {
            btnHeadStyle.normal.background = btnHeadInactiveTexture;
            btnBodyStyle.normal.background = btnBodyInactiveTexture;
            btnHandsStyle.normal.background = btnHandsInactiveTexture;
            btnLegsStyle.normal.background = btnLegsInactiveTexture;
            btnFeetStyle.normal.background = btnFeetInactiveTexture;
            btnItemStyle.normal.background = btnItemActiveTexture;
        }
    }

    // Drawing the GUI
    void OnGUI()
    {
        #region temp


        #endregion

        // Set the active skin
        GUI.skin = activeSkin;
        // The container
        GUI.BeginGroup(containerRect);
        {
            GUI.DrawTexture(bgRect, bgTexture, ScaleMode.ScaleAndCrop);
            GUI.DrawTexture(shopkeeperRect, shopkeeperTexture);

            Navigation();
            Currency();
            Items();
        }
        GUI.EndGroup();
    }

    #region GUI Parts

    void Navigation()
    {
        GUI.BeginGroup(navContainerRect);
        {
            if (GUI.Button(btnBackRect, "", btnBackStyle))
            {
            }

            if (GUI.Button(btnHeadRect, "", btnHeadStyle))
            {
                activeWindow = ItemType.eq_head;

                cur_page = 1;
                TransitionItems(inventory.equipmentsHead.Values.ToList());
                
                ChangeCategory();
            }

            if (GUI.Button(btnBodyRect, "", btnBodyStyle))
            {
                activeWindow = ItemType.eq_body;

                cur_page = 1;
                TransitionItems(inventory.equipmentsBody.Values.ToList());

                ChangeCategory();
            }

            if (GUI.Button(btnHandsRect, "", btnHandsStyle))
            {
                activeWindow = ItemType.eq_hands;

                cur_page = 1;
                TransitionItems(inventory.equipmentsHands.Values.ToList());

                ChangeCategory();
            }

            if (GUI.Button(btnLegsRect, "", btnLegsStyle))
            {
                activeWindow = ItemType.eq_legs;

                cur_page = 1;
                TransitionItems(inventory.equipmentsLegs.Values.ToList());

                ChangeCategory();
            }

            if (GUI.Button(btnFeetRect, "", btnFeetStyle))
            {
                activeWindow = ItemType.eq_feet;

                cur_page = 1;
                TransitionItems(inventory.equipmentsFeet.Values.ToList());

                ChangeCategory();
            }

            if (GUI.Button(btnItemRect, "", btnItemStyle))
            {
                activeWindow = ItemType.item_consumable;

                cur_page = 1;
                TransitionItems(inventory.itemsConsumable.Values.ToList());

                ChangeCategory();
            }
        }
        GUI.EndGroup();
    }

    void Currency()
    {
        // NTP
        GUI.Label(currencyNTPRect, ntp.ToString(), NTPStyle);
        // GTP
        GUI.Label(currencyGTPRect, gtp.ToString(), GTPStyle);
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
                    ItemInner(slotItems[0]);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[0]);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item2Rect);
            {
                GUI.DrawTexture(itemBgRect, itemBoxTexture);
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[1]);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[1]);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item3Rect);
            {
                GUI.DrawTexture(itemBgRect, itemBoxTexture);
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[2]);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[2]);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item4Rect);
            {
                GUI.DrawTexture(itemBgRect, itemBoxTexture);
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[3]);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[3]);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item5Rect);
            {
                GUI.DrawTexture(itemBgRect, itemBoxTexture);
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[4]);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[4]);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item6Rect);
            {
                GUI.DrawTexture(itemBgRect, itemBoxTexture);
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[5]);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[5]);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item7Rect);
            {
                GUI.DrawTexture(itemBgRect, itemBoxTexture);
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[6]);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[6]);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item8Rect);
            {
                GUI.DrawTexture(itemBgRect, itemBoxTexture);
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[7]);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[7]);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item9Rect);
            {
                GUI.DrawTexture(itemBgRect, itemBoxTexture);
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[8]);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[8]);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();
        }
        GUI.EndGroup();

        if (cur_page > 1)
        {
            if (GUI.Button(arrowPrevRect, "", arrowPrevStyle))
            {
                cur_page--;
                if (activeWindow == ItemType.eq_head)
                {
                    TransitionItems(inventory.equipmentsHead.Values.ToList());
                }
                else if (activeWindow == ItemType.eq_body)
                {
                    TransitionItems(inventory.equipmentsBody.Values.ToList());
                }
                else if (activeWindow == ItemType.eq_hands)
                {
                    TransitionItems(inventory.equipmentsHands.Values.ToList());
                }
                else if (activeWindow == ItemType.eq_legs)
                {
                    TransitionItems(inventory.equipmentsLegs.Values.ToList());
                }
                else if (activeWindow == ItemType.eq_feet)
                {
                    TransitionItems(inventory.equipmentsFeet.Values.ToList());
                }
                else if (activeWindow == ItemType.item_consumable)
                {
                    TransitionItems(inventory.itemsConsumable.Values.ToList());
                }

                PrevPage();
            }
        }
        if (cur_page < max_page)
        {
            if (GUI.Button(arrowNextRect, "", arrowNextStyle))
            {
                cur_page++;
                if (activeWindow == ItemType.eq_head)
                {
                    TransitionItems(inventory.equipmentsHead.Values.ToList());
                }
                else if (activeWindow == ItemType.eq_body)
                {
                    TransitionItems(inventory.equipmentsBody.Values.ToList());
                }
                else if (activeWindow == ItemType.eq_hands)
                {
                    TransitionItems(inventory.equipmentsHands.Values.ToList());
                }
                else if (activeWindow == ItemType.eq_legs)
                {
                    TransitionItems(inventory.equipmentsLegs.Values.ToList());
                }
                else if (activeWindow == ItemType.eq_feet)
                {
                    TransitionItems(inventory.equipmentsFeet.Values.ToList());
                }
                else if (activeWindow == ItemType.item_consumable)
                {
                    TransitionItems(inventory.itemsConsumable.Values.ToList());
                }

                NextPage();
            }
        }
    }

    void ItemInner(Item item)
    {
        if (item.itemId != "empty"){
            GUI.DrawTexture(itemIconRect, tempIcon);

            if (item.currency == CurrencyType.Dollar)
            {
                GUI.Button(itemBtnRect, item.dollarPrice.ToString(), dollarPurchaseBtnStyle);
            }
            else if (item.currency == CurrencyType.GTP)
            {
                GUI.Button(itemBtnRect, item.price.ToString(), gtpPurchaseBtnStyle);
            }
            else if (item.currency == CurrencyType.NTP)
            {
                GUI.Button(itemBtnRect, item.price.ToString(), ntpPurchaseBtnStyle);
            }

            if (item.type != ItemType.item_consumable &&
                item.type != ItemType.item_instant &&
                item.balance == 0)
            {
                GUI.DrawTexture(itemBgRect, soldOutTexture, ScaleMode.ScaleToFit);
            }
        }
    }
    #endregion
    #region Item data

    void TransitionItems(List<Item> items)
    {
        int numberOfItems = items.Count;
        max_page = numberOfItems / 9 + 1;
        Debug.Log("Number of items: " + numberOfItems);
        Debug.Log("Number of pages: " + max_page);

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

    #endregion
    void NextPage()
    {
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

    void ChangeCategory()
    {

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
                                   "oncomplete","OnAnimationComplete",
                                   "easetype", iTween.EaseType.easeOutBack,
                                   "time", 0.5f));
    }

    void OnAnimationComplete()
    {
        itemInnerContainerRect = itemPosCenter;
        itemTransitionContainerRect = itemPosBottom;

        for (int i = 0; i < slotItems.Length; i++)
        {
            slotItems[i] = transitionItems[i];
        }
    }
}