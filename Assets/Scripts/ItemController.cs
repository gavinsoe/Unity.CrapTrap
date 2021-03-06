﻿using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour {

    private MainGameController game;
    public AudioClip sfxPickup;

    // Item list
    public enum ItemType
    {
        ToiletPaper,
        GoldenToiletPaper
    }
    public ItemType itemType;

	// Use this for initialization
	void Start () {
        game = Camera.main.GetComponent<MainGameController>();
	}
	
    void OnTriggerEnter2D(Collider2D col)
    {
        //if (Debug.isDebugBuild) Debug.Log("Collide!! " + col.gameObject.tag);
        if (col.gameObject.tag == "Player")
        {
            // Do some logic to add score/item
            if (itemType == ItemType.ToiletPaper)
            {
                game.pickupToiletPaper(sfxPickup);
            }
            else if (itemType == ItemType.GoldenToiletPaper)
            {
                game.pickupGoldenToiletPaper(sfxPickup);
            }

            // Destroy this thing
            Destroy(gameObject);
        }
    }
}
