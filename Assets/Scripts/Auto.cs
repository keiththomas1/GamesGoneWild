using UnityEngine;
using System.Collections;

public class Auto : MonoBehaviour 
{
	public GameObject globalController;
	public bool gameOver;

	//float autoacc;
	float speed;
	float modifier;
	float angle;
	//string strSpeed;
	//float Fallspeed_1 = 3.0f;
	//float Fallspeed_2 = 2.0f;

	float curSpeed;
	Vector3 localAngle;
	//string stroutput1;

	public GameObject countdown;
	float countdownTimer;
	public bool gameStarted;
	
	// Variables for fading out the instructions
	public GameObject instructionText;
	float fadeTimer;
	Color colorStart;
	Color colorEnd;
	float fadeValue;

	// Game SFX
	public GameObject HurtSFX;

	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find( "Global Controller" );
		gameOver = false;

		// random start can f
		if (Random.value > .5) 
		{
			modifier = 1.0f;

		}
		else
		{
			modifier = -1.0f;
		}

		speed = 1.0f;
		
		countdown.GetComponent<Animator>().speed = 1.4f;
		countdownTimer = 2.7f;
		gameStarted = false;
		
		// Fading instructions variables
		fadeTimer = 3.0f; // set duration time in seconds in the Inspector
		colorStart = instructionText.renderer.material.color;
		colorEnd = new Color( colorStart.r, colorStart.g, colorStart.b, 0.0f );
		fadeValue = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( !gameOver )
		{
			if( gameStarted )
			{// Create the angle. Range is -180 to 180.
				localAngle = transform.localEulerAngles;
				if( localAngle.z > 180 )
					localAngle.z = localAngle.z - 360;
				
				// Check if the person has lost.
				if( localAngle.z > 75 || localAngle.z < -75 )
				{
					HurtSFX.GetComponent<AudioSource>().Play ();
					globalController.GetComponent<GlobalController>().LostMinigame();

				}
				
				// If person is at top of the rotation
				if( (localAngle.z > -15 && localAngle.z <= 0)
				   || (localAngle.z < 15 && localAngle.z >= 0) )
				{
					curSpeed = (Time.deltaTime * speed * modifier * 20);
				}
				else // If not at top of rotation
				{
					curSpeed = (Time.deltaTime * speed * (localAngle.z));
				}
				
				// Create a max limit for the curSpeed
				if( curSpeed > 3.0f )
					curSpeed = 3.0f;
				if( curSpeed < -3.0f )
					curSpeed = -3.0f;
				
				// Perform the rotation and increase the speed.
				transform.Rotate (0, 0, curSpeed);
				speed += .007f;
				
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
			else
			{
				countdownTimer -= Time.deltaTime;
				if( countdownTimer <= 0.0f )
				{
					Destroy( countdown );
					gameStarted = true;
				}
			}
		}
	}
}
