using UnityEngine;
using System.Collections;

public class BoostBar : MonoBehaviour {
	
	// The texture for the bar.
	public Texture BG;
	public Texture FG;
	public Texture Border;
	
	public Vector2 Size;
	public Color ForegroundColor;
	public Color BackgroundColor;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (BG == null)
			BG = Drawing.WhiteTexture;
		if (FG == null)
			FG = Drawing.WhiteTexture;
		if (Border == null)
			Border = Drawing.WhiteTexture;
	}
	
	// Draw it
	void OnGUI() {
		Drawing.DrawBar(BG, BackgroundColor, FG, ForegroundColor, Border, Color.white, 0,
						new Vector2(transform.position.x * Screen.width, transform.position.y * Screen.height),
						new Vector2(Size.x * Screen.width, Size.y * Screen.height), Score.ScoreNum/Score.MaxScore, Color.white, "Boost");
			
	}
}
