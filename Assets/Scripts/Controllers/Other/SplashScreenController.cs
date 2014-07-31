using UnityEngine;
using System.Collections;

public class SplashScreenController : MonoBehaviour 
{
	public GameObject logo;
	public GameObject logoText;
	public GameObject redGear;
	public GameObject blueGear;

	float timer;
	Color colorStart;
	Color colorEnd;

	bool fadeIn;
	float fadeValue;

	bool canClickThrough;
	float clickThroughTimer;

	Vector3 redRotate;
	Vector3 blueRotate;

	// Use this for initialization
	void Start () 
	{
		timer = 3.0f; // set duration time in seconds in the Inspector

		colorStart = logo.renderer.material.color;
		colorEnd = new Color( colorStart.r, colorStart.g, colorStart.b, 0.0f );

		fadeIn = true;
		fadeValue = 1.0f;
		
		canClickThrough = false;
		clickThroughTimer = .5f;

		redRotate = new Vector3( 0.0f, 0.0f, -.5f );
		blueRotate = new Vector3( 0.0f, 0.0f, .5f );
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
			
			//logo.renderer.material.color = Color.Lerp( colorStart, colorEnd, fadeValue/1.0f );
		}
		else
		{
			fadeValue += Time.deltaTime;
			logo.renderer.material.color = Color.Lerp( colorStart, colorEnd, fadeValue/1.0f );
			logoText.renderer.material.color = Color.Lerp( colorStart, colorEnd, fadeValue/1.0f );
			redGear.renderer.material.color = Color.Lerp( colorStart, colorEnd, fadeValue/1.0f );
			blueGear.renderer.material.color = Color.Lerp( colorStart, colorEnd, fadeValue/1.0f );
		}

		// For clicking through the screen
		clickThroughTimer -= Time.deltaTime;
		
		if( clickThroughTimer <= 0.0f )
			canClickThrough = true;
		
		if( canClickThrough && Input.GetMouseButtonDown( 0 ) )
		{
			Application.LoadLevel("MenuScene");
		}

		redGear.transform.Rotate( redRotate );
		blueGear.transform.Rotate( blueRotate );
	}

}
