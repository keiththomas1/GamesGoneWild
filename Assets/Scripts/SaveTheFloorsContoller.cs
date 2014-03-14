using UnityEngine;
using System.Collections;

public class SaveTheFloorsContoller : MonoBehaviour {

	// Creating our gameobject for controlling the scene
	public GameObject [] pukePrefabArray;

	// Index for array of pukePrefab gameobject
	int pukePrefabIndex;

	// This will initiate gravity when needed
	bool StartPuking;

	// Use this for initialization
	void Start () {

		// Set our current index of gameobject array
		pukePrefabIndex = 0;

		//
		StartPuking = false;
	}
	
	// Update is called once per frame
	void Update () {

		//
		StartPuking = StartGame ();

		// Debugging purposes
		Debug.Log ("StartPuking is " + StartPuking + " before if condition");

		if (StartPuking == true && pukePrefabIndex < 1) {

			// Debugging purposes
			//Debug.Log ("StartPuking is " + StartPuking + " before switch case");
			Debug.Log ("pukePrefabIndex is " + pukePrefabIndex + " in switch case");

			//
			pukePrefabArray [pukePrefabIndex].GetComponent<Puke_Behavior> ().Gravity ();
			StartPuking = false;
		}
	}

	/// <summary>
	/// Starts the game.
	/// </summary>
	bool StartGame(){

		// Send the first puke gameobject
		if( Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began ||
		   Input.GetMouseButtonDown(0))
			StartPuking = true;
		return StartPuking;
	}
	
	/// <summary>
	/// Destroy this instance.
	/// </summary>
	public void DestroyPuke(){

		Debug.Log ("In DestroyPuke() from controller");
		// Destroy puke gameobject at curreent element in list
		Destroy (pukePrefabArray [pukePrefabIndex]);
		pukePrefabIndex++;
		StartPuking = true;
	}
}
