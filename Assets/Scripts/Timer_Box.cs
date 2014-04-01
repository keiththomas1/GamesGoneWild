using UnityEngine;
using System.Collections;

public class Timer_Box : MonoBehaviour {

	public GameObject person;
	// Update is called once per frame
	void Update () 
	{
		if( person.GetComponent<Auto>().gameStarted )
			transform.localScale -= new Vector3(0.043F, 0, 0);
	}
}
