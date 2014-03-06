using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour {

    private MainGameController game;

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
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Character")
        {
            // Do some logic to add score/item
            if (itemType == ItemType.ToiletPaper)
            {
                game.pickupToiletPaper();
            }
            else if (itemType == ItemType.GoldenToiletPaper)
            {
                game.pickupGoldenToiletPaper();
            }

            // Destroy this thing
            Destroy(gameObject);
        }
    }
}
