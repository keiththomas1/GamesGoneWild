using UnityEngine;
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
	float speed;
	Vector2 speedVector;

	Vector3 startPosition;
	bool gameStarted;
	public Sprite brokenDart;

	public GameObject descriptionText;
	bool descriptionTextGrowing;
	float textGrowthTimer;
	float growthTimerRate; // A constantly decreasing number to make the growth exponential

	public GameObject dartBoard;
	
	int partyPoints;

	// Dart's SFX
	public GameObject DartBreakSFX;
	public GameObject TapDartSFX;
	public GameObject DartBoardHitSFX;
	
	public GameObject scoreText;
	public GameObject scoreTextBack;

	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find("Global Controller");

		gameOver = false;
		if( globalController )
			dartJumpConstant = 230.0f + (globalController.GetComponent<GlobalController>().dartLevel * 20.0f);
		else
			dartJumpConstant = 250.0f;
		canJump = true;

		canControl = true;
		horizontalMoving = false;

		startPosition = transform.position;
		gameStarted = false;

		descriptionText.renderer.enabled = false;
		descriptionTextGrowing = false;
		growthTimerRate = .06f;
		
		scoreText.GetComponent<Animator>().enabled = false;
		scoreTextBack.renderer.enabled = false;
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
						TapDartSFX.GetComponent<AudioSource>().Play ();

						canJump = false;
					}
				}

				if( !canJump )
				{
					dartJumpTimer -= Time.deltaTime;
					if( dartJumpTimer <= 0.0f )
					{
						canJump = true;
						dartJumpTimer = .18f;
					}
				}
				
				if( horizontalMoving )
				{
					transform.Translate( speedVector * 60.0f * Time.deltaTime );
				}
				
				if( transform.position.y < -6.0f || transform.position.y > 5.4 || transform.position.x > 13.0f )
				{
					globalController.GetComponent<GlobalController>().LostMinigame();
				}

				if( rigidbody2D.velocity.y > 1.0f && 
				   (transform.rotation.eulerAngles.z < 8 || transform.rotation.eulerAngles.z > 20) )
				{
					transform.Rotate( new Vector3( 0.0f, 0.0f, 2.5f * 60.0f * Time.deltaTime ) );
				}
				else if( rigidbody2D.velocity.y < 1.0f && 
				   (transform.rotation.eulerAngles.z > 340 || transform.rotation.eulerAngles.z < 330) )
				{	
					transform.Rotate( new Vector3( 0.0f, 0.0f, -0.5f * 60.0f * Time.deltaTime ) );
				}
			}
		}
		
		
		// Handles the "explosion" animation of the description text
		if( descriptionTextGrowing )
		{
			textGrowthTimer -= Time.deltaTime;
			
			if( textGrowthTimer <= 0.0f )
			{
				descriptionText.GetComponent<TextMesh>().fontSize = descriptionText.GetComponent<TextMesh>().fontSize + 3;
				growthTimerRate -= .004f * 60.0f * Time.deltaTime;
				textGrowthTimer = growthTimerRate;
			}
			if( descriptionText.GetComponent<TextMesh>().fontSize > 110 )
			{
				descriptionTextGrowing = false;
				if( globalController.GetComponent<GlobalController>().dartLevel < 4 )
				{
					globalController.GetComponent<GlobalController>().dartLevel++;
				}

				globalController.GetComponent<GlobalController>().BeatMinigame( partyPoints );
			}
		}
	}

	void OnTriggerEnter2D( Collider2D coll )
	{
		Debug.Log( "Collision with " + coll.ToString() + " and position = " + transform.position.x.ToString() );
		if( !gameOver )
		{
			if( "DartBoard" == coll.name && transform.position.x < 7.2f )
			{
				horizontalMoving = false;
				canControl = false;
				
				rigidbody2D.velocity = new Vector2( 0.0f, 0.0f );
				rigidbody2D.gravityScale = 0.0f;

				dartBoard.GetComponent<DartBoardBehavior>().StopVertical();
				
				gameOver = true;
				dartController.GetComponent<DartsController>().gameOver = true;

				// SFX first
				DartBoardHitSFX.GetComponent<AudioSource>().Play ();
				// Figure out the points that the user earned
				float dartBoardY = coll.transform.position.y;
				float dartPosition = transform.position.y - dartBoardY;

				if( dartPosition <= .25 && dartPosition >= -.4 )
				{
					partyPoints = 110;
					descriptionText.GetComponent<TextMesh>().text = "Bullseye!";
				}
				else if( dartPosition <= 1.55 && dartPosition >= -1.8 )
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
				descriptionText.GetComponent<TextMesh>().fontSize = 1;
				descriptionTextGrowing = true;

				//scoreText.transform.Translate( 1.2f, transform.transform.position.y+1.0f, 0.0f );
				scoreTextBack.GetComponent<TextMesh>().text = "+" + partyPoints.ToString();
				scoreTextBack.renderer.enabled = true;
				scoreText.GetComponent<Animator>().enabled = true;
			}
			else
			{
				// Position check is in case it hits the backed up pillars at end of map
				if( canControl && transform.position.x < 5.0f )
				{
					horizontalMoving = false;
					canControl = false;
					DartBreakSFX.GetComponent<AudioSource>().Play ();
					
					dartController.GetComponent<DartsController>().SlowDown();
					
					this.GetComponent<SpriteRenderer>().sprite = brokenDart;
				}
			}
		}
	}

	public void StartMoving()
	{
		speed = dartController.GetComponent<DartsController>().roomSpeed;
		speedVector = new Vector2( speed, 0.0f );
		horizontalMoving = true;
	}

	public void StartGame()
	{
		gameStarted = true;
		if( globalController )
			rigidbody2D.gravityScale = 1.2f + (globalController.GetComponent<GlobalController>().dartLevel * .05f);
		else
			rigidbody2D.gravityScale = 1.25f;
	}
}
