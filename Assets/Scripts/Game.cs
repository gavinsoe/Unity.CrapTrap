using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("Game")]
public class Game : MonoBehaviour {

    [XmlArray("Achievements")]
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
        string path = "game.data";
        var serializer = new XmlSerializer(typeof(Game));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

	// function to load the data; if there is no previous saved file then create a new object
    public static Game Load() {
        string path = "game.data";
        var serializer = new XmlSerializer(typeof(Game));
        if (File.Exists(path))
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return serializer.Deserialize(stream) as Game;
            }
        }
        else
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                Game newG = new Game();
				newG.Initialize();
                return newG;
            }
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
