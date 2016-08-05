using UnityEngine;
using System.Collections;

public class ScoreText : MonoBehaviour {
	
	// Public vars
	public float BounceMultiplier = 0.001f;
	
	// Private vars
	private float lastScore;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Get the delta score
		float deltaScore = Score.ScoreNum - lastScore;
		lastScore = Score.ScoreNum;
		
		// Set the text.
		GetComponent<GUIText>().text = "Score: " + ((int)Score.ScoreNum).ToString();
		GetComponent<GUIText>().GetComponent<BounceSizer>().Bounce(deltaScore * BounceMultiplier);
	}
}
