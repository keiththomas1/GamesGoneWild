using UnityEngine;
using System.Collections;

public class GlobalTimer : MonoBehaviour 
{
	float timer;
	bool timerStarted;
	int whichMinigame;

	// Use this for initialization
	void Start () 
	{
		timerStarted = false;
		whichMinigame = 0;

		// Setting the size of the timer.
		GameObject cam = GameObject.Find("Main Camera");
		float targetSize = Vector3.Distance(
							cam.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.0f, 0f, 0f)), 
							cam.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1.1f, 0f, 0f)));
		float currentSize = this.renderer.bounds.size.x;
		Vector3 newScale = this.transform.localScale;
		newScale.x = targetSize * newScale.x / currentSize;
		this.transform.localScale = newScale;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( timerStarted )
		{
			timer -= Time.deltaTime;

			if( timer < 0.0f )
			{
				switch( whichMinigame )
				{
				case 1:
					break;
				}
			}
		}
	}

	// Minigame - 1=BeerPong, 2=FlippyCup, 3=Darts, 4=Puke, 5=Fall, 6=Arm
	void StartTimer( float seconds, int minigame )
	{
		timer = seconds;
		timerStarted = true;
		whichMinigame = minigame;
	}
}
