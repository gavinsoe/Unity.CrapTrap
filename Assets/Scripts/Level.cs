using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class Level : MonoBehaviour {

    [XmlAttribute("number")]
    public int number;

    public bool unlocked;
    public int stars;
    public string sceneName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
