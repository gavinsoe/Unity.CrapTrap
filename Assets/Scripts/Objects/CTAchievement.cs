using UnityEngine;
using System.Collections;

public enum Stat
{
    totalSteps = 1,
    totalClimbs = 2,
    playingTime = 3,
    toiletPapers = 4,
    goldenPapers = 5,
    totalPulls = 6,
    totalPushes = 7,
    totalPullOuts = 8,
    totalHangingSteps = 9,
    totalSlides = 10,
    treasures = 11,
    stagesCompleted = 12,
    itemsUsed = 13,
    itemsBought = 14,
    stagesUnlocked = 15,
    objectivesEarned = 16,
    boughtGearID = 17,
    chaptersUnlocked = 18,
    skillsUsed = 19,
    achievementUnlocked = 20,
    consecutiveLogins = 21,
    capsules = 22,
    fails = 23,
    eqsBought = 24,
    getGear = 25,
}

public enum Gear
{
    mummy = 1,
    pirate = 2,
    tribal = 3,
    diver = 4,
    explorer = 5,
}
[System.Serializable]
public class CTAchievement : System.Object
{

    public Stat stat;
    public int counter;
    public string title;
    public string rewardID;
    public bool isDone;
    public string achievementID;
    public string itemID;
    public Gear gear;

    public string ToString()
    {
        string stringified = "";

        if (stat == Stat.totalSteps)
        {
            stringified = "Has moved a total of " + counter + " times";
        }
        else if (stat == Stat.totalClimbs)
        {
            stringified = "Has climbed a total of " + counter + " times";
        }
        else if (stat == Stat.playingTime)
        {
            int mins = (int)(counter / 60);
            int seconds = (int)(counter % 60);
            string time = string.Format("{0:00}:{1:00}", mins, seconds);
            stringified = "Has played for " + time;
        }
        else if (stat == Stat.toiletPapers)
        {
            stringified = "Has collected a total of " + counter + " toilet papers";
        }
        else if (stat == Stat.goldenPapers)
        {
            stringified = "Has collected a total of " + counter + " golden toilet papers";
        }
        else if (stat == Stat.totalPulls)
        {
            stringified = "Has pulled a total of " + counter + " times";
        }
        else if (stat == Stat.totalPushes)
        {
            stringified = "Has pushed a total of " + counter + " times";
        }
        else if (stat == Stat.totalPullOuts)
        {
            stringified = "Has pulled out a total of " + counter + " times";
        }
        else if (stat == Stat.totalHangingSteps)
        {
            stringified = "Has moved a total of " + counter + " times while hanging";
        }
        else if (stat == Stat.totalSlides)
        {
            stringified = "Has slide a total of " + counter + " times";
        }
        else if (stat == Stat.treasures)
        {
            stringified = "Has collected a total of " + counter + " treasures";
        }
        else if (stat == Stat.stagesCompleted)
        {
            stringified = "Has completed " + counter + " stages";
        }
        else if (stat == Stat.itemsUsed)
        {
            stringified = "Has used " + counter + " items";
        }
        else if (stat == Stat.itemsBought)
        {
            stringified = "Has bought " + counter + " items";
        }
        else if (stat == Stat.stagesUnlocked)
        {
            stringified = "Has unlocked " + counter + " stages";
        }
        else if (stat == Stat.objectivesEarned)
        {
            stringified = "Has finished " + counter + " objectives";
        }
        else if (stat == Stat.chaptersUnlocked)
        {
            stringified = "Has unlocked " + counter + " chapters";
        }
        else if (stat == Stat.skillsUsed)
        {
            stringified = "Has used " + counter + " skills";
        }
        else if (stat == Stat.achievementUnlocked)
        {
            stringified = "Has done the " + title + " achievement";
        }
        else if (stat == Stat.capsules)
        {
            stringified = "Has collected a total of " + counter + " times";
        }
        else if (stat == Stat.fails)
        {
            stringified = "Has failed a total of " + counter + " times";
        }
        else if (stat == Stat.eqsBought)
        {
            stringified = "Has bought a total of " + counter + " equipments";
        }
        else if (stat == Stat.getGear)
        {
            if (gear == Gear.diver)
            {
                stringified = "Has gotten the Diver gear set";
            }
            else if (gear == Gear.explorer)
            {
                stringified = "Has gotten the Explorer gear set";
            }
            else if (gear == Gear.tribal)
            {
                stringified = "Has gotten the Tribal gear set";
            }
            else if (gear == Gear.pirate)
            {
                stringified = "Has gotten the Pirate gear set";
            }
            else if (gear == Gear.mummy)
            {
                stringified = "Has gotten the Mummy gear set";
            }
        }
        else
        {
            stringified = "<objective unknown>";
        }

        return stringified;
    }
}
