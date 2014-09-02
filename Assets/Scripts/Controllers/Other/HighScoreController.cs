using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook;
using Facebook.MiniJSON;
using System;

public class HighScoreController : MonoBehaviour {

	public GameObject globalController;
	public GameObject twitterController;
	public GameObject FBShareButton;
	public Texture TwitterShareButton;
	public GUIStyle FBShareStyle;
	public Sprite unclicked;
	public Sprite clicked;

	private const int LeaderBoardLength = 10;

	public GameObject pointsText;
	public int points;

	public GameObject FacebookButton;
	public GameObject FacebookLogin;
	public GameObject LocalButton;
	public GameObject TwitterButton;
	public GameObject FacebookLoading;

	bool facebookScoresRetrieved;

	bool inFacebookTab;
	
	public RaycastHit hit;
	public Ray ray;

	public GameObject[] highScoreTexts;
	public GameObject[] highScoreNames;

	//facebook data
	public string FBName;
	public Texture profilePic;
	public GameObject FBPicture;
	List<object> FBScores;

	public string[,] fbData = new string[20,2];
	public string[] fbNameList = new string[5];
	public string[] fbScoreList = new string[5];
	public int[] localScoreList = new int[7];

	public GameObject ClickSound;


	void Start () 
	{
		CallFBInit ();

		/*
		else
			FBPicture.renderer.guiTexture.texture = globalController.GetComponent<GlobalController> ().profilePic;
			profilePic = globalController.GetComponent<GlobalController> ().profilePic;
			FBName = globalController.GetComponent<GlobalController> ().FBUsername;

			if (profilePic == null || FBName == null)
			{
				OnLoggedIn();
			}
		*/

		twitterController = GameObject.Find ("Twitter Controller");
		globalController = GameObject.Find ("Global Controller");
		
		if (globalController) 
		{
			//AdColony.ShowVideoAd();
			globalController.GetComponent<GlobalController>().pauseButton.renderer.enabled = false;
			points = globalController.GetComponent<GlobalController> ().partyPoints;
		}
		else
			points = 2300; // Arbitrary - for testing

		pointsText.GetComponent<TextMesh>().text = "Points: " + points.ToString();

		FacebookLogin.renderer.enabled = false;
		FacebookLogin.collider.enabled = false;
		FBShareButton.renderer.enabled = false;
		FBShareButton.collider.enabled = false;
		FacebookLoading.renderer.enabled = false;

		facebookScoresRetrieved = false;
		inFacebookTab = false;
		//HighScores = globalController.GetComponent<GlobalController>().SaveHighScore( points );

		ManageLocalHighScores (points);
		DisplayLocalScores ();

		if (FB.IsLoggedIn)
			CallPublishActions();

	}

	// Update is called once per frame
	void Update () 
	{
		if( Input.GetMouseButtonDown( 0 ) )
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if( Physics.Raycast(ray,out hit) )
			{
				switch( hit.collider.name )
				{
				case "MainMenuText":
					ClickSound.GetComponent<AudioSource>().Play();
					globalController.GetComponent<GlobalController>().LostGame();
					break;

				case "shareText":
					ClickSound.GetComponent<AudioSource>().Play();
					CallFBFeed ();
					break;

				case "TwitterButton":
					ClickSound.GetComponent<AudioSource>().Play();
					twitterController.GetComponent<TwitterController>().twitterPost();
					break;

				case "FacebookButton":
					inFacebookTab = true;
					ClickSound.GetComponent<AudioSource>().Play();
					FacebookButton.GetComponent<SpriteRenderer>().sprite = clicked;
					LocalButton.GetComponent<SpriteRenderer>().sprite = unclicked;
					if (FB.IsLoggedIn)
					{ 
						FBShareButton.renderer.enabled = true;
						FBShareButton.collider.enabled = true;
						if( facebookScoresRetrieved )
							DisplayFaceBookHighScores();
						else
							GetFaceBookScores();
					}
					else
					{
						FacebookLogin.renderer.enabled = true;
						FacebookLogin.collider.enabled = true;
						DisplayEmptyHighScores();
					}
					break;

				case "LocalButton":
					inFacebookTab = false;
					FacebookLoading.renderer.enabled = false;
					ClickSound.GetComponent<AudioSource>().Play();
					FacebookButton.GetComponent<SpriteRenderer>().sprite = unclicked;
					LocalButton.GetComponent<SpriteRenderer>().sprite = clicked;
					FacebookLogin.renderer.enabled = false;
					FacebookLogin.collider.enabled = false;
					FBShareButton.renderer.enabled = false;
					FBShareButton.collider.enabled = false;

					DisplayLocalScores();
					break;

				case "FacebookLogin":
					CallFBLogin();
					CallPublishActions();
					break;
				}
			}
		}
	}
	public void GetFaceBookScores(){
		FacebookLoading.renderer.enabled = true;
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

		facebookScoresRetrieved = true;
		DisplayFaceBookHighScores();
	}


	public void DisplayFaceBookHighScores()
	{
		Debug.Log( "display fb scores" );
		FacebookLoading.renderer.enabled = false;
		if( inFacebookTab )
		{
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
	}
	void DisplayEmptyHighScores()
	{
		int count = 0;
		foreach( GameObject scoreText in highScoreTexts )
		{
			if( count == 6 )
			{
				break;
			}
			
			scoreText.GetComponent<TextMesh>().text = " ";
			count++;
		}
	}
	
	public void ManageLocalHighScores(int points)
	{
		int[] hs = new int[7];
		hs [1] = -1; hs [2] = -1; hs [3] = -1; hs [4] = -1; hs [5] = -1;
		int temp = 0;

		hs[1] = PlayerPrefs.GetInt ("HS1");
		hs[2] = PlayerPrefs.GetInt ("HS2");
		hs[3] = PlayerPrefs.GetInt ("HS3");
		hs[4] = PlayerPrefs.GetInt ("HS4");
		hs[5] = PlayerPrefs.GetInt ("HS5");

		localScoreList = hs;

		if (points > hs[1])
		{
			temp = points;
			PlayerPrefs.SetInt("HS1",hs[1]);
			PushLocalScores (4, temp, hs, 1);
		}
		else if (points < hs[1] && points > hs[2])
		{
			temp = points;
			PlayerPrefs.SetInt("HS2", hs[2]);
			PushLocalScores(3, temp, hs, 2);
		}
		else if (points < hs[2] && points > hs[3])
		{
			temp = points;
			PushLocalScores(2, temp, hs, 3);
		}
		else if (points < hs[3] && points > hs[4])
		{
			temp = points;
			PushLocalScores(1, temp, hs, 4);
		}
		else if (points < hs[4] && points > hs[5])
		{
			localScoreList[5] = points;
			PlayerPrefs.SetInt ("HS5", points);
			PlayerPrefs.Save ();
		}
	}

	private void PushLocalScores (int numPushes, int newScore, int[] hs, int pos)
	{
		int originalPos = pos;
		int temp; 

		while (numPushes != 0)
		{
			temp = hs[pos+1];
			hs[pos+1] = hs[pos];
			hs[pos+2] = temp;
			numPushes--;
			if (hs[pos+1] == 0)
				break;
			pos++;

		}

		hs [originalPos] = newScore;
		localScoreList = hs;

		for (int i = 1; i < 6; i++)
		{
			PlayerPrefs.SetInt("HS"+ i, hs[i]);
			PlayerPrefs.Save();
		}
	}

	private void DisplayLocalScores()
	{
		int count = 1;
		foreach( GameObject scoreText in highScoreTexts )
		{
			if( count == 6 )
			{
				break;
			}
			
			scoreText.GetComponent<TextMesh>().text = localScoreList[count].ToString ();
			count++;
		}
		count = 0;
		foreach( GameObject scoreName in highScoreNames )
		{
			if (count == 6)
			{
				break;
			}
			
			scoreName.GetComponent<TextMesh>().text = " ";
			count++;
		}
	}
	
	public void CallFBInit(){
		FB.Init(OnInitComplete, OnHideUnity);
	}
	private void OnInitComplete()	{
		Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
	}
	private void OnHideUnity(bool isGameShown)		{
		Debug.Log("Is game showing? " + isGameShown);
	}
	private void CallFBLogin(){
		FB.Login("basic_info", LoginCallback);
	}
	public void CallPublishActions(){
		FB.Login ("publish_actions", PublishActionsCallBack);
	}
	private void PublishActionsCallBack(FBResult result){
		Debug.Log( "Publish returned" );
		if (FB.IsLoggedIn) {
			Debug.Log(FB.UserId + " Publish Actions Called");
			//CallFBFeed();
		}
		else {
			Debug.Log (result.Error);
		}
	}
	private void LoginCallback(FBResult result){
		if(FB.IsLoggedIn) {
			Debug.Log("Login callback; UserID: " + FB.UserId);
			//OnLoggedIn ();
			GetFaceBookScores();
			FacebookLogin.renderer.enabled = false;
			FacebookLogin.collider.enabled = false;
			FBShareButton.renderer.enabled = true;
			FBShareButton.collider.enabled = true;
		}
	}
	void OnLoggedIn()
	{
		//Get the users name and profile picture from facebook
		FB.API("/me?fields=name", Facebook.HttpMethod.GET, APICallback);
		LoadPicture (Util.GetPictureURL ("me", 128, 128), MyPictureCallback);
	}
	
	void APICallback(FBResult result)                                                                                              
	{                                                                                                                              
		Debug.Log("APICallback");                                                                                                
		if (result.Error != null)                                                                                                  
		{                                                                                                                          
			Debug.LogError(result.Error);                                                                                           
			// Let's just try again                                                                                                
			FB.API("/me?fields=name", Facebook.HttpMethod.GET, APICallback);     
			return;                                                                                                                
		}           
		//deserialzie json object
		var dict = Json.Deserialize(result.Text) as Dictionary<String,System.Object>;
		Debug.Log ("deserialized: " + dict.GetType ());
		Debug.Log ("dict['name'][0]: " + dict ["name"] as String);
		
		FBName = dict ["name"] as String;
		if ( globalController )
			globalController.GetComponent<GlobalController> ().SetUserName (FBName);
	}          
	
	void MyPictureCallback(Texture texture)                                                                                        
	{                                                                                                                              
		Util.Log ("MyPictureCallback");
		if (texture == null)
		{
			//Let's just try again
			Util.LogError ("Error Loading user picture");
			LoadPicture(Util.GetPictureURL("me", 128, 128), MyPictureCallback);
			return;
		}
		profilePic = texture;
		FBPicture.guiTexture.texture = texture;
		//FBPicture.renderer.enabled = true;
		
		globalController.GetComponent<GlobalController> ().SetProfilePic (profilePic);
		
	}
	
	delegate void LoadPictureCallback(Texture texture);
	
	IEnumerator LoadPictureEnumerator(string url, LoadPictureCallback callback)
	{
		WWW www = new WWW (url);
		yield return www;
		callback (www.texture);
	}
	
	void LoadPicture (string url, LoadPictureCallback callback)
	{
		FB.API(url,Facebook.HttpMethod.GET,result =>
		       {
			if (result.Error != null)
			{
				Util.LogError (result.Error);
				return;
			}
			var imageUrl = Util.DeserializePictureURLString(result.Text);
			StartCoroutine(LoadPictureEnumerator(imageUrl, callback));
		});
	}
	private void CallFBFeed(){
		FB.Feed(
			linkName: "Games Gone Wild!",
			linkDescription: "Minigames bringing the best of college life to you",
			linkCaption: "I just scored " + points + " points! Try to beat it.",
			picture: "http://i.imgur.com/fsOyhHF.png",
			callback: LogCallback
			);
	}
	private void CallFBLogout(){
		FB.Logout ();
	}
	void LogCallback(FBResult response) {
		Debug.Log(response.Text);
	}

}
