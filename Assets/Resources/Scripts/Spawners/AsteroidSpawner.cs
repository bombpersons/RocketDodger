using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour {
	
	// Public vars
	public GameObject AsteroidPrefab;
	public int AsteroidNum = 5;
	public float Radius = 100.0f;
	public float MinSize = 20.0f;
	public float MaxSize = 100.0f;
	
	// Private vars
	
	
	// Use this for initialization
	void Start () {
		// Create the asteroids
		for (int i = 0; i < AsteroidNum; ++i) {
			// Generate random rotation and position.
			Vector3 scale = new Vector3(Random.Range(MinSize, MaxSize), 20.0f, Random.Range(MinSize, MaxSize));
			Vector3 pos = new Vector3(Random.Range(-Radius, Radius), 1.0f, Random.Range(-Radius, Radius));
			
			// Create the object.
			GameObject ast = (GameObject)Instantiate(AsteroidPrefab, pos, Quaternion.identity);
			ast.transform.localScale = scale;
			ast.GetComponent<Rigidbody>().mass = ast.GetComponent<Rigidbody>().mass * scale.x * scale.z;
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	// RestartLevel
	public void RestartLevel() {
		Start();	
	}
}
