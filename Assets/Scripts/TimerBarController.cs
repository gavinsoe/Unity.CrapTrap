using UnityEngine;
using System.Collections;

public class TimerBarController : MonoBehaviour {

	public float maxTime;
	private float timeElapsed;

	// Use this for initialization
	void Start () {
		timeElapsed = maxTime;
	}
	
	// Update is called once per frame
	void Update () {
		if(timeElapsed >= 0) {
			timeElapsed -= Time.deltaTime;
			transform.localScale = new Vector3 (timeElapsed / maxTime, 0.5f, 1f);
		}
	}
}
