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
    // App42 Stuff
    ServiceAPI serviceAPI;
    StorageService storageService;
    Constants constants = new Constants();
    LogResponse logCallBack = new LogResponse();

    public static MainGameController instance;

    public bool isGameMenu = false;

    #region Timer Variables

    public float maxTime;
    public float timeElapsed;
    private float timerReductionRate = 1; // Defaults to 1
    public bool timerPaused = false;
    public bool isUrgent;

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
    #region Currency Variables

    public int ntp = 0; // Stores the number of ntp collected
    public int gtp = 0; // Stores the number of gtp collected
    private int ntpMax; // Stores the total number of ntp in a stage
    private int gtpMax; // Stores the total number of gtp in a stage

    #endregion

    public AudioClip loopingClip;
    public AudioClip urgentClip;

    public float vol;
    public float t1;
    public float t2;
    public float timeRightNow;
    public float crossfadeTime;

    // Components
    public int moves;
    public int hangingMoves;
    public float time;
    public int climbs;
    public int pulls;
    public int pushes;
    public int slides;
    public int pullOuts;

    public Objective[] objectives = new Objective[3];

    public int reward;

    #if UNITY_EDITOR
        public static bool Validator(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        { return true; }
    #endif

	void Awake () 
    {
        // set the static variable so that other classes can easily use this class
        instance = this;

        // Initialise timer components
        Time.timeScale = 1f;
        timeElapsed = 0;
        isUrgent = false;

        // Get the total number of ntp and gtp
        ntpMax = GameObject.FindGameObjectsWithTag("ntp").Length;
        gtpMax = GameObject.FindGameObjectsWithTag("gtp").Length;

        // Retrieve all backgrounds
        backgrounds = GameObject.FindGameObjectsWithTag("Background");
        bgZoomInScale = backgrounds[0].transform.localScale;
        bgZoomOutScale = bgZoomInScale * bgScaling;
        curBgScale = bgZoomInScale;

        // Alter culling mask ot hide map indicator
        camera.cullingMask = ~(1 << LayerMask.NameToLayer("Minimap"));

        #region App42

        #if UNITY_EDITOR
            ServicePointManager.ServerCertificateValidationCallback = Validator;
        #endif

            // Connect to the app service
            serviceAPI = new ServiceAPI(constants.apiKey, constants.secretKey);

            // Build the storage service
            storageService = serviceAPI.BuildStorageService();

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
        // If the timer has 33.3% left
        float timerPulseRate = (1 - timeElapsed / maxTime) * 1.5f;

        if (timerPulseRate < 0.5)
        {
            if (!audio.isPlaying && isUrgent) // If audio is not playing and it's urgent then play urgent music
            {
                t1 = 0;
                t2 = 0;
                vol = audio.volume;
                timeRightNow = timeElapsed;
                audio.clip = urgentClip;
                audio.loop = true;
                audio.Play();
            }
            else if (audio.isPlaying && !isUrgent) // If normal music is playing and it is not urgent then change to urgent music
            {
                if (t1 < crossfadeTime) // Fade out normal music
                {
                    t1 = timeElapsed - timeRightNow;
                    audio.volume = (crossfadeTime - t1) * vol;
                }
                else if (audio.isPlaying && t1 >= crossfadeTime && t2 == 0f) // Change music to urgent
                {
                    t2 = 0.1f;
                    audio.Stop();
                    audio.clip = urgentClip;
                    audio.loop = true;
                    audio.Play();
                    timeRightNow = timeElapsed;
                    
                }
                else if (t1 >= crossfadeTime && t2 < crossfadeTime) // Fade in urgent music
                {
                    t2 = timeElapsed - timeRightNow;
                    audio.volume = t2 * vol;
                }
                else
                {
                    isUrgent = true;
                }
            }
            else
            {
                t1 = 0;
                t2 = 0;
                vol = audio.volume;
                timeRightNow = timeElapsed;
            }
        }
        else
        {
            
            if (!audio.isPlaying && !isUrgent)
            {
                t1 = 0;
                t2 = 0;
                vol = audio.volume;
                timeRightNow = timeElapsed;
                audio.clip = loopingClip;
                audio.loop = true;
                audio.Play();
            }
            else if (audio.isPlaying && isUrgent)
            {
                if (t1 < crossfadeTime)
                {
                    t1 = timeElapsed - timeRightNow;
                    audio.volume = (crossfadeTime - t1) * vol;
                }
                else if (audio.isPlaying && t1 >= crossfadeTime && t2 == 0f)
                {
                    audio.Stop();
                    audio.clip = loopingClip;
                    audio.loop = true;
                    audio.Play();
                    timeRightNow = timeElapsed;
                    t2 = 0.1f;
                }
                else if (t1 >= crossfadeTime && t2 < crossfadeTime)
                {
                    t2 = timeElapsed - timeRightNow;
                    audio.volume = t2 * vol;
                }
                else
                {
                    isUrgent = false;
                }
            }
            else
            {
                t1 = 0;
                t2 = 0;
                vol = audio.volume;
                timeRightNow = timeElapsed;
            }
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
        ntp += 1;
        audio.PlayOneShot(sfxPickup, 1f);
    }

    public void pickupGoldenToiletPaper(AudioClip sfxPickup)
    {
        gtp += 1;
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
        UpdateStats();
        StageCompleteGUI.instance.StageComplete(timeTaken, ntp, ntpMax, gtp, gtpMax, objectives);

        #region App42 Result Tracking

        // Package the result
        SimpleJSON.JSONClass json = new SimpleJSON.JSONClass();
        json.Add("Device id", SystemInfo.deviceUniqueIdentifier);
        json.Add("Stage", Application.loadedLevelName);
        json.Add("Time", timeTaken);
        json.Add("NTP", (ntp + @"/" + ntpMax).ToString());
        json.Add("GTP", (gtp + @"/" + gtpMax).ToString());
        json.Add("Objective_1", objectives[0].ResultStat());
        json.Add("Objective_2", objectives[1].ResultStat());
        json.Add("Objective_3", objectives[2].ResultStat());
        json.Add("Result", "Complete");

        // Log result
        storageService.InsertJSONDocument(constants.dbName, constants.collectionStageStats, json, logCallBack);

        #endregion
    }

    public void GameOver(bool fell)
    {
        // Disable time and movement
        DisableTimeNMove();
        // pop up the failed menu
        if (fell)
        {
            MainGameGUI.instance.Hide();
            FailedByFallingGUI.instance.StageFailed();

            /*
            #region App42 Result Tracking

            // Package the result
            int mins = (int)(timeElapsed / 60);
            int seconds = (int)(timeElapsed % 60);
            string timeTaken = string.Format("{0:00}:{1:00}", mins, seconds);

            SimpleJSON.JSONClass json = new SimpleJSON.JSONClass();
            json.Add("Device id", SystemInfo.deviceUniqueIdentifier);
            json.Add("Stage", Application.loadedLevelName);
            json.Add("Time", timeTaken);
            json.Add("NTP", (ntp + @"/" + ntpMax).ToString());
            json.Add("GTP", (gtp + @"/" + gtpMax).ToString());
            json.Add("Objective_1", objectives[0].ResultStat());
            json.Add("Objective_2", objectives[1].ResultStat());
            json.Add("Objective_3", objectives[2].ResultStat());
            json.Add("Result", "Fell Down");

            // Log result
            storageService.InsertJSONDocument(constants.dbName, constants.collectionStageStats, json, logCallBack);

            #endregion
             * */
        }
        else
        {
            MainGameGUI.instance.Hide();
            FailedGUI.instance.Show();

            /*
            #region App42 Result Tracking

            // Package the result
            int mins = (int)(timeElapsed / 60);
            int seconds = (int)(timeElapsed % 60);
            string timeTaken = string.Format("{0:00}:{1:00}", mins, seconds);

            SimpleJSON.JSONClass json = new SimpleJSON.JSONClass();
            json.Add("Device id", SystemInfo.deviceUniqueIdentifier);
            json.Add("Stage", Application.loadedLevelName);
            json.Add("Time", timeTaken);
            json.Add("NTP", (ntp + @"/" + ntpMax).ToString());
            json.Add("GTP", (gtp + @"/" + gtpMax).ToString());
            json.Add("Objective_1", objectives[0].ResultStat());
            json.Add("Objective_2", objectives[1].ResultStat());
            json.Add("Objective_3", objectives[2].ResultStat());
            json.Add("Result", "Out of time");

            // Log result
            storageService.InsertJSONDocument(constants.dbName, constants.collectionStageStats, json, logCallBack);
            
            #endregion
             * */
        }
    }

	public void PauseGame() 
    {
        // Disable time and movement
        DisableTimeNMove();
        // Pop up the pause menu
        PauseGUI.instance.PauseGame();
	}

	public void ResumeGame() {
		// Hide the pause menu
        PauseGUI.instance.ResumeGame();
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
        // Do not zoom when game is paused
        if (timerPaused) return;
        // Set ismoving flag (to prevent other commands from coming in)
        CharacterController.instance.isMoving = true;
        //Enable Controls
        CharacterController.instance.enabled = true;
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
        // Do not zoom when game is paused
        if (timerPaused) return;
        // Set ismoving flag (to prevent other commands from coming in)
        CharacterController.instance.isMoving = true;
        // Disable Controls
        CharacterController.instance.enabled = false;
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
        CharacterController.instance.enabled = false;
	}

    public void EnableTimeNMove()
    {
        //Enable Controls
        CharacterController.instance.enabled = true;
        // Resume timer
        timerPaused = false;
	}

    public void UpdateStats()
    {
        time += timeElapsed;
        // Get counters from CharacterController
        moves = CharacterController.instance.moves;
        hangingMoves = CharacterController.instance.hangingMoves;
        climbs = CharacterController.instance.climbs;
        pulls = CharacterController.instance.pulls;
        pushes = CharacterController.instance.pushes;
        slides = CharacterController.instance.slides;
        pullOuts = CharacterController.instance.pullOuts;

        // Check the objectives
        CheckObjectives();

        // Update the game counters;
        
        Game.instance.stats[Stat.totalSteps] += moves;
        Game.instance.stats[Stat.totalHangingSteps] += hangingMoves;
        Game.instance.playingTime = Game.instance.playingTime.Add(new System.TimeSpan(0, 0, (int)time));

        Game.instance.stats[Stat.totalClimbs] += climbs;
        Game.instance.stats[Stat.totalPulls] += pulls;
        Game.instance.stats[Stat.totalPushes] += pushes;
        Game.instance.stats[Stat.totalSlides] += slides;
        Game.instance.stats[Stat.totalPullOuts] += pullOuts;
        Game.instance.stats[Stat.toiletPapers] += ntp;
        Game.instance.stats[Stat.goldenPapers] += gtp;
        Game.instance.stats[Stat.stagesCompleted] += 1;
        Game.instance.UpdateStats(NavigationManager.instance.chapter, NavigationManager.instance.stage, reward);
        //gameData.stars[Application.loadedLevelName] = reward;

        // Save progress
        Game.instance.Save();
        
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
                    if (time < objectives[i].counter + 1)
                    {
                        objectives[i].completed = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (time >= objectives[i].counter)
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
                    if (ntp < objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (ntp > objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else
                {
                    if (ntp == objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
            }
            else if (objectives[i].type == Type.goldenPapers)
            {
                if (objectives[i].option == Option.lessThan)
                {
                    if (gtp < objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (gtp > objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else
                {
                    if (gtp == objectives[i].counter)
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
        CharacterController.instance.isMoving = false;
    }
    void zoomOutComplete()
    {
        CharacterController.instance.isMoving = false;
    }
}
