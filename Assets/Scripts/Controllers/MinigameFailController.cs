using UnityEngine;
using System.Collections;

public class MinigameFailController : MonoBehaviour 
{
	GameObject globalController;

	float timer;

	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find( "Global Controller" );

		timer = 2.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer -= Time.deltaTime;

		if( timer <= 0.0f )
		{
			globalController.GetComponent<GlobalController>().NextMinigame();
		}
	}
}
