
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(SocialPlatfromSettings))]
public class SocialPlatfromSettingsEditor : Editor {


	
	
	GUIContent FBScopes   = new GUIContent("Permissions [?]:", "A person's privacy settings combined with what you ask for will determine what you can access.");
	GUIContent TConsumerKey   = new GUIContent("Consumer Key [?]:", "Twitter register app consumer key");
	GUIContent TConsumerSecret   = new GUIContent("Consumer Secret [?]:", "Twitter register app consumer secret");


	
	GUIContent SdkVersion   = new GUIContent("Plugin Version [?]", "This is Plugin version.  If you have problems or compliments please include this so we know exactly what version to look out for.");
	GUIContent SupportEmail = new GUIContent("Support [?]", "If you have any technical quastion, feel free to drop an e-mail");


	private SocialPlatfromSettings settings;

	public override void OnInspectorGUI() {
		settings = target as SocialPlatfromSettings;

		GUI.changed = false;




		FacebookSettings();
		EditorGUILayout.Space();
		TwitterSettings();
		EditorGUILayout.Space();
		AboutGUI();
	

		if(GUI.changed) {
			DirtyEditor();
		}
	}

	


	private void FacebookSettings() {
		EditorGUILayout.HelpBox("Facebook Settings", MessageType.None);


		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(FBScopes);
		settings.fb_scopes	 	= EditorGUILayout.TextField(settings.fb_scopes);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.Space();
		if(GUILayout.Button("Documentation",  GUILayout.Width(100))) {
			Application.OpenURL("https://developers.facebook.com/docs/facebook-login/permissions/v2.0");
		}

		EditorGUILayout.EndHorizontal();


	}

	public void TwitterSettings() {
		EditorGUILayout.HelpBox("Twitter Settings", MessageType.None);


		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(TConsumerKey);
		settings.TWITTER_CONSUMER_KEY	 	= EditorGUILayout.TextField(settings.TWITTER_CONSUMER_KEY);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(TConsumerSecret);
		settings.TWITTER_CONSUMER_SECRET	 	= EditorGUILayout.TextField(settings.TWITTER_CONSUMER_SECRET);
		EditorGUILayout.EndHorizontal();
	}




	private void AboutGUI() {




		EditorGUILayout.HelpBox("Mobile Social Plugin", MessageType.None);
		EditorGUILayout.Space();
		
		SelectableLabelField(SdkVersion, SocialPlatfromSettings.VERSION_NUMBER);
		SelectableLabelField(SupportEmail, "stans.assets@gmail.com");
		
		
	}
	
	private void SelectableLabelField(GUIContent label, string value) {
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(label, GUILayout.Width(180), GUILayout.Height(16));
		EditorGUILayout.SelectableLabel(value, GUILayout.Height(16));
		EditorGUILayout.EndHorizontal();
	}



	private static void DirtyEditor() {
		#if UNITY_EDITOR
		EditorUtility.SetDirty(SocialPlatfromSettings.Instance);
		#endif
	}
	
	
}
