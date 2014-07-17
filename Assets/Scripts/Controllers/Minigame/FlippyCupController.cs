using UnityEngine;
using System.Collections;

public class FlippyCupController : MonoBehaviour 
{
	public GameObject globalController;

	public GameObject cupOne;
	public GameObject cupTwo;
	public GameObject cupThree;
	public GameObject cupFour;

	public GameObject cupOneBorder;
	public GameObject cupTwoBorder;
	public GameObject cupThreeBorder;
	public GameObject cupFourBorder;

	int cupsToLand;
	int cupsToFlick;

	public GameObject scoreText;
	public GameObject scoreTextFront;
	public GameObject scoreTextBack;

	public GameObject countdown;
	// How long until countdown is done
	float countdownTimer;
	
	public GameObject timerFront;
	public GameObject timerBack;
	bool timerStarted;
	Vector3 timerSpeed;

	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find( "Global Controller" );

		if( globalController )
		{
			cupsToLand = globalController.GetComponent<GlobalController>().flippyCupLevel;
			cupsToFlick = globalController.GetComponent<GlobalController>().flippyCupLevel;
		}
		else
		{
			cupsToLand = 4;
			cupsToFlick = 4;
		}

		scoreText.GetComponent<Animator>().enabled = false;
		scoreTextFront.renderer.enabled = false;
		scoreTextBack.renderer.enabled = false;

		// Cup logic
		switch( cupsToLand )
		{
		case 1:
			// Cups
			cupOne.transform.Translate( 1.6f, 0.0f, 0.0f );
			Destroy( cupTwo );
			Destroy( cupThree );
			Destroy( cupFour );
			// Borders
			cupOneBorder.transform.Translate( 1.6f, 0.0f, 0.0f );
			Destroy( cupTwoBorder );
			Destroy( cupThreeBorder );
			Destroy( cupFourBorder );
			break;
		case 2:
			Destroy( cupThree );
			Destroy( cupFour );
			// Borders
			Destroy( cupThreeBorder );
			Destroy( cupFourBorder );
			break;
		case 3:
			cupOne.transform.Translate( 1.6f, 0.0f, 0.0f );
			cupTwo.transform.Translate( 1.6f, 0.0f, 0.0f );
			cupThree.transform.Translate( 1.6f, 0.0f, 0.0f );
			Destroy( cupFour );
			// Borders
			cupOneBorder.transform.Translate( 1.6f, 0.0f, 0.0f );
			cupTwoBorder.transform.Translate( 1.6f, 0.0f, 0.0f );
			cupThreeBorder.transform.Translate( 1.6f, 0.0f, 0.0f );
			Destroy( cupFourBorder );
			break;
		case 4:
			// Don't really have to do anything.
			break;
		}

		countdown.GetComponent<Animator>().speed = 1.4f;
		countdownTimer = 2.7f;
		
		if( globalController )
		{
			timerFront = globalController.GetComponent<GlobalController>().timerFront;
			timerBack = globalController.GetComponent<GlobalController>().timerBack;
			
			timerFront.renderer.enabled = true;
			timerBack.renderer.enabled = true;
			
			// Change position of the front timer
			Vector3 tempTimerVector = timerFront.transform.position;
			tempTimerVector.x = -7.0f;
			tempTimerVector.y = 5.2f;
			tempTimerVector.z = -.1f;
			timerFront.transform.position = tempTimerVector;

			// Change position of the back timer
			tempTimerVector = timerBack.transform.position;
			tempTimerVector.y = 5.12f;
			tempTimerVector.z = 0.0f;
			timerBack.transform.position = tempTimerVector;

			// Change the size of the front timer
			tempTimerVector = timerFront.transform.localScale;
			tempTimerVector.x = 5.7f;
			timerFront.transform.localScale = tempTimerVector;
		}
		timerStarted = false;
		timerSpeed = new Vector3( -.03f, 0.0f, 0.0f );
	}

	// Update is called once per frame
	void Update () 
	{
		if( countdown )
		{
			countdownTimer -= Time.deltaTime;
			if( countdownTimer <= 0.0f )
			{
				Destroy( countdown );

				// ALL CUPS
				cupOne.GetComponent<CupBehavior>().canStart = true;
				if( cupTwo )
					cupTwo.GetComponent<CupBehavior>().canStart = true;
				if( cupThree )
					cupThree.GetComponent<CupBehavior>().canStart = true;
				if( cupFour )
					cupFour.GetComponent<CupBehavior>().canStart = true;

				timerStarted = true;
			}
		}

		if( timerStarted )
		{
			timerFront.transform.Translate( timerSpeed * Time.deltaTime * 60.0f);
			
			if( timerFront.transform.position.x < -20.0f )
			{
				globalController.GetComponent<GlobalController>().LostMinigame();
			}
		}
	}

	public void CupLanded()
	{
		cupsToLand--;
		Debug.Log ("CUPS Not LANDED: " + cupsToLand);
		if( cupsToLand <= 0 )
		{
			if( globalController )
			{
				int partyPoints = 30 * globalController.GetComponent<GlobalController>().flippyCupLevel;

				if( globalController.GetComponent<GlobalController>().flippyCupLevel < 4 )
				{
					globalController.GetComponent<GlobalController>().flippyCupLevel++;
				}
				
				scoreTextFront.GetComponent<TextMesh>().text = "+" + partyPoints.ToString();
				scoreTextBack.GetComponent<TextMesh>().text = "+" + partyPoints.ToString();
				scoreTextFront.renderer.enabled = true;
				scoreTextBack.renderer.enabled = true;
				scoreText.GetComponent<Animator>().enabled = true;

				globalController.GetComponent<GlobalController>().BeatMinigame( partyPoints );
			}
			else
				Debug.Log( "Winner!" );
		}
	}

	public void CupFlicked()
	{
		Debug.Log( cupsToFlick );
		cupsToFlick--;

		if( cupsToFlick == 0 )
		{
			timerStarted = false;
		}
	}

	public void CupFellOver()
	{
		if( globalController )
			globalController.GetComponent<GlobalController>().LostMinigame();
		else
			Debug.Log("Failed");
	}
}
