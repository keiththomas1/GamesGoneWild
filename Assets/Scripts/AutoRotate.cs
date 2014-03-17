using UnityEngine;
using System.Collections;
//public Transform pivot;

public class AutoRotate : MonoBehaviour {
	public float accspeed = 100.0f;
	float accx;
	float acc_x;
	float speed_program;
	int speed = -25; // regular speed
	string output;
	string output1;
	string straccx;
	float time = 6.0f;
	// Use this for initialization

	void Start () 
	{
				//HingeJoint2D  hinge = GetComponent<HingeJoint2D>();
				//JointMotor2D motor = hinge.motor;
				//The character falls randomly left or right
				//hinge.motor = motor;	
		acc_x = Input.acceleration.x; // saving for reference start
	}	
	
	
	// Update is calle		d once per frame
	void Update () 
	{

		time -= Time.deltaTime;
		if (time < 0)
		{
			if((transform.localEulerAngles.z <= 85 && transform.localEulerAngles.z >= 0) || (transform.localEulerAngles.z <= 360 && transform.localEulerAngles.z >= 280))
			{
				GuiTextDebug.debug ("you won");
			}
		}
		else
		{
			float curSpeed = Time.deltaTime * speed;
			//float curspeed_program = Time.deltaTime * speed_program;
			if((transform.localEulerAngles.z <= 85 && transform.localEulerAngles.z >= 0) || (transform.localEulerAngles.z <= 360 && transform.localEulerAngles.z >= 280))
			{
				if ((Input.acceleration.x - acc_x) == 0) 
				{
					
				} 
				else 
				{
					if(transform.localEulerAngles.z >= 290 && transform.localEulerAngles.z <= 359)
					{
						accx = (Input.acceleration.x * 10) * curSpeed;
						transform.Rotate (0, 0, accx);
					}
					else if(transform.localEulerAngles.z <= 70 && transform.localEulerAngles.z >= 1)
					{
						accx = (Input.acceleration.x * 10) * curSpeed;
						transform.Rotate (0, 0, accx);
					}
					else
					{
						
					}
				}
			}
			else
			{
				
			}
		}



		straccx = accx.ToString ();
		output = "x:" + straccx;
		output1 = (transform.localEulerAngles.z).ToString ();
		//GuiTextDebug.debug (output);
		//GuiTextDebug.debug (output1);
	}

	void OnGUI()
	{
		if (time > 0.0)
		{
			GUI.Box (new Rect (20, 20, 30, 20), "" + time.ToString ("0.0"));
		}
		else
		{
			GUI.Box (new Rect (20, 20, 30, 20), "" + "0.0" );
		}
	}
}



