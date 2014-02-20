using UnityEngine;
using System.Collections;

public class SpriteRandomizer : MonoBehaviour {

    public Sprite[] sprites;

	// Use this for initialization
	void Start () {
		//var new_sprite = sprites[Random.Range(0, sprites.Length)];
		GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)]; 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
