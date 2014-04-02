using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HighScoreController : MonoBehaviour {

	public GameObject globalController;

	private const int LeaderBoardLength = 10;

	public int points;

	void Start () {
		Debug.Log ("Entered HighScore");

		globalController = GameObject.Find ("Global Controller");

		if(!globalController)
			Debug.Log ("fucking why is it false");

		points = globalController.GetComponent<GlobalController> ().partyPoints;



		SaveHighScore ("You", points);
		Debug.Log (GetHighScore());
	}

	// Update is called once per frame
	void Update () {

		
	}

	public void SaveHighScore(string name, int score){
		List<Scores> HighScores = new List<Scores> ();

		int i = 1;
		while (i<=LeaderBoardLength && i<=HighScores.Count){
	        Scores temp = new Scores ();
   		    temp.name = name;
		    temp.score = score;
		    HighScores.Add (temp);
			PlayerPrefs.SetInt("Score"+i+"score",HighScores[i-1].score);
		}
	}
	public List<Scores> GetHighScore(){
		List<Scores> HighScores = new List<Scores>();

		int i = 1;

		while(i<=LeaderBoardLength && PlayerPrefs.HasKey("Score"+i+"score")){
		    Scores temp = new Scores();
		    temp.score = PlayerPrefs.GetInt ("Score"+i+"score");
			HighScores.Add(temp);
		}
		return HighScores;
	}

	public class Scores{
		public int score;
		public string name;
	}

	void OnApplicationQuit(){
		PlayerPrefs.Save();
	}
}
