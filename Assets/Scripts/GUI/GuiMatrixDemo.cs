using UnityEngine;
using System.Collections;
using System;

public class GuiMatrixDemo : MonoBehaviour
{
    private float rotation;

    /// <summary>
    /// OnGUI is called for rendering and handling GUI events.
    /// </summary>
    public void OnGUI()
    {
        var matrix = GUI.matrix;

        GUI.Label(new Rect(5, 5, 100, 20), "before matrix");

        this.rotation += 15f * Time.deltaTime;
        var scale = Mathf.Clamp((float)Math.Sin(Time.time) + 1 * 2, 1, 3);
        GUI.matrix = Matrix4x4.TRS(new Vector3(Screen.width / 2, Screen.height / 2, 0), Quaternion.Euler(0, 0, this.rotation), Vector3.one * scale);
        var size = GUI.skin.label.CalcSize(new GUIContent("test string"));
        var rect = new Rect((-size.x / 2f), (-size.y / 2f), size.x, size.y);
        GUI.Label(rect, "test string");
        GUI.matrix = matrix;
        GUI.Label(new Rect(5, 25, 100, 20), "after matrix");
    }
}
