//#define SA_DEBUG_MODE
////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////




using UnityEngine;
using System.Collections;

#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
using System.Runtime.InteropServices;
#endif

public class MSPIOSSocialManager : EventDispatcher {


	#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
	[DllImport ("__Internal")]
	private static extern void _MSP_TwPost(string text);
	
	[DllImport ("__Internal")]
	private static extern void _MSP_TwPostWithMedia(string text, string encodedMedia);
	

	[DllImport ("__Internal")]
	private static extern void _MSP_FbPost(string text);
	
	[DllImport ("__Internal")]
	private static extern void _MSP_FbPostWithMedia(string text, string encodedMedia);

	[DllImport ("__Internal")]
	private static extern void _MSP_MediaShare(string text, string encodedMedia);
	#endif



	private static MSPIOSSocialManager _instance = null;


	
	public const string TWITTER_POST_FAILED  = "twitter_post_failed";
	public const string TWITTER_POST_SUCCESS = "twitter_post_success";
	
	public const string FACEBOOK_POST_FAILED  = "facebook_post_failed";
	public const string FACEBOOK_POST_SUCCESS = "facebook_post_success";


	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}



	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------

	public void ShareMedia(string text) {
		ShareMedia(text, null);
	}

	public void ShareMedia(string text, Texture2D texture) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			if(texture != null) {
				byte[] val = texture.EncodeToPNG();
				string bytesString = System.Convert.ToBase64String (val);
				_MSP_MediaShare(text, bytesString);
			} else {
				_MSP_MediaShare(text, "");
			}
		#endif
	}

	public void TwitterPost(string text) {
		TwitterPost(text, null);
	}


	public void TwitterPost(string text, Texture2D texture) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE

		if(texture == null) {
			_MSP_TwPost(text);
		} else {
			byte[] val = texture.EncodeToPNG();
			string bytesString = System.Convert.ToBase64String (val);
			_MSP_TwPostWithMedia(text, bytesString);
		}
		#endif

	}


	public void FacebookPost(string text) {
		FacebookPost(text, null);
	}
	
	public void FacebookPost(string text, Texture2D texture) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
		if(texture == null) {
			_MSP_FbPost(text);
		} else {
			byte[] val = texture.EncodeToPNG();
			string bytesString = System.Convert.ToBase64String (val);
			_MSP_FbPostWithMedia(text, bytesString);
		}
		#endif
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------

	public static MSPIOSSocialManager instance  {
		get {
			if(_instance == null) {
				GameObject go =  new GameObject("IOSSocialManager");
				_instance = go.AddComponent<MSPIOSSocialManager>();
			}

			return _instance;
		}
	}
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------

	private void OnTwitterPostFailed() {
		dispatch(TWITTER_POST_FAILED);
	}

	private void OnTwitterPostSuccess() {
		dispatch(TWITTER_POST_SUCCESS);
	}

	private void OnFacebookPostFailed() {
		dispatch(FACEBOOK_POST_FAILED);
	}
	
	private void OnFacebookPostSuccess() {
		dispatch(FACEBOOK_POST_SUCCESS);
	}
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------


	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------

}
