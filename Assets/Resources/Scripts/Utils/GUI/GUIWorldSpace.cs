using UnityEngine;
using System.Collections;

public class GUIWorldSpace : MonoBehaviour {
	
	// Public vars
	public Vector3 Position;
	
	// Private vars
	
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// Get screen position
		Vector3 screenPos = Camera.main.WorldToScreenPoint(Position);
		
		// Put the GUIText into position
		transform.position = new Vector3(screenPos.x / Screen.width, screenPos.y / Screen.height, 0);
	}
}
