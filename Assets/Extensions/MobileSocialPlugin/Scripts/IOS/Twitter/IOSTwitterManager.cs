////////////////////////////////////////////////////////////////////////////////
//  
// @module Mobile Social Plugin 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;

#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
using System.Runtime.InteropServices;
#endif

public class IOSTwitterManager : SA_Singleton<IOSTwitterManager>, TwitterManagerInterface {


	//error codes for failed auth
	private const int NO_TWITTER_ACCOUNTS_ON_DEVICE = 0;
	private const int TWITTER_PERMISSION_DENIED = 1;


	#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
	[DllImport ("__Internal")]
	private static extern void _twitterInit();

	[DllImport ("__Internal")]
	private static extern void _twitterLoadUserData();

	[DllImport ("__Internal")]
	private static extern void _twitterAuthificateUser();



	[DllImport ("__Internal")]
	private static extern void _twitterPost(string text);

	[DllImport ("__Internal")]
	private static extern void _twitterPostWithMedia(string text, string encodedMedia);
	#endif

	private bool _IsAuthed = false;
	private bool _IsInited = false;
	
	private TwitterUserInfo _userInfo;
	
	
	// --------------------------------------
	// INITIALIZATION
	// --------------------------------------


	void Awake() {
		DontDestroyOnLoad(gameObject);
	}


	public void Init(string consumer_key, string consumer_secret) {
		Init();
	}


	public void Init() {
		if(_IsInited) {
			return;
		}
		
		_IsInited = true;

		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			_twitterInit();
		#endif
	}
	
	
	// --------------------------------------
	// PUBLIC METHODS
	// --------------------------------------
	
	
	public void AuthenticateUser() {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
		Debug.Log("Unity AuthificateUser");
		if(_IsAuthed) {
			OnAuthSuccess();
		} else {
			_twitterAuthificateUser();

		}
		#endif
	}
	
	public void LoadUserData() {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			_twitterLoadUserData();
		#endif
	}


	public void Post(string status) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			_twitterPost(status);
		#endif
	}
	
	public void Post(string status, Texture2D texture) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
		byte[] val = texture.EncodeToPNG();
		string bytesString = System.Convert.ToBase64String (val);


		_twitterPostWithMedia(status, bytesString);

		#endif
	}

	public TwitterPostingTask PostWithAuthCheck(string status)  {
		return PostWithAuthCheck(status, null);
	}
	
	public TwitterPostingTask PostWithAuthCheck(string status, Texture2D texture) {
		TwitterPostingTask task =  TwitterPostingTask.Cretae();
		task.Post(status, texture, this);
		return task;
		
	}

	
	public void LogOut() {

	}

	
	// --------------------------------------
	// GET / SET
	// --------------------------------------
	
	public bool IsAuthed {
		get {
			return _IsAuthed;
		}
	}

	public bool IsInited {
		get {
			return _IsInited;
		}
	}


	public TwitterUserInfo userInfo {
		get {
			return _userInfo;
		}
	}
	
	
	
	// --------------------------------------
	// EVENTS
	// --------------------------------------
	
	
	
	private void OnInited(string data) {
		if(data.Equals("1")) {
			_IsAuthed = true;
		}
		
		dispatch(TwitterEvents.TWITTER_INITED);
	}
	
	private void OnAuthSuccess() {
		_IsAuthed = true;
		dispatch(TwitterEvents.AUTHENTICATION_SUCCEEDED);
	}
	
	
	private void OnAuthFailed(string data) {
		dispatch(TwitterEvents.AUTHENTICATION_FAILED, System.Convert.ToInt32(data));
	}
	
	private void OnPostSuccess() {
		dispatch(TwitterEvents.POST_SUCCEEDED);
	}
	
	
	private void OnPostFailed() {
		dispatch(TwitterEvents.POST_FAILED);
	}
	
	
	private void OnUserDataLoaded(string data) {
		_userInfo =  new TwitterUserInfo(data);
		dispatch(TwitterEvents.USER_DATA_LOADED);

	}
	
	
	private void OnUserDataLoadFailed() {
		dispatch(TwitterEvents.USER_DATA_FAILED_TO_LOAD);
	}
}
