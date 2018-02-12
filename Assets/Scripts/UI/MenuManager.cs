using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Menu Manager for title screen. This script configures the buttons to load a screen fade between scenes
/// for a set variable "fadeTime". The corresponding scene is then loaded or the game is quit. 
/// </summary>
/// 
public class MenuManager : MonoBehaviour {


	public void Start()
	{
		Cursor.visible = true;
		GameObject.Find ("CrossHairs").SetActive (false);
	}

	void Update() {
		if (Input.GetKey("escape"))
			Application.Quit();
	}


	public void StartGame()
	{
		//StartCoroutine ("StartGameEnum");
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
	}


	/*IEnumerator StartGameEnum() //Coroutine that runs in order to execute the screen fade between menu scene and main game scene.
	{
		float fadeTime = GameObject.Find ("GameMaster").GetComponent<SceneFade> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
		//Debug.Log ("Return to MENU");
	} */


	public void Quit()
	{
		Application.Quit ();
		//StartCoroutine ("QuitEnum");
		Debug.Log ("Game QUIT");
	}


	/*(IEnumerator QuitEnum() //Coroutine that runs when quit button is selected. 
	{
		float fadeTime = GameObject.Find ("GameMaster").GetComponent<SceneFade> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		Application.Quit ();
	} */
}
