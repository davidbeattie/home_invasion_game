using UnityEngine;

public class Player : MonoBehaviour {

	[System.Serializable]
	public class PlayerStats
	{

		public int maxHealth = 100;

		private int _curHealth;		
		public int curHealth 
		{
			get { return _curHealth; }
			set { _curHealth = Mathf.Clamp (value, 0, maxHealth); }
		}

		public void Init()
		{
			curHealth = maxHealth;
		}
	}

	public GameObject healthBar;

	public PlayerStats playerStats = new PlayerStats();

	public Transform deathParticles;

	public float shakeAmt = 0.01f;
	public float shakeLength = 0.01f;


	void Start ()
	{
		playerStats.Init();

		if (deathParticles == null) 
		{
			Debug.Log ("No blood splat sprite referenced on enemy.");
		}

		healthBar = GameObject.Find ("PlayerHealthBar");
		SetHealth (playerStats.curHealth, playerStats.maxHealth);
	}
		

	public void DamagePlayer (int damage) 
	{
		playerStats.curHealth -= damage;
		if (playerStats.curHealth <= 0) 
		{
			GameMaster.KillPlayer (this);
		}

		SetHealth (playerStats.curHealth, playerStats.maxHealth);
	}

	public void SetHealth ( int _cur, int _max)
	{
		float _value = (float) _cur / _max;

		healthBar.transform.localScale = new Vector3 (_value, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
	}
}



	
