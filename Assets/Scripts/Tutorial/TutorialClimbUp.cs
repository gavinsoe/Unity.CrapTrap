using UnityEngine;
using System.Collections;

public class TutorialClimbUp : MonoBehaviour {

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

    private float containerWidth = 0.45f; // Width of the container (percentage of screen size)
    private float containerHeight = 0.16f; // Height of the container (percentage of screen size)
    private float containerYOffset = 0.1f; // Y Offset of the container (percentage of the screen size)
    private float containerHPadding = 0.1f; // Horizontal padding of the container (percentage of the container)
    private float containerVPadding = 0.1f; // Vertical padding of the container (percentage of the container)

    #region Text

    private float fontScale = 0.35f;

    #endregion
    #region Blue Arrow

    public Rect blueArrowRect;
    private Texture blueArrowTexture;
    private float blueArrowScale = 0.3f; // Dimension of the blue circle (percentage of screen height)
    private float blueArrowXOffset = 0.59f;
    private float blueArrowYOffset = 0.46f;

    #endregion
    #region shininggg fingeeerrr

    private float finger_scale = 0.5f;

    private Rect cur_fingerRect;
    private Rect pos1_fingerRect;
    private Rect pos2_fingerRect;
    private Texture fingerTexture;

    private float fingerXOffset = 0.6f;
    private float fingerYOffset1 = 0.75f;
    private float fingerYOffset2 = 0.5f;

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

        // Blue Arrow
        blueArrowTexture = activeSkin.customStyles[5].normal.background;
        var blueArrowHeight = Screen.height * blueArrowScale;
        var blueArrowWidth = blueArrowHeight * ((float)blueArrowTexture.width / (float)blueArrowTexture.height);

        blueArrowRect = new Rect(Screen.width * blueArrowXOffset, Screen.height * blueArrowYOffset, blueArrowWidth, blueArrowHeight);

        // Finger
        fingerTexture = activeSkin.customStyles[9].normal.background;
        float fingerDimension = Screen.height * finger_scale;
        pos1_fingerRect = new Rect(Screen.width * fingerXOffset, Screen.height * fingerYOffset1, fingerDimension, fingerDimension);
        pos2_fingerRect = new Rect(Screen.width * fingerXOffset, Screen.height * fingerYOffset2, fingerDimension, fingerDimension);
        cur_fingerRect = pos1_fingerRect;
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
        GUILayout.Label("drag", activeSkin.customStyles[1]);
        GUILayout.Label("up to climb back", activeSkin.customStyles[0]);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.EndArea();

        //GUI.DrawTexture(blueArrowRect, blueArrowTexture);
        if (!animationStarted)
        {
            animationStarted = true;
            iTween.ValueTo(gameObject,
                      iTween.Hash("from", cur_fingerRect,
                                  "to", pos2_fingerRect,
                                  "onupdate", "AnimateFinger",
                                  "oncomplete", "onAnimateFingerComplete",
                                  "time", 1f));
        }
        if (blinkCount < MAX_COUNT) GUI.DrawTexture(cur_fingerRect, fingerTexture);
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

    // animate finger
    void AnimateFinger(Rect size)
    {
        cur_fingerRect = size;
    }

    void onAnimateFingerComplete()
    {
        blinkCount++;
        if (blinkCount < MAX_COUNT)
        {
            cur_fingerRect = pos1_fingerRect;
            iTween.ValueTo(gameObject,
                      iTween.Hash("from", cur_fingerRect,
                                  "to", pos2_fingerRect,
                                  "onupdate", "AnimateFinger",
                                  "oncomplete", "onAnimateFingerComplete",
                                  "time", 1f));
        }
    }
}
