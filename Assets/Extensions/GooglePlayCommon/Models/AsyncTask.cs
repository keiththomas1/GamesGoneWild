using UnityEngine;
using System.Collections;

public class AsyncTask : EventDispatcher {


	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

}

