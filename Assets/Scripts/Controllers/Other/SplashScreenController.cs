using UnityEngine;
using System.Collections;

public class SplashScreenController : MonoBehaviour 
{
	public GameObject logo;

	float timer;
	Color colorStart;
	Color colorEnd;

	bool fadeIn;
	float fadeValue;

	// Use this for initialization
	void Start () 
	{
		logo = GameObject.Find( "Logo" );

		timer = 3.0f; // set duration time in seconds in the Inspector

		colorStart = logo.renderer.material.color;
		colorEnd = new Color( colorStart.r, colorStart.g, colorStart.b, 0.0f );

		fadeIn = true;
		fadeValue = 1.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer -= Time.deltaTime;
		if (timer <= 0)
		{
			Application.LoadLevel("MenuScene");
		}

		if( fadeIn )
		{
			fadeValue -= (Time.deltaTime/2);

			if( fadeValue <= 0.0f )
				fadeIn = false;
		}
		else
		{
			fadeValue += Time.deltaTime;
		}
		logo.renderer.material.color = Color.Lerp( colorStart, colorEnd, fadeValue/1.0f );
	}

}
