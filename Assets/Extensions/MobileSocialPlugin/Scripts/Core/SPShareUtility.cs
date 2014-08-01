using UnityEngine;
using System.Collections;

public class SPShareUtility  {

	public static void TwitterShare(string status) {
		TwitterShare(status, null);
	}
	
	public static void TwitterShare(string status, Texture2D texture) {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			MSPAndroidSocialGate.StartShareIntent("Share", status, texture, "twi");
			break;
		case RuntimePlatform.IPhonePlayer:
			MSPIOSSocialManager.instance.TwitterPost(status, texture);
			break;
		}
	}


	public static void FacebookShare(string message) {
		FacebookShare(message, null);
	}
	
	public static void FacebookShare(string message, Texture2D texture) {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			MSPAndroidSocialGate.StartShareIntent("Share", message, texture, "face");
			break;
		case RuntimePlatform.IPhonePlayer:
			MSPIOSSocialManager.instance.FacebookPost(message, texture);
			break;
		}
	}


	public static void ShareMedia(string caption, string message) {
		ShareMedia(caption, message, null);
	}
	
	public static void ShareMedia(string caption, string message, Texture2D texture) {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			MSPAndroidSocialGate.StartShareIntent(caption, message, texture);
			break;
		case RuntimePlatform.IPhonePlayer:
			MSPIOSSocialManager.instance.ShareMedia(message, texture);
			break;
		}
	}



}
