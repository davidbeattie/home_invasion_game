using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour { //Follows player when he begins to move but locks camera to bounds of the map. 
											//Player can travel around and reach edge of map but anything off map cannot be seen.

	public Transform target;			// The position that that camera will be following.

	public Vector2			//Separate co-ordinates to adjust camera margin and smooth time between movements. 
		margin,
		smoothing;

	public BoxCollider2D bounds;	//Box collider to set bounds to map.

	private Vector3
		_min,		//Co-ordinates to minimum and maximum values for camera bounds. 
		_max;

	public bool isFollowing { get; set; } //Boolean to move camera once player begins to move.

	private float nextTimeToSearch = 0; // Assigned float value to adjust time camera looks for player during respawn.


	public void Start ()
	{
		_min = bounds.bounds.min; //Sets min & max value of bounds to box collider values.
		_max = bounds.bounds.max;
		isFollowing = true; //boolean assigned to true on start.
	}


	void FixedUpdate ()
	{
		if (target == null)  //Looks for player is there is none and runs find player method.
		{
			FindPlayer ();
			return;
		}

		var x = transform.position.x;  //sets x & y variables to player's current position. 
		var y = transform.position.y;

		if (isFollowing) // Sets a margin around the player. If player goes past margin then camera begins to follow with an associated smoothing factor..
		{
			if (Mathf.Abs (x - target.position.x) > margin.x)  
			{
				x = Mathf.Lerp (x, target.position.x, smoothing.x * Time.deltaTime);
			}
			if (Mathf.Abs (y - target.position.y) > margin.y) 
			{
				y = Mathf.Lerp (y, target.position.y, smoothing.y * Time.deltaTime);
			}
		}
		// Camera is then clamped to the margins of the 2D box collider x and y limits. 
		var cameraHalfWidth = Camera.main.orthographicSize * ((float) Screen.width / Screen.height);

		x = Mathf.Clamp (x, _min.x + cameraHalfWidth, _max.x - cameraHalfWidth);  //constrains x axis.
		y = Mathf.Clamp (y, _min.y + Camera.main.orthographicSize, _max.y - Camera.main.orthographicSize); //constrains y axis.

		transform.position = new Vector3 (x, y, transform.position.z); //position of camera updated to clamped x and y values. 
	}


	void FindPlayer()  // searches for the player game object and resets camera position during respawns. 
	{
		if (nextTimeToSearch <= Time.time)
		{
			GameObject searchResult = GameObject.FindGameObjectWithTag ("Player");
			if (searchResult != null)
				target = searchResult.transform;
			nextTimeToSearch = Time.time + 0.5f;
		}
	}
}

