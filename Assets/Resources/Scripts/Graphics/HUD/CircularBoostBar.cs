using UnityEngine;

public class CircularBoostBar : MonoBehaviour {

	public Renderer renderer;
	
	void Start () {
        renderer = GetComponent<Renderer>();
	}

	void Update () {
        renderer.material.mainTextureOffset = new Vector2(1-(Score.ScoreNum / Score.MaxScore), 0);
    }
}