using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EquipmentRenderer : MonoBehaviour {

    public enum EquipmentSlot { Head, Body, Legs };
    
    [System.Serializable]
    public class EquipmentSprite
    {
        public string itemID;
        public Sprite sprite;
        public int orderInLayer;
    }

    public EquipmentSlot slot;
    public Sprite defaultSprite;
    public List<EquipmentSprite> eqSprites = new List<EquipmentSprite>();

	// Use this for initialization
	void Update () {

        EquipmentSprite eq = null;
	    
        if (slot == EquipmentSlot.Head &&
            eqSprites.Exists(x => x.itemID == InventoryManager.instance.equippedHead.itemId))
        {
            eq = eqSprites.Where(x => x.itemID == InventoryManager.instance.equippedHead.itemId).First();
        }
        else if (slot == EquipmentSlot.Body &&
                 eqSprites.Exists(x => x.itemID == InventoryManager.instance.equippedBody.itemId))
        {
            eq = eqSprites.Where(x => x.itemID == InventoryManager.instance.equippedBody.itemId).First();
        }
        else if (slot == EquipmentSlot.Legs &&
                 eqSprites.Exists(x => x.itemID == InventoryManager.instance.equippedLegs.itemId))
        {
            eq = eqSprites.Where(x => x.itemID == InventoryManager.instance.equippedLegs.itemId).First();
        }

        // Render the sprites
        if (eq != null)
        {
            GetComponent<SpriteRenderer>().sprite = eq.sprite;
            GetComponent<SpriteRenderer>().sortingOrder = eq.orderInLayer;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = defaultSprite;
        }
	}
	
}
