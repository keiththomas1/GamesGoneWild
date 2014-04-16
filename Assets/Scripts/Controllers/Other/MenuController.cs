using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Facebook;
using Facebook.MiniJSON;

public class MenuController : MonoBehaviour 
{
	public GameObject globalController; 
	public Texture FBLoginButton;
	public RaycastHit hit;
	public Ray ray;
	public GUIStyle MenuText;
	public GUIStyle FBbuttonStyle;

	public string FBName;
	public Texture profilePic;
	public int score;

	// Text objects for changing text on the fly
	public GameObject facebookLoggedInText;
	public GameObject facebookLoggedInTextShadow;
	public GameObject highScoreText;
	public GameObject highScoreTextShadow;

	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find("Global Controller");
		score = globalController.GetComponent<GlobalController> ().totalPartyPoints;
		Debug.Log ("score: " + score);

		int partyPoints;
		if( globalController )
		{
			List<int> tempList = globalController.GetComponent<GlobalController>().GetHighScores();
			tempList.Reverse();
			partyPoints = tempList[0];
		}
		else
		{
			partyPoints = 0;
		}
		highScoreText.GetComponent<TextMesh>().text = "High Score: " + partyPoints.ToString();
		highScoreTextShadow.GetComponent<TextMesh>().text = "High Score: " + partyPoints.ToString();
		
		facebookLoggedInText.GetComponent<TextMesh>().text = "";
		facebookLoggedInTextShadow.GetComponent<TextMesh>().text = "";

		CallFBInit ();
		hit = new RaycastHit();

		if (FB.IsLoggedIn)
		{
			OnLoggedIn();
		}
	}
	
	void OnGUI()
	{

		if (!FB.IsLoggedIn)
		{   

			if (GUI.Button(new Rect(200, 20, 240, 40),"", FBbuttonStyle))
			{
				Debug.Log ("Calling FBLogin()");
				CallFBLogin();
			}
		}

		if (FB.IsLoggedIn)
		{ 
			facebookLoggedInText.GetComponent<TextMesh>().text = "Logged In - " + FBName;
			facebookLoggedInTextShadow.GetComponent<TextMesh>().text = "Logged In - " + FBName;
			//GUI.Label(new Rect(200, 10, 275, 90),"Welcome \n" + FBName + "!", MenuText);
			GUI.DrawTexture(new Rect(1500,10,256,256),profilePic,ScaleMode.ScaleToFit,true,0);
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
		FB.Login("basic_info, publish_actions", LoginCallback);
	}
	private void LoginCallback(FBResult result){
		if(FB.IsLoggedIn) {
			Debug.Log(FB.UserId);
			OnLoggedIn ();
		}
	}

	void OnLoggedIn()
	{
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
			FB.API("/me?fields=name)", Facebook.HttpMethod.GET, APICallback);     
			return;                                                                                                                
		}                                                                                                                          
		var dict = Json.Deserialize(result.Text) as Dictionary<String,System.Object>;
		Debug.Log ("deserialized: " + dict.GetType ());
		Debug.Log ("dict['name'][0]: " + dict ["name"] as String);

		FBName = dict ["name"] as String;
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
			"https://example.com/myapp/?storyID=thelarch",
			"The Larch",
			"I thought up a witty tagline about larches",
			"There are a lot of larch trees around here, aren't there?",
			"https://example.com/myapp/assets/1/larch.jpg"
			 );
	}
	private void CallFBLogout(){
		FB.Logout ();
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
				case "PlayText":
					globalController.GetComponent<GlobalController>().StartMode("Normal Mode", "");
					break;
				case "SelectionText":
					Application.LoadLevel( "SelectionScene" );
					break;
				}
			}
		}
	}
}
