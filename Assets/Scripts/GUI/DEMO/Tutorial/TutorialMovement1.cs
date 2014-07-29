using UnityEngine;
using System.Collections;

public class TutorialMovement1 : MonoBehaviour {

    public GUISkin activeSkin;
    private MainGameController mainController;
    private CameraFollow mainCamera;
    // State variables
    private bool triggered = false;
    public bool show = false;
    public bool hide = false;

    #region GUI related

    private float color_alpha; // transparency

    private Rect containerRect;
    private Rect bgContainerRect;

    private float containerWidth = 0.4f; // Width of the container (percentage of screen size)
    private float containerHeight = 0.16f; // Height of the container (percentage of screen size)
    private float containerYOffset = 0.1f; // Y Offset of the container (percentage of the screen size)
    private float containerHPadding = 0.1f; // Horizontal padding of the container (percentage of the container)
    private float containerVPadding = 0.1f; // Vertical padding of the container (percentage of the container)

    #region Text

    private float fontScale = 0.35f;

    #endregion
    #region shininggg fingeeerrr

    private float finger_scale = 0.5f;
    private Rect fingerRect;
    private Texture fingerTexture;
    private float fingerXOffset = 0.65f;
    private float fingerYOffset = 0.5f;
    private float fingerAlpha;
    private bool animationStarted = true;
    private int blinkCount;
    private const int MAX_COUNT = 2;

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

        // Le finger
        fingerTexture = activeSkin.customStyles[9].normal.background;
        float fingerDimension = Screen.height * finger_scale;
        fingerAlpha = 0;
        fingerRect = new Rect(Screen.width * fingerXOffset, Screen.height * fingerYOffset, fingerDimension, fingerDimension);
        blinkCount = 0;

    }

    // Update is called once per frame
	void OnGUI ()
    {
        if (show)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", color_alpha, "to", 1, "onupdate", "AnimateTransparency", "easetype", iTween.EaseType.easeOutQuart));
            show = false;
            animationStarted = false;
        }
        else if (hide)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", color_alpha, "to", 0, "onupdate", "AnimateTransparency", "easetype", iTween.EaseType.easeInQuart));
            hide = false;
        }

        GUI.skin = activeSkin;
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, color_alpha);

        // The Background
        GUI.Box(bgContainerRect, "");
        GUILayout.BeginArea(containerRect);
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("tap", activeSkin.customStyles[1]);
        GUILayout.Label("right or left to move", activeSkin.customStyles[0]);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.EndArea();

        // Finger
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, fingerAlpha);
        if (!animationStarted)
        {
            iTween.ValueTo(gameObject, 
                           iTween.Hash("from", fingerAlpha, 
                                       "to", 1, 
                                       "onupdate", "AnimateFinger", 
                                       "oncomplete", "AnimateFingerAlpha0",
                                       "easetype", iTween.EaseType.easeInQuart,
                                       "time", 1f));
            animationStarted = true;
        }
        GUI.DrawTexture(fingerRect, fingerTexture);
	}

    // Detects collision with character and start tutorial
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && !triggered && mainCamera.camState == CameraFollow.CameraStatus.FollowPlayer)
        {
            show = true;
            triggered = true;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && !triggered && mainCamera.camState == CameraFollow.CameraStatus.FollowPlayer)
        {
            show = true;
            triggered = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            hide = true;
        }
    }

    // animate iTween
    void AnimateTransparency(float alpha)
    {
        color_alpha = alpha;
    }

    // animate iTween
    void AnimateFinger(float alpha)
    {
        fingerAlpha = alpha;
    }

    void AnimateFingerAlpha0()
    {
        iTween.ValueTo(gameObject,
                              iTween.Hash("from", fingerAlpha,
                                          "to", 0,
                                          "onupdate", "AnimateFinger",
                                          "oncomplete", "AnimateFingerAlpha1",
                                          "easetype", iTween.EaseType.easeInQuart,
                                       "time", 0.5f));
    }

    void AnimateFingerAlpha1()
    {
        blinkCount++;
        if (blinkCount < MAX_COUNT)
        {
            iTween.ValueTo(gameObject,
                                  iTween.Hash("from", fingerAlpha,
                                              "to", 1,
                                              "onupdate", "AnimateFinger",
                                              "oncomplete", "AnimateFingerAlpha0",
                                              "easetype", iTween.EaseType.easeInQuart,
                                           "time", 0.5f));
        }
    }
}
