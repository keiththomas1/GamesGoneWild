////////////////////////////////////////////////////////////////////////////////
//  
// @module Mobile Social Plugin 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;

public class SPTwitter : MonoBehaviour,  TwitterManagerInterface {



	private static SPTwitter _instance = null;
	private static TwitterManagerInterface _twitterGate = null;



	// --------------------------------------
	// INITIALIZATION
	// --------------------------------------

	void Awake() {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			_twitterGate = AndroidTwitterManager.instance;
			break;
		default:
			_twitterGate = IOSTwitterManager.instance;
			break;
		}
		DontDestroyOnLoad(gameObject);
	}


	public static SPTwitter instance {
		get {
			if(_instance == null) {
				_instance = new GameObject("SPTwitter").AddComponent<SPTwitter>();
			}

			return _instance;
		}
	}

	public void Init() {
		_twitterGate.Init();
	}

	public void Init(string consumer_key, string consumer_secret) {
		_twitterGate.Init(consumer_key, consumer_secret);
	}



	// --------------------------------------
	// PUBLIC METHODS
	// --------------------------------------


	public void AuthenticateUser() {
		if(_twitterGate != null) {
			_twitterGate.AuthenticateUser();
		} else {
			Debug.Log("SPTwitter.Init should be called before SPTwitter.AuthificateUser");
		}

	}
	
	public void LoadUserData() {
		if(_twitterGate != null) {
			_twitterGate.LoadUserData();
		} else {
			Debug.Log("SPTwitter.Init should be called before SPTwitter.LoadUserData");
		}
	}

	
	public void Post(string status) {
		if(_twitterGate != null) {
			_twitterGate.Post(status);
		} else {
			Debug.Log("SPTwitter.Init should be called before SPTwitter.Post");
		}
	}

	public void Post(string status, Texture2D texture ) {
		if(_twitterGate != null) {
			_twitterGate.Post(status, texture);
		} else {
			Debug.Log("SPTwitter.Init should be called before SPTwitter.Post");
		}
	}




	public TwitterPostingTask PostWithAuthCheck(string status) {
		return PostWithAuthCheck(status, null);
	}

	public TwitterPostingTask PostWithAuthCheck(string status, Texture2D texture ) {
		if(_twitterGate != null) {
			return _twitterGate.PostWithAuthCheck(status, texture);
		} else {
			Debug.Log("SPTwitter.Init should be called before SPTwitter.Post");
			return null;
		}
	}
	

	public void LogOut() {
		if(_twitterGate != null) {
			_twitterGate.LogOut();
		} else {
			Debug.Log("SPTwitter.Init should be called before SPTwitter.LogOut");
		}
	}



	// --------------------------------------
	// GET / SET
	// --------------------------------------


	public bool IsAuthed {
		get {
			return _twitterGate.IsAuthed;
		}
	}
	public bool IsInited {
		get {
			return _twitterGate.IsInited;
		}
	}

	public TwitterUserInfo userInfo  {
		get {
			return _twitterGate.userInfo;
		}
	}
	

	public void addEventListener(string eventName, 	EventHandlerFunction handler) {
		_twitterGate.addEventListener(eventName, handler);
	}

	public void addEventListener(string eventName, 	DataEventHandlerFunction handler) {
		_twitterGate.addEventListener(eventName, handler);
	}

	public void addEventListener(int eventID, 	EventHandlerFunction handler) {
		_twitterGate.addEventListener(eventID, handler);
	}
	
	public void addEventListener(int eventID, 	DataEventHandlerFunction handler) {
		_twitterGate.addEventListener(eventID, handler);
	}


	public void removeEventListener(string eventName, 	EventHandlerFunction handler) {
		_twitterGate.removeEventListener(eventName, handler);
	}

	public void removeEventListener(string eventName,  DataEventHandlerFunction handler) {
		_twitterGate.removeEventListener(eventName, handler);
	}

	public void removeEventListener(int eventID, 	EventHandlerFunction handler) {
		_twitterGate.removeEventListener(eventID, handler);
	}
	
	public void removeEventListener(int eventID,  DataEventHandlerFunction handler) {
		_twitterGate.removeEventListener(eventID, handler);
	}


	//--------------------------------------
	// DISPATCH I1
	//--------------------------------------
	
	public void dispatchEvent(int eventID) {
		_twitterGate.dispatchEvent(eventID);
	}
	public void dispatchEvent(int eventID, object data) {
		_twitterGate.dispatchEvent(eventID, data);
	}

	public void dispatchEvent(string eventName) {
		_twitterGate.dispatchEvent(eventName);
	}

	public void dispatchEvent(string eventName, object data) {
		_twitterGate.dispatchEvent(eventName, data);
	}
	
	
	//--------------------------------------
	// DISPATCH I2
	//--------------------------------------
	
	
	public void dispatch(int eventID) {
		_twitterGate.dispatch(eventID);
	}

	public void dispatch(int eventID, object data) {
		_twitterGate.dispatch(eventID, data);
	}

	public void dispatch(string eventName) {
		_twitterGate.dispatch(eventName);
	}

	public void dispatch(string eventName, object data) {
		_twitterGate.dispatch(eventName, data);
	}

	
	//--------------------------------------
	// METHODS
	//--------------------------------------
	
	public void clearEvents() {
		_twitterGate.clearEvents();
	}

}
