using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the Wave UI class that updates the text on screen between enemy waves. 
/// </summary>

public class WaveUI : MonoBehaviour {

	/// <summary>
	/// A list of references to the associated text objects and animator components are required. 
	/// </summary>
	[SerializeField]
	WaveSpawner spawner;

	[SerializeField]
	Animator waveAnimator;

	[SerializeField]
	Text waveCountdownText;

	[SerializeField]
	Text waveCountText;

	private WaveSpawner.SpawnState previousState; //enum 


	void Start () {
	
		//Checks to ensure associated references are provided. 
		if (spawner == null) 
		{
			Debug.LogError ("No spawner referenced");
		}
		if (waveAnimator == null) 
		{
			Debug.LogError ("No waveAnimator referenced");
		}
		if (waveCountdownText == null) 
		{
			Debug.LogError ("No waveCountdownText referenced");
		}
		if (waveCountText == null) 
		{
			Debug.LogError ("No waveCountText referenced");
		}

	}
	

	void Update () {

		switch (spawner.State)  //switch statement to provide different actions (method calls) based on current wave spawner state. 
		{
		case WaveSpawner.SpawnState.Counting:
			UpdateCountingUI();
			break;
		case WaveSpawner.SpawnState.Spawning:
			UpdateSpawningUI();
			break;
		}

		previousState = spawner.State;
	
	}

	void UpdateCountingUI() //method called during counting spawn state where Incoming wave animation is stopped and wave countdown animation begins. 
	{
		if (previousState != WaveSpawner.SpawnState.Counting) 
		{
			waveAnimator.SetBool ("waveIncoming", false);
			waveAnimator.SetBool ("waveCountdown", true);
			//Debug.Log ("Counting");
		}
		waveCountdownText.text = ((int)spawner.WaveCountdown).ToString();
	}

	void UpdateSpawningUI() //If enemies are now spawning then new UI text updated to state incoming wave of enemies. 
	{
		if (previousState != WaveSpawner.SpawnState.Spawning) 
		{
			waveAnimator.SetBool ("waveCountdown", false);
			waveAnimator.SetBool ("waveIncoming", true);

			waveCountText.text = spawner.NextWave.ToString();
			//Debug.Log ("Spawning");
		}

	}
}
