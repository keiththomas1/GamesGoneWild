using UnityEngine;
using System.Collections;

public class MinigameFailController : MonoBehaviour 
{
	GameObject globalController;
	int beersDrank;
	int partyPoints;
	float timer;

	// All of the objects and sprites that need to be changed on the fly.
	public GameObject beerGauge;
	public Sprite beerGuage25; // 1/4 of the way full
	public Sprite beerGuage50; // 2/4 of the way full
	public Sprite beerGuage75; // 3/4 of the way full
	public Sprite beerGuage100; // all of the way full

	public GameObject faceOne;
	public Sprite faceOneColor;
	public Sprite faceOneGrey;
	public GameObject faceTwo;
	public Sprite faceTwoColor;
	public Sprite faceTwoGrey;
	public GameObject faceThree;
	public Sprite faceThreeColor;
	public Sprite faceThreeGrey;
	public GameObject faceFour;
	public Sprite faceFourColor;
	public Sprite faceFourGrey;

	public GameObject pointsText;

	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find( "Global Controller" );
		
		partyPoints = globalController.GetComponent<GlobalController>().partyPoints;

		pointsText.GetComponent<TextMesh>().text = partyPoints.ToString();

		beersDrank = globalController.GetComponent<GlobalController>().beersDrank;

		switch( beersDrank )
		{
		case 0:
			// Already preset
			break;
		case 1:
			beerGauge.GetComponent<SpriteRenderer>().sprite = beerGuage25;
			faceOne.GetComponent<SpriteRenderer>().sprite = faceOneColor;
			break;
		case 2:
			beerGauge.GetComponent<SpriteRenderer>().sprite = beerGuage50;
			faceOne.GetComponent<SpriteRenderer>().sprite = faceOneGrey;
			faceTwo.GetComponent<SpriteRenderer>().sprite = faceTwoColor;
			break;
		case 3:
			beerGauge.GetComponent<SpriteRenderer>().sprite = beerGuage75;
			faceTwo.GetComponent<SpriteRenderer>().sprite = faceTwoGrey;
			faceThree.GetComponent<SpriteRenderer>().sprite = faceThreeColor;
			break;
		case 4:
			beerGauge.GetComponent<SpriteRenderer>().sprite = beerGuage100;
			faceThree.GetComponent<SpriteRenderer>().sprite = faceThreeGrey;
			faceFour.GetComponent<SpriteRenderer>().sprite = faceFourColor;
			break;
		}

		timer = 2.5f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer -= Time.deltaTime;

		if( timer <= 0.0f )
		{
			globalController.GetComponent<GlobalController>().NextMinigame();
		}
	}
}
