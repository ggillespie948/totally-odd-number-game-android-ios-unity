using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NavigationBarController : MonoBehaviour {

	[Header("Navigation Buttons")]
	[SerializeField]
	private Button btn_1; 
	[SerializeField]
	private Button btn_2;
	[SerializeField]
	private Button btn_3;
	[SerializeField]
	private Button btn_4;

	[Header("Home Dialogues")]
	[SerializeField]
	private GameObject challengeModeDialogue;
	[SerializeField]
	private GameObject practiceModeDialogue;
	[SerializeField]
	private GameObject tutorialModeDialogue;
	[SerializeField]
	private GameObject multiplayerDialogue;

	[SerializeField]
	private GameObject gameTitle;

	[Header("Unlockables Dialogue")]
	[SerializeField]
	private GameObject unlockablesPannel;
	[SerializeField]
	private TextMeshProUGUI unlockablesPannelNotifTxt;


	[Header("Settings Buttons")]
	[SerializeField]
	private GameObject musicToggle;
	[SerializeField]
	private GameObject soundToggle;

	[Header("Link Buttons")]
	[SerializeField]
	private GameObject ratingButton;
	[SerializeField]
	private GameObject devButton;

	private Animator activeButtonIcon;

	/// <summary>
	/// Function used to change the the animated button of an icon when pressed
	/// </summary>
	/// <param name="btn"></param>
	public void SetButtonIconAnim(Button btn)
	{
		if(activeButtonIcon==null)
		{
			activeButtonIcon=btn.GetComponentInChildren<Animator>();
			activeButtonIcon.enabled=true;
			return;
		} else 
		{
			activeButtonIcon.enabled=false;
			activeButtonIcon=btn.GetComponentInChildren<Animator>();
			activeButtonIcon.enabled=true;
		}
	}

	/// <summary>
	/// Function which toggles the settings button menu option
	/// </summary>
	public void ToggleSettingsOption()
	{
		if(musicToggle.activeSelf)
		{
			musicToggle.SetActive(true);
			soundToggle.SetActive(true);
		} else{
			musicToggle.SetActive(false);
			soundToggle.SetActive(false);
		}

	}

	public void ToggleLinksOption()
	{
		//ratingButton.SetActive(!ratingButton.gameObject.activeSelf);
		devButton.SetActive(!devButton.gameObject.activeSelf);
	}

	public void PressHomeButton()
	{
		CloseAllDialogues();

		challengeModeDialogue.SetActive(true);
		practiceModeDialogue.SetActive(true);
		multiplayerDialogue.SetActive(true);
		tutorialModeDialogue.SetActive(true);
	}

	/// <summary>
	/// Method which closes all of the dialogues controlled by the main navigation bar.
	/// </summary>
	public void CloseAllDialogues()
	{
		challengeModeDialogue.SetActive(false);
		practiceModeDialogue.SetActive(false);
		multiplayerDialogue.SetActive(false);
		tutorialModeDialogue.SetActive(false);
		unlockablesPannel.SetActive(false);
		gameTitle.SetActive(false);
	}

	public void PressUnlockablesButton()
	{
		CloseAllDialogues();
		unlockablesPannel.SetActive(true);
	}

	/// <summary>
	/// Method which sets the value of the notification object for the unlockables menu option
	/// </summary>
	public void SetNotificationHolderValue(int val)
	{
		unlockablesPannelNotifTxt.text = val.ToString();
	}
}
