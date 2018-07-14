using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

	public static MenuController instance = null;

    [SerializeField]
    private LevelChanger levelChanger;
	
    [Header("Player Account")]
    [SerializeField]
    private AccountInfo Account;

    [Header("Other Panels")]
	public GameObject fireWorks;
    public GameObject mainMenuContainer;

    public GameObject loadingScreen;

    public GameObject nameSelector;

	public GameObject navHighlight;

    public NavigationBarController NavBar;
	
	[Header("Level Selection Dialogues")]
	public GUI_Dialogue_Call WorldSelectionDialogue;
	public GUI_Dialogue_Call LevelSelectionDialogue;
	public Game_Configuration LevelSelectionConfig;

	public List<GameObject> World1Levels;
	public List<GameObject> World2Levels;
	public List<GUI_Dialogue_Call> World3Levels;
	public List<GUI_Dialogue_Call> World4Levels;

	public GUI_Dialogue_Call UnlockablesPanel_Dialogue;

	public GUI_Dialogue_Call Home_Dialogue;

	public TextMeshProUGUI InfoPanelText;

	
	[SerializeField]
	private ThemeController ThemeController;

	void Start()
	{
		if(instance == null)
			instance = this;

		Time.timeScale = 1f;
		//ThemeController.EquipTheme(ApplicationModel.THEME);
		StartCoroutine(LoadIntro(0f));
	}

	private IEnumerator LoadIntro(float time)
	{
		yield return new WaitForSeconds(time);
		if(ApplicationModel.RETURN_TO_WORLD == -1)
		{
			if(Account.isActiveAndEnabled == false)
            {
                //Loading Screen, Get Playfab Account Shit
                Account.gameObject.SetActive(true);
                loadingScreen.SetActive(true);
                StartCoroutine(LoadIntro(5.5f));
            } else 
            {
                //Load Main Menu, Press PlayButton
                loadingScreen.SetActive(false);

                if(AccountInfo.Instance.Info.PlayerProfile.DisplayName == null || AccountInfo.Instance.Info.PlayerProfile.DisplayName == "GJG")
                    nameSelector.SetActive(true);
                else
                {
                    OpenMainMenu();
                }


            }
            

		} else {
			AccountInfo.GetAccountInfo();
			SelectWorld(ApplicationModel.RETURN_TO_WORLD+1);
		}
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
		WorldSelectionDialogue.Open();
		// InfoPanelText.text = "Select World";
		// InfoPanel_Dialogue.SetActive(true);
	}

	public void SelectWorld(int worldNo)
	{
		NavBar.CloseAllDialogues(true);
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
			if(AccountInfo.beginnerStars == 15)
			fireWorks.SetActive(true);
			break;

			case 2:
			foreach(GameObject Dialogue in World2Levels)
			{
				Dialogue.SetActive(true);
			}
			if(AccountInfo.intermediateStars == 15)
			fireWorks.SetActive(true);
			break;

			case 3:
			foreach(GUI_Dialogue_Call Dialogue in World3Levels)
			{
				Dialogue.Open();
			}
			break;

			case 4:
			foreach(GUI_Dialogue_Call Dialogue in World4Levels)
			{
				Dialogue.Open();
			}
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




	public void CloseAllMenus(bool closeInfo)
	{
	}

	public void Quit()
	{
		Application.Quit();
	}
}
