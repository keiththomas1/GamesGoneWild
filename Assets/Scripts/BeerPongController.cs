using UnityEngine;
using System.Collections;

public class BeerPongController : MonoBehaviour 
{
	bool isShooting;
	bool isFinished;

	public GameObject ballParent;
	public GameObject ball;
	Vector3 initialBallSize;
	Vector2 ballMovement;
	Vector2 downBallMovement;
	float ballShrinkRate = .98f;
	
	public GameObject slider;
	public GameObject slideBar;
	float slideBarActualLength;
	string sliderDirection;
	Vector2 rightSlide;
	Vector2 leftSlide;

	public GameObject cup;
	public GameObject goodJobText;

	// Use this for initialization
	void Start () 
	{
		isShooting = true;
		isFinished = false;
		goodJobText.renderer.enabled = false;

		initialBallSize = ball.transform.localScale;
		ballMovement = new Vector2( 0.0f, .1f );


		slideBarActualLength = slideBar.renderer.bounds.size.x * .9f; // HACK - This is just because the bar is curved.
		sliderDirection = "right";
		rightSlide = new Vector2( .1f, 0.0f );
		leftSlide = new Vector2( -.1f, 0.0f );

		ball.GetComponent<Animator>().enabled = false;
		//ball.GetComponentInChildren<Animator>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( isShooting )
		{
			if( "right" == sliderDirection )
			{
				ballParent.transform.Translate( rightSlide );
				slider.transform.Translate( rightSlide );

				if( slider.transform.position.x >= (slideBar.transform.position.x + slideBarActualLength/2) )
				{
					sliderDirection = "left";
				}
			}
			else // if "left" == sliderDirection
			{
				ballParent.transform.Translate( leftSlide );
				slider.transform.Translate( leftSlide );
				
				if( slider.transform.position.x <= (slideBar.transform.position.x - slideBarActualLength/2) )
				{
					sliderDirection = "right";
				}
			}

			if( Input.GetMouseButtonDown( 0 ) )
			{
				isShooting = false;
				ball.GetComponent<Animator>().enabled = true;
			}
		}
		else // If ball is in air
		{
			if( ball.transform.position.y > 4.0f ) // HACK - This just happens to be because current set-up
			{
				if( Mathf.Abs(slider.transform.position.x - cup.transform.position.x) < .4 )
				{
					goodJobText.renderer.enabled = true;
				}

				ball.GetComponent<Animator>().enabled = false;
				isFinished = true;
			}
		}

		if( isFinished )
		{
			if( Input.GetMouseButtonDown( 0 ) )
			{
				Application.LoadLevel( 0 );
			}
		}
		/*else  // If ball is in air 
		{
			if( ball.transform.localScale.x > initialBallSize.x/2 )
			{
				ball.transform.Translate( ballMovement );
				ball.transform.localScale = ball.transform.localScale * ballShrinkRate;
			}
			else
			{
				ball.transform.Translate( ballMovement );
				ball.transform.localScale = ball.transform.localScale * ballShrinkRate;
			}
		}*/
	}
}
