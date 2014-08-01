using UnityEngine;
using System.Collections;

public class SA_Texture : MonoBehaviour {


	void Awake () {
		renderer.material =new Material(renderer.material);
	}
	
	public Texture  texture {
		get {
			return renderer.material.mainTexture;
		}

		set {
			renderer.material.mainTexture = value;
		}
	}
}
