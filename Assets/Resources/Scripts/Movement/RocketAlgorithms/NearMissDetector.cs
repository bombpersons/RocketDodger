using UnityEngine;
using System.Collections;

public class NearMissDetector : MonoBehaviour {
	
	// Public vars
	public const float DETECTIONFUZZ = 5f;
	
	// Private vars
	private bool enteredRange = false;
	private float lastDistance = 0.0f;
	private float minDistance = float.MaxValue;
	private GameObject player = null;
	private bool scored = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Grab the player.
		if (player == null)
			player = GameObject.FindGameObjectWithTag("Player");
		
		// Check the distance to the player.
		Vector3 diff = player.transform.position - transform.position;
		float distance = diff.magnitude;
		
		// Calculate the delta distance.
		float delta = distance - lastDistance;
		
		// If the distance is less than the minimum near miss distance..
		if (distance < Score.NearMissRange) {
			enteredRange = true;
		}
		
		// If we are in range
		if (enteredRange && !scored) {
			if (distance < minDistance) {
				minDistance = distance;	
			}
			
			// If we are moving away.
			if (delta > 0 && (GetComponent<Rigidbody>().velocity - player.GetComponent<Rigidbody>().velocity).magnitude > DETECTIONFUZZ) {
				Score.NearMiss(distance, (player.transform.position + transform.position) / 2);
				minDistance = float.MaxValue;
				scored = true;
				enteredRange = false;
			}
		}
		
		if (distance > Score.NearMissRange) {
			scored = false;	
		}
		
		// Update the last distance.
		lastDistance = distance;
	}
}
