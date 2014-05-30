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

	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find( "Global Controller" );

		if( globalController )
			cupsToLand = globalController.GetComponent<GlobalController>().flippyCupLevel;
		else
			cupsToLand = 3;

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

		if( cupsToLand <= 0 )
		{
			if( globalController )
			{
				if( globalController.GetComponent<GlobalController>().flippyCupLevel < 4 )
				{
					globalController.GetComponent<GlobalController>().flippyCupLevel++;
				}
				globalController.GetComponent<GlobalController>().BeatMinigame( 100 );
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
