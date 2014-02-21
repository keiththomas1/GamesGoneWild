using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour 
{
	public RaycastHit hit;
	public Ray ray;

	// Use this for initialization
	void Start () 
	{
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
				switch( hit.collider.name )
				{
					case "PlayText":
						Debug.Log( "Clicked on play!" );
						break;
				}
			}
		}
	}
}
