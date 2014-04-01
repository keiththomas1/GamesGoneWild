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
		speedVector = new Vector2( -.07f, 0.0f );
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( isMoving )
		{
			transform.Translate( speedVector );
		}
		if( transform.position.x < 7.6f )
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
