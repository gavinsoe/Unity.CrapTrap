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

    public Dictionary<string, int> stars;
    public Dictionary<string, bool> lockedLevels;

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
        chaptersUnlocked = new bool[1];
        stars = new Dictionary<string, int>();
        lockedLevels = new Dictionary<string, bool>();
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

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(Game));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static Game Load(string path) {
        var serializer = new XmlSerializer(typeof(Game));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as Game;
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

    public void initializeStars()
    {
        stars = new Dictionary<string, int>()
        {
            {"C1-1", 0}
        };
    }

    public void initializeLevels()
    {
        lockedLevels = new Dictionary<string, bool>()
        {
            {"C1-1", false}
        };
    }
}
