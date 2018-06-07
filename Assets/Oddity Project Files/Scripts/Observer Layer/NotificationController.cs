using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationController : MonoBehaviour, Observer {

	[SerializeField]
	private string[] Tier0_Score_Phrase; //these are negative score for players who score < 7 points in a turn past 5 total turns
	[SerializeField]
	private string[] Tier1_Score_Phrase; 
	[SerializeField]
	private string[] Tier2_Score_Phrase;
	[SerializeField]
	private string[] Tier3_Score_Phrase;



	// Use this for initialization
	void Start () {
		GUI_Controller.instance.AddObserver(this);
		BoardController.instance.AddObserver(this);
		//foreach player var in game master.. add observer
	}
	
	public void OnNotification(MonoBehaviour _class, string e)
	{
		//Split string into three identifiers
		// Event.Score.24
		// Event.Objective. something //temp add this <-
		Debug.LogWarning("Notif recieved: " + e);
		Debug.Log("Notifcation received");
		string[] ret = e.Split('.');

		Debug.Log(ret.Length);
		
		if(ret[1] == "Score")
		{
			ProcessScoreNotification(int.Parse(ret[2]));

		}

	}

	private void ProcessScoreNotification(int score)
	{
		Debug.Log("Processing notification");
		if(score < 5 && GameMaster.instance.totalTiles > 9)
		{
			int randPhrase = Random.Range(0, Tier0_Score_Phrase.Length);
			GUI_Controller.instance.SpawnTextPopup(Tier0_Score_Phrase[randPhrase], Color.gray, GameMaster.instance.playedTiles[0].transform, 23);
			AudioManager.instance.Play("ScoreT0");

		} else if(score > 30 && score < 40)
		{
			int randPhrase = Random.Range(0, Tier1_Score_Phrase.Length);
			GUI_Controller.instance.SpawnTextPopup(Tier1_Score_Phrase[randPhrase], Color.cyan, GameMaster.instance.playedTiles[0].transform, 18);
			AudioManager.instance.Play("ScoreT2");
			
		} else if (score >= 40 && score <= 60)
		{
			int randPhrase = Random.Range(0, Tier2_Score_Phrase.Length);
			GUI_Controller.instance.SpawnTextPopup(Tier2_Score_Phrase[randPhrase], Color.yellow, GameMaster.instance.playedTiles[0].transform, 23);
			AudioManager.instance.Play("ScoreT3");

		} else if (score > 60)
		{
			int randPhrase = Random.Range(0, Tier3_Score_Phrase.Length);
			GUI_Controller.instance.SpawnTextPopup(Tier3_Score_Phrase[randPhrase], Color.white, GameMaster.instance.playedTiles[0].transform,27);
			AudioManager.instance.Play("ScoreT4");
		}
		else 
		{
			AudioManager.instance.Play("ScoreT1");
		}

	}

	

}
