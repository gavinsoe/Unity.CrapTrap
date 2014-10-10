using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Game : MonoBehaviour {

    public static Game game;

    public Achievement[] achievements;
    public Dictionary<Type2, double> stats;

	// chapter unlock variables -- TRUE means unlocked
    public bool[] chapterUnlocked;
    public bool[] challengeChapterUnlocked;

	// stars for each stage
    public int[][] stars;
    public int[][] challengeStars;

	// stage unlock variables -- TRUE means unlocked
    public bool[][] levelsUnlocked;
    public bool[][] challengeLevelsUnlocked;

    public System.DateTime lastLogin;
    public int consecutiveLogins;
    public string[] bag;
	public int bagSlots;
    public bool audio;

    public int energy;
    public int energyCap;
    public System.DateTime timeSinceFirstEnergy;
	public bool energyFull;

    public bool isUnlimitedEnergy;
    public System.TimeSpan unlimitedEnergySpan;
    public System.DateTime unlimitedEnergyStart;

    public System.TimeSpan playingTime;

	// Use this for initialization
	void Start () {
	    
	}

    void Awake()
    {
        if (game == null)
        {
            DontDestroyOnLoad(gameObject);
            game = this;
        }
        else if (game != this)
        {
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

	// Constructor
    public Game()
    {
        achievements = new Achievement[20];
        Initialize();
        energyCap = 10;
        energy = energyCap;
        setLastLogin();
        audio = true;

        stats[Type2.totalSteps] = 0;
        stats[Type2.totalClimbs] = 0;
        stats[Type2.playingTime] = 0;
        stats[Type2.toiletPapers] = 0;
        stats[Type2.goldenPapers] = 0;
        stats[Type2.totalPulls] = 0;
        stats[Type2.totalPushes] = 0;
        stats[Type2.totalPullOuts] = 0;
        stats[Type2.totalHangingSteps] = 0;
        stats[Type2.totalSlides] = 0;
        stats[Type2.treasures] = 0;
        stats[Type2.stagesCompleted] = 0;
        stats[Type2.itemsUsed] = 0;
        stats[Type2.itemsBought] = 0;
        stats[Type2.stagesUnlocked] = 0;
        stats[Type2.objectivesEarned] = 0;
        stats[Type2.boughtGearID] = 0;
        stats[Type2.skillsUsed] = 0;
        stats[Type2.achievementUnlocked] = 0;
        stats[Type2.consecutiveLogins] = 0;
		bagSlots = 2;

        isUnlimitedEnergy = false;
		energyFull = true;
    }

	// Function to initialize object; called when there is no previous saved file
    public void Initialize()
    {
        stars = new int[7][];
        levelsUnlocked = new bool[7][];
        chapterUnlocked = new bool[7];
        challengeStars = new int[7][];
        challengeLevelsUnlocked = new bool[7][];
        challengeChapterUnlocked = new bool[7];
        for (int i = 0; i < 7; i++)
        {
            stars[i] = new int[10];
            levelsUnlocked[i] = new bool[10];
            chapterUnlocked[i] = false;
            challengeStars[i] = new int[10];
            challengeLevelsUnlocked[i] = new bool[10];
            challengeChapterUnlocked[i] = false;
            for (int j = 0; j < 10; j++)
            {
                stars[i][j] = 0;
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
        info.achievementUnlocked = stats[Type2.achievementUnlocked];
        info.audio = audio;
        info.bag = bag;
        info.bagSlots = bagSlots;
        info.boughtGearID = stats[Type2.boughtGearID];
        info.challengeChapterUnlocked = challengeChapterUnlocked;
        info.challengeLevelsUnlocked = challengeLevelsUnlocked;
        info.challengeStars = challengeStars;
        info.chapterUnlocked = chapterUnlocked;
        info.consecutiveLogins = stats[Type2.consecutiveLogins];
        info.energy = energy;
        info.energyCap = energyCap;
        info.energyFull = energyFull;
        info.goldenPapers = stats[Type2.goldenPapers];
        info.isUnlimitedEnergy = isUnlimitedEnergy;
        info.itemsBought = stats[Type2.itemsBought];
        info.itemsUsed = stats[Type2.itemsUsed];
        info.lastLogin = lastLogin.ToString();
        info.levelsUnlocked = levelsUnlocked;
        info.objectivesEarned = stats[Type2.objectivesEarned];
        info.playingTime = stats[Type2.playingTime];
        info.skillsUsed = stats[Type2.skillsUsed];
        info.stagesCompleted = stats[Type2.stagesCompleted];
        info.stagesUnlocked = stats[Type2.stagesUnlocked];
        info.stars = stars;
        info.timeSinceFirstEnergy = timeSinceFirstEnergy.ToString();
        info.toiletPapers = stats[Type2.toiletPapers];
        info.totalClimbs = stats[Type2.totalClimbs];
        info.totalHangingSteps = stats[Type2.totalHangingSteps];
        info.totalPullOuts = stats[Type2.totalPullOuts];
        info.totalPulls = stats[Type2.totalPulls];
        info.totalPushes = stats[Type2.totalPushes];
        info.totalSlides = stats[Type2.totalSlides];
        info.totalSteps = stats[Type2.totalSteps];
        info.treasures = stats[Type2.treasures];

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
            stats[Type2.achievementUnlocked] = info.achievementUnlocked;
            audio = info.audio;
            bag = info.bag;
            bagSlots = info.bagSlots;
            stats[Type2.boughtGearID] = info.boughtGearID;
            challengeChapterUnlocked = info.challengeChapterUnlocked;
            challengeLevelsUnlocked = info.challengeLevelsUnlocked;
            challengeStars = info.challengeStars;
            chapterUnlocked = info.chapterUnlocked;
            stats[Type2.consecutiveLogins] = info.consecutiveLogins;
            energy = info.energy;
            energyCap = info.energyCap;
            energyFull = info.energyFull;
            stats[Type2.goldenPapers] = info.goldenPapers;
            isUnlimitedEnergy = info.isUnlimitedEnergy;
            stats[Type2.itemsBought] = info.itemsBought;
            stats[Type2.itemsUsed] = info.itemsUsed;
            lastLogin = System.DateTime.Parse(info.lastLogin);
            levelsUnlocked = info.levelsUnlocked;
            stats[Type2.objectivesEarned] = info.objectivesEarned;
            stats[Type2.playingTime] = info.playingTime;
            stats[Type2.skillsUsed] = info.skillsUsed;
            stats[Type2.stagesCompleted] = info.stagesCompleted;
            stats[Type2.stagesUnlocked] = info.stagesUnlocked;
            stars = info.stars;
            timeSinceFirstEnergy = System.DateTime.Parse(info.timeSinceFirstEnergy);
            stats[Type2.toiletPapers] = info.toiletPapers;
            stats[Type2.totalClimbs] = info.totalClimbs;
            stats[Type2.totalHangingSteps] = info.totalHangingSteps;
            stats[Type2.totalPullOuts] = info.totalPullOuts;
            stats[Type2.totalPulls] = info.totalPulls;
            stats[Type2.totalPushes] = info.totalPushes;
            stats[Type2.totalSlides] = info.totalSlides;
            stats[Type2.totalSteps] = info.totalSteps;
            stats[Type2.treasures] = info.treasures;

            energy = checkAndGetEnergy();
            checkUnlimitedEnergy();
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
        stars[chapter][level] = star;
        if (level < 9)
        {
            levelsUnlocked[chapter][level + 1] = true;
        }
        else
        {
            if (chapter < 6)
            {
                chapterUnlocked[chapter + 1] = true;
            }
            challengeChapterUnlocked[chapter] = true;
        }
    }

    public void UpdateReward()
    {
        for (int i = 0; i < 10; i++)
        {
            if (stats[achievements[i].type] >= achievements[i].counter)
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

    public Achievement[] achievements;

    // chapter unlock variables -- TRUE means unlocked
    public bool[] chapterUnlocked;
    public bool[] challengeChapterUnlocked;

    // stars for each stage
    public int[][] stars;
    public int[][] challengeStars;

    // stage unlock variables -- TRUE means unlocked
    public bool[][] levelsUnlocked;
    public bool[][] challengeLevelsUnlocked;

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
