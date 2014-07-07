using UnityEngine;
using System.Collections;

public class CupBehavior : MonoBehaviour 
{
	public GameObject globalController;
	public GameObject flippyController;
	public GameObject Cup_placeholder;
	public GameObject Table_placeholder;
	Vector3 initPos;
	Vector3 finalPos;
	Vector3 Pos;
	Vector3 FlickPos;
	Vector3 FlickAmount;
	public int flippyLevel;
	//Vector3 startPosition = new Vector3(0,1,-2);

	public GameObject countdown;
	// How long until countdown is done
	float countdownTimer;

	// Whether the countdown has finished
	bool canStart;
	bool startedSwipe;
	bool gameOver;
	int countTimer;
	int countLevel;

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


	// For tracking if you should flip or not. (In border?)
	public GameObject Border;
	RaycastHit hit;
	Ray ray;

	// Use this for initialization
	void Start () {

		globalController = GameObject.Find( "Global Controller" );
		flippyLevel = globalController.GetComponent<GlobalController> ().flippyCupLevel;
		isFlicked= false;
		makeSound = false;

		rigidbody.centerOfMass = new Vector3( 0.0f, 1.3f, 0.0f );
		
		countdown.GetComponent<Animator>().speed = 1.4f;
		countdownTimer = 2.7f;
	
		canStart = false;
		startedSwipe = false;
		gameOver = false;
		
		// Fading instructions variables
		fadeTimer = 3.0f; // set duration time in seconds in the Inspector
		colorStart = instructionText.renderer.material.color;
		colorEnd = new Color( colorStart.r, colorStart.g, colorStart.b, 0.0f );
		fadeValue = 0.0f;
		
		FlickPos = new Vector3(transform.position.x, 0.0f, 0.0f);
		FlickAmount = new Vector3(0,-300,0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( !gameOver )
		{
			if (!isFlicked && canStart)
			{
				if ( Input.GetMouseButtonDown(0) ) 
				{
					ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					if( Physics.Raycast(ray,out hit))
					{
						Debug.Log( hit.collider.name );
						if( hit.collider.name == Border.name )
						{
							initPos = Input.mousePosition;
							startedSwipe = true;
						}
					}
				}
				//swipes to mouse up and find the distance moved and apply force
				if ( Input.GetMouseButtonUp(0) && startedSwipe) 
				{
					//finalPos = Input.mousePosition*10;
					finalPos = Input.mousePosition;
					CupSFX.GetComponent<AudioSource>().Play();
					//finalPos.y *= 10;
					Pos = finalPos - initPos;
					Pos.y *= 4.0f;
					Pos.x = 0.0f;
					
					//Reducing the max amount a cup can be flicked. Reduces frustration if flicked too hard.
					if( Pos.y > 650.0f )
						Pos.y = 650.0f;
					
					// Forces to add to cup
					Cup_placeholder.rigidbody.AddForce(Pos);		//drag distance of the mouse as a force
					Cup_placeholder.rigidbody.AddForce(0,0,370);	//pushes cup from edge onto table
					Cup_placeholder.rigidbody.AddForceAtPosition(FlickAmount, FlickPos);// simulates the rotation of the cup
					
					isFlicked = true; //the cup has been flicked
				}
			}
			//if the balls y pos is in the landed area and is not changing, then success!
			if(isFlicked)
			{
				totalCount += 60.0f * Time.deltaTime;
				Debug.Log ("TotalTime: " + totalCount);
				// HACK - hard-coded values
				if (transform.position.y <= 1.4f && transform.position.y >= 1.0f
				    )
				{
					count += 60.0f * Time.deltaTime;
					Debug.Log( "InLandedArea: " + count );
				}
			}


			if (flippyLevel == 1)
				countTimer = 180;
				//countLevel = 60;}
			else if (flippyLevel == 2)
				countTimer = 200;
			//	countLevel = 40;}
			else if (flippyLevel ==3)
				countTimer = 200;
				//countLevel = 40;}	
			else 
				countTimer = 200;
				//countLevel = 40;}


			
			if (count >= 65.0f)
			{
				isFlicked = false;
				count = 0;
				gameOver = true;
				flippyController.GetComponent<FlippyCupController>().CupLanded();
				Debug.Log ("CUP DOWN");
			}
			
			// If the counter is "up" and cup has not landed, lose minigame
			if (totalCount >= countTimer)
			{
				gameOver = true;
				flippyController.GetComponent<FlippyCupController>().CupFellOver();
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
