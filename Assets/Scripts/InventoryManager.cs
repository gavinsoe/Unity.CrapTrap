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

    void Awake()
    {
        // Don't destroy the cript when changing scenese
        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
	void Start () {
        // Initialise the shop
        SoomlaStore.Initialize(new CrapTrapAssets());
        // Initialise currency balance
        InitializeCurrencies();
        // Initialise shop items
        InitializeShopItems();
        // Check for equipped items
        InitializeEquippedGear();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    #region Initialization

    void InitializeInventory()
    {
        // TODO :: Need to implement
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

    public void UpdateCurrency()
    {
        ntp = StoreInventory.GetItemBalance(CrapTrapAssets.NORMAL_TOILET_PAPER_ID);
        gtp = StoreInventory.GetItemBalance(CrapTrapAssets.GOLDEN_TOILET_PAPER_ID);
    }

    #endregion
}
