using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class GameOverUI : MonoBehaviour {


	public void Awake()
	{
		Cursor.visible = true;
		GameObject.Find ("CrossHairs").SetActive (false);
	}

	public void Menu()
	{
		StartCoroutine ("MenuEnum");
	}

	IEnumerator MenuEnum()
	{
		float fadeTime = GameObject.Find ("GameMaster").GetComponent<SceneFade> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex - 1);
		//Debug.Log ("Return to MENU");
		//Application.Quit ();
	}
	
	// Update is called once per frame
	public void Retry()
	{
		StartCoroutine ("RetryEnum");
	}

	IEnumerator RetryEnum()
	{
		float fadeTime = GameObject.Find ("GameMaster").GetComponent<SceneFade> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
