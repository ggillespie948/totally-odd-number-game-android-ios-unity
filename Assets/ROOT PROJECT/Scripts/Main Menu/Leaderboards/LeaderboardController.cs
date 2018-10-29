using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class LeaderboardController : MonoBehaviour {

	public TextMeshProUGUI leaderboardTitle;
	public LeaderboardEntry[] leaderboardEntries;

	[SerializeField]
	private GameObject leaderboardScrollView;
	private int selectedLeaderboard=0;
	
	[SerializeField]
	private BoxCollider previousButton;

	[SerializeField]
	private BoxCollider nextButton;
	
	void Awake()
	{
		if(leaderboardEntries.Length ==0)
				leaderboardEntries=leaderboardScrollView.GetComponentsInChildren<LeaderboardEntry>();
	}

	private void LockButtons()
	{
		previousButton.enabled=false;
		nextButton.enabled=false;
	}

	private void UnlockButtons()
	{
		previousButton.enabled=true;
		nextButton.enabled=true;
	}

	public void NextLeaderboard()
	{
		if(selectedLeaderboard<2)
			selectedLeaderboard++;
		else
			selectedLeaderboard=0;

		ClearLeaderboard();
		RequestLeaderboard();	

	}

	public void PreviousLeaderboard()
	{
		if(selectedLeaderboard>0)
			selectedLeaderboard--;
		else
			selectedLeaderboard=2;

		ClearLeaderboard();
		RequestLeaderboard();

	}

	public void RequestLeaderboard()
	{
		LockButtons();

		switch(selectedLeaderboard)
		{
			case 2:
			AccountInfo.Instance.LoadLeaderboard("Wins");
			leaderboardTitle.text="Wins (All)";
			break;

			case 1:
			AccountInfo.Instance.LoadLeaderboard("TilesPlayed");
			leaderboardTitle.text="Tiles Played";
			break;

			case 0:
			AccountInfo.Instance.LoadLeaderboard("Stars");
			leaderboardTitle.text="Stars";
			break;
		}

	}

	public void ClearLeaderboard()
	{
		foreach(LeaderboardEntry entry in leaderboardEntries)
		{
			entry.UnhighlightPlayer();
			entry.nameTxt.text="Loading...";
			entry.valueTxt.text="";
			entry.rankTxt.text="";
		}
	}

	public void LoadLeaderboardResponse(GetLeaderboardAroundPlayerResult result)
	{
		int indx=0;
		foreach(PlayerLeaderboardEntry entry in result.Leaderboard)
		{
			if(entry.PlayFabId == AccountInfo.playfabId)
			{
				leaderboardEntries[indx].HighlightPlayer();
			} else{
				leaderboardEntries[indx].UnhighlightPlayer();
			}
			leaderboardEntries[indx].rankTxt.text=(entry.Position+1).ToString(); //entry.Position.ToString();
			if(entry.DisplayName != null)
			{
				leaderboardEntries[indx].nameTxt.text=entry.DisplayName.ToString();
			}
			leaderboardEntries[indx].valueTxt.text=entry.StatValue.ToString();
			indx++;
		}

		foreach(LeaderboardEntry entry in leaderboardEntries)
		{
			if(entry.nameTxt.text == "-" || entry.nameTxt.text == "Loading...")
			{
				entry.gameObject.SetActive(false);
			} else { 
				entry.gameObject.SetActive(true);
			}
		}

		UnlockButtons();


	}

}
