using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store.CrapTrap
{
    public class CrapTrapAssets : IStoreAssets
    {

        public int GetVersion()
        {
            return 0;
        }

        public VirtualCurrency[] GetCurrencies()
        {
            return new VirtualCurrency[] { TOILET_PAPER, GOLDEN_TOILET_PAPER };
        }

        public VirtualGood[] GetGoods()
        {
            return new VirtualGood[] { NORIT_GOOD };
        }

        public VirtualCurrencyPack[] GetCurrencyPacks()
        {
            return new VirtualCurrencyPack[] { TENGTP_PACK };
        }

        public VirtualCategory[] GetCategories()
        {
            return new VirtualCategory[] { EQUIP_CATEGORY };
        }

        public NonConsumableItem[] GetNonConsumableItems()
        {
            return new NonConsumableItem[] { NO_ADDS_NONCONS };
        }

        #region Static Final Members

        public const string TOILET_PAPER_ITEM_ID        = "toilet_paper";
        public const string GOLDEN_TOILET_PAPER_ITEM_ID = "golden_toilet_paper";
        public const string TENGTP_PACK_ITEM_ID              = "android.test.purchased";
        public const string FIVENORIT_PACK_ITEM_ID      = "android.test.purchased";
        public const string NO_ADDS_NONCONS_PRODUCT_ID  = "no_ads";
        public const string NORIT_ITEM_ID               = "norit";

        #endregion
        #region Virtual Currencies

        public static VirtualCurrency TOILET_PAPER = new VirtualCurrency(
            "Toilet Paper",         // name
            "",                     // description
            TOILET_PAPER_ITEM_ID    // item id
        );

        public static VirtualCurrency GOLDEN_TOILET_PAPER = new VirtualCurrency(
            "Golden Toilet Paper",          // name
            "",                             // description
            GOLDEN_TOILET_PAPER_ITEM_ID     // item id
        );

        #endregion
        #region Virtual Currency Packs

        public static VirtualCurrencyPack TENGTP_PACK = new VirtualCurrencyPack(
            "5 GTP",                                            // name
            "5 Golden Toilet Papers",                           // description
            "gtp_5",                                            // item id
            5,                                                  // number of currencies in pack
            GOLDEN_TOILET_PAPER_ITEM_ID,                        // currency associated
            new PurchaseWithMarket(TENGTP_PACK_ITEM_ID, 2.99)   // the way the good is purchased
        );

        #endregion
        #region Item Packs

        public static VirtualGood FIVENORIT_PACK = new SingleUsePackVG(
            NORIT_ITEM_ID,                                          // item associated
            5,                                                      // amount
            "5 Charcoals",                                          // name
            "5 pieces of Charcoal Medicine",                        // description
            "norit_5",                                              // item id                       
            new PurchaseWithMarket(FIVENORIT_PACK_ITEM_ID, 2.99)
        );

        #endregion
        #region Virtual Goods

        public static VirtualGood NORIT_GOOD = new SingleUseVG(
            "Charcoal Medicine",                                    // name
            "Medicine to increase time",                            // description
            "norit",                                                // item id
            new PurchaseWithVirtualItem(TOILET_PAPER_ITEM_ID, 50)   // the way the good is purchased
        );

        #endregion
        #region Virtual Categories

        public static VirtualCategory EQUIP_CATEGORY = new VirtualCategory(
            "Equipment",                                        // name
            new List<string>(new string[] { NORIT_ITEM_ID })    // items in category
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
}
