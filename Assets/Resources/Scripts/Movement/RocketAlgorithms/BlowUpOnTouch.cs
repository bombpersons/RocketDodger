using UnityEngine;
using System.Collections;

public class BlowUpOnTouch : MonoBehaviour {
	
	// Public vars
	
	// Private vars
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision _col) {
		_col.gameObject.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
		gameObject.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
	}
	
	void OnTriggerEnter(Collider _collider) {
		_collider.gameObject.SendMessage("Die", SendMessageOptions.DontRequireReceiver);	
	}
}
