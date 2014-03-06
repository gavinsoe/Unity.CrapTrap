using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour
{

    public int toiletPaper = 0;
    public int goldenToiletPaper = 0;

    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Set the score text.
        guiText.text = "Toilet Paper : " + toiletPaper.ToString() + "  Gold TP : " + goldenToiletPaper.ToString();

    }
}
