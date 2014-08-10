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

public class IOSInstagramManager : SA_Singleton<IOSInstagramManager> {


	#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE

	[DllImport ("__Internal")]
	private static extern void _instaShare(string encodedMedia, string message);

	#endif

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------

	public void Share(Texture2D texture) {
		Share(texture, "");
	}


	public void Share(Texture2D texture, string message) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE

		byte[] val = texture.EncodeToPNG();
		string bytesString = System.Convert.ToBase64String (val);
		

		_instaShare(bytesString, message);

		#endif

	}




	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------


	private void OnPostSuccess() {
		dispatch(InstagramEvents.POST_SUCCEEDED);
	}
	
	
	private void OnPostFailed(string data) {

		int code = System.Convert.ToInt32(data);
		InstaErrorCode error = InstaErrorCode.NO_APPLICATION_INSTALLED;

		switch(code) {
		case 1:
			error = InstaErrorCode.NO_APPLICATION_INSTALLED;
			break;
		case 2:
			error = InstaErrorCode.USER_CANCELLED;
			break;
		case 3:
			error = InstaErrorCode.SYSTEM_VERSION_ERROR;
			break;
		}


		dispatch(InstagramEvents.POST_FAILED, error);
	}

	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------

}
