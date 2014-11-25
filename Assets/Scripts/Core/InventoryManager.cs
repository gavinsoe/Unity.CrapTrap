using Soomla;
using Soomla.Store;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum CurrencyType { Dollar, GTP, NTP };
public enum ItemType { eq_head, eq_body, eq_legs, item_consumable, item_instant, other };

[Serializable]
public class Item : IComparable<Item>
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

    public int CompareTo(Item that)
    {
        return this.itemId.CompareTo(that.itemId);
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
    private Texture ntpTexture;
    public int gtp;
    private Texture gtpTexture;

    // Items cache
    public Dictionary<string, Item> allItems = new Dictionary<string, Item>();
    public Dictionary<string, Item> equipmentsHead = new Dictionary<string,Item>();
    public Dictionary<string, Item> equipmentsBody = new Dictionary<string, Item>();
    public Dictionary<string, Item> equipmentsLegs = new Dictionary<string, Item>();
    public Dictionary<string, Item> itemsConsumable = new Dictionary<string, Item>();
    public Dictionary<string, Item> itemsOther = new Dictionary<string, Item>();

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
        InitializeItemDictionary();
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
        for (int i = 0; i < Game.instance.bagSlots; i++)
        {
            if (!String.IsNullOrEmpty(Game.instance.bag[i]))
            {
                equippedConsumables[i] = itemsConsumable[Game.instance.bag[i]];
                itemsConsumable[Game.instance.bag[i]].balance--;
            }
            else
            {
                equippedConsumables[i] = new Item();
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

    void InitializeItemDictionary()
    {
        VirtualGood[] goods;
        #region Equipment Head

        goods = CrapTrapAssets.GetSpecificGoods(ItemType.eq_head);
        foreach (VirtualGood item in goods)
        {
            equipmentsHead.Add(item.ItemId,ParseToItem(item,ItemType.eq_head));
            allItems.Add(item.ItemId, ParseToItem(item, ItemType.eq_head));
        }

        #endregion
        #region Equipment Body

        goods = CrapTrapAssets.GetSpecificGoods(ItemType.eq_body);
        foreach (VirtualGood item in goods)
        {
            equipmentsBody.Add(item.ItemId, ParseToItem(item,ItemType.eq_body));
            allItems.Add(item.ItemId, ParseToItem(item, ItemType.eq_body));
        }

        #endregion
        #region Equipment Legs

        goods = CrapTrapAssets.GetSpecificGoods(ItemType.eq_legs);
        foreach (VirtualGood item in goods)
        {
            equipmentsLegs.Add(item.ItemId, ParseToItem(item, ItemType.eq_legs));
            allItems.Add(item.ItemId, ParseToItem(item, ItemType.eq_legs));
        }

        #endregion
        #region Items Consumable

        goods = CrapTrapAssets.GetSpecificGoods(ItemType.item_consumable);
        foreach (VirtualGood item in goods)
        {
            itemsConsumable.Add(item.ItemId, ParseToItem(item, ItemType.item_consumable));
            allItems.Add(item.ItemId, ParseToItem(item, ItemType.item_consumable));
        }

        #endregion
        #region Items Instant Use

        goods = CrapTrapAssets.GetSpecificGoods(ItemType.item_instant);
        foreach (VirtualGood item in goods)
        {
            itemsConsumable.Add(item.ItemId, ParseToItem(item, ItemType.item_instant));
            allItems.Add(item.ItemId, ParseToItem(item, ItemType.item_instant));
        }

        #endregion
        #region other items

        goods = CrapTrapAssets.GetSpecificGoods(ItemType.other);
        foreach (VirtualGood item in goods)
        {
            itemsOther.Add(item.ItemId, ParseToItem(item, ItemType.other));
            allItems.Add(item.ItemId, ParseToItem(item, ItemType.other));
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
    #region Public Methods

    public void EquipItem(Item item)
    {
        // Tell soomla that we are equipping the specified item
        StoreInventory.EquipVirtualGood(item.itemId);
        
        // Update the cache
        if (item.type == ItemType.eq_head)
        {
            equippedHead = item;
            StoreInventory.EquipVirtualGood(item.itemId);
        }
        else if (item.type == ItemType.eq_body)
        {
            equippedBody = item;
            StoreInventory.EquipVirtualGood(item.itemId);
        }
        else if (item.type == ItemType.eq_legs)
        {
            equippedLegs = item;
            StoreInventory.EquipVirtualGood(item.itemId);
        }
        else if (item.type == ItemType.item_consumable)
        {
            for (int i = 0; i < Game.instance.bagSlots; i++)
            {
                if (equippedConsumables[i].itemId == "empty")
                {
                    equippedConsumables[i] = item;
                    itemsConsumable[item.itemId].balance--;
                    break;
                }
            }
        }
    }

    public void ConsumeItem(int itemSlot)
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

        // update total number of items in inventory
        selectedItem.balance--;
        itemsConsumable[selectedItem.itemId].balance--;
    }

    public void PurchaseAndUseEmergencyDiapers(string item_id)
    {
        #region Diapers

        // Diapers rank 1
        if (item_id == CrapTrapAssets.EMERGENCY_REVIVE_1_1_ID ||
            item_id == CrapTrapAssets.EMERGENCY_REVIVE_1_2_ID ||
            item_id == CrapTrapAssets.EMERGENCY_REVIVE_1_3_ID)
        {
            // Purchase the diaper
            StoreInventory.BuyItem(item_id);

            // Revives player and extends the timer by 10% of stage time (or minimum of 20 seconds)
            var timeExtension = MainGameController.instance.maxTime * 0.1f;

            // Extend timer by 10% of total stage time
            MainGameController.instance.Revive(timeExtension);
        }

        // Diapers rank 2
        else if (item_id == CrapTrapAssets.EMERGENCY_REVIVE_2_1_ID ||
                 item_id == CrapTrapAssets.EMERGENCY_REVIVE_2_2_ID ||
                 item_id == CrapTrapAssets.EMERGENCY_REVIVE_2_3_ID)
        {
            // Purchase the diaper
            StoreInventory.BuyItem(item_id);

            // Revives player and extends the timer by 30% of stage time (or minimum of 50 seconds)
            var timeExtension = MainGameController.instance.maxTime * 0.3f;

            // Extend timer by 30% of total stage time
            MainGameController.instance.Revive(timeExtension);
        }

        // Diapers rank 2
        else if (item_id == CrapTrapAssets.EMERGENCY_REVIVE_3_1_ID ||
                 item_id == CrapTrapAssets.EMERGENCY_REVIVE_3_2_ID ||
                 item_id == CrapTrapAssets.EMERGENCY_REVIVE_3_3_ID)
        {
            // Purchase the diaper
            StoreInventory.BuyItem(item_id);

            // Revives player and extends the timer by 50% of stage time (or minimum of 90 seconds)
            var timeExtension = MainGameController.instance.maxTime * 0.5f;

            // Extend timer by 50% of total stage time
            MainGameController.instance.Revive(timeExtension);
        }

        #endregion

    }

    public void UnequipItem(int itemSlot)
    {
        itemsConsumable[equippedConsumables[itemSlot].itemId].balance++;
        equippedConsumables[itemSlot] = new Item();
    }

    public void UnequipHead()
    {
        StoreInventory.UnEquipVirtualGood(equippedHead.itemId);
        equippedHead = new Item();
    }

    public void UnequipBody()
    {
        StoreInventory.UnEquipVirtualGood(equippedBody.itemId);
        equippedBody = new Item();
    }

    public void UnequipLegs()
    {
        StoreInventory.UnEquipVirtualGood(equippedLegs.itemId);
        equippedLegs = new Item();
    }

    public void UpdateCurrency()
    {
        ntp = StoreInventory.GetItemBalance(CrapTrapAssets.NORMAL_TOILET_PAPER_ID);
        gtp = StoreInventory.GetItemBalance(CrapTrapAssets.GOLDEN_TOILET_PAPER_ID);
    }

    public List<Item> GetOwnedEquipment(ItemType type)
    {
        if (type == ItemType.eq_head)
        {
            return (from h in equipmentsHead.Values
                    //where h.balance > 0
                    select h).ToList();
                    
        }
        else if (type == ItemType.eq_body)
        {
            return (from h in equipmentsBody.Values
                    //where h.balance > 0
                    select h).ToList();
        }
        else if (type == ItemType.eq_legs)
        {
            return (from h in equipmentsLegs.Values
                    //where h.balance > 0
                    select h).ToList();
        }
        else if (type == ItemType.item_consumable)
        {
            return (from h in itemsConsumable.Values
                    //where h.balance > 0
                    select h).ToList();
        }

        // Should ideally not end up here, but if it does, return empty list
        return new List<Item>();
    }

    public Texture lootItem(Loot loot)
    {
        StoreInventory.GiveItem(loot.itemID, loot.quantity);

        if (loot.itemID == CrapTrapAssets.NORMAL_TOILET_PAPER_ID)
        {
            ntp = ntp + loot.quantity;
            StoreInventory.GiveItem(CrapTrapAssets.NORMAL_TOILET_PAPER_ID, loot.quantity);
            return ntpTexture;
        }
        else if (loot.itemID == CrapTrapAssets.GOLDEN_TOILET_PAPER_ID)
        {
            gtp = gtp + loot.quantity;
            StoreInventory.GiveItem(CrapTrapAssets.GOLDEN_TOILET_PAPER_ID, loot.quantity);
            return gtpTexture;
        }
        else if (loot.itemType == ItemType.eq_head)
        {
            equipmentsHead[loot.itemID].balance = equipmentsHead[loot.itemID].balance + loot.quantity;
            return equipmentsHead[loot.itemID].icon;
        }
        else if (loot.itemType == ItemType.eq_body)
        {
            equipmentsBody[loot.itemID].balance = equipmentsBody[loot.itemID].balance + loot.quantity;
            return equipmentsBody[loot.itemID].icon;
        }
        else if (loot.itemType == ItemType.eq_legs)
        {
            equipmentsLegs[loot.itemID].balance = equipmentsLegs[loot.itemID].balance + loot.quantity;
            return equipmentsLegs[loot.itemID].icon;
        }
        else
        {
            itemsConsumable[loot.itemID].balance = itemsConsumable[loot.itemID].balance + loot.quantity;
            return itemsConsumable[loot.itemID].icon;
        }

    }

    public void AddNTP(int amount)
    {
        ntp = ntp + amount;
        StoreInventory.GiveItem(CrapTrapAssets.NORMAL_TOILET_PAPER_ID, amount);
    }

    #endregion
}
