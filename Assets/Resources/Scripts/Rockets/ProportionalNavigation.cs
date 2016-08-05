using UnityEngine;
using System.Collections;

public class ProportionalNavigation : MonoBehaviour {
	
	// Public vars
	public Transform Follow;
	public bool APN = true;
	public float ThrustDelay = 0;
	
	// Private vars
	float lastToTurn = 0;		
	Vector3 oldLOS = Vector3.zero;
	float distance = 0;	
	
	// Use this for initialization
	void Start () {
		if (Follow == null)
			Follow = ((GameObject)GameObject.FindGameObjectWithTag("Player")).transform;	
		
		oldLOS = Follow.transform.position - transform.position;
		distance = oldLOS.magnitude;		
	}
	
	// Update is called once per frame
	void Update () {
		// Delay
		ThrustDelay -= Time.deltaTime;
		if (ThrustDelay > 0)
			return;
		
		// If the follow is destroyed, then just go forward.
		if (Follow != null) {
			// Proportional navigation.
			Vector3 LOS = Follow.transform.position - transform.position;
			Vector3 LOSNormal = new Vector3(LOS.z, 0, -LOS.x);
			float NC = 20;
			
			// Calculate the LOSRate
			Vector3 LOSRate = LOS - oldLOS;
			oldLOS = LOS;
			
			// Calculate the APN bias
			Vector3 APNBias = LOSRate * (NC/2.0f);
				
			//float APNBias = (NC / 2.0f) * LOS;
			Vector3 Acc = LOS + (LOSRate * NC);
			if (APN)
				Acc += APNBias;
			
			// Slowly turn to it.
			GetComponent<Movement>().TurnTo(Acc);
			//transform.forward = Acc.normalized;
			//rigidbody.AddForce(Acc.normalized * GetComponent<Movement>().ForwardForce * Time.deltaTime);
			//Debug.Log(Acc);
		}			
			
		// Go forward.
		GetComponent<Movement>().Thrust();
	}
}
