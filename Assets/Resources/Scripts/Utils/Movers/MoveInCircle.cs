using UnityEngine;
using System.Collections;

public class MoveInCircle : MonoBehaviour {
	
	// Public vars
	public float Radius = 10.0f;
	public float Speed = 1.0f;
	
	// Private vars
	float timer = 0.0f;
	Vector3 position;
	
	// Use this for initialization
	void Start () {
		position = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		// Increase the timer.
		timer += Time.deltaTime * Speed;
		
		// Calculate where on the circle we will be.
		float xOffset = Radius * Mathf.Cos(timer);
		float zOffset = Radius * Mathf.Sin(timer);
		transform.localPosition = position + new Vector3(xOffset, zOffset, 0);
	}
}
