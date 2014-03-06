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
}
