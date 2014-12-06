using UnityEngine;
using Soomla;
using Soomla.Store;
using System.Collections;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.storage;
using com.shephertz.app42.paas.sdk.csharp.log;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

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
    public List<Capsule> capsules = new List<Capsule>(); // stores collected capsules
    private int ntpMax; // Stores the total number of ntp in a stage
    private int gtpMax; // Stores the total number of gtp in a stage
    private int capsuleMax; // Stores the total number of capsules in a stage

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
    public int failedAttempts;
    public int consumablesUsed;

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

        // If game menu, disable right after initialisation
        if (isGameMenu)
        {
            this.enabled = false;
            return;
        }

        ToggleSound(Game.instance.audio);

        // Initialise timer components
        Time.timeScale = 1f;
        timeElapsed = 0;
        isUrgent = false;

        // Initialise variables
        failedAttempts = 0;
        consumablesUsed = 0;

        // Get the total number of ntp and gtp
        ntpMax = GameObject.FindGameObjectsWithTag("ntp").Length;
        gtpMax = GameObject.FindGameObjectsWithTag("gtp").Length;
        capsuleMax = GameObject.FindGameObjectsWithTag("capsule").Length;

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

    void Start()
    {
        // Consume a plunger if it is a challenge stage
        if (NavigationManager.instance.chapter >= 7)
        {
            StoreInventory.TakeItem(CrapTrapAssets.CONSUMABLE_PLUNGER_ID, 1);
        }

        // Adjust max time based on gear
        maxTime = maxTime * (1 + InventoryManager.instance.GetBonusTime());

        if (InventoryManager.instance.setBonus == SetBonus.Explorer)
        {
            zoomOutLevel = 13;
        }
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

        if (!BGMManager.instance.IsStage())
        {
            BGMManager.instance.PlayStage();
        }

        GetComponent<AudioSource>().enabled = true;
        audio.volume = 1f;

        // play looping part of audio
        // If the timer has 33.3% left
        float timerPulseRate = (1 - timeElapsed / maxTime) * 1.5f;

        if (timerPulseRate < 0.5)
        {
            BGMManager.instance.PlayUrgent();
        }
        else
        {
            BGMManager.instance.PlayStage();
            
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

    public void pickupCapsule(AudioClip sfxPickup, Capsule capsule)
    {
        capsules.Add(capsule);
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
        
        // Update NTP
        InventoryManager.instance.AddNTP(ntp);

        #region Stage Completed Without Items Used
        // Check if the player uses any items
        if (consumablesUsed == 0)
        {
            Game.instance.stagesCompletedWOItems[NavigationManager.instance.chapter][NavigationManager.instance.stage] = true;
        }
        #endregion

        #region Stage Completed Without Equipment
        // variable to check if anything is equipped
        bool isEquipped = false;

        // check if anything is equipped in head
        foreach (string eq in CrapTrapAssets.EQ_HEAD_LIST)
        {
            if (Soomla.Store.StoreInventory.IsVirtualGoodEquipped(eq))
            {
                isEquipped = true;
            }
        }

        if (isEquipped)
        {
            // check if anything is equipped in upper body
            foreach (string eq in CrapTrapAssets.EQ_BODY_LIST)
            {
                if (Soomla.Store.StoreInventory.IsVirtualGoodEquipped(eq))
                {
                    isEquipped = true;
                }
            }
        }

        if (isEquipped)
        {
            // check if anything is equipped in legs
            foreach (string eq in CrapTrapAssets.EQ_LEGS_LIST)
            {
                if (Soomla.Store.StoreInventory.IsVirtualGoodEquipped(eq))
                {
                    isEquipped = true;
                }
            }
        }

        if (!isEquipped)
        {
            Game.instance.stagesCompletedWOEq[NavigationManager.instance.chapter][NavigationManager.instance.stage] = true;
        }
        #endregion

        #region Stage attempted With Diver Suit
        if(Soomla.Store.StoreInventory.IsVirtualGoodEquipped("eq_head_set_diver") ||
           Soomla.Store.StoreInventory.IsVirtualGoodEquipped("eq_body_set_diver") ||
           Soomla.Store.StoreInventory.IsVirtualGoodEquipped("eq_legs_set_diver"))
        {
            Game.instance.stagesAttemptedWDiver[NavigationManager.instance.chapter][NavigationManager.instance.stage] = true;
        }
        #endregion

        // Pop up the stage complete screen.
            StageCompleteGUI.instance.StageComplete(timeTaken, ntp, ntpMax, capsuleMax, capsules, objectives);

        /*
        // post score to Leaderboard ID
        Social.ReportScore(System.Convert.ToInt64(timeElapsed), NavigationManager.instance.getLeaderboardID(), (bool success) =>
        {
            // handle success or failure
        });
         */

    }

    public void GameOver(bool fell)
    {
        Game.instance.stats[Stat.fails] += 1;
        // Disable time and movement
        DisableTimeNMove();

        #region Stage attempted With Diver Suit
        if (Soomla.Store.StoreInventory.IsVirtualGoodEquipped("eq_head_set_diver") ||
           Soomla.Store.StoreInventory.IsVirtualGoodEquipped("eq_body_set_diver") ||
           Soomla.Store.StoreInventory.IsVirtualGoodEquipped("eq_legs_set_diver"))
        {
            Game.instance.stagesAttemptedWDiver[NavigationManager.instance.chapter][NavigationManager.instance.stage] = true;
        }
        #endregion

        // pop up the failed menu
        if (fell)
        {
            MainGameGUI.instance.Hide();
            InventoryManager.instance.ConsumeCharms();
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
            failedAttempts++;

            MainGameGUI.instance.Hide();
            InventoryManager.instance.ConsumeCharms();
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

    public void Revive(float extension)
    {
        timeElapsed = timeElapsed - extension;
        FailedGUI.instance.Hide();
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
        if (InventoryManager.instance.setBonus == SetBonus.Explorer)
        {
            camera.cullingMask = ~(1 << LayerMask.NameToLayer("Character"));
        }
        else
        {
            camera.cullingMask = ~(1 << LayerMask.NameToLayer("Character") | 1 << LayerMask.NameToLayer("Loot"));
        }
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
        Game.instance.stats[Stat.playingTime] += time;

        Game.instance.stats[Stat.totalClimbs] += climbs;
        Game.instance.stats[Stat.totalPulls] += pulls;
        Game.instance.stats[Stat.totalPushes] += pushes;
        Game.instance.stats[Stat.totalSlides] += slides;
        Game.instance.stats[Stat.totalPullOuts] += pullOuts;
        Game.instance.stats[Stat.toiletPapers] += ntp;
        //Game.instance.stats[Stat.goldenPapers] += gtp;
        Game.instance.stats[Stat.stagesCompleted] += 1;
        Game.instance.UpdateStats(NavigationManager.instance.chapter, NavigationManager.instance.stage, reward);
        //gameData.stars[Application.loadedLevelName] = reward;
        Game.instance.stats[Stat.capsules] += capsules.Count;

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
            else if (objectives[i].type == Type.capsules)
            {
                if (objectives[i].option == Option.lessThan)
                {
                    if (capsules.Count < objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else if (objectives[i].option == Option.greaterThan)
                {
                    if (capsules.Count > objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
                }
                else
                {
                    if (capsules.Count == objectives[i].counter)
                    {
                        objectives[i].completed = true;
                    }
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
