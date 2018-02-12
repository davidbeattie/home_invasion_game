using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, yMin, yMax;
}

public class PlayerMovement : MonoBehaviour {

	public float speed;
	public WeaponShooting weaponShooting;
	public Boundary boundary;

	Rigidbody2D playerRigidbody;
	Vector2 movement;
	Animator anim;
	public string playerFootStep;

	private AudioManager audioManager;

	private bool shooting;
	private bool isMoving;

	void Start()
	{
		audioManager = AudioManager.instance;
		if (audioManager == null) 
		{
			Debug.LogError ("No Audio Manager found in scene!");
		} 
	}


	void Awake ()
	{
		playerRigidbody = GetComponent <Rigidbody2D> ();
		anim = GetComponent <Animator> ();
	}


	void FixedUpdate ()
	{
		float moveH = Input.GetAxisRaw ("Horizontal");
		float moveV = Input.GetAxisRaw ("Vertical");

		Move (moveH, moveV);

		Turning ();

		Shoot ();
	}


	void Move (float moveH, float moveV)
	{
		movement.Set (moveH, moveV);

		movement = movement.normalized * speed;
			
		playerRigidbody.AddForce (movement);

		playerRigidbody.position = new Vector2 (Mathf.Clamp (playerRigidbody.position.x, boundary.xMin, boundary.xMax), Mathf.Clamp (playerRigidbody.position.y, boundary.yMin, boundary.yMax));

		if (moveH != 0f || moveV != 0f) 
		{
			anim.SetBool ("isWalking", true);
		}
		else 
		{
			anim.SetBool ("isWalking", false);
		}
	}


	void Turning ()
	{
		var mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Quaternion newRotation = Quaternion.LookRotation (transform.position - mousePos, Vector3.forward);

		transform.rotation = newRotation;
		transform.eulerAngles = new Vector3 (0, 0, transform.eulerAngles.z + 90f);
		playerRigidbody.angularVelocity = 0;

	}


	void Shoot ()
	{
		AnimateShoot ();

		if (weaponShooting.fireRate == 0)
		{
			if (Input.GetButtonDown ("Fire1"))
			{ 
				weaponShooting.FireWeapon();
				}
			}
			else 
			{
				if	(Input.GetButton ("Fire1") && Time.time > weaponShooting.timeToFire)
				{
					weaponShooting.timeToFire = Time.time +1/weaponShooting.fireRate;
					weaponShooting.FireWeapon();
				}
			}
	}

	void AnimateShoot()
	{
		if (Input.GetButton ("Fire1"))
		{
			anim.SetBool ("isWalking", true);	
		}
	}
}
