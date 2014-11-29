using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using UnityEngine;

public class Game : MonoBehaviour 
{
    public static Game instance;

    public CTAchievement[] achievements;
    public Dictionary<Stat, double> stats = new Dictionary<Stat,double>();
    public int numOfAchievements;

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

    public System.TimeSpan playingTime;

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

	// Function to initialize object; called when there is no previous saved file
    public void Initialize()
    {
        achievements = new CTAchievement[numOfAchievements];
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
        bagSlots = 3;
        bag = new string[bagSlots];

        isUnlimitedEnergy = false;
        energyFull = true;

        // Construct arrays
        stars = new int[7][];
        levelsUnlocked = new bool[7][];
        chapterUnlocked = new bool[7];
        challengeStars = new int[7][];
        challengeLevelsUnlocked = new bool[7][];
        challengeChapterUnlocked = new bool[7];
        stagesCompletedPerChapter = new bool[7][];
        for (int i = 0; i < 7; i++) // Construc sub-arrays
        {
            stars[i] = new int[20];
            stagesCompletedPerChapter[i] = new bool[20];
            levelsUnlocked[i] = new bool[10];
            chapterUnlocked[i] = false;
            challengeStars[i] = new int[10];
            challengeLevelsUnlocked[i] = new bool[10];
            challengeChapterUnlocked[i] = false;
            for (int j = 0; j < 10; j++) //  Initialize sub-arrays
            {
                stars[i][j] = 0;
                stagesCompletedPerChapter[i][j] = false;
                stagesCompletedPerChapter[i][j + 10] = false;
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
        info.achievements = achievements;
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
        info.capsules = stats[Stat.capsules];

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

            achievements = info.achievements;
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
            stats[Stat.capsules] = info.capsules;

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
                chapterUnlocked[chapter + 1] = true;
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
            if (stats[achievements[i].stat] >= achievements[i].counter)
            {
                if (achievements[i].isDone == false)
                {
                    achievements[i].isDone = true;

                    /* Do update for IOS Game Center/Google Play here */
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

    public CTAchievement[] achievements;

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
