using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUI_Dialogue_Call : MonoBehaviour {

    [Header("General Attributes (optional)")]
	public TextMeshProUGUI playerNameTxt;
	public TextMeshProUGUI playerScoreTxt;
	public TextMeshProUGUI playerErrorsTxt;
	public TextMeshProUGUI playerBestScoreTxt;

	public TextMeshProUGUI tilesPlayedTxt;

	public TextMeshProUGUI objective1Txt;
	public TextMeshProUGUI objective2Txt;
	public TextMeshProUGUI objective3Txt;

	[Header("VS Attributes (optional)")]
	public TextMeshProUGUI summaryText;

	[Header("World Texts (optional)")]
	public TextMeshProUGUI beginnerText;
	public TextMeshProUGUI intermediateText;
	public TextMeshProUGUI advancedText;
	public TextMeshProUGUI masterText;

    [Header("Target Mode Attributes (optional)")]
	public starFxController targetStarController;
	public TextMeshProUGUI targetScoreText;

	[Header("Objective Panel Attributes")]

	private int playerCounter;

	public bool isOpen = false;

	public GameObject objectivePanel;
	public GameObject playerPanel;

	public GameObject objectiveStar1;
	public GameObject objectiveStar2;
	public GameObject objectiveStar3;
	


	public void Open() {
		isOpen=true;
        gameObject.GetComponentInChildren<GUI_Dialogue>().Open();

		if(beginnerText != null)
			beginnerText.text = "Beginner "+AccountInfo.beginnerStars+"/15";
		if(intermediateText != null)
			intermediateText.text = "Intermediate "+AccountInfo.intermediateStars+"/15";
		if(advancedText != null)
			advancedText.text = "Requires 30 stars";
		if(masterText != null)
			masterText.text = "Requires 45 stars";
    }

	public void Close() {
		isOpen = false;
        gameObject.GetComponentInChildren<GUI_Dialogue>().Close();
    }

	public void NextPlayer(List<int> playerScores, List<int> playerBestScores, List<int> playerErrors, int targetScore)
	{
		if(GameMaster.instance.soloPlay)
			return;
		GUI_Controller.instance.StopAllCoroutines();

		playerCounter++;
		Debug.LogWarning("PC: " + playerCounter);
		if(playerCounter==1)
		{
			playerPanel.SetActive(true);
			InitDialogue(playerScores, playerBestScores, playerErrors,targetScore,1);
			objectivePanel.SetActive(false);
			return;
		} else if(playerCounter==0)
		{
			playerPanel.SetActive(false);
			objectivePanel.SetActive(true);
			return;
		}

		if(playerCounter > GameMaster.instance.playerScores.Count)
		{
			Debug.LogWarning("PC Reset");
			playerCounter=0;
			playerPanel.SetActive(false);
			objectivePanel.SetActive(true);
			return;

		}

		

		bool bestTurnScore = true;
		 foreach(int score in playerBestScores)
		{
			if(score> playerBestScores[playerCounter-1])
				bestTurnScore = false;
		} 
		bool bestScore = true;
		 foreach(int score in playerScores)
		{
			if(score> playerScores[playerCounter-1])
				bestScore = false;
		}
		bool bestTilesplayed = true;
		foreach(int score in GameMaster.instance.PlayerStatistics.playedTiles)
		{
			if(score> GameMaster.instance.PlayerStatistics.playedTiles[playerCounter-1])
				bestTilesplayed = false;
		} 

		if(bestTurnScore)
		{
			playerBestScoreTxt.color = Color.green;
		}
		else
		{
			playerBestScoreTxt.color = Color.white;
			playerBestScoreTxt.fontStyle = FontStyles.Normal;
		}

		if(playerErrors[playerCounter-1] == 0)
		{
			playerErrorsTxt.color = Color.green;
		} else 
		{
			playerErrorsTxt.color = Color.white;
		}

		if(bestScore)
			playerScoreTxt.color = Color.green;
		else
		{
			playerScoreTxt.fontStyle = FontStyles.Normal;
			playerScoreTxt.color = Color.white;
		}

		if(bestTilesplayed)
			tilesPlayedTxt.color = Color.green;
		else
		{
			tilesPlayedTxt.fontStyle = FontStyles.Normal;
			tilesPlayedTxt.color = Color.white;
		}

		if(playerCounter==0)
		{
			return;
		}

		playerNameTxt.text = "Player " + playerCounter;

		if(bestScore)
		{
			playerNameTxt.text += " (Winner)";
		}

		if(playerScoreTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,playerScores[playerCounter-1], playerScoreTxt, "Score: "));}
		if(playerErrorsTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,playerErrors[playerCounter-1], playerErrorsTxt, "Errors: "));}
		if(playerBestScoreTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,playerBestScores[playerCounter-1], playerBestScoreTxt, "Best turn score: "));}
		if(tilesPlayedTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,GameMaster.instance.PlayerStatistics.playedTiles[playerCounter-1], tilesPlayedTxt, "Tiles played: "));}
		Debug.LogWarning("Tiles played count: " + GameMaster.instance.PlayerStatistics.playedTiles.Count);
	}

	public void PreviousPlayer(List<int> playerScores, List<int> playerBestScores, List<int> playerErrors, int targetScore)
	{
		if(GameMaster.instance.soloPlay)
			return;
		GUI_Controller.instance.StopAllCoroutines();
		playerCounter--;
		if(playerCounter == 0)
		{
			playerPanel.SetActive(false);
			//InitObjectivePanel();
			objectivePanel.SetActive(true);
			playerCounter=playerScores.Count;
		} else if (playerCounter < 0)
		{
			playerPanel.SetActive(true);
			InitDialogue(playerScores, playerBestScores, playerErrors,targetScore, playerBestScores.Count);
			objectivePanel.SetActive(false);
		}

		

		if(playerScoreTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,playerScores[playerCounter-1], playerScoreTxt, "Score: "));}
		if(playerErrorsTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,playerErrors[playerCounter-1], playerErrorsTxt, "Errors: "));}
		if(playerBestScoreTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,playerBestScores[playerCounter-1], playerBestScoreTxt, "Best turn score: "));}
		if(tilesPlayedTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,GameMaster.instance.PlayerStatistics.playedTiles[playerCounter-1], tilesPlayedTxt, "Tiles played: "));}

		bool bestTurnScore = true;
		 foreach(int score in playerBestScores)
		{
			if(score> playerBestScores[playerCounter-1])
				bestTurnScore = false;
		} 
		bool bestScore = true;
		 foreach(int score in playerScores)
		{
			if(score> playerScores[playerCounter-1])
				bestScore = false;
		}
		bool bestTilesplayed = true;
		foreach(int score in GameMaster.instance.PlayerStatistics.playedTiles)
		{
			if(score> GameMaster.instance.PlayerStatistics.playedTiles[playerCounter-1])
				bestTurnScore = false;
		} 

		if(bestTurnScore)
			playerBestScoreTxt.color = Color.green;
		else
		{
			playerBestScoreTxt.color = Color.white;
			playerBestScoreTxt.fontStyle = FontStyles.Normal;
		}

		if(playerErrors[playerCounter-1] == 0)
		{
			playerErrorsTxt.color = Color.green;
		} else 
		{
			playerErrorsTxt.color = Color.white;
		}

		if(bestScore)
			playerScoreTxt.color = Color.green;
		else
		{
			playerScoreTxt.fontStyle = FontStyles.Normal;
			playerScoreTxt.color = Color.white;
		}
		if(bestTilesplayed)
			tilesPlayedTxt.color = Color.green;
		else
		{
			tilesPlayedTxt.fontStyle = FontStyles.Normal;
			tilesPlayedTxt.color = Color.white;
		}

		playerNameTxt.text = "Player " + playerCounter;
		if(bestScore)
		{
			playerNameTxt.text += " (Winner)";
		}
	}

	public void InitObjectivePanel()
	{
		objective1Txt.text = GameMaster.instance.PlayerStatistics.GenerateObjectiveText(ApplicationModel.Objective1Code);
		objective2Txt.text = GameMaster.instance.PlayerStatistics.GenerateObjectiveText(ApplicationModel.Objective2Code);
		objective3Txt.text = GameMaster.instance.PlayerStatistics.GenerateObjectiveText(ApplicationModel.Objective3Code);

		//Generate Star Animations
		GameMaster.instance.PlayerStatistics.GenerateAllOBjectiveOutcomes();


		if(targetStarController != null)
		{
			//targetStarController.Reset();
			int starCount =0;
			if(GameMaster.instance.PlayerStatistics.OBJECTIVE_1)
			{
				starCount++;
				objectiveStar1.SetActive(true);
			} 
			if(GameMaster.instance.PlayerStatistics.OBJECTIVE_2)
			{
				starCount++;
				objectiveStar2.SetActive(true);
			}
			if(GameMaster.instance.PlayerStatistics.OBJECTIVE_3)
			{
				starCount++;
				objectiveStar3.SetActive(true);
			}

			Debug.LogWarning("Star Count: " + starCount);

			
			GameMaster.instance.starCount = starCount;
		}
		

		//Update PlayFab star data based on performance temp - refactor this into 000 or 101 format!!!!

		if(ApplicationModel.LEVEL_CODE != "" && AccountInfo.playfabId != null)
		{
			Debug.Log("Star Count: " + GameMaster.instance.starCount);
			AccountInfo.Instance.UpdatePlayerStarData(ApplicationModel.WORLD_NO, ApplicationModel.LEVEL_NO, ApplicationModel.LEVEL_CODE, GameMaster.instance.starCount);
		} else 
		{
			Debug.Log("Null level code..");
		}

		GUI_Controller.instance.CashRewardAnim();

	}

	public void InitDialogue(List<int> playerScores, List<int> playerBestScores, List<int> playerErrors, int targetScore, int pc)
	{
		playerCounter=pc;
		//playerCounter=1;
		playerNameTxt.text = "Player " + playerCounter;
		if(playerScoreTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,playerScores[playerCounter-1], playerScoreTxt, "Score: "));}
		if(playerErrorsTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,playerErrors[playerCounter-1], playerErrorsTxt, "Errors: "));}
		if(playerBestScoreTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,playerBestScores[playerCounter-1], playerBestScoreTxt, "Best turn score: "));}
		if(tilesPlayedTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,GameMaster.instance.PlayerStatistics.playedTiles[playerCounter-1], tilesPlayedTxt, "Tiles played: "));}

		bool bestTurnScore = true;
		 foreach(int score in playerBestScores)
		{
			if(score> playerBestScores[0])
				bestTurnScore = false;
		} 

		bool bestTilesplayed = true;
		foreach(int score in GameMaster.instance.PlayerStatistics.playedTiles)
		{
			if(score> GameMaster.instance.PlayerStatistics.playedTiles[0])
				bestTurnScore = false;
		} 

		bool bestScore = true;
		 foreach(int score in playerScores)
		{
			if(score> playerScores[0])
				bestScore = false;
		} 

		if(bestTurnScore)
			playerBestScoreTxt.color = Color.green;
		else
		{
			playerBestScoreTxt.color = Color.white;
			playerBestScoreTxt.fontStyle = FontStyles.Normal;
		}

		if(playerErrors[0] == 0)
		{
			playerErrorsTxt.color = Color.green;
		} else 
		{
			playerErrorsTxt.color = Color.white;
		}

		if(bestScore)
			playerScoreTxt.color = Color.green;
		else
		{
			playerScoreTxt.fontStyle = FontStyles.Normal;
			playerScoreTxt.color = Color.white;
		}

		if(bestTilesplayed)
			tilesPlayedTxt.color = Color.green;
		else
		{
			tilesPlayedTxt.fontStyle = FontStyles.Normal;
			tilesPlayedTxt.color = Color.white;
		}


		

	}


}
