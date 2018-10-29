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

	[Header("Resolution Default GameObjects")]
	[SerializeField]
	private RectTransform levelsScrollViewOffsetContainer;

	[Header("Navigation Buttons")]
	[SerializeField]
	public Button btn_1;  //1 + 2 public for tutorial controller access noly
	[SerializeField]
	public Button btn_2;
	[SerializeField]
	public Button btn_3; //public for menu controller return to world code
	[SerializeField]
	private Button btn_4;
	[SerializeField]
	private Button btn_5;

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

	[SerializeField]
	private StatisticsPanel statisticsPanel;

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

	[Header("Referal Panel")]
	[SerializeField]
	public GameObject referalPanel;

	
	[Header("Confirm Purchase Panel")]
	[SerializeField]
	private GameObject confirmPurchasePanel;
	[SerializeField]
	public GameObject inspectedItemParent; //a reference of each inspected is required as the parent of the inspected itme is temporarily set ot the confirm purchase panel
	[SerializeField]
	public GameObject tileSkinParent;
	public TextMeshProUGUI shopItemTitle;
	public TextMeshProUGUI shopItemDescription;
	public TextMeshProUGUI priceText;
	public Image coinIcon;
	[Header("Currently Inspected Item")]
	[SerializeField]
	public GameObject activeGridBox;
	public GameObject activeTileBox;
	public GameObject activeShopItem;

	[Header("Inspected Item Preview Items")]
	[SerializeField]
	public GameObject[] previewItems;
	

	[SerializeField]
	private GameObject gameTitle;

	[Header("Unlockables Dialogue")]
	[SerializeField]
	public GameObject unlockablesPanel;
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

	[SerializeField]
	private RectTransform rect;
	[SerializeField]
	private RectTransform indicator;
	[SerializeField]
	public HorizontalLayoutGroup layout;

	[SerializeField]
	public GameObject[] panelSlidingTitles;
	public TextMeshProUGUI currentPanelTxt;

	public GameObject dailyChallengePanel;
	public GameObject playGameSection;


	public void LockNavBar()
	{
		btn_1.interactable=false;
		btn_2.interactable=false;
		btn_3.interactable=false;
		btn_4.interactable=false;
		btn_5.interactable=false;
	}

	public void UnlockNavBar()
	{
		btn_1.interactable=true;
		btn_2.interactable=true;
		btn_3.interactable=true;
		btn_4.interactable=true;
		btn_5.interactable=true;
	}

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		SetResolutionDefault();
	}

	private void SetResolutionDefault()
	{

		Debug.LogError("aspect: " + Camera.main.aspect);
		float aspect = Camera.main.aspect;

		if(aspect < 0.47f)
		{
			Debug.Log("Set Xtra Slim Resolution Default"); //iphone X
				float left = -175.5f;
				float right = -176.8f;

				rect.anchorMin = Vector3.zero;
				rect.anchorMax = Vector3.one;
				rect.anchoredPosition = new Vector2((left - right)/2, 0f);
				rect.sizeDelta = new Vector2(-(left + right), 0);
				rect.localPosition = new Vector2(rect.localPosition.x, -433);

				indicator.anchorMin = Vector3.zero;
				indicator.anchorMax = Vector3.one;
				indicator.anchoredPosition = new Vector2((left - right)/2, 0f);
				indicator.sizeDelta = new Vector2(-(left + right), 0);
				indicator.localPosition = new Vector2(rect.localPosition.x, -433);

				levelsScrollViewOffsetContainer.transform.localPosition=new Vector2(0,0);

				playGameSection.transform.localPosition-=new Vector3(0,-10,0);
				layout.spacing=4.5f;

		} else if (aspect < 0.51f)
		{
			 Debug.Log("Set  Slim Resolution Default"); //s8 s9
				float left = -199f;
				float right = -195f;

				rect.anchorMin = Vector3.zero;
				rect.anchorMax = Vector3.one;
				rect.anchoredPosition = new Vector2((left - right)/2, 0f);
				rect.sizeDelta = new Vector2(-(left + right), 0);
				rect.localPosition = new Vector2(rect.localPosition.x, -433);

				indicator.anchorMin = Vector3.zero;
				indicator.anchorMax = Vector3.one;
				indicator.anchoredPosition = new Vector2((left - right)/2, 0f);
				indicator.sizeDelta = new Vector2(-(left + right), 0);
				indicator.localPosition = new Vector2(rect.localPosition.x, -433);

				layout.spacing=0f;
				layout.padding.left=6;

		} else if (aspect < 0.65f)
		{
			playGameSection.transform.localPosition-=new Vector3(0,-10,0);
			levelsScrollViewOffsetContainer.transform.localPosition=new Vector2(-20,0);

		} else if (aspect > 0.7f)
		{
			float left = -278.2f;
				float right = -280.2f;

				rect.anchorMin = Vector3.zero;
				rect.anchorMax = Vector3.one;
				rect.anchoredPosition = new Vector2((left - right)/2, 0f);
				rect.sizeDelta = new Vector2(-(left + right), 0);
				rect.localPosition = new Vector2(rect.localPosition.x, -353);

				indicator.anchorMin = Vector3.zero;
				indicator.anchorMax = Vector3.one;
				indicator.anchoredPosition = new Vector2((left - right)/2, 0f);
				indicator.sizeDelta = new Vector2(-(left + right), 0);
				indicator.localPosition = new Vector2(rect.localPosition.x, -353);

				//GetComponent<Animator>().SetTrigger("default");
				playGameSection.transform.localPosition-=new Vector3(0,-20,0);
				dailyChallengePanel.transform.localPosition-=new Vector3(0,-30f,0);

				levelsScrollViewOffsetContainer.transform.localPosition=new Vector2(0,25);

				foreach(GameObject go in panelSlidingTitles)
				{
					go.SetActive(false);
				}

				currentPanelTxt.enabled=true;

				layout.spacing=-10;


		} else 
		{
			playGameSection.transform.localPosition-=new Vector3(0,-10,0);
			levelsScrollViewOffsetContainer.transform.localPosition=new Vector2(-20,0);

		}


		// if(Screen.height > 2400)
		// {
        //     if(Screen.width < 1130)
        //     {
        //        // Debug.Log("Set Xtra Slim Resolution Default"); //iphone X
		// 		float left = -175.5f;
		// 		float right = -176.8f;

		// 		rect.anchorMin = Vector3.zero;
		// 		rect.anchorMax = Vector3.one;
		// 		rect.anchoredPosition = new Vector2((left - right)/2, 0f);
		// 		rect.sizeDelta = new Vector2(-(left + right), 0);
		// 		rect.localPosition = new Vector2(rect.localPosition.x, -433);

		// 		indicator.anchorMin = Vector3.zero;
		// 		indicator.anchorMax = Vector3.one;
		// 		indicator.anchoredPosition = new Vector2((left - right)/2, 0f);
		// 		indicator.sizeDelta = new Vector2(-(left + right), 0);
		// 		indicator.localPosition = new Vector2(rect.localPosition.x, -433);

		// 		levelsScrollViewOffsetContainer.transform.localPosition=new Vector2(0,0);

		// 		playGameSection.transform.localPosition-=new Vector3(0,-10,0);
		// 		layout.spacing=4.5f;
                

        //     } else if(Screen.height > 2600)
        //     {
        //        // Debug.Log("Set  Slim Resolution Default"); //s8 s9
		// 		float left = -199f;
		// 		float right = -195f;

		// 		rect.anchorMin = Vector3.zero;
		// 		rect.anchorMax = Vector3.one;
		// 		rect.anchoredPosition = new Vector2((left - right)/2, 0f);
		// 		rect.sizeDelta = new Vector2(-(left + right), 0);
		// 		rect.localPosition = new Vector2(rect.localPosition.x, -433);

		// 		indicator.anchorMin = Vector3.zero;
		// 		indicator.anchorMax = Vector3.one;
		// 		indicator.anchoredPosition = new Vector2((left - right)/2, 0f);
		// 		indicator.sizeDelta = new Vector2(-(left + right), 0);
		// 		indicator.localPosition = new Vector2(rect.localPosition.x, -433);

		// 		// indicator.anchorMin = Vector3.zero;
		// 		// indicator.anchorMax = Vector3.one;
		// 		// indicator.anchoredPosition = new Vector2((left - right)/2, 0f);
		// 		// indicator.sizeDelta = new Vector2(-(left + right), 0);
		// 		// indicator.localPosition = new Vector2(rect.localPosition.x, -377);

		// 		levelsScrollViewOffsetContainer.transform.localPosition=new Vector2(-10,0);

		// 		layout.spacing=-18;
				
        //     } else 
        //     {
        //         //Debug.Log("Set Default Resolution Default"); //s6 s7
		// 		playGameSection.transform.localPosition-=new Vector3(0,-10,0);
		// 		levelsScrollViewOffsetContainer.transform.localPosition=new Vector2(-20,0);
			    
        //     }

		// } else if(Screen.width > 1500)
		// {
		// 	//Debug.Log("Set Wide Resolution Default");
		// 	float left = -278.2f;
		// 		float right = -280.2f;

		// 		rect.anchorMin = Vector3.zero;
		// 		rect.anchorMax = Vector3.one;
		// 		rect.anchoredPosition = new Vector2((left - right)/2, 0f);
		// 		rect.sizeDelta = new Vector2(-(left + right), 0);
		// 		rect.localPosition = new Vector2(rect.localPosition.x, -353);

		// 		indicator.anchorMin = Vector3.zero;
		// 		indicator.anchorMax = Vector3.one;
		// 		indicator.anchoredPosition = new Vector2((left - right)/2, 0f);
		// 		indicator.sizeDelta = new Vector2(-(left + right), 0);
		// 		indicator.localPosition = new Vector2(rect.localPosition.x, -353);

		// 		//GetComponent<Animator>().SetTrigger("default");
		// 		playGameSection.transform.localPosition-=new Vector3(0,-20,0);
		// 		dailyChallengePanel.transform.localPosition-=new Vector3(0,-30f,0);

		// 		levelsScrollViewOffsetContainer.transform.localPosition=new Vector2(0,25);

		// 		foreach(GameObject go in panelSlidingTitles)
		// 		{
		// 			go.SetActive(false);
		// 		}

		// 		currentPanelTxt.enabled=true;

		// 		layout.spacing=-10;

		// } else 
		// {
		// 	//Debug.Log("Set Default Resolution Default");
		// 	playGameSection.transform.localPosition-=new Vector3(0,-10,0);
		// 	levelsScrollViewOffsetContainer.transform.localPosition=new Vector2(-20,0);

		// }

	}
	
	public void UpdateNavPos()
	{
		if(!ScrollSnap._lerp)
				ScrollSnap.ScrollToClosestElement();

		//layout.enabled=false;
				
		switch(ScrollSnap._currentPage)
		{
			case 0:
			PressShopButton();
			
			break;

			case 1:
			PressUnlockablesButton();
			break;

			case 2:
			PressPlayButton();
			
			break;

			case 3:
			PressSocialButton();
			break;

			case 4:
			PressReferalButton();
			break;

		}
	}

	public void PressReferalButton()
	{
		UnselectActiveButton();
		btn_5.transform.position+=new Vector3(0,.1f,0);
		StartCoroutine(btn_5.GetComponent<GUI_Object>().ScaleUpFromBase(1.25f,1f));
		active_btn=5;
		MenuController.instance.SetInfoPanelText("Refer", false);
		btn_5.GetComponentInChildren<TextMeshProUGUI>().enabled=true;
		CloseAllDialogues(false);
		ToggleBaseMenu();

		if(ScrollSnap._currentPage!=4)
		ScrollSnap.GoToScreen(4);

	}

	public void UninspectActiveItem()
	{
		if(activeTileBox!=null)
			activeTileBox.GetComponent<TileBox>().UninspectItem();
		else if(activeShopItem!=null)
			activeShopItem.GetComponent<ShopItem>().UninspectItem();
		else if (activeGridBox!=null)
			activeGridBox.GetComponent<GridBox>().UninspectItem();
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
		referalPanel.SetActive(true);
		unlockablesPanel.SetActive(true);
		playerPanel.SetActive(true);
		MenuController.instance.NavBar.challengeModeDialogue.GetComponent<Animator>().SetTrigger("show");
		unlockPanel.SetActive(true);
		shopPanel.SetActive(true);
		extraPanel.SetActive(true);
	}

	
	public bool goodrayTutFlag=false;
	public void PressShopButton()
	{
		if(ApplicationModel.TUTORIAL_MODE && ApplicationModel.RETURN_TO_WORLD==-3 && goodrayTutFlag)
		{
			goodrayTutFlag=false;
			btn_1.transform.SetParent(btn_2.transform.parent);
			Tutorial_Controller.instance.shopGoodRay.SetActive(false);
			Tutorial_Controller.instance.OnMouseDown();
		}
		MenuController.instance.SetInfoPanelText("Shop", false);
		UnselectActiveButton();
		btn_1.transform.position+=new Vector3(0,.1f,0);
		StartCoroutine(btn_1.GetComponent<GUI_Object>().ScaleUpFromBase(1.25f,1f));
		active_btn=1;
		btn_1.GetComponentInChildren<TextMeshProUGUI>().enabled=true;
		ToggleBaseMenu();
		CloseAllDialogues(false);

		if(ScrollSnap._currentPage!=0)
			ScrollSnap.GoToScreen(0);
	}

	public void PressPlayButton()
	{
		MenuController.instance.SetInfoPanelText("Play", false);
		UnselectActiveButton();
		btn_3.transform.position+=new Vector3(0,.1f,0);
		StartCoroutine(btn_3.GetComponent<GUI_Object>().ScaleUpFromBase(1.25f,1f));
		active_btn=3;
		btn_3.GetComponentInChildren<TextMeshProUGUI>().enabled=true;
		ToggleBaseMenu();
		CloseAllDialogues(false);
		if(ScrollSnap._currentPage!=2)
			ScrollSnap.GoToScreen(2);
	}

	public void PressSocialButton()
	{
		MenuController.instance.SetInfoPanelText("Social", false);
		UnselectActiveButton();
		btn_4.transform.position+=new Vector3(0,.1f,0);
		StartCoroutine(btn_4.GetComponent<GUI_Object>().ScaleUpFromBase(1.25f,1f));
		active_btn=4;
		btn_4.GetComponentInChildren<TextMeshProUGUI>().enabled=true;
		ToggleBaseMenu();
		CloseAllDialogues(false);

		if(ScrollSnap._currentPage!=3)
		ScrollSnap.GoToScreen(3);
	}

	public void PressUnlockablesButton()
	{
		MenuController.instance.SetInfoPanelText("Unlock", false);
		UnselectActiveButton();
		btn_2.transform.position+=new Vector3(0,.1f,0);
		StartCoroutine(btn_2.GetComponent<GUI_Object>().ScaleUpFromBase(1.25f,1f));
		active_btn=2;
		btn_2.GetComponentInChildren<TextMeshProUGUI>().enabled=true;
		CloseAllDialogues(false);
		ToggleBaseMenu();

		if(ScrollSnap._currentPage!=1)
			ScrollSnap.GoToScreen(1);
	}
	

	public void UnselectActiveButton()
	{
		switch(active_btn)
		{
			case 1:
				btn_1.transform.position-=new Vector3(0,.1f,0);
				btn_1.GetComponentInChildren<TextMeshProUGUI>().enabled=false;
				StartCoroutine(btn_1.GetComponent<GUI_Object>().ScaleDownFromBase(new Vector3(1.25f,1.25f,1.25f), new Vector3(1,1,1)));
			break;

			case 2:
				btn_2.transform.position-=new Vector3(0,.1f,0);
				btn_2.GetComponentInChildren<TextMeshProUGUI>().enabled=false;
				StartCoroutine(btn_2.GetComponent<GUI_Object>().ScaleDownFromBase(new Vector3(1.25f,1.25f,1.25f), new Vector3(1,1,1)));
			break;

			case 3:
				btn_3.transform.position-=new Vector3(0,.1f,0);
				btn_3.GetComponentInChildren<TextMeshProUGUI>().enabled=false;
				StartCoroutine(btn_3.GetComponent<GUI_Object>().ScaleDownFromBase(new Vector3(1.25f,1.25f,1.25f), new Vector3(1,1,1)));
			break;

			case 4:
				btn_4.transform.position-=new Vector3(0,.1f,0);
				btn_4.GetComponentInChildren<TextMeshProUGUI>().enabled=false;
				StartCoroutine(btn_4.GetComponent<GUI_Object>().ScaleDownFromBase(new Vector3(1.25f,1.25f,1.25f), new Vector3(1,1,1)));
			break;

			case 5:
				btn_5.transform.position-=new Vector3(0,.1f,0);
				btn_5.GetComponentInChildren<TextMeshProUGUI>().enabled=false;
				StartCoroutine(btn_5.GetComponent<GUI_Object>().ScaleDownFromBase(new Vector3(1.25f,1.25f,1.25f), new Vector3(1,1,1)));
			break;
		}


	}

	/// <summary>
	/// Method which closes all of the dialogues controlled by the main navigation bar.
	/// </summary>
	public void CloseAllDialogues(bool closeBase)
	{
		challengeModeDialogue.GetComponent<ChallengeModeController>().CloseAllDialogues();
		MenuController.instance.multiplayerPanel.SetActive(false);
		MenuController.instance.customGameDialog.SetActive(false);
		if(closeBase)
		{
			//challengeModeDialogue.SetActive(false);
			practiceModeDialogue.SetActive(false);
			multiplayerDialogue.SetActive(false);
			tutorialModeDialogue.SetActive(false);
			unlockablesPanel.SetActive(false);
			playerPanel.SetActive(false);
			unlockPanel.SetActive(false);
			shopPanel.SetActive(false);
			extraPanel.SetActive(false);
			referalPanel.SetActive(false);
		}

	}


	/// <summary>
	/// Method which sets the value of the notification object for the unlockables menu option
	/// </summary>
	public void SetNotificationHolderValue(int val)
	{
		unlockablesPannelNotifTxt.text = val.ToString();
	}
}
