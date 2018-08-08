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
				Debug.Log("activate loading panel");
                loadingScreen.SetActive(true);
                Account.gameObject.SetActive(true);
                StartCoroutine(LoadIntro(7.5f));
            } else 
            {
                //Load Main Menu, Press PlayButton
                loadingScreen.SetActive(false);

                if(AccountInfo.Instance.Info.PlayerProfile.DisplayName == null || AccountInfo.Instance.Info.PlayerProfile.DisplayName == "GJG")
                    nameSelector.SetActive(true);
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
			
			Debug.LogError("RETURN FROM GAME CODE ");
																	 //
			playerNameText.text=AccountInfo.Instance.Info.PlayerProfile.DisplayName;
			MenuController.instance.navHighlight.SetActive(true);
			AccountInfo.Instance.LoadLeaderboard("Wins");
			AccountInfo.GetAccountInfo();
			AccountInfo.GetPlayerData(AccountInfo.playfabId);
			AccountInfo.Instance.GetInventory();
			MenuController.instance.NavBar.unlockablesPanel.GetComponent<UnlockablesController>().LoadTileStore();
			MenuController.instance.InAppPurchasesController.LoadStore();
			MenuController.instance.NavBar.challengeModeDialogue.GetComponent<ChallengeModeController>().SelectWorld(ApplicationModel.RETURN_TO_WORLD);
			MenuController.instance.NavBar.gameObject.SetActive(true);
			AccountInfo.UpdateDailyChallengeValue();
			//Invoke("CheckForUnlock", 2.5f);
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
	/// This method is an overload, which is called when the daily challenge is reset or loaded for first time
	/// </summary>
	/// <param name="title"></param>
	/// <param name="val"></param>
	public void UpdateDailyChallengeUI(string title, int val, int maxVal)
	{
		Debug.Log("overload update val: " + val);
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
		LevelSelectionDialogue.GetComponent<Game_Configuration>().oneTiles = config.oneTiles;
		LevelSelectionDialogue.GetComponent<Game_Configuration>().twoTiles = config.twoTiles;
		LevelSelectionDialogue.GetComponent<Game_Configuration>().threeTiles = config.threeTiles;
		LevelSelectionDialogue.GetComponent<Game_Configuration>().fourTiles = config.fourTiles;
		LevelSelectionDialogue.GetComponent<Game_Configuration>().fiveTiles = config.fiveTiles;
		LevelSelectionDialogue.GetComponent<Game_Configuration>().sixTiles = config.sixTiles;
		LevelSelectionDialogue.GetComponent<Game_Configuration>().sevenTiles = config.sevenTiles;
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
			if(AccountInfo.advancedStars == 30)
			fireWorks.SetActive(true);
			break;

			case 5:
			foreach(GameObject Dialogue in World5Levels)
			{
				Dialogue.SetActive(true);
			}
			if(AccountInfo.masterStars == 30)
			fireWorks.SetActive(true);
			break;

			case 6:
			foreach(GameObject Dialogue in World6Levels)
			{
				Dialogue.SetActive(true);
			}
			if(AccountInfo.grandMasterStars == 30)
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
		ApplicationModel.THEME = 3;
        NavBar.CloseAllDialogues(true);
		levelChanger.FadeToLevel("Main");
		
	}

	public void StartGameAI(int difficulty)
	{
		ApplicationModel.GAME_DIFFICULTY = difficulty;
		ApplicationModel.TUTORIAL_MODE = false;
		ApplicationModel.VS_AI = true;
		ApplicationModel.VS_LOCAL_MP = false;
		ApplicationModel.SOLO_PLAY = false;
        NavBar.CloseAllDialogues(true);
		levelChanger.FadeToLevel("Main");
	}

	public void SoloPlay()
	{
		ApplicationModel.VS_AI = false;
		ApplicationModel.VS_LOCAL_MP = false;
		ApplicationModel.SOLO_PLAY = true;
		ApplicationModel.TUTORIAL_MODE = false;
		//ApplicationModel.theme = 1;
        NavBar.CloseAllDialogues(true);
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
					Debug.LogError("FAILED TO GET PRICE OR NOT ENOUGH GOLD");
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
							NavBar.unlockablesPanel.GetComponent<UnlockablesController>().UnlockShopButtons();
						}
					}

			break;

			case "Energy Pass":
			break;

			case "Coin Bundle":
			break;

			case "Full Game Level Pack":
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
