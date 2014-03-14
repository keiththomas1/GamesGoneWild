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
		speedVector = new Vector2( -.06f, 0.0f );
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
		}
	}
	
	public void StartMoving()
	{
		isMoving = true;
	}
}
