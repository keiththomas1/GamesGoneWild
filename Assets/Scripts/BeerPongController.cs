using UnityEngine;
using System.Collections;

public class BeerPongController : MonoBehaviour 
{
	bool isShooting;
	public GameObject ball;
	Vector3 initialBallSize;
	Vector2 upBallMovement;
	Vector2 downBallMovement;
	float ballShrinkRate = .98f;
	
	public GameObject slider;
	public GameObject slideBar;
	float slideBarActualLength;
	string sliderDirection;
	Vector2 rightSlide;
	Vector2 leftSlide;

	// Use this for initialization
	void Start () 
	{
		isShooting = true;
		initialBallSize = ball.transform.localScale;

		slideBarActualLength = slideBar.renderer.bounds.size.x * .9f;
		sliderDirection = "right";
		rightSlide = new Vector2( .1f, 0.0f );
		leftSlide = new Vector2( -.1f, 0.0f );
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( isShooting )
		{
			if( "right" == sliderDirection )
			{
				ball.transform.Translate( rightSlide );
				slider.transform.Translate( rightSlide );

				if( slider.transform.position.x >= (slideBar.transform.position.x + slideBarActualLength/2) )
				{
					sliderDirection = "left";
				}
			}
			else // if "left" == sliderDirection
			{
				ball.transform.Translate( leftSlide );
				slider.transform.Translate( leftSlide );
				
				if( slider.transform.position.x <= (slideBar.transform.position.x - slideBarActualLength/2) )
				{
					sliderDirection = "right";
				}
			}

			if( Input.GetMouseButtonDown( 0 ) )
			{
				float xDifferential = (slider.transform.position.x - slideBar.transform.position.x) * .01f;
				upBallMovement = new Vector2( xDifferential, .1f );
				downBallMovement = new Vector2( xDifferential, -.1f );

				isShooting = false;
			}
		}
		else  // If ball is in air 
		{
			if( ball.transform.localScale.x > initialBallSize.x/2 )
			{
				ball.transform.Translate( upBallMovement );
				ball.transform.localScale = ball.transform.localScale * ballShrinkRate;
			}
			else
			{
				ball.transform.Translate( downBallMovement );
				ball.transform.localScale = ball.transform.localScale * ballShrinkRate;
			}
		}
	}
}
