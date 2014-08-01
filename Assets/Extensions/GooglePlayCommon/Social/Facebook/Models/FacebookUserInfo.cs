////////////////////////////////////////////////////////////////////////////////
//  
// @module Common Android Native Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FacebookUserInfo : EventDispatcherBase {

	private string _id 			= string.Empty;
	private string _name 		= string.Empty;
	private string _first_name  = string.Empty;
	private string _last_name 	= string.Empty;
	private string _username 	= string.Empty;

	private string _profile_url = string.Empty;
	private string _email 		= string.Empty;
	
	private string _location 	= string.Empty;
	private string _locale 		= string.Empty;
	
	private string _rawJSON 	= string.Empty;

	private GoogleGenger _gender = GoogleGenger.Unknown;


	private Dictionary<FacebookProfileImageSize, Texture2D> profileImages =  new Dictionary<FacebookProfileImageSize, Texture2D>();


	public const string PROFILE_IMAGE_LOADED		 = "profile_image_loaded";



	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	



	public FacebookUserInfo(string data) {
		_rawJSON = data;

		IDictionary JSON =  ANMiniJSON.Json.Deserialize(_rawJSON) as IDictionary;	

		InitializeData(JSON);
	}


	public FacebookUserInfo(IDictionary JSON) {
		InitializeData(JSON);
	}

	public void InitializeData(IDictionary JSON) {

		

		if(JSON.Contains("id")) {
			_id 								= System.Convert.ToString(JSON["id"]);
		}

		if(JSON.Contains("name")) {
			_name 								= System.Convert.ToString(JSON["name"]);
		}

		if(JSON.Contains("first_name")) {
			_first_name 								= System.Convert.ToString(JSON["first_name"]);
		}

		if(JSON.Contains("last_name")) {
			_last_name 								= System.Convert.ToString(JSON["last_name"]);
		}

		if(JSON.Contains("username")) {
			_username 								= System.Convert.ToString(JSON["username"]);
		}

		if(JSON.Contains("link")) {
			_profile_url 								= System.Convert.ToString(JSON["link"]);
		}

		if(JSON.Contains("email")) {
			_email 								= System.Convert.ToString(JSON["email"]);
		}

		if(JSON.Contains("locale")) {
			_locale 								= System.Convert.ToString(JSON["locale"]);
		}

		if(JSON.Contains("location")) {
			IDictionary loc = JSON["location"] as IDictionary;
			_location							= System.Convert.ToString(loc["name"]);
		}

		if(JSON.Contains("gender")) {
			string g = System.Convert.ToString(JSON["gender"]);
			if(g.Equals("male")) {
				_gender = GoogleGenger.Male;
			} else {
				_gender = GoogleGenger.Female;
			}
		}


	}


	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------

	public string GetProfileUrl(FacebookProfileImageSize size) {
		return  "https://graph.facebook.com/" + id + "/picture?type=" + size.ToString();
	} 

	public Texture2D  GetProfileImage(FacebookProfileImageSize size) {
		if(profileImages.ContainsKey(size)) {
			return profileImages[size];
		} else {
			return null;
		}
	}

	public void LoadProfileImage(FacebookProfileImageSize size) {
		if(GetProfileImage(size) != null) {
			Debug.LogWarning("Profile image already loaded, size: " + size);
			dispatch(PROFILE_IMAGE_LOADED);
		}


		WWWTextureLoader loader = WWWTextureLoader.Create();

		switch(size) {
		case FacebookProfileImageSize.large:
			loader.addEventListener(BaseEvent.LOADED, OnLargeImageLoaded);
			break;
		case FacebookProfileImageSize.normal:
			loader.addEventListener(BaseEvent.LOADED, OnNormalImageLoaded);
			break;
		case FacebookProfileImageSize.small:
			loader.addEventListener(BaseEvent.LOADED, OnSmallImageLoaded);
			break;
		case FacebookProfileImageSize.square:
			loader.addEventListener(BaseEvent.LOADED, OnSquareImageLoaded);
			break;

		}

		Debug.Log("LOAD IMAGE URL: " + GetProfileUrl(size));

		loader.LoadTexture(GetProfileUrl(size));


	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------

	public string rawJSON {
		get {
			return _rawJSON;
		}
	}
	
	
	public string id {
		get {
			return _id;
		}
	}

	public string name {
		get {
			return _name;
		}
	}

	public string first_name {
		get {
			return _first_name;
		}
	}

	public string last_name {
		get {
			return _last_name;
		}
	}


	public string username {
		get {
			return _username;
		}
	}


	public string profile_url {
		get {
			return _profile_url;
		}
	}

	public string email {
		get {
			return _email;
		}
	}


	public string locale {
		get {
			return _locale;
		}
	}

	public string location {
		get {
			return _location;
		}
	}


	public GoogleGenger gender {
		get {
			return _gender;
		}
	}

	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------

	private void OnSquareImageLoaded(CEvent e) {
		Debug.Log("OnSquareImageLoaded");
		e.dispatcher.removeEventListener(BaseEvent.LOADED, OnSquareImageLoaded);
		Texture2D image = e.data as Texture2D;
		if(image != null && !profileImages.ContainsKey(FacebookProfileImageSize.square)) {
			profileImages.Add(FacebookProfileImageSize.square, image);
		}
		
		dispatch(PROFILE_IMAGE_LOADED);
	}

	private void OnLargeImageLoaded(CEvent e) {
		e.dispatcher.removeEventListener(BaseEvent.LOADED, OnLargeImageLoaded);
		Texture2D image = e.data as Texture2D;
		if(image != null && !profileImages.ContainsKey(FacebookProfileImageSize.large)) {
			profileImages.Add(FacebookProfileImageSize.large, image);
		}
		
		dispatch(PROFILE_IMAGE_LOADED);
	}


	private void OnNormalImageLoaded(CEvent e) {
		e.dispatcher.removeEventListener(BaseEvent.LOADED, OnNormalImageLoaded);
		Texture2D image = e.data as Texture2D;
		if(image != null && !profileImages.ContainsKey(FacebookProfileImageSize.normal)) {
			profileImages.Add(FacebookProfileImageSize.normal, image);
		}
		
		dispatch(PROFILE_IMAGE_LOADED);
	}

	private void OnSmallImageLoaded(CEvent e) {
		e.dispatcher.removeEventListener(BaseEvent.LOADED, OnSmallImageLoaded);
		Texture2D image = e.data as Texture2D;
		if(image != null && !profileImages.ContainsKey(FacebookProfileImageSize.small)) {
			profileImages.Add(FacebookProfileImageSize.small, image);
		}
		
		dispatch(PROFILE_IMAGE_LOADED);
	}




	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------

}
