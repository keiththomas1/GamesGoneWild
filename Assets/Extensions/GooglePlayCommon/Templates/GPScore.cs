////////////////////////////////////////////////////////////////////////////////
//  
// @module Common Android Native Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


 

using UnityEngine;
using System.Collections;

public class GPScore  {


	private int _rank;
	private int _score;

	private string _playerId;
	private string _leaderboardId;

	private GPCollectionType _collection;
	private GPBoardTimeSpan _timeSpan;

	

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------


	public GPScore(int vScore, int vRank, GPBoardTimeSpan vTimeSpan, GPCollectionType sCollection, string lid, string pid) {
		_score = vScore;
		_rank = vRank;

		_playerId = pid;
		_leaderboardId = lid;
	

		_timeSpan  = vTimeSpan;
		_collection = sCollection;

	}


	//--------------------------------------
	// GET / SET
	//--------------------------------------


	public int rank {
		get {
			return _rank;
		}
	}


	public int score {
		get {
			return _score;
		}
	}

	public string playerId {
		get {
			return _playerId;
		}
	}

	public string leaderboardId {
		get {
			return _leaderboardId;
		}
	}
	

	public GPCollectionType collection {
		get {
			return _collection;
		}
	}


	public GPBoardTimeSpan timeSpan {
		get {
			return _timeSpan;
		}
	}

}
