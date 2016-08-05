using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	// Public vars
	public float TurnForce = 10;
	public float ForwardForce = 100;
	
	// Private vars
	float lastToTurn = 0;
	
	// Turn to a direction
	public void TurnTo(Vector3 _dir) {
		_dir.Normalize();
		_dir.y = transform.position.y;
		float dist = -Mathf.DeltaAngle(Quaternion.LookRotation(_dir).eulerAngles.y, transform.eulerAngles.y);	
		
		float maxAcc = TurnForce / (GetComponent<Rigidbody>().mass * 10f);
		
		maxAcc = Mathf.Min(maxAcc, Mathf.Abs(dist));
		
		float targetVel;
		if (dist>0)
			targetVel = Mathf.Sqrt(2.0f * maxAcc * dist);		
		else
			targetVel = -Mathf.Sqrt(2.0f * maxAcc * -dist);	
		
		float thrust = targetVel - GetComponent<Rigidbody>().angularVelocity.y;	
		
		float force = thrust * GetComponent<Rigidbody>().mass * 10f;
		
		// Turn the rocket.
		Turn(force);
	}
	
	public void Turn(float _val) {
		_val = Mathf.Clamp(_val, -TurnForce, TurnForce);
		
		// Turn the rocket.
		GetComponent<Rigidbody>().AddTorque(new Vector3(0, _val, 0));
	}
	
	public void Thrust() {
		GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward) * ForwardForce * Time.deltaTime);
	}
}
