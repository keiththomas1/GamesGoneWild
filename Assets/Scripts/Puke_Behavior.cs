using UnityEngine;
using System.Collections;

public class Puke_Behavior : MonoBehaviour {

	// Public variable
	public float speed = 0.1f;

	// 
	GameObject Puke;

	// Use this for initialization
	void Start () {

		if (Puke == null) {
			Puke = GameObject.FindGameObjectWithTag ("Puke");
			Debug.Log("Puke object found\n");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D( Collider2D coll)
	{
		Destroy ( gameObject);
	}
}
