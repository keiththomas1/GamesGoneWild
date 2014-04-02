﻿using UnityEngine;
using System.Collections;

public class GlobalController : MonoBehaviour 
{
	string gameMode;
	public string[] minigameNames;
	string previousMode;

	// All of the music to play
	public GameObject menuMusic;
	public GameObject playGameMusic;

	// Variables kept for overall progress between mini-games
	public int partyPoints;
	public int beersDrank;
	public int beerLives;

	// Variables kept for progress in mini-games
	public bool[] CupsPlaced;	// For beer pong.
	public int armEnemyLevel;	// For arm wrestle.
	public int dartLevel;
	public int pukeLevel;

	// If in selection mode, this is filled with the current game being played
	public string currentSelectionLevel;

	// Variables to keep track of party points
	public GameObject pointsBox;
	public GameObject scoreText;
	int totalPartyPoints;

	// Use this for initialization
	void Start () 
	{
		// Essential for making this "global" and persistent.
		Object.DontDestroyOnLoad( this );

		// No mode to start
		previousMode = "";

		// Set all the children of the global controller to invisible for now.
		Component[] children = GetComponentsInChildren(typeof(Renderer));
		foreach( Component c in children )
		{
			Renderer r = (Renderer)c;
			r.enabled = false;
		}

		// Technically setting for the first time, but hey, modularization..
		ResetVariables();

		StartMenuMusic();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public void StartMode( string mode )
	{
		gameMode = mode;
		StartModeMusic();
		
		NextMinigame();
	}

	public void NextMinigame()
	{
		if( beersDrank < beerLives )	// If we haven't lost yet
		{
			if( gameMode == "Normal Mode" )
			{
				// Choose a random minigame
				// HACK - Consider a more intelligent ordering before finished
				int random = Random.Range( 0, minigameNames.Length);
				if( minigameNames[random] == previousMode )
				{
					NextMinigame();
					return;
				}
				previousMode = minigameNames[random];
				Application.LoadLevel( minigameNames[random] );
			}
			else
			{
				Application.LoadLevel( currentSelectionLevel );
			}
		}
		else 	// If we've lost..
		{
			LostGame();
		}
	}

	// Call this if the player won a minigame and make sure to increment
	// any global variables located in this associated with that minigame.
	public void BeatMinigame()	
	{
		partyPoints+=100;

		Application.LoadLevel( "MinigameWin");
	}

	// Call this if the player lost the minigame
	public void LostMinigame()
	{
		beersDrank++;

		Application.LoadLevel( "MinigameFail");
	}

	// When you drink all of your beers
	void LostGame()
	{
		// HACK: This will eventually route to a "losing" screen where the player is passed out
		// or something. Then a high score type thing and THEN back to the menu screen.

		// Reset all variables.
		ResetVariables();
		Application.LoadLevel( "MenuScene" );
	}

	void ResetVariables()
	{
		partyPoints = 0;	
		beersDrank = 0;	// Lives lost
		beerLives = 4;	// Total lives
		totalPartyPoints = 0;

		// Beer Pong
		CupsPlaced = new bool[10];
		for(int i=0; i<10; i++)
		{
			CupsPlaced[i] = true;
		}
		// Arm Wrestling
		armEnemyLevel = 1;
		// Darts
		dartLevel = 1;
		// Save the Floors
		pukeLevel = 4;
	}

	// This is called when the game is started
	void StartModeMusic()
	{
		Debug.Log("Play");
		if( menuMusic.GetComponent<AudioSource>().isPlaying )
		{
			menuMusic.GetComponent<AudioSource>().Stop();
		}
		if( playGameMusic.GetComponent<AudioSource>().isPlaying )
		{
			playGameMusic.GetComponent<AudioSource>().Stop();
		}
		playGameMusic.GetComponent<AudioSource>().Play();
	}

	// This is called when the menu is started
	void StartMenuMusic()
	{
		if( menuMusic.GetComponent<AudioSource>().isPlaying )
		{
			menuMusic.GetComponent<AudioSource>().Stop();
		}
		if( playGameMusic.GetComponent<AudioSource>().isPlaying )
		{
			playGameMusic.GetComponent<AudioSource>().Stop();
		}
		menuMusic.GetComponent<AudioSource>().Play();
	}
}
