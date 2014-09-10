using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("Game")]
public class Game : MonoBehaviour {

    [XmlArray("Achievements")]
    public bool[][] achievements;

    public bool[] chaptersUnlocked;
    public bool[] challengeChaptersUnlocked;

    public int[][] stars;
    public int[][] challengeStars;
    public bool[][] levelsUnlocked;
    public bool[][] challengeLevelsUnlocked;

    public System.DateTime lastLogin;
    public int consecutiveLogins;
    public string[] bag;
    public bool audio;

    public int energy;
    public int energyCap;
    public System.DateTime timeSinceEnergy;

    public bool isUnlimitedEnergy;
    public System.TimeSpan unlimitedEnergySpan;
    public System.DateTime unlimitedEnergyStart;

    public int totalSteps;
    public int totalClimbs;
    public System.TimeSpan playingTime;
    public int totalToiletPapers;
    public int totalGoldenPapers;
    public int totalPulls;
    public int totalPushes;
    public int totalPullOuts;
    public int totalHangingSteps;
    public int totalSlides;
    public int treasures;
    public int stagesCompleted;
    public int itemsUsed;
    public int itemsBought;
    public int stagesUnlocked;
    public int objectivesEarned;
    public int boughtGearID;
    public int skillsUsed;
    public int achievementUnlocked;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Game()
    {
        achievements = new bool[1][];
        Initialize();
        energyCap = 10;
        energy = energyCap;
        setLastLogin();
        audio = true;
        consecutiveLogins = 0;

        totalSteps = 0;
        totalClimbs = 0;
        totalToiletPapers = 0;
        totalGoldenPapers = 0;
        totalPulls = 0;
        totalPushes = 0;
        totalPullOuts = 0;
        totalHangingSteps = 0;
        totalSlides = 0;
        treasures = 0;
        stagesCompleted = 0;
        itemsUsed = 0;
        itemsBought = 0;
        stagesUnlocked = 0;
        objectivesEarned = 0;
        boughtGearID = 0;
        skillsUsed = 0;
        achievementUnlocked = 0;

        isUnlimitedEnergy = false;
    }

    public void Initialize()
    {
        stars = new int[7][];
        levelsUnlocked = new bool[7][];
        chaptersUnlocked = new bool[7];
        challengeStars = new int[7][];
        challengeLevelsUnlocked = new bool[7][];
        challengeChaptersUnlocked = new bool[7];
        for (int i = 0; i < 7; i++)
        {
            stars[i] = new int[10];
            levelsUnlocked[i] = new bool[10];
            chaptersUnlocked[i] = false;
            challengeStars[i] = new int[10];
            challengeLevelsUnlocked[i] = new bool[10];
            challengeChaptersUnlocked[i] = false;
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
        chaptersUnlocked[0] = true;
    }

    public void Save()
    {
        string path = "game.data";
        var serializer = new XmlSerializer(typeof(Game));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

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
                return newG;
            }
        }
    }

    public void increaseCap()
    {
        energyCap += 2;
    }

    public void setTimeSinceEnergy()
    {
        timeSinceEnergy = System.DateTime.Now;
    }

    public int checkAndGetEnergy()
    {
        System.TimeSpan diff = System.DateTime.Now - timeSinceEnergy;
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

    public void setLastLogin()
    {
        lastLogin = System.DateTime.Now;
    }

    public void Update(int chapter, int level, int star)
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
                chaptersUnlocked[chapter + 1] = true;
            }
            challengeChaptersUnlocked[chapter] = true;
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
