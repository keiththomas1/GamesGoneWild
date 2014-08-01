////////////////////////////////////////////////////////////////////////////////
//  
// @module Mobile Social Plugin 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;

public class AndroidInstagramManager : SA_Singleton<AndroidInstagramManager> {


	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------

	
	public void Share(Texture2D texture) {
		Share(texture, "");
	}
	
	
	public void Share(Texture2D texture, string message) {
		byte[] val = texture.EncodeToPNG();
		string bytesString = System.Convert.ToBase64String (val);


		AndroidNative.InstagramPostImage(bytesString, message);

	}



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
			error = InstaErrorCode.INTERNAL_EXCEPTION;
			break;
		}
		
		
		dispatch(InstagramEvents.POST_FAILED, error);
	}

}
