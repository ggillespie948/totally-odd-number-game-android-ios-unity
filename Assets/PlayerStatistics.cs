using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour {

	//Main Objective indicators
	public bool OBJECTIVE_1 = false;
	public bool OBJECTIVE_2 = false;
	public bool OBJECTIVE_3 = false;

	//Exact Score indicators
	public int exactScore = 0;
	public bool turnScoreExactMet = false;

	public int p1Tiles = 0;
	public int p2Tiles = 0;
	public int p3Tiles = 0;
	public int p4Tiles = 0;

	public List<int> playedTiles;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		TurnScoreExactInit(ApplicationModel.Objective1Code);
		TurnScoreExactInit(ApplicationModel.Objective2Code);
		TurnScoreExactInit(ApplicationModel.Objective3Code);
		 
	}

	private void TurnScoreExactInit(string objective)
	{
		string[] ret = objective.Split('.');

		if(ret[0]=="TurnScoreExact")
		{
			Debug.LogWarning("TURN SCORE EXACT SET");
			exactScore=int.Parse(ret[1]);
		}
	}



	public string GenerateObjectiveText(string objCode)
	{
		string[] ret = objCode.Split('.');

		switch(ret[0])
		{
			case "Win":
				return "Win the game";

			case "WinBy":
				return "Win by "+ret[1]+" points";

			case "Fill":
				return "Fill the game grid completely";

			case "FillWin":
				return "Fill the game grid and win";

			case "BestTurnScore":
				return "Finish with the best turn score";

			case "TurnScore":
				return "Score " + ret[1] + " or more in a single turn";

			case "TurnScoreExact":
				return "Score exactly " + ret[1] + " in a single turn";

			case "MostTiles":
				return "Play the most tiles in the game";

			case "PlayTiles":
				return "Play " + ret[1] + " or more tiles in the game";

			case "Score":
				return "Score " + ret[1] + " or more";

			case "Errors":
				return "Finish with " + ret[1] + " errors or less";

			case "ErrorsWin":
				return "Win with  " + ret[1] + "errors or less";

			case "ErrorsMore":
				return "Win with  " + ret[1] + "errors or more";

			case "Odd":
				return "Win the game with an odd score";

			case "Even":
				return "Win the game with an even score";

			case "Activate":
				return "Activate " + ret[0] + " tiles in a single turn";

			case "RunnerUp":
				return "Finish 2nd or better in the game";

			default:
				return "404: Object code unrecognised";

		}

	}

	public void GenerateAllOBjectiveOutcomes()
	{
		OBJECTIVE_1=GenerateObjectiveOutcome(ApplicationModel.Objective1Code);
		OBJECTIVE_2=GenerateObjectiveOutcome(ApplicationModel.Objective2Code);
		OBJECTIVE_3=GenerateObjectiveOutcome(ApplicationModel.Objective3Code);
	}

	public bool GenerateObjectiveOutcome(string objCode)
	{
		string[] ret = objCode.Split('.');

		switch(ret[0])
		{
			case "Win":
				if(GameMaster.instance.playerWin) {return true;} 
			break;

			case "WinBy":
				if(GameMaster.instance.playerWin && (WinMargin(GameMaster.instance.playerScores) >= int.Parse(ret[1]))) {return true;} 
			break;

			case "Fill":
				return GameMaster.instance.gridFull;

			case "FillWin":
				if(GameMaster.instance.playerWin && GameMaster.instance.gridFull) {return true;}
			break;

			case "BestTurnScore":
				return BestTurnScore(GameMaster.instance.playerBestScores);

			case "TurnScore":
				if(GameMaster.instance.playerBestScores[0] >= int.Parse(ret[1])) {return true;} 
			break;

			case "TurnScoreExact":
				return turnScoreExactMet;

			case "MostTiles":
				return PlayedMostTiles(GUI_Controller.instance.GetAllTiles());

			case "PlayTiles":
				if(GameMaster.instance.playerPlayedTiles[0] >= int.Parse(ret[1])) {return true;} 
			break;

			case "Score":
				if(GameMaster.instance.playerScores[0] >= int.Parse(ret[1])){return true;}
			break;

			case "Errors":
				if(GameMaster.instance.playerErrors[0] <= int.Parse(ret[1])) {return true;} 
			break;

			case "ErrorsWin":
				if(GameMaster.instance.playerWin && GameMaster.instance.playerErrors[0] <= int.Parse(ret[1])) {return true;} 
			break;

			case "ErrorsMore":
				if(GameMaster.instance.playerWin && GameMaster.instance.playerErrors[0] >= int.Parse(ret[1])) {return true;} 
			break;

			case "Odd":
				if(GameMaster.instance.playerWin && GameMaster.instance.playerScores[0] %2 != 0) {return true;} 
			break;

			case "Even":
				if( GameMaster.instance.playerWin && GameMaster.instance.playerScores[0] %2 == 0) {return true;} 
			break;

			case "Activate":
				if(GameMaster.instance.playerBestTurnActivateTiles[0] >= int.Parse(ret[1])) {return true;}
			break;

			case "RunnerUp":
				return RunnerUp(GameMaster.instance.playerScores);

		}

		return false;

	}

	public bool BestScore(List<int> playerScores)
	{
		bool bestScore = true;
		foreach(int score in playerScores)
		{
			if(score> playerScores[0])
				bestScore = false;
		} 

		return bestScore;		
	}

	public bool PlayedMostTiles(GridTile[] playerTilesPlayed)
	{
		p1Tiles = 0;
		p2Tiles = 0;
		p3Tiles = 0;
		p4Tiles = 0;

		foreach(GridTile tile in playerTilesPlayed)
		{
			switch(tile.placedBy)
			{
				case 1:
				p1Tiles++;
				break;

				case 2:
				p2Tiles++;
				break;

				case 3:
				p3Tiles++;
				break;

				case 4:
				p4Tiles++;
				break;
			}
		}

		playedTiles = new List<int>();
		playedTiles.Clear();

		playedTiles.Add(p1Tiles);
		playedTiles.Add(p2Tiles);
		playedTiles.Add(p3Tiles);
		playedTiles.Add(p4Tiles);

		if((p1Tiles > p2Tiles)&&(p1Tiles > p3Tiles)&&(p1Tiles > p4Tiles))
		{
			return true;
		} else 
		{
			return false;
		}
		
	}

	public bool RunnerUp(List<int> playerScores)
	{
		if(!GameMaster.instance.playerWin)
		{
			return true;
		} else 
		{
			int betterCount = 0;
			for(int i=1; i<playerScores.Count; i++)
				{
					if(playerScores[i] > playerScores[0])
						betterCount++;
				}
			if(betterCount <= 1)
				return true;
			else
				return false;
		}
	}

	public int WinMargin(List<int> playerScores)
	{
		// //sort by scores,
		if(!GameMaster.instance.playerWin)
		{
			return 0;
		} else {
			int secondHighest = 0;
			for(int i=1; i<playerScores.Count; i++)
			{
				if(playerScores[i] > secondHighest)
					secondHighest=playerScores[i];
			}
			return playerScores[0]-secondHighest;
		}
	}

	public bool BestTurnScore(List<int> playerBestScores)
	{
		bool bestTurnScore = true;
		foreach(int score in playerBestScores)
		{
			if(score> playerBestScores[0])
				bestTurnScore = false;
		}

		return bestTurnScore; 
		
	}
	
}
