﻿using UnityEngine;
using System.Collections;

public class PrologueController : MonoBehaviour {

    [System.Serializable]
    public class Frame
    {
        public string name;
        public Hook hook;
        public Texture sprite;
        public ScaleMode scaleMode;

        //[HideInInspector]
        public Rect position;
        
        public float startTime;
        public float endTime;

        public float startScale;
        public float startXOffset;
        public float startYOffset;
        public float transparency;

        public AnimationFrame[] animations;
    }

    [System.Serializable]
    public class AnimationFrame
    {
        public string name;
        public float triggerTime;
        public bool triggered = false;

        public Hook hook;
        public ScaleMode scaleMode;

        public float targetScale;
        public float targetXOffset;
        public float targetYOffset;
        public float targetAlpha;
        public float animationTime;

        public iTween.EaseType transitionType;

        public Rect targetRect(Texture sprite)
        {
            //Initialise
            float posHeight = Screen.height * targetScale;
            float posWidth = posHeight * ((float)sprite.width / (float)sprite.height);
            float posXOffset = Screen.width * targetXOffset;
            float posYOffset = Screen.height * targetYOffset;

            return CalculatePosition(sprite, targetScale, targetXOffset, targetYOffset, hook, scaleMode);
        }
    }

    public enum Hook { UpperLeft, UpperCenter, UpperRight, MiddleLeft, MiddleCenter, MiddleRight, LowerLeft, LowerCenter, LowerRight };
    public enum ScaleMode { Height, Width, FullScreen }

    public Frame[] frames;
    public AudioClip bgm;
    #region Timer

    public float timeElapsed;

    #endregion

    // Use this for initialization
	void Start () {
	    // Initialise Time
        timeElapsed = 0;

        // Initialise rect objects
        foreach (Frame frame in frames)
        {
            float posHeight = Screen.height * frame.startScale;
            float posWidth = posHeight * ((float)frame.sprite.width / (float)frame.sprite.height);
            float posXOffset = Screen.width * frame.startXOffset;
            float posYOffset = Screen.height * frame.startYOffset;

            frame.position = CalculatePosition(frame.sprite, frame.startScale, frame.startXOffset, frame.startYOffset, frame.hook, frame.scaleMode);
        }
	}
	
	// Update is called once per frame
	void Update () {
        timeElapsed += Time.deltaTime;
        //for (int i = 0; i < frames.Length; i++)
        foreach (Frame frame in frames)
        {
            StartCoroutine(TriggerAnimation(frame));
        }
	}

    public IEnumerator TriggerAnimation(Frame frame)
    {
        // Check for animation frames
        foreach (AnimationFrame anim in frame.animations)
        {
            if (!anim.triggered && anim.triggerTime <= timeElapsed)
            {
                iTween.ValueTo(gameObject,
                    iTween.Hash("from", frame.position,
                                "to", anim.targetRect(frame.sprite),
                                "onupdate", (System.Action<object>)(newVal => frame.position = (Rect)newVal),
                                "easetype", anim.transitionType,
                                "time", anim.animationTime));
                iTween.ValueTo(gameObject,
                    iTween.Hash("from", frame.transparency,
                                "to", anim.targetAlpha,
                                "onupdate", (System.Action<object>)(newVal => frame.transparency = (float)newVal),
                                "easetype", anim.transitionType,
                                "time", anim.animationTime));
                anim.triggered = true;
                yield return null;
            }
        }
    }
    void OnGUI()
    {
        foreach (Frame frame in frames)
        {
            if (frame.startTime < timeElapsed && timeElapsed < frame.endTime)
            {
                GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, frame.transparency);
                GUI.DrawTexture(frame.position, frame.sprite);
            }
        }
    }

    private static Rect CalculatePosition(Texture sprite, float scale, float xOffset, float yOffset, Hook hook, ScaleMode sMode)
    {
        float posHeight;
        float posWidth;
        float posXOffset;
        float posYOffset;

        if (sMode == ScaleMode.Height)
        {
            posHeight = Screen.height * scale;
            posWidth = posHeight * ((float)sprite.width / (float)sprite.height);
        }
        else if (sMode == ScaleMode.Width)
        {
            posWidth = Screen.width * scale;
            posHeight = posWidth * ((float)sprite.height / (float)sprite.width);
        }
        else
        {
            posHeight = Screen.height;
            posWidth = Screen.width;
        }

        if (sMode != ScaleMode.FullScreen)
        {
            if (hook == Hook.UpperLeft)
            {
                posXOffset = Screen.width * yOffset;
                posYOffset = Screen.height * yOffset;
            }
            else if (hook == Hook.UpperCenter)
            {
                posXOffset = (Screen.width - posWidth) * 0.5f + (Screen.width * xOffset);
                posYOffset = Screen.height * yOffset;
            }
            else if (hook == Hook.UpperRight)
            {
                posXOffset = Screen.width - posWidth - (Screen.width * xOffset);
                posYOffset = Screen.height * yOffset;
            }
            else if (hook == Hook.MiddleLeft)
            {
                posXOffset = Screen.width * yOffset;
                posYOffset = (Screen.height - posHeight) * 0.5f + (Screen.height * yOffset);
            }
            else if (hook == Hook.MiddleCenter)
            {
                posXOffset = (Screen.width - posWidth) * 0.5f + Screen.width * yOffset;
                posYOffset = (Screen.height - posHeight) * 0.5f + (Screen.height * yOffset);
            }
            else if (hook == Hook.MiddleRight)
            {
                posXOffset = Screen.width - posWidth - (Screen.width * xOffset);
                posYOffset = (Screen.height - posHeight) * 0.5f + (Screen.height * yOffset);
            }
            else if (hook == Hook.LowerLeft)
            {
                posXOffset = Screen.width * yOffset;
                posYOffset = Screen.height - posHeight - (Screen.height * yOffset);
            }
            else if (hook == Hook.LowerCenter)
            {
                posXOffset = (Screen.width - posWidth) * 0.5f + Screen.width * yOffset;
                posYOffset = Screen.height - posHeight - (Screen.height * yOffset);
            }
            else if (hook == Hook.LowerRight)
            {
                posXOffset = Screen.width - posWidth - (Screen.width * xOffset);
                posYOffset = Screen.height - posHeight - (Screen.height * yOffset);
            }
            else //should never end up here
            {
                posXOffset = 0;
                posYOffset = 0;
            }
        }
        else
        {
            posXOffset = 0;
            posYOffset = 0;
        }

        return new Rect(posXOffset, posYOffset, posWidth, posHeight);
    }
}
