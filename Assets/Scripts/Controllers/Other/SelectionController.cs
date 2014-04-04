using UnityEngine;
using System.Collections;

public class SelectionController : MonoBehaviour 
{
	public GameObject globalController;
	
	public RaycastHit hit;
	public Ray ray;

	// For arrow transitions
	public GameObject rightArrow;
	public GameObject leftArrow;
	bool transitioning;
	string direction;
	Vector2 transitionVector;
	int currentGroup;
	public GameObject[] groups;

	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find("Global Controller");
		
		hit = new RaycastHit();

		transitioning = false;
		direction = "";
		transitionVector = new Vector2( .3f, 0.0f );
		currentGroup = 0;
		
		leftArrow.renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( Input.GetMouseButtonDown( 0 ) )
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if( Physics.Raycast(ray,out hit) && !transitioning)
			{
				Debug.Log( hit.collider.name );
				switch( hit.collider.name )
				{
				case "LeftArrow":
					GoLeft();
					break;
				case "RightArrow":
					GoRight();
					break;
				case "BackText":
					Application.LoadLevel( "MenuScene" );
					break;
				case "BPText":
				case "BPPic":
					globalController.GetComponent<GlobalController>().StartMode( "Selection", "BeerPong" );
					break;
				case "FlippyText":
				case "FlippyPic":
					globalController.GetComponent<GlobalController>().StartMode( "Selection", "FlippyCup" );
					break;
				case "DartsText":
				case "DartsPic":
					globalController.GetComponent<GlobalController>().StartMode( "Selection", "Darts" );
					break;
				case "ArmWrestlingText":
				case "ArmWrestlingPic":
					globalController.GetComponent<GlobalController>().StartMode( "Selection", "ArmWrestle" );
					break;
				case "ThrowUpText":
				case "ThrowUpPic":
					globalController.GetComponent<GlobalController>().StartMode( "Selection", "Save_The_Floor" );
					break;
				case "TiltText":
				case "TiltPic":
					globalController.GetComponent<GlobalController>().StartMode( "Selection", "fall" );
					break;
				}
			}
		}

		if( transitioning )
		{
			if( direction == "Left" )
			{
				// HACK - dynamic for loop isn't working for this for some reason, hard-coded
				groups[0].transform.Translate( transitionVector );
				groups[1].transform.Translate( transitionVector );
				
				if( groups[currentGroup-1].transform.position.x >= -1.7 )
				{
					transitioning = false;
					currentGroup--;
					
					rightArrow.renderer.enabled = true;
					rightArrow.layer = 0;
					if( 0 == currentGroup )
					{
						leftArrow.renderer.enabled = false;
						leftArrow.layer = 2;
					}
					else
					{
						leftArrow.renderer.enabled = true;
						leftArrow.layer = 0;
					}
				}
			}
			else // direction == "Right" 
			{
				// HACK - dynamic for loop isn't working for this for some reason, hard-coded
				groups[0].transform.Translate( -transitionVector );
				groups[1].transform.Translate( -transitionVector );

				if( groups[currentGroup+1].transform.position.x <= -1.7 )
				{
					transitioning = false;
					currentGroup++;
					
					leftArrow.renderer.enabled = true;
					leftArrow.layer = 0;
					if( groups.Length == (currentGroup+1) )
					{
						rightArrow.renderer.enabled = false;
						rightArrow.layer = 2;
					}
					else
					{
						rightArrow.renderer.enabled = true;
						rightArrow.layer = 0;
					}
				}
			}
		}
	}

	void GoLeft()
	{
		direction = "Left";
		transitioning = true;
		rightArrow.renderer.enabled = false;
		leftArrow.renderer.enabled = false;
	}
	
	void GoRight()
	{
		direction = "Right";
		transitioning = true;
		rightArrow.renderer.enabled = false;
		leftArrow.renderer.enabled = false;
	}
}
