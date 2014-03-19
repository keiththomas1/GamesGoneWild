using UnityEngine;
using System.Collections;

public class Puke_Behavior : MonoBehaviour {

	// Identifier for our puke gameobject
	public GameObject pukePrefab;

	// Identifier for SaveTheFloorsController
	public GameObject controller;

	// For storing position of pukePrefab gameobject
	Vector3 pukePrefabPos;

	// Use this for initialization
	void Start (){

		// Finding our gameobject with the puke tag
		pukePrefab = GameObject.FindGameObjectWithTag ("Puke");

		// Storing pukePrefab gameobject (float) position
		pukePrefabPos = transform.position;

		// Debugging purposes
		Debug.Log("Puke Position: " + pukePrefabPos);
	}

	// Update is called once per frame
	void Update (){

	}

	/// <summary>
	/// Gravity this instance. Giving the puke gameobject some gravity so it call fall
	/// </summary>
	public void Gravity(){
		
		// We need to give our gameobject some gravity inorder for it to fall.
		// Currently, gravity is set to 0 on all pukePrefab gameobjects.
		Debug.Log ("In Puke_Behavior.cs gravity()");
		this.rigidbody2D.gravityScale = 1;
	}

	// Obviously destroying our puke gameobject once it collides with the bucket 
	void OnTriggerEnter2D( Collider2D coll){

		if( coll.name == "Bucket")
			// Lets destroy the puke gameobject in the Controller file
			controller.GetComponent<SaveTheFloorsContoller> ().DestroyPuke ();
	}
}