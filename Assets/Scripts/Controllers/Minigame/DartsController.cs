using UnityEngine;
using System.Collections;

public class DartsController : MonoBehaviour 
{
	GameObject globalController;
	public GameObject dart;
	public bool dartFlying;
	public bool dartMoving;
	public bool gameOver;

	public GameObject countdown;
	float gameStartTimer;
	bool gameStarted;

	public GameObject floor;
	Vector2 floorSpeed;

	public GameObject[] pillars;
	float pillarTimer;
	int pillarCount;
	int currentPillar;

	public GameObject board;
	
	// Variables for fading out the instructions
	public GameObject instructionText;
	float fadeTimer;
	Color colorStart;
	Color colorEnd;
	float fadeValue;

	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find("Global Controller");
		
		countdown.GetComponent<Animator>().speed = 1.4f;
		gameStartTimer = 2.7f;
		gameStarted = false;
		gameOver = false;

		dartFlying = true;
		dartMoving = false;

		floorSpeed = new Vector2( -.07f, 0.0f );

		pillarTimer = 2.0f;
		currentPillar = 0;
		if( globalController )
			pillarCount = globalController.GetComponent<GlobalController>().dartLevel + 3;
		else
			pillarCount = 5;
		
		// Fading instructions variables
		fadeTimer = 3.0f; // set duration time in seconds in the Inspector
		colorStart = instructionText.renderer.material.color;
		colorEnd = new Color( colorStart.r, colorStart.g, colorStart.b, 0.0f );
		fadeValue = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( !gameOver )
		{
			if( gameStarted )
			{
				if( dartFlying )
				{
					PillarUpdate();
					FloorUpdate();
				}
				
				if( fadeValue < 1.0f )
				{
					fadeTimer -= Time.deltaTime;
					fadeValue += Time.deltaTime;
					instructionText.renderer.material.color = Color.Lerp( colorStart, colorEnd, fadeValue/1.0f );
					
					if( fadeValue >= 1.0f )
					{
						Destroy( instructionText );
					}
				}
			}
			else
			{
				gameStartTimer -= Time.deltaTime;
				if( gameStartTimer <= 0.0f )
				{
					gameStarted = true;
					dart.GetComponent<DartBehavior>().StartGame();
					Destroy( countdown );
				}
			}
		}
	}
	
	void PillarUpdate()
	{
		pillarTimer -= Time.deltaTime;

		// Start moving the next pillar when this stops
		if( pillarTimer <= 0.0f )
		{	
			if( currentPillar < (pillarCount-1) )	// Still moving pillars
			{
				pillars[currentPillar].GetComponent<PillarBehavior>().StartMoving();
			}
			else if( currentPillar == (pillarCount-1) )	// Move the dartboard
			{
				board.GetComponent<DartBoardBehavior>().StartMoving();
			}

			if( currentPillar == (pillarCount-2) )
			{
				pillarTimer = 4.0f;
			}
			else
			{
				pillarTimer = 2.0f;
			}

			currentPillar++;
		}
	}

	void FloorUpdate()
	{
		floor.transform.Translate( floorSpeed * 60.0f * Time.deltaTime );
	}

	public void StartMovingDart()
	{
		dartMoving = true;
		dart.GetComponent<DartBehavior>().horizontalMoving = true;
	}

	public void SlowDown()
	{
		for( int i=0; i<pillars.Length; i++ )
		{
			pillars[i].GetComponent<PillarBehavior>().SlowDown();
		}
		board.GetComponent<DartBoardBehavior>().SlowDown();
		floorSpeed = new Vector2( -.03f, 0.0f );
	}
}
