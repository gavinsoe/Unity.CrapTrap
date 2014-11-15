using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Loot : System.Object
{
    public string itemID;
    public ItemType itemType;
    public float percentageChance;
    public int quantity;

    Texture2D icon
    {
        get
        {
            return (Texture2D)Resources.Load(itemID);
        }
    }
}
