using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChallengeModeController : MonoBehaviour {

	[Header("Challenege Mode Dialogues")]
	[SerializeField]
	private NavigationBarController navBar;

	[SerializeField]
	public ParticleSystem selectionFX;

	[Header("Challenege Mode Dialogues")]
	[SerializeField]
	private GameObject worldSelector;
	[SerializeField]
	private GameObject levelSelector;
	[SerializeField]
	public List<GameObject> worldScrollViews;
	[SerializeField]
	private List<GameObject> worldScrollViewPanels;
	[SerializeField]
	private List<GameObject> worldBoxes;

	public void CloseAllDialogues()
	{
		foreach(GameObject g in worldScrollViews)
		{
			g.SetActive(false);
		}

		foreach(GameObject g in worldScrollViewPanels)
		{
			g.SetActive(false);
		}

		worldSelector.SetActive(false);
		levelSelector.SetActive(false);
	}

	public void CheckWorldForStarImprovement()
	{
		Debug.Log("Checking for star improvement.. ");
		foreach(GameObject g in worldScrollViews)
		{
			Component[] configs =g.GetComponentsInChildren<Game_Configuration>();
			foreach(Game_Configuration config in configs)
			{
				config.CheckStarReq();
			}
		}
	}

	public void OpenChallenegeMode()
	{
		CloseAllDialogues();
		selectionFX.Play();
		worldSelector.SetActive(true);
		LoadWorldData();
		Invoke("HidePlayerPanel", .05f);
	}

	private void HidePlayerPanel()
	{
		navBar.playerPanel.SetActive(false);
	}

	public void SelectWorld(int worldNo)
	{
		CloseAllDialogues();
		navBar.CloseAllDialogues(true);

		worldScrollViews[worldNo].SetActive(true);
		worldScrollViewPanels[worldNo].SetActive(true);
	}

	public void LoadWorldData()
	{
		worldBoxes[0].GetComponentInChildren<TextMeshProUGUI>().text="Beginner " + AccountInfo.beginnerStars.ToString() + "/30";
		worldBoxes[1].GetComponentInChildren<TextMeshProUGUI>().text="Novice " + AccountInfo.noviceStars.ToString() + "/30";
		worldBoxes[2].GetComponentInChildren<TextMeshProUGUI>().text="Intermediate " + AccountInfo.intermediateStars.ToString() + "/30";
		worldBoxes[3].GetComponentInChildren<TextMeshProUGUI>().text="Advanced " + AccountInfo.advancedStars.ToString() + "/30";
		worldBoxes[4].GetComponentInChildren<TextMeshProUGUI>().text="Master " + AccountInfo.masterStars.ToString() + "/30";
		worldBoxes[5].GetComponentInChildren<TextMeshProUGUI>().text="Grand Master " + AccountInfo.grandMasterStars.ToString() + "/30";
	}
}
