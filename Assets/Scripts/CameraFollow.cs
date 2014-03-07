﻿using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
    public enum CameraStatus { Introduction, TrackPlayer }
    public CameraStatus camState; // Defaults to introduction

    public float initialZoom = 1f;  // Initial zoom level when previewing the destination
    public float gameplayZoom = 3f; // Zoom level for gameplay
    public float zoomSmooth = 4f;

	public float xMargin = 1f;		// Distance in the x axis the player can move before the camera follows.
	public float yMargin = 1f;		// Distance in the y axis the player can move before the camera follows.
	public float xSmooth = 8f;		// How smoothly the camera catches up with it's target movement in the x axis.
	public float ySmooth = 8f;		// How smoothly the camera catches up with it's target movement in the y axis.
	//public Vector2 maxXAndY;		// The maximum x and y coordinates the camera can have.
	//public Vector2 minXAndY;		// The minimum x and y coordinates the camera can have.
	
	private Transform player;		// Reference to the player's transform.

    void Start()
    {
        // Set the starting position to the final destination
        Vector3 finishLocation = GameObject.FindGameObjectWithTag("Finish").transform.position;
        transform.position = new Vector3(finishLocation.x, finishLocation.y, transform.position.z);

		Camera.main.GetComponent<MainGameController>().DisableTimeNMove();

        // Set default state to 'Introduction', and set the initial zoom level
        camera.orthographicSize = initialZoom;
        camState = CameraStatus.Introduction;
    }
	
	void Awake ()
	{
		// Setting up the reference.
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	bool CheckXMargin()
	{
		// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
		return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
	}
	
	bool CheckYMargin()
	{
		// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
		return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
	}
	
	void FixedUpdate ()
	{
        if (camState == CameraStatus.Introduction)
        {
            // Set the starting animation
            var camera = transform.GetComponent<Camera>();
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, gameplayZoom, zoomSmooth * Time.deltaTime);

            if ((gameplayZoom - camera.orthographicSize) < 0.01)
            {
                camera.orthographicSize = gameplayZoom;
                camState = CameraStatus.TrackPlayer;
				Camera.main.GetComponent<MainGameController>().EnableTimeNMove();
            }
        }
        else if (camState == CameraStatus.TrackPlayer)
        {
            TrackPlayer();
        }
	}
	
	void TrackPlayer ()
	{
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float targetX = transform.position.x;
		float targetY = transform.position.y;
		
		// If the player has moved beyond the x margin...
		if(CheckXMargin())
			// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
			targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);
		
		// If the player has moved beyond the y margin...
		if(CheckYMargin())
			// ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
			targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);
		
		// The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
		//targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
		//targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);
		
		// Set the camera's position to the target position with the same z component.
		transform.position = new Vector3(targetX, targetY, transform.position.z);
	}
}
