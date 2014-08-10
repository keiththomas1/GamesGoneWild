////////////////////////////////////////////////////////////////////////////////
//  
// @module Common Android Native Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


 

using UnityEngine;
using System.Collections;

public abstract class SA_Singleton<T> : EventDispatcher where T : MonoBehaviour {

	private static T _instance = null;



	public static T instance {

		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType(typeof(T)) as T;
				if (_instance == null) {
					_instance = new GameObject ().AddComponent<T> ();
					_instance.gameObject.name = _instance.GetType ().Name;
				}
			}

			return _instance;

		}

	}

	public static bool IsDestroyed {
		get {
			if(_instance == null) {
				return true;
			} else {
				return false;
			}
		}
	}

}
