using UnityEngine;
using System.Collections;

public class TwitterController : MonoBehaviour {

	public bool isAuthenticated = false;

	// Use this for initialization
	void Start () {
	
		Object.DontDestroyOnLoad( this );

		SPTwitter.instance.Init("8hRLFbccSGeLbWwbA6r7DYDrh","GZzjXcv66dvCRyq9wW5YjcWie7R31RA9hNte2bq0BY0JELQmAA");

		//Events to listen to
		SPTwitter.instance.addEventListener(TwitterEvents.TWITTER_INITED,  OnInit);
		SPTwitter.instance.addEventListener(TwitterEvents.AUTHENTICATION_SUCCEEDED,  OnAuth);
		SPTwitter.instance.addEventListener(TwitterEvents.AUTHENTICATION_FAILED,  OnAuthFailed);
		SPTwitter.instance.addEventListener(TwitterEvents.POST_SUCCEEDED,  OnPost);
		SPTwitter.instance.addEventListener(TwitterEvents.POST_FAILED,  OnPostFailed);




	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnInit() {
		if (SPTwitter.instance.IsAuthed) {
			isAuthenticated = true;
			Debug.Log ("User is Authenticated");
		}
		else {
			Debug.Log("User is still not authenticated");
			SPTwitter.instance.AuthenticateUser();
		}
	}

	private void OnAuth() {
		if (SPTwitter.instance.IsAuthed) {
			isAuthenticated = true;
			Debug.Log ("User is Authenticated");
		}
		else {
			Debug.Log("User is still not authenticated");
		}
	}

	private void OnPost(){
		Debug.Log ("User posted!");
	}
	private void OnPostFailed(){
		Debug.Log ("User post FAILED!");
	}
	private void OnAuthFailed() {
		Debug.Log ("Authentication FAILED");
	}

	public void twitterPost() {
		if (isAuthenticated){
			SPTwitter.instance.Post ("This is a Twitter Post");
			Debug.Log ("User is supposedly authenticated");
		}
		else {
			Debug.Log ("Attempting to Authenticate again");
			SPTwitter.instance.AuthenticateUser();
		}
		
	}
}

