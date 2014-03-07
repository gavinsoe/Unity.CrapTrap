using UnityEngine;
using System.Collections;

public class DestinationController : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Character")
        {
            col.gameObject.GetComponent<CharacterController>().reachedDestination = true;
        }
    }
}
