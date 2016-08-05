using UnityEngine;
using System.Collections;

public class CloseTailDetector : MonoBehaviour {
	
	// Public vars
	public static GameObject textPrefab;
	public const float ANGLEFUZZ = 45.0f;
	
	// Private vars
	private GameObject player;
	private GameObject text;
	private float score;
	
	// Use this for initialization
	void Start () {
		// Grab the prefab
		textPrefab = (GameObject)Resources.Load("Prefabs/Text/TrickText", typeof(GameObject));
	}
	
	// Update is called once per frame
	void Update () {
		// Grab the player.
		if (player == null)
			player = (GameObject)GameObject.FindGameObjectWithTag("Player");
		
		// Check if the rocket is within tailing range.
		Vector3 diff = player.transform.position - transform.position;
		float distance = diff.magnitude;
		
		// Get the angle difference to make sure we are pointing at the player.
		float angle = Vector3.Angle(GetComponent<Rigidbody>().velocity.normalized, diff);
		
		if (distance < Score.TailingRange && angle < ANGLEFUZZ && (GetComponent<Rigidbody>().velocity - player.GetComponent<Rigidbody>().velocity).magnitude < 5f) {
			if (text == null) {
				text = (GameObject)Instantiate(textPrefab);
			}
			
			// Inc score
			score += (1 + (score / 30)) * Score.TailingScoreRate * Time.deltaTime;
			
			// Make sure the text doesn't get destroyed for now.
			text.GetComponent<DeathTimer>().Reset();
			
			// Update the text.
			text.GetComponent<GUIText>().text = "Tailing: " + ((int)score).ToString();
			
			// Get the position on screen.
			Vector3 worldPos = (player.transform.position + transform.position) / 2;
			text.GetComponent<GUIWorldSpace>().Position = worldPos;
			text.GetComponent<BounceSizer>().Bounce(0.02f);
			//text.guiText.pixelOffset = new Vector2(Score.TrickTextScreenPos.x * Screen.width, Score.TrickTextScreenPos.y * Screen.height);
		}
		
		if (!(distance < Score.TailingRange && angle < ANGLEFUZZ) && text != null) {
			// Score!
			Score.WasTailed(score);
			
			// Stop tracking the text.
			text = null;
			score = 0;
		}
	}
	
	void Die() {
		// If we are dying, then award the points.
		if (score > 0)
			Score.WasTailed(score);
	}
}
