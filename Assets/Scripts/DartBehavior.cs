using UnityEngine;
using System.Collections;

public class DartBehavior : MonoBehaviour 
{
	GameObject globalController;
	public GameObject dartController;
	
	Vector2 dartJumpHeight;
	float dartJumpConstant;

	public bool dartMoving;
	Vector2 speedVector;

	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find("Global Controller");

		dartJumpConstant = 200.0f;

		dartMoving = false;
		speedVector = new Vector2( .06f, 0.0f );
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( Input.GetMouseButtonDown( 0 ) )
		{
			dartJumpHeight = new Vector2( 0.0f, (rigidbody2D.velocity.y*-50.0f) + dartJumpConstant );
			rigidbody2D.AddForce( dartJumpHeight );
		}

		if( dartMoving )
		{
			transform.Translate( speedVector );
		}
	}
}
