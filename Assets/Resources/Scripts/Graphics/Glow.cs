using UnityEngine;
using System.Collections;

public class Glow : MonoBehaviour {
	
	// Public
	const float LIFETIME = 5.0f;
	public GameObject Parent;
	public float FadeTime;
	
	// Private
	private float timer;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Parent == null) {
			Destroy(this.gameObject);
		}
	}
}
