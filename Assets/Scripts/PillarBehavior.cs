using UnityEngine;
using System.Collections;

public class PillarBehavior : MonoBehaviour 
{
	bool isMoving;

	Vector2 speedVector;

	// Use this for initialization
	void Start () 
	{
		transform.Translate( new Vector2( 0.0f, (Random.value*4)-2 ) );

		speedVector = new Vector2( -.06f, 0.0f );
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( isMoving )
		{
			transform.Translate( speedVector );
		}
	}

	public void StartMoving()
	{
		isMoving = true;
	}
}
