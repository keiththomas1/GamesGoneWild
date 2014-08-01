////////////////////////////////////////////////////////////////////////////////
//  
// @module Mobile Social Plugin 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;

public class SPInstagram  {

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------


	public static void Share(Texture2D texture) {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			IOSInstagramManager.instance.Share(texture);
			break;
		case RuntimePlatform.Android:
			AndroidInstagramManager.instance.Share(texture);
			break;
		}

	}
	
	
	public static void Share(Texture2D texture, string message) {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			IOSInstagramManager.instance.Share(texture, message);
			break;
		case RuntimePlatform.Android:
			AndroidInstagramManager.instance.Share(texture, message);
			break;
		}
	}


	public static void addEventListener(string eventName, EventHandlerFunction handler) {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			IOSInstagramManager.instance.addEventListener(eventName, handler);
			break;
		case RuntimePlatform.Android:
			AndroidInstagramManager.instance.addEventListener(eventName, handler);
			break;
		}
	}
	
	public static void addEventListener(string eventName, DataEventHandlerFunction handler) {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			IOSInstagramManager.instance.addEventListener(eventName, handler);
			break;
		case RuntimePlatform.Android:
			AndroidInstagramManager.instance.addEventListener(eventName, handler);
			break;
		}
	}


	public static void removeEventListener(string eventName, EventHandlerFunction handler) {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			IOSInstagramManager.instance.removeEventListener(eventName, handler);
			break;
		case RuntimePlatform.Android:
			AndroidInstagramManager.instance.removeEventListener(eventName, handler);
			break;
		}
	}
	
	public static void removeEventListener(string eventName, DataEventHandlerFunction handler) {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			IOSInstagramManager.instance.removeEventListener(eventName, handler);
			break;
		case RuntimePlatform.Android:
			AndroidInstagramManager.instance.removeEventListener(eventName, handler);
			break;
		}
	}

	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------

}
