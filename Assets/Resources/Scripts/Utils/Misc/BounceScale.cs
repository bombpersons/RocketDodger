using UnityEngine;
using System.Collections;

public class BounceScale : MonoBehaviour {
	
	Vector3 originalSize;
	public float duration;
	public float scale;
	
	float startTime;
	
	// Use this for initialization
	void Start () {
		startTime = Time.time;
		originalSize = transform.localScale;
		
		// Call this here so that the first frame that the text is drawn,
		// it is draw with 
		CalculateSize();
	}
	
	public void Reset(){
		Start();
	}
	
	// Update is called once per frame
	void Update () {
		CalculateSize();
	}
	
	// Calculate size
	void CalculateSize() {
		float x = (Time.time -startTime)/duration;
		x = Mathf.Clamp01(x);
		
		x = Mathf.Min(1, 16*Mathf.Pow(x,3)*(12*Mathf.Pow(x,2)-15*x+5))-Mathf.Max(0, Mathf.Pow((2*x-1),3)*(24*Mathf.Pow(x,2)-54*x+31));
				
		transform.localScale = originalSize * x * scale;
	}
}
