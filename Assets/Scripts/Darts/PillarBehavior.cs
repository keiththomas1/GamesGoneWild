using UnityEngine;
using System.Collections;

public class PillarBehavior : MonoBehaviour 
{
	public GameObject controller;
	bool isMoving;

	float speed;
	Vector2 speedVector;

	// Use this for initialization
	void Start () 
	{
		transform.Translate( new Vector2( 0.0f, (Random.value*6)-3 ) );
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( isMoving )
		{
			transform.Translate( speedVector * 60.0f * Time.deltaTime );
		}
	}

	public void StartMoving()
	{
		speed = controller.GetComponent<DartsController>().roomSpeed;
		speedVector = new Vector2( -speed, 0.0f );
		isMoving = true;
	}

	public void SlowDown()
	{
		speedVector = new Vector2( -speed/2, 0.0f );
	}
}
