using UnityEngine;
using System.Collections;

public class MinigameFailController : MonoBehaviour 
{
	GameObject globalController;
	int beersDrank;
	int partyPoints;
	float timer;

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
		
		partyPoints = globalController.GetComponent<GlobalController>().partyPoints;

		pointsText.GetComponent<TextMesh>().text = partyPoints.ToString();

		beersDrank = globalController.GetComponent<GlobalController>().beersDrank;

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
