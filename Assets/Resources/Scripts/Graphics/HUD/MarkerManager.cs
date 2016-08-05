using UnityEngine;
using System.Collections;

public class MarkerManager : MonoBehaviour
{
	
	Transform player;
	
	const float LINE_MAX_LENGTH = 10.0f;
	const float LINE_LENGTH_SCALE = 0.5f;
	const float LINE_THICKNESS_MULTIPLIER = 0.1f;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// create fustrum planes
		Plane[] fustrumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);		
		GameObject[] rockets = GameObject.FindGameObjectsWithTag("Rocket");
		Vector3 playerPos = player.position;
		
		// loop through each rocket
		foreach (GameObject rocket in rockets) {
			LineRenderer line = rocket.GetComponent<LineRenderer>();
			Vector3 rocketScreenPos = Camera.main.WorldToScreenPoint(rocket.transform.position);
			
			// check if the rocket in on the screen
			if (!(rocketScreenPos.x < 0 || rocketScreenPos.y < 0 || rocketScreenPos.x > Screen.width || rocketScreenPos.y > Screen.height)){
				// rocket is on the screen, disable the line
				line.enabled = false;
				continue;			
			}
			
			// create ray
			Vector3 rayDir = rocket.transform.position - playerPos;
			Ray ray = new Ray(playerPos, rayDir);			
			float closeestRayHitDist = float.MaxValue;
			
			// loop throuch each plane and check for collision with the ray
			foreach (Plane plane in fustrumPlanes) {
				float rayHitDist;
				if (plane.Raycast(ray, out rayHitDist)) {
					// the ray hit the plane
					if (rayHitDist > 0 && rayHitDist < closeestRayHitDist) {	
						// this is the closest plane intersection so far
						closeestRayHitDist = rayHitDist;						
					}
				}
			}
			
			if(closeestRayHitDist == float.MaxValue)
				continue;// ray didn't hit any of the planes
			
			
			Vector3 rayHitPos = ray.GetPoint(closeestRayHitDist);
			
			// calculate Line length
			float rayLength = (rayHitPos - rocket.transform.position).magnitude;
			rayLength = Mathf.Clamp(rayLength, 0, LINE_MAX_LENGTH);
			rayLength *= LINE_LENGTH_SCALE;
			Vector3 lineStart = ray.GetPoint(closeestRayHitDist - rayLength);
			
			float width = rayLength * LINE_THICKNESS_MULTIPLIER;	
			Color color = rocket.GetComponent<AutoSpawnSmokeTrail>().Color;
			Color color2 = color;
			color2.a = 0;
			line.enabled = true;
			line.SetColors(color2, color);
			line.SetWidth(0, width);
			line.SetPosition(0, lineStart);
			line.SetPosition(1, rayHitPos);
			
			//Debug.DrawLine(ray.GetPoint(lowestRayHitDist - rayLength * 0.5f), rayHitPos);
		}
	}
}
