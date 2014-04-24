using UnityEngine;
using System.Collections;

public class FlipCupController : MonoBehaviour {
	public GameObject globalController;
	public GameObject Cup_placeholder;
	public GameObject Table_placeholder;
	Vector3 initPos;
	Vector3 finalPos;
	Vector3 Pos;
	Vector3 FlickPos = new Vector3(0,0,0);
	Vector3 FlickAmount = new Vector3(0,-80,0);
	//Vector3 startPosition = new Vector3(0,1,-2);

	public GameObject countdown;
	// How long until countdown is done
	float countdownTimer;

	// Whether the countdown has finished
	bool canStart;

	float totalCount = 0;
	float count = 0;
	bool isFlicked;
	
	public GameObject instructionText;
	public GameObject instructionTextShadow;
	// Variables for fading out the instructions
	float fadeTimer;
	Color colorStart;
	Color colorEnd;
	float fadeValue;

	// Use this for initialization
	void Start () {

		globalController = GameObject.Find( "Global Controller" );
		isFlicked= false;
		
		countdown.GetComponent<Animator>().speed = 1.4f;
		countdownTimer = 2.7f;
	
		canStart = false;
		
		// Fading instructions variables
		fadeTimer = 3.0f; // set duration time in seconds in the Inspector
		colorStart = instructionText.renderer.material.color;
		colorEnd = new Color( colorStart.r, colorStart.g, colorStart.b, 0.0f );
		fadeValue = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		//mouse down
		if (!isFlicked && canStart)
		{
			if (Input.GetMouseButtonDown (0)) 
			{
				//initPos = Input.mousePosition*10;
				initPos = Input.mousePosition;
				initPos.y *= 10;
				//initPos.x *= 2;

			}
			//swipes to mouse up and find the distance moved and apply force
			if (Input.GetMouseButtonUp (0)) 
				{
					//finalPos = Input.mousePosition*10;
				finalPos = Input.mousePosition;
				finalPos.y *= 8;
				Pos = finalPos - initPos;
				Pos.x *= 1.3f;

				//Reducing the max amount a cup can be flicked. Reduces frustration if flicked too hard.
				if( Pos.y > 1200.0f )
					Pos.y = 1200.0f;
				if( Pos.x > 450.0f )
					Pos.x = 450.0f;
				if( Pos.x < -450.0f )
					Pos.x = -450.0f;
				Debug.Log( "Flick vector: " + Pos );
				Cup_placeholder.rigidbody.AddForce(Pos);		//drag distance of the mouse as a force
				Cup_placeholder.rigidbody.AddForce(0,0,180);	//pushes cup from edge onto table
				Cup_placeholder.rigidbody.AddForceAtPosition(FlickAmount, FlickPos);// simulates the rotation of the cup
				isFlicked = true; //the cup has been flicked
			}
		}
		//if the balls y pos is in the landed area and is not changing, then success!
		if(isFlicked){
			//hardcoded....only way i found that worked..
			if (transform.position.y >= 2.2 && transform.position.y <= 2.4)
				count += 60.0f * Time.deltaTime;
			totalCount += 60.0f * Time.deltaTime;
		}

		if (count >= 50.0f){
			Debug.Log ("landed!!!!! Reloading level");
			isFlicked = false;
			count = 0;
			globalController.GetComponent<GlobalController>().BeatMinigame( 100 );
			//DestroyObject(Cup_placeholder);
			//Instantiate(Cup_placeholder,startPosition,transform.rotation);
			//Debug.Log (transform.position);
			//**********Once ball is instantiated.. it won't let me flick it again.
		}
		//Debug.Log(totalCount);
		if (totalCount >= 200.0f)
		{
			globalController.GetComponent<GlobalController>().LostMinigame();
			Debug.Log("Failed");
		}
		if (transform.position.y <0){
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
		else
		{
			if( fadeValue < 1.0f )
			{
				fadeTimer -= Time.deltaTime;
				fadeValue += Time.deltaTime;
				instructionText.renderer.material.color = Color.Lerp( colorStart, colorEnd, fadeValue/1.0f );
				instructionTextShadow.renderer.material.color = Color.Lerp( colorStart, colorEnd, fadeValue/1.0f );
				
				if( fadeValue >= 1.0f )
				{
					Destroy( instructionText );
					Destroy( instructionTextShadow );
				}
			}
		}
	}
}	

