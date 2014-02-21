using UnityEngine;
using System.Collections;

public class DrunkKeysController : MonoBehaviour 
{
	public GameObject[] slowGuys;
	public GameObject[] fastGuys;
	public GameObject[] slowGirls;
	public GameObject[] fastGirls;
	public bool[] slowGuysWalking;
	int startSlowGuy;	// Starting index of the "linked list" of slow guys currently active.
	int endSlowGuy;	// Ending index.

	bool timerStarted;
	float timerLength;
	int randomChoice;

	// Use this for initialization
	void Start () 
	{
		timerStarted = true;
		timerLength = 3.0f;

		slowGuysWalking = new bool[slowGuys.Length];
		for( int i=0; i < slowGuys.Length; i++ )
		{
			slowGuysWalking[i] = false;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( timerStarted )
		{
			timerLength -= Time.deltaTime;
		}

		if( timerLength <= 0.0f )
		{
			randomChoice = (int)Mathf.Round( Random.value * 3.0f );
			Debug.Log( randomChoice );

			switch( randomChoice )
			{
			case 0:
				slowGuys[0].GetComponent<DrunkKeyCharacter>().StartWalking();
				break;
			case 1:
				fastGuys[0].GetComponent<DrunkKeyCharacter>().StartWalking();
				break;
				// Pick fast guy
			case 2:
				slowGirls[0].GetComponent<DrunkKeyCharacter>().StartWalking();
				break;
				// Pick slow girl
			case 3:
				fastGirls[0].GetComponent<DrunkKeyCharacter>().StartWalking();
				break;
				// Pick fast girl
			}

			timerLength = 3.0f;
		}
	}
}
