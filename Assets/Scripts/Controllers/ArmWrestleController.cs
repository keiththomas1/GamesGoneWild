using UnityEngine;
using System.Collections;

public class ArmWrestleController : MonoBehaviour 
{
	public GameObject globalController;

	bool startedGame;
	float startTimer;	// HACK - This shouldn't be needed. It is timed based on an animation, ideally
							// we would base it on when the animation ends.

	public GameObject arms;
	float initialArmX;
	public float loseX;
	public float winX;
	float winGuage;

	public GameObject countdownTimer;
	Vector3 tempPos;

	float playerStrength;
	float enemyStrength;
	int enemyMultiplier;

	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find( "Global Controller" );

		startedGame = false;
		startTimer = 3.4f;

		initialArmX = arms.transform.position.x;
		winGuage = initialArmX;

		/*foreach (AnimationState state in countdownTimer.GetComponent<Animator>().animation) 
		{
			state.speed = 0.2F;
		}*/
		//countdownTimer.animation.Stop();
		
		playerStrength = .1f;
		enemyStrength = .001f;
		if( globalController )
		{
			enemyMultiplier = globalController.GetComponent<GlobalController>().armEnemyLevel;
		}
		else
		{
			Debug.Log("Missing a global controller.");
			enemyMultiplier = 1;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( startedGame )
		{
			if( Input.GetMouseButtonDown( 0 ) )
			{
				winGuage -= playerStrength;
			}
			
			winGuage += (enemyStrength * enemyMultiplier);
			
			tempPos = arms.transform.position;
			tempPos.x = winGuage;
			arms.transform.position = tempPos;
		}
		else
		{
			startTimer -= Time.deltaTime;

			if( startTimer <= 0.0f )
			{
				startedGame = true;
				Destroy( countdownTimer );
			}
		}

		// Check if win or lose.
		if( tempPos.x >= loseX )
		{
			globalController.GetComponent<GlobalController>().LostMinigame();
		}
		if( tempPos.x <= winX )
		{
			globalController.GetComponent<GlobalController>().armEnemyLevel++;
			globalController.GetComponent<GlobalController>().BeatMinigame();
		}
	}
}
