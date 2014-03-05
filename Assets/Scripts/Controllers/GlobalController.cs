using UnityEngine;
using System.Collections;

public class GlobalController : MonoBehaviour 
{
	string gameMode;
	public string[] minigameNames;
	string previousMode;

	public GameObject menuMusic;
	public GameObject playGameMusic;

	// Variables kept for progress in mini-games
	int gamesWon;
	public bool[] CupsPlaced;	// For beer pong.
	public int armEnemyLevel;	// For arm wrestle.

	// Use this for initialization
	void Start () 
	{
		// Essential for making this "global" and persistent.
		Object.DontDestroyOnLoad( this );

		previousMode = "";

		gamesWon = 0;

		CupsPlaced = new bool[10];
		for(int i=0; i<10; i++)
		{
			CupsPlaced[i] = true;
		}

		armEnemyLevel = 1;

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
		}
	}

	public void NextMinigame()
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

	public void BeatMinigame()
	{
		gamesWon++;
		Debug.Log ("Games won: " + gamesWon );

		Application.LoadLevel( "MinigameWin");
	}

	public void LostMinigame()
	{
		
		Application.LoadLevel( "MinigameFail");
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
