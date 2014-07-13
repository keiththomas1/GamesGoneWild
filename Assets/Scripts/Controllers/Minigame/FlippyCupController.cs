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

	public GameObject scoreText;
	public GameObject scoreTextFront;
	public GameObject scoreTextBack;

	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find( "Global Controller" );

		if( globalController )
			cupsToLand = globalController.GetComponent<GlobalController>().flippyCupLevel;
		else
			cupsToLand = 4;
		
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
	}

	// Update is called once per frame
	void Update () 
	{
	
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

	public void CupFellOver()
	{
		if( globalController )
			globalController.GetComponent<GlobalController>().LostMinigame();
		else
			Debug.Log("Failed");
	}
}
