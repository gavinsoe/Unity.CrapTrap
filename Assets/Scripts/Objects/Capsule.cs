using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Capsule : System.Object
{
    public Loot[] loot;

    public Loot obtainLoot()
    {  
        // Check how many lucky charms are inside the bag
        int luckyCharmCount = 0;
        foreach (Item consumable in InventoryManager.instance.equippedConsumables)
        {
            if (consumable.itemId == CrapTrapAssets.CONSUMABLE_LUCKY_CHARM_ID)
            {
                luckyCharmCount++;
            }
        }

        // Adjust the drop rate (if plungers are part of the loot).
        if (luckyCharmCount > 0)
        {
            if (loot.Any<Loot>(x => x.itemID == CrapTrapAssets.CONSUMABLE_PLUNGER_ID))
            {
                float plungerPercent = 0;

                // Update drop rate of plunger
                foreach (Loot lootItem in loot)
                {
                    if (lootItem.itemID == CrapTrapAssets.CONSUMABLE_PLUNGER_ID)
                    {
                        plungerPercent = lootItem.percentageChance + 5 * luckyCharmCount;
                        lootItem.percentageChance = plungerPercent;
                        break;
                    }
                }

                // Update drop rate of other items
                foreach (Loot lootItem in loot)
                {
                    if (lootItem.itemID != CrapTrapAssets.CONSUMABLE_PLUNGER_ID)
                    {
                        lootItem.percentageChance = (lootItem.percentageChance / 100) * (100 - plungerPercent);
                    }
                }
            }
        }

        float totalPercentage = loot.Sum<Loot>(x => x.percentageChance);
        var index = Random.Range(0.00f, totalPercentage);
        
        foreach (Loot lootItem in loot)
        {
            index = index - lootItem.percentageChance;
            if (index < 0) return lootItem;
        }

        // Return first element by default
        return loot[0];
    }
}
