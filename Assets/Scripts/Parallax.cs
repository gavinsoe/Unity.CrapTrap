using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {

	private float X;
	public int offset;
	public bool FollowCamera;

    private Bounds bg_bounds;
    private float bg_offset;
    private float bg_tile_size;

	// Use this for initialization
	void Start () {
		X = Camera.main.transform.position.x;

        // Retrieve the bounds of the background
        bg_bounds = transform.gameObject.GetComponent<MeshRenderer>().bounds;
        // Initialise background offset to 0
        bg_offset = 0f;
        // calculate the tile size of the background.
        bg_tile_size = bg_bounds.size.x / transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 camera_pos = Camera.main.transform.position;

        // Check if character is going out of initial bounds (bounds are set to the size of the tile
        // If character did go out of bounds, reposition background to follow.
        if ((camera_pos.x - bg_bounds.center.x) > bg_tile_size)
        {
            bg_offset += bg_tile_size;
            bg_bounds.center = new Vector3(bg_bounds.center.x + bg_tile_size, bg_bounds.center.y, bg_bounds.center.z);
        }
        if ((camera_pos.x - bg_bounds.center.x) < -bg_tile_size)
        {
            bg_offset -= bg_tile_size;
            bg_bounds.center = new Vector3(bg_bounds.center.x - bg_tile_size, bg_bounds.center.y, bg_bounds.center.z);
        }

        // The parallax effect. (With the offset from above)
		if (FollowCamera){
            transform.position = new Vector3(((camera_pos.x - X) / offset) + bg_offset, camera_pos.y, transform.position.z);
		} else {
            transform.position = new Vector3((X - camera_pos.x) / offset, camera_pos.y, transform.position.z);
		}
	}
}
