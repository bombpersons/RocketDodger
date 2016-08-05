using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
	
	// Public vars
	public static float ScoreNum
	{
		get {
			return scoreNum;
		}
		set {
			if (value <= MaxScore) {	
				scoreNum = value;
			} else {
				scoreNum = MaxScore;	
			}
		}
	}
	public static float ElapsedTime = 0;
	
	public const float MaxScore = 5000.0f;
	public const float TimeMultiplier = 10.0f;
	
	public const float NearMissRange = 3.0f;
	public const float NearMissMax = 700.0f;
	public const float NearMissConstant = 0.0f;
	public const float NearMissLinear = 1.0f;
	public const float NearMissQuadratic = 0.3f;
	
	public const float TailingRange = 5.0f;
	public const float TailingScoreRate = 50.0f;
	
	public const float CollisionPoints = 500.0f;
	
	public static Vector2 TrickTextScreenPos = new Vector2(0.1f, 0.9f);
	
	// Private vars
	private static GameObject TextPrefab;
	private static float scoreNum;
	
	// Use this for initialization
	void Start () {
		ScoreNum = 0;
		ElapsedTime = 0;
		
		// Grab the text prefab
		TextPrefab = (GameObject)Resources.Load("Prefabs/Text/TrickText", typeof(GameObject));
	}
	
	// Update is called once per frame
	void Update () {
		// If the player is dead don't do anything.
		GameObject player = GameObject.FindWithTag("Player");
		if (player == null)
			return;
		if (player.GetComponent<PlayerDie>().IsDead)
			return;
		
		// Inc score based on time.
		ScoreNum += TimeMultiplier * Time.deltaTime;
		ElapsedTime += Time.deltaTime;
	}
	
	// RestartLevel
	public void RestartLevel() {
		scoreNum = 0;
	}
	
	// Tricks
	public static void NearMiss(float _distance, Vector3 _pos) {
		// No trick if we are too far away
		if (_distance < 0 || _distance > NearMissRange)
			return;
		
		// Get the score for this distance.
		float score = NearMissMax / (NearMissConstant + NearMissLinear * _distance + NearMissQuadratic * _distance * _distance);
		
		// Add to score
		ScoreNum += score;
		
		// Change score text.
		GameObject text = (GameObject)Instantiate(TextPrefab);
		text.GetComponent<GUIText>().text = "Near Miss: " + score.ToString("#");
		
		// Set the position.
		text.GetComponent<BounceSizer>().Bounce(1.0f);
		text.GetComponent<GUIWorldSpace>().Position = _pos;
		//text.guiText.pixelOffset = new Vector2(TrickTextScreenPos.x * Screen.width, TrickTextScreenPos.y * Screen.height);
	}
	
	public static void WasTailed(float _score) {
		ScoreNum += _score;
	}
	
	public static void Collision(Vector3 _pos) {
		// Give the player some points.
		float score = CollisionPoints;
		ScoreNum += score;
		
		// Change score text.
		GameObject text = (GameObject)Instantiate(TextPrefab);
		text.GetComponent<GUIText>().text = "Collision: " + ((int)score).ToString();
		
		// Set the position.
		text.GetComponent<GUIWorldSpace>().Position = _pos;
		text.GetComponent<BounceSizer>().Bounce(1.0f);
	}
}