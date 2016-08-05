using UnityEngine;
using System.Collections;

public class AnalogueStick : MonoBehaviour {
	
	// Public vars
	public Texture Tex;
	public Texture InnerTex;
	public Vector2 Position = Vector2.zero;
	public float Radius;
	public float Sensitivity = 1;
	
	public static Vector3 Val = Vector3.zero;
	
	// Private vars
	private int touchId = -1;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		// If we are on pc
//#if UNITY_EDITOR
		// Check for a button press
		if (Input.GetMouseButtonDown(0)) {
			// Change the position of the pad to here.
			Vector3 mousePos = Input.mousePosition;
			Position = new Vector2(mousePos.x, Screen.height - mousePos.y);
		}
		
		// Check for mouse presses.
		if (Input.GetMouseButton(0)) {
			Vector3 mouse = Input.mousePosition;
			mouse.y = Screen.height - mouse.y;
			mouse.z = mouse.y;
			mouse.y = 0;
			
			// Get the distance from the stick.
			Vector3 diff = mouse - new Vector3(Position.x, 100, Position.y);
			diff.z *= -1;
			float distance = diff.magnitude;
			float val = (distance / Radius) * Sensitivity;
			Val = diff.normalized * val;
		} else {
			Val = Vector3.zero;
		}
//#else	
//		// Touch screen controls.
//		// Check for touches.
//		if (Input.touchCount > 0) {
//			
//			// Loop through events
//			for (int i = 0; i < Input.touchCount; ++i) {
//				Touch touch = Input.GetTouch(i);
//				
//				if (touchId == -1 && touch.phase == TouchPhase.Began) {
//					// Set the id.
//					touchId = touch.fingerId;			
//					
//					// Change the position of the pad to here.
//					Vector3 mousePos = touch.position;
//					Position = new Vector2(mousePos.x, Screen.height - mousePos.y);
//				}
//				
//				// Check for mouse presses.
//				if (touch.fingerId == touchId) {
//					if (touch.phase == TouchPhase.Moved) {
//						Vector3 mouse = touch.position;
//						mouse.y = Screen.height - mouse.y;
//						mouse.z = mouse.y;
//						mouse.y = 0;
//						
//						// Get the distance from the stick.
//						Vector3 diff = mouse - new Vector3(Position.x, 100, Position.y);
//						diff.z *= -1;
//						float distance = diff.magnitude;
//						float val = (distance / Radius) * Sensitivity;
//						Val = diff.normalized * val;
//					} else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended) {
//						Val = Vector2.zero;	
//						touchId = -1;
//					}
//				}			
//			}
//		}
//#endif
		
		// Make sure Val is within limits.
		Vector2 limits = new Vector2(Val.x, Val.z);
		if (limits.magnitude >= 1) {
			limits.Normalize();
			Val = new Vector3(limits.x, Val.y, limits.y);
		}
	}
	
	// Draw
	void OnGUI() {
		// If the player is dead then don't draw.
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if (player == null)
			return;
		if (player.GetComponent<PlayerDie>().IsDead)
			return;
		
		if (Input.GetMouseButton(0)) {
			// Draw the radius.
			GUI.DrawTexture(new Rect(Position.x - (Tex.width/2), Position.y - (Tex.height/2), Tex.width, Tex.height), Tex);
			
			// Draw a circle indicating in which direction the analogue stick is.
			GUI.DrawTexture(new Rect((Position.x + Val.x * Tex.width * 0.5f) - (InnerTex.width/2), (Position.y - Val.z * Tex.height * 0.5f) - (InnerTex.height/2), InnerTex.width, InnerTex.height), InnerTex);
		}
	}
}
