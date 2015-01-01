using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using UnityEngine;
using ChartboostSDK;

public class Game : MonoBehaviour 
{
    public static Game instance;

    public CTAchievement[] achievements;
    private bool[] achievementsDone;
    public Dictionary<Stat, double> stats = new Dictionary<Stat,double>();
    public int numOfAchievements;

	// chapter unlock variables -- TRUE means unlocked
    public bool[] chapterUnlocked;
    public bool[] challengeChapterUnlocked;
    public bool[] chapterReleased;

	// stars for each stage
    public int[][] stars;
    public int[][] challengeStars;

	// stage unlock variables -- TRUE means unlocked
    public bool[][] levelsUnlocked;
    public bool[][] challengeLevelsUnlocked;

    // number of stages completed in a chapter
    public bool[][] stagesCompletedPerChapter;
    public bool[][] stagesCompletedWOItems;
    public bool[][] stagesCompletedWOEq;
    public bool[][] stagesAttemptedWDiver;

    // Login variables
    public System.DateTime lastLogin;
    public int consecutiveLogins;

    // Bag variables
    public string[] bag;
	public int bagSlots;

    public bool audio;

    // Energy Variables
    public int energy;
    public int energyCap;
    public System.DateTime timeSinceFirstEnergy;
	public bool energyFull;
    public bool isUnlimitedEnergy;
    public System.TimeSpan unlimitedEnergySpan;
    public System.DateTime unlimitedEnergyStart;

    //public System.TimeSpan playingTime;

    void Awake()
    {
        // Forces a different code path in the BinaryFormatter that doesn't rely on run-time code generation (which would break on iOS).
        System.Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");

        Debug.Log(Application.persistentDataPath);

        // Make sure there is only 1 instance of this class.
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            instance.Load();

            // debugging for google services
            PlayGamesPlatform.DebugLogEnabled = true;

            // Activate the Google Play Games platform
            PlayGamesPlatform.Activate();

            // authenticate user
            Social.localUser.Authenticate((bool success) =>
            {
                // handle success or failure
            });
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initialise Adbuddiz
        AdBuddizBinding.SetAndroidPublisherKey(Constants.adBuddizPublisherKeyAndroid);

        // Cache Ads
        AdBuddizBinding.CacheAds();

        AdBuddizManager.didShowAd += showChartboostInterstitial;
    }

	// Function to initialize object; called when there is no previous saved file
    public void Initialize()
    {
        //achievements = new CTAchievement[numOfAchievements];
        achievementsDone = new bool[numOfAchievements];
        energyCap = 10;
        energy = energyCap;
        setLastLogin();
        audio = true;

        stats[Stat.totalSteps] = 0;
        stats[Stat.totalClimbs] = 0;
        stats[Stat.playingTime] = 0;
        stats[Stat.toiletPapers] = 0;
        stats[Stat.goldenPapers] = 0;
        stats[Stat.totalPulls] = 0;
        stats[Stat.totalPushes] = 0;
        stats[Stat.totalPullOuts] = 0;
        stats[Stat.totalHangingSteps] = 0;
        stats[Stat.totalSlides] = 0;
        stats[Stat.treasures] = 0;
        stats[Stat.stagesCompleted] = 0;
        stats[Stat.itemsUsed] = 0;
        stats[Stat.itemsBought] = 0;
        stats[Stat.stagesUnlocked] = 0;
        stats[Stat.objectivesEarned] = 0;
        stats[Stat.boughtGearID] = 0;
        stats[Stat.skillsUsed] = 0;
        stats[Stat.achievementUnlocked] = 0;
        stats[Stat.consecutiveLogins] = 0;
        stats[Stat.capsules] = 0;
        stats[Stat.fails] = 0;
        stats[Stat.eqsBought] = 0;
        stats[Stat.setCollected] = 0;
        stats[Stat.achievementUnlocked] = 0;
        stats[Stat.clothingCollected] = 0;
        stats[Stat.diapers] = 0;
        stats[Stat.completeStageWOEq] = 0;
        stats[Stat.completeStageWOItems] = 0;
        stats[Stat.stareAtItemShop] = 0;
        stats[Stat.attemptWDiver] = 0;
        stats[Stat.itemsBoughtUsingGTP] = 0;
        stats[Stat.gtpBought] = 0;
        bagSlots = 3;
        bag = new string[bagSlots];

        isUnlimitedEnergy = false;
        energyFull = true;

        for (int z = 0; z < numOfAchievements; z++)
        {
            achievementsDone[z] = false;
        }

        // Construct arrays
        stars = new int[7][];
        levelsUnlocked = new bool[7][];
        chapterUnlocked = new bool[7];
        challengeStars = new int[7][];
        challengeLevelsUnlocked = new bool[7][];
        challengeChapterUnlocked = new bool[7];
        stagesCompletedPerChapter = new bool[7][];
        stagesAttemptedWDiver = new bool[7][];
        stagesCompletedWOEq = new bool[7][];
        stagesCompletedWOItems = new bool[7][];
        for (int i = 0; i < 7; i++) // Construc sub-arrays
        {
            stars[i] = new int[20];
            stagesCompletedPerChapter[i] = new bool[20];
            stagesAttemptedWDiver[i] = new bool[20];
            stagesCompletedWOEq[i] = new bool[20];
            stagesCompletedWOItems[i] = new bool[20];
            levelsUnlocked[i] = new bool[10];
            chapterUnlocked[i] = false;
            challengeStars[i] = new int[10];
            challengeLevelsUnlocked[i] = new bool[10];
            challengeChapterUnlocked[i] = false;
            for (int j = 0; j < 10; j++) //  Initialize sub-arrays
            {
                stars[i][j] = 0;
                stagesCompletedPerChapter[i][j] = false;
                stagesAttemptedWDiver[i][j] = false;
                stagesCompletedWOEq[i][j] = false;
                stagesCompletedWOItems[i][j] = false;
                stagesCompletedPerChapter[i][j + 10] = false;
                stagesAttemptedWDiver[i][j + 10] = false;
                stagesCompletedWOEq[i][j + 10] = false;
                stagesCompletedWOItems[i][j + 10] = false;
                levelsUnlocked[i][j] = false;
                challengeStars[i][j] = 0;
                challengeLevelsUnlocked[i][j] = false;
                if (j == 0)
                {
                    levelsUnlocked[i][j] = true;
                    challengeLevelsUnlocked[i][j] = true;
                }
            }
        }
        chapterUnlocked[0] = true;
    }

	// function to save all the data
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameInfo.dat");

        GameInfo info = new GameInfo();
        info.achievementsDone = achievementsDone;
        info.achievementUnlocked = stats[Stat.achievementUnlocked];
        info.audio = audio;
        info.bag = bag;
        info.bagSlots = bagSlots;
        info.boughtGearID = stats[Stat.boughtGearID];
        info.challengeChapterUnlocked = challengeChapterUnlocked;
        info.challengeLevelsUnlocked = challengeLevelsUnlocked;
        info.challengeStars = challengeStars;
        info.chapterUnlocked = chapterUnlocked;
        info.consecutiveLogins = stats[Stat.consecutiveLogins];
        info.energy = energy;
        info.energyCap = energyCap;
        info.energyFull = energyFull;
        info.goldenPapers = stats[Stat.goldenPapers];
        info.isUnlimitedEnergy = isUnlimitedEnergy;
        info.itemsBought = stats[Stat.itemsBought];
        info.itemsUsed = stats[Stat.itemsUsed];
        info.lastLogin = lastLogin.ToString();
        info.levelsUnlocked = levelsUnlocked;
        info.objectivesEarned = stats[Stat.objectivesEarned];
        info.playingTime = stats[Stat.playingTime];
        info.skillsUsed = stats[Stat.skillsUsed];
        info.stagesCompleted = stats[Stat.stagesCompleted];
        info.stagesUnlocked = stats[Stat.stagesUnlocked];
        info.stars = stars;
        info.timeSinceFirstEnergy = timeSinceFirstEnergy.ToString();
        info.toiletPapers = stats[Stat.toiletPapers];
        info.totalClimbs = stats[Stat.totalClimbs];
        info.totalHangingSteps = stats[Stat.totalHangingSteps];
        info.totalPullOuts = stats[Stat.totalPullOuts];
        info.totalPulls = stats[Stat.totalPulls];
        info.totalPushes = stats[Stat.totalPushes];
        info.totalSlides = stats[Stat.totalSlides];
        info.totalSteps = stats[Stat.totalSteps];
        info.treasures = stats[Stat.treasures];
        info.stagesCompletedPerChapter = stagesCompletedPerChapter;
        info.stagesCompletedWOEq = stagesCompletedWOEq;
        info.stagesCompletedWOItems = stagesCompletedWOItems;
        info.stagesAttemptedWDiver = stagesAttemptedWDiver;
        info.capsules = stats[Stat.capsules];
        info.fails = stats[Stat.fails];
        info.eqsBought = stats[Stat.eqsBought];
        info.setCollected = stats[Stat.setCollected];
        info.clothingCollected = stats[Stat.clothingCollected];
        info.diapers = stats[Stat.diapers];
        info.stareAtItemShop = stats[Stat.stareAtItemShop];
        info.itemsBoughtUsingGTP = stats[Stat.itemsBoughtUsingGTP];
        info.gtpBought = stats[Stat.gtpBought];

        bf.Serialize(file, info);
        file.Close();
    }

	// function to load the data; if there is no previous saved file then create a new object
    public void Load() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        GameInfo info;

        if(File.Exists((Application.persistentDataPath + "/gameInfo.dat"))) 
        {
            file = File.Open(Application.persistentDataPath + "/gameInfo.dat", FileMode.Open);
            info = (GameInfo)bf.Deserialize(file);
            file.Close();

            achievementsDone = info.achievementsDone;
            stats[Stat.achievementUnlocked] = info.achievementUnlocked;
            audio = info.audio;
            bag = info.bag;
            bagSlots = info.bagSlots;
            stats[Stat.boughtGearID] = info.boughtGearID;
            challengeChapterUnlocked = info.challengeChapterUnlocked;
            challengeLevelsUnlocked = info.challengeLevelsUnlocked;
            challengeStars = info.challengeStars;
            chapterUnlocked = info.chapterUnlocked;
            stats[Stat.consecutiveLogins] = info.consecutiveLogins;
            energy = info.energy;
            energyCap = info.energyCap;
            energyFull = info.energyFull;
            stats[Stat.goldenPapers] = info.goldenPapers;
            isUnlimitedEnergy = info.isUnlimitedEnergy;
            stats[Stat.itemsBought] = info.itemsBought;
            stats[Stat.itemsUsed] = info.itemsUsed;
            lastLogin = System.DateTime.Parse(info.lastLogin);
            levelsUnlocked = info.levelsUnlocked;
            stats[Stat.objectivesEarned] = info.objectivesEarned;
            stats[Stat.playingTime] = info.playingTime;
            stats[Stat.skillsUsed] = info.skillsUsed;
            stats[Stat.stagesCompleted] = info.stagesCompleted;
            stats[Stat.stagesUnlocked] = info.stagesUnlocked;
            stars = info.stars;
            timeSinceFirstEnergy = System.DateTime.Parse(info.timeSinceFirstEnergy);
            stats[Stat.toiletPapers] = info.toiletPapers;
            stats[Stat.totalClimbs] = info.totalClimbs;
            stats[Stat.totalHangingSteps] = info.totalHangingSteps;
            stats[Stat.totalPullOuts] = info.totalPullOuts;
            stats[Stat.totalPulls] = info.totalPulls;
            stats[Stat.totalPushes] = info.totalPushes;
            stats[Stat.totalSlides] = info.totalSlides;
            stats[Stat.totalSteps] = info.totalSteps;
            stats[Stat.treasures] = info.treasures;
            stagesCompletedPerChapter = info.stagesCompletedPerChapter;
            stagesAttemptedWDiver = info.stagesAttemptedWDiver;
            stagesCompletedWOEq = info.stagesCompletedWOEq;
            stagesCompletedWOItems = info.stagesCompletedWOItems;
            stats[Stat.capsules] = info.capsules;
            stats[Stat.fails] = info.fails;
            stats[Stat.eqsBought] = info.eqsBought;
            stats[Stat.setCollected] = info.setCollected;
            stats[Stat.clothingCollected] = info.clothingCollected;
            stats[Stat.diapers] = info.diapers;
            stats[Stat.stareAtItemShop] = info.stareAtItemShop;
            stats[Stat.itemsBoughtUsingGTP] = info.itemsBoughtUsingGTP;
            stats[Stat.gtpBought] = info.gtpBought;

            setLastLogin();

            energy = checkAndGetEnergy();
            checkUnlimitedEnergy();
        }
        else
        {
            Initialize();
        }
    }

	// function called when doing a stage to decrease energy
	public void useEnergy() {
		if (energy > 0) {
			energy -= 1;
		}
		if (energyFull) {
			setTimeSinceFirstEnergy();
		}
		energyFull = false;
	}

	// function to increase energy cap
    public void increaseCap(int energyPlus)
    {
		energyCap += energyPlus;
    }

	// function called after first energy is used
    public void setTimeSinceFirstEnergy()
    {
        timeSinceFirstEnergy = System.DateTime.Now;
    }

	// function that checks if energy should be replenished, and returns the energy
    public int checkAndGetEnergy()
    {
        System.TimeSpan diff = System.DateTime.Now - timeSinceFirstEnergy;
        if (diff.Days > 0)
        {
            energy = energyCap;
        }
        else
        {
            energy += diff.Hours;
            if (energy > energyCap)
            {
                energy = energyCap;
            }
        }
        return energy;
    }

    // Function to check whether unlimited energy is active
    public void checkUnlimitedEnergy()
    {
        if(System.DateTime.Now.CompareTo(unlimitedEnergyStart.Add(unlimitedEnergySpan)) > 0) 
        {
            isUnlimitedEnergy = false;
        }
    }

	// function to set the last time the user uses the application
    public void setLastLogin()
    {
        lastLogin = System.DateTime.Now;
    }

	// function to update the stars for a stage
    public void UpdateStats(int chapter, int level, int star)
    {
        // Update completed stages
        if (chapter < 7) // If normal stage
        {
            stagesCompletedPerChapter[chapter][level] = true;
        }
        else // If challenge stage
        {
            stagesCompletedPerChapter[chapter-7][level + 10] = true;
        }

        // Update Stars if the current number of stars is more than previous number of stars
        if (chapter < 7)
        {
            if (stars[chapter][level] < star)
            {
                stars[chapter][level] = star;
            }
        }
        else 
        {
            if (challengeStars[chapter-7][level] < star)
            {
                challengeStars[chapter-7][level] = star;
            }
        }

        // If stage is not the last normal stage in the chapter then open the next stage
        if (level < 9)
        {
            if (chapter > 6)
            {
                if (star == 3)
                {
                    challengeLevelsUnlocked[chapter-7][level + 1] = true;
                }
            }
            else
            {
                levelsUnlocked[chapter][level + 1] = true;
            }
        }
        else // If it is the last normal stage
        {
            // Open next chapter if not currently in the last chapter
            if (chapter < 6) 
            {
                if (chapterReleased[chapter + 1])
                {
                    chapterUnlocked[chapter + 1] = true;
                }
            }
            
            // Open the challenge stages
            if(chapter < 7)
                challengeChapterUnlocked[chapter] = true;
        }
    }

    // Function to get how many stages have been completed in the chapter specified
    public int getStagesCompleted(int chapter)
    {
        int count = 0;
        for (int i = 0; i < 20; i++)
        {
            if (stagesCompletedPerChapter[chapter][i] == true)
                count += 1;
        }
        return count;
    }

    // Function to check for achievements and their rewards
    public void UpdateReward()
    {
        for (int i = 0; i < 10; i++)
        {
            if (!achievements[i].hidden)
            {
                if (!achievementsDone[i])
                {
                    if (achievements[i].stat == Stat.getGear)
                    {
                        if (achievements[i].gear == Gear.diver)
                        {
                            if (Soomla.Store.StoreInventory.GetItemBalance("eq_head_set_diver") > 0 &&
                                Soomla.Store.StoreInventory.GetItemBalance("eq_body_set_diver") > 0 &&
                                Soomla.Store.StoreInventory.GetItemBalance("eq_legs_set_diver") > 0)
                            {
                                //achievements[i].isDone = true;
                                achievementsDone[i] = true;
                                stats[Stat.setCollected] += 1;

                                // Pass on to GlobalGUI
                                GlobalGUI.instance.AddAchievement(achievements[i]);

                                // Give Reward if any
                                if (achievements[i].reward.Length > 0)
                                {
                                    foreach (Reward reward in achievements[i].reward)
                                    {
                                        Soomla.Store.StoreInventory.GiveItem(reward.rewardID, reward.rewardQuantity);
                                    }
                                }
                                stats[Stat.setCollected] += 1;

                                /* Do update for IOS Game Center/Google Play here */
                            }
                        }
                        else if (achievements[i].gear == Gear.explorer)
                        {
                            if (Soomla.Store.StoreInventory.GetItemBalance("eq_head_set_explorer") > 0 &&
                                Soomla.Store.StoreInventory.GetItemBalance("eq_body_set_explorer") > 0 &&
                                Soomla.Store.StoreInventory.GetItemBalance("eq_legs_set_explorer") > 0)
                            {
                                //achievements[i].isDone = true;
                                achievementsDone[i] = true; //
                                stats[Stat.setCollected] += 1;

                                // Pass on to GlobalGUI
                                GlobalGUI.instance.AddAchievement(achievements[i]);

                                // Give Reward if any
                                if (achievements[i].reward.Length > 0)
                                {
                                    foreach (Reward reward in achievements[i].reward)
                                    {
                                        Soomla.Store.StoreInventory.GiveItem(reward.rewardID, reward.rewardQuantity);
                                    }
                                }
                                stats[Stat.setCollected] += 1;

                                /* Do update for IOS Game Center/Google Play here */
                            }
                        }
                        else if (achievements[i].gear == Gear.pirate)
                        {
                            if (Soomla.Store.StoreInventory.GetItemBalance("eq_head_set_pirate") > 0 &&
                                Soomla.Store.StoreInventory.GetItemBalance("eq_body_set_pirate") > 0 &&
                                Soomla.Store.StoreInventory.GetItemBalance("eq_legs_set_pirate") > 0)
                            {
                                achievementsDone[i] = true; //achievements[i].isDone = true;
                                stats[Stat.setCollected] += 1;

                                // Pass on to GlobalGUI
                                GlobalGUI.instance.AddAchievement(achievements[i]);

                                // Give Reward if any
                                if (achievements[i].reward.Length > 0)
                                {
                                    foreach (Reward reward in achievements[i].reward)
                                    {
                                        Soomla.Store.StoreInventory.GiveItem(reward.rewardID, reward.rewardQuantity);
                                    }
                                }
                                stats[Stat.setCollected] += 1;

                                /* Do update for IOS Game Center/Google Play here */
                            }
                        }
                        else if (achievements[i].gear == Gear.tribal)
                        {
                            if (Soomla.Store.StoreInventory.GetItemBalance("eq_head_set_tribal") > 0 &&
                                Soomla.Store.StoreInventory.GetItemBalance("eq_body_set_tribal") > 0 &&
                                Soomla.Store.StoreInventory.GetItemBalance("eq_legs_set_tribal") > 0)
                            {
                                achievementsDone[i] = true; //achievements[i].isDone = true;
                                stats[Stat.setCollected] += 1;

                                // Pass on to GlobalGUI
                                GlobalGUI.instance.AddAchievement(achievements[i]);

                                // Give Reward if any
                                if (achievements[i].reward.Length > 0)
                                {
                                    foreach (Reward reward in achievements[i].reward)
                                    {
                                        Soomla.Store.StoreInventory.GiveItem(reward.rewardID, reward.rewardQuantity);
                                    }
                                }
                                stats[Stat.setCollected] += 1;

                                /* Do update for IOS Game Center/Google Play here */
                            }
                        }
                        else if (achievements[i].gear == Gear.mummy)
                        {
                            if (Soomla.Store.StoreInventory.GetItemBalance("eq_head_set_mummy") > 0 &&
                                Soomla.Store.StoreInventory.GetItemBalance("eq_body_set_mummy") > 0 &&
                                Soomla.Store.StoreInventory.GetItemBalance("eq_legs_set_mummy") > 0)
                            {
                                achievementsDone[i] = true; //achievements[i].isDone = true;
                                stats[Stat.setCollected] += 1;

                                // Pass on to GlobalGUI
                                GlobalGUI.instance.AddAchievement(achievements[i]);

                                // Give Reward if any
                                if (achievements[i].reward.Length > 0)
                                {
                                    foreach (Reward reward in achievements[i].reward)
                                    {
                                        Soomla.Store.StoreInventory.GiveItem(reward.rewardID, reward.rewardQuantity);
                                    }
                                }
                                stats[Stat.setCollected] += 1;

                                /* Do update for IOS Game Center/Google Play here */
                            }
                        }
                    }
                    else if (achievements[i].stat == Stat.completeStageWOEq)
                    {
                        int count = 0;
                        for (int k = 0; k < 7; k++)
                        {
                            for (int j = 0; j < 20; j++)
                            {
                                if (stagesCompletedWOEq[k][j] == true)
                                    count += 1;
                            }
                        }
                        if (count >= achievements[i].counter)
                        {
                            achievementsDone[i] = true; //achievements[i].isDone = true;

                            // Pass on to GlobalGUI
                            GlobalGUI.instance.AddAchievement(achievements[i]);

                            // Give Reward if any
                            if (achievements[i].reward.Length > 0)
                            {
                                foreach (Reward reward in achievements[i].reward)
                                {
                                    Soomla.Store.StoreInventory.GiveItem(reward.rewardID, reward.rewardQuantity);
                                }
                            }
                        }
                    }
                    else if (achievements[i].stat == Stat.completeStageWOItems)
                    {
                        int count = 0;
                        for (int k = 0; k < 7; k++)
                        {
                            for (int j = 0; j < 20; j++)
                            {
                                if (stagesCompletedWOItems[k][j] == true)
                                    count += 1;
                            }
                        }
                        if (count >= achievements[i].counter)
                        {
                            achievementsDone[i] = true; //achievements[i].isDone = true;

                            // Pass on to GlobalGUI
                            GlobalGUI.instance.AddAchievement(achievements[i]);

                            // Give Reward if any
                            if (achievements[i].reward.Length > 0)
                            {
                                foreach (Reward reward in achievements[i].reward)
                                {
                                    Soomla.Store.StoreInventory.GiveItem(reward.rewardID, reward.rewardQuantity);
                                }
                            }
                        }
                    }
                    else if (achievements[i].stat == Stat.attemptWDiver)
                    {
                        int count = 0;
                        for (int k = 0; k < 7; k++)
                        {
                            for (int j = 0; j < 20; j++)
                            {
                                if (stagesAttemptedWDiver[k][j] == true)
                                    count += 1;
                            }
                        }
                        if (count >= achievements[i].counter)
                        {
                            achievementsDone[i] = true; //achievements[i].isDone = true;

                            // Pass on to GlobalGUI
                            GlobalGUI.instance.AddAchievement(achievements[i]);

                            // Give Reward if any
                            if (achievements[i].reward.Length > 0)
                            {
                                foreach (Reward reward in achievements[i].reward)
                                {
                                    Soomla.Store.StoreInventory.GiveItem(reward.rewardID, reward.rewardQuantity);
                                }
                            }
                        }
                    }
                    else if (achievements[i].stat == Stat.completeStageW3Stars)
                    {
                        int count = 0;
                        for (int k = 0; k < 7; k++)
                        {
                            for (int j = 0; j < 20; j++)
                            {
                                if (stars[k][j] == 3)
                                    count += 1;
                            }
                        }
                        if (count >= achievements[i].counter)
                        {
                            achievementsDone[i] = true; //achievements[i].isDone = true;

                            // Pass on to GlobalGUI
                            GlobalGUI.instance.AddAchievement(achievements[i]);

                            // Give Reward if any
                            if (achievements[i].reward.Length > 0)
                            {
                                foreach (Reward reward in achievements[i].reward)
                                {
                                    Soomla.Store.StoreInventory.GiveItem(reward.rewardID, reward.rewardQuantity);
                                }
                            }
                        }
                    }
                    else if (achievements[i].stat == Stat.completeChallengeStages)
                    {
                        int count = 0;
                        for (int k = 0; k < 7; k++)
                        {
                            for (int j = 10; j < 20; j++)
                            {
                                if (stagesCompletedPerChapter[k][j])
                                    count += 1;
                            }
                        }
                        if (count >= achievements[i].counter)
                        {
                            achievementsDone[i] = true; //achievements[i].isDone = true;

                            // Pass on to GlobalGUI
                            GlobalGUI.instance.AddAchievement(achievements[i]);

                            // Give Reward if any
                            if (achievements[i].reward.Length > 0)
                            {
                                foreach (Reward reward in achievements[i].reward)
                                {
                                    Soomla.Store.StoreInventory.GiveItem(reward.rewardID, reward.rewardQuantity);
                                }
                            }
                        }
                    }
                    else if (stats[achievements[i].stat] >= achievements[i].counter)
                    {
                        achievementsDone[i] = true; //achievements[i].isDone = true;

                        // Pass on to GlobalGUI
                        GlobalGUI.instance.AddAchievement(achievements[i]);

                        // Give Reward if any
                        if (achievements[i].reward.Length > 0)
                        {
                            foreach (Reward reward in achievements[i].reward)
                            {
                                Soomla.Store.StoreInventory.GiveItem(reward.rewardID, reward.rewardQuantity);
                            }
                        }

                        /* Do update for IOS Game Center/Google Play here */
                    }
                }
            }
        }
    }

    /*
    public void checkLevel(string key)
    {
        if (!stars.ContainsKey(key))
        {
            stars.Add(key, 0);
            lockedLevels.Add(key, false);
        }
    } */

    public void showChartboostInterstitial()
    {
        Chartboost.showInterstitial(CBLocation.Default);
    }
}

// Class to save the data into a Binary Formatter
[System.Serializable]
class GameInfo
{
    public double totalSteps = 0;
    public double totalClimbs = 0;
    public double playingTime = 0;
    public double toiletPapers = 0;
    public double goldenPapers = 0;
    public double totalPulls = 0;
    public double totalPushes = 0;
    public double totalPullOuts = 0;
    public double totalHangingSteps = 0;
    public double totalSlides = 0;
    public double treasures = 0;
    public double stagesCompleted = 0;
    public double itemsUsed = 0;
    public double itemsBought = 0;
    public double stagesUnlocked = 0;
    public double objectivesEarned = 0;
    public double boughtGearID = 0;
    public double skillsUsed = 0;
    public double achievementUnlocked = 0;
    public double consecutiveLogins = 0;
    public double capsules = 0;
    public double fails = 0;
    public double eqsBought = 0;
    public double setCollected = 0;
    public double clothingCollected = 0;
    public double diapers = 0;
    public double stareAtItemShop = 0;
    public double itemsBoughtUsingGTP = 0;
    public double gtpBought = 0;

    public CTAchievement[] achievements;
    public bool[] achievementsDone;

    // chapter unlock variables -- TRUE means unlocked
    public bool[] chapterUnlocked;
    public bool[] challengeChapterUnlocked;

    // stars for each stage
    public int[][] stars;
    public int[][] challengeStars;

    // stage unlock variables -- TRUE means unlocked
    public bool[][] levelsUnlocked;
    public bool[][] challengeLevelsUnlocked;

    // number of stages completed in a chapter
    public bool[][] stagesCompletedPerChapter;
    public bool[][] stagesCompletedWOItems;
    public bool[][] stagesCompletedWOEq;
    public bool[][] stagesAttemptedWDiver;

    public string lastLogin;
    public string[] bag;
    public int bagSlots;
    public bool audio;

    public int energy;
    public int energyCap;
    public string timeSinceFirstEnergy;
    public bool energyFull;

    public bool isUnlimitedEnergy;
    public string unlimitedEnergySpan;
    public string unlimitedEnergyStart;
}
