using UnityEngine;
using System.Collections;

public class DrunkKeyCharacter : MonoBehaviour 
{
	public float speed;
	Vector2 speedVector;
	bool walking;

	// Use this for initialization
	void Start () 
	{
		walking = false;
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( walking )
		{
			transform.Translate( speedVector );

			if( transform.position.y < -9.0f )
			{
				Vector3 tempPosition = transform.position;
				tempPosition.y = 7.0f;
				transform.position = tempPosition;
				walking = false;
			}
		}
	}

	public void StartWalking()
	{
		walking = true;
		speedVector = new Vector2( 0.0f, -speed );
	}
}
