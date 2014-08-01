////////////////////////////////////////////////////////////////////////////////
//  
// @module <module_name>
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class MSPFacebookUseExample : MonoBehaviour {

	private static bool IsUserInfoLoaded = false;
	private static bool IsFrindsInfoLoaded = false;
	private static bool IsAuntifivated = false;
	
	
	private string statusMessage = "";
	
	public GameObject[] showedOnAuth;
	
	public DefaultPreviewButton connectButton;
	public SA_Texture avatar;
	public SA_Label Location;
	public SA_Label Language;
	public SA_Label Mail;
	public SA_Label Name;
	
	
	public SA_Label f1;
	public SA_Label f2;
	
	public SA_Texture fi1;
	public SA_Texture fi2;
	
	
	public Texture2D ImageToShare;
	
	public GameObject friends;
	
	
	void Awake() {
		



		SPFacebook.instance.addEventListener(FacebookEvents.FACEBOOK_INITED, 			 OnInit);
		SPFacebook.instance.addEventListener(FacebookEvents.AUTHENTICATION_SUCCEEDED,  	 OnAuth);
		
		
		SPFacebook.instance.addEventListener(FacebookEvents.USER_DATA_LOADED,  			OnUserDataLoaded);
		SPFacebook.instance.addEventListener(FacebookEvents.USER_DATA_FAILED_TO_LOAD,   OnUserDataLoadFailed);
		
		SPFacebook.instance.addEventListener(FacebookEvents.FRIENDS_DATA_LOADED,  			OnFriendsDataLoaded);
		SPFacebook.instance.addEventListener(FacebookEvents.FRIENDS_FAILED_TO_LOAD,   		OnFriendDataLoadFailed);
		
		SPFacebook.instance.addEventListener(FacebookEvents.POST_FAILED,  			OnPostFailed);
		SPFacebook.instance.addEventListener(FacebookEvents.POST_SUCCEEDED,   		OnPost);
		
		
		SPFacebook.instance.addEventListener(FacebookEvents.GAME_FOCUS_CHANGED,   OnFocusChanged);
		

		SPFacebook.instance.Init();
		
		statusMessage = "initializing Facebook";
		
		
		
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
			
			friends.SetActive(false);
			return;
		}
		
		if(IsUserInfoLoaded) {
			if(SPFacebook.instance.userInfo.GetProfileImage(FacebookProfileImageSize.square) != null) {
				avatar.texture = SPFacebook.instance.userInfo.GetProfileImage(FacebookProfileImageSize.square);
				Name.text = SPFacebook.instance.userInfo.name + " aka " + SPFacebook.instance.userInfo.username;
				Location.text = SPFacebook.instance.userInfo.location;
				Language.text = SPFacebook.instance.userInfo.locale;
			}
		}
		
		
		if(IsFrindsInfoLoaded) {
			friends.SetActive(true);
			int i = 0;
			foreach(FacebookUserInfo friend in SPFacebook.instance.friendsList) {
				
				if(i == 0) {
					f1.text = friend.name;
					if(friend.GetProfileImage(FacebookProfileImageSize.square) != null) {
						fi1.texture = friend.GetProfileImage(FacebookProfileImageSize.square);
					} 
				} else {
					f2.text = friend.name;
					if(friend.GetProfileImage(FacebookProfileImageSize.square) != null) {
						fi2.texture = friend.GetProfileImage(FacebookProfileImageSize.square);
					} 
				}
				
				i ++;
			}
		} else {
			friends.SetActive(false);
		}
		
		
		
	}
	
	
	private void PostWithAuthCheck() {
		SPFacebook.instance.PostWithAuthCheck (
			link: "https://example.com/myapp/?storyID=thelarch",
			linkName: "The Larch",
			linkCaption: "I thought up a witty tagline about larches",
			linkDescription: "There are a lot of larch trees around here, aren't there?",
			picture: "https://example.com/myapp/assets/1/larch.jpg"
			);
	}
	
	
	private void PostNativeScreenshot() {
		StartCoroutine(PostFBScreenshot());
	}
	
	private void PostImage() {
		SPShareUtility.FacebookShare("This is my text to share", ImageToShare);
	}

	private void PostMSG() {
		SPShareUtility.FacebookShare("This is my text to share");
	}
	
	
	private IEnumerator PostFBScreenshot() {
		
		
		yield return new WaitForEndOfFrame();
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D( width, height, TextureFormat.RGB24, false );
		// Read screen contents into the texture
		tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		tex.Apply();

		SPShareUtility.FacebookShare("This is my text to share", tex);
	
		
		Destroy(tex);
		
	}
	
	
	private void Connect() {
		if(!IsAuntifivated) {
			SPFacebook.instance.Login();
			statusMessage = "Log in...";
		} else {
			LogOut();
			statusMessage = "Logged out";
		}
	}
	
	private void LoadUserData() {
		SPFacebook.instance.LoadUserData();
		statusMessage = "Loadin user data..";
	}
	
	private void PostMessage() {
		SPFacebook.instance.Post (
			link: "https://example.com/myapp/?storyID=thelarch",
			linkName: "The Larch",
			linkCaption: "I thought up a witty tagline about larches",
			linkDescription: "There are a lot of larch trees around here, aren't there?",
			picture: "https://example.com/myapp/assets/1/larch.jpg"
			);
		
		statusMessage = "Positng..";
	}
	
	private void PostScreehShot() {
		StartCoroutine(PostScreenshot());
		statusMessage = "Positng..";
	}
	
	private void LoadFriends() {
		SPFacebook.instance.LoadFrientdsInfo(5);
		statusMessage = "Loading friends..";
	}
	
	private void AppRequest() {
		SPFacebook.instance.AppRequest("Come play this great game!");
	}
	
	
	
	void OnGUI() {
		GUIStyle statusStyle =  new GUIStyle();
		statusStyle.fontSize = 16;
		statusStyle.normal.textColor = Color.black;
		statusStyle.fontStyle = FontStyle.Italic;
		statusStyle.alignment = TextAnchor.UpperLeft;
		GUI.Label(new Rect(Screen.width - 200, Screen.height - 20, Screen.width, 40), statusMessage, statusStyle);
	}
	
	
	// --------------------------------------
	// EVENTS
	// --------------------------------------
	
	
	private void OnFocusChanged(CEvent e) {
		bool focus = (bool) e.data;
		
		if (!focus)  {                                                                                        
			// pause the game - we will need to hide                                             
			Time.timeScale = 0;                                                                  
		} else  {                                                                                        
			// start the game back up - we're getting focus again                                
			Time.timeScale = 1;                                                                  
		}   
	}
	
	
	private void OnUserDataLoadFailed() {
		statusMessage ="Opps, user data load failed, something was wrong";
		Debug.Log("Opps, user data load failed, something was wrong");
	}
	
	
	private void OnUserDataLoaded() {
		statusMessage = "User data loaded";
		IsUserInfoLoaded = true;
		SPFacebook.instance.userInfo.LoadProfileImage(FacebookProfileImageSize.square);
	}
	
	private void OnFriendDataLoadFailed() {
		statusMessage = "Opps, friends data load failed, something was wrong";
		Debug.Log("Opps, friends data load failed, something was wrong");
	}
	
	private void OnFriendsDataLoaded() {
		statusMessage = "Friends data loaded";
		foreach(FacebookUserInfo friend in SPFacebook.instance.friendsList) {
			friend.LoadProfileImage(FacebookProfileImageSize.square);
		}
		
		IsFrindsInfoLoaded = true;
	}
	
	
	
	
	private void OnInit() {
		
		if(SPFacebook.instance.IsLoggedIn) {
			OnAuth();
		} else {
			statusMessage = "user Login -> fale";
		}
	}
	
	
	private void OnAuth() {
		IsAuntifivated = true;
		statusMessage = "user Login -> true";
	}
	
	private void OnPost() {
		statusMessage = "Posting complete";
	}
	
	private void OnPostFailed() {
		statusMessage = "Opps, post failed, something was wrong";
		Debug.Log("Opps, post failed, something was wrong");
	}
	
	
	
	
	// --------------------------------------
	// PRIVATE METHODS
	// --------------------------------------
	
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
		
		SPFacebook.instance.PostImage("My app ScreehShot", tex);;
		
		Destroy(tex);
		
	}
	
	private void LogOut() {
		IsUserInfoLoaded = false;
		
		IsAuntifivated = false;
		
		SPFacebook.instance.Logout();
	}
	


}
