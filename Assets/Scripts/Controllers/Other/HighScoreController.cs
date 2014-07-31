using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook;
using Facebook.MiniJSON;
using System;

public class HighScoreController : MonoBehaviour {

	public GameObject globalController;
	public GameObject twitterController;
	public Texture FBShareButton;
	public Texture TwitterShareButton;
	public GUIStyle FBShareStyle;
	public Sprite unclicked;
	public Sprite clicked;

	private const int LeaderBoardLength = 10;

	public GameObject pointsText;
	public int points;

	public GameObject FacebookButton;
	public GameObject LocalButton;
	public GameObject TwitterButton;

	
	public RaycastHit hit;
	public Ray ray;

	public GameObject[] highScoreTexts;
	public GameObject[] highScoreNames;

//	List<int> HighScores;
	List<object> FBScores;

	// Bars
	public GameObject BarOne;
	public GameObject BarTwo;
	public GameObject BarThree;
	public GameObject BarFour;
	public GameObject BarFive;
	public GameObject BarSix;

	public string[,] fbData = new string[20,2];
	public string[] fbNameList = new string[5];
	public string[] fbScoreList = new string[5];



	public GameObject ClickSound;


	void Start () 
	{
		twitterController = GameObject.Find ("Twitter Controller");
		globalController = GameObject.Find ("Global Controller");
		globalController.GetComponent<GlobalController>().pauseButton.renderer.enabled = false;

		CallPublishActions();
		
		if (globalController) 
		{
			AdColony.ShowVideoAd();
			points = globalController.GetComponent<GlobalController> ().partyPoints;
		}
		else
			points = 4300; // Arbitrary - for testing

		pointsText.GetComponent<TextMesh>().text = "Points: " + points.ToString();

		// Height one - .6f
		// Height two - 1.2f
		// Height three - 1.8f
		// Height four - 2.44f (Untouched, default)
		Vector3 tempScale = BarOne.transform.localScale;
		tempScale.y = globalController.GetComponent<GlobalController>().beerPongLevel * .6f;
		BarOne.transform.localScale = tempScale;
		tempScale.y = .6f;
		BarTwo.transform.localScale = tempScale;
		tempScale.y = .6f; // falllevel globalController.GetComponent<GlobalController>() * .6f;
		BarThree.transform.localScale = tempScale;
		tempScale.y = (globalController.GetComponent<GlobalController>().pukeLevel - 3) * .6f;
		BarFour.transform.localScale = tempScale;
		tempScale.y = globalController.GetComponent<GlobalController>().dartLevel * .6f;
		BarFive.transform.localScale = tempScale;
		tempScale.y = globalController.GetComponent<GlobalController>().armEnemyLevel * .6f;
		BarSix.transform.localScale = tempScale;
		
		//HighScores = globalController.GetComponent<GlobalController>().SaveHighScore( points );

	}

	public void CallPublishActions(){
		FB.Login ("publish_actions", PublishActionsCallBack);

	}
	private void PublishActionsCallBack(FBResult result){
		if (FB.IsLoggedIn) {
			Debug.Log(FB.UserId + " Publish Actions Called");
			GetFaceBookScores ();
			Debug.Log ("calling GetFacebookScoreS()");
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
					ClickSound.GetComponent<AudioSource>().Play();
					globalController.GetComponent<GlobalController>().LostGame();
					break;

				case "shareText":
					ClickSound.GetComponent<AudioSource>().Play();
					CallFBFeed();
					break;

				case "TwitterButton":
					ClickSound.GetComponent<AudioSource>().Play();
					twitterController.GetComponent<TwitterController>().twitterPost();
					break;

				case "FacebookButton":
					ClickSound.GetComponent<AudioSource>().Play();
					FacebookButton.GetComponent<SpriteRenderer>().sprite = clicked;
					LocalButton.GetComponent<SpriteRenderer>().sprite = unclicked;
					DisplayFaceBookHighScores();
					break;

				case "LocalButton":
					ClickSound.GetComponent<AudioSource>().Play();
					FacebookButton.GetComponent<SpriteRenderer>().sprite = unclicked;
					LocalButton.GetComponent<SpriteRenderer>().sprite = clicked;
					break;
				}


			}
		}
	}
	public void GetFaceBookScores(){
		FB.API ("/1439214366319352/scores", Facebook.HttpMethod.GET, scoreCallBack);
	}
	public int getScoreFromEntry(object obj)
	{
		Dictionary<string,object> entry = (Dictionary<string,object>) obj;
		return Convert.ToInt32(entry["score"]);
	}

	public void scoreCallBack(FBResult result){
		if (result.Error == null)
			Debug.Log ("Scores received");
		else
			Debug.Log (result.Error);


		int playerHighScore;

		//deserialize the scores from facebook
		/*the data that is stored in FBScores is an array of objects
		 * This is just to show you what facebook has for us so far:
		 * {
		  "data": [
		    {
		      "user": {
		        "id": "680564761", 
		        "name": "Colin Blaise"
		      }, 
		      "score": 110, 
		      "application": {
		        "name": "Games Gone Wild!", 
		        "id": "1439214366319352"
		      }
		    }, 
		    {
		      "user": {
		        "id": "100004603986976", 
		        "name": "Raul Luna Jr."
		      }, 
		      "score": 0, 
		      "application": {
		        "name": "Games Gone Wild!", 
		        "id": "1439214366319352"
		      }
		    }, 
		    {
		      "user": {
		        "id": "1535344784", 
		        "name": "Hardik Prajapati"
		      }, 
		      "score": 0, 
		      "application": {
		        "name": "Games Gone Wild!", 
		        "id": "1439214366319352"
		      }
		    }, 
		    {
		      "user": {
		        "id": "1237718228", 
		        "name": "Keith Thomas"
		      }, 
		      "score": 0, 
		      "application": {
		        "name": "Games Gone Wild!", 
		        "id": "1439214366319352"
		      }
		    }
		  ]
		}
		* 
		*/
		FBScores = Util.DeserializeScores (result.Text);

		int entryCount = 0;
		//for each object that contains a persons data store the info
		foreach(object score in FBScores){

			//entry is the player object that contains keys "user", "score", 
			//and "application"(we don't need this one)
			var entry = (Dictionary<string,object>) score;

			//user is the object that contains keys "id" and "name"
			var user = (Dictionary<string,object>) entry["user"];

			//id and name are keys inside the user object
			string userId = (string)user["id"];
			string name = (string)user["name"];
			fbNameList[entryCount] = name;

			//score is a key inside the entry object,
			playerHighScore = getScoreFromEntry(entry);
			entry["score"] = playerHighScore.ToString(); 
			fbScoreList[entryCount] = playerHighScore.ToString ();

			//Check if this entry is the current local user and 
			//if the user's score is higher than his saved facebook score, update it.
			if (string.Equals (userId,FB.UserId)){
				Util.Log ("Local player: " + name + "'s score on server is " + playerHighScore);

				//if new highscore, update it to facebook
				if (points > playerHighScore){
					entry["score"] = points.ToString();
					var scoreData = new Dictionary<string,string>() {{"score", points.ToString()}};
					FB.API("/me/scores",Facebook.HttpMethod.POST,LogCallback,scoreData);
					fbScoreList[entryCount] = points.ToString ();
				}

			}
			Debug.Log ("userID: " + userId + " name: " + name + " score: " + entry["score"]);

			entryCount++;
		}

	}


	public void DisplayFaceBookHighScores()
	{
		//int count = HighScores.Count;
		int count = 0;
		foreach( GameObject scoreText in highScoreTexts )
		{
			if( count == 6 )
			{
				break;
			}

			scoreText.GetComponent<TextMesh>().text = fbScoreList[count];
			count++;
		}
		count = 0;
		foreach( GameObject scoreName in highScoreNames )
		{
			if (count == 6)
			{
				break;
			}

			scoreName.GetComponent<TextMesh>().text = fbNameList[count];
			count++;
		}

	}

	void OnApplicationQuit(){
		PlayerPrefs.Save();
	}
}
