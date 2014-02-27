using UnityEngine;
using System.Collections;

public class BeerPongController : MonoBehaviour 
{
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

	public GameObject cup;
	public GameObject goodJobText;

	// Use this for initialization
	void Start () 
	{
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
		//ball.GetComponentInChildren<Animator>().enabled = false;

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

				//ball.GetComponent<Animator>().enabled = true;
			}
		}

		if( isFinished )
		{
			if( Input.GetMouseButtonDown( 0 ) )
			{
				Application.LoadLevel( 0 );
			}
		}

		BallMovement();
	}

	void BallMovement()
	{
		if( !isShootingVertical && !isShootingHorizontal && !isFinished ) // Then ball should be in air
		{
			ball.transform.Translate( ballMovement );

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
			}
		}
	}
}
