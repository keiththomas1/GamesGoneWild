using UnityEngine;
using System.Collections;

/* With this script, we would like to control the garbage bucket with a click and drag movement
 * when using a mouse and a swipe in our mobile platforms
 */
public class Bucket_Controller : MonoBehaviour {

	// Identifier to locate our bucket gameobject
	//GameObject Bucket;

	// Bucket speed
	//float speed = 0.1F;

	// Used for keyboard / desktop mode
	Vector3 DesktopScreenPoint;

	// Used for mobile mode
	Vector3 MobileScreenPoint;
	
	public RaycastHit hit;
	public Ray ray;
	
	bool startDragging;

	// Use this for initialization
	void Start ()
	{
		// Controlling our bucket gameobject
		//Bucket = GameObject.FindGameObjectWithTag ("Bucket");

		hit = new RaycastHit();

		startDragging = false;
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
				if( hit.collider.name == this.name )
				{
					startDragging = true;
				}
			}
		}
		if( Input.GetMouseButtonUp( 0 ) )
		{
			startDragging = false;
		}

		if( startDragging )
		{
			// Determines screen point position in desktop mode
			Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, MobileScreenPoint.y, MobileScreenPoint.z);

			Vector3 currentPosition = Camera.main.ScreenToWorldPoint (currentScreenPoint);
			currentPosition.y = -4.0f;
			currentPosition.z = -1.0f;

			// Move the bucket to the mouse position.
			transform.position = currentPosition;
		}
	}

	/// <summary>
	/// Touchs the bucket navigation.
	/// </summary>
	void touchBucketNavigation(){

		// If player is touching screen, lets move the object
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) 
		{
			Vector3 currentScreenPoint = new Vector3 (Input.GetTouch (0).position.x, MobileScreenPoint.y, MobileScreenPoint.z);
			Vector3 currentPosition = Camera.main.ScreenToWorldPoint (currentScreenPoint);
			transform.position = currentPosition;
		}
	}
}