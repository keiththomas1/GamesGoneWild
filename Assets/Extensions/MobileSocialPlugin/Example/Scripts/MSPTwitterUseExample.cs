////////////////////////////////////////////////////////////////////////////////
//  
// @module <module_name>
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class MSPTwitterUseExample : MonoBehaviour {

	
	//Replace with your key and secret
	private static string TWITTER_CONSUMER_KEY = "wEvDyAUr2QabVAsWPDiGwg";
	private static string TWITTER_CONSUMER_SECRET = "igRxZbOrkLQPNLSvibNC3mdNJ5tOlVOPH3HNNKDY0";
	
	
	
	
	private static bool IsUserInfoLoaded = false;
	
	private static bool IsAuntifivated = false;
	
	public Texture2D ImageToShare;
	public DefaultPreviewButton connectButton;
	public SA_Texture avatar;
	public SA_Label Location;
	public SA_Label Language;
	public SA_Label Status;
	public SA_Label Name;
	public GameObject[] showedOnAuth;
	
	void Awake() {


		
		
		SPTwitter.instance.addEventListener(TwitterEvents.TWITTER_INITED,  OnInit);
		SPTwitter.instance.addEventListener(TwitterEvents.AUTHENTICATION_SUCCEEDED,  OnAuth);
		
		SPTwitter.instance.addEventListener(TwitterEvents.POST_SUCCEEDED,  OnPost);
		SPTwitter.instance.addEventListener(TwitterEvents.POST_FAILED,  OnPostFailed);
		
		SPTwitter.instance.addEventListener(TwitterEvents.USER_DATA_LOADED,  OnUserDataLoaded);
		SPTwitter.instance.addEventListener(TwitterEvents.USER_DATA_FAILED_TO_LOAD,  OnUserDataLoadFailed);
		
		
		//You can use:
		//SPTwitter.instance.Init();
		//if TWITTER_CONSUMER_KEY and TWITTER_CONSUMER_SECRET was alredy set in 
		//Window -> Mobile Social Plugin -> Edit Settings menu.
		
		
		SPTwitter.instance.Init(TWITTER_CONSUMER_KEY, TWITTER_CONSUMER_SECRET);
		
		
		
	}
	
	void FixedUpdate() {
		if(IsAuntifivated) {
			connectButton.text = "Disconnect";
			Name.text = "Player Connected";
			foreach(GameObject o in showedOnAuth) {
				o.SetActive(true);
			}
		} else {
			foreach(GameObject o in showedOnAuth) {
				o.SetActive(false);
			}
			connectButton.text = "Connect";
			Name.text = "Player Disconnected";
			
			return;
		}
		
		
		if(IsUserInfoLoaded) {
			
			
			if(SPTwitter.instance.userInfo.profile_image != null) {
				avatar.texture = SPTwitter.instance.userInfo.profile_image;
			}
			
			Name.text = SPTwitter.instance.userInfo.name + " aka " + SPTwitter.instance.userInfo.screen_name;
			Location.text = SPTwitter.instance.userInfo.location;
			Language.text = SPTwitter.instance.userInfo.lang;
			Status.text = SPTwitter.instance.userInfo.status.text;
			
			
		}
		
	}
	
	private void Connect() {
		if(!IsAuntifivated) {
			SPTwitter.instance.AuthenticateUser();
			if(SPTwitter.instance.IsAuthed)
				Debug.Log ("Authenticated");
		} else {
			LogOut();
		}
	}
	
	private void PostWithAuthCheck() {
		SPTwitter.instance.PostWithAuthCheck("Hello, I'am posting this from my app");
	}
	
	private void PostNativeScreenshot() {
		StartCoroutine(PostTWScreenshot());
	}
	
	private void PostMSG() {
		SPShareUtility.TwitterShare("This is my text to share");
	}
	
	
	private void PostImage() {
		SPShareUtility.TwitterShare("This is my text to share", ImageToShare);
	}
	
	
	
	private IEnumerator PostTWScreenshot() {
		
		
		yield return new WaitForEndOfFrame();
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D( width, height, TextureFormat.RGB24, false );
		// Read screen contents into the texture
		tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		tex.Apply();


		SPShareUtility.TwitterShare("This is my text to share", tex);
		
		Destroy(tex);
		
	}
	
	
	private void LoadUserData() {
		SPTwitter.instance.LoadUserData();
	}
	
	private void PostMessage() {
		SPTwitter.instance.Post("Hello, I'am posting this from my app");
	}
	
	private void PostScreehShot() {
		StartCoroutine(PostScreenshot());
	}
	
	void OnGUI() {
		
		
		
		/*

		if(!IsAuntifivated) {
			GUI.Label(new Rect(10, 10, Screen.width, 100), "App do not have permission to use your twitter account, press the button to auntificate", style);
			if(GUI.Button(new Rect(10, 70, 150, 50), "Twitter Auth")) {
				SPTwitter.instance.AuthificateUser();
			}
		} else {

			if(!IsUserInfoLoaded) {
				GUI.Label(new Rect(10, 10, Screen.width, 100), "Great, app have  permission to use your twitter account, see the avaliable action bellow", style);

				if(GUI.Button(new Rect(10, 70, 150, 50), "Load User Data")) {
					SPTwitter.instance.LoadUserData();
				}
				
				if(GUI.Button(new Rect(10, 130, 150, 50), "Post Message")) {
					SPTwitter.instance.Post("Hello, I'am posting this from my app");
				}

				if(GUI.Button(new Rect(10, 190, 150, 50), "Post ScreehShot")) {
					StartCoroutine(PostScreenshot());
				}

				if(GUI.Button(new Rect(10, 250, 150, 50), "Log out")) {
					LogOut();
				}
			} else {

				if(SPTwitter.instance.userInfo.profile_background != null) {
					GUI.DrawTexture(new Rect(0, 0, SPTwitter.instance.userInfo.profile_background.width, SPTwitter.instance.userInfo.profile_background.height),  SPTwitter.instance.userInfo.profile_background);
				}


				if(SPTwitter.instance.userInfo.profile_image != null) {
					GUI.DrawTexture(new Rect(10, 10, 60, 60),  SPTwitter.instance.userInfo.profile_image);
				}


				GUI.Label(new Rect(150, 10, Screen.width, 100),  SPTwitter.instance.userInfo.name + " aka " + SPTwitter.instance.userInfo.screen_name, style2);
				GUI.Label(new Rect(150, 30, Screen.width, 100),  "Location:  " + SPTwitter.instance.userInfo.location, style2);
				GUI.Label(new Rect(150, 50, Screen.width, 100),  "Language:  " + SPTwitter.instance.userInfo.lang, style2);

				GUI.Label(new Rect(150, 70, Screen.width, 100),  "Status:  " + SPTwitter.instance.userInfo.status.text , style2);

				GUI.Label(new Rect(150, 90, Screen.width, 100),  SPTwitter.instance.userInfo.name + " has  " + SPTwitter.instance.userInfo.friends_count +  " friends and " + SPTwitter.instance.userInfo.statuses_count + " twits", style2);


				if(GUI.Button(new Rect(10, 130, 150, 50), "Post Message")) {
					SPTwitter.instance.Post("Hello, I'am posting this from my app");
				}
				
				if(GUI.Button(new Rect(10, 190, 150, 50), "Post ScreehShot")) {
					StartCoroutine(PostScreenshot());
				}
				
				if(GUI.Button(new Rect(10, 250, 150, 50), "Log out")) {
					LogOut();
				}
			}
		}

*/
	}
	
	
	
	// --------------------------------------
	// EVENTS
	// --------------------------------------
	
	
	
	private void OnUserDataLoadFailed() {
		Debug.Log("Opps, user data load failed, something was wrong");
	}
	
	
	private void OnUserDataLoaded() {
		IsUserInfoLoaded = true;
		SPTwitter.instance.userInfo.LoadProfileImage();
		SPTwitter.instance.userInfo.LoadBackgroundImage();
		
		
		//style2.normal.textColor 							= SPTwitter.instance.userInfo.profile_text_color;
		//Camera.main.GetComponent<Camera>().backgroundColor  = SPTwitter.instance.userInfo.profile_background_color;
	}
	
	
	private void OnPost() {
		Debug.Log("Congrats, you just postet something to twitter");
	}
	
	private void OnPostFailed() {
		Debug.Log("Opps, post failed, something was wrong");
	}
	
	
	private void OnInit() {
		if(SPTwitter.instance.IsAuthed) {
			OnAuth();
		}
	}
	
	
	private void OnAuth() {
		IsAuntifivated = true;
	}
	
	// --------------------------------------
	// PRIVATE METHODS
	// --------------------------------------
	
	private IEnumerator PostScreenshot() {
		
		
		yield return new WaitForEndOfFrame();
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D( width, height, TextureFormat.RGB24, false );
		// Read screen contents into the texture
		tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		tex.Apply();
		
		SPTwitter.instance.Post("My app ScreehShot", tex);
		
		Destroy(tex);
		
	}
	
	private void LogOut() {
		IsUserInfoLoaded = false;
		
		IsAuntifivated = false;
		
		SPTwitter.instance.LogOut();
	}

}
