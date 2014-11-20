using Soomla;
using Soomla.Store;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CharacterPageGUI : MonoBehaviour 
{
    /* GUI Skin
     * Custom Styles [0] = Background
     * Custom Styles [1] = Back Button
     * Custom Styles [2] = Head Button
     * Custom Styles [3] = Upper Body Button
     * Custom Styles [4] = Lower Body Button
     * Custom Styles [5] = Item Button
     * Custom Styles [6] = GTP
     * Custom Styles [7] = NTP
     * Custom Styles [8] = Item Frame
     * Custom Styles [9] = Arrow Next
     * Custom Styles [10] = Arrow Prev
     * Custom Styles [11] = Backpack
     * Custom Styles [12] = Equipment Triangle
     * Custom Styles [13] = Item Slot Empty
     * Custom Styles [14] = Item Slot Filled
     * Custom Styles [15] = Gear Slot Empty
     * Custom Styles [16] = Gear Slot Head
     * Custom Styles [17] = Gear Slot Body
     * Custom Styles [18] = Gear Slot Legs
     * Custom Styles [19] = Popup Box
     * Custom Styles [20] = Cancel Button
     * Custom Styles [21] = Shading Base
     * Custom Styles [22] = Popup Button
     */

    public GUISkin activeSkin;
    public Texture tempIcon;

    private ItemType activeWindow; // the active equipment window
    private int cur_page; // active page number
    private int max_page; // number of pages
    private int selected_item = 0; // index of the selected item
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
    #region Item Frame

    private Rect itemShelfRect;
    private Rect itemShelfBgRect;
    private Texture itemShelfTexture;

    private float itemShelfXOffset = 0.15f;
    private float itemShelfScale = 0.98f;
    private float itemBoxScale = 0.217f;
    private float colOffset1 = 0.045f;
    private float colOffset2 = 0.361f;
    private float colOffset3 = 0.677f;
    private float rowOffset1 = 0.032f;
    private float rowOffset2 = 0.270f;
    private float rowOffset3 = 0.513f;

    #endregion
    #region Items

    private Rect item1Rect;
    private Rect item2Rect;
    private Rect item3Rect;
    private Rect item4Rect;
    private Rect item5Rect;
    private Rect item6Rect;
    private Rect item7Rect;
    private Rect item8Rect;
    private Rect item9Rect;

    private Rect itemInnerContainerRect;
    private Rect itemTransitionContainerRect;
    private Rect itemPosCenter;
    private Rect itemPosLeft;
    private Rect itemPosRight;
    private Rect itemPosTop;
    private Rect itemPosBottom;

    private float itemIconScale = 1;
    private Rect itemIconRect;

    public float itemSlotInnerScale = 0.7f;
    private Rect itemSlotInnerRect;

    #endregion
    #region Arrows

    private GUIStyle arrowNextStyle;
    private Rect arrowNextRect;
    private GUIStyle arrowPrevStyle;
    private Rect arrowPrevRect;
    private float arrowScale = 0.08f;

    #endregion
    #region Equipped Consumables

    private Texture backpackTexture;
    private Rect backpackRect;
    private float backpackScale = 0.17f;
    private float backpackXOffset = 0.07f;
    private float backpackYOffset = 0.775f;

    private float consumablesXOffset = 0.3f;
    private float consumablesYOffset = 0.78f;
    private float consumablesScale = 0.16f;
    private Texture itemEmptyTexture;
    private Texture itemFilledTexture;
    private Rect consumable1Rect;
    private Rect consumable2Rect;
    private Rect consumable3Rect;

    private float consumableIconScale = 1f;
    private Rect consumableIconRect;

    public float consumableInnerScale = 0.7f;
    private Rect consumableInnerRect;

    #endregion
    #region Equipment Triangle

    private Rect eqTriangleContainerRect;
    public float triangleYOffset;
    private Texture triangleTexture;
    private Rect triangleRect;

    private float eqSlotScale = 0.28f;
    private Texture gearEmptyTexture;
    private Texture gearHeadTexture;
    private Texture gearBodyTexture;
    private Texture gearLegsTexture;

    private float eqSlotHeadXOffset = 0.5f;
    private float eqSlotHeadYOffset = 0.16f;
    private Rect eqSlotHeadRect;

    private float eqSlotBodyXOffset = 0.12f;
    private float eqSlotBodyYOffset = 0.86f;
    private Rect eqSlotBodyRect;

    private float eqSlotLegsXOffset = 0.88f;
    private float eqSlotLegsYOffset = 0.86f;
    private Rect eqSlotLegsRect;

    public float eqSlotIconScale = 1f;
    private Rect eqSlotIconRect;

    public float eqSlotInnerScale = 0.7f;
    private Rect eqSlotInnerRect;

    private Rect triangleContainer;

    #endregion
    #region

    public Rect popupRect;
    public Rect popupBgRect;
    public Rect popupPictureRect;
    public Rect popupLabelRect;
    public Rect popupCancelButtonRect;
    public Rect popupConfirmButtonRect;

    private GUIStyle shadingStyle;
    private GUIStyle popupBgStyle;
    private GUIStyle popupCancelStyle;
    public float popupXScale = 0.9f; // ratio of the popup box, based on the 'shelves' (3x3 boxes)
    public float popupRatio = 0.4f; // y:x ratio 2:5
    public float popupPadding = 20f;

    public float popupConfirmBtnScale = 0.25f;

    public float popupCancelBtnScale = 0.28f;
    public float popupCancelXOffset = 0.7f;
    public float popupCancelYOffset = 0.27f;

    private GUIStyle popupLabelStyle;
    private GUIStyle popupBtnStyle;
    private float popupLabelScale;

    #endregion

    #endregion

    // Use this for initialization
	void Start () {

        // Set the container rect
        containerRect = new Rect(0, 0, Screen.width, Screen.height);

        #region background

        // Background texture
        bgRect = new Rect(0, 0, Screen.width, Screen.height);
        bgTexture = activeSkin.customStyles[0].normal.background;

        #endregion
        #region navigation

        btnBackStyle = activeSkin.customStyles[1];

        btnHeadActiveTexture = activeSkin.customStyles[2].onNormal.background;
        btnHeadInactiveTexture = activeSkin.customStyles[2].normal.background;
        btnHeadStyle = new GUIStyle(activeSkin.customStyles[2]);

        btnBodyActiveTexture = activeSkin.customStyles[3].onNormal.background;
        btnBodyInactiveTexture = activeSkin.customStyles[3].normal.background;
        btnBodyStyle = new GUIStyle(activeSkin.customStyles[3]);

        btnLegsActiveTexture = activeSkin.customStyles[4].onNormal.background;
        btnLegsInactiveTexture = activeSkin.customStyles[4].normal.background;
        btnLegsStyle = new GUIStyle(activeSkin.customStyles[4]);

        btnItemActiveTexture = activeSkin.customStyles[5].onNormal.background;
        btnItemInactiveTexture = activeSkin.customStyles[5].normal.background;
        btnItemStyle = new GUIStyle(activeSkin.customStyles[5]);

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

        NTPStyle = new GUIStyle(activeSkin.customStyles[7]);
        GTPStyle = new GUIStyle(activeSkin.customStyles[6]);
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
        #region Item Shelf

        itemShelfTexture = activeSkin.customStyles[8].normal.background;
        float shelfHeight = Screen.height * itemShelfScale;
        float shelfWidth = Screen.height * ((float)itemShelfTexture.width / (float)itemShelfTexture.height);
        float shelfXOffset = Screen.width * itemShelfXOffset;
        float shelfYOffset = (Screen.height - shelfHeight) / 2;

        itemShelfRect = new Rect(shelfXOffset, shelfYOffset, shelfWidth, shelfHeight);
        itemShelfBgRect = new Rect(0, 0, shelfWidth, shelfHeight);

        #endregion
        #region Items

        // Calculate item dimensions and offsets
        float col1 = shelfWidth * colOffset1;
        float col2 = shelfWidth * colOffset2;
        float col3 = shelfWidth * colOffset3;
        float row1 = shelfHeight * rowOffset1;
        float row2 = shelfHeight * rowOffset2;
        float row3 = shelfHeight * rowOffset3;
        float itemBoxDimension = shelfHeight * itemBoxScale;

        // Calculate item Icon dimension and offset
        float itemIconDimension = itemBoxDimension * itemIconScale;
        float itemIconOffset = (itemBoxDimension - itemIconDimension) * 0.5f;

        // Positioning
        itemIconRect = new Rect(itemIconOffset, itemIconOffset, itemIconDimension, itemIconDimension);
        item1Rect = new Rect(col1, row1, itemBoxDimension, itemBoxDimension);
        item2Rect = new Rect(col2, row1, itemBoxDimension, itemBoxDimension);
        item3Rect = new Rect(col3, row1, itemBoxDimension, itemBoxDimension);
        item4Rect = new Rect(col1, row2, itemBoxDimension, itemBoxDimension);
        item5Rect = new Rect(col2, row2, itemBoxDimension, itemBoxDimension);
        item6Rect = new Rect(col3, row2, itemBoxDimension, itemBoxDimension);
        item7Rect = new Rect(col1, row3, itemBoxDimension, itemBoxDimension);
        item8Rect = new Rect(col2, row3, itemBoxDimension, itemBoxDimension);
        item9Rect = new Rect(col3, row3, itemBoxDimension, itemBoxDimension);

        itemPosCenter = new Rect(0, 0, itemBoxDimension, itemBoxDimension);
        itemPosLeft = new Rect(-itemBoxDimension, 0, itemBoxDimension, itemBoxDimension);
        itemPosRight = new Rect(itemBoxDimension, 0, itemBoxDimension, itemBoxDimension);
        itemPosTop = new Rect(0, -itemBoxDimension, itemBoxDimension, itemBoxDimension);
        itemPosBottom = new Rect(0, itemBoxDimension, itemBoxDimension, itemBoxDimension);
        itemInnerContainerRect = itemPosCenter;
        itemTransitionContainerRect = itemPosLeft;

        #endregion
        #region Arrow Nav

        arrowNextStyle = activeSkin.customStyles[9];
        arrowPrevStyle = activeSkin.customStyles[10];

        Texture arrowTexture = arrowNextStyle.normal.background;
        float arrowHeight = Screen.height * arrowScale;
        float arrowWidth = arrowHeight * ((float)arrowTexture.width / (float)arrowTexture.height);
        float arrowPrevXOffset = itemShelfRect.x - arrowWidth;
        float arrowNextXOffset = itemShelfRect.x + itemShelfRect.width;
        float arrowYOffset = itemShelfRect.y + row2 + 0.5f * (itemBoxDimension - arrowHeight);
        arrowNextRect = new Rect(arrowNextXOffset, arrowYOffset, arrowWidth, arrowHeight);
        arrowPrevRect = new Rect(arrowPrevXOffset, arrowYOffset, arrowWidth, arrowHeight);

        #endregion
        #region Backpack

        backpackTexture = activeSkin.customStyles[11].normal.background;
        float bpkHeight = itemShelfRect.height * backpackScale;
        float bpkWidth = bpkHeight * ((float)backpackTexture.width / (float)backpackTexture.height);
        float bpkXOffset = itemShelfRect.width * backpackXOffset;
        float bpkYOffset = itemShelfRect.height * backpackYOffset;

        backpackRect = new Rect(bpkXOffset, bpkYOffset, bpkWidth, bpkHeight);

        #endregion
        #region Consumable Items

        itemEmptyTexture = activeSkin.customStyles[13].normal.background;
        itemFilledTexture = activeSkin.customStyles[14].normal.background;

        float conHeight = itemShelfRect.height * consumablesScale;
        float conWidth = conHeight * ((float)itemEmptyTexture.width / (float)itemEmptyTexture.height);
        float conXOffset = itemShelfRect.width * consumablesXOffset;
        float conYOffset = itemShelfRect.height * consumablesYOffset;

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

        consumableIconRect = new Rect(conIconXOffset, conIconYOffset, conIconWidth, conIconHeight);
        consumableInnerRect = new Rect(conInnerXOffset, conInnerYOffset, conInnerWidth, conInnerHeight);

        #endregion
        #region Equipment Triangle

        gearEmptyTexture = activeSkin.customStyles[15].normal.background;
        gearHeadTexture = activeSkin.customStyles[16].normal.background;
        gearBodyTexture = activeSkin.customStyles[17].normal.background;
        gearLegsTexture = activeSkin.customStyles[18].normal.background;

        float tContainerHeight = Screen.height;
        float tContainerWidth = Screen.width - arrowNextRect.x;
        float tContainerXOffset = arrowNextRect.x;
        float tContainerYOffset = 0f;

        triangleTexture = activeSkin.customStyles[12].normal.background;
        float tWidth = tContainerWidth;
        float tHeight = tWidth * ((float)triangleTexture.height / (float)triangleTexture.width);
        float tXOffset = 0;
        float tYOffset = 0.5f * (tContainerHeight - tHeight) + (tContainerHeight * triangleYOffset);

        float eqSlotHeight = tHeight * eqSlotScale;
        float eqSlotWidth = eqSlotHeight * ((float)gearEmptyTexture.width / (float)gearEmptyTexture.height);

        float eqHeadXOffset = tXOffset + (tWidth * eqSlotHeadXOffset) - (eqSlotWidth * 0.5f);
        float eqHeadYOffset = tYOffset + (tHeight * eqSlotHeadYOffset) - (eqSlotHeight * 0.5f);

        float eqBodyXOffset = tXOffset + (tWidth * eqSlotBodyXOffset) - (eqSlotWidth * 0.5f);
        float eqBodyYOffset = tYOffset + (tHeight * eqSlotBodyYOffset) - (eqSlotHeight * 0.5f);

        float eqLegsXOffset = tXOffset + (tWidth * eqSlotLegsXOffset) - (eqSlotWidth * 0.5f);
        float eqLegsYOffset = tYOffset + (tHeight * eqSlotLegsYOffset) - (eqSlotHeight * 0.5f);

        // calculate the container icon size
        float eqSlotIconHeight = eqSlotHeight * eqSlotIconScale;
        float eqSlotIconWidth = eqSlotWidth * eqSlotIconScale;
        float eqSlotIconYOffset = (eqSlotHeight - eqSlotIconHeight) * 0.5f;
        float eqSlotIconXOffset = (eqSlotWidth - eqSlotIconWidth) * 0.5f;

        // calculate the equipment icon size
        float eqSlotInnerHeight = eqSlotIconHeight * eqSlotInnerScale;
        float eqSlotInnerWidth = eqSlotInnerHeight;
        float eqslotInnerXOffset = (eqSlotIconHeight - eqSlotInnerHeight) * 0.5f;
        float eqSlotInnerYOffset = (eqSlotIconWidth - eqSlotInnerWidth) * 0.5f;

        //float eqSlotIconHeight = eqSlotHeight * eqSlotInnerIconScale;

        eqTriangleContainerRect = new Rect(tContainerXOffset, tContainerYOffset, tContainerWidth, tContainerHeight);
        triangleRect = new Rect(tXOffset, tYOffset, tWidth, tHeight);
        eqSlotHeadRect = new Rect(eqHeadXOffset, eqHeadYOffset, eqSlotWidth, eqSlotHeight);
        eqSlotBodyRect = new Rect(eqBodyXOffset, eqBodyYOffset, eqSlotWidth, eqSlotHeight);
        eqSlotLegsRect = new Rect(eqLegsXOffset, eqLegsYOffset, eqSlotWidth, eqSlotHeight);
        eqSlotIconRect = new Rect(eqSlotIconXOffset, eqSlotIconYOffset, eqSlotIconWidth, eqSlotIconHeight);
        eqSlotInnerRect = new Rect(eqslotInnerXOffset, eqSlotInnerYOffset, eqSlotInnerWidth, eqSlotInnerHeight);


        #endregion
        #region popup confirmation

        // Shading base
        shadingStyle = activeSkin.customStyles[21];

        // Container
        float popupXDimension = itemShelfRect.width * popupXScale;
        float popupYDimension = popupXDimension * popupRatio;
        float popupXOffset = itemShelfRect.x + (itemShelfRect.width - popupXDimension) * 0.5f;
        float popupYOffset = itemShelfRect.y + (itemShelfRect.height - popupYDimension) * 0.5f;
        popupBgStyle = activeSkin.customStyles[19];

        // Preview Pic
        float popupPicDimension = popupYDimension - 2 * popupPadding;
        float popupPicXOffset = popupPadding;
        float popupPicYOffset = popupPadding;

        // Cancel button
        popupCancelStyle = activeSkin.customStyles[20];
        float cancelBtnHeight = popupYDimension * popupCancelBtnScale;
        float cancelBtnWidth = cancelBtnHeight * ((float)popupCancelStyle.normal.background.width /
                                                  (float)popupCancelStyle.normal.background.height);
        float cancelBtnXOffset = popupXDimension + popupXOffset - (cancelBtnWidth * popupCancelXOffset);
        float cancelBtnYOffset = popupYOffset - (cancelBtnHeight * popupCancelYOffset);

        // Confirm Button
        popupBtnStyle = activeSkin.customStyles[22];
        float confirmBtnHeight = popupYDimension * popupConfirmBtnScale;
        float confirmBtnWidth = confirmBtnHeight * ((float)popupBtnStyle.normal.background.width /
                                                    (float)popupBtnStyle.normal.background.height);
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

        // Initialise empty item objects
        int count = 0;
        while (count < 9)
        {
            slotItems[count] = new Item();
            transitionItems[count] = new Item();
            count++;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Initialise Items
        if (!initialized)
        {
            activeWindow = ItemType.eq_head;

            cur_page = 1;

            TransitionItems(InventoryManager.instance.GetOwnedEquipment(ItemType.eq_head));

            ChangeCategory();

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

    }

    // Draw the GUI
    void OnGUI()
    {
        // Set the active skin
        GUI.skin = activeSkin;
        // The container
        GUI.BeginGroup(containerRect);
        {
            GUI.DrawTexture(bgRect, bgTexture, ScaleMode.ScaleAndCrop);
            Navigation();
            Currency();
            ItemShelf();
            EquipmentTriangle();

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
            }

            if (goodButton(btnHeadRect, "", btnHeadStyle) && !show_popup)
            {
                activeWindow = ItemType.eq_head;

                cur_page = 1;
                TransitionItems(InventoryManager.instance.GetOwnedEquipment(ItemType.eq_head));
                
                ChangeCategory();
            }

            if (goodButton(btnBodyRect, "", btnBodyStyle) && !show_popup)
            {
                activeWindow = ItemType.eq_body;

                cur_page = 1;
                TransitionItems(InventoryManager.instance.GetOwnedEquipment(ItemType.eq_body));

                ChangeCategory();
            }

            if (goodButton(btnLegsBodyRect, "", btnLegsStyle) && !show_popup)
            {
                activeWindow = ItemType.eq_legs;

                cur_page = 1;
                TransitionItems(InventoryManager.instance.GetOwnedEquipment(ItemType.eq_legs));

                ChangeCategory();
            }

            if (goodButton(btnItemRect, "", btnItemStyle) && !show_popup)
            {
                activeWindow = ItemType.item_consumable;

                cur_page = 1;
                TransitionItems(InventoryManager.instance.GetOwnedEquipment(ItemType.item_consumable));
                
                ChangeCategory();
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

    void ItemShelf()
    {
        GUI.BeginGroup(itemShelfRect);
        {
            GUI.DrawTexture(itemShelfBgRect, itemShelfTexture);

            GUI.BeginGroup(item1Rect);
            {
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[0], 1);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[0], 0);
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
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[1], 0);
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
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[2], 0);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item4Rect);
            {
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[3], 4);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[3], 0);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item5Rect);
            {
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[4], 5);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[4], 0);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item6Rect);
            {
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[5], 6);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[5], 0);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item7Rect);
            {
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[6], 7);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[6], 0);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item8Rect);
            {
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[7], 8);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[7], 0);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            GUI.BeginGroup(item9Rect);
            {
                GUI.BeginGroup(itemInnerContainerRect);
                {
                    ItemInner(slotItems[8], 9);
                }
                GUI.EndGroup();
                GUI.BeginGroup(itemTransitionContainerRect);
                {
                    ItemInner(transitionItems[8], 0);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();

            EquippedConsumables();
        }
        GUI.EndGroup();

        #region navigation arrows
        
        if (cur_page > 1)
        {
            if (goodButton(arrowPrevRect, "", arrowPrevStyle))
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
        if (cur_page < max_page)
        {
            if (goodButton(arrowNextRect, "", arrowNextStyle))
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
        }
        
        #endregion


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

            if (goodButton(itemPosCenter, "", activeSkin.button) && !show_popup)
            {
                selected_item = index;
                show_popup = true;
                //InventoryManager.instance.EquipItem(item);
            }
        }
    }

    void EquippedConsumables()
    {
        GUI.DrawTexture(backpackRect, backpackTexture);
        
        GUI.BeginGroup(consumable1Rect);
        {
            if (InventoryManager.instance.equippedConsumables[0].itemId != "empty")
            {
                GUI.DrawTexture(consumableIconRect, itemFilledTexture);
                GUI.DrawTexture(consumableInnerRect, InventoryManager.instance.equippedConsumables[0].icon);
                if(goodButton(consumableIconRect,"",activeSkin.button))
                {
                    InventoryManager.instance.UnequipItem(0);
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
                if (goodButton(consumableIconRect, "", activeSkin.button))
                {
                    InventoryManager.instance.UnequipItem(1);
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
                if (goodButton(consumableIconRect, "", activeSkin.button))
                {
                    InventoryManager.instance.UnequipItem(2);
                }
            }
            else
            {
                GUI.DrawTexture(consumableIconRect, itemEmptyTexture);
            }
        }
        GUI.EndGroup();
    }

    void EquipmentTriangle()
    {
        GUI.BeginGroup(eqTriangleContainerRect);
        {
            GUI.DrawTexture(triangleRect, triangleTexture);
            
            GUI.BeginGroup(eqSlotHeadRect);
            {
                if (InventoryManager.instance.equippedHead.itemId != "empty")
                {
                    GUI.DrawTexture(eqSlotIconRect, gearHeadTexture);
                    GUI.DrawTexture(eqSlotInnerRect, InventoryManager.instance.equippedHead.icon);
                    if (goodButton(eqSlotIconRect, "", activeSkin.button))
                    {
                        InventoryManager.instance.UnequipHead();
                    }
                }
                else
                {
                    GUI.DrawTexture(eqSlotIconRect, gearEmptyTexture);
                }
            }
            GUI.EndGroup();

            GUI.BeginGroup(eqSlotBodyRect);
            {
                if (InventoryManager.instance.equippedBody.itemId != "empty")
                {
                    GUI.DrawTexture(eqSlotIconRect, gearBodyTexture);
                    GUI.DrawTexture(eqSlotInnerRect, InventoryManager.instance.equippedBody.icon);
                    if (goodButton(eqSlotIconRect, "", activeSkin.button))
                    {
                        InventoryManager.instance.UnequipBody();
                    }
                }
                else
                {
                    GUI.DrawTexture(eqSlotIconRect, gearEmptyTexture);
                }
            }
            GUI.EndGroup();

            GUI.BeginGroup(eqSlotLegsRect);
            {
                if (InventoryManager.instance.equippedLegs.itemId != "empty")
                {
                    GUI.DrawTexture(eqSlotIconRect, gearLegsTexture);
                    GUI.DrawTexture(eqSlotInnerRect, InventoryManager.instance.equippedLegs.icon);
                    if (goodButton(eqSlotIconRect, "", activeSkin.button))
                    {
                        InventoryManager.instance.UnequipLegs();
                    }
                }
                else
                {
                    GUI.DrawTexture(eqSlotIconRect, gearEmptyTexture);
                }
            }
            GUI.EndGroup();
        }
        GUI.EndGroup();
    }

    void PopupConfirmation(Item item)
    {
        GUI.color = new Color(0f, 0f, 0f, 0.8f);
        GUI.DrawTexture(containerRect, shadingStyle.normal.background);
        GUI.color = Color.white;

        GUI.BeginGroup(popupRect);
        {
            GUI.Box(popupBgRect, "", popupBgStyle);
            if (item.icon != null)
            {
                GUI.DrawTexture(popupPictureRect, item.icon);
            }
            else
            {
                GUI.DrawTexture(popupPictureRect, tempIcon);
            }

            GUI.Label(popupLabelRect, "Equip " + item.name + "?", popupLabelStyle);
            if (goodButton(popupConfirmButtonRect, "Equip", popupBtnStyle))
            {
                InventoryManager.instance.EquipItem(item);
            }
        }
        GUI.EndGroup();

        if (goodButton(popupCancelButtonRect, "", popupCancelStyle))
        {
            show_popup = false;
            selected_item = 0;
        }
    }

    #endregion

    void NextPage()
    {
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

    void TransitionItems(List<Item> items)
    {
        int numberOfItems = items.Count;
        max_page = (numberOfItems + 1) / 9 + 1;
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

    void ChangeCategory()
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

    void OnAnimationComplete()
    {
        itemInnerContainerRect = itemPosCenter;
        itemTransitionContainerRect = itemPosBottom;

        for (int i = 0; i < slotItems.Length; i++)
        {
            slotItems[i] = transitionItems[i];
        }
    }

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
