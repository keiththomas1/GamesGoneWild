using UnityEngine;
using System.Collections;

public class DartsController : MonoBehaviour 
{
	GameObject globalController;
	public GameObject dart;
	public bool dartFlying;
	public bool dartMoving;
	
	public GameObject[] pillars;
	float pillarTimer;
	int pillarCount;
	int currentPillar;

	public GameObject board;

	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find("Global Controller");

		dartFlying = true;
		dartMoving = false;

		pillarTimer = 2.0f;
		pillarCount = 4; // HACK - put this variable in the global controller
		currentPillar = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( dartFlying )
		{
			PillarUpdate();
		}
	}
	
	void PillarUpdate()
	{
		pillarTimer -= Time.deltaTime;

		// Start moving the next pillar when this stops
		if( pillarTimer <= 0.0f )
		{	
			if( currentPillar < (pillarCount-1) )	// Still moving pillars
			{
				pillars[currentPillar].GetComponent<PillarBehavior>().StartMoving();
			}
			else if( currentPillar == (pillarCount-1) )	// Move the dartboard
			{
				board.GetComponent<DartBoardBehavior>().StartMoving();
				dartFlying = false;
			}

			if( currentPillar == (pillarCount-2) )
			{
				pillarTimer = 4.0f;
			}
			else
			{
				pillarTimer = 2.0f;
			}

			currentPillar++;
		}
	}

	public void StartMovingDart()
	{
		dartMoving = true;
		dart.GetComponent<DartBehavior>().dartMoving = true;
	}
}
