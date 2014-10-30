using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CTAchievements
{
    public CTAchievement[] rows;
};

public class AchievementController : MonoBehaviour {

    // Achievement Variables
    public CTAchievements[] achievements = new CTAchievements[5];
    public string[] titles;
    public Texture[] rewards;
    public string[] rewardAmount;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
