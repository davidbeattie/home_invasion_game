using UnityEngine;
using System.Collections;

public class SceneFade : MonoBehaviour {

	public Texture2D fadeOutTexture; // image for screen overlay.
	public float fadeSpeed = 0.0f; //speed to fade.

	private int drawDepth = -1000; //draw order in hierarchy.

	private float alpha = 1.0f; 
	private int fadeDir = -1; //fade out direction.

	void OnGUI() 
	{
		alpha += fadeDir * fadeSpeed * Time.deltaTime; //fade in direction at designated speed in seconds. 

		alpha = Mathf.Clamp01 (alpha); //clamp alpha value to max 1;

		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha); //set alpha.
		GUI.depth = drawDepth;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture); //draw texture to fit screen area.
	}


	// set fade directoin to -1 (fade in) or 0 (fade out).
	public float BeginFade (int direction)
	{
		fadeDir = direction;
		return (fadeSpeed); //return fade speed.
	}

	void OnLevelLoaded()
	{
		BeginFade (-1); //call fade function.
	}
}
