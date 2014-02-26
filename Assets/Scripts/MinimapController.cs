using UnityEngine;
using System.Collections;

public class MinimapController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.camera.cullingMask = (1 << LayerMask.NameToLayer("Terrain"))|(1 << LayerMask.NameToLayer("Character"));
	}
}
