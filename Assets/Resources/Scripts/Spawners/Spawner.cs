using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	
	// Public vars
	public GameObject ObjectToSpawn;
	public float MinTime;
	public float MaxTime;

	public float MinMinTime;
	public float MinMaxTime;
	
	public float TimeTillMaxDifficulty;
	public float TillNextSpawn;
	
	// Private vars
	private float timer;
	
	// Use this for initialization
	void Start () {
		if (TillNextSpawn == 0)
			TillNextSpawn = Random.Range(Mathf.Lerp(MinTime, MinMinTime, timer / TimeTillMaxDifficulty), Mathf.Lerp(MaxTime, MinMaxTime, timer / TimeTillMaxDifficulty));
	}
	
	// Update is called once per frame
	void Update () {
		TillNextSpawn -= Time.deltaTime;
		timer += Time.deltaTime;
		if (timer > TimeTillMaxDifficulty)
			timer = TimeTillMaxDifficulty;
		
		if (TillNextSpawn < 0) {
			// Spawn
			Vector3 pos = Camera.main.transform.position + new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized * Camera.main.orthographicSize * 2;
			pos.y = 0;
			GameObject rocket = (GameObject)Instantiate(ObjectToSpawn, pos, Quaternion.identity);
			rocket.GetComponent<Rigidbody>().velocity = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity;
			
			// Pick the next tillNextSpawn time.
			TillNextSpawn = Random.Range(Mathf.Lerp(MinTime, MinMinTime, timer / TimeTillMaxDifficulty), Mathf.Lerp(MaxTime, MinMaxTime, timer / TimeTillMaxDifficulty));
		}
	}
}
