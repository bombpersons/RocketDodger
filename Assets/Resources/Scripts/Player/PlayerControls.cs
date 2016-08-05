using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {
	
	// Public vars
	public float DoubleClickThreshold = 0.2f;
	
	// Private vars
	float lastClick = 9999;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// If we are dead then don't do anything.
		if (GetComponent<PlayerDie>().IsDead) {
			// Turn off the trail
			if (GetComponent<AutoSpawnSmokeTrail>().Trail != null) {
				GetComponent<AutoSpawnSmokeTrail>().Trail.GetComponent<SmokeTrail>().Enabled = false;
			}			
			
			return;
		}
		
		// Inc last click
		lastClick += Time.deltaTime;
		
		// Find the world position of the mouse pointer.
		Vector3 world = transform.position + AnalogueStick.Val;
		
		// Get the smoke trail and point it.
		SmokeTrail trail = null;
		if (GetComponent<AutoSpawnSmokeTrail>().Trail != null) {
			trail = GetComponent<AutoSpawnSmokeTrail>().Trail.GetComponent<SmokeTrail>();
			Vector3 outDir = AnalogueStick.Val;
			trail.OutDir = new Vector2(outDir.x, outDir.z).normalized;
		}
		
		// Control the player 
		if (Input.GetMouseButton(0)) {
			// Move to that point.
			GetComponent<PlayerMovement>().PropMoveTo(world);
			
			// Enable the trail and push it in that direction.
			if (trail != null) {
				trail.Enabled = true;
			}
		} else {
			if (trail != null)
				trail.Enabled = false;
		}
		
		// Check for double clicks
		if (Input.GetMouseButtonDown(0) && lastClick > DoubleClickThreshold) {
			lastClick = 0;
		} else if (Input.GetMouseButtonDown(0) && lastClick < DoubleClickThreshold) {
			GetComponent<PlayerMovement>().StartBoost();
		}
		
		if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(1)) {
			GetComponent<PlayerMovement>().StartBoost();	
		}
		
		if (Input.touchCount >= 2) {
			GetComponent<PlayerMovement>().StartBoost();	
		}
	}
}
