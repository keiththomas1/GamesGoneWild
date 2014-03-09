using UnityEngine;
using System.Collections;

public class BagsController : MonoBehaviour 
{
	Vector3 initPos;
	Vector3 finalPos;

	// Use this for initialization
	void Start () 
	{
		initPos = new Vector3(0.0f, 0.0f, 0.0f);
		finalPos = new Vector3(0.0f, 0.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{

		if ( Input.GetMouseButtonDown (0) ) 
		{
			initPos = Input.mousePosition;
		}

		if ( Input.GetMouseButtonUp (0) ) 
		{
			/*finalPos = Input.mousePosition;
			Pos = finalPos - initPos;
			
			Cup_placeholder.rigidbody.AddForce(Pos);		//drag distance of the mouse as a force
			Cup_placeholder.rigidbody.AddForce(0,0,300);	//pushes cup from edge onto table
			Cup_placeholder.rigidbody.AddForceAtPosition(FlickAmmount,FlickPos); // simulates the rotation of the cup
			*/
		}
	}
}
