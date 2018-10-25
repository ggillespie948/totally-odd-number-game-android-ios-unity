using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using PlayFab;
using UnityEngine.UI.Extensions;

public class MenuController : MonoBehaviour {

	public static MenuController instance = null;

	[SerializeField]
	public Game_Configuration activeGameConfig;

    [SerializeField]
    private LevelChanger levelChanger;
	
    [Header("Player Account")]
    [SerializeField]
    private AccountInfo Account;
    public NavigationBarController NavBar;

	public GameObject textInfoBar; //top information bar which displays scrolling (and stationary) text;

	
	[Header("In-Game Store Controllers")]
	[SerializeField]
	public UnlockablesController UnlockablesController;
	[SerializeField]
	public InAppPurchasesController InAppPurchasesController;

	
	[Header("Daily Challenge")]
	[SerializeField]
	private TextMeshProUGUI dailyChallengeTitleText;
	[SerializeField]
	private TextMeshProUGUI dailyChallengeValueText;
	[SerializeField]
	//private Progress
	private Slider progressBar;

	public void SetInfoPanelText(string text, bool scrolling)
	{
		if(!scrolling)
		{
			textInfoBar.GetComponentInChildren<TextMeshProUGUI>().text=text;
			return;
		} else 
		{

		}

	}

	public void LifePurchaseComplete()
	{
		GUI_Controller.instance.LivesDialogue.GetComponentInChildren<TextMeshProUGUI>().text = "Unlimited";
		GUI_Controller.instance.CurrencyUI.playerLives=15;
		GUI_Controller.instance.LivesDialogue.GetComponent<Image>().color=Color.red;
		GUI_Controller.instance.LivesDialogue.GetComponent<NoGravity>().enabled=true;
		NavBar.UninspectActiveItem();
	}

	public void CoinPurchaseComplete()
	{
		switch(MenuController.instance.NavBar.activeShopItem.GetComponent<ShopItem>().itemData.Cost)
		{
			case 99:
			CurrencyUI.AddCurrency(2500);
			break;

			case 199:
			CurrencyUI.AddCurrency(10000);
			break;

			case 499:
			CurrencyUI.AddCurrency(25000);
			break;
		}


		NavBar.UninspectActiveItem();
		
	}

	public void LevelPurchaseComplete()
	{
		GUI_Controller.instance.LivesDialogue.GetComponentInChildren<TextMeshProUGUI>().text = "Unlimited";
		GUI_Controller.instance.CurrencyUI.playerLives=15;
		GUI_Controller.instance.LivesDialogue.GetComponent<Image>().color=Color.red;
		GUI_Controller.instance.LivesDialogue.GetComponent<NoGravity>().enabled=true;
		CurrencyUI.AddCurrency(2500);
		NavBar.UninspectActiveItem();
	}


    [Header("Other Panels")]
	public GameObject fireWorks;
    public GameObject mainMenuContainer;

    public GameObject loadingScreen;

    public GameObject nameSelector;

	public GameObject navHighlight;

	
	[Header("Level Selection Dialogues")]
	public GameObject WorldSelectionDialogue;
	public GUI_Dialogue_Call LevelSelectionDialogue;
	public Game_Configuration LevelSelectionConfig;

	public List<GameObject> World1Levels; //beginner
	public List<GameObject> World2Levels; //novice
	public List<GameObject> World3Levels; //intermediate
	public List<GameObject> World4Levels; //advanced
	public List<GameObject> World5Levels; //master
	public List<GameObject> World6Levels; //grand master

	public GUI_Dialogue_Call UnlockablesPanel_Dialogue;

	public LeaderboardController leaderboardController;

	[Header("Resource Notificaiton Panels")]
	public GameObject noEnergyPanel;
	public GameObject notEnoughCoinsPanel;
	public GameObject fullGamePassRequiredPanel;
	public GameObject dailyChallengeCompletePanel;

	
	[Header("Confirm Purchase Panel")]
	

	public GameObject confirmPurchaseBtn;
	public ParticleSystem coinEmitter;
	public CurrenyBarController CurrencyUI;

	public TextMeshProUGUI playerNameText;

	/// <summary>
	/// Method which is called on the success of AccountInfo.DeductEnergy()
	/// </summary>
	/// <param name="result"></param>
	public void StartActiveGameConfiguration()
	{
		if(activeGameConfig!=null)
			activeGameConfig.StartLevel();
		
	}

	public void NavTransitionTrigger()
	{
		//textInfoBar.GetComponentInChildren<Animator>().SetTrigger("fadeOut");
	}

	public void NavTransitionTriggerEnd()
	{
		AudioManager.instance.Play("MenuWoosh");
		NavBar.UpdateNavPos();
		//textInfoBar.GetComponentInChildren<Animator>().SetTrigger("fadeIn");
	}

	public void GoTo(int screen)
	{
		NavBar.ScrollSnap.GoToScreen(screen);
	}

	void Start()
	{
		if(instance == null)
			instance = this;

		Time.timeScale = 1f;
		//ThemeController.EquipTheme(ApplicationModel.THEME);
		StartCoroutine(LoadIntro(0f));
	}

	public void RetryConnection()
	{
		AccountInfo.RetryLogin();
	}

	private IEnumerator LoadIntro(float time)
	{
		yield return new WaitForSeconds(time);
		if(ApplicationModel.RETURN_TO_WORLD == -1)
		{
			if(Account.isActiveAndEnabled == false)
            {
                //Loading Screen, Get Playfab Account
				//Debug.Log("activate loading panel");
                loadingScreen.SetActive(true);
                Account.gameObject.SetActive(true);
                StartCoroutine(LoadIntro(8f));
            } else 
            {
                //Load Main Menu, Press PlayButton
                loadingScreen.SetActive(false);

                if(AccountInfo.Instance.Info.PlayerProfile.DisplayName == null || AccountInfo.Instance.Info.PlayerProfile.DisplayName == "GJG")
				{
					MenuController.instance.navHighlight.SetActive(false);
                    nameSelector.SetActive(true);
				}
                else
                {
					playerNameText.text=AccountInfo.Instance.Info.PlayerProfile.DisplayName;
                    OpenMainMenu();
                }

            }
         
		} else {

			//															 //
			// LOAD INTRO RETURNING FROM SINGLEPLAYER / MULTIPLAYER GAME //
			//	
			
			Debug.LogError("RETURN FROM GAME CODE @ RTW: " + ApplicationModel.RETURN_TO_WORLD);
																	 //
			playerNameText.text=AccountInfo.Instance.Info.PlayerProfile.DisplayName;
			MenuController.instance.navHighlight.SetActive(true);
			AccountInfo.Instance.LoadLeaderboard("Stars");
			AccountInfo.GetAccountInfo();
			AccountInfo.GetPlayerData(AccountInfo.playfabId);
			AccountInfo.Instance.GetInventory();
			MenuController.instance.NavBar.unlockablesPanel.GetComponent<UnlockablesController>().LoadTileStore();
			MenuController.instance.NavBar.unlockablesPanel.GetComponent<UnlockablesController>().LoadGridStore();
			MenuController.instance.InAppPurchasesController.LoadStore();
			MenuController.instance.NavBar.challengeModeDialogue.GetComponent<ChallengeModeController>().InitGameConfigs();
			MenuController.instance.NavBar.challengeModeDialogue.GetComponent<ChallengeModeController>().SelectWorld(ApplicationModel.RETURN_TO_WORLD);
			MenuController.instance.NavBar.gameObject.SetActive(true);
			AccountInfo.UpdateDailyChallengeValue();
			Invoke("CheckForUnlock", 2.5f);
		}
	}

	private void CheckForUnlock()
	{
		NavBar.unlockPanel.GetComponent<UnlockablesController>().CheckForItemUnlock();
		NavBar.challengeModeDialogue.GetComponent<ChallengeModeController>().CheckWorldForStarImprovement();

	}

	/// <summary>
	/// Method which updates the value of challenge bar progressO#
	/// </summary>
	/// <param name="val"></param>
	public void UpdateDailyChallengeUI(int val)
	{
		Debug.Log("update val: " + val);
		progressBar.value=val;
	}

	/// <summary>
	/// Method called when player attempts to start a game without the required amount fo energy
	/// </summary>
	public void EnergyEmpty()
	{
		CloseInspectedItems();
		NavBar.CloseAllDialogues(true);
		NavBar.gameObject.SetActive(false);
		noEnergyPanel.SetActive(true);
		navHighlight.SetActive(false);
	}

	/// <summary>
	/// Method is called via a button on the energy empty panel
	/// </summary>
	public void GoToEnergyStore()
	{
		CloseInspectedItems();
		NavBar.gameObject.SetActive(true);
		NavBar.PressShopButton();
		noEnergyPanel.SetActive(false);
		navHighlight.SetActive(true);
		InAppPurchasesController.scrollRect.ScrollToTop();
	}

	/// <summary>
	/// Method is called via a button on the energy empty panel, returns the player to player panel
	///  This method is called by MULTIPLE resource notificaitons
	///  closes active items and returns to player panel
	/// </summary>
	public void IllWait()
	{
		CloseInspectedItems();
		NavBar.gameObject.SetActive(true);
		NavBar.PressPlayButton();
		noEnergyPanel.SetActive(false);
		notEnoughCoinsPanel.SetActive(false);
		fullGamePassRequiredPanel.SetActive(false);
		navHighlight.SetActive(true);
		dailyChallengeCompletePanel.SetActive(false);


	}

	private void CloseInspectedItems()
	{
		if(NavBar.activeTileBox!=null)
			NavBar.activeTileBox.GetComponent<TileBox>().QuickUninspect();
		else if(NavBar.activeShopItem!=null)
			NavBar.activeShopItem.GetComponent<ShopItem>().QuickUninspect();
		else if (NavBar.activeGridBox!=null)
			NavBar.activeGridBox.GetComponent<GridBox>().QuickUninspect();

	}

	public void InsufficientCoins()
	{
		CloseInspectedItems();
		NavBar.CloseAllDialogues(true);
		NavBar.gameObject.SetActive(false);
		notEnoughCoinsPanel.SetActive(true);
		navHighlight.SetActive(false);
	}

	public void LevelPassRequired()
	{
		CloseInspectedItems();
		NavBar.CloseAllDialogues(true);
		NavBar.gameObject.SetActive(false);
		fullGamePassRequiredPanel.SetActive(true);
		navHighlight.SetActive(false);
	}

	public void GoToCoinStore()
	{
		CloseInspectedItems();
		NavBar.gameObject.SetActive(true);
		NavBar.PressShopButton();
		notEnoughCoinsPanel.SetActive(false);
		navHighlight.SetActive(true);
		InAppPurchasesController.scrollRect.SnapToPositionVertical(InAppPurchasesController.coinStoreMarker, InAppPurchasesController.scrollRectContent, new Vector3(0,1,0));
	}

	public void GoToFullLevelPassStore()
	{
		CloseInspectedItems();
		NavBar.gameObject.SetActive(true);
		NavBar.PressShopButton();
		fullGamePassRequiredPanel.SetActive(false);
		navHighlight.SetActive(true);
		InAppPurchasesController.scrollRect.ScrollToBottom();
	}

	/// <summary>
	/// This method is an overload, which is called when the daily challenge is reset or loaded for first time
	/// </summary>
	/// <param name="title"></param>
	/// <param name="val"></param>
	public void UpdateDailyChallengeUI(string title, int val, int maxVal)
	{
		Debug.Log("Daily Challenge Value: " + val);
		//Get daily challenge complete value
		if(!AccountInfo.DAILY_CHALLENGE_COMPLETED && val >= maxVal)
		{
			Debug.Log("Daily Challenge Complete >>>>>>>>>>!!!!!!!!");
			AccountInfo.AddInGameCurrency(500);
			dailyChallengeTitleText.text=title;
			progressBar.minValue=0;
			progressBar.maxValue=maxVal;
			progressBar.value=maxVal;
			dailyChallengeValueText.text=("Daily Challenge Complete");
			navHighlight.SetActive(false);
			dailyChallengeCompletePanel.SetActive(true);
			CurrencyUI.AddCurrency(500);
			AccountInfo.SetDailyChallengeComplete();
			NavBar.CloseAllDialogues(true);
			NavBar.gameObject.SetActive(false);
			CloseAllMenus(true);

		} else if (AccountInfo.DAILY_CHALLENGE_COMPLETED && val==0)
		{
			Debug.Log("RESET DAILY CHALLENGE!!!");
			AccountInfo.ResetDailyChallenge();
		}

		if(val>maxVal)
			val=maxVal;

		dailyChallengeTitleText.text=title;
		progressBar.minValue=0;
		progressBar.maxValue=maxVal;
		progressBar.value=val;
		dailyChallengeValueText.text=(val+"/"+maxVal);
	}

    public void OpenMainMenu()
    {
        loadingScreen.SetActive(false);
        nameSelector.SetActive(false);
        mainMenuContainer.SetActive(true);
        NavBar.gameObject.SetActive(true);
		navHighlight.SetActive(true);
        NavBar.PressPlayButton();
    }

	public void InitLevelSelector(Game_Configuration config)
	{
		CloseAllMenus(true);
		LevelSelectionDialogue.GetComponent<Game_Configuration>().levelCode = config.levelCode;
		LevelSelectionDialogue.GetComponent<Game_Configuration>().ai_difficulty = config.ai_difficulty;
		LevelSelectionDialogue.GetComponent<Game_Configuration>().levelTitle = config.levelTitle;
		LevelSelectionDialogue.GetComponent<Game_Configuration>().levelNo = config.levelNo;
		LevelSelectionDialogue.GetComponent<Game_Configuration>().worldNo = config.worldNo;
		LevelSelectionDialogue.GetComponent<Game_Configuration>().gridSize = config.gridSize;
		LevelSelectionDialogue.GetComponent<Game_Configuration>().human_players = config.human_players;
		LevelSelectionDialogue.GetComponent<Game_Configuration>().ai_opponents = config.ai_opponents;
		LevelSelectionDialogue.GetComponent<Game_Configuration>().turnTime = config.turnTime;
		LevelSelectionDialogue.GetComponent<Game_Configuration>().maxTile = config.maxTile;
		LevelSelectionDialogue.GetComponent<Game_Configuration>().startTileCounts = config.startTileCounts;
		LevelSelectionDialogue.GetComponent<Game_Configuration>().targetScore = config.targetScore;
		LevelSelectionDialogue.GetComponent<Game_Configuration>().objective1Code = config.objective1Code;
		LevelSelectionDialogue.GetComponent<Game_Configuration>().objective2Code = config.objective2Code;
		LevelSelectionDialogue.GetComponent<Game_Configuration>().objective3Code = config.objective3Code;
		LevelSelectionDialogue.GetComponent<Game_Configuration>().InitaliseLevelSelection();
		if(!LevelSelectionDialogue.isOpen)
			LevelSelectionDialogue.Open();
	}

	public void BackToLevelSelect()
	{
		SelectWorld(LevelSelectionDialogue.GetComponent<Game_Configuration>().worldNo+1);
	}



	public void OpenWorldSelector()
	{
		CloseAllMenus(true);
		WorldSelectionDialogue.SetActive(true);
		// InfoPanelText.text = "Select World";
		// InfoPanel_Dialogue.SetActive(true);
	}

	public void SelectWorld(int worldNo)
	{
		NavBar.CloseAllDialogues(true);
		navHighlight.SetActive(true);
		GUI_Controller.instance.CoinDialogue.SetActive(true);
        NavBar.gameObject.SetActive(true);
        mainMenuContainer.SetActive(true);
		switch(worldNo)
		{
			case 1:
			foreach(GameObject Dialogue in World1Levels)
			{
				Dialogue.SetActive(true);
			}
			if(AccountInfo.beginnerStars == 30)
			fireWorks.SetActive(true);
			break;

			case 2:
			foreach(GameObject Dialogue in World2Levels)
			{
				Dialogue.SetActive(true);
			}
			if(AccountInfo.noviceStars == 30)
			fireWorks.SetActive(true);
			break;

			case 3:
			foreach(GameObject Dialogue in World3Levels)
			{
				Dialogue.SetActive(true);
			}
			if(AccountInfo.intermediateStars == 30)
			fireWorks.SetActive(true);
			break;

			case 4:
			foreach(GameObject Dialogue in World4Levels)
			{
				Dialogue.SetActive(true);
			}
			if(AccountInfo.adeptStars == 30)
			fireWorks.SetActive(true);
			break;
			
			case 5:
			foreach(GameObject Dialogue in World4Levels)
			{
				Dialogue.SetActive(true);
			}
			if(AccountInfo.advancedStars == 30)
			fireWorks.SetActive(true);
			break;

			case 6:
			foreach(GameObject Dialogue in World4Levels)
			{
				Dialogue.SetActive(true);
			}
			if(AccountInfo.expertStars == 30)
			fireWorks.SetActive(true);
			break;

			case 7:
			foreach(GameObject Dialogue in World5Levels)
			{
				Dialogue.SetActive(true);
			}
			if(AccountInfo.masterStars == 30)
			fireWorks.SetActive(true);
			break;

			case 8:
			foreach(GameObject Dialogue in World4Levels)
			{
				Dialogue.SetActive(true);
			}
			if(AccountInfo.targetMasterStars == 30)
			fireWorks.SetActive(true);
			break;

			case 9:
			foreach(GameObject Dialogue in World6Levels)
			{
				Dialogue.SetActive(true);
			}
			if(AccountInfo.grandMasterStars == 30)
			fireWorks.SetActive(true);
			break;

			case 10:
			foreach(GameObject Dialogue in World4Levels)
			{
				Dialogue.SetActive(true);
			}
			if(AccountInfo.legendStars == 30)
			fireWorks.SetActive(true);
			break;

		}

		//Home_Dialogue.GetComponentInChildren<Button>().interactable = true;
	}


	public void ShowUnlockablesPAnel()
	{
		CloseAllMenus(false);
		UnlockablesPanel_Dialogue.Open();
	}

	public void StartTutorial()
	{
		ApplicationModel.TUTORIAL_MODE = true;
		ApplicationModel.GRID_SIZE = 5;
		ApplicationModel.VS_AI = true;
		ApplicationModel.VS_LOCAL_MP=false;
		ApplicationModel.SOLO_PLAY = false;
		ApplicationModel.AI_PLAYERS=1;
		ApplicationModel.HUMAN_PLAYERS=1;
		ApplicationModel.RETURN_TO_WORLD=-1;
		levelChanger.FadeToLevel("Main");
	}

	public void StartLocalMultiplayer(int difficulty)
	{
		ApplicationModel.GAME_DIFFICULTY = difficulty;
		ApplicationModel.TUTORIAL_MODE = false;
		ApplicationModel.VS_AI = false;
		ApplicationModel.VS_LOCAL_MP = true;
		ApplicationModel.SOLO_PLAY = false;
		ApplicationModel.THEME = 0;
		ApplicationModel.PLAYERS=2;
		ApplicationModel.AI_PLAYERS=0;
		ApplicationModel.HUMAN_PLAYERS=2;
        //NavBar.CloseAllDialogues(true);
		levelChanger.FadeToLevel("Main");
		
	}

	public void StartGameAI(int difficulty)
	{
		ApplicationModel.GAME_DIFFICULTY = difficulty;
		ApplicationModel.TUTORIAL_MODE = false;
		ApplicationModel.VS_AI = true;
		ApplicationModel.VS_LOCAL_MP = false;
		ApplicationModel.SOLO_PLAY = false;
        //NavBar.CloseAllDialogues(true);
		levelChanger.FadeToLevel("Main");
	}

	public void SoloPlay()
	{
		ApplicationModel.VS_AI = false;
		ApplicationModel.VS_LOCAL_MP = false;
		ApplicationModel.SOLO_PLAY = true;
		ApplicationModel.TUTORIAL_MODE = false;
		//ApplicationModel.theme = 1;
        //NavBar.CloseAllDialogues(true);
		levelChanger.FadeToLevel("Main");
	}


	public void ToggleDialogueInteractions(List<GUI_Dialogue_Call> Dialogues, bool open)
	{
		if(open)
		{
			foreach(GUI_Dialogue_Call Dialogue in Dialogues)
			{
				if(Dialogue.GetComponentInChildren<Button>() != null)
					Dialogue.GetComponentInChildren<Button>().interactable = true;
			}
		} else 
		{
			foreach(GUI_Dialogue_Call Dialogue in Dialogues)
			{
				if(Dialogue.GetComponentInChildren<Button>() != null)
					Dialogue.GetComponentInChildren<Button>().interactable = false;
			}
		}
	}


	public void StartPracticeMode()
	{
		ApplicationModel.SOLO_PLAY = true;
		ApplicationModel.VS_LOCAL_MP = false;
		ApplicationModel.VS_AI = false;
		ApplicationModel.TURNS = 999;
		ApplicationModel.TARGET = 400;
		ApplicationModel.TUTORIAL_MODE = false;
	}

	private void PurchaseSelectedTileSkin()
	{
		NavBar.unlockablesPanel.GetComponent<UnlockablesController>().LockShopButtons();
		if(AccountInfo.Instance.InventoryContains(NavBar.activeTileBox.GetComponent<TileBox>().item))
		{
			NavBar.activeTileBox.GetComponent<TileBox>().EquipTileSkin();
		} else 
		{
			uint price=0;
			if(NavBar.activeTileBox.GetComponent<TileBox>().item.VirtualCurrencyPrices.TryGetValue(AccountInfo.COINS_CODE, out price))
			{
				if(price != 0 && price <=  GUI_Controller.instance.CurrencyUI.playerCoins)
				{
					confirmPurchaseBtn.GetComponent<Animator>().enabled=true;
					confirmPurchaseBtn.GetComponent<Animator>().SetTrigger("pressPurchase");
					NavBar.activeTileBox.GetComponent<TileBox>().PurchaseWithCoins();
				} else
				{
					Debug.LogError("FAILED TO GET PRICE OR NOT ENOUGH GOLD price: " + price);
					if(price!=0)
					{
						MenuController.instance.InsufficientCoins();
					} else if(price==0)
					{
						Debug.Log("Equipping tile skin");
						NavBar.activeTileBox.GetComponent<TileBox>().unlocked=true;
						NavBar.activeTileBox.GetComponent<TileBox>().EquipTileSkin();
					}

					NavBar.unlockablesPanel.GetComponent<UnlockablesController>().UnlockShopButtons();
				}
			}
		}
	}

	private void PurchaseSelectedGridSkin()
	{
		NavBar.unlockablesPanel.GetComponent<UnlockablesController>().LockShopButtons();
		if(AccountInfo.Instance.InventoryContains(NavBar.activeGridBox.GetComponent<GridBox>().item))
		{
			NavBar.activeGridBox.GetComponent<GridBox>().EquipGridSkin();
		} else 
		{
			uint price=0;
			if(NavBar.activeGridBox.GetComponent<GridBox>().item.VirtualCurrencyPrices.TryGetValue(AccountInfo.COINS_CODE, out price))
			{
				if(price != 0 && price <=  GUI_Controller.instance.CurrencyUI.playerCoins)
				{
					confirmPurchaseBtn.GetComponent<Animator>().enabled=true;
					confirmPurchaseBtn.GetComponent<Animator>().SetTrigger("pressPurchase");
					NavBar.activeGridBox.GetComponent<GridBox>().PurchaseWithCoins();
				} else if (price ==0)
				{
					NavBar.activeGridBox.GetComponent<GridBox>().EquipGridSkin();
				} else
				{
					Debug.LogError("FAILED TO GET PRICE OR NOT ENOUGH GOLD");
					if(price!=0)
						MenuController.instance.InsufficientCoins();

					NavBar.unlockablesPanel.GetComponent<UnlockablesController>().UnlockShopButtons();
				}
			}
		}
	}

	public void PurchaseActiveItem()
	{
		if(NavBar.activeTileBox!=null)
			PurchaseSelectedTileSkin();
		else if(NavBar.activeShopItem!=null)
			PurchaseSelectedIAP();
		else if (NavBar.activeGridBox!=null)
			PurchaseSelectedGridSkin();
			
	}

	private void PurchaseSelectedIAP()
	{
		ShopItem selectedItem = NavBar.activeShopItem.GetComponent<ShopItem>();

		if(selectedItem==null)
		{
			Debug.LogError("Selected Item Null");
			return;
		}
		
		MenuController.instance.InAppPurchasesController.LockShopButtons();
		switch(selectedItem.item.ItemClass)
		{
			case "Life Refill":
				
					uint price=0;
					if(selectedItem.item.VirtualCurrencyPrices.TryGetValue(AccountInfo.COINS_CODE, out price))
					{
						if(price != 0 && price <=  GUI_Controller.instance.CurrencyUI.playerCoins)
						{
							confirmPurchaseBtn.GetComponent<Animator>().enabled=true;
							confirmPurchaseBtn.GetComponent<Animator>().SetTrigger("pressPurchase");
							selectedItem.PurchaseWithCoins();
						} else
						{
							Debug.LogError("FAILED TO GET PRICE OR NOT ENOUGH GOLD");
							if(price!=0)
							MenuController.instance.InsufficientCoins();

							NavBar.unlockablesPanel.GetComponent<UnlockablesController>().UnlockShopButtons();
						}
					}

			break;

			case "Life Pass":
				MenuController.instance.InAppPurchasesController.UnlockShopButtons();
				InAppPurchasesController.BuyProductID(selectedItem.itemID);
			break;

			case "Coin Pack":
				MenuController.instance.InAppPurchasesController.UnlockShopButtons();
				InAppPurchasesController.BuyProductID(selectedItem.itemID);
			break;

			case "Level Pack":
				MenuController.instance.InAppPurchasesController.UnlockShopButtons();
				InAppPurchasesController.BuyProductID(selectedItem.itemID);
			break;

			default:
			Debug.LogError("IAP CLASS NOT FOUND");
			break;
		}		
	}


	public void CloseAllMenus(bool closeInfo)
	{

	}

	public void Quit()
	{
		Application.Quit();
	}
}
