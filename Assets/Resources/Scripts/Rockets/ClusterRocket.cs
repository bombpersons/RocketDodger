using UnityEngine;
using System.Collections;

public class ClusterRocket : MonoBehaviour {
	
	// Public vars
	public int SmallerRocketNum = 5;
	public GameObject SmallerRocketPrefab = null;
	public float DistanceTrigger = 2.0f;
	public float BreakOffForce = 100.0f;
	public float spread = 180f;
	
	// Private vars
	GameObject player;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// Grab the player
		if (player == null)
			player = (GameObject)GameObject.FindGameObjectWithTag("Player");
		
		// Check if we are in range of the player
		Vector3 diff = player.transform.position - transform.position;
		float dist = diff.magnitude;
		if (dist < DistanceTrigger) {
			SpawnSmallRockets();
			Destroy(this.gameObject);
		}
	}
	
	// Spawn the smaller rockets.
	void SpawnSmallRockets() {
		float spreadPerRocket = spread/(SmallerRocketNum-1);
		float spreadCenter = spread/2f;
		for (int i = 0; i < SmallerRocketNum; ++i) {

			//Quaternion angle = Quaternion.LookRotation(rigidbody.velocity) * Quaternion.Euler(new Vector3(0, (spreadPerRocket * i) - spreadCenter));
			Quaternion angle = GetComponent<Rigidbody>().transform.rotation * Quaternion.Euler(new Vector3(0, (spreadPerRocket * i) - spreadCenter));
			// Spawn the rockets.
			GameObject obj = (GameObject)Instantiate(SmallerRocketPrefab, transform.position, angle);
			
			// Force them in the direction they are facing.
			obj.GetComponent<Rigidbody>().AddForce(obj.transform.forward * BreakOffForce);
		}
	}
}
