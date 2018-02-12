using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour {

	[HideInInspector] public float fireRate;
	[HideInInspector] public float timeBetweenBursts;
	[HideInInspector] public float burstTime;
	[HideInInspector] public float lengthOfBurst;
	[HideInInspector] public float timeToFire = 0;

	public float firerateMin = 1;
	public float firerateMax = 7;
	public float timeBetweenBurstsMin = 0.8f;
	public float timeBetweenBurstsMax = 3f;
	public float lengthOfBurstMin = 0.4f;
	public float lengthOfBurstMax = 1f;

	public GameObject enemyBullet;
	public Transform enemyMuzzleFlash;
	public Transform firePoint;

	public string enemyGunShot;

	private AudioManager audioManager;

	void Start ()
	{
		audioManager = AudioManager.instance;
		if (audioManager == null) 
		{
			Debug.LogError ("No Audio Manager found in scene!");
		} 
	}
	void Awake ()
	{
		firePoint = transform;
		if (firePoint == null) 
			{
				Debug.LogError ("No Fire");
			}
	}

	void Update ()
	{
		fireRate = Random.Range (firerateMin, firerateMax);
		timeBetweenBursts = Random.Range (timeBetweenBurstsMin, timeBetweenBurstsMax);
		lengthOfBurst = Random.Range (lengthOfBurstMin, lengthOfBurstMax);
		burstTime += Time.deltaTime;
	}


	public void isShooting(float length)
	{
		InvokeRepeating ("Shoot", 0, 0.01f);
		Invoke ("StopShoot", length);
	}


	void Shoot()
	{
		if (Time.time > timeToFire) 
		{
			timeToFire = Time.time + 1 / fireRate;
			Instantiate (enemyBullet, firePoint.position, firePoint.rotation);
			Transform clone = Instantiate (enemyMuzzleFlash, firePoint.position, firePoint.rotation) as Transform;
			clone.parent = firePoint;
			Destroy (clone.gameObject, 0.05f);
			audioManager.PlaySound ("EnemyGunShot");
		}
	}


	void StopShoot()
	{
		CancelInvoke ("Shoot");
	}

}