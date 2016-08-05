using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	
	// Public vars
	public float Acc;
	public float MaxSpeed;
	public float Force = 100.0f;
	
	public const float BOOST = 3.0f;
	public const float BOOSTCOOLDOWN = 3.0f;
	public const float BOOSTSCOREUSE = 1000.0f;
	
	// Private vars
	float boostCooldown = 0.0f;
	bool boosting = false;
	float boostUsed = 0.0f;
	
//	float lastToTurn = 0;		
	Vector3 oldLOS = Vector3.zero;
//	float distance = 0;	
	
	public void StartBoost() {
		if (Score.ScoreNum > 0) {
			boosting = true;
		}
	}
	public void StopBoost() {
		boosting = false;
	}
	
	// Use proportional navigation to move to a point.
	public void PropMoveTo(Vector3 _point, float _force) {

		// Proportional navigation.
		Vector3 LOS = _point - transform.position;
		Vector3 LOSNormal = new Vector3(LOS.z, 0, -LOS.x);
		float NC = 20;
		
		// Calculate the LOSRate
		Vector3 LOSRate = LOS - oldLOS;
		oldLOS = LOS;
		
		// Calculate the APN bias
		Vector3 APNBias = LOSRate * (NC/2.0f);
			
		//float APNBias = (NC / 2.0f) * LOS;
		Vector3 Acc = LOS + (LOSRate * NC);// + APNBias;
		
		// Move in that direction.
		if (boosting) {
			GetComponent<Rigidbody>().AddForce(Acc.normalized * BOOST * _force * Time.deltaTime);			
		} else {
			GetComponent<Rigidbody>().AddForce(Acc.normalized * _force * Time.deltaTime);
		}
		
		// Give the trail an update on whether or not we are boosting.
		if (GetComponent<AutoSpawnSmokeTrail>().Trail != null)
			GetComponent<AutoSpawnSmokeTrail>().Trail.GetComponent<SmokeTrail>().BoostEnabled = boosting;
	}
	public void PropMoveTo(Vector3 _point) {
		PropMoveTo(_point, Force);
	}
	
	// Move to a point.
	public void MoveTo(Vector3 _point) {
		Vector3 diff = _point - transform.position;
		diff.y = 0;
		diff.Normalize();
			
		// Add a force.
		GetComponent<Rigidbody>().AddForce((diff).normalized * Force * Time.deltaTime);
		//rigidbody.AddForce((diff).normalized * Force * Time.deltaTime);
	}
	
	public void Boost(Vector3 _dir) {
		if (Score.ScoreNum > 0) {
			// Move
			PropMoveTo(_dir, Force * BOOST);
			
			// Reset the cooldown
			boostCooldown = BOOSTCOOLDOWN;
			
			// Reduce score
			Score.ScoreNum -= BOOSTSCOREUSE * Time.deltaTime;
		}
	}
	
	public void Update() {
		boostCooldown -= Time.deltaTime;	
		
		// If we are boosting reduce score
		if (boosting) {
			// Reduce score
			Score.ScoreNum -= BOOSTSCOREUSE * Time.deltaTime;
			boostUsed += BOOSTSCOREUSE * Time.deltaTime;
			if (boostUsed >= Score.MaxScore / 4 || Score.ScoreNum < 0) {
				boostUsed = 0.0f;
				boosting = false;	
			}
		}
	}
}
