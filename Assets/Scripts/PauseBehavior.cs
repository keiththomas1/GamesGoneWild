using UnityEngine;
using System.Collections;

public class PauseBehavior : MonoBehaviour 
{
	public RaycastHit hit;
	public Ray ray;

	bool paused;

	// Use this for initialization
	void Start () 
	{
		hit = new RaycastHit();

		paused = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( Input.GetMouseButtonDown( 0 ) )
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if( Physics.Raycast(ray,out hit) )
			{
				Debug.Log( hit.collider.name );
				switch( hit.collider.name )
				{
				case "PauseButton":
					PauseGame();
					break;
				}
			}
		}
	}

	void PauseGame()
	{
		if( paused )
		{
			paused = false;
		}
		else
		{
			paused = true;

			Time.timeScale = 0;
		}
	}
}
