using UnityEngine;
using System.Collections;

public class MoveBullet : MonoBehaviour {

	public int bulletSpeed = 200;
	public int damageQuota = 20;

	public GameObject bulletSparks;
	public GameObject bloodSpray;
	public GameObject explosion;

	//public AudioSource ricochet;
	//public AudioSource bulletHit;

	Vector3 mousePosition;
	Vector3 direction;

	public float camShakeAmount = 0.05f;
	public float camShakeLength = 0.3f;

	CameraShake camShake;
	AudioManager audioManager;

	void Start()
	{
		camShake = GameMaster.gm.GetComponent<CameraShake> ();
		if (camShake == null)
			Debug.LogError ("No CameraShake script found on GM object.");

		mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePosition.z = 0.0f;
		direction = (mousePosition - transform.position).normalized;

		audioManager = AudioManager.instance;

		if (audioManager == null)
		{
			Debug.LogError ("No Audio Manager found");
		}
	}

	void Update ()
	{
		transform.position += direction * bulletSpeed  * Time.deltaTime;
	}


	void OnTriggerEnter2D (Collider2D coll)
	{
		//Debug.Log (coll.gameObject.name+", "+coll.gameObject.tag);
		if (coll.gameObject.tag == ("Enemy")) 
		{
			PlayBloodSpray ();
			Destroy (gameObject);

			EnemyHealth enemyHealth = coll.gameObject.GetComponent<EnemyHealth> ();
			if (enemyHealth != null) 
			{
				//Debug.Log ("We Hit " + hit.collider.name + " and did " + damageQuota + " damage.");
				enemyHealth.DamageEnemy (damageQuota);
			}
		}

		if (coll.gameObject.tag == ("Object"))
		{
			//Debug.Log(coll.gameObject.name+", "+coll.gameObject.tag);
			PlayBulletSparks ();
			Destroy (gameObject);
		}

		if (coll.gameObject.tag == ("Destructible")) 
		{
			PlayExplosion ();
			Destroy (coll.gameObject);
			Destroy (gameObject);
			CamShakeEffect ();
			audioManager.PlaySound ("Explosion");
		} 
		else
			Destroy (gameObject, 3f);
	}


	void PlayBulletSparks()
	{
		Instantiate (bulletSparks, transform.position, transform.rotation);
	}


	void PlayBloodSpray()
	{
		Instantiate (bloodSpray, transform.position, transform.rotation);
	}

	void PlayExplosion()
	{
		Instantiate (explosion, transform.position, transform.rotation);
	}

	void CamShakeEffect()
	{
		camShake.Shake (camShakeAmount, camShakeLength);
	}
}