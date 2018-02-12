using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour {

	[SerializeField]
	private RectTransform HealthBarRect;


	void Start () 
	{
		if (HealthBarRect == null) 
		{
			Debug.LogError ("STATUS INDICATOR: No Health Bar object referenced!");
		}
	}
	

	public void SetHealth( int _cur, int _max)
	{
		float _value = (float) _cur / _max;
	
		HealthBarRect.localScale = new Vector3 (_value, HealthBarRect.localScale.y, HealthBarRect.localScale.z);
	}
}
