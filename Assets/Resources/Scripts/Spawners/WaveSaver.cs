using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveSaver : MonoBehaviour {
	public WaveManager.Wave wave;
	
	List<GameObject> spawnedRockets = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	
	
	// Update is called once per frame
	void Update () {
		GameObject[] rockets =  GameObject.FindGameObjectsWithTag("Rocket");
		
		foreach(GameObject rocket in rockets)
		{
			if (!spawnedRockets.Contains(rocket))
			{
				spawnedRockets.Add(rocket);
				WaveManager.WaveEntry entry = new WaveManager.WaveEntry();
				entry.Type = rocket.name;
				entry.Time = Time.time;
				entry.Position = rocket.transform.position;
				wave.Entries.Add(entry);
			}
		}
		
		foreach(GameObject spawnedRocket in spawnedRockets)
		{
			bool found = false;
			foreach(GameObject rocket in rockets)
			{
				if(spawnedRocket == rocket)
				{
					found = true;
					break;
				}
			}
			if(!found)
			{
				spawnedRockets.Remove(spawnedRocket);
			}
		}
		
		
	}
}
