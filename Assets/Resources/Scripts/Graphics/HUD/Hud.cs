using UnityEngine;
using System.Collections;

public class Hud : MonoBehaviour {
	
	float lowestFps = float.MaxValue;
	float adverageFps;
	float backLowestFps = float.MaxValue;
	float timer =0;
	const float FPS_ADVERAGE_RATE = 0.1f;
	
	void Start()
	{
		adverageFps = 1/Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (backLowestFps > 1/Time.deltaTime)
			backLowestFps = 1/Time.deltaTime;
		
		adverageFps = Mathf.Lerp(adverageFps, 1/Time.deltaTime, FPS_ADVERAGE_RATE);
		
		if(Time.time > timer){
			lowestFps = backLowestFps;
			backLowestFps = float.MaxValue;
			timer = Time.time + 1;
		}
	
	}
	
	// Draw the hud
	void OnGUI() {
		//GUI.Label(new Rect(0, 0, 100, 50), Score.ElapsedTime.ToString());	
		//GUI.Label(new Rect(0, 0, 100, 50), lowestFps.ToString("F1") + "	" + adverageFps.ToString("F1") + "	" + (1/Time.deltaTime).ToString("F1"));	
	}
}
