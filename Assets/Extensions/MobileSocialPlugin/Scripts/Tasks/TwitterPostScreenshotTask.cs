using UnityEngine;
using System.Collections;

public class TwitterPostScreenshotTask : EventDispatcher {

	private string text;

	public static TwitterPostScreenshotTask  Create() {
		return new GameObject("TwitterPostScreenshotTask").AddComponent<TwitterPostScreenshotTask>();
	}


	public void Post(string _text) {
		text = _text;
		StartCoroutine(PostScreenshot());
	}


	private IEnumerator PostScreenshot() {

		yield return new WaitForEndOfFrame();
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D( width, height, TextureFormat.RGB24, false );
		// Read screen contents into the texture
		tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		tex.Apply();
		
		SPShareUtility.TwitterShare(text, tex);

		Destroy(tex);
		dispatch(BaseEvent.COMPLETE);
		
	}
	

}
