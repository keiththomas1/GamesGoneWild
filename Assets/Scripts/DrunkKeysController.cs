using UnityEngine;
using System.Collections;

public class DrunkKeysController : MonoBehaviour 
{
	public GameObject[] slowGuys;
	public GameObject[] fastGuys;
	public GameObject[] slowGirls;
	public GameObject[] fastGirls;
	int currentSlowGuy;
	int currentFastGuy;
	int currentSlowGirl;
	int currentFastGirl;
	int startSlowGuy;	// Starting index of the "linked list" of slow guys currently active.
	int endSlowGuy;	// Ending index.

	bool timerStarted;
	float timerLength;
	int randomChoice;
	Vector3 tempPosition;
	float randomXPosition;

	// Use this for initialization
	void Start () 
	{
		currentSlowGuy = 0;
		currentFastGuy = 0;
		currentSlowGirl = 0;
		currentFastGirl = 0;

		timerStarted = true;
		timerLength = 2.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		CaptureInput();

		if( timerStarted )
		{
			timerLength -= Time.deltaTime;
		}

		if( timerLength <= 0.0f )
		{
			randomChoice = (int)Mathf.Round( Random.value * 3.0f );
			randomXPosition = Mathf.Round( Random.value * 12.0f ) - 6.0f;


			switch( randomChoice )
			{
			case 0:
				tempPosition = slowGuys[currentSlowGuy].transform.position;
				tempPosition.x = randomXPosition;
				slowGuys[currentSlowGuy].transform.position = tempPosition;
				slowGuys[currentSlowGuy].GetComponent<DrunkKeyCharacter>().StartWalking();
				currentSlowGuy++;
				if( currentSlowGuy >= slowGuys.Length )
					currentSlowGuy = 0;
				break;
			case 1:
				tempPosition = fastGuys[currentFastGuy].transform.position;
				tempPosition.x = randomXPosition;
				fastGuys[currentFastGuy].transform.position = tempPosition;
				fastGuys[currentFastGuy].GetComponent<DrunkKeyCharacter>().StartWalking();
				currentFastGuy++;
				if( currentFastGuy >= fastGuys.Length )
					currentFastGuy = 0;
				break;
				// Pick fast guy
			case 2:
				tempPosition = slowGirls[currentSlowGirl].transform.position;
				tempPosition.x = randomXPosition;
				slowGirls[currentSlowGirl].transform.position = tempPosition;
				slowGirls[currentSlowGirl].GetComponent<DrunkKeyCharacter>().StartWalking();
				currentSlowGirl++;
				if( currentSlowGirl >= slowGirls.Length )
					currentSlowGirl = 0;
				break;
				// Pick slow girl
			case 3:
				tempPosition = fastGirls[currentFastGirl].transform.position;
				tempPosition.x = randomXPosition;
				fastGirls[currentFastGirl].transform.position = tempPosition;
				fastGirls[currentFastGirl].GetComponent<DrunkKeyCharacter>().StartWalking();
				currentFastGirl++;
				if( currentFastGirl >= fastGirls.Length )
					currentFastGirl = 0;
				break;
				// Pick fast girl
			}

			timerLength = 2.0f;
		}
	}
	
	void CaptureInput()
	{
		//Debug.Log( Input
	}
}
