using UnityEngine;
using System.Collections;

public class BackgroundParallax : MonoBehaviour {

    public bool FollowCamera;
    public bool MoveContinously;
    public float speed;

    private float X;


	// Use this for initialization
    void Start()
    {
        X = Camera.main.transform.position.x;
	}
	
	// Update is called once per frame
    void Update()
    {
        Vector3 camera_pos = Camera.main.transform.position;
        
        // The parallax effect. (With the offset from above)
        if (FollowCamera)
        {
            if (MoveContinously) 
            {
                renderer.material.mainTextureOffset = new Vector2(Time.time * speed, 0f);
            }
            else
            {
                renderer.material.mainTextureOffset = new Vector2((camera_pos.x - X) * speed, 0f);
            }
        }
        else
        {
            if (MoveContinously)
            {
                renderer.material.mainTextureOffset = new Vector2(-Time.time * speed, 0f);
            }
            else
            {
                renderer.material.mainTextureOffset = new Vector2((X - camera_pos.x) * speed, 0f);
            }
        }
	}
}
