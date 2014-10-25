using UnityEngine;
using System.Collections;
using Soomla;
using Soomla.Store;

public class LootController : MonoBehaviour {

    public AudioClip sfxPickup;

    // Item Types
    public enum ItemType
    {
        NTP,
        Capsule
    }
    public ItemType itemType;
    public Capsule capsule;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (itemType == ItemType.NTP)
            {
                MainGameController.instance.pickupToiletPaper(sfxPickup);
            }
            else if (itemType == ItemType.Capsule)
            {
                MainGameController.instance.pickupCapsule(sfxPickup, capsule);
            }
        }
    }
}
