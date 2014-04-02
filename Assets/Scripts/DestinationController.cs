using UnityEngine;
using System.Collections;

public class DestinationController : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<CharacterController>().reachedDestination = true;
        }
    }
}
