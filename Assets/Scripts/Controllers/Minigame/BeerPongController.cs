﻿using UnityEngine;
using System.Collections;

public class BeerPongController : MonoBehaviour 
{
	public GameObject globalController;
	bool gameOver;
	
	bool isShootingHorizontal;
	bool isShootingVertical;
	bool isFinished;

	// Concerning bouncing when cup missed.
	bool isBouncing;
	Vector3 bounceStartPosition;
	
	public GameObject ballParent;
	public GameObject ball;
	//Vector3 initialBallSize;
	Vector2 ballMovement;
	Vector2 downBallMovement;
	float ballGrowRate = 1.03f;
	float ballShrinkRate = .92f;
	
	public GameObject sliderHorizontal;
	public GameObject sliderVertical;
	public GameObject slideBarHorizontal;
	public GameObject slideBarVertical;
	float slideBarHorizontalActualLength;
	float slideBarVerticalActualLength;
	string sliderDirection;
	Vector2 rightSlide;
	Vector2 leftSlide;
	float sliderMultiplier;
	
	public GameObject[] cups;
	bool cupAnimTimerStart;
	float cupAnimTimer;
	int cupIndex;
	
	public GameObject descriptionText;
	public GameObject descriptionTextShadow;
	bool descriptionTextGrowing;
	float textGrowthTimer;
	float growthTimerRate; // A constantly decreasing number to make the growth exponential

	public GameObject instructionText;
	public GameObject instructionTextShadow;
	int partyPoints;
	
	// Sound Effects
	public GameObject inCupSFX;
	public GameObject MadeCupSFX;
	
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

		isBouncing = false;

		descriptionText.renderer.enabled = false;
		descriptionTextShadow.renderer.enabled = false;
		descriptionTextGrowing = false;
		growthTimerRate = .06f;
		
		slideBarHorizontalActualLength = slideBarHorizontal.renderer.bounds.size.x * .9f; // HACK - This is just because the bar is curved.
		slideBarVerticalActualLength = slideBarVertical.renderer.bounds.size.y * .85f; // HACK - This is just because the bar is curved.
		sliderDirection = "right";

		if( globalController )
			sliderMultiplier = globalController.GetComponent<GlobalController>().beerPongLevel * .01f;
		else
			sliderMultiplier = .01f;
		ballMovement = new Vector2( 0.0f, .1f + sliderMultiplier );
		rightSlide = new Vector2( .1f + sliderMultiplier, 0.0f );
		leftSlide = new Vector2( -.1f - sliderMultiplier, 0.0f );
		
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
		// HACK - implement pause functionality here.
		if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }

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
				instructionTextShadow.renderer.material.color = Color.Lerp( colorStart, colorEnd, fadeValue/1.0f );

				if( fadeValue >= 1.0f )
				{
					Destroy( instructionText );
					Destroy( instructionTextShadow );
				}
			}

			// Handles the "explosion" animation of the description text
			if( descriptionTextGrowing )
			{
				textGrowthTimer -= Time.deltaTime;

				if( textGrowthTimer <= 0.0f )
				{
					descriptionText.GetComponent<TextMesh>().fontSize = descriptionText.GetComponent<TextMesh>().fontSize + 3;
					descriptionTextShadow.GetComponent<TextMesh>().fontSize = descriptionTextShadow.GetComponent<TextMesh>().fontSize + 3;
					growthTimerRate -= .004f * 60.0f * Time.deltaTime;
					textGrowthTimer = growthTimerRate;
				}
				if( descriptionText.GetComponent<TextMesh>().fontSize > 110 )
				{
					descriptionTextGrowing = false;
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
			if( !isBouncing )
			{
				ballParent.transform.Translate( ballMovement * 60.0f * Time.deltaTime );
				
				// If ball is still on the "up" trajectory
				if( ball.transform.position.y < 
				   (sliderVertical.transform.position.y - (sliderVertical.transform.position.y - sliderHorizontal.transform.position.y)/3 ) )
				{
					ball.transform.localScale = ball.transform.localScale * ballGrowRate;
				}
				else 	// If ball is falling down
				{
					ball.transform.localScale = ball.transform.localScale * ballShrinkRate;
				}
				
				if( ball.transform.position.y > (sliderVertical.transform.position.y+.1f) )
				{
					int finishType = CheckBallInCup();
					if( finishType != 0 )
					{
						isFinished = true;

						switch( finishType )
						{
						case 1:
							descriptionText.GetComponent<TextMesh>().text = "Island!";
							descriptionTextShadow.GetComponent<TextMesh>().text = "Island!";
							partyPoints = 120;
							break;
						case 2:
							descriptionText.GetComponent<TextMesh>().text = "Freshman Cup!";
							descriptionTextShadow.GetComponent<TextMesh>().text = "Freshman Cup!";
							partyPoints = 60;
							break;
						case 3:
							descriptionText.GetComponent<TextMesh>().text = "Water Cup!";
							descriptionTextShadow.GetComponent<TextMesh>().text = "Water Cup!";
							partyPoints = 10;
							break;
						case 4:
							descriptionText.GetComponent<TextMesh>().text = "Nice Shot!";
							descriptionTextShadow.GetComponent<TextMesh>().text = "Nice Shot!";
							MadeCupSFX.GetComponent<AudioSource>().Play();
							partyPoints = 100;
							break;
						}
						
						descriptionText.renderer.enabled = true;
						descriptionTextShadow.renderer.enabled = true;
						descriptionText.GetComponent<TextMesh>().fontSize = 1;
						descriptionTextShadow.GetComponent<TextMesh>().fontSize = 1;
						descriptionTextGrowing = true;
					}
					else
					{
						isFinished = false;
						isBouncing = true;
						bounceStartPosition = ball.transform.position;
					}
				}
			}
			else 	// If in the process of bouncing away from the table. HACK - hardcoded values.
			{
				Debug.Log( "here" );
				ballParent.transform.Translate( ballMovement * 30.0f * Time.deltaTime );
				
				// If ball is still on the "up" trajectory
				if( ball.transform.position.y < 3.95f )
				{
					ball.transform.localScale = ball.transform.localScale * ballGrowRate;
				}
				else 	// If ball is falling down
				{
					ball.transform.localScale = ball.transform.localScale * ballShrinkRate;
				}

				
				
				if( ball.transform.position.y > 5.0f )
				{
					isFinished = true;
				
					// Kill off the ball
					Destroy( ball );
					if( globalController )
						globalController.GetComponent<GlobalController>().LostMinigame();
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
					   && ballParent.transform.position.y >= (cups[i].transform.position.y + cups[i].renderer.bounds.size.y/7)
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
				if( globalController )
				{
					globalController.GetComponent<GlobalController>().beerPongLevel++;
					globalController.GetComponent<GlobalController>().BeatMinigame( partyPoints );
				}
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
