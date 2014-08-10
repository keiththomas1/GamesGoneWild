using UnityEngine;
using System.Collections;

public class MSPAndroidSocialGate  {

	public static void StartShareIntent(string caption, string message, string packageNamePattern = "") {
		StartShareIntentWithSubject(caption, message, "", packageNamePattern);
	}
	
	public static void StartShareIntentWithSubject(string caption, string message, string subject, string packageNamePattern = "") {
		AndroidNative.StartShareIntent(caption, message, subject, packageNamePattern);
	}
	
	
	public static void StartShareIntent(string caption, string message, Texture2D texture,  string packageNamePattern = "") {
		StartShareIntentWithSubject(caption, message, "", texture, packageNamePattern);
	}
	
	public static void StartShareIntentWithSubject(string caption, string message, string subject, Texture2D texture,  string packageNamePattern = "") {



		if(texture != null) {
			byte[] val = texture.EncodeToPNG();
			string bytesString = System.Convert.ToBase64String (val);
			
			AndroidNative.StartShareIntent(caption, message, subject, bytesString, packageNamePattern);
		} else {
			AndroidNative.StartShareIntent(caption, message, subject, packageNamePattern);
		}

	}



}

