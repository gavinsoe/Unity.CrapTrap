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

    public string ToString()
    {
        string stringified = "";

        if (type == Type.steps)
        {
            if (option == Option.lessThan)
            {
                stringified = "Finish stage in less than " + counter + " moves";
            }
            else if (option == Option.greaterThan)
            {
                stringified = "Finish stage using more than " + counter + " moves";
            }
            else if (option == Option.equal)
            {
                stringified = "Finish stage in " + counter + " moves";
            }
        }
        else
        {
            stringified = "<objective unknown>";
        }

        return stringified;
    }

}
