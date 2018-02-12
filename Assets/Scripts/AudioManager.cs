using UnityEngine;

/// <summary>
/// Class manages all audio clips within scene. New audio sources are created for each of the sounds set
/// within the array. Parameters can be altered and randomised to provide more diversity during playback.
/// </summary>


[System.Serializable]  
public class Sound { //Separate sound class accessible in inspector. Allows selection of audio clips and setting of various parameters. 

	public string name;
	public AudioClip audioFile;

	[Range(0f,1f)]
	public float volume = 0.7f;
	[Range(0.5f, 1.5f)]
	public float pitch = 1f;

	[Range(0f, 0.5f)]
	public float randomVolume = 0.1f;
	[Range(0f, 0.5f)]
	public float randomPitch = 0.1f;

	public bool looping = false;

	private AudioSource audioSource;

	public void SetSource (AudioSource _audioSource)
	{
		audioSource = _audioSource;
		audioSource.clip = audioFile;
		audioSource.loop = looping;
	}

	public void Play()
	{
		audioSource.volume = volume * (1 + Random.Range(-randomVolume/2, +randomVolume/2));
		audioSource.pitch = pitch * (1 + Random.Range(-randomPitch/2, +randomPitch/2));
		audioSource.Play ();
	}

	public void Stop()
	{
		audioSource.Stop();
	}

}

public class AudioManager : MonoBehaviour { //Audio manager class handles array of sounds.

	public static AudioManager instance;

	[SerializeField]
	Sound[] sounds;

	void Awake()
	{
		if (instance != null) 
		{
			if (instance != this) 
			{
				Destroy (this.gameObject);
			}

			Debug.LogError ("More than one AudioManager found in this scene!");
		}
		else
		{
			instance = this;
			DontDestroyOnLoad (this); //during scene change, stops Unity killing audio game object.
		}
	}


	void Start () //Start creates list of game objects for each sound in array. 
	{
		for (int i = 0; i < sounds.Length; i++) 
		{
			GameObject _go = new GameObject ("Sound_" +i + "_" + sounds[i].name);
			_go.transform.SetParent (this.transform);
			//instantiated game objects have audio source component added. 
			sounds[i].SetSource(_go.AddComponent<AudioSource>()); 
		}

		PlaySound ("MenuMusic");
	}


	public void PlaySound(string _name) //Plays sound with specified name from array.
	{
		for (int i = 0; i < sounds.Length; i++) 
		{
			if (sounds[i].name == _name)
			{
				sounds [i].Play ();
				return;
			}
		}
		Debug.LogWarning ("Audio Manager: No Sound Found: " + _name);
	}

	public void StopSound(string _name) //Plays sound with specified name from array.
	{
		for (int i = 0; i < sounds.Length; i++) 
		{
			if (sounds[i].name == _name)
			{
				sounds [i].Stop();
				return;
			}
		}
		Debug.LogWarning ("Audio Manager: No Sound Found: " + _name);
	}

}
