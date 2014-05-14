using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("Game")]
public class Game : MonoBehaviour {

    [XmlArray("Achievements")]
    [XmlArrayItem("Achievement")]
    public List<Achievement> achievements = new List<Achievement>();

    [XmlArray("Chapters")]
    [XmlArrayItem("Chapter")]
    public List<Chapter> chapters = new List<Chapter>();

    public Dictionary<int, int> levels;
    public Dictionary<int, bool> lockedLevels;

    public int moves;
    public int hangingMoves;
    public float time;
    public int climbs;
    public int pulls;
    public int pushes;
    public int slides;
    public int pullOuts;
    public int toiletPapers;
    public int goldPapers;
    public int stagesDone;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(Game));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static Game Load(string path) {
        var serializer = new XmlSerializer(typeof(Game));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as Game;
        }
    }
}
