using UnityEngine;
using System.Collections;

public class DefaultPreviewButton : EventDispatcher {

	public Texture normalTexture;
	public Texture pressedTexture;
	public Texture disabledTexture;
	public AudioClip sound;
	public AudioClip disabledsound;


	private bool IsDisabled = false;


	void Awake() {
		if(audio == null) {
			gameObject.AddComponent<AudioSource>();
			audio.clip = sound;
			audio.Stop();
		}

		renderer.material =  new Material(renderer.material);
	}


	public void DisabledButton() {
		if(disabledTexture != null) {
			renderer.material.mainTexture = disabledTexture;
		}
		IsDisabled = true;
	}

	public void EnabledButton() {
		if(disabledTexture != null) {
			renderer.material.mainTexture = normalTexture;
		}
		IsDisabled = false;
	}



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


	void Update() {

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		if(Input.GetMouseButtonDown(0)){

			
			if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
				if(hit.transform.gameObject == gameObject) {
					OnClick();
				}
			}
		} 

	}


	protected virtual void OnClick() {
		if(IsDisabled) {
			audio.PlayOneShot(disabledsound);
			return;
		} 
		audio.PlayOneShot(sound);
		renderer.material.mainTexture = pressedTexture;
		dispatch(BaseEvent.CLICK);
		CancelInvoke("OnTimeoutPress");
		Invoke("OnTimeoutPress", 0.1f);
	}

	private void OnTimeoutPress() {
		renderer.material.mainTexture = normalTexture;
	}
}
