using UnityEngine;
using System.Collections;

public class NavigationManager : MonoBehaviour {
    public static NavigationManager instance;
    public int stage;
    public int chapter;

    void Awake()
    {
        // Make sure there is only 1 instance of this class.
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
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
        if (stage == 19)
        {
            stage = 1;
            chapter += 1;
        }
        else
        {
            stage += 1;
        }
        Application.LoadLevel(Application.loadedLevel + 1);
    }
}
