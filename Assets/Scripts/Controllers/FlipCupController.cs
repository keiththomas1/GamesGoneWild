using UnityEngine;
using System.Collections;

public class FlipCupController : MonoBehaviour {

	public GameObject Cup_placeholder;
	Vector3 initPos;
	Vector3 finalPos;
	Vector3 Pos;
	Vector3 FlickPos = new Vector3(0,0,0);
	Vector3 FlickAmmount = new Vector3(0,-100,0);


	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
		//mouse down
		if (Input.GetMouseButtonDown (0)) {
			initPos = Input.mousePosition*10;
		}
		//swipes to mouse up and find the distance moved and apply force
		if (Input.GetMouseButtonUp (0)) {
			finalPos = Input.mousePosition*10;
			Pos = finalPos - initPos;

			Cup_placeholder.rigidbody.AddForce(Pos);		//drag distance of the mouse as a force
			Cup_placeholder.rigidbody.AddForce(0,0,300);	//pushes cup from edge onto table
			Cup_placeholder.rigidbody.AddForceAtPosition(FlickAmmount,FlickPos); // simulates the rotation of the cup

		}
		                            
	}

	void OnCollisionEnter(Collision collision) {
		foreach (ContactPoint contact in collision.contacts) {
			Debug.DrawRay(contact.point, contact.normal, Color.white);
		}
	}	   
}
