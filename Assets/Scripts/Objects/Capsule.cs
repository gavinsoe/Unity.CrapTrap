using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Capsule : System.Object
{
    public Loot[] loot;

    public Loot obtainLoot()
    {    
        var index = Random.Range(0.00f, 100.00f);
        
        foreach (Loot lootItem in loot)
        {
            index = index - lootItem.percentageChance;
            if (index < 0) return lootItem;
        }

        // Return first element by default
        return loot[0];
    }
}
