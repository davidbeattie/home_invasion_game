using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour {

	public enum SpawnState {Spawning, Waiting, Counting};

	[System.Serializable] //Enables instances of 'Wave" class to be editable in inspector.
	public class Wave 
	{
		public string waveName;
		public Transform enemyAI;
		public int amount;
		public float spawnRate;
	}


	public Wave[] waves;
	public Transform[] spawnPoints;
	private int nextWave = 0;
	public int NextWave 
	{
		get { return nextWave +1; }
	}

	public float timeBetweenWaves = 5f;
	private float waveCountdown;

	public float WaveCountdown
	{
		get {return waveCountdown;}
	}

	private float enemyAliveCountdown = 1f;

	private SpawnState state = SpawnState.Counting;

	public SpawnState State 
	{
		get { return state; }
	}

	void Start()
	{
		waveCountdown = timeBetweenWaves; //Sets wave countdown to time between waves. 

		if (spawnPoints.Length == 0) //Checks if spawnpoints have been set. If not, error thrown.
		{
			Debug.LogError ("No spawn points referenced.");
		}
	}

	void Update()
	{
		if (state == SpawnState.Waiting) 
		{
			if (!EnemyIsAlive ()) //runs boolean method to check for any enemies left.
			{ 
				WaveCompleted(); //Begin new round.
			} 
			else 
			{
				return;
			}
		}

		if (waveCountdown <= 0) 
		{
			if (state != SpawnState.Spawning) //Once wave countdown timer hits 0 then first check if already spawning. 
			{
				StartCoroutine (SpawnWave (waves[nextWave])); //If not spawning then spawn coroutine begins.
			}
		} 
		else 
		{
			waveCountdown -= Time.deltaTime; //Makes sure countdown timer is relative to real time instead of frames/second.  
		}									
	}


	bool EnemyIsAlive() //Boolean checking to see if enemies are still alive once per second. 
	{
		enemyAliveCountdown -= Time.deltaTime;

		if (enemyAliveCountdown <= 0f) 
		{
			enemyAliveCountdown = 1f;
			if (GameObject.FindGameObjectWithTag ("Enemy") == null) 
			{
				return false;
			}
		}
		return true;
	}

	IEnumerator SpawnWave (Wave _wave)
	{
		Debug.Log ("Spawning Wave: " + _wave.waveName);
		state = SpawnState.Spawning; //sets state to spawning. 

		for (int i = 0; i < _wave.amount; i++) //Loops through amount of enemies to spawn. 
		{
			SpawnEnemy (_wave.enemyAI); //For each enemy to spawn, call spawnEnemy method. 
			yield return new WaitForSeconds (1f/_wave.spawnRate); //We continue spawn loop until max enemies have been spawned for specific wave. 
		}

		state = SpawnState.Waiting;

		yield break;
	}

	void SpawnEnemy (Transform _enemy) //Instantiates enemies.
	{
		Debug.Log ("Spawning Enemy: " + _enemy.name);

		Transform _sPoint = spawnPoints [Random.Range (0, spawnPoints.Length)];
		Instantiate (_enemy, _sPoint.position, _sPoint.rotation);
	}

	void WaveCompleted()
	{
		Debug.Log ("Wave completed.");

		state = SpawnState.Counting;
		waveCountdown = timeBetweenWaves;

		if (nextWave + 1 > waves.Length -1)  //Checks to see if waves are completed so error isn't thrown once max waves are reached.
		{
			nextWave = 0;
			Debug.Log ("Completed all waves. Looping..");
		}

		nextWave++;
	}

}
