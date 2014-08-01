
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.Xml;
using System.Text;
using System.Linq;

using System.Collections;

public class SAPostProcessAndroid : MonoBehaviour {

	//TODO Check Manifest

	[PostProcessBuild(99)]
	public static void OnPostProcessBuild(BuildTarget target, string path) {
		var outputFile = Path.Combine(Application.dataPath, "Plugins/Android/");
		if (!Directory.Exists(outputFile)) {
			Debug.LogError("Plugins/Android Directory not found. Android plugin failed to run. Make sure you have read set up Guide");
		} 
	}
}
