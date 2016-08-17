using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	// Public vars
	public float MaxSpeed;
	public float Force = 100.0f;

	public const float BOOST = 3.0f;
	public const float BOOSTSCOREUSE = 1000.0f;

	// Private vars
	bool boosting = false;

	// Move to a point.
	public void Accelerate(Vector3 _dir, float _force) {
		Vector3 diff = _dir;
		diff.y = 0;
		diff.Normalize();

		// Add a force.
		Debug.Log(GetComponent<Rigidbody>().velocity.magnitude);
		if (GetComponent<Rigidbody>().velocity.magnitude < MaxSpeed)
			GetComponent<Rigidbody>().AddForce(diff * _force * Time.deltaTime);
	}

	// Use proportional navigation to move to a point.
	Vector3 oldLOS = Vector3.zero;
	public void PropMoveTo(Vector3 _dir, float _force) {

		// Proportional navigation.
		Vector3 LOS = _dir;
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
		GetComponent<Rigidbody>().AddForce(Acc.normalized * _force * Time.deltaTime);
	}
	public void PropMoveTo(Vector3 _point) {
		PropMoveTo(_point, Force);
	}


	// Shunt in a direction
	public void Shunt(Vector3 _dir) {

	}

	public void Boost(Vector3 _loc) {
		// Only boost if we can.
		bool canBoost = Score.ScoreNum > 0;

		// Give the trail an update on whether or not we are boosting.
		if (GetComponent<AutoSpawnSmokeTrail>().Trail != null)
			GetComponent<AutoSpawnSmokeTrail>().Trail.GetComponent<SmokeTrail>().BoostEnabled = canBoost;

		if (canBoost) {
			// Move
			PropMoveTo(_loc, Force * BOOST);

			// Reduce score
			Score.ScoreNum -= BOOSTSCOREUSE * Time.deltaTime;
		}
	}

	public void Move(Vector3 _loc) {
		// Move without boost.
		//PropMoveTo(_loc, Force);
		Accelerate(_loc, Force);

		// Give the trail an update on whether or not we are boosting.
		if (GetComponent<AutoSpawnSmokeTrail>().Trail != null)
			GetComponent<AutoSpawnSmokeTrail>().Trail.GetComponent<SmokeTrail>().BoostEnabled = false;
	}

	public void Update() {
	}
}
