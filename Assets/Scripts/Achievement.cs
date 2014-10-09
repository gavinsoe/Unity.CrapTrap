using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public enum Type2
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
}
[System.Serializable]
public class Achievement : System.Object
{

    public Type2 type;
    public int counter;
    public string title;
    public string rewardID;
    public bool isDone;
    public string achievementID;

    public string ToString()
    {
        string stringified = "";

        if (type == Type2.totalSteps)
        {
            stringified = "Has moved a total of " + counter + " times";
        }
        else if (type == Type2.totalClimbs)
        {
            stringified = "Has climbed a total of " + counter + " times";
        }
        else if (type == Type2.playingTime)
        {
            int mins = (int)(counter / 60);
            int seconds = (int)(counter % 60);
            string time = string.Format("{0:00}:{1:00}", mins, seconds);
            stringified = "Has played for " + time;
        }
        else if (type == Type2.toiletPapers)
        {
            stringified = "Has collected a total of " + counter + " toilet papers";
        }
        else if (type == Type2.goldenPapers)
        {
            stringified = "Has collected a total of " + counter + " golden toilet papers";
        }
        else if (type == Type2.totalPulls)
        {
            stringified = "Has pulled a total of " + counter + " times";
        }
        else if (type == Type2.totalPushes)
        {
            stringified = "Has pushed a total of " + counter + " times";
        }
        else if (type == Type2.totalPullOuts)
        {
            stringified = "Has pulled out a total of " + counter + " times";
        }
        else if (type == Type2.totalHangingSteps)
        {
            stringified = "Has moved a total of " + counter + " times while hanging";
        }
        else if (type == Type2.totalSlides)
        {
            stringified = "Has slide a total of " + counter + " times";
        }
        else if (type == Type2.treasures)
        {
            stringified = "Has collected a total of " + counter + " treasures";
        }
        else if (type == Type2.stagesCompleted)
        {
            stringified = "Has completed " + counter + " stages";
        }
        else if (type == Type2.itemsUsed)
        {
            stringified = "Has used " + counter + " items";
        }
        else if (type == Type2.itemsBought)
        {
            stringified = "Has bought " + counter + " items";
        }
        else if (type == Type2.stagesUnlocked)
        {
            stringified = "Has unlocked " + counter + " stages";
        }
        else if (type == Type2.objectivesEarned)
        {
            stringified = "Has finished " + counter + " objectives";
        }
        else if (type == Type2.chaptersUnlocked)
        {
            stringified = "Has unlocked " + counter + " chapters";
        }
        else if (type == Type2.skillsUsed)
        {
            stringified = "Has used " + counter + " skills";
        }
        else if (type == Type2.achievementUnlocked)
        {
            stringified = "Has done the " + title + " achievement";
        }
        else
        {
            stringified = "<objective unknown>";
        }

        return stringified;
    }
}
