using UnityEngine;
using System.Collections;

/// <summary>
/// Game Master script handles score indexing as well player and enemy deaths.
/// </summary>

public class GameMaster : MonoBehaviour {

	public static GameMaster gm; 
	AudioManager audioManager; 

	[SerializeField]
	private int maxLives = 3;		//private integer but max lives can be set  in inspector.
	private static int _remainingLives = 3; 
	public static int RemainingLives 
	{
		get { return _remainingLives; }
	}

	[SerializeField]
	private int startScore = 0;  
	public static int playerScore;

	public GameObject playerPrefab; //reference to player Gameobject.
	public Transform[] playerSpawnPoints; //array of spawn point transforms.
	public int spawnDelay = 3; //pre-defined delay between respawning.

	public CameraShake camShake; //reference to the camera shake class.

	[SerializeField]
	private GameObject gameOverUI; //reference to the game over UI gameobject (menu manager).


	void Awake () 
	{ 
		if (gm == null) //check to see if game master gameobject is present, if not next line finds it. 
		{
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
		}
	}

	void Start ()
	{
		playerScore = startScore;  //player score initialised to starting score integer.
		_remainingLives = maxLives; //remaining lives initialised to maximum player lives integer. 

		if (camShake == null) //check to see if camera shake reference is present on game master script.
		{
			Debug.Log ("No cam shake referenced in game master");
		}
		if (playerSpawnPoints.Length == 0) //check to ensure the spawn points array contains a number of spawn points. 
		{
			Debug.LogError("No player spawn points referenced.");
		}

		audioManager = AudioManager.instance; //reference to audio manager.

		if (audioManager == null) //check to see if audio manager is present in scene.
		{
			Debug.LogError ("No Audio Manager found");
		}
	}

	void Update()
	{
	if (Input.GetKey("escape"))
		Application.Quit();
	}

	public void GameOver()
	{
		gameOverUI.SetActive (true);  //Game over method that makes the game over UI active. 
		//Debug.Log ("GAME OVER");
	}


	public IEnumerator RespawnPlayer ()  //Co-routine to repsawn player based on the designated spawn delay.
	{
	//	Debug.Log ("TODO: respawn sound");
		yield return new WaitForSeconds (spawnDelay);
		int spawnPointIndex = Random.Range (0, playerSpawnPoints.Length); //spawn point is set to random point within the array of given spawn points.
		Instantiate (playerPrefab, playerSpawnPoints[spawnPointIndex].position, playerSpawnPoints[spawnPointIndex].rotation); //player game object is instantiated at one of the random spawn points selected. 
		//	Debug.Log ("TODO: Add spawn particles");

	}


	public static void KillPlayer (Player player) //static method that calls the _Killplayer public method from within game master.
	{
		gm._KillPlayer (player); 
		_remainingLives -= 1;
		if (_remainingLives <= 0)  //if statement that checks if lives have reached 0. if so then game over method is called. 
		{
			gm.GameOver ();
		} 
		else 
		{
			gm.StartCoroutine (gm.RespawnPlayer ()); //if lives are not yet 0 then respawn co-routine commences. 
		}
	}


	public void _KillPlayer(Player _player) //this class plays effects and kills player when called from static kill player method. 
	{
		GameObject _clone = Instantiate (_player.deathParticles, _player.transform.position, _player.transform.rotation) as GameObject; //death particles are played (blood sprite).
		camShake.Shake (_player.shakeAmt, _player.shakeLength); //camera shake class is given a designated shake amount and length which in turn shakes camera.
		Destroy (_player.gameObject); //player game object destroyed.
		Destroy (_clone, 1f); // death particle game object is destroyed.
		audioManager.PlaySound ("PlayerDeath"); //death sound for player is triggered. 
	}


	public static void KillEnemy (EnemyHealth enemyHealth) //static method that calls the _KillEnemy public method from within game master.
	{
		gm._KillEnemy (enemyHealth);
	}


	public void _KillEnemy(EnemyHealth _enemyHealth) 
	{

		playerScore += _enemyHealth.scorePoints; // player enemy is killed then the associated points for enemy death is given to player. passed to Score UI text and updated. 
		GameObject _clone = Instantiate (_enemyHealth.deathParticles, _enemyHealth.transform.position, _enemyHealth.transform.rotation) as GameObject; //death particles instantiated.
		camShake.Shake (_enemyHealth.shakeAmt, _enemyHealth.shakeLength); //camera is shaken for set length and intensity.
		Destroy (_enemyHealth.gameObject); //enemy game object destroyed. 
		Destroy (_clone, 3f); //death particles destroyed.
		audioManager.PlaySound ("EnemyDeath"); //enemy death sound played. 
	}

}
