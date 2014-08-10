////////////////////////////////////////////////////////////////////////////////
//  
// @module Common Android Native Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


 

using UnityEngine;
using Facebook;
using System.Collections;
using System.Collections.Generic;

public class SPFacebook : SA_Singleton<SPFacebook> {


	private FacebookUserInfo _userInfo = null;
	private Dictionary<string,  FacebookUserInfo> _friends;
	private bool _IsInited = false;

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	public void Init() {
		FB.Init(OnInitComplete, OnHideUnity);
	}


	public void Login() {
		Login(SocialPlatfromSettings.Instance.fb_scopes);
	}

	public void Login(string scopes) {
		Debug.Log("AndroidNative login");
		FB.Login(scopes, LoginCallback);
	}


	//--------------------------------------
	//  API METHODS
	//--------------------------------------


	public void Logout() {
		FB.Logout();
	}

	public void LoadUserData() {
		if(IsLoggedIn) {
			
			FB.API("/me", Facebook.HttpMethod.GET, UserDataCallBack);  
			
		} else {
			Debug.LogWarning("Auth user before loadin data, fail event generated");
			dispatch(FacebookEvents.USER_DATA_FAILED_TO_LOAD, null);
		}
	}

	public void LoadFrientdsInfo(int limit) {
		if(IsLoggedIn) {
			
			FB.API("/me?fields=friends.limit(" + limit.ToString() + ").fields(first_name,id,last_name,name,link,locale,location)", Facebook.HttpMethod.GET, FriendsDataCallBack);  


		} else {
			Debug.LogWarning("Auth user before loadin data, fail event generated");
			dispatch(FacebookEvents.FRIENDS_FAILED_TO_LOAD, null);
		}
	}

	public FacebookUserInfo GetFrindById(string id) {
		if(_friends != null) {
			if(_friends.ContainsKey(id)) {
				return _friends[id];
			}
		}

		return null;
	}




	public void PostImage(string caption, Texture2D image) {


		byte[] imageBytes = image.EncodeToPNG();

		WWWForm wwwForm = new WWWForm();
		wwwForm.AddField("message", caption);
		wwwForm.AddBinaryData("image", imageBytes, "InteractiveConsole.png");


		FB.API("me/photos", Facebook.HttpMethod.POST, PostCallBack, wwwForm);


	}


	public FBPostingTask PostWithAuthCheck(
		string toId = "",
		string link = "",
		string linkName = "",
		string linkCaption = "",
		string linkDescription = "",
		string picture = "",
		string actionName = "",
		string actionLink = "",
		string reference = ""
		) {

		FBPostingTask task = FBPostingTask.Cretae();

		task.Post(toId,
		          link,
		          linkName,
		          linkCaption,
		          linkDescription,
		          picture,
		          actionName,
		          actionLink,
		          reference);


		return task;

	} 



	public void Post (
								string toId = "",
								string link = "",
								string linkName = "",
								string linkCaption = "",
								string linkDescription = "",
								string picture = "",
								string actionName = "",
								string actionLink = "",
								string reference = ""
							) 
	{

		if(!IsLoggedIn) { 
			Debug.LogWarning("Auth user before posting, fail event generated");
			dispatch(FacebookEvents.POST_FAILED, null);
			return;
		}

		FB.Feed(
			toId: toId,
			link: link,
			linkName: linkName,
			linkCaption: linkCaption,
			linkDescription: linkDescription,
			picture: picture,
			actionName : actionName,
			actionLink : actionLink,
			reference : reference,
			callback: PostCallBack
			);

	}





	public void AppRequest(

		string message,
		OGActionType actionType,
		string objectId,
		string[] to,
		string data = "",
		string title = "")
	{

		if(!IsLoggedIn) { 
			Debug.LogWarning("Auth user before AppRequest, fail event generated");
			dispatch(FacebookEvents.APP_REQUEST_COMPLETE, new FBResult("","User isn't authed"));
			return;
		}

		FB.AppRequest(message, actionType, objectId, to, data, title, AppRequestCallBack);
	}
	
	public void AppRequest(
		string message,
		OGActionType actionType,
		string objectId,
		string filters = "",
		string[] excludeIds = null,
		int? maxRecipients = null,
		string data = "",
		string title = "")
	{
		if(!IsLoggedIn) { 
			Debug.LogWarning("Auth user before AppRequest, fail event generated");
			dispatch(FacebookEvents.APP_REQUEST_COMPLETE, new FBResult("","User isn't authed"));
			return;
		}

		FB.AppRequest(message, actionType, objectId, filters, excludeIds, maxRecipients, data, title, AppRequestCallBack);
	}
	
	public void AppRequest(
		string message,
		string[] to = null,
		string filters = "",
		string[] excludeIds = null,
		int? maxRecipients = null,
		string data = "",
		string title = "")
	{
		if(!IsLoggedIn) { 
			Debug.LogWarning("Auth user before AppRequest, fail event generated");
			dispatch(FacebookEvents.APP_REQUEST_COMPLETE, new FBResult("","User isn't authed"));
			return;
		}

		FB.AppRequest(message, to, filters, excludeIds, maxRecipients, data, title, AppRequestCallBack);
	}


	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------


	public bool IsInited  {
		get {
			return _IsInited;
		}
	}

	public bool IsLoggedIn {
		get {
			return FB.IsLoggedIn;
		}
	}


	public string UserId {
		get {
			return FB.UserId;
		}
	}

	public string AccessToken {
		get {
			return FB.AccessToken;
		}
	}

	public FacebookUserInfo userInfo {
		get {
			return _userInfo;
		}
	}

	public Dictionary<string,  FacebookUserInfo> friends {
		get {
			return _friends;
		}
	}

	public List<string> friendsIds {
		get {
			if(_friends == null) {
				return null;
			}

			List<string> ids = new List<string>();
			foreach(KeyValuePair<string, FacebookUserInfo> item in _friends) {
				ids.Add(item.Key);
			}

			return ids;
		}
	}

	public List<FacebookUserInfo> friendsList {
		get {
			if(_friends == null) {
				return null;
			}
			
			List<FacebookUserInfo> flist = new List<FacebookUserInfo>();
			foreach(KeyValuePair<string, FacebookUserInfo> item in _friends) {
				flist.Add(item.Value);
			}

			return flist;
		}
	}




	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------

	private void PostCallBack(FBResult result) {
		if (result.Error != null)  {                                                                                                                          
			Debug.LogWarning(result.Error);
			dispatch(FacebookEvents.POST_FAILED, result);
			return;
		}          

		dispatch(FacebookEvents.POST_SUCCEEDED, result);
	}


	private void AppRequestCallBack(FBResult result) {
		dispatch(FacebookEvents.APP_REQUEST_COMPLETE, result);
	}


	private void FriendsDataCallBack(FBResult result) {
		if (result.Error != null)  {                                                                                                                          
			Debug.LogWarning(result.Error);
			dispatch(FacebookEvents.FRIENDS_FAILED_TO_LOAD, result);
			
			return;
		}          
		
		ParceFriendsData(result.Text);
		dispatch(FacebookEvents.FRIENDS_DATA_LOADED, result);
	}


	public void ParceFriendsData(string data) {

		Debug.Log("ParceFriendsData");
		Debug.Log(data);

		try {
			_friends =  new Dictionary<string, FacebookUserInfo>();
			IDictionary JSON =  ANMiniJSON.Json.Deserialize(data) as IDictionary;	
			IDictionary f = JSON["friends"] as IDictionary;
			IList flist = f["data"] as IList;


			for(int i = 0; i < flist.Count; i++) {
				FacebookUserInfo user = new FacebookUserInfo(flist[i] as IDictionary);
				_friends.Add(user.id, user);
			}

		} catch(System.Exception ex) {
			Debug.LogWarning("Parceing Friends Data failed");
			Debug.LogWarning(ex.Message);
		}

	}



	private void UserDataCallBack(FBResult result) {
		if (result.Error != null)  {         

			Debug.LogWarning(result.Error);
			dispatch(FacebookEvents.USER_DATA_FAILED_TO_LOAD, result);

			return;
		}          

		_userInfo = new FacebookUserInfo(result.Text);
		dispatch(FacebookEvents.USER_DATA_LOADED, result);

	}


	private void OnInitComplete() {
		_IsInited = true;
		dispatch(FacebookEvents.FACEBOOK_INITED);

		Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
	}



	private void OnHideUnity(bool isGameShown) {
		dispatch(FacebookEvents.GAME_FOCUS_CHANGED, isGameShown);
	}
	
	
	private void LoginCallback(FBResult result) {
		if(FB.IsLoggedIn) {
			dispatch(FacebookEvents.AUTHENTICATION_SUCCEEDED, result);
		} else {
			dispatch(FacebookEvents.AUTHENTICATION_FAILED, result);
		}
	}



	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------

}
