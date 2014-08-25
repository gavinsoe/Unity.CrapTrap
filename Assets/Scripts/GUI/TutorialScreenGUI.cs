using UnityEngine;
using System.Collections;

public class TutorialScreenGUI : MonoBehaviour {

    private MainGameController mainController;

    public Texture bgTexture;
    public Texture[] tutorialScreen;
    public GUIStyle nextButtonStyle;
    public GUIStyle timerBorderStyle;
    public int borderScreenNum;

    public enum TriggerMode { onStart, Collider };
    public TriggerMode triggerMode;

    private CameraFollow mainCamera;
    private int screen = 0;
    private int maxScreen;

    // State variable
    private bool triggered = false;

    #region GUI related

    private float color_alpha = 0; // transparency

    private Rect bgRect;
    private Rect tutorialRect;

    private Rect btnRect;
    private float btnScale = 0.15f;
    private float btnXOffset = 0.06f; // Offset from the right
    private float btnYOffset = 0.01f; // Offset from the left

    #region timer border

    private Rect timerBorder; // Timer border (around edges of screen)

    #endregion

    #endregion

    void Awake()
    {
        mainCamera = Camera.main.GetComponent<CameraFollow>();
        // Retrieve the main game controller
        mainController = Camera.main.GetComponentInChildren<MainGameController>();
    }

    // Use this for initialization
	void Start () {

        bgRect = new Rect(0, 0, Screen.width, Screen.height);
        tutorialRect = new Rect(0, 0, Screen.width, Screen.height);

        // set the timer border
        var timer_margin = Screen.height * 0.02f;
        timerBorder = new Rect(timer_margin / 2, timer_margin / 2, Screen.width - timer_margin, Screen.height - timer_margin);

        // Button
        float btnHeight = Screen.height * btnScale;
        float btnWidth = btnHeight * ((float)nextButtonStyle.normal.background.width /
                                      (float)nextButtonStyle.normal.background.height);
        float xOffset = Screen.width - btnWidth - (Screen.width * btnXOffset);
        float yOffset = Screen.height - btnHeight - (Screen.height * btnYOffset);
        btnRect = new Rect(xOffset, yOffset, btnWidth, btnHeight);

        // calculate max screen
        maxScreen = tutorialScreen.Length;
	}

    void Update()
    {
        if (triggerMode == TriggerMode.onStart &&
            mainCamera.camState == CameraFollow.CameraStatus.TrackPlayer && !triggered)
        {
            triggered = true;
            showTutorial();
        }
    }

    void LateUpdate()
    {
        if (triggerMode == TriggerMode.onStart &&
               mainCamera.camState == CameraFollow.CameraStatus.TrackPlayer && !triggered)
        {
            mainCamera.paused = true;
        }
    }

	// Update is called once per frame
    void OnGUI()
    {
        #region temp

        #endregion
        GUI.depth = 0;
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, color_alpha);

        GUI.DrawTexture(bgRect, bgTexture,ScaleMode.ScaleAndCrop);
        GUI.DrawTexture(tutorialRect, tutorialScreen[screen], ScaleMode.ScaleToFit);
        if (screen + 1 == borderScreenNum)
        {
            GUI.Box(timerBorder, "", timerBorderStyle);
        }
        if (GUI.Button(btnRect, "", nextButtonStyle))
        {
            if ((screen + 1) < maxScreen)
            {
                screen++;
            }
            else
            {
                hideTutorial();
            }
        }
    }

    #region triggers

    // Detects collision with character and start tutorial
    void OnTriggerEnter2D(Collider2D col)
    {
        if (triggerMode == TriggerMode.Collider &&
            col.gameObject.tag == "Player" && !triggered && mainCamera.camState == CameraFollow.CameraStatus.FollowPlayer)
        {
            showTutorial();
            triggered = true;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (triggerMode == TriggerMode.Collider &&
            col.gameObject.tag == "Player" && !triggered && mainCamera.camState == CameraFollow.CameraStatus.FollowPlayer)
        {
            showTutorial();
            triggered = true;
        }
    }

    #endregion

    // Animate transparency
    void AnimateTransparency(float alpha)
    {
        color_alpha = alpha;
    }

    void showTutorial()
    {
        this.enabled = true;
        iTween.ValueTo(gameObject, 
                       iTween.Hash("from", color_alpha, 
                                   "to", 1, 
                                   "onupdate", "AnimateTransparency", 
                                   "easetype", iTween.EaseType.easeOutQuart));
        mainController.DisableTimeNMove();
    }

    void hideTutorial()
    {
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", color_alpha,
                                   "to", 0,
                                   "onupdate", "AnimateTransparency",
                                   "oncomplete", "DestroyObject",
                                   "easetype", iTween.EaseType.easeOutQuart));
        if (triggerMode != TriggerMode.onStart) mainController.EnableTimeNMove();
    }

    void DestroyObject()
    {
        mainCamera.paused = false;
        Destroy(gameObject);
    }
}
