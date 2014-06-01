using UnityEngine;
using System.Collections;

public class GlobalTimer : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		int screenWidth = Screen.width;
		GameObject cam = GameObject.Find("Main Camera");
		Vector3 timerScale = cam.GetComponent<Camera>().ScreenToWorldPoint( new Vector3( screenWidth, 0.0f, 0.0f ) );
		timerScale.x = timerScale.x / 2;
		timerScale.y = transform.localScale.y;
		timerScale.z = transform.localScale.z;
		transform.localScale = timerScale;

		/*float targetSize = Vector3.Distance(cam.ViewportToWorldPoint(new Vector3(0.1f, 0f, 0f)), cam.ViewportToWorldPoint(new Vector3(0.9f, 0f, 0f)));
		float currentSize = timerScale.renderer.bounds.size.x;
		Vector3 newScale = timerScale.transform.localScale;
		newScale.x = targetSize * newScale.y / currentSize;
		planePrefab.transform.localScale = newScale;*/
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
