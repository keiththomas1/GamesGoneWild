using UnityEngine;
using System.Collections;

public class SaveTheFloorsContoller : MonoBehaviour {

	// Creating our gameobject for controlling the scene
	public GameObject [] pukePrefabArray;

	// Index for array of pukePrefab gameobject
	int pukePrefabIndex;

	// This will initiate gravity when needed
	bool StartPuking;

	// Storing y - location of current puke gameobject
	Vector3 currentPos;

	// Use this for initialization
	void Start () {

		// Set our current index of gameobject array
		pukePrefabIndex = 0;

		// Initializing identifier so no gravity is given to puke gameobject
		StartPuking = false;
	}
	
	// Update is called once per frame
	void Update () {

		// Click on screen in order to start the game
		StartPuking = StartGame ();

		// Debugging purposes
		Debug.Log ("StartPuking is " + StartPuking + " before if condition");

		if (StartPuking == true && pukePrefabIndex < 5) {

			// Debugging purposes
			//Debug.Log ("StartPuking is " + StartPuking + " before switch case");
			Debug.Log ("pukePrefabIndex is " + pukePrefabIndex);

			// Giving the current element in pukePrefabArray gravity
			pukePrefabArray [pukePrefabIndex].GetComponent<Puke_Behavior> ().Gravity ();
			StartPuking = false;
		}

		// Detecting current location of the puke gameobject
		currentPos = pukePrefabArray [pukePrefabIndex].transform.position;

		// If puke gameobject falls below this range, player loses game and exits game
		if ( currentPos.y < -7)
			Debug.Log ("You lose");
	}
	
	// Starts the game by touching the screen or clicking on mouse
	bool StartGame(){

		// Send the first puke gameobject with a touch of the screen or click of a mouse button
		if( Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began ||
		   Input.GetMouseButtonDown(0))
			StartPuking = true;
		return StartPuking;
	}

	// Destroy this instance puke gameobject.
	public void DestroyPuke(){

		// Debugging purposes
		Debug.Log ("In DestroyPuke() from controller");

		// Destroy puke gameobject at curreent element in list
		DestroyObject (pukePrefabArray [pukePrefabIndex]);
		// Go the next element of pukePrefabArray
		pukePrefabIndex++;
		// Give the next pukePrefab element some gravity
		StartPuking = true;
	}

	void OnGUI(){

		GUILayout.BeginArea ( new Rect (90, 10, 100, 100));
		GUILayout.Label ("Tap to start\n\nCatch the falling puke by moving the bucket");
		GUILayout.EndArea ();
	}
}
