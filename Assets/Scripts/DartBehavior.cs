﻿using UnityEngine;
using System.Collections;

public class DartBehavior : MonoBehaviour 
{
	GameObject globalController;
	public GameObject dartController;
	bool gameOver;
	
	Vector2 dartJumpHeight;
	float dartJumpConstant;
	float dartJumpTimer;
	bool canJump;

	bool canControl;
	public bool horizontalMoving;
	Vector2 speedVector;

	Vector3 startPosition;
	bool gameStarted;
	public Sprite brokenDart;

	public GameObject descriptionText;

	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find("Global Controller");

		gameOver = false;
		dartJumpConstant = 200.0f;
		canJump = true;

		canControl = true;
		horizontalMoving = false;
		speedVector = new Vector2( .08f, 0.0f );

		startPosition = transform.position;
		gameStarted = false;

		descriptionText.renderer.enabled = false;
	}

	// Update is called once per frame
	void Update () 
	{
		if( !gameOver )
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
					if( Input.GetMouseButtonDown( 0 ) && canJump )
					{
						dartJumpHeight = new Vector2( 0.0f, (rigidbody2D.velocity.y*-50.0f) + dartJumpConstant );
						rigidbody2D.AddForce( dartJumpHeight );

						canJump = false;
					}
				}

				if( !canJump )
				{
					dartJumpTimer -= Time.deltaTime;
					if( dartJumpTimer <= 0.0f )
					{
						canJump = true;
						dartJumpTimer = .2f;
					}
				}
				
				if( horizontalMoving )
				{
					transform.Translate( speedVector * 60.0f * Time.deltaTime );
				}
				
				if( transform.position.y < -6.0f || transform.position.x > 13.0f )
				{
					globalController.GetComponent<GlobalController>().LostMinigame();
				}

				if( rigidbody2D.velocity.y > 0.0f && 
				   (transform.rotation.eulerAngles.z < 10 || transform.rotation.eulerAngles.z > 20) )
				{
					transform.Rotate( new Vector3( 0.0f, 0.0f, 2.5f * 60.0f * Time.deltaTime ) );
				}
				if( rigidbody2D.velocity.y < 1.0f && 
				   (transform.rotation.eulerAngles.z > 340 || transform.rotation.eulerAngles.z < 330) )
				{	
					transform.Rotate( new Vector3( 0.0f, 0.0f, -0.5f * 60.0f * Time.deltaTime ) );
				}
			}
		}
	}

	void OnTriggerEnter2D( Collider2D coll )
	{
		Debug.Log( "Collision with " + coll.ToString() + " and game over = " + gameOver.ToString() );
		if( !gameOver )
		{
			if( "DartBoard" == coll.name && transform.position.x < 7.7f )
			{
				horizontalMoving = false;
				canControl = false;
				
				rigidbody2D.velocity = new Vector2( 0.0f, 0.0f );
				rigidbody2D.gravityScale = 0.0f;
				
				gameOver = true;
				dartController.GetComponent<DartsController>().gameOver = true;

				// Figure out the points that the user earned
				float dartBoardY = coll.transform.position.y;
				float dartPosition = transform.position.y - dartBoardY;
				int partyPoints;
				Debug.Log(dartPosition);
				if( dartPosition <= .45 && dartPosition >= -.3 )
				{
					partyPoints = 110;
					descriptionText.GetComponent<TextMesh>().text = "Bullseye!";
				}
				else if( dartPosition <= 1.75 && dartPosition >= -1.7 )
				{
					partyPoints = 80;
					descriptionText.GetComponent<TextMesh>().text = "Great Throw!";
				}
				else
				{
					partyPoints = 60;
					descriptionText.GetComponent<TextMesh>().text = "Nice!";
				}
				descriptionText.renderer.enabled = true;
				
				globalController.GetComponent<GlobalController>().dartLevel++;
				globalController.GetComponent<GlobalController>().BeatMinigame( partyPoints );
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
	}

	public void StartGame()
	{
		gameStarted = true;
		rigidbody2D.gravityScale = 0.6f;
	}
}
