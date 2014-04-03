using UnityEngine;
using System.Collections;

public class MinigameWinController : MonoBehaviour 
{
	GameObject globalController;
	public GameObject pointsText;

	int points;

	float timer;
	
	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find( "Global Controller" );

		points = globalController.GetComponent<GlobalController>().partyPoints;

		pointsText.GetComponent<TextMesh>().text = points.ToString();

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
