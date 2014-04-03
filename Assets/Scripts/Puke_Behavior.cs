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
	public void Shoot( float angle ){
		
		// We need to give our gameobject some gravity inorder for it to fall.
		// Currently, gravity is set to 0 on all pukePrefab gameobjects.
		Debug.Log ("In Puke_Behavior.cs gravity()");
		
		Vector3 tempRotation = this.transform.rotation.eulerAngles;
		tempRotation.z = angle;
		this.transform.Rotate( tempRotation );

		this.rigidbody2D.gravityScale = 1.5f;
		rigidbody2D.AddForce( new Vector2( angle*13.0f, -50.0f ) );
	}

	// Obviously destroying our puke gameobject once it collides with the bucket 
	void OnTriggerEnter2D( Collider2D coll){

		if( coll.name == "Bucket")
			// Lets destroy the puke gameobject in the Controller file
			controller.GetComponent<SaveTheFloorsContoller> ().DestroyPuke ();
	}
}