using UnityEngine;
using System.Collections;

public class SelectionController : MonoBehaviour 
{
	public GameObject globalController;
	
	public RaycastHit hit;
	public Ray ray;
	
	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find("Global Controller");
		
		hit = new RaycastHit();
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
				case "BackText":
					Application.LoadLevel( "MenuScene" );
					break;
				case "BPText":
					globalController.GetComponent<GlobalController>().currentSelectionLevel = "BeerPong";
					globalController.GetComponent<GlobalController>().StartMode( "Selection" );
					break;
				case "FlippyText":
					globalController.GetComponent<GlobalController>().currentSelectionLevel = "FlippyCup";
					globalController.GetComponent<GlobalController>().StartMode( "Selection" );
					break;
				case "DartsText":
					globalController.GetComponent<GlobalController>().currentSelectionLevel = "Darts";
					globalController.GetComponent<GlobalController>().StartMode( "Selection" );
					break;
				}
			}
		}
	}
}
