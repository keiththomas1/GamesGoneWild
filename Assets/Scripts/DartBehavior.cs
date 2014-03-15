using UnityEngine;
using System.Collections;

public class DartBehavior : MonoBehaviour 
{
	GameObject globalController;
	public GameObject dartController;
	
	Vector2 dartJumpHeight;
	float dartJumpConstant;

	bool canControl;
	public bool horizontalMoving;
	Vector2 speedVector;

	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find("Global Controller");

		dartJumpConstant = 200.0f;

		canControl = true;
		horizontalMoving = false;
		speedVector = new Vector2( .06f, 0.0f );
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( canControl )
		{
			if( Input.GetMouseButtonDown( 0 ) )
			{
				dartJumpHeight = new Vector2( 0.0f, (rigidbody2D.velocity.y*-50.0f) + dartJumpConstant );
				rigidbody2D.AddForce( dartJumpHeight );
			}
		}

		if( horizontalMoving )
		{
			Debug.Log("Moving " + Time.frameCount);
			transform.Translate( speedVector );
		}

		if( transform.position.y < -6.0f )
		{
			globalController.GetComponent<GlobalController>().LostMinigame();
		}

		transform.Rotate( new Vector3( 0.0f, 0.0f, 
		                              transform.rotation.z + (rigidbody2D.velocity.y) ) );
	}

	void OnTriggerEnter2D( Collider2D coll )
	{
		if( "DartBoard" == coll.name )
		{
			horizontalMoving = false;
			canControl = false;
			
			rigidbody2D.velocity = new Vector2( 0.0f, 0.0f );
			rigidbody2D.gravityScale = 0.0f;

			// HACK - Show points or something here instead of immediate win

			globalController.GetComponent<GlobalController>().dartLevel++;
			globalController.GetComponent<GlobalController>().BeatMinigame();
		}
		else
		{
			if( canControl )
			{
				horizontalMoving = false;
				canControl = false;
				
				rigidbody2D.AddForce( new Vector2( 0.0f, (rigidbody2D.velocity.y*-50.0f) + dartJumpConstant ) );
			}
		}
	}
}
