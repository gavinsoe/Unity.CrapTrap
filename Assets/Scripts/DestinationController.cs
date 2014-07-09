using UnityEngine;
using System.Collections;

public class DestinationController : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            CharacterController character = col.gameObject.GetComponent<CharacterController>();
            if (!character.isHanging)
            {
                col.gameObject.GetComponent<CharacterController>().reachedDestination = true;
            }
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            CharacterController character = col.gameObject.GetComponent<CharacterController>();
            if (!character.isHanging)
            {
                col.gameObject.GetComponent<CharacterController>().reachedDestination = true;
            }
        }
    }
}
