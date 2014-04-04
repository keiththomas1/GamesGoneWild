using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour 
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
				case "PlayText":
					globalController.GetComponent<GlobalController>().StartMode("Normal Mode", "");
					break;
				case "SelectionText":
					Application.LoadLevel( "SelectionScene" );
					break;
				}
			}
		}
	}
}
