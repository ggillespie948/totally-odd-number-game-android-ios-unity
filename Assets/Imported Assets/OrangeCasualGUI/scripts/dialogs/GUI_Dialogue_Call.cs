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
	public starFxController objectiveStarController;

	private int playerCounter;

	public bool isOpen = false;

	public GameObject objectivePanel;
	public GameObject playerPanel;
	


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
		if(playerCounter==1)
		{
			playerPanel.SetActive(true);
			objectivePanel.SetActive(false);
		} else if(playerCounter==0)
		{
			playerPanel.SetActive(false);
			objectivePanel.SetActive(true);
		}


		if(playerCounter > GameMaster.instance.playerScores.Count)
			playerCounter=0;

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

		playerNameTxt.text = "Player " + playerCounter;
		if(playerScoreTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,playerScores[playerCounter-1], playerScoreTxt, "Score: "));}
		if(playerErrorsTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,playerErrors[playerCounter-1], playerErrorsTxt, "Errors: "));}
		if(playerBestScoreTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,playerBestScores[playerCounter-1], playerBestScoreTxt, "Best turn score: "));}
		
	}

	public void PreviousPlayer(List<int> playerScores, List<int> playerBestScores, List<int> playerErrors, int targetScore)
	{
		if(GameMaster.instance.soloPlay)
			return;
		GUI_Controller.instance.StopAllCoroutines();
		playerCounter--;
		if(playerCounter == 0)
			playerCounter=playerScores.Count;

		playerNameTxt.text = "Player " + playerCounter;
		if(playerScoreTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,playerScores[playerCounter-1], playerScoreTxt, "Score: "));}
		if(playerErrorsTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,playerErrors[playerCounter-1], playerErrorsTxt, "Errors: "));}
		if(playerBestScoreTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,playerBestScores[playerCounter-1], playerBestScoreTxt, "Best turn score: "));}

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
	}

	public void InitObjectivePanel()
	{
		//objective1Txt.text = GUI_Controller.instance.NotificationController.

	}

	public void InitDialogue(List<int> playerScores, List<int> playerBestScores, List<int> playerErrors, int targetScore)
	{
		playerCounter=1;
		playerNameTxt.text = "Player " + playerCounter;
		if(playerScoreTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,playerScores[playerCounter-1], playerScoreTxt, "Score: "));}
		if(playerErrorsTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,playerErrors[playerCounter-1], playerErrorsTxt, "Errors: "));}
		if(playerBestScoreTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,playerBestScores[playerCounter-1], playerBestScoreTxt, "Best turn score: "));}

		bool bestTurnScore = true;
		 foreach(int score in playerBestScores)
		{
			if(score> playerBestScores[0])
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

		if(GameMaster.instance.soloPlay)
		{
			if(targetStarController != null)
				targetStarController.ea = GameMaster.instance.starCount;

			playerNameTxt.text = "Target: " + GameMaster.instance.targetScore_3Star; 
			if(playerScoreTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,playerScores[0], playerScoreTxt, "Score: "));}
			if(playerErrorsTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,playerErrors[playerCounter-1], playerErrorsTxt, "Errors: "));}
			if(playerBestScoreTxt != null) { StartCoroutine(GUI_Controller.instance.UpdateUIScore(0,playerBestScores[playerCounter-1], playerBestScoreTxt, "Best turn score: "));}
			
		} else if (GameMaster.instance.vsAi)
		{
			if(targetStarController != null)
			{
				int starCount;
				if(bestScore)
				{
					starCount=1;
					if(bestTurnScore)
						starCount++;
					if(playerErrors[0]==0)
						starCount++;
				} else {
					starCount=0;
				}
				targetStarController.ea = starCount;
				GameMaster.instance.starCount = starCount;
			}
		}
		
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


}
