using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]

public class EnemyPathfinding : MonoBehaviour {

	public EnemyShooting enemyShooting;
	Animator anim;

	//the target
	public Transform target;

	//update rate of path
	public float updateRate = 2;

	//references
	private Seeker seeker;
	private Rigidbody2D enemyRigidbody;

	//calculated path
	public Path path;

	//ai's speed
	public float speed = 300;
	//forcemode
	public ForceMode2D fMode;
	//honestly can’t remember
	[HideInInspector] public bool pathIsEnded = false;

	//the waypoint are we going to
	private int currentWaypoint = 0;
	//how close to a waypoint to continue on
	public float nextWaypointDist = 3;

	private bool searchingForPlayer = false;

	public float playerDistance;
	public float searchWaitTime = 2f;


	public void Start()
	{
		seeker = GetComponent <Seeker> ();
		enemyRigidbody = GetComponent <Rigidbody2D> ();
		anim = GetComponent <Animator> ();

		if (target == null)
		{
			if (!searchingForPlayer) 
			{
				searchingForPlayer = true;
				StartCoroutine (SearchForPlayer ());
			}
			return;
		}

		//return new path result to on path complete
		seeker.StartPath (transform.position, target.position, OnPathComplete);
		StartCoroutine (UpdatePath ());
		anim.SetBool ("isWalking", true);
	}


	IEnumerator SearchForPlayer ()
	{
		GameObject sResult = GameObject.FindGameObjectWithTag ("Player");

		if (sResult == null) 
		{
			yield return new WaitForSeconds (searchWaitTime);
			StartCoroutine (SearchForPlayer ());
		} 
		else 
		{
			target = sResult.transform;
			searchingForPlayer = false;
			StartCoroutine (UpdatePath ());
			yield return false;
		} 

	}


	IEnumerator UpdatePath ()
	{
		if (target == null)
		{
			if (!searchingForPlayer) 
			{
				searchingForPlayer = true;
				StartCoroutine (SearchForPlayer ());
			}
			yield return false;
		}

		seeker.StartPath (transform.position, target.position, OnPathComplete);

		yield return new WaitForSeconds (1/updateRate);
		StartCoroutine (UpdatePath ());
	}


	public void OnPathComplete (Path p)
	{
		Debug.Log ("We got a path, is there an error? " + p.error);
		if (!p.error)
		{
			path = p;
			currentWaypoint = 0;
		}
	}


	void FixedUpdate ()
	{
		if (target == null)
		{
			if (!searchingForPlayer) 
			{
				searchingForPlayer = true;
				StartCoroutine (SearchForPlayer ());
			}
			return;
		}

		if (path == null){
			return;
		}
		if (currentWaypoint >= path.vectorPath.Count) {
			if (pathIsEnded) {
				return;
			}		
			Debug.Log ("End of path reached");
			pathIsEnded = true;
			return;
			}

		if (target != null) 
		{
			pathIsEnded = false;
			lookAtPlayer ();
			//find direction to next waypoint
			Vector2 direction = (path.vectorPath [currentWaypoint] - transform.position).normalized;
			direction *= speed * Time.deltaTime;
		
			//move the ai
			enemyRigidbody.AddForce (direction, fMode);				

			float curWayPointDist = Vector2.Distance (transform.position, path.vectorPath [currentWaypoint]);

			playerDistance = Vector2.Distance (target.position, transform.position);

			if (curWayPointDist < nextWaypointDist && playerDistance > 1f) 
			{
				currentWaypoint++;
				return;
			}

			if (playerDistance <= 10f) 
			{
				if (PlayerHiddenByObstacles () == false) 
				{
					enemyShooting.isShooting (enemyShooting.lengthOfBurst);
					enemyShooting.burstTime = 0;
					anim.SetBool ("isWalking", true);
					//Debug.Log ("Player in sight");
				}
			}
		}
	}


	void lookAtPlayer()
	{
		Vector2 dirToPlayer = target.position - transform.position;
		float rotationZ = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
	}


	bool PlayerHiddenByObstacles ()
	{
		float distanceToPlayer = Vector2.Distance(transform.position, target.position);
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, target.position - transform.position, distanceToPlayer );
		//Debug.DrawRay(transform.position, target.position - transform.position, Color.blue);
		foreach (RaycastHit2D hit in hits)
		{           
			//Debug.DrawRay (transform.position, hit.collider.transform.position - transform.position, Color.red);
			//Debug.Log (hit.collider.name + " has been hit.");
			// ignore the enemy's own colliders (and other enemies)
			if (hit.transform.tag == "Enemy")
				continue;
			// if anything other than the player is hit then it must be between the player and the enemy's eyes (since the player can only see as far as the player)
			if (hit.transform.tag != "Player")
			{
				return true;
			}
		}
		// if no objects were closer to the enemy than the player return false (player is not hidden by an object) 
		return false;
	}
}
