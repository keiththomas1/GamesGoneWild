using UnityEngine;
using System.Collections;

public class MinigameFailController : MonoBehaviour 
{
	GameObject globalController;
	int beersDrank;
	int partyPoints;

	bool stayOnScreen;
	float switchScreenTimer;

	bool beerDrip;
	float beerDripTimer;
	public GameObject beerDripBack;

	// All of the objects and sprites that need to be changed on the fly.
	public GameObject background;
	public Sprite background50Percent;
	public Sprite background75Percent;
	public Sprite background100Percent;

	public GameObject pointsText;

	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find( "Global Controller" );

		if( globalController )
		{
			partyPoints = globalController.GetComponent<GlobalController>().partyPoints;
			beersDrank = globalController.GetComponent<GlobalController>().beersDrank;
		}
		else
		{
			partyPoints = 1320;
			beersDrank = 4;
		}

		pointsText.GetComponent<TextMesh>().text = partyPoints.ToString();


		switch( beersDrank )
		{
		case 0:
			// Already preset
			break;
		case 1:
			break;
		case 2:
			background.GetComponent<SpriteRenderer>().sprite = background50Percent;
			break;
		case 3:
			background.GetComponent<SpriteRenderer>().sprite = background75Percent;
			break;
		case 4:
			background.GetComponent<SpriteRenderer>().sprite = background100Percent;
			break;
		}

		stayOnScreen = true;
		switchScreenTimer = 2.5f;

		beerDripBack.GetComponent<Animator>().enabled = false;
		beerDrip = false;
		beerDripTimer = 3.6f;
	}
	
	// Update is called once per frame
	void Update () 
	{

		if( stayOnScreen )
		{
			switchScreenTimer -= Time.deltaTime;

			if( switchScreenTimer <= 0.0f )
			{
				if( beersDrank == 4 ) // Start losing animation
				{
					beerDripBack.GetComponent<Animator>().enabled = true;
					beerDrip = true;
					stayOnScreen = false;
				}
				else
				{
					if( globalController )
						globalController.GetComponent<GlobalController>().NextMinigame();
				}
			}
		}

		if( beerDrip )
		{
			beerDripTimer -= Time.deltaTime;
			//beerDripBack.transform.Translate( 0.0f, -.1f * 60.0f * Time.deltaTime, 0.0f );

			if( beerDripTimer <= 0.0f )
			{
				globalController.GetComponent<GlobalController>().NextMinigame();
			}
			if( beerDripTimer <= .62f )
			{
				beerDripBack.GetComponent<Animator>().speed = 0.0f;
			}
		}
	}
}
