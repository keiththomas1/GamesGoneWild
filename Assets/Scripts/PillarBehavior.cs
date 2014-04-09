using UnityEngine;
using System.Collections;

public class PillarBehavior : MonoBehaviour 
{
	bool isMoving;

	Vector2 speedVector;

	// Use this for initialization
	void Start () 
	{
		transform.Translate( new Vector2( 0.0f, (Random.value*6)-3 ) );

		speedVector = new Vector2( -.07f, 0.0f );
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
		isMoving = true;
	}

	public void SlowDown()
	{
		speedVector = new Vector2( -.03f, 0.0f );
	}
}
