using UnityEngine;
using System.Collections;

public class GlobalController : MonoBehaviour 
{
	string gameMode;
	public string[] minigameNames;

	public GameObject menuMusic;
	public GameObject playGameMusic;

	public bool[] CupsPlaced;

	// Use this for initialization
	void Start () 
	{
		// Essential for making this "global" and persistent.
		Object.DontDestroyOnLoad( this );
		
		CupsPlaced = new bool[10];
		for(int i=0; i<10; i++)
		{
			CupsPlaced[i] = true;
		}

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

	void NextMinigame()
	{
		int random = Random.Range( 0, minigameNames.Length-1 );

		Application.LoadLevel( minigameNames[random] );
	}

	public void BeatMinigame()
	{
		// Score++
		// Show celebration transition screen

		NextMinigame();
	}

	public void LostMinigame()
	{
		// Show beer drinking "loser" transition screen
		// Drunk++

		NextMinigame();
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
