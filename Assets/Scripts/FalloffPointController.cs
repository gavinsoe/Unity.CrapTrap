using UnityEngine;
using System.Collections;

public class FalloffPointController : MonoBehaviour {
    private MainGameController game;

    // Use this for initialization
    void Start()
    {
        game = Camera.main.GetComponent<MainGameController>();
    }

    // Activates the block when it collides with the "Block Activator" collider, which is attached to the character
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            game.GameOver();
        }
    }
}
