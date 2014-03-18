using UnityEngine;
using System.Collections;

public class GlobalController : MonoBehaviour 
{
	string gameMode;
	public string[] minigameNames;
	string previousMode;

	public GameObject menuMusic;
	public GameObject playGameMusic;

	// Variables kept for overall progress
	public int gamesWon;
	public int beersDrank;
	public int beerLives;

	// Variables kept for progress in mini-games
	public bool[] CupsPlaced;	// For beer pong.
	public int armEnemyLevel;	// For arm wrestle.
	public int dartLevel;
	public int pukeLevel;

	public string currentSelectionLevel;

	// Use this for initialization
	void Start () 
	{
		// Essential for making this "global" and persistent.
		Object.DontDestroyOnLoad( this );

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

		switch( gameMode )
		{
		case "Normal Mode":
			NextMinigame();
			break;
		case "Selection":
			break;
		}
	}

	public void NextMinigame()
	{
		if( gameMode == "Normal Mode" )
		if( beersDrank < beerLives )
		{
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
			LostGame();
		}
	}

	public void BeatMinigame()
	{
		gamesWon++;
		Debug.Log ("Games won: " + gamesWon );

		Application.LoadLevel( "MinigameWin");
	}

	public void LostMinigame()
	{
		beersDrank++;

		Application.LoadLevel( "MinigameFail");
	}

	void LostGame()
	{
		// HACK: This will eventually route to a "losing" screen where the player is passed out
		// or something. Then a high score type thing and THEN back to the menu screen.
		Application.LoadLevel( "MenuScene" );
	}

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
