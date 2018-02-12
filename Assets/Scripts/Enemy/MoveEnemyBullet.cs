using UnityEngine;
using System.Collections;

public class MoveEnemyBullet : MonoBehaviour {

	public int bulletSpeed = 200;
	public int damageQuota = 5;

	public Transform player;
	public GameObject bulletSparks;
	public GameObject bloodSpray;
	public GameObject explosion;

	public float camShakeAmount = 0.05f;
	public float camShakeLength = 0.2f;

	CameraShake camShake;
	AudioManager audioManager;


	void Start ()
	{
		camShake = GameMaster.gm.GetComponent<CameraShake> ();
		if (camShake == null)
			Debug.LogError ("No CameraShake script found on GM object.");

		audioManager = AudioManager.instance;

		if (audioManager == null)
		{
			Debug.LogError ("No Audio Manager found");
		}
	}


	void Update ()
	{
		transform.Translate (Vector3.right * Time.deltaTime * bulletSpeed);
	}
		

	void OnTriggerEnter2D (Collider2D coll)
	{
		//Debug.Log (coll.gameObject.name+", "+coll.gameObject.tag);
		if (coll.gameObject.tag == ("Player")) 
		{
			PlayBloodSpray ();
			Destroy (gameObject);

			Player player = coll.gameObject.GetComponent<Player> ();
			if (player != null) 
			{
				//Debug.Log ("We Hit " + hit.collider.name + " and did " + damageQuota + " damage.");
				player.DamagePlayer (damageQuota);
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