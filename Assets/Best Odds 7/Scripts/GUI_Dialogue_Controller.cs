using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Dialogue_Controller : MonoBehaviour {

	// Use this for initialization
	public GUI_Dialogue_Call ActiveDialogue;
	public GUI_Dialogue_Call YouWinDialogue;
    public GUI_Dialogue_Call YouLoseDialogue;
    public GUI_Dialogue_Call LevelClearDialogue;

    public GUI_Dialogue_Call LevelLostDialogue;

	public GUI_Dialogue_Call LocalMpDialogue;

    //public GUI_Dialogue_Call PauseGameDialogue;
	
	public void LoadDialogue(string dialogue, List<int> playerScores, List<int> playerBestScores, List<int> playerErrors, int targetScore)
	{
		switch(dialogue)
		{
			case "Win":
				YouWinDialogue.Open();
				ActiveDialogue = YouWinDialogue;
				YouWinDialogue.InitDialogue(playerScores, playerBestScores, playerErrors, targetScore);
			break;

			case "Lose":
				YouLoseDialogue.Open();
				ActiveDialogue = YouLoseDialogue;
				YouLoseDialogue.InitDialogue(playerScores, playerBestScores, playerErrors, targetScore);
			break;

			// case "SoloWin":
			// 	LevelClearDialogue.Open();
			// 	LevelClearDialogue.InitDialogue(player1score, player2score, avgturntime, targetScore);
			// break;

			// case "SoloLose":
			// 	LevelLostDialogue.Open();
			// 	LevelLostDialogue.InitDialogue(player1score, player2score, avgturntime, targetScore);
			// break;

			// case "LocalMP":
			// 	LocalMpDialogue.Open();
			// 	LocalMpDialogue.InitDialogue(player1score, player2score, avgturntime, targetScore);
			// break;

		}
	}
}
