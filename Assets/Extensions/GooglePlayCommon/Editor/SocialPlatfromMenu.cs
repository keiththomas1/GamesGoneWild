////////////////////////////////////////////////////////////////////////////////
//  
// @module V2D
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System.Collections;

public class SocialPlatfromMenu : EditorWindow {
	


	#if UNITY_EDITOR

	//--------------------------------------
	//  GENERAL
	//--------------------------------------

	[MenuItem("Window/Mobile Social Plugin/Edit Settings")]
	public static void Edit() {
		Selection.activeObject = SocialPlatfromSettings.Instance;
	}



	[MenuItem("Window/Mobile Social Plugin/Documentation/Getting Started")]
	public static void GettingStarted() {
		string url = "http://goo.gl/oaeXpk";
		Application.OpenURL(url);
	}


	
	//--------------------------------------
	//  FACEBOOK
	//--------------------------------------

	[MenuItem("Window/Mobile Social Plugin/Documentation/Facebook/SetUp")]
	public static void p1() {
		string url = "http://goo.gl/z1NBIR";
		Application.OpenURL(url);
	}

	[MenuItem("Window/Mobile Social Plugin/Documentation/Facebook/API References")]
	public static void p12() {
		string url = "http://goo.gl/SGtQF7";
		Application.OpenURL(url);
	}

	[MenuItem("Window/Mobile Social Plugin/Documentation/Facebook/Coding Guidelines")]
	public static void p13() {
		string url = "http://goo.gl/HD5wA5";
		Application.OpenURL(url);
	}

	[MenuItem("Window/Mobile Social Plugin/Documentation/Facebook/Platform Behavior differences")]
	public static void p14() {
		string url = "http://goo.gl/KBfHiC";
		Application.OpenURL(url);
	}



	//--------------------------------------
	//  TWITTER
	//--------------------------------------

	[MenuItem("Window/Mobile Social Plugin/Documentation/Twitter/SetUp")]
	public static void p15() {
		string url = "http://goo.gl/WJmzBp";
		Application.OpenURL(url);
	}
	
	[MenuItem("Window/Mobile Social Plugin/Documentation/Twitter/API References")]
	public static void p16() {
		string url = "http://goo.gl/nNigmA";
		Application.OpenURL(url);
	}
	
	[MenuItem("Window/Mobile Social Plugin/Documentation/Twitter/Coding Guidelines")]
	public static void p17() {
		string url = "http://goo.gl/FpvVRc";
		Application.OpenURL(url);
	}
	
	[MenuItem("Window/Mobile Social Plugin/Documentation/Twitter/Platform Behavior differences")]
	public static void p18() {
		string url = "http://goo.gl/h2gNO1";
		Application.OpenURL(url);
	}




	//--------------------------------------
	//  OTHER
	//--------------------------------------

	[MenuItem("Window/Mobile Social Plugin/Documentation/More Social Networks/Instagram")]
	public static void p20() {
		string url = "http://goo.gl/h2gNO1";
		Application.OpenURL(url);
	}

	[MenuItem("Window/Mobile Social Plugin/Documentation/More Social Networks/Native Sharing")]
	public static void p21() {
		string url = "http://goo.gl/JdVwo6";
		Application.OpenURL(url);
	}



	#endif

}
