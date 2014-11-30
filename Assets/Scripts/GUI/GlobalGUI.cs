using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalGUI : MonoBehaviour 
{
    public static GlobalGUI instance;
    private Queue<CTAchievement> achievementQueue;

    /* GUI Skin
     * Custom Styles [0] = Achievement Popup Frame
     */
    public bool show;
    public GUISkin activeSkin;

    #region GUI Related

    #region Achievement Popup

    public bool achievementPopupVisible;
    private float achievementPopupAlpha;
    private Rect achievementContainerRect;

    private Rect achievementPopupRect;
    private Rect achievementPopupClosed;
    private Rect achievementPopupOpened;
    private Texture achievementPopupTexture;
    private float achievementPopupScale = 0.4f;
    private float achievementPopupYOffset = -0.05f;

    private GUIStyle achievementLabelStyle;
    private Rect achievementLabelRect;
    private Rect achievementLabelClosed;
    private Rect achievementLabelOpened;
    private float achievementLabelScale = 0.08f;
    private float achievementLabelXOffset = 0.315f;
    private float achievementLabelYOffset = 0.5f;
    private float achievementLabelWidth = 0.4f;
    private float achievementLabelHeight = 0.2f;

    public string achievementText;

    #endregion

    #endregion

    void Awake()
    {
        // Make sure there is only 1 instance of this class.
        if (instance == null)
        {
            show = false;
            achievementQueue = new Queue<CTAchievement>();
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        #region Calculate GUI Components

        // Calculate size of achievement popup
        achievementPopupTexture = activeSkin.customStyles[0].normal.background;
        float aPopupHeight = Screen.height * achievementPopupScale;
        float aPopupWidth = aPopupHeight * ((float)achievementPopupTexture.width / (float)achievementPopupTexture.height);
        float aPopupXOffset = 0.5f * (Screen.width - aPopupWidth);
        float aPopupYOffset = Screen.height * achievementPopupYOffset;

        achievementContainerRect = new Rect(aPopupXOffset, aPopupYOffset, aPopupWidth, aPopupHeight);
        achievementPopupOpened = new Rect(0, 0, aPopupWidth, aPopupHeight);
        achievementPopupClosed = new Rect(aPopupWidth * 0.5f, aPopupHeight * 0.5f, 0, 0);

        achievementLabelStyle = new GUIStyle(activeSkin.label);
        achievementLabelStyle.fontSize = (int)(aPopupHeight * achievementLabelScale);
        float contentXOffset = aPopupWidth * achievementLabelXOffset;
        float contentYOffset = aPopupHeight * achievementLabelYOffset;
        float contentWidth = aPopupWidth * achievementLabelWidth;
        float contentHeight = aPopupHeight * achievementLabelHeight;

        achievementLabelOpened = new Rect(contentXOffset, contentYOffset, contentWidth, contentHeight);
        achievementLabelClosed = new Rect(0.5f * (contentXOffset + contentWidth), 0.5f * (contentYOffset + contentHeight), 0, 0);

        ResetAchievementPopup();

        #endregion
    }

	// Update is called once per frame
	void OnGUI ()
    {
        // Sets the GUI depth
        GUI.depth = 0;

        // Set the active skin
        GUI.skin = activeSkin;

        AchievementPopup();
    }

    void Update()
    {

        // Calculate size of achievement popup
        achievementPopupTexture = activeSkin.customStyles[0].normal.background;
        float aPopupHeight = Screen.height * achievementPopupScale;
        float aPopupWidth = aPopupHeight * ((float)achievementPopupTexture.width / (float)achievementPopupTexture.height);
        float aPopupXOffset = 0.5f * (Screen.width - aPopupWidth);
        float aPopupYOffset = Screen.height * achievementPopupYOffset;

        achievementContainerRect = new Rect(aPopupXOffset, aPopupYOffset, aPopupWidth, aPopupHeight);
        achievementPopupOpened = new Rect(0, 0, aPopupWidth, aPopupHeight);
        achievementPopupClosed = new Rect(aPopupWidth * 0.5f, aPopupHeight * 0.5f, 0, 0);

        achievementLabelStyle = new GUIStyle(activeSkin.label);

        float textWidth = activeSkin.customStyles[0].CalcSize(new GUIContent(achievementText)).x;
        float newScale = 0.08f;
        float popWidth = aPopupWidth * (45f / 100);
        if (textWidth < popWidth)
        {
            newScale = 0.08f;
        }
        else if (textWidth > aPopupWidth * (45f / 100))
        {
            newScale = (aPopupWidth * (45f / 100)) / textWidth * 0.08f;
        }
        if (newScale < 0.06f)
        {
            newScale = 0.06f;
        }

        achievementLabelStyle.fontSize = (int)(aPopupHeight * newScale);
        float contentXOffset = aPopupWidth * achievementLabelXOffset;
        float contentYOffset = aPopupHeight * achievementLabelYOffset;
        float contentWidth = aPopupWidth * achievementLabelWidth;
        float contentHeight = aPopupHeight * achievementLabelHeight;

        achievementLabelOpened = new Rect(contentXOffset, contentYOffset, contentWidth, contentHeight);
        achievementLabelClosed = new Rect(0.5f * (contentXOffset + contentWidth), 0.5f * (contentYOffset + contentHeight), 0, 0);
        /*
        if (show)
        {
            AchievementUnlocked(new CTAchievement());
            show = false;
        }
         * */
        if (achievementQueue.Count > 0 && !show)
        {
            AchievementUnlocked(achievementQueue.Dequeue());
        }
    }


    #region GUI Sections

    void AchievementPopup()
    {
        var color = GUI.color;
        GUI.color = new Color(color.r, color.g, color.b, achievementPopupAlpha);
        GUI.BeginGroup(achievementContainerRect);
        {
            GUI.DrawTexture(achievementPopupRect, achievementPopupTexture);
            GUI.Label(achievementLabelRect, achievementText, achievementLabelStyle);
        }
        GUI.EndGroup();
        GUI.color = color;
    }

    #endregion
    #region Animations

    void AnimateAchievementPopup(Rect pos)
    {
        achievementPopupRect = pos;
    }
    void AnimateAchievementLabel(Rect pos)
    {
        achievementLabelRect = pos;
        achievementLabelStyle.fontSize = (int)(achievementPopupRect.height * achievementLabelScale);
    }
    void AnimateAchievementAlpha(float alpha)
    {
        achievementPopupAlpha = alpha;
    }

    void DelayAnimation(int placeholder)
    {
        // Do Nothing
    }

    #endregion
    #region public methods

    public void AchievementUnlocked(CTAchievement achievement)
    {
        show = true;
        //this.enabled = true;
        achievementPopupVisible = true;
        achievementText = achievement.title;

        iTween.ValueTo(gameObject,
                       iTween.Hash("from", achievementPopupClosed,
                                   "to", achievementPopupOpened,
                                   "onupdate", "AnimateAchievementPopup",
                                   "easetype", iTween.EaseType.easeOutBack,
                                   "time", 0.5f));
        
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", achievementLabelClosed,
                                   "to", achievementLabelOpened,
                                   "onupdate", "AnimateAchievementLabel",
                                   "easetype", iTween.EaseType.easeOutBack,
                                   "time", 0.5f));
        
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", 0,
                                   "to", 1,
                                   "onupdate", "AnimateAchievementAlpha",
                                   "easetype", iTween.EaseType.easeOutBack,
                                   "time", 0.5f));

        // This part would probably not make sense, it's just to delay before hiding the poup
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", 0,
                                   "to", 0,
                                   "onupdate","DelayAnimation",
                                   "oncomplete", "HideAchievementPopup",
                                   "time", 3f));
    }

    public void HideAchievementPopup() 
    {
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", 1,
                                   "to", 0,
                                   "onupdate", "AnimateAchievementAlpha",
                                   "oncomplete", "ResetAchievementPopup",
                                   "easetype", iTween.EaseType.linear,
                                   "time", 1f));
    }

    public void ResetAchievementPopup()
    {
        achievementPopupVisible = false;
        achievementPopupRect = achievementPopupClosed; 
        achievementLabelRect = achievementLabelClosed;
        achievementPopupAlpha = 0;

        show = false;
        //this.enabled = false;
    }

    public void AddAchievement(CTAchievement achievement) 
    {
        achievementQueue.Enqueue(achievement);
    }
    #endregion
}
