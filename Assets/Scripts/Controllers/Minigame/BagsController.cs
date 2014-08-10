using UnityEngine;
using System.Collections;

public class BagsController : MonoBehaviour 
{
	public GameObject bag;
	public GameObject bagBoard;

	bool hasBeenTossed;

	Vector3 initPos;
	Vector3 finalPos;
	float distanceToBoard;

	Vector3 throwVector;

	// Use this for initialization
	void Start () 
	{
		hasBeenTossed = false;

		initPos = new Vector3(0.0f, 0.0f, 0.0f);
		finalPos = new Vector3(0.0f, 0.0f, 0.0f);
		distanceToBoard = bagBoard.transform.position.y - bag.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Start and end of the 'flick' to throw the bag.
		if( !hasBeenTossed )
		{
			if ( Input.GetMouseButtonDown (0) ) 
			{
				initPos = Input.mousePosition;
			}
			if ( Input.GetMouseButtonUp (0) ) 
			{
				finalPos = Input.mousePosition;
				throwVector =(finalPos - initPos);
				throwVector.Normalize();
				throwVector *= .1f;
				hasBeenTossed = true;
			}
		}
		else
		{
			bag.transform.Translate( throwVector );
			
			if( bag.transform.position.y < ( bagBoard.transform.position.y - distanceToBoard )/2 )
			{
				bag.transform.localScale = bag.transform.localScale * 1.02f;
			}
			else
			{
				bag.transform.localScale = bag.transform.localScale * .97f;
			}
		}
	}
}
