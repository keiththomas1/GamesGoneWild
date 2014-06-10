using UnityEngine;
using System.Collections;

public class MinigameWinController : MonoBehaviour 
{
	GameObject globalController;
	public GameObject pointsBox;
	public GameObject pointsText;

	int oldPoints;
	int points;

	// Timer till the scene ends
	float timer;

	// Timer/bool for "tapping" through this screen
	float clickThroughTimer;
	bool canClickThrough;

	// Timer for "counting" up the points
	float countingTimer;
	bool isCountingUp;
	
	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find( "Global Controller" );

		if( globalController.GetComponent<GlobalController>().previousMode == "FlippyCup" )
		{ // If in flippy cup, we need to rotate the points box to face the screen.
			Debug.Log( "Special case points box." );
			pointsBox.transform.Rotate( new Vector3( 25.0f, 0.0f, 0.0f ) );
			pointsBox.transform.Translate( new Vector3( 0.0f, 2.38f, 0.0f ) );
		}

		oldPoints = globalController.GetComponent<GlobalController>().oldPartyPoints;
		points = globalController.GetComponent<GlobalController>().partyPoints;

		pointsText.GetComponent<TextMesh>().text = oldPoints.ToString();

		timer = 2.0f;

		clickThroughTimer = 0.7f;
		canClickThrough = false;

		// HACK - do this after points get "dragged"
		StartCounting();
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer -= Time.deltaTime;
		clickThroughTimer -= Time.deltaTime;
		
		if( timer <= 0.0f )
		{
			globalController.GetComponent<GlobalController>().NextMinigame();
		}

		if( clickThroughTimer <= 0.0f )
		{
			canClickThrough = true;
		}

		if( canClickThrough && Input.GetMouseButtonDown(0) )
		{
			globalController.GetComponent<GlobalController>().NextMinigame();
		}

		if( isCountingUp && oldPoints<points)
		{
			countingTimer -= Time.deltaTime;

			if( countingTimer <= 0.0f )
			{
				countingTimer = .01f;

				oldPoints += 2;
				pointsText.GetComponent<TextMesh>().text = oldPoints.ToString();
			}
		}
	}

	void StartCounting()
	{
		countingTimer = .01f;
		isCountingUp = true;
	}
}
