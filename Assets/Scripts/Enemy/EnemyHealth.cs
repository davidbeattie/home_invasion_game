using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
	[System.Serializable] //Class within EnemyHealth class to set health stats. 
	public class EnemyStats 
	{
		public int maxHealth = 100; //max health given to enemies.

		private int _curHealth; 
		public int curHealth
		{
			get { return _curHealth; }
			set { _curHealth = Mathf.Clamp (value, 0, maxHealth); }
		}

		public void Init()
		{
			curHealth = maxHealth;  //initialises current health and designated max health integer.
		}
	}


	public EnemyStats enemyStats = new EnemyStats(); //references enemy stats class.

	public Transform deathParticles; //reference to associated death sprite game object.

	public float shakeAmt = 0.1f; //values for how much the camera should shake during enemy death. 
	public float shakeLength = 0.1f;

	public int scorePoints = 50; //pre set score value for enemy kill.

	[Header ("Optional: ")]
	[SerializeField]
	private StatusIndicator StatusIndicator; //Class linked to health bar that follows enemies around screen. Set to optional as not necessary to be included.


	void Start()
	{
		enemyStats.Init (); //anything in enemy stats Init() method is assigned. 

		if (StatusIndicator == null) { //checks for associated status indicator on enemy player. 
			Debug.LogError ("No Status Indicator Ref'd on enemy");
		} 
		else 
		{
			StatusIndicator.SetHealth (enemyStats.curHealth, enemyStats.maxHealth); //if status indicator is found then health is set to the current health enemy player has. 
		}
		if (deathParticles == null) //check for death particles game object.
		{
			Debug.Log ("No blood splat sprite referenced on enemy.");
		}
	}


	public void DamageEnemy (int damage)  //damage to enemy method 
	{
		enemyStats.curHealth -= damage; 
		if (enemyStats.curHealth <= 0) //if state that checks the current health of the enemy. If this reaches 0 or less than 0 then the enemy is killed via call to KillEnemy method in game master script. 
		{
			GameMaster.KillEnemy(this);
		}

		if (StatusIndicator != null) 
		{
			StatusIndicator.SetHealth (enemyStats.curHealth, enemyStats.maxHealth); //health bar on status indicator is updated to show current health - any damage incurred.
		}
	}
}