using UnityEngine;
using System.Collections;

public class DeathTimer : MonoBehaviour {
	
	// Public vars
	public float Timer;
	public bool Fade = true;
	public float Randomness = 1.0f;
	public bool DontSendDieMessage = false;
	public bool Glow = false;
	
	const float RING_LIFETIME = 2.0f;
	const float RING_SCALE = 0.3f;
	const float RING_OFFSET = 0.2f;
	
	// Private vars
	private float timer;
	private GameObject glow;
	
	// Use this for initialization
	void Start () {
		Reset();
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0) {
			// Die
			if (DontSendDieMessage) {
				Destroy(this.gameObject);	
			} else {
				SendMessage("Die", SendMessageOptions.DontRequireReceiver);
			}
		}
		
		// Fade the alpha
		if (Fade) {
			if (GetComponent<GUIText>() != null) {
				GetComponent<GUIText>().material.color = Color.white * (timer / Timer);
			}
			if (GetComponent<Renderer>() != null) {
				GetComponent<Renderer>().material.color = Color.white * (timer / Timer);
			}
		}
		
		// Glow
		if (Glow) {
			// Create the glow prefab if it doesn't exist.
			if (glow == null) {
				// Find the prefab.
				GameObject glowPrefab = (GameObject)Resources.Load("Prefabs/Effects/Glow", typeof(GameObject));
				glow = (GameObject)(Instantiate(glowPrefab, transform.position - new Vector3(0, 1, 0), Quaternion.identity));
				glow.GetComponent<Glow>().Parent = this.gameObject;
			}
			
			float g = timer / RING_LIFETIME;	
			g += RING_OFFSET;
			g = Mathf.Clamp01(g);
			
			float scale = g * RING_SCALE;
				
			// Set the color
			glow.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1-g);
			glow.transform.localScale = new Vector3(scale, 1, scale);
			glow.transform.position = transform.position - new Vector3(0, 1, 0);
		}
	}
	
	public void Reset() {
		timer = Timer + Random.Range(-Randomness, Randomness);
	}
}
