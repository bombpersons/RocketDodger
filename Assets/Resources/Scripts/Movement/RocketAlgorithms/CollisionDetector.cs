using UnityEngine;
using System.Collections;

public class CollisionDetector : MonoBehaviour {
	
	// Public vars
	
	// Private vars
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Detect any collision with another rocket.
	void OnCollisionEnter(Collision _col) {
		if (_col.gameObject.GetComponent<CollisionDetector>() != null) {
			if (_col.gameObject.GetComponent<BlowUpOnDeath>().SpawnImmunity < 0) {
				Score.Collision(_col.contacts[0].point);
			}
		}
	}
}
