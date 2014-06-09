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
    private int fontSize;

    #endregion
    #region Arrow

    public Rect cur_ArrowUpRect;
    public Rect pos1_ArrowUpRect;
    public Rect pos2_ArrowUpRect;
    private Texture arrowUpTexture;

    public Rect cur_ArrowDownRect;
    public Rect pos1_ArrowDownRect;
    public Rect pos2_ArrowDownRect;
    private Texture arrowDownTexture;

    private float arrowScale = 0.23f; // Dimension of the blue circle (percentage of screen height)
    private bool animationStarted = false;

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

        fontSize = (int)(_containerHeight * fontScale);
        activeSkin.customStyles[0].fontSize = fontSize;
        activeSkin.customStyles[1].fontSize = fontSize;

        containerRect = new Rect(_containerXOffset, _containerYOffset, _containerWidth, _containerHeight);
        bgContainerRect = new Rect(_bgContainerXOffset, _bgContainerYOffset, _bgContainerWidth, _bgContainerHeight);

        var arrowDimension = Screen.height * arrowScale;

        arrowUpTexture = activeSkin.customStyles[7].normal.background;
        pos1_ArrowUpRect = new Rect(Screen.width * 0.3f, 0, arrowDimension, arrowDimension);
        pos2_ArrowUpRect = new Rect(pos1_ArrowUpRect.x + arrowDimension * 0.15f, pos1_ArrowUpRect.y + arrowDimension * 0.15f, arrowDimension, arrowDimension);
        cur_ArrowUpRect = pos1_ArrowUpRect;

        arrowDownTexture = activeSkin.customStyles[8].normal.background;
        pos1_ArrowDownRect = new Rect(Screen.width * 0.6f, Screen.height * 0.15f, arrowDimension, arrowDimension);
        pos2_ArrowDownRect = new Rect(pos1_ArrowDownRect.x + arrowDimension * 0.15f, pos1_ArrowDownRect.y - arrowDimension * 0.15f, arrowDimension, arrowDimension);
        cur_ArrowDownRect = pos2_ArrowDownRect;
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
            GUILayout.Label(@"QUICK! ", activeSkin.customStyles[1]);
            GUILayout.Label("Get to the toilet", activeSkin.customStyles[0]);
            //GUILayout.Label("toilet", activeSkin.customStyles[1]);
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
            GUILayout.Label("Notice this red pulsing bar?", activeSkin.customStyles[0]);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("This is your lifeline.", activeSkin.customStyles[0]);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        else if (screen == 2)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("The closer you are to letting go,", activeSkin.customStyles[0]);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("the faster it pulses. So hurry up!", activeSkin.customStyles[0]);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        else if (screen == 3)
        {
            hide = true;
            mainCamera.paused = false;
            screen++;
        }
        else if (screen == 4)
        {
            if (color_alpha == 0) this.enabled = false;
        }
        
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();

        GUILayout.EndArea();
        if (screen == 0)    
        {
            if (!animationStarted)
            {
                animationStarted = true;
                iTween.ValueTo(gameObject,
                          iTween.Hash("from", cur_ArrowDownRect,
                                      "to", pos2_ArrowDownRect,
                                      "onupdate", "AnimateArrowDown",
                                      "oncomplete", "onAnimateArrowDownPos2Complete",
                                      "time", 0.5f));
            }
            GUI.DrawTexture(cur_ArrowDownRect, arrowDownTexture);
        }
        else if (screen == 1)
        {
            if (!animationStarted)
            {
                animationStarted = true;
                iTween.ValueTo(gameObject,
                          iTween.Hash("from", cur_ArrowUpRect,
                                      "to", pos2_ArrowUpRect,
                                      "onupdate", "AnimateArrowUp",
                                      "oncomplete", "onAnimateArrowUpPos2Complete",
                                      "time", 0.5f));
            }
            GUI.DrawTexture(cur_ArrowUpRect, arrowUpTexture);
        }
        else if (screen == 2)
        {
            GUI.DrawTexture(cur_ArrowUpRect, arrowUpTexture);
        }
        if (GUI.Button(new Rect(0,0,Screen.width, Screen.height), "",activeSkin.customStyles[6]))
        {
            if (screen < 3 && color_alpha == 1)
            {
                screen++;
                animationStarted = false;
            }
        }
        
	}

    // animate iTween
    void AnimateTransparency(float alpha)
    {
        color_alpha = alpha;
    }

    // animate iTween
    void AnimateArrowUp(Rect size)
    {
        cur_ArrowUpRect = size;
    }
    void onAnimateArrowUpPos1Complete()
    {
        iTween.ValueTo(gameObject, 
                       iTween.Hash("from", cur_ArrowUpRect,
                                   "to", pos2_ArrowUpRect,
                                   "onupdate", "AnimateArrowUp", 
                                   "oncomplete","onAnimateArrowUpPos2Complete",
                                   "time", 0.5f));
    }
    void onAnimateArrowUpPos2Complete()
    {
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", cur_ArrowUpRect,
                                   "to", pos1_ArrowUpRect,
                                   "onupdate", "AnimateArrowUp",
                                   "oncomplete", "onAnimateArrowUpPos1Complete",
                                   "time", 0.5f));
    }

    // animate iTween
    void AnimateArrowDown(Rect size)
    {
        cur_ArrowDownRect = size;
    }
    void onAnimateArrowDownPos1Complete()
    {
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", cur_ArrowDownRect,
                                   "to", pos2_ArrowDownRect,
                                   "onupdate", "AnimateArrowDown",
                                   "oncomplete", "onAnimateArrowDownPos2Complete",
                                   "time", 0.5f));
    }
    void onAnimateArrowDownPos2Complete()
    {
        iTween.ValueTo(gameObject,
                       iTween.Hash("from", cur_ArrowDownRect,
                                   "to", pos1_ArrowDownRect,
                                   "onupdate", "AnimateArrowDown",
                                   "oncomplete", "onAnimateArrowDownPos1Complete",
                                   "time", 0.5f));
    }
}
