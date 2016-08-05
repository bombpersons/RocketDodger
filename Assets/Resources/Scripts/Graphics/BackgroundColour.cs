using UnityEngine;
using System.Collections;

public class BackgroundColour : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 camPos = Camera.main.transform.position;
		
		GetComponent<Renderer>().material.color = Color.white * (1-(Mathf.Max(Mathf.Abs(camPos.x), Mathf.Abs(camPos.z))*0.01f - 0.1f));
	
	}
}
