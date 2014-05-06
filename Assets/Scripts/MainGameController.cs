using UnityEngine;
using System.Collections;

public class MainGameController : MonoBehaviour
{

    #region Timer Variables
    public float maxTime;
    private float timeElapsed;
    private float timerReductionRate = 1; // Defaults to 1
    #endregion

    // Components
    private TimerBarController timer; // The game timer
    private MainGameGUI main;

	// Use this for initialization
	void Start () {
        // Initialise timer components
        Time.timeScale = 1f;
        timeElapsed = maxTime;

        timer = gameObject.GetComponentInChildren<TimerBarController>();
        main = gameObject.GetComponentInChildren<MainGameGUI>();
	}
	
	// Update is called once per frame
	void Update () {
	    // Update timer
        if (timeElapsed >= 0)
        {
            timeElapsed -= Time.deltaTime * timerReductionRate;
            updateTimerPulseRate();
        }
        else
        {
            GameOver();
        }
	}

    public void updateTimerPulseRate()
    {
        if ((timeElapsed / maxTime) > 0.75)
        {
            main.timerPulseRate = 2;
        }
        else if ((timeElapsed / maxTime) > 0.5)
        {
            main.timerPulseRate = 1f;
        }
        else if ((timeElapsed / maxTime) > 0.25)
        {
            main.timerPulseRate = 0.75f;
        }
        else if ((timeElapsed / maxTime) > 0.15)
        {
            main.timerPulseRate = 0.5f;
        }
        else if ((timeElapsed / maxTime) > 0.05)
        {
            main.timerPulseRate = 0.25f;
        }
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
		// Camera.main.GetComponentInChildren<TimerBarController>().enabled = false;
	}

	public void EnableTimeNMove() {
        // Enable Controls
        GameObject.FindGameObjectWithTag("Player").GetComponent<PCControls>().enabled = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTouchControls>().enabled = true;
        // Enable Timer
		//Camera.main.GetComponentInChildren<TimerBarController>().enabled = true;
	}
}
