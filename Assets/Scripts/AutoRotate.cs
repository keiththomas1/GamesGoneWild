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

	float randomSpeed = 13.0f;
	
	float countdownTimer;
	bool gameStarted;
	
	public GameObject scoreText;
	public GameObject scoreTextFront;
	public GameObject scoreTextBack;
	
	public GameObject timerFront;
	public GameObject timerBack;
	bool timerStarted;
	Vector3 timerSpeed;

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
		
		scoreText.GetComponent<Animator>().enabled = false;
		scoreTextFront.renderer.enabled = false;
		scoreTextBack.renderer.enabled = false;
		
		if( globalController )
		{
			timerFront = globalController.GetComponent<GlobalController>().timerFront;
			timerBack = globalController.GetComponent<GlobalController>().timerBack;
			
			timerFront.renderer.enabled = true;
			timerBack.renderer.enabled = true;
		}
		timerStarted = false;
		timerSpeed = new Vector3( -.08f, 0.0f, 0.0f );
	}	
	
	
	// Update is calle		d once per frame
	void Update () 
	{
		if( !gameOver )
		{
			if( gameStarted )
			{
				float curSpeed = Time.deltaTime * speed;
				if ((Input.acceleration.x - acc_x) != 0) // If player is tilting the phone
				{
					accx = (Input.acceleration.x * randomSpeed) * curSpeed;
					transform.Rotate (0, 0, accx);
				}
				
				// This is a random modifer added to the user control so that it's slightly unpredicable.
				randomSpeed += (Random.value * 4) - 2;
				if( randomSpeed < 10.0f )
					randomSpeed = 10.0f;
				if( randomSpeed > 15.0f )
					randomSpeed = 15.0f;
				
				if( timerStarted )
				{
					timerFront.transform.Translate( timerSpeed * Time.deltaTime * 60.0f);
					
					if( timerFront.transform.position.x < -20.0f )
					{
						gameOver = true;
						this.GetComponent<Auto>().gameOver = true;
						
						scoreTextFront.GetComponent<TextMesh>().text = "+ 100";
						scoreTextBack.GetComponent<TextMesh>().text = "+ 100";
						scoreTextFront.renderer.enabled = true;
						scoreTextBack.renderer.enabled = true;
						scoreText.GetComponent<Animator>().enabled = true;
						
						globalController.GetComponent<GlobalController>().BeatMinigame( 100 );
					}
				}
			}
			else
			{
				countdownTimer -= Time.deltaTime;
				if( countdownTimer <= 0.0f )
				{
					gameStarted = true;
					timerStarted = true;
				}
			}
		}
	}
}



