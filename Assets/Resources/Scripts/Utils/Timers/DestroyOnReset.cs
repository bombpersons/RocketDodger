using UnityEngine;
using System.Collections;

public class DestroyOnReset : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// RestartLevel
	public void RestartLevel() {
		Destroy(this.gameObject);	
	}
}
