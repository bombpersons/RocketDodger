using UnityEngine;
using System.Collections;

public class PointAt : MonoBehaviour {
	
	// Public vars
	public Transform Follow;
	
	// Private vars
	
	// Use this for initialization
	void Start () {
		// If follow is null, then pick the player.
		if (Follow == null)
			Follow = ((GameObject)GameObject.FindGameObjectWithTag("Player")).transform;
	}
	
	// Update is called once per frame
	void Update () {
		// Point at it.
		GetComponent<Movement>().TurnTo(Follow.position - transform.position);
		
		// Go forward
		GetComponent<Movement>().Thrust();
	}
}
