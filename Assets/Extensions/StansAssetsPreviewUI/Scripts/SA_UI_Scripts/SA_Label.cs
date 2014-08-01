using UnityEngine;
using System.Collections;

public class SA_Label : MonoBehaviour {

	public string text {
		get {
			TextMesh mesh  = gameObject.GetComponentInChildren<TextMesh>();
			return mesh.text;
		}
		
		set {
			TextMesh[] meshes  = gameObject.GetComponentsInChildren<TextMesh>();
			foreach(TextMesh mesh in meshes) {
				mesh.text = value;
			}
		}
	}
}
