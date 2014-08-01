////////////////////////////////////////////////////////////////////////////////
//  
// @module Common Android Native Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


 

using UnityEngine;
using System.Collections;

public class FacebookEvents {
	
	public const string FACEBOOK_INITED 				= "facebook_inited";
	

	public const string AUTHENTICATION_SUCCEEDED 		= "authentication_succeeded";
	public const string AUTHENTICATION_FAILED   		= "authentication_failed";


	public const string POST_SUCCEEDED 					= "post_succeeded";
	public const string POST_FAILED   					= "post_failed";


	public const string APP_REQUEST_COMPLETE   			= "app_request_complete";


	public const string GAME_FOCUS_CHANGED 				= "game_focus_changed";

	public const string USER_DATA_LOADED 				= "user_data_loaded";
	public const string USER_DATA_FAILED_TO_LOAD   		= "user_data_failed_to_load";


	public const string FRIENDS_DATA_LOADED 			= "friends_data_loaded";
	public const string FRIENDS_FAILED_TO_LOAD   		= "friends_failed_to_load";

}
