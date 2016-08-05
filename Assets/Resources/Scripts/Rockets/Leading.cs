using UnityEngine;
using System.Collections;

public class Leading : MonoBehaviour {
	
	// Public vars
	public Transform Follow;
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
	
	protected float AimAhead(Vector3 delta, Vector3 vr, float muzzleV)
	{
		// Quadratic equation coefficients a*t^2 + b*t + c = 0
		float a = Vector3.Dot(vr, vr) - muzzleV*muzzleV;
		float b = 2f*Vector3.Dot(vr, delta);
		float c = Vector3.Dot(delta, delta);
		
		float det = b*b - 4f*a*c;
		
		// If the determinant is negative, then there is no solution
		if(det > 0f && !float.IsInfinity(det))
		{
			return 2f*c/(Mathf.Sqrt(det) - b);
		} else 
		{
			return -1f;
		}
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Delay
		ThrustDelay -= Time.deltaTime;
		if (ThrustDelay > 0)
			return;
		Vector3 delta = Follow.position - transform.position;
		// If the follow is destroyed, then just go forward.
		if (Follow != null) 
		{
			// Leading
			
			
			Vector3 vr = Follow.GetComponent<Rigidbody>().velocity - GetComponent<Rigidbody>().velocity;
			
			// Calculate the time a bullet will collide
			// if it's possible to hit the target.
			float t = AimAhead(delta, vr, GetComponent<Rigidbody>().velocity.magnitude);
			
			// If the time is negative, then we didn't get a solution.
			if(t > 0.0f && !float.IsInfinity(t))
			{
				// Aim at the point where the target will be at the time
				// of the collision.
				Vector3 aimPoint = Follow.position + t*vr;
				//transform.LookAt(aimPoint);
				GetComponent<Movement>().TurnTo(aimPoint - transform.position);
			}
			else
			{
				//GetComponent<Movement>().TurnTo(delta);
				transform.LookAt(Follow.position);
			}
				
		}		
			
		// Go forward.
		GetComponent<Movement>().Thrust();
	}
}
