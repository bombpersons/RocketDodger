using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {
	
	// Public vars
	public static bool RetryScreenEnabled = false;
	
	// GUI Skin
	public GUISkin Skin;
	
	// Private vars
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	// Draw the GUI
	void OnGUI() {
		// Load the skin
		GUI.skin = Skin;
		
		// Scale the GUI stuff so that it stretches across the screen.
		float xRatio = Screen.width / 800.0f;
		float yRatio = Screen.height / 600.0f;
		GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(xRatio, yRatio, 1));		
		
		// If the retry screen is enabled
		if (RetryScreenEnabled) {
			// Draw the stuff.
			if (GUI.Button(new Rect(300, 350, 200, 200), "Retry")) {
				ResetManager.Reset();
			}
			
			// Draw the wave we got to at the top.
			GUIStyle centered = GUI.skin.GetStyle("Label");
			centered.alignment = TextAnchor.MiddleCenter;
			GUI.Label(new Rect(0, 50, 800, 100), "You made it to Wave " + WaveManager.WaveNum.ToString(), centered);
			
			// Tell them what the furthest they ever got was
//			string tense = "";
//			if (WaveManager.WaveNum > WaveManager.GetPreviousFurthestWave()) // Make sure the tenses make sense.
//				tense = "was";
//			else
//				tense = "is";
			GUI.Label(new Rect(0, 150, 800, 200), "Your best wave: " + WaveManager.GetFurthestWave().ToString(), centered);
		}
	}
	
	// RestartLevel
	public void RestartLevel() {
		RetryScreenEnabled = false;	
	}
}
