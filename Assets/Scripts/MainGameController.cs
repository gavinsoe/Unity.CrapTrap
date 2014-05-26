using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

public class MainGameController : MonoBehaviour
{

    #region Timer Variables

    public float maxTime;
    public float timeElapsed;
    private float timerReductionRate = 1; // Defaults to 1
    private bool timerPaused = false;

    #endregion
    #region Map Variables

    public bool mapEnabled = true;
    private Camera minimap;

    #endregion
    #region CUrrency Variables
    int ntpMax; // Stores the total number of ntp in a stage
    int gtpMax; // Stores the total number of gtp in a stage
    #endregion

    public enum Type
    {
        steps = 1,
        climbs = 2,
        time = 3,
        withoutClimb = 4,
        toiletPapers = 5,
        goldenPapers = 6,
        pulls = 7,
        pushes = 8,
        pullOuts = 9,
        hangingSteps = 10,
        slides = 11,
        noHanging = 12,
        noPulling = 13,
        noPushing = 14,
        treasures = 15,
        noPullOuts = 16,
    }

    public enum Option
    {
        lessThan = 1,
        greaterThan = 2,
        equal = 3,
    }

    [System.Serializable]
    public class Objective
    {
        public Type type;
        public Option option;
        public int counter;
    }

    // Components
    private CharacterController character; // the character controller
    private int moves;
    private int hangingMoves;
    private float time;
    private int climbs;
    private int pulls;
    private int pushes;
    private int slides;
    private int pullOuts;

    public Objective[] objectives = new Objective[3];

    public bool[] objectiveFlags = new bool[3];
    public int reward;
    private MainGameGUI mainGUI;
    private PauseGUI pauseGUI;
    private FailedGUI failedGUI;
    private GameCompletedGUI stageCompleteGUI;

	// Use this for initialization
	void Start () {
        // Initialise timer components
        Time.timeScale = 1f;
        timeElapsed = 0;

        // Initialise objectives
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        objectiveFlags[0] = false;
        objectiveFlags[1] = false;
        objectiveFlags[2] = false;
        mainGUI = gameObject.GetComponentInChildren<MainGameGUI>();
        pauseGUI = gameObject.GetComponentInChildren<PauseGUI>();
        stageCompleteGUI = gameObject.GetComponentInChildren<GameCompletedGUI>();
        failedGUI = gameObject.GetComponentInChildren<FailedGUI>();

        // Obtain the minimap component
        minimap = GameObject.FindGameObjectWithTag("Minimap").GetComponent<Camera>();

        // Get the total number of ntp and gtp
        ntpMax = GameObject.FindGameObjectsWithTag("ntp").Length;
        gtpMax = GameObject.FindGameObjectsWithTag("gtp").Length;
	}
	
	// Update is called once per frame
	void Update () {
	    // Update timer
        if (timeElapsed < maxTime)
        {
            if (!timerPaused)
            {
                timeElapsed += Time.deltaTime * timerReductionRate;
                updateTimerPulseRate();
            }
        }
        else
        {
            if (!timerPaused)
            {
                GameOver();
            }
        }
	}

    public void updateTimerPulseRate()
    {
        if ((timeElapsed / maxTime) < 0.25)
        {
            mainGUI.timerPulseRate = 2;
        }
        else if ((timeElapsed / maxTime) < 0.5)
        {
            mainGUI.timerPulseRate = 1f;
        }
        else if ((timeElapsed / maxTime) < 0.75)
        {
            mainGUI.timerPulseRate = 0.75f;
        }
        else if ((timeElapsed / maxTime) < 0.85)
        {
            mainGUI.timerPulseRate = 0.5f;
        }
        else if ((timeElapsed / maxTime) < 0.95)
        {
            mainGUI.timerPulseRate = 0.25f;
        }
    }

    public void setTimerReductionRate(float rate)
    {        
        timerReductionRate = rate;
    }

    public void pickupToiletPaper()
    {
        mainGUI.ntp += 1;
    }

    public void pickupGoldenToiletPaper()
    {
        mainGUI.gtp += 1;
    }

    public void StageComplete()
    {
        // Stop the timer
        timerPaused = true;
        // Disable controls
        character.enabled = false;
        // Pop the stage complete menu
        stageCompleteGUI.StageComplete(timeElapsed,mainGUI.ntp,ntpMax,mainGUI.gtp,gtpMax);
    }

    public void GameOver()
    {
        // Stop the timer
        timerPaused = true;
        // Disable controls
        character.enabled = false;
        // pop up the failed menu
        failedGUI.StageFailed();
    }

	public void PauseGame() {
        // Pause the timer
        timerPaused = true;
        // Disable Controls
        character.enabled = false;
        // Pop up the pause menu
        pauseGUI.PauseGame();
	}

	public void ResumeGame() {
		// Hide the pause menu
        pauseGUI.ResumeGame();
        //Enable Controls
        character.enabled = true;
        // Resume timer
        timerPaused = false;
	}

    public void RetryLevel()
    {
        // Restart level
        Application.LoadLevel(Application.loadedLevel);
    }

    public void ReturnToTitle()
    {
        // Return to title screen
        Application.LoadLevel("GUI_TitleScreen");
    }

    public void NextStage()
    {
        // Loads the next stage (or screen)
        Application.LoadLevel(Application.loadedLevel + 1);
    }

    public void ToggleMap(bool enabled)
    {
        mapEnabled = enabled;
        minimap.enabled = enabled;
    }

    public void ToggleSound(bool enabled)
    {
        if (enabled)
        {
            AudioListener.volume = 1.0f;
        }
        else
        {
            AudioListener.volume = 0.0f;
        }
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

    public void UpdateStats()
    {
        CharacterController character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        time += timeElapsed;
        // Get counters from CharacterController
        moves = character.moves;
        hangingMoves = character.hangingMoves;
        climbs = character.climbs;
        pulls = character.pulls;
        pushes = character.pushes;
        slides = character.slides;
        pullOuts = character.pullOuts;

        // Check the objectives
        CheckObjectives();

        // Update the game counters;
        var gameData = Game.Load(Path.Combine(Application.dataPath, "game.xml"));
        gameData.moves += moves;
        gameData.hangingMoves += hangingMoves;
        gameData.time += time;
        gameData.climbs += climbs;
        gameData.pulls += pulls;
        gameData.pushes += pushes;
        gameData.slides += slides;
        gameData.pullOuts += pullOuts;
        gameData.toiletPapers += mainGUI.ntp;
        gameData.goldPapers += mainGUI.gtp;
        gameData.stagesDone += 1;

        if (!gameData.levels.ContainsKey(Application.loadedLevel))
        {
            gameData.levels.Add(Application.loadedLevel, reward);
        }
        else
        {
            gameData.levels[Application.loadedLevel] = reward;
        }

        gameData.Save(Path.Combine(Application.persistentDataPath, "game.xml"));
    }

    public void CheckObjectives()
    {
        int i;
        for (i = 0; i < 3; i++)
        {
            if (objectives[i].type == Type.steps)
            {
                if (objectives[i].option == Option.lessThan)
                {
                    if (moves < objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (moves > objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
                else
                {
                    if (moves == objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
            }
            else if (objectives[i].type == Type.climbs)
            {
                if (objectives[i].option == Option.lessThan)
                {
                    if (climbs < objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (climbs > objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
                else
                {
                    if (climbs == objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
            }
            else if (objectives[i].type == Type.time)
            {
                if (objectives[i].option == Option.lessThan)
                {
                    if (time < objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (time > objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
                else
                {
                    if (time == objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
            }
            else if (objectives[i].type == Type.pulls)
            {
                if (objectives[i].option == Option.lessThan)
                {
                    if (pulls < objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (pulls > objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
                else
                {
                    if (pulls == objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
            }
            else if (objectives[i].type == Type.pushes)
            {
                if (objectives[i].option == Option.lessThan)
                {
                    if (pushes < objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (pushes > objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
                else
                {
                    if (pushes == objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
            }
            else if (objectives[i].type == Type.pullOuts)
            {
                if (objectives[i].option == Option.lessThan)
                {
                    if (pullOuts < objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (pullOuts > objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
                else
                {
                    if (pullOuts == objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
            }
            else if (objectives[i].type == Type.hangingSteps)
            {
                if (objectives[i].option == Option.lessThan)
                {
                    if (hangingMoves < objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (hangingMoves > objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
                else
                {
                    if (hangingMoves == objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
            }
            else if (objectives[i].type == Type.slides)
            {
                if (objectives[i].option == Option.lessThan)
                {
                    if (slides < objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (slides > objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
                else
                {
                    if (slides == objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
            }
            else if (objectives[i].type == Type.toiletPapers)
            {
                if (objectives[i].option == Option.lessThan)
                {
                    if (mainGUI.ntp < objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (mainGUI.ntp > objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
                else
                {
                    if (mainGUI.ntp == objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
            }
            else if (objectives[i].type == Type.goldenPapers)
            {
                if (objectives[i].option == Option.lessThan)
                {
                    if (mainGUI.gtp < objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (mainGUI.gtp > objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
                else
                {
                    if (mainGUI.gtp == objectives[i].counter)
                    {
                        objectiveFlags[i] = true;
                    }
                }
            }
            else if (objectives[i].type == Type.withoutClimb)
            {
                if (climbs == 0)
                {
                    objectiveFlags[i] = true;
                }
            }
            else if (objectives[i].type == Type.noHanging)
            {
                if (hangingMoves == 0)
                {
                    objectiveFlags[i] = true;
                }
            }
            else if (objectives[i].type == Type.noPulling)
            {
                if (pulls == 0)
                {
                    objectiveFlags[i] = true;
                }
            }
            else if (objectives[i].type == Type.noPushing)
            {
                if (pushes == 0)
                {
                    objectiveFlags[i] = true;
                }
            }
            else if (objectives[i].type == Type.noPullOuts)
            {
                if (pullOuts == 0)
                {
                    objectiveFlags[i] = true;
                }
            }
        }

        reward = 0;

        if (objectiveFlags[0] == false)
        {
            objectiveFlags[1] = false;
            objectiveFlags[2] = false;
        }
        else
        {
            reward += 1;
        }
        if (objectiveFlags[1] == false)
        {
            objectiveFlags[2] = false;
        }
        else
        {
            reward += 1;
        }
        if (objectiveFlags[2] == true)
        {
            reward += 1;
        }
    }
}
