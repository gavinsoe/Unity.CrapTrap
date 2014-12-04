using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla;
using Soomla.Store;

public class CrapTrapAssets : IStoreAssets
{
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
            EQ_HEAD_SET_TRIBAL,
            EQ_HEAD_SET_DIVER,
            EQ_HEAD_APPLE_ARROW,
            EQ_HEAD_BEANIE,
            EQ_HEAD_CAP,
            EQ_HEAD_HELMET_BLUE,
            EQ_HEAD_HELMET_WHITE,
            EQ_HEAD_NEKOMIMI,
            EQ_HEAD_PAPERBAG,
            EQ_HEAD_SHARK,
            EQ_HEAD_SPECS_BLUE,
            EQ_BODY_SET_EXPLORER,
            EQ_BODY_SET_PIRATE,
            EQ_BODY_SET_TRIBAL,
            EQ_BODY_SET_DIVER,
            EQ_BODY_BARREL,
            EQ_BODY_BOWTIE,
            EQ_BODY_BOXING_GLOVES,
            EQ_BODY_CHESTHAIR,     
            EQ_BODY_HOODIE,
            EQ_BODY_KARATEGI,
            EQ_BODY_LABCOAT_KRIEGER,
            EQ_BODY_LABCOAT_NORMAL,
            EQ_BODY_NAKED,
            EQ_BODY_SHIRT_PINK,
            EQ_BODY_SHIRT_RED,
            EQ_BODY_SHIRT_YELLOW,
            EQ_BODY_SUIT,
            EQ_BODY_TATTOO,
            EQ_LEGS_SET_EXPLORER,
            EQ_LEGS_SET_PIRATE,
            EQ_LEGS_SET_TRIBAL,
            EQ_LEGS_SET_DIVER,
            EQ_LEGS_ARMY,
            EQ_LEGS_BALLET,
            EQ_LEGS_COWBOY,
            EQ_LEGS_HERMES,
            EQ_LEGS_LEATHER,
            EQ_LEGS_LOAFERS,
            EQ_LEGS_SNEAKERS,
            EQ_LEGS_SUIT,
            CONSUMABLE_CHARCOAL_1,
            CONSUMABLE_CHARCOAL_2,
            CONSUMABLE_CHARCOAL_3,
            CONSUMABLE_CHARCOAL_1_5PACK,
            CONSUMABLE_CHARCOAL_2_5PACK,
            CONSUMABLE_CHARCOAL_3_5PACK,
            CONSUMABLE_CHARCOAL_1_10PACK,
            CONSUMABLE_CHARCOAL_2_10PACK,
            CONSUMABLE_CHARCOAL_3_10PACK,
            CONSUMABLE_LUCKY_CHARM,
            CONSUMABLE_PLUNGER,
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
            PACK_1_NTP,
            PACK_2_NTP,
            PACK_3_NTP,
            PACK_4_NTP,
            PACK_5_NTP,
            PACK_1_GTP,
            PACK_2_GTP,
            PACK_3_GTP,
            PACK_4_GTP,
            PACK_5_GTP,
            PACK_6_GTP
        };
    }

    public VirtualCategory[] GetCategories()
    {
        return new VirtualCategory[] { 
            EQUIP_HEAD_CATEGORY,
            EQUIP_BODY_CATEGORY,
            EQUIP_LEGS_CATEGORY
        };
    }

    public static VirtualGood[] GetSpecificGoods(ItemType type)
    {
        if (type == ItemType.eq_head)
        {
            return new VirtualGood[] {
                EQ_HEAD_SET_EXPLORER,
                EQ_HEAD_APPLE_ARROW,
                EQ_HEAD_BEANIE,
                EQ_HEAD_CAP,
                EQ_HEAD_HELMET_BLUE,
                EQ_HEAD_HELMET_WHITE,
                EQ_HEAD_NEKOMIMI,
                EQ_HEAD_PAPERBAG,
                EQ_HEAD_SHARK,
                EQ_HEAD_SPECS_BLUE
            };
        }
        else if (type == ItemType.eq_body)
        {
            return new VirtualGood[] { 
                EQ_BODY_SET_EXPLORER,
                EQ_BODY_BARREL,
                EQ_BODY_BOWTIE,
                EQ_BODY_BOXING_GLOVES,
                EQ_BODY_CHESTHAIR,     
                EQ_BODY_HOODIE,
                EQ_BODY_KARATEGI,
                EQ_BODY_LABCOAT_KRIEGER,
                EQ_BODY_LABCOAT_NORMAL,
                EQ_BODY_NAKED,
                EQ_BODY_SHIRT_PINK,
                EQ_BODY_SHIRT_RED,
                EQ_BODY_SHIRT_YELLOW,
                EQ_BODY_SUIT,
                EQ_BODY_TATTOO
            };
        }
        else if (type == ItemType.eq_legs)
        {
            return new VirtualGood[] {
                EQ_LEGS_SET_EXPLORER,
                EQ_LEGS_ARMY,
                EQ_LEGS_BALLET,
                EQ_LEGS_COWBOY,
                EQ_LEGS_HERMES,
                EQ_LEGS_LEATHER,
                EQ_LEGS_LOAFERS,
                EQ_LEGS_SNEAKERS,
                EQ_LEGS_SUIT
            };
        }
        else if (type == ItemType.item_consumable)
        {
            return new VirtualGood[] {
                CONSUMABLE_CHARCOAL_1,
                CONSUMABLE_CHARCOAL_2,
                CONSUMABLE_CHARCOAL_3,
                CONSUMABLE_LUCKY_CHARM
            };
        }
        else if (type == ItemType.item_instant)
        {
            return new VirtualGood[] {
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
                EMERGENCY_REVIVE_3_3,
                CONSUMABLE_PLUNGER
            };
        }
        else // Should never end up here (just included to prevent an error message)
        {
            return new VirtualGood[] {
            };
        }
    }

    public static VirtualCurrencyPack[] GetCurrencyPacksCustom()
    {
        return new VirtualCurrencyPack[] {  
            PACK_1_NTP,
            PACK_2_NTP,
            PACK_3_NTP,
            PACK_4_NTP,
            PACK_5_NTP,
            PACK_1_GTP,
            PACK_2_GTP,
            PACK_3_GTP,
            PACK_4_GTP,
            PACK_5_GTP,
            PACK_6_GTP
        };
    }

    #region Constants

    #region Currency

    public const string NORMAL_TOILET_PAPER_ID = "currency_ntp";
    public const string GOLDEN_TOILET_PAPER_ID = "currency_gtp";

    #endregion
    #region Equipment Head

    // items
    public const string EQ_HEAD_SET_EXPLORER_ID = "eq_head_set_explorer";
    public const string EQ_HEAD_SET_PIRATE_ID = "eq_head_set_pirate";
    public const string EQ_HEAD_SET_TRIBAL_ID = "eq_head_set_tribal";
    public const string EQ_HEAD_SET_DIVER_ID = "eq_head_set_diver";
    public const string EQ_HEAD_APPLE_ARROW_ID = "eq_head_apple_arrow";
    public const string EQ_HEAD_BEANIE_ID = "eq_head_beanie";
    public const string EQ_HEAD_CAP_ID = "eq_head_cap";
    public const string EQ_HEAD_HELMET_BLUE_ID = "eq_head_helmet_blue";
    public const string EQ_HEAD_HELMET_WHITE_ID = "eq_head_helmet_white";
    public const string EQ_HEAD_NEKOMIMI_ID = "eq_head_nekomimi";
    public const string EQ_HEAD_PAPERBAG_ID = "eq_head_paperbag";
    public const string EQ_HEAD_SHARK_ID = "eq_head_shark";
    public const string EQ_HEAD_SPECS_BLUE_ID = "eq_head_specs_blue";

    // category name
    public const string EQ_HEAD_CATEGORY_NAME = "eq_head";

    // list of items (to put into the category)
    public static List<string> EQ_HEAD_LIST =
        new List<string>(new string[] {
            EQ_HEAD_SET_EXPLORER_ID,
            EQ_HEAD_SET_PIRATE_ID,
            EQ_HEAD_SET_TRIBAL_ID,
            EQ_HEAD_APPLE_ARROW_ID,
            EQ_HEAD_BEANIE_ID,
            EQ_HEAD_CAP_ID,
            EQ_HEAD_HELMET_BLUE_ID,
            EQ_HEAD_HELMET_WHITE_ID,
            EQ_HEAD_NEKOMIMI_ID,
            EQ_HEAD_PAPERBAG_ID,
            EQ_HEAD_SHARK_ID,
            EQ_HEAD_SPECS_BLUE_ID
        });

    #endregion
    #region Equipment Upper Body

    // items
    public const string EQ_BODY_SET_EXPLORER_ID = "eq_body_set_explorer";
    public const string EQ_BODY_SET_PIRATE_ID = "eq_body_set_pirate";
    public const string EQ_BODY_SET_TRIBAL_ID = "eq_body_set_tribal";
    public const string EQ_BODY_SET_DIVER_ID = "eq_body_set_diver";
    public const string EQ_BODY_BARREL_ID = "eq_body_barrel";
    public const string EQ_BODY_BOWTIE_ID = "eq_body_bowtie";
    public const string EQ_BODY_BOXING_GLOVES_ID = "eq_body_boxing_gloves";
    public const string EQ_BODY_CHESTHAIR_ID = "eq_body_chesthair";
    public const string EQ_BODY_HOODIE_ID = "eq_body_hoodie";
    public const string EQ_BODY_KARATEGI_ID = "eq_body_karategi";
    public const string EQ_BODY_LABCOAT_KRIEGER_ID = "eq_body_labcoat_krieger";
    public const string EQ_BODY_LABCOAT_NORMAL_ID = "eq_body_labcoat_normal";
    public const string EQ_BODY_NAKED_ID = "eq_body_naked";
    public const string EQ_BODY_SHIRT_PINK_ID = "eq_body_shirt_pink";
    public const string EQ_BODY_SHIRT_RED_ID = "eq_body_shirt_red";
    public const string EQ_BODY_SHIRT_YELLOW_ID = "eq_body_shirt_yellow";
    public const string EQ_BODY_SUIT_ID = "eq_body_suit";
    public const string EQ_BODY_TATTOO_ID = "eq_body_tattoo";

    // category name
    public const string EQ_BODY_CATEGORY_NAME = "eq_body";

    // list of items (to put into the category)
    public static List<string> EQ_BODY_LIST =
        new List<string>(new string[] {
            EQ_BODY_SET_EXPLORER_ID,
            EQ_BODY_SET_PIRATE_ID,
            EQ_BODY_SET_TRIBAL_ID,
            EQ_BODY_BARREL_ID,
            EQ_BODY_BOWTIE_ID,
            EQ_BODY_BOXING_GLOVES_ID,
            EQ_BODY_CHESTHAIR_ID,     
            EQ_BODY_HOODIE_ID,
            EQ_BODY_KARATEGI_ID,
            EQ_BODY_LABCOAT_KRIEGER_ID,
            EQ_BODY_LABCOAT_NORMAL_ID,
            EQ_BODY_NAKED_ID,
            EQ_BODY_SHIRT_PINK_ID,
            EQ_BODY_SHIRT_RED_ID,
            EQ_BODY_SHIRT_YELLOW_ID,
            EQ_BODY_SUIT_ID,
            EQ_BODY_TATTOO_ID
        });

    #endregion
    #region Equipment Legs

    // items
    public const string EQ_LEGS_SET_EXPLORER_ID = "eq_legs_set_explorer";
    public const string EQ_LEGS_SET_PIRATE_ID = "eq_legs_set_pirate";
    public const string EQ_LEGS_SET_TRIBAL_ID = "eq_legs_set_tribal";
    public const string EQ_LEGS_SET_DIVER_ID = "eq_legs_set_diver";
    public const string EQ_LEGS_ARMY_ID = "eq_legs_army";
    public const string EQ_LEGS_BALLET_ID = "eq_legs_ballet";
    public const string EQ_LEGS_COWBOY_ID = "eq_legs_cowboy";
    public const string EQ_LEGS_HERMES_ID = "eq_legs_hermes";
    public const string EQ_LEGS_LEATHER_ID = "eq_legs_leather";
    public const string EQ_LEGS_LOAFERS_ID = "eq_legs_loafers";
    public const string EQ_LEGS_SNEAKERS_ID = "eq_legs_sneakers";
    public const string EQ_LEGS_SUIT_ID = "eq_legs_suit";

    // category name
    public const string EQ_LEGS_CATEGORY_NAME = "eq_legs";

    // list of items (to put into the category)
    public static List<string> EQ_LEGS_LIST =
        new List<string>(new string[] {
            EQ_LEGS_SET_EXPLORER_ID,
            EQ_LEGS_SET_PIRATE_ID,
            EQ_LEGS_SET_TRIBAL_ID,
            EQ_LEGS_ARMY_ID,
            EQ_LEGS_BALLET_ID,
            EQ_LEGS_COWBOY_ID,
            EQ_LEGS_HERMES_ID,
            EQ_LEGS_LEATHER_ID,
            EQ_LEGS_LOAFERS_ID,
            EQ_LEGS_SNEAKERS_ID,
            EQ_LEGS_SUIT_ID
        });

    #endregion
    #region Consumables

    // items
    public const string CONSUMABLE_CHARCOAL_1_ID = "consumable_charcoal_1";
    public const string CONSUMABLE_CHARCOAL_2_ID = "consumable_charcoal_2";
    public const string CONSUMABLE_CHARCOAL_3_ID = "consumable_charcoal_3";

    public const string CONSUMABLE_CHARCOAL_1_5PACK_ID = "consumable_charcoal_1_5pack";
    public const string CONSUMABLE_CHARCOAL_2_5PACK_ID = "consumable_charcoal_2_5pack";
    public const string CONSUMABLE_CHARCOAL_3_5PACK_ID = "consumable_charcoal_3_5pack";

    public const string CONSUMABLE_CHARCOAL_1_10PACK_ID = "consumable_charcoal_1_10pack";
    public const string CONSUMABLE_CHARCOAL_2_10PACK_ID = "consumable_charcoal_2_10pack";
    public const string CONSUMABLE_CHARCOAL_3_10PACK_ID = "consumable_charcoal_3_10pack";

    public const string CONSUMABLE_LUCKY_CHARM_ID = "consumable_lucky_charm";
    public const string CONSUMABLE_PLUNGER_ID = "consumable_plunger";

    // category name
    public const string CONSUMABLE_CATEGORY_NAME = "consumable";

    // list of items (to put into the category)
    public static List<string> CONSUMABLE_LIST =
        new List<string>(new string[] {
            CONSUMABLE_CHARCOAL_1_ID,
            CONSUMABLE_CHARCOAL_2_ID,
            CONSUMABLE_CHARCOAL_3_ID,
            CONSUMABLE_CHARCOAL_1_5PACK_ID,
            CONSUMABLE_CHARCOAL_2_5PACK_ID,
            CONSUMABLE_CHARCOAL_3_5PACK_ID,
            CONSUMABLE_CHARCOAL_1_10PACK_ID,
            CONSUMABLE_CHARCOAL_2_10PACK_ID,
            CONSUMABLE_CHARCOAL_3_10PACK_ID,
            CONSUMABLE_LUCKY_CHARM_ID,
        });

    #endregion
    #region Instant Use Items

    // currency packs
    public const string PACK_1_NTP_ID = "pack_1_ntp";
    public const string PACK_2_NTP_ID = "pack_2_ntp";
    public const string PACK_3_NTP_ID = "pack_3_ntp";
    public const string PACK_4_NTP_ID = "pack_4_ntp";
    public const string PACK_5_NTP_ID = "pack_5_ntp";
    public const string PACK_1_GTP_ID = "pack_1_gtp";
    public const string PACK_2_GTP_ID = "pack_2_gtp";
    public const string PACK_3_GTP_ID = "pack_3_gtp";
    public const string PACK_4_GTP_ID = "pack_4_gtp";
    public const string PACK_5_GTP_ID = "pack_5_gtp";
    public const string PACK_6_GTP_ID = "pack_6_gtp";

    // list of items that is used instantly
    public static List<string> INSTANT_USE_LIST =
        new List<string>(new string[] {
            PACK_1_NTP_ID,
            PACK_2_NTP_ID,
            PACK_3_NTP_ID,
            PACK_4_NTP_ID,
            PACK_5_NTP_ID,
            PACK_1_GTP_ID,
            PACK_2_GTP_ID,
            PACK_3_GTP_ID,
            PACK_4_GTP_ID,
            PACK_5_GTP_ID,
            PACK_6_GTP_ID,
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

    public const string NO_ADDS_NONCONS_PRODUCT_ID = "no_ads";

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
    #region Virtual Goods

    #region Equipment Head

    public static EquippableVG EQ_HEAD_SET_EXPLORER = new EquippableVG(
        EquippableVG.EquippingModel.CATEGORY,                        // equipping model
        "Explorer Hat",                                           // name
        "Complete set shows location of treasures.",                                    // description
        EQ_HEAD_SET_EXPLORER_ID,                                     // item id
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 75) // the way the good is purchase
    );

    // Non purchasable
    public static EquippableVG EQ_HEAD_SET_PIRATE = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Pirate Hat and Patch",                                             // name
       "“Arrrrrrrrrrrg!!!” Increase chance of treasure appearing behind blocks at the cost of losing an eye.",                                      // description
       EQ_HEAD_SET_PIRATE_ID,                                       // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchase
    );

    public static EquippableVG EQ_HEAD_SET_TRIBAL = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Voodoo Mask",                                             // name
       "Mask worn by tribal shamans",                                      // description
       EQ_HEAD_SET_TRIBAL_ID,                                       // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchase
    );

    public static EquippableVG EQ_HEAD_SET_DIVER = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Diving Helmet",                                             // name
       "Highly pressurised compressed gas",                                      // description
       EQ_HEAD_SET_DIVER_ID,                                       // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchase
    );

    public static EquippableVG EQ_HEAD_APPLE_ARROW = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Apple of Archer",                                             // name
       "Symbol of accuracy and precision. Increase time by a small amount",                                      // description
       EQ_HEAD_APPLE_ARROW_ID,                                       // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 25) // the way the good is purchase
    );

    public static EquippableVG EQ_HEAD_BEANIE = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Beanie",                                             // name
       "Beanie",                                      // description
       EQ_HEAD_BEANIE_ID,                                       // item id
       new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 250) // the way the good is purchase
    );

    public static EquippableVG EQ_HEAD_CAP = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Crap Cap",                                             // name
       "Crap Cap",                                      // description
       EQ_HEAD_CAP_ID,                                       // item id
       new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 300) // the way the good is purchase
    );

    public static EquippableVG EQ_HEAD_HELMET_BLUE = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Blue Biker Helmet",                                             // name
       "Provides protection and aesthetics. Increase time by a small amount.",                                      // description
       EQ_HEAD_HELMET_BLUE_ID,                                       // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 25) // the way the good is purchase
    );

    public static EquippableVG EQ_HEAD_HELMET_WHITE = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "White Biker Helmet",                                             // name
       "White Biker Helmet",                                      // description
       EQ_HEAD_HELMET_WHITE_ID,                                       // item id
       new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 400) // the way the good is purchase
    );

    public static EquippableVG EQ_HEAD_NEKOMIMI = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Nekomimi",                                             // name
       "Black cats are not symbols of bad luck. Increases time by medium amount.",                                      // description
       EQ_HEAD_NEKOMIMI_ID,                                       // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 35) // the way the good is purchase
    );

    public static EquippableVG EQ_HEAD_PAPERBAG = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Paper-bag",                                             // name
       "Protection against embarassment. Increases time by a small amount.",                                      // description
       EQ_HEAD_PAPERBAG_ID,                                       // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 30) // the way the good is purchase
    );

    public static EquippableVG EQ_HEAD_SHARK = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Jaws",                                             // name
       "Symbol of strength in the rough seas. Increases time by a medium amount.",                                      // description
       EQ_HEAD_SHARK_ID,                                       // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 35) // the way the good is purchase
    );

    public static EquippableVG EQ_HEAD_SPECS_BLUE = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Glasses",                                             // name
       "Glasses",                                      // description
       EQ_HEAD_SPECS_BLUE_ID,                                       // item id
       new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 250) // the way the good is purchase
    );

    #endregion
    #region Equipment Body

    public static EquippableVG EQ_BODY_SET_EXPLORER = new EquippableVG(
        EquippableVG.EquippingModel.CATEGORY,                        // equipping model
        "Explorer Gear",                                           // name
        "Complete set shows location of treasures.",                                    // description
        EQ_BODY_SET_EXPLORER_ID,                                     // item id
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 75) // the way the good is purchase
    );

    public static EquippableVG EQ_BODY_SET_PIRATE = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Pirate garb",                                             // name
       "“Thar she blows!” Increase time by a large amount and allows hanging on ice blocks",                                      // description
       EQ_BODY_SET_PIRATE_ID,                                       // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchase
    );

    public static EquippableVG EQ_BODY_SET_TRIBAL = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Tribal Regalia",                                             // name
       "Worn by tribal shamans during rituals to perform miracles.",                                      // description
       EQ_BODY_SET_TRIBAL_ID,                                       // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchase
    );

    public static EquippableVG EQ_BODY_SET_DIVER = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Diving Suit",                                             // name
       "Highly Flammable and combustible",                                      // description
       EQ_BODY_SET_DIVER_ID,                                       // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchase
    );

    public static EquippableVG EQ_BODY_BARREL = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Barreled",                                             // name
       "A shell that can be used for privacy. Increases time by a medium amount.",                                      // description
       EQ_BODY_BARREL_ID,                                       // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 35) // the way the good is purchase
    );

    public static EquippableVG EQ_BODY_BOWTIE = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Bowtie",                                             // name
       "Bowtie",                                      // description
       EQ_BODY_BOWTIE_ID,                                       // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchase
    );

    public static EquippableVG EQ_BODY_BOXING_GLOVES = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Boxing Gloves",                                             // name
       "Domination of strength and speed, Inceases time by a small amount.",                                      // description
       EQ_BODY_BOXING_GLOVES_ID,                                       // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 25) // the way the good is purchase
    );

    public static EquippableVG EQ_BODY_CHESTHAIR = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Chest Rug",                                             // name
       "Chest Rug",                                      // description
       EQ_BODY_CHESTHAIR_ID,                                       // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchase
    );

    public static EquippableVG EQ_BODY_HOODIE = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Blue Hoodie",                                             // name
       "Blue Hoodie",                                      // description
       EQ_BODY_HOODIE_ID,                                       // item id
       new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 400) // the way the good is purchase
    );

    public static EquippableVG EQ_BODY_KARATEGI = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Black Gi (top)",                                             // name
       "Allows the wearer to focus their internal energy. Increses time by a medium amount.",                                      // description
       EQ_BODY_KARATEGI_ID,                                       // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 35) // the way the good is purchase
    );

    public static EquippableVG EQ_BODY_LABCOAT_KRIEGER = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "[Labcoat Krieger Body]",                                             // name
       "[Labcoat Krieger Description]",                                      // description
       EQ_BODY_LABCOAT_KRIEGER_ID,                                       // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchase
    );

    public static EquippableVG EQ_BODY_LABCOAT_NORMAL = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Lab Coat",                                             // name
       "Lab Coat",                                      // description
       EQ_BODY_LABCOAT_NORMAL_ID,                                       // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchase
    );

    public static EquippableVG EQ_BODY_NAKED = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Bare naked",                                             // name
       "Streaker",                                      // description
       EQ_BODY_NAKED_ID,                                       // item id
       new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 400) // the way the good is purchase
    );

    public static EquippableVG EQ_BODY_SHIRT_PINK = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Pink shirt",                                             // name
       "Pink shirt",                                      // description
       EQ_BODY_SHIRT_PINK_ID,                                       // item id
       new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 300) // the way the good is purchase
    );

    public static EquippableVG EQ_BODY_SHIRT_RED = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Red shirt",                                             // name
       "Red shirt",                                      // description
       EQ_BODY_SHIRT_RED_ID,                                       // item id
       new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 250) // the way the good is purchase
    );

    public static EquippableVG EQ_BODY_SHIRT_YELLOW = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Yellow shirt",                                             // name
       "Yellow shirt",                                      // description
       EQ_BODY_SHIRT_YELLOW_ID,                                       // item id
       new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 250) // the way the good is purchase
    );

    public static EquippableVG EQ_BODY_SUIT = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Black Suit",                                             // name
       "This suit is too expensive to be soiled. Increases time by a small amount.",                                      // description
       EQ_BODY_SUIT_ID,                                       // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 25) // the way the good is purchase
    );

    public static EquippableVG EQ_BODY_TATTOO = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Inked",                                             // name
       "Special ink that has magical powers. Increases time by a small amount.",                                      // description
       EQ_BODY_TATTOO_ID,                                       // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 25) // the way the good is purchase
    );

    #endregion
    #region Equipment Legs

    public static EquippableVG EQ_LEGS_SET_EXPLORER = new EquippableVG(
        EquippableVG.EquippingModel.CATEGORY,                        // equipping model
        "Explorer Gear",                                          // name
        "Complete set shows location of treasures.",                                    // description
        EQ_LEGS_SET_EXPLORER_ID,                                    // item id
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 75) // the way the good is purchase
    );

    public static EquippableVG EQ_LEGS_SET_PIRATE = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Pirate garb",                                            // name
       "“Walk the plank!”",                                      // description
       EQ_LEGS_SET_PIRATE_ID,                                      // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchase
    );

    public static EquippableVG EQ_LEGS_SET_TRIBAL = new EquippableVG(
        EquippableVG.EquippingModel.CATEGORY,                        // equipping model
        "Tribal Regalia",                                          // name
        "Worn by tribal shamans to walk on fire",                                    // description
        EQ_LEGS_SET_TRIBAL_ID,                                    // item id
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchase
    );

    public static EquippableVG EQ_LEGS_SET_DIVER = new EquippableVG(
       EquippableVG.EquippingModel.CATEGORY,                        // equipping model
       "Diving Suit",                                             // name
       "Unstable and reactive",                                      // description
       EQ_LEGS_SET_DIVER_ID,                                       // item id
       new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50) // the way the good is purchase
    );

    public static EquippableVG EQ_LEGS_ARMY = new EquippableVG(
        EquippableVG.EquippingModel.CATEGORY,                        // equipping model
        "Military fatigues",                                          // name
        "Camoflaged. Increases time by a small amount.",                                    // description
        EQ_LEGS_ARMY_ID,                                    // item id
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 25) // the way the good is purchase
    );

    public static EquippableVG EQ_LEGS_BALLET = new EquippableVG(
        EquippableVG.EquippingModel.CATEGORY,                        // equipping model
        "Tutu",                                          // name
        "Enhances grace, beauty and femininity. Increases time by a medium amount",                                    // description
        EQ_LEGS_BALLET_ID,                                    // item id
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 35) // the way the good is purchase
    );

    public static EquippableVG EQ_LEGS_COWBOY = new EquippableVG(
        EquippableVG.EquippingModel.CATEGORY,                        // equipping model
        "Cowboy chaps",                                          // name
        "Mounted riders needs protection. Increases time by a small amount.",                                    // description
        EQ_LEGS_COWBOY_ID,                                    // item id
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 25) // the way the good is purchase
    );

    public static EquippableVG EQ_LEGS_HERMES = new EquippableVG(
        EquippableVG.EquippingModel.CATEGORY,                        // equipping model
        "Hermes' Talaria",                                          // name
        "Winged sandals symbolizes the god Hermes. Increases time by a medium amount",                                    // description
        EQ_LEGS_HERMES_ID,                                    // item id
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 35) // the way the good is purchase
    );

    public static EquippableVG EQ_LEGS_LEATHER = new EquippableVG(
        EquippableVG.EquippingModel.CATEGORY,                        // equipping model
        "Leather Boots",                                          // name
        "Footwear made of leather covering the feet and ankles",                                    // description
        EQ_LEGS_LEATHER_ID,                                    // item id
        new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 250) // the way the good is purchase
    );

    public static EquippableVG EQ_LEGS_LOAFERS = new EquippableVG(
        EquippableVG.EquippingModel.CATEGORY,                        // equipping model
        "Loafers",                                          // name
        "Loafers",                                    // description
        EQ_LEGS_LOAFERS_ID,                                    // item id
        new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 250) // the way the good is purchase
    );

    public static EquippableVG EQ_LEGS_SNEAKERS = new EquippableVG(
        EquippableVG.EquippingModel.CATEGORY,                        // equipping model
        "Sneakers",                                          // name
        "Increases atheletic performance. Increases time by a medium amount.",                                    // description
        EQ_LEGS_SNEAKERS_ID,                                    // item id
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 35) // the way the good is purchase
    );

    public static EquippableVG EQ_LEGS_SUIT = new EquippableVG(
        EquippableVG.EquippingModel.CATEGORY,                        // equipping model
        "Suit pants",                                          // name
        "Too expensive to be soiled. Increases time by a small amount.",                                    // description
        EQ_LEGS_SUIT_ID,                                    // item id
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 25) // the way the good is purchase
    );

    #endregion
    #region Consumables

    public static SingleUseVG CONSUMABLE_CHARCOAL_1 = new SingleUseVG(
        "Charcoal Lite",                                              // name 
        "Medication which will temporarily relieve you from the minor symptoms of diarrhea.",                                  // description
        CONSUMABLE_CHARCOAL_1_ID,                                    // item id
        new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 25) // the way the good is purchased
    );

    public static SingleUseVG CONSUMABLE_CHARCOAL_2 = new SingleUseVG(
        "Charcoal",                                               // name 
        "Medication which will temporarily relieve you from some symptoms of diarrhea.",                                   // description
        CONSUMABLE_CHARCOAL_2_ID,                                     // item id
        new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 75) // the way the good is purchased
    );

    public static SingleUseVG CONSUMABLE_CHARCOAL_3 = new SingleUseVG(
        "Extra Strength Charcoal",                                              // name 
        "Medication which will temporarily relieve you from the major symptoms of diarrhea.",                                  // description
        CONSUMABLE_CHARCOAL_3_ID,                                    // item id
        new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 125) // the way the good is purchased
    );

    public static SingleUseVG CONSUMABLE_LUCKY_CHARM = new SingleUseVG(
        "Four Leaf Clover",
        "This charm give you a chance to find more plungers. Destroyed upon soiled pants.",
        CONSUMABLE_LUCKY_CHARM_ID,
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 7)
    );

    // Non-purchasable
    public static SingleUseVG CONSUMABLE_PLUNGER = new SingleUseVG(
        "[Plunger]",
        "[Plunger Description]",
        CONSUMABLE_PLUNGER_ID,
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 7)
    );

    #region Item Packs

    public static SingleUsePackVG CONSUMABLE_CHARCOAL_1_5PACK = new SingleUsePackVG(
        CONSUMABLE_CHARCOAL_1_ID,   // Single item id
        5,                          // amount
        "Pack of Charcoal Lite",      // name
        "A pack of 5 Charcoal Lites", // description
        CONSUMABLE_CHARCOAL_1_5PACK_ID, // Item pack ID
        new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 115) // price
    );

    public static SingleUsePackVG CONSUMABLE_CHARCOAL_2_5PACK = new SingleUsePackVG(
        CONSUMABLE_CHARCOAL_2_ID,   // Single item id
        5,                          // amount
        "Pack of Charcoal",      // name
        "A pack of 5 Charcoals", // description
        CONSUMABLE_CHARCOAL_2_5PACK_ID, // Item pack ID
        new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 340) // price
    );

    public static SingleUsePackVG CONSUMABLE_CHARCOAL_3_5PACK = new SingleUsePackVG(
        CONSUMABLE_CHARCOAL_3_ID,   // Single item id
        5,                          // amount
        "Pack of Extra Strength Charcoal",      // name
        "A pack of 5 Extra Strength Charcoals", // description
        CONSUMABLE_CHARCOAL_3_5PACK_ID, // Item pack ID
        new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 560) // price
    );

    public static SingleUsePackVG CONSUMABLE_CHARCOAL_1_10PACK = new SingleUsePackVG(
        CONSUMABLE_CHARCOAL_1_ID,   // Single item id
        10,                          // amount
        "Bag of Charcoal Lite",      // name
        "A bag of 10 Charcoal Lites", // description
        CONSUMABLE_CHARCOAL_1_10PACK_ID, // Item pack ID
        new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 220) // price
    );

    public static SingleUsePackVG CONSUMABLE_CHARCOAL_2_10PACK = new SingleUsePackVG(
        CONSUMABLE_CHARCOAL_2_ID,   // Single item id
        10,                          // amount
        "Bag of Charcoal",      // name
        "A bag of 10 Charcoals", // description
        CONSUMABLE_CHARCOAL_2_10PACK_ID, // Item pack ID
        new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 650) // price
    );

    public static SingleUsePackVG CONSUMABLE_CHARCOAL_3_10PACK = new SingleUsePackVG(
        CONSUMABLE_CHARCOAL_3_ID,   // Single item id
        10,                          // amount
        "Bag of Extra Strength Charcoal",      // name
        "A bag of 10 Extra Strength Charcoals", // description
        CONSUMABLE_CHARCOAL_3_10PACK_ID, // Item pack ID
        new PurchaseWithVirtualItem(NORMAL_TOILET_PAPER_ID, 1000) // price
    );

    #endregion

    #endregion

    #region Instant Use Items

    #region Virtual Currency Packs

    public static VirtualCurrencyPack PACK_1_NTP = new VirtualCurrencyPack(
        "25 Normal Toilet Papers",                          // name
        "25 Normal Toilet Papers",                          // description
        PACK_1_NTP_ID,                                     // item id
        25,                                                 // number of currencies in pack
        NORMAL_TOILET_PAPER_ID,                        // currency associated
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 1)   // the way the good is purchased
    );
        
    public static VirtualCurrencyPack PACK_2_NTP = new VirtualCurrencyPack(
        "125 Normal Toilet Papers",                          // name
        "125 Normal Toilet Papers",                          // description
        PACK_2_NTP_ID,                                     // item id
        125,                                                 // number of currencies in pack
        NORMAL_TOILET_PAPER_ID,                        // currency associated
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 5)   // the way the good is purchased
    );
    
    public static VirtualCurrencyPack PACK_3_NTP = new VirtualCurrencyPack(
        "625 Normal Toilet Papers",                          // name
        "625 Normal Toilet Papers",                          // description
        PACK_3_NTP_ID,                                     // item id
        625,                                                 // number of currencies in pack
        NORMAL_TOILET_PAPER_ID,                        // currency associated
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 25)   // the way the good is purchased
    );

    public static VirtualCurrencyPack PACK_4_NTP = new VirtualCurrencyPack(
        "1250 Normal Toilet Papers",                          // name
        "1250 Normal Toilet Papers",                          // description
        PACK_4_NTP_ID,                                     // item id
        1250,                                                 // number of currencies in pack
        NORMAL_TOILET_PAPER_ID,                        // currency associated
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 50)   // the way the good is purchased
    );
    
    public static VirtualCurrencyPack PACK_5_NTP = new VirtualCurrencyPack(
        "2500 Normal Toilet Papers",                          // name
        "2500 Normal Toilet Papers",                          // description
        PACK_5_NTP_ID,                                     // item id
        2500,                                                 // number of currencies in pack
        NORMAL_TOILET_PAPER_ID,                        // currency associated
        new PurchaseWithVirtualItem(GOLDEN_TOILET_PAPER_ID, 100)   // the way the good is purchased
    );

    public static VirtualCurrencyPack PACK_1_GTP = new VirtualCurrencyPack(
        "10 Golden Toilet Papers",                          // name
        "10 Golden Toilet Papers",                          // description
        PACK_1_GTP_ID,                                     // item id
        10,                                                 // number of currencies in pack
        GOLDEN_TOILET_PAPER_ID,                        // currency associated
        new PurchaseWithMarket(PACK_1_GTP_ID, 1.99)   // the way the good is purchased
    );

    public static VirtualCurrencyPack PACK_2_GTP = new VirtualCurrencyPack(
        "20 Golden Toilet Papers",                          // name
        "20 Golden Toilet Papers",                          // description
        PACK_2_GTP_ID,                                     // item id
        20,                                                 // number of currencies in pack
        GOLDEN_TOILET_PAPER_ID,                        // currency associated
        new PurchaseWithMarket(PACK_2_GTP_ID, 3.98)   // the way the good is purchased
    );

    public static VirtualCurrencyPack PACK_3_GTP = new VirtualCurrencyPack(
        "55 Golden Toilet Papers",                          // name
        "55 Golden Toilet Papers",                          // description
        PACK_3_GTP_ID,                                     // item id
        55,                                                 // number of currencies in pack
        GOLDEN_TOILET_PAPER_ID,                        // currency associated
        new PurchaseWithMarket(PACK_3_GTP_ID, 9.99)   // the way the good is purchased
    );
    public static VirtualCurrencyPack PACK_4_GTP = new VirtualCurrencyPack(
        "112 Golden Toilet Papers",                          // name
        "112 Golden Toilet Papers",                          // description
        PACK_4_GTP_ID,                                     // item id
        112,                                                 // number of currencies in pack
        GOLDEN_TOILET_PAPER_ID,                        // currency associated
        new PurchaseWithMarket(PACK_4_GTP_ID,19.99)   // the way the good is purchased
    );
    public static VirtualCurrencyPack PACK_5_GTP = new VirtualCurrencyPack(
        "290 Golden Toilet Papers",                          // name
        "290 Golden Toilet Papers",                          // description
        PACK_5_GTP_ID,                                     // item id
        290,                                                 // number of currencies in pack
        GOLDEN_TOILET_PAPER_ID,                        // currency associated
        new PurchaseWithMarket(PACK_5_GTP_ID,49.99)   // the way the good is purchased
    );
    public static VirtualCurrencyPack PACK_6_GTP = new VirtualCurrencyPack(
        "600 Golden Toilet Papers",                          // name
        "600 Golden Toilet Papers",                          // description
        PACK_6_GTP_ID,                                     // item id
        600,                                                 // number of currencies in pack
        GOLDEN_TOILET_PAPER_ID,                        // currency associated
        new PurchaseWithMarket(PACK_1_GTP_ID,99.99)   // the way the good is purchased
    );
    #endregion

    #endregion
    #region Non Shop Items

    public static SingleUseVG EMERGENCY_REVIVE_1_1 = new SingleUseVG(
        "Emergency Diapers (Rank 1)",                                               // name
        "Emergency Diapers (Rank 1)",                                     // description
        EMERGENCY_REVIVE_1_1_ID,                                     // item id
        new PurchaseWithMarket(EMERGENCY_REVIVE_1_1_ID, 0.99) // the way the good is purchased
    );
    public static SingleUseVG EMERGENCY_REVIVE_1_2 = new SingleUseVG(
        "Emergency Diapers (Rank 1)",                                               // name
        "Emergency Diapers (Rank 1)",                                  // description
        EMERGENCY_REVIVE_1_2_ID,                                     // item id
        new PurchaseWithMarket(EMERGENCY_REVIVE_1_2_ID, 1.25) // the way the good is purchased
    );
    public static SingleUseVG EMERGENCY_REVIVE_1_3 = new SingleUseVG(
        "Emergency Diapers (Rank 1)",                                               // name
        "Emergency Diapers (Rank 1)",                                    // description
        EMERGENCY_REVIVE_1_3_ID,                                     // item id
        new PurchaseWithMarket(EMERGENCY_REVIVE_1_3_ID, 1.5) // the way the good is purchased
    );

    public static SingleUseVG EMERGENCY_REVIVE_2_1 = new SingleUseVG(
        "Emergency Diapers (Rank 2)",                                               // name
        "Emergency Diapers (Rank 2)",                                    // description
        EMERGENCY_REVIVE_2_1_ID,                                     // item id
        new PurchaseWithMarket(EMERGENCY_REVIVE_2_1_ID, 1.75) // the way the good is purchased
    );
    public static SingleUseVG EMERGENCY_REVIVE_2_2 = new SingleUseVG(
        "Emergency Diapers (Rank 2)",                                               // name
        "Emergency Diapers (Rank 2)",                                     // description
        EMERGENCY_REVIVE_2_2_ID,                                     // item id
        new PurchaseWithMarket(EMERGENCY_REVIVE_2_2_ID, 2.25) // the way the good is purchased
    );
    public static SingleUseVG EMERGENCY_REVIVE_2_3 = new SingleUseVG(
        "Emergency Diapers (Rank 2)",                                               // name
        "Emergency Diapers (Rank 2)",                                     // description
        EMERGENCY_REVIVE_2_3_ID,                                     // item id
        new PurchaseWithMarket(EMERGENCY_REVIVE_2_3_ID, 2.75) // the way the good is purchased
    );

    public static SingleUseVG EMERGENCY_REVIVE_3_1 = new SingleUseVG(
        "Emergency Diapers (Rank 3)",                                               // name
        "Emergency Diapers (Rank 3)",                                   // description
        EMERGENCY_REVIVE_3_1_ID,                                     // item id
        new PurchaseWithMarket(EMERGENCY_REVIVE_3_1_ID, 3.00) // the way the good is purchased
    );
    public static SingleUseVG EMERGENCY_REVIVE_3_2 = new SingleUseVG(
        "Emergency Diapers (Rank 3)",                                               // name
        "Emergency Diapers (Rank 3)",                                  // description
        EMERGENCY_REVIVE_3_2_ID,                                     // item id
        new PurchaseWithMarket(EMERGENCY_REVIVE_3_2_ID, 3.75) // the way the good is purchased
    );
    public static SingleUseVG EMERGENCY_REVIVE_3_3 = new SingleUseVG(
        "Emergency Diapers (Rank 3)",                                               // name
        "Emergency Diapers (Rank 3)",                                     // description
        EMERGENCY_REVIVE_3_3_ID,                                     // item id
        new PurchaseWithMarket(EMERGENCY_REVIVE_3_3_ID, 4.5) // the way the good is purchased
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

    // Equipment legs category
    public static VirtualCategory EQUIP_LEGS_CATEGORY = new VirtualCategory(
        EQ_LEGS_CATEGORY_NAME,     // name
        EQ_LEGS_LIST               // items in category
    );

    #endregion
}
