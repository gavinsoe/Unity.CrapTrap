using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla;
using Soomla.Store;

public class CrapTrapAssets : IStoreAssets
{
    private static List<string> eq_head_list  = new List<string>();
    private List<string> eq_body_list  = new List<string>();
    private List<string> eq_hands_list = new List<string>();
    private List<string> eq_legs_list  = new List<string>();
    private List<string> eq_feet_list  = new List<string>();

    public int GetVersion()
    {
        return 0;
    }

    public VirtualCurrency[] GetCurrencies()
    {
        return new VirtualCurrency[] { 
            TOILET_PAPER, 
            GOLDEN_TOILET_PAPER
        };
    }

    public VirtualGood[] GetGoods()
    {
        return new VirtualGood[] {
            EQ_HEAD_SET_EXPLORER,
            EQ_HEAD_SET_PIRATE,
            EQ_BODY_SET_EXPLORER,
            EQ_BODY_SET_PIRATE,
            EQ_HANDS_SET_EXPLORER,
            EQ_HANDS_SET_PIRATE,
            EQ_LEGS_SET_EXPLORER,
            EQ_LEGS_SET_PIRATE,
            EQ_FEET_SET_EXPLORER,
            EQ_FEET_SET_PIRATE,
            CONSUMABLE_CHARCOAL_1,
            CONSUMABLE_CHARCOAL_2,
            CONSUMABLE_CHARCOAL_3,
            CONSUMABLE_DIAPERS_1,
            CONSUMABLE_DIAPERS_2,
            CONSUMABLE_DIAPERS_3,
            CONSUMABLE_DETERGENT,
            USE_PORTABLE_POTTY_1,
            USE_PORTABLE_POTTY_2,
            USE_PORTABLE_POTTY_3,
            EMERGENCY_REVIVE_1_1,
            EMERGENCY_REVIVE_1_2,
            EMERGENCY_REVIVE_1_3,
            EMERGENCY_REVIVE_2_1,
            EMERGENCY_REVIVE_2_2,
            EMERGENCY_REVIVE_2_3,
            EMERGENCY_REVIVE_3_1,
            EMERGENCY_REVIVE_3_2,
            EMERGENCY_REVIVE_3_3
        };
    }

    public VirtualCurrencyPack[] GetCurrencyPacks()
    {
        return new VirtualCurrencyPack[] {  
            PACK_NTP_10,
            PACK_NTP_50,
            PACK_NTP_100,
            PACK_GTP_10,
            PACK_GTP_50,
            PACK_GTP_100
        };
    }

    public VirtualCategory[] GetCategories()
    {
        return new VirtualCategory[] { 
            EQUIP_HEAD_CATEGORY,
            EQUIP_BODY_CATEGORY,
            EQUIP_HANDS_CATEGORY,
            EQUIP_LEGS_CATEGORY,
            EQUIP_FEET_CATEGORY
        };
    }

    public NonConsumableItem[] GetNonConsumableItems()
    {
        return new NonConsumableItem[] { NO_ADDS_NONCONS };
    }

    public static VirtualGood[] GetSpecificGoods(ItemType type)
    {
        if (type == ItemType.eq_head)
        {
            return new VirtualGood[] {
                EQ_HEAD_SET_EXPLORER,
                EQ_HEAD_SET_PIRATE
            };
        }
        else if (type == ItemType.eq_body)
        {
            return new VirtualGood[] { 
                EQ_BODY_SET_EXPLORER,
                EQ_BODY_SET_PIRATE
            };
        }
        else if (type == ItemType.eq_hands)
        {
            return new VirtualGood[] {
                EQ_HANDS_SET_EXPLORER,
                EQ_HANDS_SET_PIRATE
            };
        }
        else if (type == ItemType.eq_legs)
        {
            return new VirtualGood[] {
                EQ_LEGS_SET_EXPLORER,
                EQ_LEGS_SET_PIRATE
            };
        }
        else if (type == ItemType.eq_feet)
        {
            return new VirtualGood[] {
                EQ_FEET_SET_EXPLORER,
                EQ_FEET_SET_PIRATE
            };
        }
        else if (type == ItemType.item_consumable)
        {
            return new VirtualGood[] {
                CONSUMABLE_CHARCOAL_1,
                CONSUMABLE_CHARCOAL_2,
                CONSUMABLE_CHARCOAL_3,
                CONSUMABLE_DIAPERS_1,
                CONSUMABLE_DIAPERS_2,
                CONSUMABLE_DIAPERS_3,
                CONSUMABLE_DETERGENT
            };
        }
        else if (type == ItemType.item_instant)
        {
            return new VirtualGood[] {
                USE_PORTABLE_POTTY_1,
                USE_PORTABLE_POTTY_2,
                USE_PORTABLE_POTTY_3
            };
        }
        else if (type == ItemType.other)
        {
            return new VirtualGood[] {
                EMERGENCY_REVIVE_1_1,
                EMERGENCY_REVIVE_1_2,
                EMERGENCY_REVIVE_1_3,
                EMERGENCY_REVIVE_2_1,
                EMERGENCY_REVIVE_2_2,
                EMERGENCY_REVIVE_2_3,
                EMERGENCY_REVIVE_3_1,
                EMERGENCY_REVIVE_3_2,
                EMERGENCY_REVIVE_3_3
            };
        }
        else // Should never end up here (just included to prevent an error message)
        {
            return new VirtualGood[] {
            };
        }
    }

    #region Constants

    #region Currency

    public const string NORMAL_TOILET_PAPER_ID = "ntp";
    public const string GOLDEN_TOILET_PAPER_ID = "gtp";

    #endregion
    #region Equipment Head

    // items
    public const string EQ_HEAD_SET_EXPLORER_ID = "eq_head_set_explorer";
    public const string EQ_HEAD_SET_PIRATE_ID = "eq_head_set_pirate";

    // category name
    public const string EQ_HEAD_CATEGORY_NAME = "eq_head";

    // list of items (to put into the category)
    public static List<string> EQ_HEAD_LIST = 
        new List<string>(new string[] {
            EQ_HEAD_SET_EXPLORER_ID,
            EQ_HEAD_SET_PIRATE_ID
        });

    #endregion
    #region Equipment Body

    // items
    public const string EQ_BODY_SET_EXPLORER_ID = "eq_body_set_explorer";
    public const string EQ_BODY_SET_PIRATE_ID = "eq_body_set_pirate";

    // category name
    public const string EQ_BODY_CATEGORY_NAME = "eq_body";

    // list of items (to put into the category)
    public static List<string> EQ_BODY_LIST =
        new List<string>(new string[] {
            EQ_BODY_SET_EXPLORER_ID,
            EQ_BODY_SET_PIRATE_ID
        });

    #endregion
    #region Equipment Hands

    // items
    public const string EQ_HANDS_SET_EXPLORER_ID = "eq_hands_set_explorer";
    public const string EQ_HANDS_SET_PIRATE_ID = "eq_hands_set_pirate";

    // category name
    public const string EQ_HANDS_CATEGORY_NAME = "eq_hands";

    // list of items (to put into the category)
    public static List<string> EQ_HANDS_LIST =
        new List<string>(new string[] {
            EQ_HANDS_SET_EXPLORER_ID,
            EQ_HANDS_SET_PIRATE_ID
        });

    #endregion
    #region Equipment Legs

    // items
    public const string EQ_LEGS_SET_EXPLORER_ID = "eq_legs_set_explorer";
    public const string EQ_LEGS_SET_PIRATE_ID = "eq_legs_set_pirate";

    // category name
    public const string EQ_LEGS_CATEGORY_NAME = "eq_legs";

    // list of items (to put into the category)
    public static List<string> EQ_LEGS_LIST =
        new List<string>(new string[] {
            EQ_LEGS_SET_EXPLORER_ID,
            EQ_LEGS_SET_PIRATE_ID
        });

    #endregion
    #region Equipment Feet

    // items
    public const string EQ_FEET_SET_EXPLORER_ID = "eq_feet_set_explorer";
    public const string EQ_FEET_SET_PIRATE_ID = "eq_feet_set_pirate";

    // category name
    public const string EQ_FEET_CATEGORY_NAME = "eq_feet";

    // list of items (to put into the category)
    public static List<string> EQ_FEET_LIST =
        new List<string>(new string[] {
            EQ_FEET_SET_EXPLORER_ID,
            EQ_FEET_SET_PIRATE_ID
        });

    #endregion
    #region Consumables

    // items
    public const string CONSUMABLE_CHARCOAL_1_ID = "consumable_charcoal_1";
    public const string CONSUMABLE_CHARCOAL_2_ID = "consumable_charcoal_2";
    public const string CONSUMABLE_CHARCOAL_3_ID = "consumable_charcoal_3";
    public const string CONSUMABLE_DIAPERS_1_ID = "consumable_diapers_1";
    public const string CONSUMABLE_DIAPERS_2_ID = "consumable_diapers_2";
    public const string CONSUMABLE_DIAPERS_3_ID = "consumable_diapers_3";
    public const string CONSUMABLE_DETERGENT_ID = "consumable_detergent";

    // category name
    public const string CONSUMABLE_CATEGORY_NAME = "consumable";

    // list of items (to put into the category)
    public static List<string> CONSUMABLE_LIST =
        new List<string>(new string[] {
            CONSUMABLE_CHARCOAL_1_ID,
            CONSUMABLE_CHARCOAL_2_ID,
            CONSUMABLE_CHARCOAL_3_ID,
            CONSUMABLE_DIAPERS_1_ID,
            CONSUMABLE_DIAPERS_2_ID,
            CONSUMABLE_DIAPERS_3_ID,
            CONSUMABLE_DETERGENT_ID
        });

    #endregion
    #region Instant Use Items

    public const string USE_PORTABLE_POTTY_1_ID = "use_portable_potty_1";
    public const string USE_PORTABLE_POTTY_2_ID = "use_portable_potty_2";
    public const string USE_PORTABLE_POTTY_3_ID = "use_portable_potty_3";

    // currency packs
    public const string PACK_NTP_10_ID = "pack_ntp_10";
    public const string PACK_NTP_50_ID = "pack_ntp_50";
    public const string PACK_NTP_100_ID = "pack_ntp_100";
    public const string PACK_GTP_10_ID = "pack_gtp_10";
    public const string PACK_GTP_50_ID = "pack_gtp_50";
    public const string PACK_GTP_100_ID = "pack_gtp_100";

    // list of items that is used instantly
    public static List<string> INSTANT_USE_LIST =
        new List<string>(new string[] {
            USE_PORTABLE_POTTY_1_ID,
            USE_PORTABLE_POTTY_2_ID,
            USE_PORTABLE_POTTY_3_ID,
            PACK_NTP_10_ID,
            PACK_NTP_50_ID,
            PACK_NTP_100_ID,
            PACK_GTP_10_ID,
            PACK_GTP_50_ID,
            PACK_GTP_100_ID
    });

    #endregion
    #region Non Shop Items

    public const string EMERGENCY_REVIVE_1_1_ID = "emergency_revive_1_1";
    public const string EMERGENCY_REVIVE_1_2_ID = "emergency_revive_1_2";
    public const string EMERGENCY_REVIVE_1_3_ID = "emergency_revive_1_3";
    public const string EMERGENCY_REVIVE_2_1_ID = "emergency_revive_2_1";
    public const string EMERGENCY_REVIVE_2_2_ID = "emergency_revive_2_2";
    public const string EMERGENCY_REVIVE_2_3_ID = "emergency_revive_2_3";
    public const string EMERGENCY_REVIVE_3_1_ID = "emergency_revive_3_1";
    public const string EMERGENCY_REVIVE_3_2_ID = "emergency_revive_3_2";
    public const string EMERGENCY_REVIVE_3_3_ID = "emergency_revive_3_3";

    #endregion

    public const string NO_ADDS_NONCONS_PRODUCT_ID  = "no_ads";

    #endregion
    #region Virtual Currencies

    public static VirtualCurrency TOILET_PAPER = new VirtualCurrency(
        "Toilet Paper",              // name
        "",                          // description
        NORMAL_TOILET_PAPER_ID       // item id
    );

    public static VirtualCurrency GOLDEN_TOILET_PAPER = new VirtualCurrency(
        "Golden Toilet Paper",          // name
        "",                             // description
        GOLDEN_TOILET_PAPER_ID          // item id
    );

    #endregion
    #region Item Packs

    #endregion
    #region Virtual Goods

    #region Equipment Head

    public static EquippableVG EQ_HEAD_SET_EXPLORER = new EquippableVG(
        EquippableVG.EquippingModel.CATEGORY,                        // equipping model
        "[Explorer Head]",                                           // name
        "[Explorer Description]",                                    // description
        EQ_HEAD_SET_EXPLORER_ID,                                     // item id
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchase
    );

    public static EquippableVG EQ_HEAD_SET_PIRATE = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "[Pirate Head]",                                             // name
       "[Pirate Description]",                                      // description
       EQ_HEAD_SET_PIRATE_ID,                                       // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchase
    );

    #endregion
    #region Equipment Body

    public static EquippableVG EQ_BODY_SET_EXPLORER = new EquippableVG(
        EquippableVG.EquippingModel.CATEGORY,                        // equipping model
        "[Explorer Body]",                                           // name
        "[Explorer Description]",                                    // description
        EQ_BODY_SET_EXPLORER_ID,                                     // item id
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchase
    );

    public static EquippableVG EQ_BODY_SET_PIRATE = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "[Pirate Body]",                                             // name
       "[Pirate Description]",                                      // description
       EQ_BODY_SET_PIRATE_ID,                                       // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchase
    );

    #endregion
    #region Equipment Hands

    public static EquippableVG EQ_HANDS_SET_EXPLORER = new EquippableVG(
        EquippableVG.EquippingModel.CATEGORY,                        // equipping model
        "[Explorer Hands]",                                          // name
        "[Explorer Description]",                                    // description
        EQ_HANDS_SET_EXPLORER_ID,                                    // item id
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchase
    );

    public static EquippableVG EQ_HANDS_SET_PIRATE = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "[Pirate Hands]",                                            // name
       "[Pirate Description]",                                      // description
       EQ_HANDS_SET_PIRATE_ID,                                      // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchase
    );

    #endregion
    #region Equipment Legs

    public static EquippableVG EQ_LEGS_SET_EXPLORER = new EquippableVG(
        EquippableVG.EquippingModel.CATEGORY,                        // equipping model
        "[Explorer Legs]",                                          // name
        "[Explorer Description]",                                    // description
        EQ_LEGS_SET_EXPLORER_ID,                                    // item id
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchase
    );

    public static EquippableVG EQ_LEGS_SET_PIRATE = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "[Pirate Legs]",                                            // name
       "[Pirate Description]",                                      // description
       EQ_LEGS_SET_PIRATE_ID,                                      // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchase
    );

    #endregion
    #region Equipment Feet

    public static EquippableVG EQ_FEET_SET_EXPLORER = new EquippableVG(
        EquippableVG.EquippingModel.CATEGORY,                        // equipping model
        "[Explorer Feet]",                                          // name
        "[Explorer Description]",                                    // description
        EQ_FEET_SET_EXPLORER_ID,                                    // item id
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchase
    );

    public static EquippableVG EQ_FEET_SET_PIRATE = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "[Pirate Feet]",                                            // name
       "[Pirate Description]",                                      // description
       EQ_FEET_SET_PIRATE_ID,                                      // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchase
    );

    #endregion
    #region Consumables

    public static SingleUseVG CONSUMABLE_CHARCOAL_1 = new SingleUseVG(
        "[Charcoal 1]",                                              // name 
        "[Charcoal 1 Description]",                                  // description
        CONSUMABLE_CHARCOAL_1_ID,                                    // item id
        new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 50) // the way the good is purchased
    );

    public static SingleUseVG CONSUMABLE_CHARCOAL_2 = new SingleUseVG(
        "[Charcoal 2]",                                               // name 
        "[Charcoal 2 Description]",                                   // description
        CONSUMABLE_CHARCOAL_2_ID,                                     // item id
        new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 100) // the way the good is purchased
    );

    public static SingleUseVG CONSUMABLE_CHARCOAL_3 = new SingleUseVG(
        "[Charcoal 3]",                                              // name 
        "[Charcoal 3 Description]",                                  // description
        CONSUMABLE_CHARCOAL_3_ID,                                    // item id
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchased
    );

    public static SingleUseVG CONSUMABLE_DIAPERS_1 = new SingleUseVG(
        "[Diapers 1]",                                               // name
        "[Diapers 1 Description]",                                   // description
        CONSUMABLE_DIAPERS_1_ID,                                     // item id
        new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 50) // the way the good is purchased
    );

    public static SingleUseVG CONSUMABLE_DIAPERS_2 = new SingleUseVG(
        "[Diapers 2]",                                               // name
        "[Diapers 2 Description]",                                   // description
        CONSUMABLE_DIAPERS_2_ID,                                     // item id
        new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 100) // the way the good is purchased
    );

    public static SingleUseVG CONSUMABLE_DIAPERS_3 = new SingleUseVG(
        "[Diapers 3]",                                               // name
        "[Diapers 3 Description]",                                   // description
        CONSUMABLE_DIAPERS_3_ID,                                     // item id
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchased
    );

    public static SingleUseVG CONSUMABLE_DETERGENT = new SingleUseVG(
        "[Detergent]",                                               // name
        "[Detergent Description]",                                   // description
        CONSUMABLE_DETERGENT_ID,                                     // item id
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchased
    );

    #endregion
    #region Instant Use Items

    public static SingleUseVG USE_PORTABLE_POTTY_1 = new SingleUseVG(
        "[Portable Potty 1]",                                               // name
        "[Portable Potty 1 Description]",                                   // description
        USE_PORTABLE_POTTY_1_ID,                                     // item id
        new PurchaseWithMarket(USE_PORTABLE_POTTY_1_ID, 0.99) // the way the good is purchased
    );

    public static SingleUseVG USE_PORTABLE_POTTY_2 = new SingleUseVG(
        "[Portable Potty 2]",                                               // name
        "[Portable Potty 2 Description]",                                   // description
        USE_PORTABLE_POTTY_2_ID,                                     // item id
        new PurchaseWithMarket(USE_PORTABLE_POTTY_2_ID, 1.50) // the way the good is purchased
    );

    public static SingleUseVG USE_PORTABLE_POTTY_3 = new SingleUseVG(
        "[Portable Potty 3]",                                               // name
        "[Portable Potty 3 Description]",                                   // description
        USE_PORTABLE_POTTY_3_ID,                                     // item id
        new PurchaseWithMarket(USE_PORTABLE_POTTY_3_ID, 1.99) // the way the good is purchased
    );

    #region Virtual Currency Packs

    public static VirtualCurrencyPack PACK_NTP_10 = new VirtualCurrencyPack(
        "Normal Toilet Paper x10",                          // name
        "Normal Toilet Paper x10",                          // description
        PACK_NTP_10_ID,                                     // item id
        10,                                                 // number of currencies in pack
        NORMAL_TOILET_PAPER_ID,                        // currency associated
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 1)   // the way the good is purchased
    );

    public static VirtualCurrencyPack PACK_NTP_50 = new VirtualCurrencyPack(
        "Normal Toilet Paper x50",                          // name
        "Normal Toilet Paper x50",                          // description
        PACK_NTP_50_ID,                                     // item id
        50,                                                 // number of currencies in pack
        NORMAL_TOILET_PAPER_ID,                        // currency associated
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 5)   // the way the good is purchased
    );

    public static VirtualCurrencyPack PACK_NTP_100 = new VirtualCurrencyPack(
       "Normal Toilet Paper x100",                          // name
       "Normal Toilet Paper x100",                          // description
       PACK_NTP_100_ID,                                     // item id
       100,                                                 // number of currencies in pack
       NORMAL_TOILET_PAPER_ID,                        // currency associated
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 10)   // the way the good is purchased
    );

    public static VirtualCurrencyPack PACK_GTP_10 = new VirtualCurrencyPack(
        "Golden Toilet Paper x10",                          // name
        "Golden Toilet Paper x10",                          // description
        PACK_GTP_10_ID,                                     // item id
        10,                                                 // number of currencies in pack
        GOLDEN_TOILET_PAPER_ID,                        // currency associated
        new PurchaseWithVirtualItem(PACK_GTP_10_ID, 1)   // the way the good is purchased
    );

    public static VirtualCurrencyPack PACK_GTP_50 = new VirtualCurrencyPack(
        "Golden Toilet Paper x50",                          // name
        "Golden Toilet Paper x50",                          // description
        PACK_GTP_50_ID,                                     // item id
        50,                                                 // number of currencies in pack
        GOLDEN_TOILET_PAPER_ID,                        // currency associated
        new PurchaseWithVirtualItem(PACK_GTP_50_ID, 5)   // the way the good is purchased
    );

    public static VirtualCurrencyPack PACK_GTP_100 = new VirtualCurrencyPack(
       "Golden Toilet Paper x100",                          // name
       "Golden Toilet Paper x100",                          // description
       PACK_GTP_100_ID,                                     // item id
       100,                                                 // number of currencies in pack
       GOLDEN_TOILET_PAPER_ID,                        // currency associated
       new PurchaseWithMarket(PACK_GTP_100_ID, 10)   // the way the good is purchased
    );

    #endregion

    #endregion
    #region Non Shop Items

    public static SingleUseVG EMERGENCY_REVIVE_1_1 = new SingleUseVG(
        "[Emergency Diapers 1 (1st attempt)]",                                               // name
        "[Emergency Diapers 1 Description]",                                   // description
        EMERGENCY_REVIVE_1_1_ID,                                     // item id
        new PurchaseWithMarket(EMERGENCY_REVIVE_1_1_ID, 0.99) // the way the good is purchased
    );
    public static SingleUseVG EMERGENCY_REVIVE_1_2 = new SingleUseVG(
        "[Emergency Diapers 1 (2nd attempt)]",                                               // name
        "[Emergency Diapers 1 Description]",                                   // description
        EMERGENCY_REVIVE_1_2_ID,                                     // item id
        new PurchaseWithMarket(EMERGENCY_REVIVE_1_2_ID, 1.25) // the way the good is purchased
    );
    public static SingleUseVG EMERGENCY_REVIVE_1_3 = new SingleUseVG(
        "[Emergency Diapers 1 (3rd attempt)]",                                               // name
        "[Emergency Diapers 1 Description]",                                   // description
        EMERGENCY_REVIVE_1_3_ID,                                     // item id
        new PurchaseWithMarket(EMERGENCY_REVIVE_1_3_ID, 1.5) // the way the good is purchased
    );

    public static SingleUseVG EMERGENCY_REVIVE_2_1 = new SingleUseVG(
        "[Emergency Diapers 2 (1st attempt)]",                                               // name
        "[Emergency Diapers 2 Description]",                                   // description
        EMERGENCY_REVIVE_2_1_ID,                                     // item id
        new PurchaseWithMarket(EMERGENCY_REVIVE_2_1_ID, 1.75) // the way the good is purchased
    );
    public static SingleUseVG EMERGENCY_REVIVE_2_2 = new SingleUseVG(
        "[Emergency Diapers 2 (2nd attempt)]",                                               // name
        "[Emergency Diapers 2 Description]",                                   // description
        EMERGENCY_REVIVE_2_2_ID,                                     // item id
        new PurchaseWithMarket(EMERGENCY_REVIVE_2_2_ID, 2.25) // the way the good is purchased
    );
    public static SingleUseVG EMERGENCY_REVIVE_2_3 = new SingleUseVG(
        "[Emergency Diapers 2 (3rd attempt)]",                                               // name
        "[Emergency Diapers 2 Description]",                                   // description
        EMERGENCY_REVIVE_2_3_ID,                                     // item id
        new PurchaseWithMarket(EMERGENCY_REVIVE_2_3_ID, 2.75) // the way the good is purchased
    );

    public static SingleUseVG EMERGENCY_REVIVE_3_1 = new SingleUseVG(
        "[Emergency Diapers 3 (1st attempt)]",                                               // name
        "[Emergency Diapers 3 Description]",                                   // description
        EMERGENCY_REVIVE_3_1_ID,                                     // item id
        new PurchaseWithMarket(EMERGENCY_REVIVE_3_1_ID, 3) // the way the good is purchased
    );
    public static SingleUseVG EMERGENCY_REVIVE_3_2 = new SingleUseVG(
        "[Emergency Diapers 3 (2nd attempt)]",                                               // name
        "[Emergency Diapers 3 Description]",                                   // description
        EMERGENCY_REVIVE_3_2_ID,                                     // item id
        new PurchaseWithMarket(EMERGENCY_REVIVE_3_2_ID, 3) // the way the good is purchased
    );
    public static SingleUseVG EMERGENCY_REVIVE_3_3 = new SingleUseVG(
        "[Emergency Diapers 3 (3rd attempt)]",                                               // name
        "[Emergency Diapers 3 Description]",                                   // description
        EMERGENCY_REVIVE_3_3_ID,                                     // item id
        new PurchaseWithMarket(EMERGENCY_REVIVE_3_3_ID, 3) // the way the good is purchased
    );

    #endregion

    #endregion
    #region Virtual Categories

    // Equipment head category
    public static VirtualCategory EQUIP_HEAD_CATEGORY = new VirtualCategory(
        EQ_HEAD_CATEGORY_NAME,      // name
        EQ_HEAD_LIST                // items in category
    );

    // Equipment body category
    public static VirtualCategory EQUIP_BODY_CATEGORY = new VirtualCategory(
        EQ_BODY_CATEGORY_NAME,      // name
        EQ_BODY_LIST                // items in category
    );

    // Equipment hands category
    public static VirtualCategory EQUIP_HANDS_CATEGORY = new VirtualCategory(
        EQ_HANDS_CATEGORY_NAME,     // name
        EQ_HANDS_LIST               // items in category
    );

    // Equipment legs category
    public static VirtualCategory EQUIP_LEGS_CATEGORY = new VirtualCategory(
        EQ_LEGS_CATEGORY_NAME,     // name
        EQ_LEGS_LIST               // items in category
    );

    // Equipment feet category
    public static VirtualCategory EQUIP_FEET_CATEGORY = new VirtualCategory(
        EQ_FEET_CATEGORY_NAME,     // name
        EQ_FEET_LIST               // items in category
    );

    #endregion
    #region Market MANAGED Items

    public static NonConsumableItem NO_ADDS_NONCONS = new NonConsumableItem(
        "No Ads",
        "Test purchase of MANAGED item.",
        "no_ads",
        new PurchaseWithMarket(new MarketItem(NO_ADDS_NONCONS_PRODUCT_ID, MarketItem.Consumable.NONCONSUMABLE, 0.99))
    );

    #endregion
}
