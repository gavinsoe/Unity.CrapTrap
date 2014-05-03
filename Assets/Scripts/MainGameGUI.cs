using UnityEngine;
using System.Collections;

public class MainGameGUI : MonoBehaviour {
    
    public int ntp = 0;
    public int gtp = 0;

    public Texture ntpTexture; // Texture for 'normal toilet paper'
    public Texture gtpTexture; // Texture for 'golden toilet paper'
    public GUIStyle gtpStyle;
    public GUIStyle ntpStyle;

    public float fontScale = 0.04f;
    public float xOffset = 0.03f;
    public float yOffset = -0.055f;
    
    // Make the box
    void OnGUI(){

        float height = Screen.height / 14;
        float width = Screen.width / 8;
        NTPScore(width, height);
        GTPScore(width, height);
    }

    void NTPScore(float width, float height)
    {
        ntpStyle.fontSize = (int)(Screen.height * fontScale);
        ntpStyle.contentOffset = new Vector2(width * xOffset, height * yOffset);
        GUI.Label(new Rect(Screen.width - 2 * width, 0, width, height), ntp.ToString() , ntpStyle);
    }

    void GTPScore(float width, float height)
    {
        gtpStyle.fontSize = (int)(Screen.height * fontScale);
        gtpStyle.contentOffset = new Vector2(width * xOffset, height * yOffset);
        GUI.Label(new Rect(Screen.width - width, 0, width, height), gtp.ToString() , gtpStyle);
    }
}
