using UnityEngine;
using System.Collections;

public static class ResetManager {
	
	// Public vars
	
	// Private vars
	
	public static void Reset() {
		// Send a reset message to all objects.
		GameObject[] objects = (GameObject[])Object.FindObjectsOfType(typeof(GameObject));
		
		// Loop through all of these and send them a reset message.
		foreach(GameObject obj in objects) {
			obj.SendMessage("RestartLevel", SendMessageOptions.DontRequireReceiver);	
		}
	}
}
