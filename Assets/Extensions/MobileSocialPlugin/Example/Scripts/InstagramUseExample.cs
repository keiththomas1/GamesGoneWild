////////////////////////////////////////////////////////////////////////////////
//  
// @module Mobile Social Plugin 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;

public class InstagramUseExample : MonoBehaviour {

	public Texture2D imageForPosting;

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	void Awake() {
		SPInstagram.addEventListener(InstagramEvents.POST_SUCCEEDED, OnPost);
		SPInstagram.addEventListener(InstagramEvents.POST_FAILED, OnPostFailed);
	}

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------

	void OnGUI() {
		if(GUI.Button(new Rect(30, 70, 180, 50), "Share Image")) {
			SPInstagram.Share(imageForPosting);
		}
		
		if(GUI.Button(new Rect(250, 70, 180, 50), "Share Image With Message")) {
			SPInstagram.Share(imageForPosting, "I am posting from my app");
		}
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	private void OnPost() {

	}

	private void OnPostFailed(CEvent e) {
		InstaErrorCode error = (InstaErrorCode) e.data;
		Debug.Log("Posting failed with error code " + error.ToString());
	}
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------

}
