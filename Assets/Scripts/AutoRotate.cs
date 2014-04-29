using UnityEngine;
using System.Collections;
//public Transform pivot;

public class AutoRotate : MonoBehaviour {
	public GameObject globalController;
	bool gameOver;

	public float accspeed = 100.0f;
	float accx;
	float acc_x;
	float speed_program;
	int speed = -25; // regular speed
	string output;
	//string output1;
	string straccx;
	float time = 6.0f;

	float randomSpeed = 13.0f;
	
	float countdownTimer;
	bool gameStarted;

	void Start () 
	{
		globalController = GameObject.Find( "Global Controller" );
		gameOver = false;
				//HingeJoint2D  hinge = GetComponent<HingeJoint2D>();
				//JointMotor2D motor = hinge.motor;
				//The character falls randomly left or right
				//hinge.motor = motor;	
		acc_x = Input.acceleration.x; // saving for reference start
		
		countdownTimer = 3.5f;
		gameStarted = false;
	}	
	
	
	// Update is calle		d once per frame
	void Update () 
	{
		if( !gameOver )
		{
			if( gameStarted )
			{
				time -= Time.deltaTime;
				if (time < 0)
				{
					if( globalController )
					{
						gameOver = true;
						this.GetComponent<Auto>().gameOver = true;
						globalController.GetComponent<GlobalController>().BeatMinigame( 100 );
					}
					else
						Debug.Log("You won");
				}
				else
				{
					float curSpeed = Time.deltaTime * speed;
					if ((Input.acceleration.x - acc_x) != 0) // If player is tilting the phone
					{
						accx = (Input.acceleration.x * randomSpeed) * curSpeed;
						transform.Rotate (0, 0, accx);
					}
				}
				
				// This is a random modifer added to the user control so that it's slightly unpredicable.
				randomSpeed += (Random.value * 4) - 2;
				if( randomSpeed < 10.0f )
					randomSpeed = 10.0f;
				if( randomSpeed > 15.0f )
					randomSpeed = 15.0f;
			}
			else
			{
				countdownTimer -= Time.deltaTime;
				if( countdownTimer <= 0.0f )
				{
					gameStarted = true;
				}
			}
		}
	}
}



