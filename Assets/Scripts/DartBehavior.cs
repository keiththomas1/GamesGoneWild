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

	Vector3 startPosition;
	bool gameStarted;
	public Sprite brokenDart;

	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find("Global Controller");

		dartJumpConstant = 200.0f;

		canControl = true;
		horizontalMoving = false;
		speedVector = new Vector2( .06f, 0.0f );

		startPosition = transform.position;
		gameStarted = false;
	}

	// Update is called once per frame
	void Update () 
	{
		if( !gameStarted )
		{
			rigidbody2D.gravityScale = 0.0f;
			transform.position = startPosition;
		}
		else
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
			
			if( transform.position.y < -6.0f || transform.position.x > 13.0f )
			{
				globalController.GetComponent<GlobalController>().LostMinigame();
			}
			
			//Debug.Log( transform.rotation.z
			if( rigidbody2D.velocity.y > 0.0f && 
			   (transform.rotation.eulerAngles.z < 10 || transform.rotation.eulerAngles.z > 20) )
			{
				transform.Rotate( new Vector3( 0.0f, 0.0f, 2.5f ) );
			}
			if( rigidbody2D.velocity.y < 1.0f && 
			   (transform.rotation.eulerAngles.z > 340 || transform.rotation.eulerAngles.z < 330) )
			{	
				transform.Rotate( new Vector3( 0.0f, 0.0f, -1.5f ) );
			}
		}
	}

	void OnTriggerEnter2D( Collider2D coll )
	{
		if( "DartBoard" == coll.name && transform.position.x < 7.7f )
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
			// Position check is in case it hits the backed up pillars at end of map
			if( canControl && transform.position.x < 5.0f )
			{
				horizontalMoving = false;
				canControl = false;

				dartController.GetComponent<DartsController>().SlowDown();

				this.GetComponent<SpriteRenderer>().sprite = brokenDart;
			}
		}
	}

	public void StartGame()
	{
		gameStarted = true;
		rigidbody2D.gravityScale = 0.6f;
	}
}
