using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HighScoreController : MonoBehaviour {

	public GameObject globalController;

	private const int LeaderBoardLength = 10;

	public GameObject pointsText;
	public int points;
	
	public RaycastHit hit;
	public Ray ray;

	public GameObject[] highScoreTexts;
	List<int> HighScores;

	void Start () 
	{
		globalController = GameObject.Find ("Global Controller");

		if( globalController )
			points = globalController.GetComponent<GlobalController> ().partyPoints;
		else
			points = 4300; // Arbitrary - for testing

		pointsText.GetComponent<TextMesh>().text = "Points: " + points.ToString();
		
		HighScores = globalController.GetComponent<GlobalController>().SaveHighScore( points );
		DisplayHighScores();
	}

	// Update is called once per frame
	void Update () 
	{
		if( Input.GetMouseButtonDown( 0 ) )
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if( Physics.Raycast(ray,out hit) )
			{
				Debug.Log( hit.collider.name );
				switch( hit.collider.name )
				{
				case "MainMenuText":
					globalController.GetComponent<GlobalController>().LostGame();
					break;
				}
			}
		}
	}

	public void DisplayHighScores()
	{
		int count = HighScores.Count;

		foreach( GameObject scoreText in highScoreTexts )
		{
			if( count == 0 )
			{
				break;
			}
			scoreText.GetComponent<TextMesh>().text = HighScores[count-1].ToString();
			count--;
		}
	}

	void OnApplicationQuit(){
		PlayerPrefs.Save();
	}
}
