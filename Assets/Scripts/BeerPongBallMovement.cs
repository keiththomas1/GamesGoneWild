﻿using UnityEngine;
using System.Collections;

public class BeerPongBallMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) 
	{
		Debug.Log("Collision!");
	}
	
	void OnCollisionEnter(Collision other) 
	{
		Debug.Log("Collision!");
	}
}
