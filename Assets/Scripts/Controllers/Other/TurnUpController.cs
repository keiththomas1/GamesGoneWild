using UnityEngine;
using System.Collections;

public class TurnUpController : MonoBehaviour 
{
	public GameObject globalController;

	public GameObject fasterOne;
	public GameObject fasterTwo;
	public GameObject fasterThree;
	public GameObject fasterOneShadow;
	public GameObject fasterTwoShadow;
	public GameObject fasterThreeShadow;

	int count;

	float timerSpeed;
	float changeTextTimer;


	float transitionTimer; // When to switch screens

	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find("Global Controller");
		if( globalController )
		{
			timerSpeed = .9f - (.2f * globalController.GetComponent<GlobalController>().turnUpLevel);
			// This makes the transition end the first time after the first rotation, second after the second, etc.
			transitionTimer = timerSpeed * 3 * globalController.GetComponent<GlobalController>().turnUpLevel;
		}
		else
		{
			timerSpeed = .9f;
			transitionTimer = timerSpeed * 3;
		}

		count = 1;
		
		fasterOne.renderer.enabled = true;
		fasterTwo.renderer.enabled = false;
		fasterThree.renderer.enabled = false;

		changeTextTimer = timerSpeed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Change text timer stuff
		changeTextTimer -= Time.deltaTime;
		if( changeTextTimer <= 0.0f )
		{
			count++;
			if( count == 4 ) { count = 1; }
			switch( count )
			{
			case 1:
				fasterOne.renderer.enabled = true;
				fasterTwo.renderer.enabled = false;
				fasterThree.renderer.enabled = false;
				break;
			case 2:
				fasterOne.renderer.enabled = false;
				fasterTwo.renderer.enabled = true;
				fasterThree.renderer.enabled = false;
				break;
			case 3:
				fasterOne.renderer.enabled = false;
				fasterTwo.renderer.enabled = false;
				fasterThree.renderer.enabled = true;
				break;
			}

			changeTextTimer = timerSpeed;
		}

		// Transition timer stuff
		transitionTimer -= Time.deltaTime;
		if( transitionTimer <= 0.0f )
		{
			if( globalController )
			{
				globalController.GetComponent<GlobalController>().NextMinigame();
			}
			else
			{
				Debug.Log( "Transition" );
			}
		}
	}
}
