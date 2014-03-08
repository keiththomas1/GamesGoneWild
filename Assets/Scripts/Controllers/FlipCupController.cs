using UnityEngine;
using System.Collections;

public class FlipCupController : MonoBehaviour {

	public GameObject Cup_placeholder;
	public GameObject Table_placeholder;
	Vector3 initPos;
	Vector3 finalPos;
	Vector3 Pos;
	Vector3 FlickPos = new Vector3(0,0,0);
	Vector3 FlickAmmount = new Vector3(0,-70,0);
	Vector3 startPosition = new Vector3(0,1,-2);


	int count = 0;
	bool isFlicked;

	// Use this for initialization
	void Start () {
		isFlicked= false;
	    float mass = (float).2;
		rigidbody.centerOfMass = new Vector3 (0,mass, 0);
	
	}
	
	// Update is called once per frame
	void Update () {
		//mouse down
		if (!isFlicked){
		if (Input.GetMouseButtonDown (0)) {
			initPos = Input.mousePosition*10;
		}
		//swipes to mouse up and find the distance moved and apply force
		if (Input.GetMouseButtonUp (0)) {
			finalPos = Input.mousePosition*10;
			Pos = finalPos - initPos;

			Cup_placeholder.rigidbody.AddForce(Pos);		//drag distance of the mouse as a force
			Cup_placeholder.rigidbody.AddForce(0,0,200);	//pushes cup from edge onto table
			Cup_placeholder.rigidbody.AddForceAtPosition(FlickAmmount,FlickPos);// simulates the rotation of the cup
			isFlicked = true; //the cup has been flicked
		}
		}
		//if the balls y pos is in the landed area and is not changing, then success!
		if(isFlicked){
			//hardcoded....only way i found that worked..
			if (transform.position.y >= 1.129990 && transform.position.y <= 13.0006)
				count++;
		}

		if (count == 200){
			Debug.Log ("landed!!!!! Reloading level");
			isFlicked = false;
			count = 0;
			DestroyObject(Cup_placeholder);
			Instantiate(Cup_placeholder,startPosition,transform.rotation);
			Debug.Log (transform.position);
			//**********Once ball is instantiated.. it won't let me flick it again.
		}
		if (transform.position.y <0){
			DestroyObject(Cup_placeholder);
			//Instantiate(Cup_placeholder,startPosition,transform.rotation);
		}
	}
}	

