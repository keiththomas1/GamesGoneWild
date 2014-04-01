﻿using UnityEngine;
using System.Collections;
//public Transform pivot;

public class AutoRotate : MonoBehaviour {
	public GameObject globalController;

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
		if( gameStarted )
		{
			time -= Time.deltaTime;
			if (time < 0)
			{
				if( globalController )
					globalController.GetComponent<GlobalController>().BeatMinigame();
				else
					GuiTextDebug.debug ("you won");
			}
			else
			{
				float curSpeed = Time.deltaTime * speed;
				if ((Input.acceleration.x - acc_x) != 0) 
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

	void OnGUI()
	{
		if (time > 0.0)
		{
			GUI.Box (new Rect (20, 20, 30, 20), "" + time.ToString ("0.0"));
		}
		else
		{
			GUI.Box (new Rect (20, 20, 30, 20), "" + "0.0" );
		}
	}
}


