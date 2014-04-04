using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

public class Chapter : MonoBehaviour {

    [XmlArray("Levels")]
    [XmlArrayItem("Level")]
    public List<Level> levels = new List<Level>();

    [XmlAttribute("number")]
    public int number;

    public bool unlocked;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
