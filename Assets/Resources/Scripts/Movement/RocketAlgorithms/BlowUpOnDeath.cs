using UnityEngine;
using System.Collections;

public class BlowUpOnDeath : MonoBehaviour {
	
	// Public vars
	public GameObject Explosion;
	public GameObject Shockwave;
	public float SpawnImmunity = 1.0f;
	public bool DestroyOurself = true;
	
	// Private vars
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		SpawnImmunity -= Time.deltaTime;
	}
	
	void Die() {
		// If we are immune, don't die.
		if (SpawnImmunity > 0)
			return;
		
		// Kill ourselves
		if (DestroyOurself)
			Destroy(gameObject);
		
		// Create an explosion
		if (Explosion != null) {
			GameObject explosion = (GameObject)Instantiate(Explosion, transform.position, Quaternion.identity);
		}
		if (Shockwave != null) {
			GameObject shockwave = (GameObject)Instantiate(Shockwave, transform.position, Quaternion.identity);
		}
	}
}
