using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Text))]

public class ScoreManager : MonoBehaviour
{

	private Text scoreText;                      // Reference to the Text component.


	void  Awake() 
	{
		// Set up the reference.
		scoreText = GetComponent<Text> ();
	}

	void Update()
	{	// Set the displayed text to be the word "Score" followed by the score value.
		scoreText.text = "SCORE: " + GameMaster.playerScore.ToString ();
	}
}