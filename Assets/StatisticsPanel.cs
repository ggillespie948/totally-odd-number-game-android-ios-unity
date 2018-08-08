using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatisticsPanel : MonoBehaviour {

	public TextMeshProUGUI winTxt;
	public TextMeshProUGUI loseTxt;
	public TextMeshProUGUI tilesPlayedTxt;
	public TextMeshProUGUI bestTurnScoreTxt;
	public TextMeshProUGUI bestScoreTxt;

	public GameObject winBar;
	public GameObject lossBar;

	public void Close()
	{
		MenuController.instance.NavBar.challengeModeDialogue.GetComponent<ChallengeModeController>().selectionFX.Play();
		Invoke("CloseDelay", .05f);
	}

	public void CloseDelay()
	{
		this.gameObject.SetActive(false);
	}

	public void Open()
	{
		MenuController.instance.NavBar.challengeModeDialogue.GetComponent<ChallengeModeController>().selectionFX.Play();
		this.gameObject.SetActive(true);
	}

	public void AdjustWinLossRatio()
	{
	 

	}




	
}
