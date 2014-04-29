using UnityEngine;
using System.Collections;

public class QuartersController : MonoBehaviour {
	public GameObject Ball;
	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 position = new Vector3 (0f, 0f, 0f);
	    position = Input.mousePosition;
		//Debug.Log (Input.mousePosition);
		bool touchingBall=false;
		if (position.x <571f && position.x >553f && position.y <161f && position.y >139f)
			touchingBall=true;
		if (Input.GetMouseButtonDown(0) && touchingBall){
			Ball.transform.position = position;
		}

	
	}
}
