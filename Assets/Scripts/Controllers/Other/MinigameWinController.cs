﻿using UnityEngine;
using System.Collections;

public class MinigameWinController : MonoBehaviour 
{
	GameObject globalController;
	public GameObject pointsBox;
	public GameObject pointsText;

	int points;

	// Timer till the scene ends
	float timer;

	// Timer/bool for "tapping" through this screen
	float clickThroughTimer;
	bool canClickThrough;
	
	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find( "Global Controller" );

		if( globalController.GetComponent<GlobalController>().previousMode == "FlippyCup" )
		{ // If in flippy cup, we need to rotate the points box to face the screen.
			Debug.Log( "Special case points box." );
			pointsBox.transform.Rotate( new Vector3( 35.0f, 0.0f, 0.0f ) );
			pointsBox.transform.Translate( new Vector3( 0.0f, 2.38f, 0.0f ) );
		}

		points = globalController.GetComponent<GlobalController>().partyPoints;

		pointsText.GetComponent<TextMesh>().text = points.ToString();

		timer = 2.0f;

		clickThroughTimer = 0.5f;
		canClickThrough = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer -= Time.deltaTime;
		
		if( timer <= 0.0f )
		{
			globalController.GetComponent<GlobalController>().NextMinigame();
		}

		if( canClickThrough && Input.GetMouseButtonDown(0) )
		{
			globalController.GetComponent<GlobalController>().NextMinigame();
		}
	}
}
