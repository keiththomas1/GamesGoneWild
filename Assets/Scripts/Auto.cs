using UnityEngine;
using System.Collections;

public class Auto : MonoBehaviour {
	public GameObject globalController;

	float autoacc;
	float speed;
	float angle;
	string strSpeed;
	float Fallspeed_1 = 3.0f;
	float Fallspeed_2 = 2.0f;
	//string stroutput1;


	// Use this for initialization
	void Start () 
	{
		globalController = GameObject.Find( "Global Controller" );

		// random start can f
		if (Random.value > .5) 
		{
			speed = 25.0f;

		}
		else
		{
			speed = -25.0f;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{

		//stroutput1 = (transform.localEulerAngles.z).ToString ();
		//GuiTextDebug.debug (stroutput1);
		// at different angles the speed increases so its not static
		if (speed > 0.0f)  // postive speed
		{
			if(transform.localEulerAngles.z <= 30 && transform.localEulerAngles.z >= 0)
			{
				float curSpeed = Time.deltaTime *1* speed;
				transform.Rotate (0, 0, curSpeed);
				strSpeed = speed.ToString();
				GuiTextDebug.debug ("1");
			}
			else if(transform.localEulerAngles.z <= 60 && transform.localEulerAngles.z >= 30)
			{
				float curSpeed = Time.deltaTime *Fallspeed_1*speed;
				transform.Rotate (0, 0, curSpeed);
				GuiTextDebug.debug ("2");
			}
			else if (transform.localEulerAngles.z <= 80 && transform.localEulerAngles.z >= 60) 
			{
				float curSpeed = Time.deltaTime *Fallspeed_2* speed;
				transform.Rotate (0, 0, curSpeed);
				GuiTextDebug.debug ("3");
			} 
			else if(transform.localEulerAngles.z >= 330 && transform.localEulerAngles.z < 360 )
			{
				float curSpeed = Time.deltaTime *-1* speed;
				transform.Rotate (0, 0, curSpeed);
				GuiTextDebug.debug ("4");
			}
			else if(transform.localEulerAngles.z >= 300 && transform.localEulerAngles.z <= 330)
			{
				float curSpeed = Time.deltaTime *-Fallspeed_1* speed;
				transform.Rotate (0, 0, curSpeed);
				GuiTextDebug.debug ("5");
			}
			else if(transform.localEulerAngles.z <= 300 && transform.localEulerAngles.z >= 280) 
			{
				float curSpeed = Time.deltaTime *-Fallspeed_2* speed;
				transform.Rotate (0, 0, curSpeed);
				GuiTextDebug.debug ("6");
			}
			else if(transform.localEulerAngles.z > 80 && transform.localEulerAngles.z < 280 )
			{
				globalController.GetComponent<GlobalController>().LostMinigame();
				GuiTextDebug.debug ("you lost");
			}
		}
		else // negative speed;
		{
			if(transform.localEulerAngles.z <= 30 && transform.localEulerAngles.z >= 0)
			{
				float curSpeed = Time.deltaTime *-1* speed;
				transform.Rotate (0, 0, curSpeed);
				GuiTextDebug.debug ("11");
				strSpeed = speed.ToString();
				GuiTextDebug.debug (strSpeed);
			}
			else if(transform.localEulerAngles.z <= 60 && transform.localEulerAngles.z >= 30)
			{
				float curSpeed = Time.deltaTime *-Fallspeed_1*speed;
				transform.Rotate (0, 0, curSpeed);
				GuiTextDebug.debug ("12");
			}
			else if (transform.localEulerAngles.z <= 80 && transform.localEulerAngles.z >= 60) 
			{
				float curSpeed = Time.deltaTime *-Fallspeed_2* speed;
				transform.Rotate (0, 0, curSpeed);
				GuiTextDebug.debug ("13");
			} 
			else if(transform.localEulerAngles.z >= 330 && transform.localEulerAngles.z < 360 )
			{
				float curSpeed = Time.deltaTime *1* speed;
				transform.Rotate (0, 0, curSpeed);
				GuiTextDebug.debug ("14");
			}
			else if(transform.localEulerAngles.z >= 300 && transform.localEulerAngles.z <= 330)
			{
				float curSpeed = Time.deltaTime *Fallspeed_2* speed;
				transform.Rotate (0, 0, curSpeed);
				GuiTextDebug.debug ("15");
			}
			else if( transform.localEulerAngles.z <= 300 && transform.localEulerAngles.z >= 280 ) 
			{
				float curSpeed = Time.deltaTime *Fallspeed_2* speed;
				transform.Rotate (0, 0, curSpeed);
				GuiTextDebug.debug ("16");
			}
			else if(transform.localEulerAngles.z > 80 && transform.localEulerAngles.z < 280)
			{
				globalController.GetComponent<GlobalController>().LostMinigame();
				GuiTextDebug.debug ("you lost");
			}
		}

	}
}
