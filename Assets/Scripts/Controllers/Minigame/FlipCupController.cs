using UnityEngine;
using System.Collections;

public class FlipCupController : MonoBehaviour {
	public GameObject globalController;
	public GameObject Cup_placeholder;
	public GameObject Table_placeholder;
	Vector3 initPos;
	Vector3 finalPos;
	Vector3 Pos;
	Vector3 FlickPos;
	Vector3 FlickAmount;
	//Vector3 startPosition = new Vector3(0,1,-2);

	public GameObject countdown;
	// How long until countdown is done
	float countdownTimer;

	// Whether the countdown has finished
	bool canStart;

	float totalCount = 0;
	float count = 0;
	bool isFlicked;
	bool makeSound;
	int makeSoundFlag = 0;
	
	public GameObject instructionText;
	// Variables for fading out the instructions
	float fadeTimer;
	Color colorStart;
	Color colorEnd;
	float fadeValue;
	// Cup SFX
	public GameObject CupSFX;
	public GameObject FlippyTable;
	public GameObject FlippyCup;

	// Use this for initialization
	void Start () {

		globalController = GameObject.Find( "Global Controller" );
		isFlicked= false;
		makeSound = false;
		
		countdown.GetComponent<Animator>().speed = 1.4f;
		countdownTimer = 2.7f;
	
		canStart = false;
		
		// Fading instructions variables
		fadeTimer = 3.0f; // set duration time in seconds in the Inspector
		colorStart = instructionText.renderer.material.color;
		colorEnd = new Color( colorStart.r, colorStart.g, colorStart.b, 0.0f );
		fadeValue = 0.0f;
		
		FlickPos = new Vector3(0,0,0);
		FlickAmount = new Vector3(0,-150,0);
	}
	
	// Update is called once per frame
	void Update () {
		if (!isFlicked && canStart)
		{
			if ( Input.GetMouseButtonDown(0) ) 
			{
				initPos = Input.mousePosition;
			}
			//swipes to mouse up and find the distance moved and apply force
			if ( Input.GetMouseButtonUp(0) ) 
			{
					//finalPos = Input.mousePosition*10;
				finalPos = Input.mousePosition;
				CupSFX.GetComponent<AudioSource>().Play();
				//finalPos.y *= 10;
				Pos = finalPos - initPos;
				Pos.y *=2.5f;
			    Pos.x *= 1.3f;

				//Reducing the max amount a cup can be flicked. Reduces frustration if flicked too hard.
				if( Pos.y > 1200.0f )
					Pos.y = 1200.0f;
				if( Pos.x > 450.0f )
					Pos.x = 450.0f;
				if( Pos.x < -450.0f )
					Pos.x = -450.0f;

				// Forces to add to cup
				Cup_placeholder.rigidbody.AddForce(Pos);		//drag distance of the mouse as a force
				Cup_placeholder.rigidbody.AddForce(0,0,300);	//pushes cup from edge onto table
				Cup_placeholder.rigidbody.AddForceAtPosition(FlickAmount, FlickPos);// simulates the rotation of the cup

				isFlicked = true; //the cup has been flicked
			}
		}
		//if the balls y pos is in the landed area and is not changing, then success!
		if(isFlicked)
		{
			// HACK - hard-coded values
			if (transform.position.y <= 1.4f && transform.position.y >= 1.0f
			    && transform.rotation.eulerAngles.y > 170 && transform.rotation.eulerAngles.y < 200
			    && transform.rotation.eulerAngles.z > 170 && transform.rotation.eulerAngles.z < 200)
			{
				count += 60.0f * Time.deltaTime;
			}
			totalCount += 60.0f * Time.deltaTime;
		}

		if (count >= 50.0f)
		{
			isFlicked = false;
			count = 0;
			if( globalController )
				globalController.GetComponent<GlobalController>().BeatMinigame( 100 );
			else
				Debug.Log( "Winner!" );
		}

		// If the counter is "up" and cup has not landed, lose minigame
		if (totalCount >= 200.0f)
		{
			if( globalController )
				globalController.GetComponent<GlobalController>().LostMinigame();
			else
				Debug.Log("Failed");
		}
		// If cup falls off table, lose game
		if (transform.position.y < -2.0f)
		{
			globalController.GetComponent<GlobalController>().LostMinigame();
			DestroyObject(Cup_placeholder);
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
		
				if( fadeValue >= 1.0f )
				{
					Destroy( instructionText );
				}
			}
		}
	}

	// Check to see if Flippy Cup is hitting Flippy Table
	void OnCollisionStay( Collision coll)
	{
		makeSound = true;
		if (coll.gameObject.name == "FlippyTable")
		{
			if (isFlicked) 
			{
				if (makeSound && makeSoundFlag < 2) 
				{
					makeSoundFlag++;
					if( !CupSFX.GetComponent<AudioSource>().isPlaying)
						CupSFX.GetComponent<AudioSource> ().Play ();
				}
			}
		}
		makeSound = false;
	}
}	
