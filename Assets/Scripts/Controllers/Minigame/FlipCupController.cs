using UnityEngine;
using System.Collections;

public class FlipCupController : MonoBehaviour 
{
	public GameObject globalController;
	public GameObject Cup_placeholder;
	public GameObject Table_placeholder;
	Vector3 initPos;
	Vector3 finalPos;
	Vector3 Pos;
	Vector3 FlickPos = new Vector3(0,0,0);
	Vector3 FlickAmmount = new Vector3(0,-70,0);
	Vector3 startPosition = new Vector3(0,1,-2);

	public GameObject countdown;
	// How long until countdown is done
	float countdownTimer = 3.5f;
	// Whether the countdown has finished
	bool canStart;

	public float massCenter;

	int totalCount = 0;
	int landedCount = 0;
	bool isFlicked;

	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find( "Global Controller" );
		isFlicked= false;
		rigidbody.centerOfMass = new Vector3 (0, massCenter, 0);
	
		canStart = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//mouse down
		if (!isFlicked && canStart)
		{
			if (Input.GetMouseButtonDown (0)) 
			{
				initPos = Input.mousePosition;
			}
			//swipes to mouse up and find the distance moved and apply force
			if (Input.GetMouseButtonUp (0)) 
			{
				finalPos = Input.mousePosition;
				if( finalPos.y > initPos.y ) // else it isn't a correct flip (flipping downwards)
				{
					Pos = finalPos - initPos;
					Pos.y *= 10;
					Pos.x *= 1.3f;
					
					Debug.Log("True Pos: " + Pos);
					
					//Reducing the max amount a cup can be flicked. Reduces frustration if flicked too hard.
					if( Pos.y > 1200.0f )
						Pos.y = 1200.0f;
					if( Pos.x > 450.0f )
						Pos.x = 450.0f;
					if( Pos.x < -450.0f )
						Pos.x = -450.0f;
					Debug.Log( "Flick vector: " + Pos );
					
					Cup_placeholder.rigidbody.AddForce(Pos);		//drag distance of the mouse as a force
					Cup_placeholder.rigidbody.AddForce(0,0,200);	//pushes cup from edge onto table
					Cup_placeholder.rigidbody.AddForceAtPosition(FlickAmmount,FlickPos);// simulates the rotation of the cup
					isFlicked = true; //the cup has been flicked
				}
			}
		}

		//if the balls y pos is in the landed area and is not changing, then success!
		if(isFlicked)
		{
			//hardcoded....only way i found that worked..
			if (transform.position.y >= 1.129990 && transform.position.y <= 13.0006)
				landedCount++;
			totalCount++;
		}

		if (landedCount == 200)
		{
			Debug.Log ("landed!!!!! Reloading level");
			isFlicked = false;
			landedCount = 0;
			if( globalController )
				globalController.GetComponent<GlobalController>().BeatMinigame();
			//DestroyObject(Cup_placeholder);
			//Instantiate(Cup_placeholder,startPosition,transform.rotation);
			Debug.Log (transform.position);
			//**********Once ball is instantiated.. it won't let me flick it again.
		}

		if (totalCount >= 350)
		{
			if( globalController )
				globalController.GetComponent<GlobalController>().LostMinigame();
			Debug.Log("Failed");
		}
		if (transform.position.y <0){
			if( globalController )
				globalController.GetComponent<GlobalController>().LostMinigame();
			DestroyObject(Cup_placeholder);
			//Instantiate(Cup_placeholder,startPosition,transform.rotation);
		}

		if( countdown )
		{
			countdownTimer -= Time.deltaTime;
			if( countdownTimer <= 0.0f )
			{
				Destroy( countdown );
				canStart = true;
			}
		}
	}
}	

