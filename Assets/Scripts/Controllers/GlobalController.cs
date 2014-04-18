using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalController : MonoBehaviour 
{
	string gameMode;
	List<string> allMinigames;
	List<string> currentMinigames;
	public string previousMode;

	// Facebook User Information -- name, picture, etc.
	public GameObject menuController;
	public string FBUsername;

	// All of the music to play
	public GameObject menuMusic;
	public GameObject playGameMusic;

	// Variables kept for overall progress between mini-games
	public int partyPoints;
	public int beersDrank;
	public int beerLives;
	public int turnUpLevel;

	// Variables kept for progress in mini-games
	public bool[] CupsPlaced;	// For beer pong.
	public int armEnemyLevel;	// For arm wrestle.
	public int dartLevel;
	public int pukeLevel;

	// If in selection mode, this is filled with the current game being played
	public string currentSelectionLevel;
	// Which minigame are we playing? In numerics
	public int currentLevel;

	public GameObject pointsBox;
	public GameObject scoreText;

	// Use this for initialization
	void Start () 
	{
		menuController = GameObject.Find( "Menu Controller" );

		allMinigames = new List<string>();
		allMinigames.Add("BeerPong");
		allMinigames.Add("FlippyCup");
		allMinigames.Add("Darts");
		allMinigames.Add("Save_The_Floor");
		allMinigames.Add("fall");
		allMinigames.Add("ArmWrestle");

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

	public void StartMode( string mode, string game )
	{
		gameMode = mode;
		StartModeMusic();

		/*if( mode == "Selection" )
		{
			for( int i=0; i<allMinigames.Count; i++ )
			{
				if( allMinigames[i] == game )
				{
					currentLevel = i;
					break;
				}
			}
		}*/
		currentSelectionLevel = game;
		
		NextMinigame();
	}

	public void NextMinigame()
	{
		if( beersDrank < beerLives )	// If we haven't lost yet
		{
			if( gameMode == "Normal Mode" )
			{
				if( currentMinigames.Count > 0 )
				{
					int random = Random.Range( 0, currentMinigames.Count);
					previousMode = currentMinigames[random];
					currentMinigames.Remove( previousMode );
					currentLevel = random;
					Application.LoadLevel( previousMode );
				}
				else // it's time to turn up
				{
					turnUpLevel++;

					for( int i=0; i<(2+turnUpLevel); i++ )
					{
						currentMinigames.Add( allMinigames[i] );
					}

					Application.LoadLevel( "TurnUpScene" );
				}
			}
			else
			{
				Application.LoadLevel( currentSelectionLevel );
			}
		}
		else 	// If we've lost..
		{
			if( gameMode == "Normal Mode" )
			{
				Application.LoadLevel( "HighScore" );
			}
			else
			{
				LostGame();
			}
		}
	}

	// Call this if the player won a minigame and make sure to increment
	// any global variables located in this associated with that minigame.
	public void BeatMinigame( int score )	
	{
		partyPoints += score;

		Application.LoadLevelAdditive( "MinigameWin");
	}

	// Call this if the player lost the minigame
	public void LostMinigame()
	{
		beersDrank++;

		Application.LoadLevel( "MinigameFail");
	}

	// When you drink all of your beers
	public void LostGame()
	{
		// HACK: This will eventually route to a "losing" screen where the player is passed out
		// or something. Then a high score type thing and THEN back to the menu screen.

		// Reset all variables.
		ResetVariables();
		Application.LoadLevel( "MenuScene" );
	}

	void ResetVariables()
	{
		currentMinigames = new List<string>();
		currentMinigames.Add("BeerPong");
		currentMinigames.Add("FlippyCup");
		currentMinigames.Add("Darts");

		partyPoints = 0;	
		beersDrank = 0;	// Lives lost
		beerLives = 4;	// Total lives
		turnUpLevel = 1;

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
		// Disabled till we get real music.
		//playGameMusic.GetComponent<AudioSource>().Play();
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
		// Disabled till we get real music.
		//menuMusic.GetComponent<AudioSource>().Play();
	}


	public void SetUserName()
	{
		FBUsername = menuController.GetComponent<MenuController>().FBName;
	}


	// Adds new score and returns the list.
	// HACK - Only shows four high scores right now, will need to change in the future.
	public List<int> SaveHighScore(int score)
	{
		List<int> tempList = GetHighScores();

		if( score > tempList[0] )
		{
			if( tempList.Count >= 5 )
			{
				tempList[0] = score;
				Debug.Log( tempList[0] );
			}
			else
			{
				tempList.Add( score );
			}
			tempList.Sort();

			// Save the high scores to the preferences
			for( int i=0; i < tempList.Count; i++ )
			{
				PlayerPrefs.SetInt("HighScore" + i.ToString(), tempList[i]);
				
				if( i >= 5 )
				{
					break;
				}
			}
			PlayerPrefs.Save();
		}

		return tempList;
	}

	// Returns list of highscores in ascending order.
	public List<int> GetHighScores()
	{
		int tempScore; 
		List<int> newHighscores = new List<int>();
		// Get the high scores from the preferences
		for( int i=0; i < 5; i++ )
		{
			tempScore = PlayerPrefs.GetInt("HighScore" + i.ToString());
			Debug.Log("High score " + i.ToString() + ": " + tempScore.ToString());
			if( tempScore != 0 )
			{
				newHighscores.Add( tempScore );
			}
			else
			{
				break;
			}
		}

		return newHighscores;
	}
}
