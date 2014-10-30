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
    }

    public EquipmentSlot slot;
    public Sprite defaultSprite;
    public List<EquipmentSprite> eqSprites = new List<EquipmentSprite>();

	// Use this for initialization
	void Start () {

        EquipmentSprite eq = null;
	    
        if (slot == EquipmentSlot.Head)
        {
            eq = eqSprites.Where(x => x.itemID == InventoryManager.instance.equippedHead.itemId).First();
        }
        else if (slot == EquipmentSlot.Body)
        {
            eq = eqSprites.Where(x => x.itemID == InventoryManager.instance.equippedBody.itemId).First();
        }
        else if (slot == EquipmentSlot.Legs)
        {
            eq = eqSprites.Where(x => x.itemID == InventoryManager.instance.equippedLegs.itemId).First();
        }

        // Render the sprites
        if (eq != null)
        {
            GetComponent<SpriteRenderer>().sprite = eq.sprite;
        }
        else if (defaultSprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = defaultSprite;
        }
	}
	
}
