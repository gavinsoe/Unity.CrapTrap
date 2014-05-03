using UnityEngine;
using System.Collections;

public class MainGameController : MonoBehaviour {

    // Components
    private TimerBarController timer; // The game timer
    private ScoreController score;
    private MainGameGUI main;

	// Use this for initialization
	void Start () {
        timer = gameObject.GetComponentInChildren<TimerBarController>();
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreController>();
        main = gameObject.GetComponentInChildren<MainGameGUI>();
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
        main.ntp += 1;
    }

    public void pickupGoldenToiletPaper()
    {
        main.gtp += 1;
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        Camera.main.GetComponent<FailedGUI>().enabled = true;
    }

	public void PauseGame() {
		Time.timeScale = 0;
	}

	public void ResumeGame() {
		Time.timeScale = 1;
	}

	public void DisableTimeNMove() {
        // Disable Controls
		GameObject.FindGameObjectWithTag("Player").GetComponent<PCControls>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTouchControls>().enabled = false;
        // Disable Timer
		Camera.main.GetComponentInChildren<TimerBarController>().enabled = false;
	}

	public void EnableTimeNMove() {
        // Enable Controls
        GameObject.FindGameObjectWithTag("Player").GetComponent<PCControls>().enabled = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTouchControls>().enabled = true;
        // Enable Timer
		Camera.main.GetComponentInChildren<TimerBarController>().enabled = true;
	}
}
