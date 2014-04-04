using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("Game")]
public class Game : MonoBehaviour {

    [XmlArray("Achievements")]
    [XmlArrayItem("Achievement")]
    public List<Achievement> achievements = new List<Achievement>();

    [XmlArray("Chapters")]
    [XmlArrayItem("Chapter")]
    public List<Chapter> chapters = new List<Chapter>();

    public int moves;
    public int hangingMoves;
    public int time;
    public int climbs;
    public int pulls;
    public int pushes;
    public int toiletPapers;
    public int goldPapers;
    public int stagesDone;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
