using UnityEngine;
using System.Collections;

public enum Type
{
    steps = 1,
    climbs = 2,
    time = 3,
    withoutClimb = 4,
    toiletPapers = 5,
    goldenPapers = 6,
    pulls = 7,
    pushes = 8,
    pullOuts = 9,
    hangingSteps = 10,
    slides = 11,
    noHanging = 12,
    noPulling = 13,
    noPushing = 14,
    treasures = 15,
    noPullOuts = 16,
}

public enum Option
{
    lessThan = 1,
    greaterThan = 2,
    equal = 3,
}

[System.Serializable]
public class Objective : System.Object
{
    public Type type;
    public Option option;
    public int counter;
    public bool completed = false; // default state to false

    public override string ToString()
    {
        string stringified = "";

        if (type == Type.steps)
        {
            if (option == Option.lessThan)
            {
                stringified = "Complete stage in less than " + counter + " moves";
            }
            else if (option == Option.greaterThan)
            {
                stringified = "Complete stage using more than " + counter + " moves";
            }
            else if (option == Option.equal)
            {
                stringified = "Complete stage in " + counter + " moves";
            }
        }
        else if (type == Type.climbs)
        {
            if (option == Option.lessThan)
            {
                stringified = "Complete stage in less than " + counter + " climbs";
            }
            else if (option == Option.equal)
            {
                stringified = "Complete stage in " + counter + " climbs";
            }
            else if (option == Option.greaterThan)
            {
                stringified = "Complete stage using more than " + counter + " climbs";
            }
        }
        else if (type == Type.time)
        {
            int mins = (int)(counter / 60);
            int seconds = (int)(counter % 60);
            string time = string.Format("{0:00}:{1:00}", mins, seconds);

            if (option == Option.lessThan)
            {
                stringified = "Complete stage within " + time;
            }
        }
        else if (type == Type.withoutClimb)
        {
            stringified = "Complete stage without climbing";
        }
        else if (type == Type.toiletPapers)
        {
            if (option == Option.lessThan)
            {
                stringified = "Collect no more than " + counter + " toilet papers";
            }
            else if (option == Option.equal)
            {
                stringified = "Collect " + counter + " toilet papers";
            }
            else if (option == Option.greaterThan)
            {
                stringified = "Collect more than " + counter + " toilet papers";
            }
        }
        else if (type == Type.goldenPapers)
        {
            if (option == Option.lessThan)
            {
                stringified = "Collect no more than " + counter + " golden toilet papers";
            }
            else if (option == Option.equal)
            {
                stringified = "Collect " + counter + " golden toilet papers";
            }
            else if (option == Option.greaterThan)
            {
                stringified = "Collect more than " + counter + " golden toilet papers";
            }
        }
        else if (type == Type.pulls)
        {
            if (option == Option.lessThan)
            {
                stringified = "Complete stage using less than " + counter + " pulls";
            }
            else if (option == Option.equal)
            {
                stringified = "Complete stage using " + counter + " pulls";
            }
            else if (option == Option.greaterThan)
            {
                stringified = "Complete stage using more than " + counter + " pulls";
            }
        }
        else if (type == Type.pushes)
        {
            if (option == Option.lessThan)
            {
                stringified = "Complete stage using less than " + counter + " pushes";
            }
            else if (option == Option.equal)
            {
                stringified = "Complete stage using " + counter + " pushes";
            }
            else if (option == Option.greaterThan)
            {
                stringified = "Complete stage using more than " + counter + " pushes";
            }
        }
        else if (type == Type.pullOuts)
        {
            if (option == Option.lessThan)
            {
                stringified = "Complete stage by pulling out less than " + counter + " blocks";
            }
            else if (option == Option.equal)
            {
                stringified = "Complete stage by pulling out " + counter + " blocks";
            }
            else if (option == Option.greaterThan)
            {
                stringified = "Complete stage by pulling out more than " + counter + " blocks";
            }
        }
        else if (type == Type.hangingSteps)
        {
            if (option == Option.lessThan)
            {
                stringified = "Complete stage in less than " + counter + " shimmies";
            }
            else if (option == Option.equal)
            {
                stringified = "Complete stage in " + counter + " shimmies";
            }
            else if (option == Option.greaterThan)
            {
                stringified = "Complete stage using more than " + counter + " shimmies";
            }
        }
        else if (type == Type.slides)
        {
            if (option == Option.lessThan)
            {
                stringified = "Slide less than " + counter + " times";
            }
            else if (option == Option.equal)
            {
                stringified = "Slide " + counter + " times";
            }
            else if (option == Option.greaterThan)
            {
                stringified = "Slide more than " + counter + " times";
            }
        }
        else if (type == Type.noHanging)
        {
            stringified = "Complete stage without hanging";
        }
        else if (type == Type.noPulling)
        {
            stringified = "Complete stage without pulling";
        }
        else if (type == Type.noPushing)
        {
            stringified = "Complete stage without pushing";
        }
        else if (type == Type.treasures)
        {
            if (option == Option.lessThan)
            {
                stringified = "Collect no more than " + counter + " treasures";
            }
            else if (option == Option.equal)
            {
                stringified = "Collect " + counter + " treasures";
            }
            else if (option == Option.greaterThan)
            {
                stringified = "Collect more than " + counter + " treasures";
            }
        }
        else if (type == Type.noPullOuts)
        {
            stringified = "Complete stage without pulling out blocks";
        }
        else
        {
            stringified = "<objective unknown>";
        }

        return stringified;
    }

    /// <summary>
    /// This is used to track results in App42
    /// </summary>
    /// <returns></returns>
    public string ResultStat()
    {
        if (completed)
        {
            return "[Completed] " + this.ToString();
        }
        else
        {
            return "[Failed] " + this.ToString();
        }
    }
}
