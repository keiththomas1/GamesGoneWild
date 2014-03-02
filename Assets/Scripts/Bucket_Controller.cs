using UnityEngine;
using System.Collections;

/* With this script, we would like to control the garbage bucket with a click and drag movement
 * when using a mouse and a swipe in our mobile platforms
 */
public class Bucket_Controller : MonoBehaviour {
	
	// Garbage bucket that will catch the puke stream
	GameObject Bucket;

	// Bucket speed
	public float speed = 0.1f;

	// Used for keyboard / desktop mode
	Vector3 screenPoint;
	
	// Use this for initialization
	void Start (){
		
		if (Bucket == null) {
			Bucket = GameObject.FindGameObjectWithTag ("GameController");
			Debug.Log("GameController found\n");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		//OnGUI ();
		OnMouseOver ();
		touchPosition ();
	}
	
	void OnMouseOver()
	{
		//Ray ray = new Ray ();
		//RaycastHit hit = new RaycastHit ();
		
		// This was for testing purposes
		//if (Input.GetMouseButtonDown (0)) {
				
			//ray = Camera.main.ScreenPointToRay( (Input.mousePosition));
			
			//if( Physics.Raycast( ray, out hit)){
				// Determines position in mobile platform mode
				//Debug.Log (hit.collider.name);
				//Debug.Log ("Physics.Raycast");
				
				//Vector2 touchDeltaPosition = Input.GetTouch (0).deltaPosition;
				//transform.Translate (-touchDeltaPosition.x * speed, 0, 0);
			//}
			
			Debug.Log ("Mouse-Clicked\n");
			//renderer.material.color -= new Color(0.1F, 0, 0) * Time.deltaTime;
			screenPoint = Camera.main.WorldToScreenPoint(Bucket.transform.position);
			//OnMouseDrag ();
		//}
	}
	
	void OnMouseDrag()
	{
		
		// Determines screen point position in desktop mode
		// We only want to move horizaontally as well
		Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, screenPoint.y, screenPoint.z);
		Vector3 currentPosition = Camera.main.ScreenToWorldPoint (currentScreenPoint);
		transform.position = currentPosition;
	}
	
	/*void OnGUI() {
		Event e = Event.current;
		Debug.Log(e.mousePosition);
	}*/
	
	void touchPosition()
	{
		//Ray ray = new Ray ();
		//RaycastHit hit = new RaycastHit();
		
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {

			//ray = Camera.main.ScreenPointToRay( Input.GetTouch(0));

			//if( Physics2D.Raycast( hit, out ray, Mathf)){
				// Determines position in mobile platform mode
				//print(hit.collider.name);
				Vector2 touchDeltaPosition = Input.GetTouch (0).deltaPosition;
				transform.Translate (-touchDeltaPosition.x * speed, 0, 0);
			//}
		}
	}
}