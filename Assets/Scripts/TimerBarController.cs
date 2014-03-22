using UnityEngine;
using System.Collections;

public class TimerBarController : MonoBehaviour {

	public float maxTime;
	private float timeElapsed;
    public float timerReductionRate = 1; // Defaults to 1

    private MainGameController game;

	// Use this for initialization
	void Start () {
		Time.timeScale = 1f;
		timeElapsed = maxTime;

        game = Camera.main.GetComponent<MainGameController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(timeElapsed >= 0) {
            timeElapsed -= Time.deltaTime * timerReductionRate;
			transform.localScale = new Vector3 (timeElapsed / maxTime, 0.5f, 1f);
		} else {
            game.GameOver();
			/*Time.timeScale = 0;
			Camera.main.GetComponent<FailedGUI>().enabled = true;*/
		}
	}

}
