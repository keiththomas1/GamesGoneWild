using UnityEngine;
using System.Collections;

public class DartBoardBehavior : MonoBehaviour 
{
	public GameObject controller;
	bool horizontalMoving;
	bool verticalMoving;
	bool movingUpwards;

	float speed;
	Vector2 horizontalVector;
	Vector2 verticalVector;
	
	// Use this for initialization
	void Start () 
	{
		transform.Translate( new Vector2( 0.0f, (Random.value*4)-2 ) );

		horizontalMoving = false;
		verticalMoving = true;
		movingUpwards = true;

		verticalVector = new Vector2( 0.0f, .06f );
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( horizontalMoving )
		{
			transform.Translate( horizontalVector * 60.0f * Time.deltaTime );
		}
		if( verticalMoving )
		{
			if( movingUpwards )
			{
				transform.Translate( verticalVector * 60.0f * Time.deltaTime );

				if( transform.position.y > 2.1f )
				{
					movingUpwards = false;
				}
			}
			else
			{
				transform.Translate( -verticalVector * 60.0f * Time.deltaTime );
				
				if( transform.position.y < -2.1f )
				{
					movingUpwards = true;
				}
			}
		}
		if( transform.position.x < 7.0f )
		{
			controller.GetComponent<DartsController>().StartMovingDart();

			horizontalMoving = false;
			
			controller.GetComponent<DartsController>().dartFlying = false;

			Vector3 tempPos = transform.position;
			tempPos.x = 7.0f;
			transform.position = tempPos;
		}
	}
	
	public void StartMoving()
	{
		speed = controller.GetComponent<DartsController>().roomSpeed;
		horizontalVector = new Vector2( -speed, 0.0f );
		horizontalMoving = true;
	}

	public void StopVertical()
	{
		verticalMoving = false;
	}
	
	public void SlowDown()
	{
		horizontalVector = new Vector2( -speed/2, 0.0f );
	}
}
