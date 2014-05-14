using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Facebook;
using Facebook.MiniJSON;

public class MenuController : MonoBehaviour 
{
	public GameObject globalController; 
	public RaycastHit hit;
	public Ray ray;

	//facebook data
	public string FBName;
	public Texture profilePic;
	public int score;
	public GameObject FBPicture;

	// Text objects for changing text on the fly
	public GameObject facebookLoggedInText;
	public GameObject facebookLoggedInTextShadow;
	public GameObject highScoreText;
	public GameObject highScoreTextShadow;
	public Texture FBLoginButton;
	public GUIStyle FBbuttonStyle;

	//sounds
	public GameObject ClickSound;

	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find("Global Controller");

		//Initial check to see if user is logged into Facebook
		if (!FB.IsLoggedIn) {
			CallFBInit ();
		}
		else{
			//If logged in then show the name and picture
			Debug.Log("Logged in? " + FB.IsLoggedIn);
			FBPicture.renderer.guiTexture.texture = globalController.GetComponent<GlobalController> ().profilePic;
			profilePic = globalController.GetComponent<GlobalController> ().profilePic;
			FBName = globalController.GetComponent<GlobalController> ().FBUsername;
			if (profilePic == null || FBName == null){
				OnLoggedIn();
			}
		
		}

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

		hit = new RaycastHit();
	}
	
	void OnGUI()
	{
		//If not logged in, display the login button
		if (!FB.IsLoggedIn)
		{   

			if (GUI.Button(new Rect(200, 20, 260, 60),"", FBbuttonStyle))
			{
				Debug.Log ("Calling FBLogin()");
				CallFBLogin();
			}
		}
		//If logged in, display the user name and picture
		if (FB.IsLoggedIn)
		{ 
			facebookLoggedInText.GetComponent<TextMesh>().text = "Logged In - " + FBName;
			facebookLoggedInTextShadow.GetComponent<TextMesh>().text = "Logged In - " + FBName;
			if (profilePic != null)
				//FacebookPicture.guiTexture.texture = profilePic;
			    GUI.DrawTexture(new Rect(10,10,110,110),profilePic,ScaleMode.ScaleToFit,true,0);    
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
		if (FB.IsLoggedIn) {
			Debug.Log(FB.UserId + " Publish Actions Called");
			CallFBFeed();
		}
		else {
			Debug.Log (result.Error);
		}
	}
	private void LoginCallback(FBResult result){
		if(FB.IsLoggedIn) {
			Debug.Log(FB.UserId);
			OnLoggedIn ();
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
					ClickSound.GetComponent<AudioSource>().Play();
					globalController.GetComponent<GlobalController>().StartMode("Normal Mode", "");
					break;
				case "PracticeText":
					ClickSound.GetComponent<AudioSource>().Play();
					Application.LoadLevel( "SelectionScene" );
					break;
				}
			}
		}
	}
}
