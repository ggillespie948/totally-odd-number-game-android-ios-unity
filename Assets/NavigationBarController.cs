using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UI.Extensions;

public class NavigationBarController : MonoBehaviour {

	[Header("Scroll Snap")]
	[SerializeField]
	public HorizontalScrollSnap ScrollSnap;

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
	public GameObject challengeModeDialogue;
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
	public GameObject unlockablesPannel;
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

	[SerializeField]
	private Color enabledCol;

	public void UpdateNavPos()
	{
		switch(ScrollSnap._currentPage)
		{
			case 0:
			SetButtonIconAnim(btn_1.GetComponent<Button>());
			break;

			case 1:
			SetButtonIconAnim(btn_2.GetComponent<Button>());
			break;

			case 2:
			//SetButtonIconAnim(btn_3.GetComponent<Button>());
			break;

		}
	}

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
			activeButtonIcon.transform.position = activeButtonIcon.transform.position+new Vector3(0,.2f,0);
			activeButtonIcon.gameObject.transform.parent.GetComponentInChildren<TextMeshProUGUI>().enabled=true;
			return;
		} else 
		{
			if(activeButtonIcon==btn.GetComponentInChildren<Animator>())
				return;
			ClearButtonIconAnim();
			activeButtonIcon=btn.GetComponentInChildren<Animator>();
			activeButtonIcon.enabled=true;
			activeButtonIcon.transform.position = activeButtonIcon.transform.position+new Vector3(0,.2f,0);
			activeButtonIcon.gameObject.transform.parent.GetComponentInChildren<TextMeshProUGUI>().enabled=true;
		}
	}

	public void ClearButtonIconAnim()
	{
		activeButtonIcon.enabled=false;
		activeButtonIcon.transform.position = activeButtonIcon.transform.position+new Vector3(0,-.2f,0);
		activeButtonIcon.gameObject.transform.parent.GetComponentInChildren<TextMeshProUGUI>().enabled=false;
	}

	

	/// <summary>
	/// Function which toggles the settings button menu option
	/// </summary>
	public void ToggleSettingsOption()
	{
		Debug.LogWarning("Weow");
		if(musicToggle.gameObject.activeSelf)
		{
			musicToggle.SetActive(false);
			soundToggle.SetActive(false);
		} else{
			Debug.LogWarning("nah wtf");
			musicToggle.SetActive(true);
			soundToggle.SetActive(true);
		}

	}

	public void ToggleLinksOption()
	{
		//ratingButton.SetActive(!ratingButton.gameObject.activeSelf);
		devButton.SetActive(!devButton.gameObject.activeInHierarchy);
	}

	public void PressHomeButton()
	{
		ScrollSnap.gameObject.SetActive(true);
		challengeModeDialogue.SetActive(true);
		practiceModeDialogue.SetActive(true);
		multiplayerDialogue.SetActive(true);
		tutorialModeDialogue.SetActive(true);
		unlockablesPannel.SetActive(true);
		CloseAllDialogues(false);
		ScrollSnap.PreviousScreen();
	}

	/// <summary>
	/// Method which closes all of the dialogues controlled by the main navigation bar.
	/// </summary>
	public void CloseAllDialogues(bool closeBase)
	{
		challengeModeDialogue.GetComponent<ChallengeModeController>().CloseAllDialogues();
		if(closeBase)
		{
			challengeModeDialogue.SetActive(false);
			practiceModeDialogue.SetActive(false);
			multiplayerDialogue.SetActive(false);
			tutorialModeDialogue.SetActive(false);
			unlockablesPannel.SetActive(false);
		}

		//unlockablesPannel.GetComponent<UnlockablesController>().CloseAllTabs();

		gameTitle.SetActive(false);
	}

	public void PressUnlockablesButton()
	{
		CloseAllDialogues(false);
		ScrollSnap.gameObject.SetActive(true);
		challengeModeDialogue.SetActive(true);
		practiceModeDialogue.SetActive(true);
		multiplayerDialogue.SetActive(true);
		tutorialModeDialogue.SetActive(true);
		unlockablesPannel.SetActive(true);
		ScrollSnap.NextScreen();
	}

	/// <summary>
	/// Method which sets the value of the notification object for the unlockables menu option
	/// </summary>
	public void SetNotificationHolderValue(int val)
	{
		unlockablesPannelNotifTxt.text = val.ToString();
	}
}
