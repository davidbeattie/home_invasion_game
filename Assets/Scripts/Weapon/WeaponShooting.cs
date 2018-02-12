using UnityEngine;
using System.Collections;

public class WeaponShooting : MonoBehaviour {

	public float fireRate = 0;
	public int damageQuota = 10;

	public Transform muzzleFlash;
	public GameObject bullet;

	//Handle camera shaking
	public float camShakeAmount = 0.05f;
	public float camShakeLength = 0.1f;



	[HideInInspector] public float timeToFire = 0;
	Transform firePoint;

	CameraShake camShake;
	private AudioManager audioManager;

	void Start()
	{
		camShake = GameMaster.gm.GetComponent<CameraShake> ();
		if (camShake == null)
			Debug.LogError ("No CameraShake script found on GM object.");

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


	public void FireWeapon()
	{
		Bullet ();
		CamShakeEffect ();
		audioManager.PlaySound ("PlayerGunShot");
	}


	void Bullet()
	{
		Instantiate (bullet, firePoint.position, firePoint.rotation);
		Transform cloneMuzzleFlash = Instantiate (muzzleFlash, firePoint.position, firePoint.rotation) as Transform;
		cloneMuzzleFlash.parent = firePoint;
		Destroy (cloneMuzzleFlash.gameObject, 0.03f);
	}


	void CamShakeEffect()
	{
		camShake.Shake (camShakeAmount, camShakeLength);
	}
}