using UnityEngine;
using System.Collections;
using Soomla;
using Soomla.Store;

public class NavigationManager : MonoBehaviour {
    public static NavigationManager instance;
    public int stage;
    public int chapter;
    public string[] leaderboardIDs;

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
        // Redirect to stage select if ran out of plungers
        if (StoreInventory.GetItemBalance(CrapTrapAssets.CONSUMABLE_PLUNGER_ID) <= 0)
        {
            if (stage >= 7)
            {
                Application.LoadLevel("GUI_ChapterStageSelect_" + (chapter - 6).ToString());
            }
        }
        
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
        Application.LoadLevel("GUI_Achievement");
    }

    public void NavToCharacterPage()
    {
        // Navigate to character page
        Application.LoadLevel("GUI_CharacterPage");
    }

    public void NavToItemShop()
    {
        // Navigate to Item Shop
        Application.LoadLevel("GUI_ItemShop");
    }

    public void NavToChapterSelect()
    {
        // Navigate to chapter select
        Application.LoadLevel("GUI_ChapterSelect");
    }

    public void NavToCredits()
    {
        // Navigate to credits
        Application.LoadLevel("GUI_Credits");
    }

    public void NextStage()
    {
        // Loads the next stage (or screen)
        if (stage < 9)
        {
            stage += 1;
        }
        else if (stage == 9)
        {
            stage = 0;
            chapter += 1;
        }
        else if (stage < 19)
        {
            chapter += 1;
        }
        else
        {
            NavToChapterSelect();
        }
        Application.LoadLevel(Application.loadedLevel + 1);
    }

    public string getLeaderboardID()
    {
        return leaderboardIDs[chapter * 20 + stage];
    }
}
