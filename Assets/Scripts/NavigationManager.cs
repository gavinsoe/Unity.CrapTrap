using UnityEngine;
using System.Collections;

public class NavigationManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void RetryLevel()
    {
        // Restart level
        Application.LoadLevel(Application.loadedLevel);
    }

    public void NavToTitle()
    {
        // Return to title screen
        Application.LoadLevel("GUI_TitleScreen");
    }

    public void NavToAchievements()
    {
        // Navigate to achievements page
    }

    public void NavToCharacterPage()
    {
        // Navigate to character page
    }

    public void NavToItemShop()
    {
        // Navigate to Item Shop
    }

    public void NavToChapterSelect()
    {
        // Navigate to chapter select
        Application.LoadLevel("GUI_ChapterSelect");
    }

    public void NextStage()
    {
        // Loads the next stage (or screen)
        Application.LoadLevel(Application.loadedLevel + 1);
    }
}
