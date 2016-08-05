using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PushAway : MonoBehaviour {
	
	// Public vars
	public float Force = 1000.0f;
	
	// Private vars
	List<GameObject> enteredObjects = new List<GameObject>();
	
	// Use this for initialization
	void Start () {
	
	}
	
	public const float PushAwayMax = 10.0f;
	public const float PushAwayConstant = 1.0f;
	public const float PushAwayLinear = 0.0f;
	public const float PushAwayQuadratic = 0.01f;
	
	// Update is called once per frame
	void FixedUpdate () {
		for(int i = 0; i < enteredObjects.Count; ++i) {
			if (enteredObjects[i] == null) {
				enteredObjects.RemoveAt(i);
				i--;
				continue;
			}
			Vector3 diff = enteredObjects[i].transform.position - transform.position;
			float dist = diff.magnitude;
			float force = Force / (PushAwayConstant + PushAwayLinear * dist + PushAwayQuadratic * dist * dist);

			enteredObjects[i].GetComponent<Rigidbody>().AddForce(diff.normalized * force * Time.fixedDeltaTime);
		}
	}
	
	void OnTriggerEnter(Collider _collider) {
		if(_collider.GetComponent<Rigidbody>() != null)
			enteredObjects.Add(_collider.gameObject);
	}
	void OnTriggerExit(Collider _collider) {
		enteredObjects.Remove(_collider.gameObject);	
	}
}
