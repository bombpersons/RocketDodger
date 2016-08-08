using UnityEngine;
using System.Collections;

public class SoftFollow : MonoBehaviour {
	
	// Public vars
	public Transform target;
	public Vector3 mapSize = new Vector3(10000, 0, 10000);
	
	const float FOLLOW_MAX_DIST = 9962.00f;
	
	Vector3 restartLerpStartPos;
	
	const float RESTART_LERP_DURATION = 2.0f;
	float restartLerpStartTime;
	
	// Use this for initialization
	void Start () {
		if(target == null)
			target = GameObject.FindGameObjectWithTag("Player").transform;
		restartLerpStartPos = transform.position;
		restartLerpStartTime = restartLerpStartTime = Time.time - RESTART_LERP_DURATION;
	}
	
	void RestartLevel(){
		restartLerpStartTime = Time.time;
		restartLerpStartPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetPos = target.position;
		
		Camera cam = gameObject.GetComponent<Camera>();
		Vector3 camSize = transform.position - cam.ScreenToWorldPoint(new Vector3(0,0, cam.nearClipPlane)); 
		
		Vector3 cameraAtEdgeOfMap;
		Vector3 playerAtEdgeOfScreen;
		
		camSize.x *= Mathf.Sign(targetPos.x);
		camSize.z *= Mathf.Sign(targetPos.z);

		playerAtEdgeOfScreen.x = targetPos.x + (0.4f) - camSize.x;
		cameraAtEdgeOfMap.x = mapSize.x - (camSize.x);

		
		playerAtEdgeOfScreen.z = targetPos.z + (0.4f) - camSize.z;
		cameraAtEdgeOfMap.z = mapSize.z - (camSize.z);

		
		Vector3 lerpAmmount = Vector3.zero;
		lerpAmmount.x = targetPos.x / FOLLOW_MAX_DIST;
		lerpAmmount.x = Mathf.Clamp(lerpAmmount.x, -1, 1);	
		lerpAmmount.x *= 0.993f;
		lerpAmmount.x = 1- Mathf.Sqrt(1-(lerpAmmount.x*lerpAmmount.x));
		
		lerpAmmount.z = targetPos.z / FOLLOW_MAX_DIST;
		lerpAmmount.z = Mathf.Clamp(lerpAmmount.z, -1, 1);	
		lerpAmmount.z *= 0.995f;
		lerpAmmount.z = 1- Mathf.Sqrt(1-(lerpAmmount.z*lerpAmmount.z));
		
		Vector3 pos = Vector3.zero;
		//pos.x = Mathf.Lerp(targetPos.x, cameraAtEdgeOfMap.x, Mathf.Abs(lerpAmmount.x)) * Mathf.Sign(lerpAmmount.x);
		pos.x = Mathf.Lerp(targetPos.x, playerAtEdgeOfScreen.x, lerpAmmount.x);
		pos.z = Mathf.Lerp(targetPos.z, playerAtEdgeOfScreen.z, lerpAmmount.z);
		//pos.x = Mathf.Lerp(targetPos.x, playerAtEdgeOfScreen.x, lerpAmmount.x);
		//pos.x = Mathf.Lerp(targetPos.x, pos.x, lerpAmmount.x);
//		const float DEADZONE = 0.3f;
//		
//		float lerpX = (targetPos.x / FOLLOW_MAX_DIST) - DEADZONE;
//		lerpX *= 1.0f / ((1 - DEADZONE));
//		
//		if (Mathf.Abs(targetPos.x / FOLLOW_MAX_DIST) < DEADZONE)
//		{
//			pos.x = targetPos.x;
//		}
//		else
//		{
//			pos.x = Mathf.Lerp(DEADZONE * FOLLOW_MAX_DIST, cameraAtEdgeOfMap.x, Mathf.Abs(lerpX)) * Mathf.Sign(lerpX);
//			Debug.Log(lerpX);
//		}
//		
//		pos.z = targetPos.z;
		//pos.z = Mathf.Lerp(0, cameraAtEdgeOfMap.z, Mathf.Abs(lerpAmmount.z)) * Mathf.Sign(lerpAmmount.z);
		//pos.z = Mathf.Lerp(PlayerAtEdgeOfScreen.z, cameraAtEdgeOfMap.z, lerpAmmount.z);
		//pos.z = Mathf.Lerp(targetPos.z, pos.z, lerpAmmount.z);
		
		
		pos.y = transform.position.y;
		
		// Restart lerp
		float a = (Time.time - restartLerpStartTime)/RESTART_LERP_DURATION;
		a = Mathf.Clamp01(a);			
		transform.position = Vector3.Lerp(restartLerpStartPos, pos, Mathf.SmoothStep(0, 1, a));		

	}
}