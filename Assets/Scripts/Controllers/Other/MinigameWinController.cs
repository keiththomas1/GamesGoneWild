using UnityEngine;
using System.Collections;

public class MinigameWinController : MonoBehaviour 
{
	GameObject globalController;
	public GameObject gamesWonText;
	
	public GameObject[] beerCans;
	int beersDrank;
	public Sprite drankCan;

	float timer;
	
	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find( "Global Controller" );
		
		gamesWonText.GetComponent<TextMesh>().text = 
			globalController.GetComponent<GlobalController>().gamesWon.ToString();
		
		beersDrank = globalController.GetComponent<GlobalController>().beersDrank;
		
		for( int i=0; i < beersDrank; i++ )
		{
			beerCans[i].GetComponent<SpriteRenderer>().sprite = drankCan;
		}

		timer = 2.0f;
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
