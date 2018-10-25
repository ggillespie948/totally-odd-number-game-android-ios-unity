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

	[SerializeField]
	private BoardEvaluator averageScoreEvaluator;

	[SerializeField]
	public Material[] fontPresets;



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
		string[] ret = e.Split('.');

		
		if(ret[1] == "Score")
		{
			ProcessScoreNotification(int.Parse(ret[2]));

		} else if (ret[1] == "ScoreSound")
		{
			ProcessScoreSoundNotification(int.Parse(ret[2]));
		} else if (ret[1] == "TimesUp")
		{
			GUI_Controller.instance.SpawnTextPopup("Times Up!", Color.white, GUI_Controller.instance.transform, 36, fontPresets[0]);
			AudioManager.instance.Play("TimesUp");
			foreach(GridTile tile in GameMaster.instance.currentHand)
			{
				Debug.Log("Disable disable");
				tile.GetComponent<BoxCollider2D>().enabled=false;
			}
			foreach(GridCell cell in GameMaster.instance.playedTiles)
			{
				Debug.Log("Disable played tile");
				if(cell.cellTile != null)
				cell.cellTile.GetComponent<BoxCollider2D>().enabled=false;
			}
		} else if (ret[1] == "Odd")
		{
			
		} else if (ret[1] == "Exchange")
		{
			GUI_Controller.instance.SpawnTextPopup("Exchange", Color.white, GUI_Controller.instance.transform, 36, fontPresets[0]);

			
		}


		


	}

	private void ProcessScoreNotification(int score)
	{
		if(score < 5 && GameMaster.instance.totalTiles > 9)
		{
			int randPhrase = Random.Range(0, Tier0_Score_Phrase.Length);
			GUI_Controller.instance.SpawnTextPopup(Tier0_Score_Phrase[randPhrase], Color.white, GameMaster.instance.playedTiles[0].transform, 30, fontPresets[0]);

		} else if(score > 30 && score < 40)
		{
			int randPhrase = Random.Range(0, Tier1_Score_Phrase.Length);
			GUI_Controller.instance.SpawnTextPopup(Tier1_Score_Phrase[randPhrase], Color.cyan, GameMaster.instance.playedTiles[0].transform, 30, fontPresets[1]);
			
		} else if (score >= 40 && score <= 60)
		{
			int randPhrase = Random.Range(0, Tier2_Score_Phrase.Length);
			GUI_Controller.instance.SpawnTextPopup(Tier2_Score_Phrase[randPhrase], Color.white, GameMaster.instance.playedTiles[0].transform, 32, fontPresets[2]);

		} else if (score > 60)
		{
			int randPhrase = Random.Range(0, Tier3_Score_Phrase.Length);
			GUI_Controller.instance.SpawnTextPopup(Tier3_Score_Phrase[randPhrase], Color.white, GameMaster.instance.playedTiles[0].transform,35, fontPresets[3]);
		}
	}

	private void ProcessScoreSoundNotification(int score)
	{
		Debug.Log("Processing notification");
		if(score < 5 && GameMaster.instance.totalTiles > 9)
		{
			int randPhrase = Random.Range(0, Tier0_Score_Phrase.Length);
			AudioManager.instance.Play("ScoreT0");

		} else if(score > 30 && score < 40)
		{
			int randPhrase = Random.Range(0, Tier1_Score_Phrase.Length);
			AudioManager.instance.Play("ScoreT2");
			
		} else if (score >= 40 && score <= 60)
		{
			int randPhrase = Random.Range(0, Tier2_Score_Phrase.Length);
			AudioManager.instance.Play("ScoreT3");

		} else if (score > 60)
		{
			int randPhrase = Random.Range(0, Tier3_Score_Phrase.Length);
			AudioManager.instance.Play("ScoreT4");
		}
		else 
		{
			AudioManager.instance.Play("ScoreT1");
		}

	}

	

}
