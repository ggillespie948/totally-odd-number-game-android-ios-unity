using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChallengeModeController : MonoBehaviour {

	[Header("Challenege Mode Dialogues")]
	[SerializeField]
	private NavigationBarController navBar;

	[Header("Challenege Mode Dialogues")]
	[SerializeField]
	private GameObject worldSelector;
	[SerializeField]
	private GameObject levelSelector;
	[SerializeField]
	private List<GameObject> worldScrollViews;
	[SerializeField]
	private List<GameObject> worldScrollViewPanels;

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

	public void OpenChallenegeMode()
	{
		worldSelector.SetActive(true);
	}

	public void SelectWorld(int worldNo)
	{
		CloseAllDialogues();
		navBar.CloseAllDialogues();

		worldScrollViews[worldNo].SetActive(true);
		worldScrollViewPanels[worldNo].SetActive(true);
	}
}
