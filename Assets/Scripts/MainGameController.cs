using UnityEngine;
using System.Collections;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.storage;
using com.shephertz.app42.paas.sdk.csharp.log;

public class MainGameController : MonoBehaviour
{
    public bool isGameMenu = false;

    #region Timer Variables

    public float maxTime;
    public float timeElapsed;
    private float timerReductionRate = 1; // Defaults to 1
    public bool timerPaused = false;

    #endregion
    #region Map Variables

    public bool mapEnabled = true;
    public Camera mainCamera;
    public float zoomLevel = 3;
    public float zoomOutLevel;
    public bool zoomIn;
    public bool zoomOut;

    public float bgScaling = 2;
    private Vector3 bgZoomOutScale;
    private Vector3 bgZoomInScale;
    private Vector3 curBgScale;
    private GameObject[] backgrounds;

    #endregion
    #region CUrrency Variables
    int ntpMax; // Stores the total number of ntp in a stage
    int gtpMax; // Stores the total number of gtp in a stage
    #endregion

    public AudioClip loopingClip;

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

    public int reward;
    private MainGameGUI mainGUI;
    private PauseGUI pauseGUI;
    private FailedGUI failGUI;
    private FailedByFallingGUI failByFallingGUI;
    private StageCompleteGUI stageCompleteGUI;


    #if UNITY_EDITOR
        public static bool Validator(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        { return true; }
    #endif

	void Awake () {
        // Initialise timer components
        Time.timeScale = 1f;
        timeElapsed = 0;

        // Initialise objectives
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        mainGUI = gameObject.GetComponentInChildren<MainGameGUI>();
        pauseGUI = gameObject.GetComponentInChildren<PauseGUI>();
        stageCompleteGUI = gameObject.GetComponentInChildren<StageCompleteGUI>();
        failGUI = gameObject.GetComponentInChildren<FailedGUI>();

        failByFallingGUI = GameObject.Find("GUI Fail by Falling").GetComponent<FailedByFallingGUI>();

        // Get the total number of ntp and gtp
        ntpMax = GameObject.FindGameObjectsWithTag("ntp").Length;
        gtpMax = GameObject.FindGameObjectsWithTag("gtp").Length;

        // Retrieve all backgrounds
        backgrounds = GameObject.FindGameObjectsWithTag("Background");
        bgZoomInScale = backgrounds[0].transform.localScale;
        bgZoomOutScale = bgZoomInScale * bgScaling;
        curBgScale = bgZoomInScale;
        #region App42

        #if UNITY_EDITOR
            ServicePointManager.ServerCertificateValidationCallback = Validator;
        #endif

        #endregion
	}

	// Update is called once per frame
    void Update()
    {
        // Update timer
        if (timeElapsed < maxTime)
        {
            if (!timerPaused)
            {
                timeElapsed += Time.deltaTime * timerReductionRate;
            }
        }
        else
        {
            if (!timerPaused)
            {
                GameOver(false);
            }
        }

        // play looping part of audio
        if (!audio.isPlaying)
        {
            audio.clip = loopingClip;
            audio.loop = true;
            audio.Play();
        }

        if (zoomIn)
        {
            ZoomIn();
            zoomIn = false;
        }
        if (zoomOut)
        {
            ZoomOut();
            zoomOut = false;
        }
    }

    void OnDestroy()
    {
    }

    public void setTimerReductionRate(float rate)
    {        
        timerReductionRate = rate;
    }

    public void pickupToiletPaper(AudioClip sfxPickup)
    {
        mainGUI.ntp += 1;
        audio.PlayOneShot(sfxPickup, 1f);
    }

    public void pickupGoldenToiletPaper(AudioClip sfxPickup)
    {
        mainGUI.gtp += 1;
        audio.PlayOneShot(sfxPickup, 1f);
    }

    public void StageComplete()
    {
        // Disable time and movement
        DisableTimeNMove();
        // Pop the stage complete menu
        int mins = (int)(timeElapsed / 60);
        int seconds = (int)(timeElapsed % 60);
        string timeTaken = string.Format("{0:00}:{1:00}", mins, seconds);
        stageCompleteGUI.StageComplete(timeTaken, mainGUI.ntp, ntpMax, mainGUI.gtp, gtpMax, objectives);
    }

    public void GameOver(bool fell)
    {
        // Disable time and movement
        DisableTimeNMove();
        // pop up the failed menu
        if (fell)
        {
            mainGUI.Hide();
            failByFallingGUI.StageFailed();
        }
        else
        {
            mainGUI.Hide();
            failGUI.Show();
        }
    }

	public void PauseGame() 
    {
        // Disable time and movement
        DisableTimeNMove();
        // Pop up the pause menu
        pauseGUI.PauseGame();
	}

	public void ResumeGame() {
		// Hide the pause menu
        pauseGUI.ResumeGame();
        // Disable time and movement
        EnableTimeNMove();
	}

    public void ToggleMap(bool enabled)
    {
        if (mapEnabled != enabled)
        {
            mapEnabled = enabled;
        }
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

    public void ZoomIn()
    {
        // Set ismoving flag (to prevent other commands from coming in)
        character.isMoving = true;
        //Enable Controls
        character.enabled = true;
        // Animate the zoom
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", zoomLevel,
                                   "to", 3,
                                   "onupdate", "animateZoom",
                                   "oncomplete", "zoomInComplete",
                                   "easetype", iTween.EaseType.linear,
                                   "time", 0.5));
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", curBgScale,
                                   "to", bgZoomInScale,
                                   "onupdate", "scaleBackground",
                                   "easetype", iTween.EaseType.linear,
                                   "time", 0.5));
    }

    public void ZoomOut()
    {
        // Set ismoving flag (to prevent other commands from coming in)
        character.isMoving = true;
        // Disable Controls
        character.enabled = false;
        // Animate the zoom
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", zoomLevel,
                                   "to", zoomOutLevel,
                                   "onupdate", "animateZoom",
                                   "oncomplete", "zoomOutComplete",
                                   "easetype", iTween.EaseType.linear,
                                   "time", 0.5));
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", curBgScale,
                                   "to", bgZoomOutScale,
                                   "onupdate", "scaleBackground",
                                   "easetype", iTween.EaseType.linear,
                                   "time", 0.5));

        // Alter culling mask
        camera.cullingMask = ~(1 << LayerMask.NameToLayer("Character"));
    }

	public void DisableTimeNMove() {
        // Pause the timer
        timerPaused = true;
        // Disable Controls
        character.enabled = false;
	}

    public void EnableTimeNMove()
    {
        //Enable Controls
        character.enabled = true;
        // Resume timer
        timerPaused = false;
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
        var gameData = Game.Load();
        gameData.stats[Type2.totalSteps] += moves;
        gameData.stats[Type2.totalHangingSteps] += hangingMoves;
        gameData.playingTime = gameData.playingTime.Add(new System.TimeSpan(0,0,(int)time));
        gameData.stats[Type2.totalClimbs] += climbs;
        gameData.stats[Type2.totalPulls] += pulls;
        gameData.stats[Type2.totalPushes] += pushes;
        gameData.stats[Type2.totalSlides] += slides;
        gameData.stats[Type2.totalPullOuts] += pullOuts;
        gameData.stats[Type2.toiletPapers] += mainGUI.ntp;
        gameData.stats[Type2.goldenPapers] += mainGUI.gtp;
        gameData.stats[Type2.stagesCompleted] += 1;
        //gameData.stars[Application.loadedLevelName] = reward;

        gameData.Save();
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
                        objectives[i].completed = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (moves > objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else
                {
                    if (moves == objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
            }
            else if (objectives[i].type == Type.climbs)
            {
                if (objectives[i].option == Option.lessThan)
                {
                    if (climbs < objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (climbs > objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else
                {
                    if (climbs == objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
            }
            else if (objectives[i].type == Type.time)
            {
                if (objectives[i].option == Option.lessThan)
                {
                    if (time < objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (time > objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else
                {
                    if (time == objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
            }
            else if (objectives[i].type == Type.pulls)
            {
                if (objectives[i].option == Option.lessThan)
                {
                    if (pulls < objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (pulls > objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else
                {
                    if (pulls == objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
            }
            else if (objectives[i].type == Type.pushes)
            {
                if (objectives[i].option == Option.lessThan)
                {
                    if (pushes < objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (pushes > objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else
                {
                    if (pushes == objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
            }
            else if (objectives[i].type == Type.pullOuts)
            {
                if (objectives[i].option == Option.lessThan)
                {
                    if (pullOuts < objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (pullOuts > objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else
                {
                    if (pullOuts == objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
            }
            else if (objectives[i].type == Type.hangingSteps)
            {
                if (objectives[i].option == Option.lessThan)
                {
                    if (hangingMoves < objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (hangingMoves > objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else
                {
                    if (hangingMoves == objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
            }
            else if (objectives[i].type == Type.slides)
            {
                if (objectives[i].option == Option.lessThan)
                {
                    if (slides < objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (slides > objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else
                {
                    if (slides == objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
            }
            else if (objectives[i].type == Type.toiletPapers)
            {
                if (objectives[i].option == Option.lessThan)
                {
                    if (mainGUI.ntp < objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (mainGUI.ntp > objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else
                {
                    if (mainGUI.ntp == objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
            }
            else if (objectives[i].type == Type.goldenPapers)
            {
                if (objectives[i].option == Option.lessThan)
                {
                    if (mainGUI.gtp < objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (mainGUI.gtp > objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else
                {
                    if (mainGUI.gtp == objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
            }
            else if (objectives[i].type == Type.withoutClimb)
            {
                if (climbs == 0)
                {
                    objectives[i].completed = true;
                }
            }
            else if (objectives[i].type == Type.noHanging)
            {
                if (hangingMoves == 0)
                {
                    objectives[i].completed = true;
                }
            }
            else if (objectives[i].type == Type.noPulling)
            {
                if (pulls == 0)
                {
                    objectives[i].completed = true;
                }
            }
            else if (objectives[i].type == Type.noPushing)
            {
                if (pushes == 0)
                {
                    objectives[i].completed = true;
                }
            }
            else if (objectives[i].type == Type.noPullOuts)
            {
                if (pullOuts == 0)
                {
                    objectives[i].completed = true;
                }
            }
        }

        reward = 0;

        if (objectives[0].completed == true)
        {
            reward += 1;
        }
        if (objectives[1].completed == true)
        {
            reward += 1;
        }
        if (objectives[2].completed == true)
        {
            reward += 1;
        }
    }

    // iTween camera ortographic size
    void animateZoom(float zoom)
    {
        zoomLevel = zoom;
        Camera.main.GetComponent<Camera>().orthographicSize = zoom;
    }

    void scaleBackground(Vector3 scale)
    {
        curBgScale = scale;
        foreach (GameObject background in backgrounds)
        {
            background.transform.localScale = scale;
        }
    }

    void zoomInComplete()
    {
        // Alter culling mask
        camera.cullingMask = ~(1 << LayerMask.NameToLayer("Minimap"));
        character.isMoving = false;
    }
    void zoomOutComplete()
    {
        character.isMoving = false;
    }
}
