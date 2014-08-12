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
	}

	// Update is called once per frame
	void Update (){

	}

	/// <summary>
	/// Gravity this instance. Giving the puke gameobject some gravity so it call fall
	/// </summary>
	public void Shoot( float angle, int level)
	{
		
		Vector3 tempRotation = this.transform.rotation.eulerAngles;
		tempRotation.z = angle;
		this.transform.Rotate( tempRotation );

		if( level < 6)
			this.rigidbody2D.gravityScale = 1.5f;
		else if( level >= 6 && level < 8)
			this.rigidbody2D.gravityScale = 2.0f;
		else if( level >= 8 && level < 10)
			this.rigidbody2D.gravityScale = 2.5f;
		else if( level >= 10 &&level < 12)
			this.rigidbody2D.gravityScale = 3.0f;
		else if( level >= 12 && level < 14)
			this.rigidbody2D.gravityScale = 3.5f;
		else if( level >= 14 && level < 16)
			this.rigidbody2D.gravityScale = 4.0f;
		else if( level >= 16 && level < 18)
			this.rigidbody2D.gravityScale = 4.5f;
		else
			this.rigidbody2D.gravityScale = 5.0f;

		rigidbody2D.AddForce( new Vector2( angle*13.0f, -50.0f ) );
	}

	// Obviously destroying our puke gameobject once it collides with the bucket 
	void OnTriggerEnter2D( Collider2D coll)
	{
		if( coll.name == "BucketCatch")
			// Lets destroy the puke gameobject in the Controller file
			controller.GetComponent<SaveTheFloorsContoller> ().DestroyPuke ();
	}
}