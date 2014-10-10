using Soomla;
using Soomla.Store;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum CurrencyType { Dollar, GTP, NTP };
public enum ItemType { eq_head, eq_body, eq_legs, item_consumable, item_instant, other };

[Serializable]
public class Item
{
    public string itemId; // item ID
    public Texture2D icon; // icon of the item
    public string name; // name of the item
    public string description; // description of the item
    public CurrencyType currency; // currency used to buy the item
    public int price; // price of the item (in the shop)
    public double dollarPrice; // price in dollars
    public ItemType type; // item type
    public int balance; // number owned

    public Item()
    {
        itemId = "empty";
    }
}

[Serializable]
public class ShopItem
{
    void test()
    {
        //StoreInventory.BuyItem(test.ItemId);
    }
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    
    // currencies
    public int ntp;
    public int gtp;

    // Items cache
    public Dictionary<string,Item> equipmentsHead = new Dictionary<string,Item>();
    public Dictionary<string, Item> equipmentsBody = new Dictionary<string, Item>();
    public Dictionary<string, Item> equipmentsLegs = new Dictionary<string, Item>();
    public Dictionary<string, Item> itemsConsumable = new Dictionary<string, Item>();

    // List that stores equipped items (defaults to empty on startup, until initialization funtion runs)
    public Item equippedHead = new Item();
    public Item equippedBody = new Item();
    public Item equippedLegs = new Item();

    // Variables that store items inside the bag (number of bag slots retrieved from database)
    public Item[] equippedConsumables;
   
    void Awake()
    {
        // Make sure there is only 1 instance of this class.
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () 
    {
        // Initialise the shop
        SoomlaStore.Initialize(new CrapTrapAssets());
        // Initialise currency balance
        InitializeCurrencies();
        // Initialise shop items
        InitializeShopItems();
        // Check for equipped items
        InitializeEquippedGear();
        // Initialize Bag
        InitializeBag();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
    
    #region Initialization
    void InitializeBag()
    {
        // Initialise number of bag slots
        equippedConsumables = new Item[Game.instance.bagSlots];

        for (int i = 0; i < Game.instance.bagSlots; i++)
        {
            if (Game.instance.bag[i] != null)
            {
                equippedConsumables[i] = itemsConsumable[Game.instance.bag[i]];
            }
        }
    }

    void InitializeEquippedGear()
    {
        // Check for equipped gear (head)
        foreach (Item gear in equipmentsHead.Values)
        {
            if (StoreInventory.IsVirtualGoodEquipped(gear.itemId))
            {
                equippedHead = gear;
                break;
            }
        }

        // Check for equipped gear (body)
        foreach (Item gear in equipmentsBody.Values)
        {
            if (StoreInventory.IsVirtualGoodEquipped(gear.itemId))
            {
                equippedBody = gear;
                break;
            }
        }
        
        // Check for equipped gear (legs)
        foreach (Item gear in equipmentsLegs.Values)
        {
            if (StoreInventory.IsVirtualGoodEquipped(gear.itemId))
            {
                equippedLegs = gear;
                break;
            }
        }

    }

    void InitializeCurrencies()
    {
        UpdateCurrency();
    }

    void InitializeShopItems()
    {
        VirtualGood[] goods;
        #region Equipment Head

        goods = CrapTrapAssets.GetSpecificGoods(ItemType.eq_head);
        foreach (VirtualGood item in goods)
        {
            equipmentsHead.Add(item.ItemId,ParseToItem(item,ItemType.eq_head));
        }

        #endregion
        #region Equipment Body

        goods = CrapTrapAssets.GetSpecificGoods(ItemType.eq_body);
        foreach (VirtualGood item in goods)
        {
            equipmentsBody.Add(item.ItemId, ParseToItem(item,ItemType.eq_body));
        }

        #endregion
        #region Equipment Legs

        goods = CrapTrapAssets.GetSpecificGoods(ItemType.eq_legs);
        foreach (VirtualGood item in goods)
        {
            equipmentsLegs.Add(item.ItemId, ParseToItem(item, ItemType.eq_legs));
        }

        #endregion
        #region Items Consumable

        goods = CrapTrapAssets.GetSpecificGoods(ItemType.item_consumable);
        foreach (VirtualGood item in goods)
        {
            itemsConsumable.Add(item.ItemId, ParseToItem(item, ItemType.item_consumable));
        }

        #endregion
        #region Items Instant Use

        goods = CrapTrapAssets.GetSpecificGoods(ItemType.item_instant);
        foreach (VirtualGood item in goods)
        {
            itemsConsumable.Add(item.ItemId, ParseToItem(item, ItemType.item_instant));
        }

        #endregion
    }

    // Function to parse a virtual good into an Item object
    Item ParseToItem(VirtualGood item, ItemType type)
    {
        // Create a new item object
        Item new_item = new Item();

        // retrieve the id, name and description
        new_item.itemId = item.ItemId;
        new_item.name = item.Name;
        new_item.description = item.Description;

        // place the sprite of the item
        new_item.icon = (Texture2D)Resources.Load(item.ItemId);

        // retrieve quantity owned
        new_item.balance = StoreInventory.GetItemBalance(item.ItemId);

        // retrieve the currency type and price
        if (item.PurchaseType is PurchaseWithVirtualItem &&
           ((PurchaseWithVirtualItem)item.PurchaseType).ItemId == CrapTrapAssets.NORMAL_TOILET_PAPER_ID)
        {
            new_item.currency = CurrencyType.NTP;
            new_item.price = ((PurchaseWithVirtualItem)item.PurchaseType).Amount;
        }
        else if (item.PurchaseType is PurchaseWithVirtualItem &&
                ((PurchaseWithVirtualItem)item.PurchaseType).ItemId == CrapTrapAssets.GOLDEN_TOILET_PAPER_ID)
        {
            new_item.currency = CurrencyType.GTP;
            new_item.price = ((PurchaseWithVirtualItem)item.PurchaseType).Amount;
        }
        else if (item.PurchaseType is PurchaseWithMarket)
        {
            new_item.currency = CurrencyType.Dollar;
            new_item.dollarPrice = ((PurchaseWithMarket)item.PurchaseType).MarketItem.Price;
        }

        // set the type
        new_item.type = type;

        return new_item;
    }

    #endregion
    #region Methods

    void EquipItem(Item equipment)
    {
        // Tell soomla that we are equipping the specified item
        StoreInventory.EquipVirtualGood(equipment.itemId);
        
        // Update the cache
        if (equipment.type == ItemType.eq_head)
        {
            equippedHead = equipment;
        }
        else if (equipment.type == ItemType.eq_body)
        {
            equippedBody = equipment;
        }
        else if (equipment.type == ItemType.eq_legs)
        {
            equippedLegs = equipment;
        }
        else if (equipment.type == ItemType.item_consumable)
        {
        }
    }

    void ConsumeItem(int itemSlot)
    {
        var selectedItem = equippedConsumables[itemSlot];

        #region Charcoal

        // Charcoal rank 1
        if (selectedItem.itemId == CrapTrapAssets.CONSUMABLE_CHARCOAL_1_ID)
        {
            // Extend timer by 20 seconds
            MainGameController.instance.timeElapsed = MainGameController.instance.timeElapsed - 20;

            // Make sure it doesn't go negative
            if (MainGameController.instance.timeElapsed < 0) MainGameController.instance.timeElapsed = 0;
        }

        // Charcoal rank 2
        if (selectedItem.itemId == CrapTrapAssets.CONSUMABLE_CHARCOAL_2_ID)
        {
            // Extend timer by 50 seconds
            MainGameController.instance.timeElapsed = MainGameController.instance.timeElapsed - 50;

            // make sure it doesn't go negative
            if (MainGameController.instance.timeElapsed < 0) MainGameController.instance.timeElapsed = 0;
        }

        // Charcoal rank 3
        if (selectedItem.itemId == CrapTrapAssets.CONSUMABLE_CHARCOAL_3_ID)
        {
            // Extend timer by 90 seconds
            MainGameController.instance.timeElapsed = MainGameController.instance.timeElapsed - 90;

            // make sure it doesn't go negative
            if (MainGameController.instance.timeElapsed < 0) MainGameController.instance.timeElapsed = 0;
        }

        #endregion
        #region diapers

        // Diapers rank 1
        if (selectedItem.itemId == CrapTrapAssets.CONSUMABLE_DIAPERS_1_ID)
        {
            // Revives player and extends the timer by 10% of stage time (or minimum of 20 seconds)
            var timeExtension = MainGameController.instance.maxTime * 0.1f;

            if (timeExtension < 20)
            {
                // Extend timer by 20 seconds
                MainGameController.instance.timeElapsed = MainGameController.instance.timeElapsed - 20;
            }
            else
            {
                // Extend timer by 10% of total stage time
                MainGameController.instance.timeElapsed = MainGameController.instance.timeElapsed - timeExtension;
            }
        }

        // Diapers rank 2
        if (selectedItem.itemId == CrapTrapAssets.CONSUMABLE_DIAPERS_2_ID)
        {
            // Revives player and extends the timer by 30% of stage time (or minimum of 50 seconds)
            var timeExtension = MainGameController.instance.maxTime * 0.3f;

            if (timeExtension < 50)
            {
                // Extend timer by 20 seconds
                MainGameController.instance.timeElapsed = MainGameController.instance.timeElapsed - 50;
            }
            else
            {
                // Extend timer by 30% of total stage time
                MainGameController.instance.timeElapsed = MainGameController.instance.timeElapsed - timeExtension;
            }
        }

        // Diapers rank 2
        if (selectedItem.itemId == CrapTrapAssets.CONSUMABLE_DIAPERS_3_ID)
        {
            // Revives player and extends the timer by 50% of stage time (or minimum of 90 seconds)
            var timeExtension = MainGameController.instance.maxTime * 0.5f;

            if (timeExtension < 90)
            {
                // Extend timer by 20 seconds
                MainGameController.instance.timeElapsed = MainGameController.instance.timeElapsed - 90;
            }
            else
            {
                // Extend timer by 50% of total stage time
                MainGameController.instance.timeElapsed = MainGameController.instance.timeElapsed - timeExtension;
            }
        }

        #endregion

        // update total number of items in inventory
        selectedItem.balance--;
        itemsConsumable[selectedItem.itemId].balance--;
    }

    public void UpdateCurrency()
    {
        ntp = StoreInventory.GetItemBalance(CrapTrapAssets.NORMAL_TOILET_PAPER_ID);
        gtp = StoreInventory.GetItemBalance(CrapTrapAssets.GOLDEN_TOILET_PAPER_ID);
    }

    #endregion
}
