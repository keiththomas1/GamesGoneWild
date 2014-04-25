using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook;
using Facebook.MiniJSON;

public class HighScoreController : MonoBehaviour {

	public GameObject globalController;
	public Texture FBShareButton;
	public GUIStyle FBShareStyle;

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

	public void CallPublishActions(){
		FB.Login ("publish_actions", PublishActionsCallBack);

	}
	private void PublishActionsCallBack(FBResult result){
		if (FB.IsLoggedIn) {
			Debug.Log(FB.UserId + " Publish Actions Called");
			CallFBFeed();
		}
		else {
			Debug.Log (result.Error);
		}
	}
	private void CallFBFeed(){
		FB.Feed(
			linkName: "Curry Furry Games",
			linkDescription: "Games Gone Wild!",
			linkCaption: "I just scored " + points + " points! Can you beat it?",
			picture: "http://gamesgonewild.files.wordpress.com/2014/03/bpscreenshot.png",
			callback: LogCallback
			);
	}
	void LogCallback(FBResult response) {
		Debug.Log(response.Text);
	}

	void OnGUI(){

		/*if (GUI.Button(new Rect(200, 920, 2000, 500),FBShareButton, FBShareStyle)){
			CallPublishActions();
		}*/
	
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

				case "shareText":
					CallPublishActions();
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
