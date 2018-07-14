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

	[SerializeField]
	public int active_btn = 3;
	[Header("Home Dialogues")]
	[SerializeField]
	public GameObject challengeModeDialogue;
	[SerializeField]
	private GameObject practiceModeDialogue;
	[SerializeField]
	private GameObject tutorialModeDialogue;
	[SerializeField]
	private GameObject multiplayerDialogue;

	[Header("Player Panel")]
	[SerializeField]
	public GameObject playerPanel;

	[Header("Unlock Panel")]
	[SerializeField]
	public GameObject unlockPanel;

	[Header("Shop Panel")]
	[SerializeField]
	public GameObject shopPanel;

	[Header("Extra Panel")]
	[SerializeField]
	public GameObject extraPanel;

	
	[Header("Confirm Purchase Panel")]
	
	[SerializeField]
	private GameObject confirmPurchasePanel;
	[SerializeField]
	public GameObject tileSkinParent;
	public TextMeshProUGUI shopItemTitle;
	public TextMeshProUGUI priceText;
	[Header("Currently Inspected Item")]
	[SerializeField]
	public GameObject activeTileBox;

	[Header("Inspected Item Preview Items")]
	[SerializeField]
	public GameObject[] previewItems;
	

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

	private Button activeButtonIcon;

	[SerializeField]
	private Color enabledCol;

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	

	public void UpdateNavPos()
	{
		switch(ScrollSnap._currentPage)
		{
			case 0:
			PressExtraButton();
			break;

			case 1:
			PressShopButton();
			break;

			case 3:
			PressPlayButton();
			break;

			case 4:
			PressUnlockablesButton();
			break;

		}
	}

	public void UninspectTileBox()
	{
		activeTileBox.GetComponent<TileBox>().UninspectItem();
	}

	/// <summary>
	/// Function used to change the the animated button of an icon when pressed
	/// </summary>
	/// <param name="btn"></param>
	public void SetButtonIconAnim(Button btn)
	{
		Debug.Log("Set Button Icon");
		if(activeButtonIcon==null)
		{
			activeButtonIcon=btn;
			btn.GetComponentInChildren<SpriteRenderer>().transform.position+=new Vector3(0,.2f,0);
			return;
		} else 
		{
			if(activeButtonIcon==btn)
				return;
			activeButtonIcon=btn;
			btn.GetComponentInChildren<SpriteRenderer>().transform.position+=new Vector3(0,.2f,0);
			ClearButtonIconAnim();
			
		}
	}

	public void ClearButtonIconAnim()
	{
		activeButtonIcon.GetComponentInChildren<SpriteRenderer>().transform.position-=new Vector3(0,.2f,0);
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

	public void ToggleBaseMenu()
	{
		ScrollSnap.gameObject.SetActive(true);
		//challengeModeDialogue.SetActive(true);
		//practiceModeDialogue.SetActive(true);
		//multiplayerDialogue.SetActive(true);
		//tutorialModeDialogue.SetActive(true);
		unlockablesPannel.SetActive(true);
		playerPanel.SetActive(true);
		unlockPanel.SetActive(true);
		shopPanel.SetActive(true);
		extraPanel.SetActive(true);
	}

	public void PressExtraButton()
	{
		UnselectActiveButton();
		btn_1.transform.position+=new Vector3(0,.1f,0);
		active_btn=1;
		btn_1.GetComponentInChildren<TextMeshProUGUI>().enabled=true;
		CloseAllDialogues(false);
		ScrollSnap.GoToScreen(0);
	}

	public void PressPlayButton()
	{
		UnselectActiveButton();
		btn_3.transform.position+=new Vector3(0,.1f,0);
		active_btn=3;
		btn_3.GetComponentInChildren<TextMeshProUGUI>().enabled=true;
		ToggleBaseMenu();
		CloseAllDialogues(false);
		ScrollSnap.GoToScreen(2);
	}

	public void PressShopButton()
	{
		UnselectActiveButton();
		btn_2.transform.position+=new Vector3(0,.1f,0);
		active_btn=2;
		btn_2.GetComponentInChildren<TextMeshProUGUI>().enabled=true;
		ToggleBaseMenu();
		CloseAllDialogues(false);
		ScrollSnap.GoToScreen(1);
	}

	

	public void UnselectActiveButton()
	{
		switch(active_btn)
		{
			case 1:
				btn_1.transform.position-=new Vector3(0,.1f,0);
				btn_1.GetComponentInChildren<TextMeshProUGUI>().enabled=false;
			break;

			case 2:
				btn_2.transform.position-=new Vector3(0,.1f,0);
				btn_2.GetComponentInChildren<TextMeshProUGUI>().enabled=false;
			break;

			case 3:
				btn_3.transform.position-=new Vector3(0,.1f,0);
				btn_3.GetComponentInChildren<TextMeshProUGUI>().enabled=false;
			break;

			case 4:
				btn_4.transform.position-=new Vector3(0,.1f,0);
				btn_4.GetComponentInChildren<TextMeshProUGUI>().enabled=false;
			break;
		}


	}

	/// <summary>
	/// Method which closes all of the dialogues controlled by the main navigation bar.
	/// </summary>
	public void CloseAllDialogues(bool closeBase)
	{
		challengeModeDialogue.GetComponent<ChallengeModeController>().CloseAllDialogues();
		if(closeBase)
		{
			//challengeModeDialogue.SetActive(false);
			practiceModeDialogue.SetActive(false);
			multiplayerDialogue.SetActive(false);
			tutorialModeDialogue.SetActive(false);
			unlockablesPannel.SetActive(false);
			playerPanel.SetActive(false);
			unlockPanel.SetActive(false);
			shopPanel.SetActive(false);
			extraPanel.SetActive(false);
		}

	}

	public void PressUnlockablesButton()
	{
		UnselectActiveButton();
		btn_4.transform.position+=new Vector3(0,.1f,0);
		active_btn=4;
		btn_4.GetComponentInChildren<TextMeshProUGUI>().enabled=true;
		CloseAllDialogues(false);
		ToggleBaseMenu();
		ScrollSnap.GoToScreen(3);
	}

	/// <summary>
	/// Method which sets the value of the notification object for the unlockables menu option
	/// </summary>
	public void SetNotificationHolderValue(int val)
	{
		unlockablesPannelNotifTxt.text = val.ToString();
	}
}
