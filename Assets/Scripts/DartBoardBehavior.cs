using UnityEngine;
using System.Collections;

public class DartBoardBehavior : MonoBehaviour 
{
	public GameObject controller;
	bool isMoving;
	
	Vector2 speedVector;
	
	// Use this for initialization
	void Start () 
	{
		transform.Translate( new Vector2( 0.0f, (Random.value*4)-2 ) );

		speedVector = new Vector2( -.08f, 0.0f );
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( isMoving )
		{
			transform.Translate( speedVector * 60.0f * Time.deltaTime );
		}
		if( transform.position.x < 7.0f )
		{
			controller.GetComponent<DartsController>().StartMovingDart();

			isMoving = false;
			
			controller.GetComponent<DartsController>().dartFlying = false;

			Vector3 tempPos = transform.position;
			tempPos.x = 7.6f;
			transform.position = tempPos;
		}
	}
	
	public void StartMoving()
	{
		isMoving = true;
	}
	
	public void SlowDown()
	{
		speedVector = new Vector2( -.03f, 0.0f );
	}
}
