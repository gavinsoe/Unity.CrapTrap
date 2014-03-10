using UnityEngine;
using System.Collections;

public class MainGameController : MonoBehaviour {

    // Components
    private TimerBarController timer; // The game timer
    private ScoreController score;

	// Use this for initialization
	void Start () {
        timer = gameObject.GetComponentInChildren<TimerBarController>();
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setTimerReductionRate(float rate)
    {        
        timer.timerReductionRate = rate;
    }

    public void pickupToiletPaper()
    {
        score.toiletPaper += 1;
    }

    public void pickupGoldenToiletPaper()
    {
        score.goldenToiletPaper += 1;
    }

	public void PauseGame() {
		Time.timeScale = 0;
	}

	public void ResumeGame() {
		Time.timeScale = 1;
	}

	public void DisableTimeNMove() {
		GameObject.FindGameObjectWithTag("Player").GetComponent<PCControls>().enabled = false;
		Camera.main.GetComponentInChildren<TimerBarController>().enabled = false;
	}

	public void EnableTimeNMove() {
		GameObject.FindGameObjectWithTag("Player").GetComponent<PCControls>().enabled = true;
		Camera.main.GetComponentInChildren<TimerBarController>().enabled = true;
	}
}
