﻿using UnityEngine;
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
    #region Blue shit

    public Rect blueCircle1Rect;
    public Rect blueCircle2Rect;

    private Texture blueCircleTexture;
    private float blueCircleScale = 0.23f; // Dimension of the blue circle (percentage of screen height)

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

        // Blue circles
        blueCircleTexture = activeSkin.customStyles[2].normal.background;
        var circleDimension = Screen.height * blueCircleScale;

        blueCircle1Rect = new Rect(Screen.width / 4 - circleDimension / 2, (Screen.height - circleDimension) * 0.5f, circleDimension, circleDimension);
        blueCircle2Rect = new Rect(3 * (Screen.width / 4) - circleDimension / 2, (Screen.height - circleDimension) * 0.5f, circleDimension, circleDimension);
    }

    // Update is called once per frame
	void OnGUI ()
    {
        if (show)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", color_alpha, "to", 1, "onupdate", "AnimateTransparency", "easetype", iTween.EaseType.easeOutQuart));
            show = false;
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

        // Blue circles

        GUI.DrawTexture(blueCircle1Rect, blueCircleTexture);
        GUI.DrawTexture(blueCircle2Rect, blueCircleTexture);
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
}