using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {

	private float X;
	public int offset;
	public bool FollowCamera;

	// Use this for initialization
	void Start () {
		X = Camera.main.transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		if (FollowCamera){
			Vector3 camera_pos = Camera.main.transform.position;
			transform.position = new Vector3((camera_pos.x - X)/offset, camera_pos.y, transform.position.z);
		} else {
			Vector3 camera_pos = Camera.main.transform.position;
			transform.position = new Vector3((X - camera_pos.x)/offset, camera_pos.y, transform.position.z);
		}
	}
}
