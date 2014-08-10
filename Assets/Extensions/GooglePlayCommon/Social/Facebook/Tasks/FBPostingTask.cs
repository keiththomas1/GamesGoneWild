using UnityEngine;
using System.Collections;

public class FBPostingTask : AsyncTask {

	private string _toId = "";
	private string _link = "";
	private string _linkName = "";
	private string _linkCaption = "";
	private string _linkDescription = "";
	private string _picture = "";
	private string _actionName = "";
	private string _actionLink = "";
	private string _reference = "";



	public static FBPostingTask Cretae() {
		return	new GameObject("PostingTask").AddComponent<FBPostingTask>();
	}


	public void Post(
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

		_toId = toId;
		_link = link;
		_linkName = linkName;
		_linkCaption = linkCaption;
		_linkDescription = linkDescription;
		_picture = picture;
		_actionName = actionName;
		_actionLink = actionLink;
		_reference = reference;


		if(SPFacebook.instance.IsInited) {
			OnFBInited();
		} else {
			SPFacebook.instance.addEventListener(FacebookEvents.FACEBOOK_INITED, 			 OnFBInited);
			SPFacebook.instance.Init();
		}


	}


	private void OnFBInited() {
		SPFacebook.instance.removeEventListener(FacebookEvents.FACEBOOK_INITED, 			 OnFBInited);
		if(SPFacebook.instance.IsLoggedIn) {
			OnFBAuth();
		} else {
			SPFacebook.instance.addEventListener(FacebookEvents.AUTHENTICATION_FAILED,  	 OnFBAuthFailed);
			SPFacebook.instance.addEventListener(FacebookEvents.AUTHENTICATION_SUCCEEDED,  	 OnFBAuth);
			SPFacebook.instance.Login();
		}
	}


	private void OnFBAuth() {
		SPFacebook.instance.removeEventListener(FacebookEvents.AUTHENTICATION_FAILED,  	     OnFBAuthFailed);
		SPFacebook.instance.removeEventListener(FacebookEvents.AUTHENTICATION_SUCCEEDED,  	 OnFBAuth);


		SPFacebook.instance.addEventListener(FacebookEvents.POST_FAILED,  			OnPostFailed);
		SPFacebook.instance.addEventListener(FacebookEvents.POST_SUCCEEDED,   		OnPost);

		SPFacebook.instance.Post(_toId,
		                         _link,
		                         _linkName,
		                         _linkCaption,
		                         _linkDescription,
		                         _picture,
		                         _actionName,
		                         _actionLink,
		                         _reference);

	}
	private void OnFBAuthFailed() {
		SPFacebook.instance.removeEventListener(FacebookEvents.AUTHENTICATION_FAILED,  	 	OnFBAuthFailed);
		SPFacebook.instance.removeEventListener(FacebookEvents.AUTHENTICATION_SUCCEEDED,  	OnFBAuth);

		FBResult res =  new FBResult("", "Auth failed");
		dispatch(BaseEvent.COMPLETE, res);
	}


	private void OnPost(CEvent e) {
		FBResult res = e.data as FBResult;
		dispatch(BaseEvent.COMPLETE, res);
	}

	private void OnPostFailed(CEvent e) {
		FBResult res = e.data as FBResult;
		dispatch(BaseEvent.COMPLETE, res);
	}

}
