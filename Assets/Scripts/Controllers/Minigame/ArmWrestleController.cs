using UnityEngine;
using System.Collections;

public class ArmWrestleController : MonoBehaviour 
{
	public GameObject globalController;

	bool gameOver;
	bool startedGame;
	float startTimer;	

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
	float strengthRotation;
	
	// Variables for fading out the instructions
	public GameObject instructionText;
	float fadeTimer;
	Color colorStart;
	Color colorEnd;
	float fadeValue;

	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find( "Global Controller" );

		gameOver = false;
		startedGame = false;
		countdownTimer.GetComponent<Animator>().speed = 1.4f;
		startTimer = 2.7f;

		initialArmX = arms.transform.position.x;
		winGuage = initialArmX;

		/*foreach (AnimationState state in countdownTimer.GetComponent<Animator>().animation) 
		{
			state.speed = 0.2F;
		}*/
		//countdownTimer.animation.Stop();
		
		playerStrength = .6f;
		enemyStrength = .015f;
		if( globalController )
		{
			enemyMultiplier = globalController.GetComponent<GlobalController>().armEnemyLevel;
		}
		else
		{
			Debug.Log("Missing a global controller.");
			enemyMultiplier = 1;
		}

		// Fading instructions variables
		fadeTimer = 3.0f; // set duration time in seconds in the Inspector
		colorStart = instructionText.renderer.material.color;
		colorEnd = new Color( colorStart.r, colorStart.g, colorStart.b, 0.0f );
		fadeValue = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( !gameOver )
		{
			if( startedGame )
			{
				if( Input.GetMouseButtonDown( 0 ) )
				{
					winGuage -= playerStrength;
					strengthRotation = 10.0f;
				}
				else
				{
					strengthRotation = -.2f * enemyMultiplier;
				}
				
				winGuage += (enemyStrength * enemyMultiplier);
				
				// Not needed at the moment.
				//tempPos = arms.transform.position;
				//tempPos.x = winGuage;
				//arms.transform.position = tempPos;
				
				arms.transform.Rotate( new Vector3( 0.0f, strengthRotation, 0.0f ) );
				
				
				if( fadeValue < 1.0f )
				{
					fadeTimer -= Time.deltaTime;
					fadeValue += Time.deltaTime;
					instructionText.renderer.material.color = Color.Lerp( colorStart, colorEnd, fadeValue/1.0f );
					
					if( fadeValue >= 1.0f )
					{
						Destroy( instructionText );
					}
				}
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
			if( arms.transform.rotation.eulerAngles.y > 70.0f && arms.transform.rotation.eulerAngles.y < 180.0f )
			{
				gameOver = true;
				globalController.GetComponent<GlobalController>().armEnemyLevel++;
				globalController.GetComponent<GlobalController>().BeatMinigame( 100 );
			}
			if( arms.transform.rotation.eulerAngles.y < 290.0f && arms.transform.rotation.eulerAngles.y > 180.0f )
			{
				globalController.GetComponent<GlobalController>().LostMinigame();
			}
		}
	}
}
