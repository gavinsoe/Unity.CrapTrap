using Soomla;
using Soomla.Store;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum CurrencyType { Dollar, GTP, NTP };
public enum ItemType { eq_head, eq_body, eq_legs, item_consumable, item_instant, currency_pack, other };
public enum SetBonus { None, Explorer, Pirate, Tribal }

[System.Serializable]
public class Item : System.IComparable<Item>
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

[System.Serializable]
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
    public Texture ntpTexture;
    public int gtp;
    public Texture gtpTexture;

    // Items cache
    public Dictionary<string, Item> allItems = new Dictionary<string, Item>();
    public Dictionary<string, Item> equipmentsHead = new Dictionary<string, Item>();
    public Dictionary<string, Item> equipmentsBody = new Dictionary<string, Item>();
    public Dictionary<string, Item> equipmentsLegs = new Dictionary<string, Item>();
    public Dictionary<string, Item> itemsConsumable = new Dictionary<string, Item>();
    public Dictionary<string, Item> itemsOther = new Dictionary<string, Item>();

    // List that stores equipped items (defaults to empty on startup, until initialization funtion runs)
    public Item equippedHead = new Item();
    public Item equippedBody = new Item();
    public Item equippedLegs = new Item();

    // Active equipment set bonus
    public SetBonus setBonus;

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
    void Start()
    {
        // Initialise the shop
        SoomlaStore.Initialize(new CrapTrapAssets());
        SoomlaStore.StartIabServiceInBg();
        StoreEvents.OnMarketPurchase += onMarketPurchase;
        StoreEvents.OnItemPurchased += onItemPurchased;
        StoreEvents.OnGoodEquipped += onGoodEquipped;
        StoreEvents.OnGoodUnEquipped += onGoodUnequipped;
        StoreEvents.OnCurrencyBalanceChanged += onCurrencyBalanceChanged;

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
    void Update()
    {

    }

    #region Initialization

    void InitializeBag()
    {
        for (int i = 0; i < Game.instance.bagSlots; i++)
        {
            if (!System.String.IsNullOrEmpty(Game.instance.bag[i]))
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
            equipmentsHead.Add(item.ItemId, ParseToItem(item, ItemType.eq_head));
            allItems.Add(item.ItemId, ParseToItem(item, ItemType.eq_head));
        }

        #endregion
        #region Equipment Body

        goods = CrapTrapAssets.GetSpecificGoods(ItemType.eq_body);
        foreach (VirtualGood item in goods)
        {
            equipmentsBody.Add(item.ItemId, ParseToItem(item, ItemType.eq_body));
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

        foreach (VirtualCurrencyPack cp in CrapTrapAssets.GetCurrencyPacksCustom())
        {
            itemsConsumable.Add(cp.ItemId, ParseToItem(cp, ItemType.currency_pack));
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
           ((PurchaseWithVirtualItem)item.PurchaseType).TargetItemId == CrapTrapAssets.NORMAL_TOILET_PAPER_ID)
        {
            new_item.currency = CurrencyType.NTP;
            new_item.price = ((PurchaseWithVirtualItem)item.PurchaseType).Amount;
        }
        else if (item.PurchaseType is PurchaseWithVirtualItem &&
                ((PurchaseWithVirtualItem)item.PurchaseType).TargetItemId == CrapTrapAssets.GOLDEN_TOILET_PAPER_ID)
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

    Item ParseToItem(VirtualCurrencyPack cp, ItemType type)
    {
        // Create a new item object
        Item new_item = new Item();

        // retrieve the id, name and description
        new_item.itemId = cp.ItemId;
        new_item.name = cp.Name;
        new_item.description = cp.Description;

        // place the sprite of the item
        new_item.icon = (Texture2D)Resources.Load(cp.ItemId);

        // retrieve the currency type and price
        if (cp.PurchaseType is PurchaseWithVirtualItem &&
            ((PurchaseWithVirtualItem)cp.PurchaseType).TargetItemId == CrapTrapAssets.NORMAL_TOILET_PAPER_ID)
        {
            new_item.currency = CurrencyType.NTP;
            new_item.price = ((PurchaseWithVirtualItem)cp.PurchaseType).Amount;
        }
        else if (cp.PurchaseType is PurchaseWithVirtualItem &&
            ((PurchaseWithVirtualItem)cp.PurchaseType).TargetItemId == CrapTrapAssets.GOLDEN_TOILET_PAPER_ID)
        {
            new_item.currency = CurrencyType.GTP;
            new_item.price = ((PurchaseWithVirtualItem)cp.PurchaseType).Amount;
        }
        else if (cp.PurchaseType is PurchaseWithMarket)
        {
            new_item.currency = CurrencyType.Dollar;
            new_item.dollarPrice = ((PurchaseWithMarket)cp.PurchaseType).MarketItem.Price;
        }

        // set the type
        new_item.type = type;

        return new_item;
    }

    #endregion
    #region Public Methods

    public void EquipItem(Item item)
    {
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
            // Consume the item
            StoreInventory.TakeItem(selectedItem.itemId, 1);

            // Extend max time by 20 seconds
            MainGameController.instance.timeElapsed = MainGameController.instance.maxTime + 20;

            // Remove equipped item
            equippedConsumables[itemSlot] = new Item();

            // Update balance
            itemsConsumable[selectedItem.itemId].balance--;
        }

        // Charcoal rank 2
        if (selectedItem.itemId == CrapTrapAssets.CONSUMABLE_CHARCOAL_2_ID)
        {
            // Consume the item
            StoreInventory.TakeItem(selectedItem.itemId, 1);

            // Extend max time by 50 seconds
            MainGameController.instance.timeElapsed = MainGameController.instance.maxTime + 50;
            
            // Remove equipped item
            equippedConsumables[itemSlot] = new Item();

            // Update balance
            itemsConsumable[selectedItem.itemId].balance--;
        }

        // Charcoal rank 3
        if (selectedItem.itemId == CrapTrapAssets.CONSUMABLE_CHARCOAL_3_ID)
        {
            // Consume the item
            StoreInventory.TakeItem(selectedItem.itemId, 1);

            // Extend max time by 90 seconds
            MainGameController.instance.timeElapsed = MainGameController.instance.maxTime + 90;

            // Remove equipped item
            equippedConsumables[itemSlot] = new Item();

            // Update balance
            itemsConsumable[selectedItem.itemId].balance--;
        }

        #endregion
    }

    // Called when player fails the stage
    public void ConsumeCharms()
    {
        for (int i = 0; i < equippedConsumables.Length; i++)
        {
            if (equippedConsumables[i].itemId == CrapTrapAssets.CONSUMABLE_LUCKY_CHARM_ID)
            {
                equippedConsumables[i] = new Item();
                StoreInventory.TakeItem(CrapTrapAssets.CONSUMABLE_LUCKY_CHARM_ID, 1);
            }
        }
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

    public void UpdateItemDictionary()
    {
        #region Equipment Head

        foreach (Item item in equipmentsHead.Values)
        {
            item.balance = StoreInventory.GetItemBalance(item.itemId);
        }

        #endregion
        #region Equipment Body

        foreach (Item item in equipmentsBody.Values)
        {
            item.balance = StoreInventory.GetItemBalance(item.itemId);
        }

        #endregion
        #region Equipment Legs

        foreach (Item item in equipmentsLegs.Values)
        {
            item.balance = StoreInventory.GetItemBalance(item.itemId);
        }

        #endregion
        #region Items Consumable

        foreach (Item item in itemsConsumable.Values)
        {
            if (item.type != ItemType.currency_pack)
            {
                item.balance = StoreInventory.GetItemBalance(item.itemId);
            }
        }

        #endregion
        #region other items

        foreach (Item item in itemsOther.Values)
        {
            if (item.type != ItemType.currency_pack)
            {
                item.balance = StoreInventory.GetItemBalance(item.itemId);
            }
        }

        #endregion

        foreach (Item item in allItems.Values)
        {
            if (item.type != ItemType.currency_pack)
            {
                item.balance = StoreInventory.GetItemBalance(item.itemId);
            }
        }
    }

    public void UpdateSetBonus()
    {
        if (equippedHead.itemId == CrapTrapAssets.EQ_HEAD_SET_EXPLORER_ID &&
            equippedBody.itemId == CrapTrapAssets.EQ_BODY_SET_EXPLORER_ID &&
            equippedLegs.itemId == CrapTrapAssets.EQ_LEGS_SET_EXPLORER_ID)
        {
            setBonus = SetBonus.Explorer;
        }
        else if (equippedHead.itemId == CrapTrapAssets.EQ_HEAD_SET_PIRATE_ID &&
                 equippedBody.itemId == CrapTrapAssets.EQ_BODY_SET_PIRATE_ID &&
                 equippedLegs.itemId == CrapTrapAssets.EQ_LEGS_SET_PIRATE_ID)
        {
            setBonus = SetBonus.Pirate;
        }
        else if (equippedHead.itemId == CrapTrapAssets.EQ_HEAD_SET_TRIBAL_ID &&
                 equippedBody.itemId == CrapTrapAssets.EQ_BODY_SET_TRIBAL_ID &&
                 equippedLegs.itemId == CrapTrapAssets.EQ_LEGS_SET_TRIBAL_ID)
        {
            setBonus = SetBonus.Tribal;
        }
        else
        {
            setBonus = SetBonus.None;
        }
    }

    public List<Item> GetOwnedEquipment(ItemType type)
    {
        if (type == ItemType.eq_head)
        {
            return (from h in equipmentsHead.Values
                    where h.balance > 0
                    select h).ToList();

        }
        else if (type == ItemType.eq_body)
        {
            return (from h in equipmentsBody.Values
                    where h.balance > 0
                    select h).ToList();
        }
        else if (type == ItemType.eq_legs)
        {
            return (from h in equipmentsLegs.Values
                    where h.balance > 0
                    select h).ToList();
        }
        else if (type == ItemType.item_consumable)
        {
            return (from h in itemsConsumable.Values
                    where h.balance > 0
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

    // Get amount of bonus time based on equipment
    public float GetBonusTime()
    {
        float bonusTotal = 0;

        #region headgear bonus

        if (equippedHead.itemId == CrapTrapAssets.EQ_HEAD_APPLE_ARROW_ID ||
            equippedHead.itemId == CrapTrapAssets.EQ_HEAD_HELMET_BLUE_ID ||
            equippedHead.itemId == CrapTrapAssets.EQ_HEAD_PAPERBAG_ID)
        {
            // 2% bonus time
            bonusTotal += 0.02f;
        }
        else if (equippedHead.itemId == CrapTrapAssets.EQ_HEAD_NEKOMIMI_ID ||
                 equippedHead.itemId == CrapTrapAssets.EQ_HEAD_SHARK_ID)
        {
            // 3% bonus time
            bonusTotal += 0.03f;
        }
        else if (equippedHead.itemId == CrapTrapAssets.EQ_HEAD_SET_EXPLORER_ID ||
                 equippedHead.itemId == CrapTrapAssets.EQ_HEAD_SET_PIRATE_ID ||
                 equippedHead.itemId == CrapTrapAssets.EQ_HEAD_SET_TRIBAL_ID ||
                 equippedHead.itemId == CrapTrapAssets.EQ_HEAD_SET_DIVER_ID)
        {
            // 5% bonus time
            bonusTotal += 0.05f;
        }

        #endregion
        #region body gear bonus

        if (equippedHead.itemId == CrapTrapAssets.EQ_BODY_BOXING_GLOVES_ID ||
            equippedHead.itemId == CrapTrapAssets.EQ_BODY_SUIT_ID ||
            equippedHead.itemId == CrapTrapAssets.EQ_BODY_TATTOO_ID)
        {
            // 2% bonus time
            bonusTotal += 0.02f;
        }
        else if (equippedHead.itemId == CrapTrapAssets.EQ_BODY_BARREL_ID ||
                 equippedHead.itemId == CrapTrapAssets.EQ_BODY_KARATEGI_ID ||
                 equippedHead.itemId == CrapTrapAssets.EQ_BODY_LABCOAT_KRIEGER_ID)
        {
            // 3% bonus time
            bonusTotal += 0.03f;
        }
        else if (equippedHead.itemId == CrapTrapAssets.EQ_BODY_SET_EXPLORER_ID ||
                 equippedHead.itemId == CrapTrapAssets.EQ_BODY_SET_PIRATE_ID ||
                 equippedHead.itemId == CrapTrapAssets.EQ_BODY_SET_TRIBAL_ID ||
                 equippedHead.itemId == CrapTrapAssets.EQ_BODY_SET_DIVER_ID)
        {
            // 5% bonus time
            bonusTotal += 0.05f;
        }

        #endregion
        #region legs gear bonus

        if (equippedHead.itemId == CrapTrapAssets.EQ_LEGS_ARMY_ID ||
            equippedHead.itemId == CrapTrapAssets.EQ_LEGS_COWBOY_ID ||
            equippedHead.itemId == CrapTrapAssets.EQ_LEGS_SUIT_ID)
        {
            // 2% bonus time
            bonusTotal += 0.02f;
        }
        else if (equippedHead.itemId == CrapTrapAssets.EQ_LEGS_SNEAKERS_ID ||
                 equippedHead.itemId == CrapTrapAssets.EQ_LEGS_HERMES_ID ||
                 equippedHead.itemId == CrapTrapAssets.EQ_LEGS_BALLET_ID)
        {
            // 3% bonus time
            bonusTotal += 0.03f;
        }
        else if (equippedHead.itemId == CrapTrapAssets.EQ_LEGS_SET_EXPLORER_ID ||
                 equippedHead.itemId == CrapTrapAssets.EQ_LEGS_SET_PIRATE_ID ||
                 equippedHead.itemId == CrapTrapAssets.EQ_LEGS_SET_TRIBAL_ID ||
                 equippedHead.itemId == CrapTrapAssets.EQ_LEGS_SET_DIVER_ID)
        {
            // 5% bonus time
            bonusTotal += 0.05f;
        }

        #endregion

        return bonusTotal;
    }
    
    #endregion
    #region Market event handlers

    /// <summary>
    /// Handles a currency balance changed event.
    /// </summary>
    /// <param name="virtualCurrency">Virtual currency whose balance has changed.</param>
    /// <param name="balance">Balance of the given virtual currency.</param>
    /// <param name="amountAdded">Amount added to the balance.</param>
    public void onCurrencyBalanceChanged(VirtualCurrency virtualCurrency, int balance, int amountAdded)
    {
        UpdateCurrency();
    }

    /// <summary>
    /// Handles a good equipped event.
    /// </summary>
    /// <param name="good">Equippable virtual good.</param>
    public void onGoodEquipped(EquippableVG good)
    {
        UpdateItemDictionary();
        UpdateSetBonus();
    }

    /// <summary>
    /// Handles a good unequipped event.
    /// </summary>
    /// <param name="good">Equippable virtual good.</param>
    public void onGoodUnequipped(EquippableVG good)
    {
        UpdateItemDictionary();
        UpdateSetBonus();
    }

    /// <summary>
    /// Handles an item purchase event.
    /// </summary>
    /// <param name="pvi">Purchasable virtual item.</param>
    public void onItemPurchased(PurchasableVirtualItem pvi, string payload)
    {
        if (pvi.ItemId == CrapTrapAssets.CONSUMABLE_CHARCOAL_1_10PACK_ID)
        {
            Game.instance.stats[Stat.itemsBought] += 10;
        }
        else if (pvi.ItemId == CrapTrapAssets.CONSUMABLE_CHARCOAL_1_5PACK_ID)
        {
            Game.instance.stats[Stat.itemsBought] += 5;
        }
        else if (pvi.ItemId == CrapTrapAssets.CONSUMABLE_CHARCOAL_2_10PACK_ID)
        {
            Game.instance.stats[Stat.itemsBought] += 10;
        }
        else if (pvi.ItemId == CrapTrapAssets.CONSUMABLE_CHARCOAL_2_5PACK_ID)
        {
            Game.instance.stats[Stat.itemsBought] += 5;
        }
        else if (pvi.ItemId == CrapTrapAssets.CONSUMABLE_CHARCOAL_3_10PACK_ID)
        {
            Game.instance.stats[Stat.itemsBought] += 10;
        }
        else if (pvi.ItemId == CrapTrapAssets.CONSUMABLE_CHARCOAL_3_5PACK_ID)
        {
            Game.instance.stats[Stat.itemsBought] += 5;
        }
        else if (pvi.ItemId == CrapTrapAssets.PACK_1_NTP_ID)
        {
            Game.instance.stats[Stat.itemsBought] += 25;
        }
        else if (pvi.ItemId == CrapTrapAssets.PACK_2_NTP_ID)
        {
            Game.instance.stats[Stat.itemsBought] += 50;
        }
        else if (pvi.ItemId == CrapTrapAssets.PACK_3_NTP_ID)
        {
            Game.instance.stats[Stat.itemsBought] += 125;
        }
        else if (pvi.ItemId == CrapTrapAssets.PACK_4_NTP_ID)
        {
            Game.instance.stats[Stat.itemsBought] += 250;
        }
        else if (pvi.ItemId == CrapTrapAssets.PACK_5_NTP_ID)
        {
            Game.instance.stats[Stat.itemsBought] += 625;
        }
        else
        {
            Game.instance.stats[Stat.itemsBought] += 1;
        }

        InventoryManager.instance.UpdateItemDictionary();
    }

    public void onMarketPurchase(PurchasableVirtualItem pvi, string payload, Dictionary<string, string> extra)
    {
        // pvi is the PurchasableVirtualItem that was just purchased
        // payload is a text that you can give when you initiate the purchase operation and you want to receive back upon completion
        // extra will contain platform specific information about the market purchase.
        //      Android: The "extra" dictionary will contain "orderId" and "purchaseToken".
        //      iOS: The "extra" dictionary will contain "receipt" and "token".

        #region if using emergency diapers

        // Diapers rank 1
        if (pvi.ItemId == CrapTrapAssets.EMERGENCY_REVIVE_1_1_ID ||
            pvi.ItemId == CrapTrapAssets.EMERGENCY_REVIVE_1_2_ID ||
            pvi.ItemId == CrapTrapAssets.EMERGENCY_REVIVE_1_3_ID)
        {
            StoreInventory.TakeItem(pvi.ItemId, 1);

            // Revives player and extends the timer by 10% of stage time (or minimum of 20 seconds)
            var timeExtension = MainGameController.instance.maxTime * 0.1f;

            // Extend timer by 10% of total stage time
            MainGameController.instance.Revive(timeExtension);
        }

        // Diapers rank 2
        else if (pvi.ItemId == CrapTrapAssets.EMERGENCY_REVIVE_2_1_ID ||
                    pvi.ItemId == CrapTrapAssets.EMERGENCY_REVIVE_2_2_ID ||
                    pvi.ItemId == CrapTrapAssets.EMERGENCY_REVIVE_2_3_ID)
        {
            // Revives player and extends the timer by 30% of stage time (or minimum of 50 seconds)
            var timeExtension = MainGameController.instance.maxTime * 0.3f;

            // Extend timer by 30% of total stage time
            MainGameController.instance.Revive(timeExtension);
        }

        // Diapers rank 2
        else if (pvi.ItemId == CrapTrapAssets.EMERGENCY_REVIVE_3_1_ID ||
                    pvi.ItemId == CrapTrapAssets.EMERGENCY_REVIVE_3_2_ID ||
                    pvi.ItemId == CrapTrapAssets.EMERGENCY_REVIVE_3_3_ID)
        {
            // Revives player and extends the timer by 50% of stage time (or minimum of 90 seconds)
            var timeExtension = MainGameController.instance.maxTime * 0.5f;

            // Extend timer by 50% of total stage time
            MainGameController.instance.Revive(timeExtension);
        }

        #endregion

        // ... your game specific implementation here ...

        #region if buying gtp
        // If buying GTP
        if (pvi.ItemId == CrapTrapAssets.PACK_1_GTP_ID)
        {
            Game.instance.stats[Stat.gtpBought] += 10;
            Game.instance.stats[Stat.itemsBought] += 10;
        }
        else if (pvi.ItemId == CrapTrapAssets.PACK_2_GTP_ID)
        {
            Game.instance.stats[Stat.gtpBought] += 20;
            Game.instance.stats[Stat.itemsBought] += 20;
        }
        else if (pvi.ItemId == CrapTrapAssets.PACK_3_GTP_ID)
        {
            Game.instance.stats[Stat.gtpBought] += 55;
            Game.instance.stats[Stat.itemsBought] += 55;
        }
        else if (pvi.ItemId == CrapTrapAssets.PACK_4_GTP_ID)
        {
            Game.instance.stats[Stat.gtpBought] += 112;
            Game.instance.stats[Stat.itemsBought] += 112;
        }
        else if (pvi.ItemId == CrapTrapAssets.PACK_5_GTP_ID)
        {
            Game.instance.stats[Stat.gtpBought] += 290;
            Game.instance.stats[Stat.itemsBought] += 290;
        }
        else if (pvi.ItemId == CrapTrapAssets.PACK_6_GTP_ID)
        {
            Game.instance.stats[Stat.gtpBought] += 600;
            Game.instance.stats[Stat.itemsBought] += 600;
        }
        #endregion

        UpdateCurrency();
        UpdateItemDictionary();
    }

    #endregion
}
