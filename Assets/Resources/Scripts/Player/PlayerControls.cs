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
			if (Input.touchCount >= 2 || Input.GetMouseButton(1)) {
				GetComponent<PlayerMovement>().Boost(AnalogueStick.Val);
			} else {
				GetComponent<PlayerMovement>().Move(AnalogueStick.Val);
			}

			// Enable the trail and push it in that direction.
			if (trail != null) {
				trail.Enabled = true;
			}
		} else {
			if (trail != null)
				trail.Enabled = false;
		}
	}
}
