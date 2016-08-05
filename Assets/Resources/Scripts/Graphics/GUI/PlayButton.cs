using UnityEngine;
using System.Collections;

public class PlayButton : MonoBehaviour {
	
	// Public vars
	public GUISkin Skin;
	public bool Enabled = true;
	
	// Use this for initialization
	void Start () {
		if (!Enabled)
			return;
		
		// Kill the player to begin with.
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		player.GetComponent<PlayerDie>().IsDead = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!Enabled) {
			if (GetComponent<DeathTimer>() == null) {
				gameObject.AddComponent<DeathTimer>();
				GetComponent<DeathTimer>().Timer = 1.0f;
				GetComponent<DeathTimer>().Fade = true;
			}
		}
	}
	
	void OnGUI() {
		if (!Enabled)
			return;
		
		// Set the skin.
		GUI.skin = Skin;
		
		// Scale the GUI stuff so that it stretches across the screen.
		float xRatio = Screen.width / 800.0f;
		float yRatio = Screen.height / 600.0f;
		GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(xRatio, yRatio, 1));				
		
		// Draw the play button
		GUIStyle centered = GUI.skin.GetStyle("Label");
		centered.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(0, 350, 800, 300), "Tap to start!", centered);
		
		// Detect tap
		if (Input.GetMouseButtonDown(0)) {
			ResetManager.Reset();	
		}
	}
	
	void RestartLevel() {
		Enabled = false;
		//renderer.enabled = false;
	}
}
