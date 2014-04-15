using UnityEngine;
using System.Collections;

/* With this script, we would like to control the garbage bucket with a click and drag movement
 * when using a mouse and a swipe in our mobile platforms
 */
public class Bucket_Controller : MonoBehaviour {

	// Identifier to locate our bucket gameobject
	GameObject Bucket;

	// Bucket speed
	//float speed = 0.1F;

	// Used for keyboard / desktop mode
	Vector3 DesktopScreenPoint;

	// Used for mobile mode
	Vector3 MobileScreenPoint;

	// Use this for initialization
	void Start (){

		// Controlling our bucket gameobject
		Bucket = GameObject.FindGameObjectWithTag ("Bucket");

	}
	
	// Update is called once per frame
	void Update (){

		// Movement in desktop mode
		OnMobileOver ();

		// Movement in mobile platform mode
		touchBucketNavigation ();
	}

	/// <summary>
	/// Raises the mouse over event.
	/// </summary>
	void OnMobileOver(){
			
		//
		MobileScreenPoint = Camera.main.WorldToScreenPoint(Bucket.transform.position);
	}

	/// <summary>
	/// Raises the mouse drag event.
	/// </summary>
	void OnMouseDrag(){
		Debug.Log( Camera.main.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, 0f, 0f ) ));
		// Determines screen point position in desktop mode
		Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, MobileScreenPoint.y, MobileScreenPoint.z);

		// 
		Vector3 currentPosition = Camera.main.ScreenToWorldPoint (currentScreenPoint);

		//
		transform.position = currentPosition;

		//transform.camera.WorldToScreenPoint;
	}

	/// <summary>
	/// Touchs the bucket navigation.
	/// </summary>
	void touchBucketNavigation(){

		// If player is touching screen, lets move the object
		/*if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
			Vector2 touchDeltaPosition = Input.GetTouch (0).deltaPosition;
			transform.Translate ( touchDeltaPosition.x * speed, 0, 0);
		}*/

		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			Vector3 currentScreenPoint = new Vector3 (Input.GetTouch (0).position.x, MobileScreenPoint.y, MobileScreenPoint.z);
			Vector3 currentPosition = Camera.main.ScreenToWorldPoint (currentScreenPoint);
			transform.position = currentPosition;
		}
	}
}