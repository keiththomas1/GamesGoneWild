using UnityEngine;
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
	public int gamesWon;
	public int beersDrank;
	public int beerLives;

	// Variables kept for progress in mini-games
	public bool[] CupsPlaced;	// For beer pong.
	public int armEnemyLevel;	// For arm wrestle.
	public int dartLevel;
	public int pukeLevel;

	// If in selection mode, this is filled with the current game being played
	public string currentSelectionLevel;

	// Use this for initialization
	void Start () 
	{
		// Essential for making this "global" and persistent.
		Object.DontDestroyOnLoad( this );

		// No mode to start
		previousMode = "";

		gamesWon = 0;
		beersDrank = 0;
		beerLives = 4;

		CupsPlaced = new bool[10];
		for(int i=0; i<10; i++)
		{
			CupsPlaced[i] = true;
		}
		armEnemyLevel = 1;
		dartLevel = 1;
		pukeLevel = 4;

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
		gamesWon++;
		Debug.Log ("Games won: " + gamesWon );

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
		beersDrank = 0;
		Application.LoadLevel( "MenuScene" );
	}

	// This is called when the game is started
	void StartModeMusic()
	{
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
