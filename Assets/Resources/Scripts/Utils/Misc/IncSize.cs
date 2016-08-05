using UnityEngine;
using System.Collections;

public class IncSize : MonoBehaviour {
	
	// Public vars
	public float IncRate = 1.0f;
	
	// Private vars
	
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale += transform.localScale * IncRate * Time.deltaTime;
	}
}
