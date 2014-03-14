using UnityEngine;
using System.Collections;

public class BeerPongController : MonoBehaviour 
{
	public GameObject globalController;

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
	public GameObject goodJobText;

	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find( "Global Controller" );

		isShootingHorizontal = true;
		isShootingVertical = false;
		isFinished = false;
		goodJobText.renderer.enabled = false;

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
		if( isShootingHorizontal )
		{
			if( "right" == sliderDirection )
			{
				ballParent.transform.Translate( rightSlide );
				sliderHorizontal.transform.Translate( rightSlide );

				if( sliderHorizontal.transform.position.x >= (slideBarHorizontal.transform.position.x + slideBarHorizontalActualLength/2) )
				{
					sliderDirection = "left";
				}
			}
			else // if "left" == sliderDirection
			{
				ballParent.transform.Translate( leftSlide );
				sliderHorizontal.transform.Translate( leftSlide );
				
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
				sliderVertical.transform.Translate( rightSlide );
				
				if( sliderVertical.transform.position.y >= (slideBarVertical.transform.position.y + slideBarVerticalActualLength/2) )
				{
					sliderDirection = "down";
				}
			}
			else // if "down" == sliderDirection
			{
				sliderVertical.transform.Translate( leftSlide );
				
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

		BallMovement();
		TickTimers();
	}

	void BallMovement()
	{
		if( !isShootingVertical && !isShootingHorizontal && !isFinished ) // Then ball should be in air
		{
			ballParent.transform.Translate( ballMovement );

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

				if( CheckBallInCup() )
				{
					goodJobText.renderer.enabled = true;
				}
			}
		}
	}

	bool CheckBallInCup()
	{
		// if( globalController.cups[0] = true )
		
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
						globalController.GetComponent<GlobalController>().CupsPlaced[i] = false;
						cups[i].animation.Play();
						cupAnimTimerStart = true;
						cupIndex = i;
						Destroy( ball );
						return true;
					}
				}
			}
			globalController.GetComponent<GlobalController>().LostMinigame();
		}
		else
		{
			Debug.Log( "No global controller" );
		}
			
		return false;
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
				globalController.GetComponent<GlobalController>().BeatMinigame();
			}
		}
	}
}
