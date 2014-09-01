using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MenuController : MonoBehaviour 
{
	public GameObject globalController; 
	public GameObject twitterController;
	public RaycastHit hit;
	public Ray ray;

	//twitter and instagram
	public Texture2D InstaExample;

	// Text objects for changing text on the fly
	public GameObject facebookButton;
	public Texture FBLoginButton;
	public GUIStyle FBbuttonStyle;

	//sounds
	public GameObject ClickSound;

	// Bios
	public GameObject keithBio;
	public GameObject kellieBio;
	public GameObject colinBio;
	public GameObject hardikBio;
	public GameObject juniorBio;

	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find("Global Controller");
		globalController.GetComponent<GlobalController>().StartModeMusic();

		int partyPoints;
		if( globalController )
		{
			List<int> tempList = globalController.GetComponent<GlobalController>().GetHighScores();
			tempList.Reverse();
			partyPoints = tempList[0];
		}
		else
		{
			partyPoints = 0;
		}

		hit = new RaycastHit();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( Input.GetMouseButtonDown( 0 ) )
		{
			keithBio.renderer.enabled = false;
			kellieBio.renderer.enabled = false;
			colinBio.renderer.enabled = false;
			hardikBio.renderer.enabled = false;
			juniorBio.renderer.enabled = false;

			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if( Physics.Raycast(ray,out hit) )
			{
				Debug.Log( hit.collider.name );
				switch( hit.collider.name )
				{
				case "PlayText":
					ClickSound.GetComponent<AudioSource>().Play();
					globalController.GetComponent<GlobalController>().StartMode("Normal Mode", "");
					break;
				case "PracticeText":
					ClickSound.GetComponent<AudioSource>().Play();
					Application.LoadLevel( "SelectionScene" );
					break;
				case "ScoreText":
					ClickSound.GetComponent<AudioSource>().Play();
					Application.LoadLevel( "HighScore" );
					break;
				case "Keith":
					keithBio.renderer.enabled = true;
					break;
				case "Kellie":
					kellieBio.renderer.enabled = true;
					break;
				case "Colin":
					colinBio.renderer.enabled = true;
					break;
				case "Hardik":
					hardikBio.renderer.enabled = true;
					break;
				case "Junior":
					juniorBio.renderer.enabled = true;
					break;
				}
			}
		}
	}
}
