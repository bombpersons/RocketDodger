using UnityEngine;
using System.Collections;

public class AutoSpawnSmokeTrail : MonoBehaviour {
	
	// Public vars
	public float DispersionRate;
	public float AlphaDecay;
	public float DropRate;
	public int MaxVerts;
	public float BackSpeed;
	public Color BoostColor;
	public Color Color;
	public GameObject Trail;
	public Vector2 BackwardsDir;
	public bool AutoSetBackwardsDir;
	public float StartWidth;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Trail == null) {
			// Grab the prefab.
			GameObject smokeTrailPrefab = (GameObject)Resources.Load("Prefabs/Effects/SmokeTrail", typeof(GameObject));
			if (smokeTrailPrefab == null)
				return;
			
			if (AutoSetBackwardsDir) {
				BackwardsDir = -(new Vector2(transform.right.x, transform.right.z));
			}
			
			Trail = (GameObject)Object.Instantiate(smokeTrailPrefab, Vector3.zero, Quaternion.identity);
			Trail.transform.position = new Vector3(0, -10, 0);
			
			SmokeTrail smokeTrail = Trail.GetComponent<SmokeTrail>();
			
			smokeTrail.DispersionRate = DispersionRate;
			smokeTrail.AlphaDecay = AlphaDecay;
			smokeTrail.DropRate = DropRate;
			smokeTrail.MaxVerts = MaxVerts;
			smokeTrail.Follow = this.transform;
			smokeTrail.BackSpeed = BackSpeed;
			smokeTrail.Enabled = true;
			smokeTrail.Color = Color;
			smokeTrail.BoostColor = BoostColor;
			smokeTrail.BackwardsDir = BackwardsDir;
			smokeTrail.StartWidth = StartWidth;
			
			// Change the color of the material.
			Trail.GetComponent<Renderer>().material.SetColor("_TintColor", Color.white);	
		}
	}
}
