using System;
using UnityEngine;
 
public class Drawing
{	
	// Public vars
	public static Texture WhiteTexture {
		get {
			if (whiteTexture == null) {
				Texture2D tex = new Texture2D(1, 1);
				tex.SetPixel(0, 0, Color.white);
				tex.Apply();
				
				whiteTexture = tex;
			}
			return whiteTexture;
		}
	}
	
	// Private vars
	private static Texture whiteTexture = null;
	
	// Hud elements
	public static void DrawBar(Texture _bg, Color _bgColor, Texture _fg, Color _fgColor, Texture _border, Color _borderColor, float _borderWidth, Vector2 _pos, Vector2 _size, float _fill, Color _textColor, string _text) {
		// Save the old color
		Color oldColor = GUI.color;
		
		// Center it.
		_pos.x -= _size.x/2;
		_pos.y -= _size.y/2;
		
		// Draw the border.
		GUI.color = _borderColor;
		GUI.DrawTexture(new Rect(_pos.x, _pos.y, _size.x, _size.y), _border);
		
		// Draw the background
		GUI.color = _bgColor;
		GUI.DrawTexture(new Rect(_pos.x + _borderWidth, _pos.y + _borderWidth, _size.x - _borderWidth * 2, _size.y - _borderWidth * 2), _bg);
		
		// Draw the foreground
		GUI.color = _fgColor;
		GUI.DrawTexture(new Rect(_pos.x + _borderWidth, _pos.y + _borderWidth, (_size.x - _borderWidth * 2) * _fill, _size.y - _borderWidth * 2), _fg);
		
		// Get the size of the label
		GUIStyle style = "Label";
		Vector2 textSize = style.CalcSize(new GUIContent(_text));
		
		// Draw the label
		GUI.color = _textColor;
		GUI.Label(new Rect((_pos.x + _size.x / 2) - textSize.x / 2, (_pos.y + _size.y / 2) - textSize.y / 2, textSize.x, textSize.y), _text); 
		
		// Put the old color back.
		GUI.color = oldColor;		
	}	
}