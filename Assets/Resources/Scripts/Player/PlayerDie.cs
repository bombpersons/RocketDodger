using UnityEngine;
using System.Collections;

public class PlayerDie : MonoBehaviour {
	// Public vars
	public bool IsDead = false;
//	public GameObject Explosion;
//	public GameObject Shockwave;
	public float RandomExplosionTimer = 3.0f;
	
	// Private vars
	private Color color;
	private float timer;
	
	// Use this for initialization
	void Start () {
		color = GetComponent<Renderer>().material.color;
	}

	void FixedUpdate() {
//		if (IsDead) {
//			timer += Time.deltaTime;
//			
//			// Cause random explosions for a while
//			if (timer < RandomExplosionTimer) {
//				if (Random.Range(0, 100) > 98) {
//					// Create an explosion
//					if (Explosion != null) {
//						GameObject explosion = (GameObject)Instantiate(Explosion, transform.position, Quaternion.identity);
//					}
//					if (Shockwave != null) {
//						GameObject shockwave = (GameObject)Instantiate(Shockwave, transform.position, Quaternion.identity);
//					}
//				}
//			}
//		} else {
//			timer = 0;
//		}
	}

	void Die() {
		// If we are already dead don't do anything.
		if (IsDead)
			return;
		
		// Set the player to be burnt up.
		GetComponent<Renderer>().material.color = Color.black;
		
		// Show the retry button.
		GUIManager.RetryScreenEnabled = true;
		
		// We are dead
		IsDead = true;
		
		// Save our score
		WaveManager.SaveScore();
	}
	
	// Restart Level
	public void RestartLevel() {
		// Set the color back.
		GetComponent<Renderer>().material.color = color;
		
		// Reset the position
		transform.position = new Vector3(0, 0, 0);	
		GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
		
		// Not dead anymore
		IsDead = false;
	}
}
