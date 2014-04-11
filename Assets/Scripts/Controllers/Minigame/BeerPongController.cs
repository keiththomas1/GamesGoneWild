using UnityEngine;
using System.Collections;

public class BeerPongController : MonoBehaviour 
{
	public GameObject globalController;
	bool gameOver;
	
	bool isShootingHorizontal;
	bool isShootingVertical;
	bool isFinished;
	
	public GameObject ballParent;
	public GameObject ball;
	//Vector3 initialBallSize;
	Vector2 ballMovement;
	Vector2 downBallMovement;
	float ballGrowRate = 1.02f;
	float ballShrinkRate = .96f;
	
	public GameObject sliderHorizontal;
	public GameObject sliderVertical;
	public GameObject slideBarHorizontal;
	public GameObject slideBarVertical;
	float slideBarHorizontalActualLength;
	float slideBarVerticalActualLength;
	string sliderDirection;
	Vector2 rightSlide;
	Vector2 leftSlide;
	
	public GameObject[] cups;
	bool cupAnimTimerStart;
	float cupAnimTimer;
	int cupIndex;

	public GameObject instructionText;
	public GameObject descriptionText;
	int partyPoints;
	
	// Sound Effects
	public GameObject inCupSFX;
	
	public GameObject countdown;
	float gameStartTimer;
	bool gameStarted;

	// Variables for fading out the instructions
	float fadeTimer;
	Color colorStart;
	Color colorEnd;
	float fadeValue;
	
	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find( "Global Controller" );
		
		isShootingHorizontal = true;
		isShootingVertical = false;
		isFinished = false;
		descriptionText.renderer.enabled = false;
		
		//initialBallSize = ball.transform.localScale;
		ballMovement = new Vector2( 0.0f, .1f );
		
		slideBarHorizontalActualLength = slideBarHorizontal.renderer.bounds.size.x * .9f; // HACK - This is just because the bar is curved.
		slideBarVerticalActualLength = slideBarVertical.renderer.bounds.size.y * .85f; // HACK - This is just because the bar is curved.
		sliderDirection = "right";
		rightSlide = new Vector2( .1f, 0.0f );
		leftSlide = new Vector2( -.1f, 0.0f );
		
		ball.GetComponent<Animator>().enabled = false;
		
		if( globalController )
		{
			for(int i=0; i<cups.Length; i++)
			{
				if( !globalController.GetComponent<GlobalController>().CupsPlaced[i] )
				{
					Destroy( cups[i] );
				}
			}
		}
		cupAnimTimerStart = false;
		cupAnimTimer = 1.0f;

		countdown.GetComponent<Animator>().speed = 1.4f;
		gameStartTimer = 2.7f;
		gameStarted = false;

		gameOver = false;

		partyPoints = 0;

		// Fading instructions variables
		fadeTimer = 3.0f; // set duration time in seconds in the Inspector
		colorStart = instructionText.renderer.material.color;
		colorEnd = new Color( colorStart.r, colorStart.g, colorStart.b, 0.0f );
		fadeValue = 0.0f;
		
		RandomizeSliderStartPosition();
	}
	
	void RandomizeSliderStartPosition()
	{
		Vector3 tempSliderPosition = sliderHorizontal.transform.position;
		Vector3 tempBallPosition = ballParent.transform.position;
		
		float randomX = Mathf.Round( Random.value * slideBarHorizontalActualLength ) + 
			( slideBarHorizontal.transform.position.x - slideBarHorizontalActualLength/2 );
		tempSliderPosition.x = randomX;
		tempBallPosition.x = randomX;
		
		sliderHorizontal.transform.position = tempSliderPosition;
		ballParent.transform.position = tempBallPosition;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( !gameOver && gameStarted )
		{
			if( isShootingHorizontal )
			{
				if( "right" == sliderDirection )
				{
					ballParent.transform.Translate( rightSlide * 60.0f * Time.deltaTime );
					sliderHorizontal.transform.Translate( rightSlide * 60.0f * Time.deltaTime );
					
					if( sliderHorizontal.transform.position.x >= (slideBarHorizontal.transform.position.x + slideBarHorizontalActualLength/2) )
					{
						sliderDirection = "left";
					}
				}
				else // if "left" == sliderDirection
				{
					ballParent.transform.Translate( leftSlide * 60.0f * Time.deltaTime );
					sliderHorizontal.transform.Translate( leftSlide * 60.0f * Time.deltaTime );
					
					if( sliderHorizontal.transform.position.x <= (slideBarHorizontal.transform.position.x - slideBarHorizontalActualLength/2) )
					{
						sliderDirection = "right";
					}
				}
				
				if( Input.GetMouseButtonDown( 0 ) )
				{
					isShootingHorizontal = false;
					isShootingVertical = true;
					sliderDirection = "up";
					//ball.GetComponent<Animator>().enabled = true;
				}
			}
			else if( isShootingVertical )
			{
				if( "up" == sliderDirection )
				{
					sliderVertical.transform.Translate( rightSlide * 60.0f * Time.deltaTime );
					
					if( sliderVertical.transform.position.y >= (slideBarVertical.transform.position.y + slideBarVerticalActualLength/2) )
					{
						sliderDirection = "down";
					}
				}
				else // if "down" == sliderDirection
				{
					sliderVertical.transform.Translate( leftSlide * 60.0f * Time.deltaTime );
					
					if( sliderVertical.transform.position.y <= (slideBarVertical.transform.position.y - slideBarVerticalActualLength/2) )
					{
						sliderDirection = "up";
					}
				}
				
				if( Input.GetMouseButtonDown( 0 ) )
				{
					isShootingVertical = false;
				}
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
			
			BallMovement();
			TickTimers();
		}

		if( !gameStarted )
		{
			gameStartTimer -= Time.deltaTime;
			if( gameStartTimer <= 0.0f )
			{
				Destroy( countdown );
				gameStarted = true;
			}
		}
	}
	
	void BallMovement()
	{
		if( !isShootingVertical && !isShootingHorizontal && !isFinished ) // Then ball should be in air
		{
			ballParent.transform.Translate( ballMovement * 60.0f * Time.deltaTime );
			
			if( ball.transform.position.y < 
			   (sliderVertical.transform.position.y - (sliderVertical.transform.position.y - sliderHorizontal.transform.position.y)/2 ) )
			{
				ball.transform.localScale = ball.transform.localScale * ballGrowRate;
			}
			else
			{
				ball.transform.localScale = ball.transform.localScale * ballShrinkRate;
			}
			
			if( ball.transform.position.y > sliderVertical.transform.position.y )
			{
				isFinished = true;

				int finishType = CheckBallInCup();
				if( finishType != 0 )	// != 0
				{
					switch( finishType )
					{
					case 1:
						descriptionText.GetComponent<TextMesh>().text = "Island!";
						partyPoints = 120;
						break;
					case 2:
						descriptionText.GetComponent<TextMesh>().text = "Freshman Cup!";
						partyPoints = 60;
						break;
					case 3:
						descriptionText.GetComponent<TextMesh>().text = "Water Cup!";
						partyPoints = 10;
						break;
					case 4:
						descriptionText.GetComponent<TextMesh>().text = "Nice Shot!";
						partyPoints = 100;
						break;
					}
					descriptionText.renderer.enabled = true;
				}
			}
		}
	}

	// Returns 1 if island, 2 if freshman cup, 3 if water cup, 4 if other
	int CheckBallInCup()
	{
		if( globalController )
		{
			for( int i=0; i<10; i++ )
			{
				if( globalController.GetComponent<GlobalController>().CupsPlaced[i] )
				{
					if( ballParent.transform.position.x >= (cups[i].transform.position.x - cups[i].renderer.bounds.size.x/2)
					   && ballParent.transform.position.x <= (cups[i].transform.position.x + cups[i].renderer.bounds.size.x/2)
					   && ballParent.transform.position.y >= (cups[i].transform.position.y + cups[i].renderer.bounds.size.y/6)
					   && ballParent.transform.position.y <= (cups[i].transform.position.y + cups[i].renderer.bounds.size.y/2) )
					{	
						// Play ball in cup sound
						inCupSFX.GetComponent<AudioSource>().Play();
						// Remove cup in global controller
						globalController.GetComponent<GlobalController>().CupsPlaced[i] = false;
						// Play "moving" animation
						cups[i].animation.Play();
						// Start timer until animation is over
						cupAnimTimerStart = true;
						// Keep current cup for future use
						cupIndex = i;
						
						// Kill off the ball
						Destroy( ball );

						if( isIsland( cupIndex ) )
							return 1;
						if( isFreshman( cupIndex ) )
							return 2;
						// if cupIndex == 10, don't think this is implemented yet
						//   return 3
						return 4;
					}
				}
			}
		}
						             
		// Kill off the ball
		Destroy( ball );
		globalController.GetComponent<GlobalController>().LostMinigame();
		
		return 0;
	}
			
	void TickTimers()
	{
		if( cupAnimTimerStart )
		{
			cupAnimTimer -= Time.deltaTime;
			
			if( cupAnimTimer <= 0.0f )
			{
				// Try to fade it out eventually
				Destroy( cups[cupIndex] );
				globalController.GetComponent<GlobalController>().BeatMinigame( partyPoints );
				gameOver = true;
			}
		}
	}

	bool isIsland( int index )
	{
		switch( index )
		{
		case 0:
			if( !globalController.GetComponent<GlobalController>().CupsPlaced[1] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[4] )
				return true;
			break;
		case 1:
			if( !globalController.GetComponent<GlobalController>().CupsPlaced[0] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[2] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[4] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[5] )
				return true;
			break;
		case 2:
			if( !globalController.GetComponent<GlobalController>().CupsPlaced[1] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[3] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[5] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[6] )
				return true;
			break;
		case 3:
			if( !globalController.GetComponent<GlobalController>().CupsPlaced[2] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[6] )
				return true;
			break;
		case 4:
			if( !globalController.GetComponent<GlobalController>().CupsPlaced[0] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[1] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[5] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[7] )
				return true;
			break;
		case 5:
			if( !globalController.GetComponent<GlobalController>().CupsPlaced[1] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[2] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[4] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[6] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[7] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[8] )
				return true;
			break;
		case 6:
			if( !globalController.GetComponent<GlobalController>().CupsPlaced[2] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[3] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[5] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[8] )
				return true;
			break;
		case 7:
			if( !globalController.GetComponent<GlobalController>().CupsPlaced[4] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[5] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[8] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[9] )
				return true;
			break;
		case 8:
			if( !globalController.GetComponent<GlobalController>().CupsPlaced[5] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[6] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[7] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[9] )
				return true;
			break;
		case 9:
			if( !globalController.GetComponent<GlobalController>().CupsPlaced[7] &&
			   !globalController.GetComponent<GlobalController>().CupsPlaced[8] )
				return true;
			break;
		}

		return false;
	}

	bool isFreshman( int index )
	{
		if( index == 5 )
		{
			if( globalController.GetComponent<GlobalController>().CupsPlaced[0] &&
			   globalController.GetComponent<GlobalController>().CupsPlaced[1] &&
			   globalController.GetComponent<GlobalController>().CupsPlaced[2] &&
			   globalController.GetComponent<GlobalController>().CupsPlaced[3] &&
			   globalController.GetComponent<GlobalController>().CupsPlaced[4] &&
			   globalController.GetComponent<GlobalController>().CupsPlaced[6] &&
			   globalController.GetComponent<GlobalController>().CupsPlaced[7] &&
			   globalController.GetComponent<GlobalController>().CupsPlaced[8] &&
			   globalController.GetComponent<GlobalController>().CupsPlaced[9] )
				return true;
		}

		return false;
	}
}
