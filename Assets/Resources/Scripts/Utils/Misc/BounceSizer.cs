using UnityEngine;
using System.Collections;

public class BounceSizer : MonoBehaviour {
	
	// Public vars
	public float ScaleDecrease = 1.0f;
	public float Bounciness = 1.0f;
	
	// Private vars
	private Vector3 originalSize = Vector3.one;
	private float scaleIncrease = 0;
	
	// Use this for initialization
	void Start () {
		originalSize = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		scaleIncrease -= scaleIncrease * ScaleDecrease * Time.deltaTime;
		transform.localScale = originalSize * (1 + scaleIncrease);
	}
	
	// Bounce the text by an amount
	public void Bounce(float _amount) {
		if (_amount < 0)
			_amount = 0;
		
		scaleIncrease += _amount * Bounciness;
	}
}
