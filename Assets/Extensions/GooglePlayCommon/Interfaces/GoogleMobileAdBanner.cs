////////////////////////////////////////////////////////////////////////////////
//  
// @module Common Android Native Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


 

using UnityEngine;
using System.Collections;

public interface GoogleMobileAdBanner  {
	
	void Hide();
	void Show();
	void Refresh();
	void SetBannerPosition(int x, int y);
	void SetBannerPosition(TextAnchor anchor);



	int id {get;}
	int width {get;}
	int height {get;}

	bool IsLoaded {get;}
	bool IsOnScreen {get;}
	bool ShowOnLoad{get; set;}

	GADBannerSize size {get;}
	TextAnchor anchor {get;}
	

	void addEventListener(string eventName, EventHandlerFunction handler);
	void addEventListener(string eventName, DataEventHandlerFunction handler);

	void removeEventListener(string eventName, 	EventHandlerFunction handler);
	void removeEventListener(string eventName,  DataEventHandlerFunction handler);

}
