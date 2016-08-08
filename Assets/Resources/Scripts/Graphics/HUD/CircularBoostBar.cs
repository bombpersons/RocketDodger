using UnityEngine;

public class CircularBoostBar : MonoBehaviour {

	Renderer renderer;    
	
	void Start () {
        renderer = GetComponent<Renderer>();
	}

	void Update () {

        float score = (Score.ScoreNum / Score.MaxScore);

        //textureOffset 0.5 = 0% boost
        //textureOffset -0.5 = 100% boost

        float textrueOfset = -0.5f + (1.0f - score);

        renderer.material.mainTextureOffset = new Vector2(textrueOfset, 0.0f);
    }
}