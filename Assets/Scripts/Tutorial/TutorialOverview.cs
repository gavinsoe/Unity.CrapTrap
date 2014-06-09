using UnityEngine;
using System.Collections;

public class TutorialOverview : MonoBehaviour {

    public GUISkin activeSkin;
    private MainGameController mainController;
    private CameraFollow mainCamera;
    public int screen = 0;

    // State variables
    private bool triggered = false;
    public bool show = false;
    public bool hide = false;

    #region GUI related

    private float color_alpha = 0; // transparency

    private Rect containerRect;
    private Rect bgContainerRect;

    private float containerWidth = 0.5f; // Width of the container (percentage of screen size)
    private float containerHeight = 0.16f; // Height of the container (percentage of screen size)
    private float containerYOffset = 0.366f; // Y Offset of the container (percentage of the screen size)
    private float containerHPadding = 0.1f; // Horizontal padding of the container (percentage of the container)
    private float containerVPadding = 0.1f; // Vertical padding of the container (percentage of the container)

    #region Text

    private float fontScale = 0.35f;

    #endregion

    #endregion

    void Awake()
    {
        mainCamera = Camera.main.GetComponent<CameraFollow>();

        // Locate the textbox
        float _containerXOffset = Screen.width * ((1 - containerWidth) / 2);
        float _containerYOffset = Screen.width * containerYOffset;
        float _containerWidth = Screen.width * containerWidth;
        float _containerHeight = Screen.height * containerHeight;

        float _bgContainerXOffset = _containerXOffset - (_containerWidth * containerHPadding);
        float _bgContainerYOffset = _containerYOffset - (_containerHeight * containerVPadding);
        float _bgContainerWidth = _containerWidth + 2 * (_containerWidth * containerHPadding);
        float _bgContainerHeight = _containerHeight + 2 * (_containerHeight * containerVPadding);

        int fontSize = (int)(_containerHeight * fontScale);
        activeSkin.customStyles[0].fontSize = fontSize;
        activeSkin.customStyles[1].fontSize = fontSize;

        containerRect = new Rect(_containerXOffset, _containerYOffset, _containerWidth, _containerHeight);
        bgContainerRect = new Rect(_bgContainerXOffset, _bgContainerYOffset, _bgContainerWidth, _bgContainerHeight);

    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera.camState == CameraFollow.CameraStatus.TrackPlayer && !triggered)
        {
            mainCamera.paused = true;
            triggered = true;
            show = true;
        }
    }

	void OnGUI ()
    {
        #region temp

        // Locate the textbox
        float _containerXOffset = Screen.width * ((1 - containerWidth) / 2);
        float _containerYOffset = Screen.width * containerYOffset;
        float _containerWidth = Screen.width * containerWidth;
        float _containerHeight = Screen.height * containerHeight;

        float _bgContainerXOffset = _containerXOffset - (_containerWidth * containerHPadding);
        float _bgContainerYOffset = _containerYOffset - (_containerHeight * containerVPadding);
        float _bgContainerWidth = _containerWidth + 2 * (_containerWidth * containerHPadding);
        float _bgContainerHeight = _containerHeight + 2 * (_containerHeight * containerVPadding);

        int fontSize = (int)(_containerHeight * fontScale);
        activeSkin.customStyles[0].fontSize = fontSize;
        activeSkin.customStyles[1].fontSize = fontSize;

        containerRect = new Rect(_containerXOffset, _containerYOffset, _containerWidth, _containerHeight);
        bgContainerRect = new Rect(_bgContainerXOffset, _bgContainerYOffset, _bgContainerWidth, _bgContainerHeight);

        #endregion
        

        if (show)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", color_alpha, "to", 1, "onupdate", "AnimateTransparency", "easetype", iTween.EaseType.easeOutQuart));
            show = false;
        }
        else if (hide)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", color_alpha, "to", 0, "onupdate", "AnimateTransparency", "easetype", iTween.EaseType.easeInQuart,"time", 0.2f));
            hide = false;
        }

        GUI.skin = activeSkin;
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, color_alpha);

        // The Background
        GUI.Box(bgContainerRect, "");
        GUILayout.BeginArea(containerRect);

        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        if (screen == 0)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("quickly get to the", activeSkin.customStyles[0]);
            GUILayout.Label("toilet", activeSkin.customStyles[1]);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("before your", activeSkin.customStyles[0]);
            GUILayout.Label("bowels burst!", activeSkin.customStyles[1]);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        else if (screen == 1)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("the", activeSkin.customStyles[0]);
            GUILayout.Label("pulsing red border", activeSkin.customStyles[1]);
            GUILayout.Label("indicates", activeSkin.customStyles[0]);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("how close you are from", activeSkin.customStyles[0]);
            GUILayout.Label("bursting", activeSkin.customStyles[1]);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        else if (screen == 2)
        {
            hide = true;
            mainCamera.paused = false;
            screen++;
        }
        else if (screen == 3)
        {
            if (color_alpha == 0) this.enabled = false;
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();

        GUILayout.EndArea();
        if (GUI.Button(new Rect(0,0,Screen.width, Screen.height), "",activeSkin.customStyles[6]))
        {
            if (screen < 2 && color_alpha == 1) screen ++;
        }
        
	}

    // animate iTween
    void AnimateTransparency(float alpha)
    {
        color_alpha = alpha;
    }
}
