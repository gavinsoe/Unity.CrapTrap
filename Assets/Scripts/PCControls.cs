using UnityEngine;
using System.Collections;

public class PCControls : MonoBehaviour {

    // The component that handles all the character movement.
    private CharacterController character;

    // Use this for initialization
    void Start()
    {
        // Retrieve the characterController
        character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!character.isMoving)
        {
            var inputH = Input.GetAxis("Horizontal");
            var inputV = Input.GetAxis("Vertical");

            if (inputH != 0)
            {
                if (Input.GetKey("o"))
                {
                    StartCoroutine(character.move(transform, System.Math.Sign(inputH), true));
                }
                else if (Input.GetKey("p"))
                {
                    StartCoroutine(character.move(transform, System.Math.Sign(inputH), true));
                }
                else
                {
                    StartCoroutine(character.move(transform, System.Math.Sign(inputH), false));
                }
            }
            else if (inputV != 0)
            {
                StartCoroutine(character.hang(transform, System.Math.Sign(inputV)));
            }
            else if (Input.GetKey("l"))
            {
                StartCoroutine(character.pull(transform));
            }
            else if (Input.GetKey(KeyCode.Escape))
            {
                Time.timeScale = 0;
                Camera.main.GetComponent<PauseGUI>().enabled = true;
            }
            else
            {
                character.idle();
            }
        }
    }
}
