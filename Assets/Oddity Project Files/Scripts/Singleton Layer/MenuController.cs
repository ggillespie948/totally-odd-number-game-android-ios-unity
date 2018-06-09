using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

	public static MenuController instance = null;

	public bool isAnimating = false;

    [Header("Main Menu Buttons")]
	public GUI_Object EasyBtn;
	public GUI_Object MedBtn;
	public GUI_Object HardBtn;

	public GUI_Object EasyBtnLM;
	public GUI_Object MedBtnLM;
	public GUI_Object HardBtnLM;

    [Header("Other Panels")]
	public GameObject fireWorks;
	
	[Header("Tile Spawners")]
	public TileSpawner[] Tile1Spanwers;

	public TileSpawner[] Tile2Spanwers;

	public TileSpawner[] Tile3Spanwers;

	[Header("Level Selection Dialogues")]
	public GUI_Dialogue_Call WorldSelectionDialogue;
	public GUI_Dialogue_Call LevelSelectionDialogue;
	public Game_Configuration LevelSelectionConfig;

	public List<GameObject> World1Levels;
	public List<GameObject> World2Levels;
	public List<GUI_Dialogue_Call> World3Levels;
	public List<GUI_Dialogue_Call> World4Levels;

	[Header("Menu Dialogues")]
	public GUI_Dialogue_Call Singleplayer_MenuButton;
	public GUI_Dialogue_Call Multiplayer_MenuButton;
	public GUI_Dialogue_Call Unlockables_MenuButton;
	public GUI_Dialogue_Call Tutorial_MenuButton;
	public GUI_Dialogue_Call PlayGame_MenuButton;
	private List<GUI_Dialogue_Call> MainMenuDialogues;
	private List<GUI_Dialogue_Call> SinglePlayerDialogues;

	private List<GUI_Dialogue_Call> PlayerNumberDialogues;
	public GUI_Dialogue_Call Two_MenuButton;
	public GUI_Dialogue_Call Three_MenuButton;
	public GUI_Dialogue_Call Four_MenuButton;


	public GUI_Dialogue_Call ChallengeMode_MenuButton;
	public GUI_Dialogue_Call TargetMode_MenuButton;
	public GUI_Dialogue_Call PuzzleMode_MenuButton;
	public GUI_Dialogue_Call Practice_MenuButton;
	private List<GUI_Dialogue_Call> MultiplayerDialogues;

	public GUI_Dialogue_Call UnlockablesPanel_Dialogue;

	private List<GUI_Dialogue_Call> GridSizeDialogues;

	public GUI_Dialogue_Call Five_MenuButton;
	public GUI_Dialogue_Call Seven_MenuButton;
	public GUI_Dialogue_Call Nine_MenuButton;

	public GameObject InfoPanel_Dialogue;

	public GUI_Dialogue_Call Home_Dialogue;

	public TextMeshProUGUI InfoPanelText;

	private List<GUI_Dialogue_Call> PlayerNumberHumanDialogues;
	public GUI_Dialogue_Call Hu_One_MenuButton;
	public GUI_Dialogue_Call Hu_Two_MenuButton;

	public GUI_Dialogue_Call Hu_Three_MenuButton;
	public GUI_Dialogue_Call Hu_Four_MenuButton;

	private List<GUI_Dialogue_Call> PlayerNumberAIDialogues;
	public GUI_Dialogue_Call Ai_Zero_MenuButton;
	public GUI_Dialogue_Call Ai_One_MenuButton;
	public GUI_Dialogue_Call Ai_Two_MenuButton;

	public GUI_Dialogue_Call Ai_Three_MenuButton;

	public GUI_Dialogue_Call CustomGame_Dialogue;

	public GUI_Dialogue_Call PuzzleMode_Dialogue;

	public GUI_Dialogue_Call ThemeMenu_Dialogue;

	[SerializeField]
	private ThemeController ThemeController;

	void Start()
	{
		if(instance == null)
			instance = this;

		Time.timeScale = 1f;
		MainMenuDialogues = new List<GUI_Dialogue_Call>();
		if(Singleplayer_MenuButton != null) { MainMenuDialogues.Add(Singleplayer_MenuButton); }
		if(Multiplayer_MenuButton != null) { MainMenuDialogues.Add(Multiplayer_MenuButton); }
		if(Unlockables_MenuButton != null) { MainMenuDialogues.Add(Unlockables_MenuButton); }
		if(Tutorial_MenuButton != null) { MainMenuDialogues.Add(Tutorial_MenuButton); }

		SinglePlayerDialogues = new List<GUI_Dialogue_Call>();
		if(ChallengeMode_MenuButton != null) { SinglePlayerDialogues.Add(ChallengeMode_MenuButton); }
		if(TargetMode_MenuButton != null) { SinglePlayerDialogues.Add(TargetMode_MenuButton); }
		if(PuzzleMode_MenuButton != null) { SinglePlayerDialogues.Add(PuzzleMode_MenuButton); }
		if(Practice_MenuButton != null) { SinglePlayerDialogues.Add(Practice_MenuButton); }
		if(Home_Dialogue != null) {SinglePlayerDialogues.Add(Home_Dialogue); }

		GridSizeDialogues = new List<GUI_Dialogue_Call>();
		if(Five_MenuButton != null) { GridSizeDialogues.Add(Five_MenuButton); }
		if(Seven_MenuButton != null) { GridSizeDialogues.Add(Seven_MenuButton); }
		if(Nine_MenuButton != null) { GridSizeDialogues.Add(Nine_MenuButton); }
		if(Home_Dialogue != null) {GridSizeDialogues.Add(Home_Dialogue); }

		PlayerNumberDialogues = new List<GUI_Dialogue_Call>();
		if(Two_MenuButton != null) { PlayerNumberDialogues.Add(Two_MenuButton); }
		if(Three_MenuButton != null) { PlayerNumberDialogues.Add(Three_MenuButton); }
		if(Four_MenuButton != null) { PlayerNumberDialogues.Add(Four_MenuButton); }
		if(Home_Dialogue != null) {PlayerNumberDialogues.Add(Home_Dialogue); }

		PlayerNumberHumanDialogues = new List<GUI_Dialogue_Call>();
		if(Hu_One_MenuButton != null) {PlayerNumberHumanDialogues.Add(Hu_One_MenuButton);} 
		if(Hu_Two_MenuButton != null) {PlayerNumberHumanDialogues.Add(Hu_Two_MenuButton);} 
		if(Hu_Three_MenuButton != null) {PlayerNumberHumanDialogues.Add(Hu_Three_MenuButton);} 
		if(Hu_Four_MenuButton != null) {PlayerNumberHumanDialogues.Add(Hu_Four_MenuButton);}
		if(Home_Dialogue != null) {PlayerNumberHumanDialogues.Add(Home_Dialogue); } 

		PlayerNumberAIDialogues = new List<GUI_Dialogue_Call>();
		if(Ai_Zero_MenuButton != null) {PlayerNumberAIDialogues.Add(Ai_Zero_MenuButton);} 
		if(Ai_One_MenuButton != null) {PlayerNumberAIDialogues.Add(Ai_One_MenuButton);} 
		if(Ai_Two_MenuButton != null) {PlayerNumberAIDialogues.Add(Ai_Two_MenuButton);} 
		if(Ai_Three_MenuButton != null) {PlayerNumberAIDialogues.Add(Ai_Three_MenuButton);} 
		if(Home_Dialogue != null) {PlayerNumberAIDialogues.Add(Home_Dialogue); }

		ThemeController.EquipTheme(ApplicationModel.THEME);

		//PlayGame_MenuButton.Open();
		StartCoroutine(LoadIntro(3f));
	}

	private IEnumerator LoadIntro(float time)
	{
		yield return new WaitForSeconds(time);
		if(ApplicationModel.RETURN_TO_WORLD == -1)
		{
			PlayGame_MenuButton.Open();
		} else {
			SelectWorld(ApplicationModel.RETURN_TO_WORLD+1);
		}
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

	public void OpenCustomGame()
	{
		CloseAllMenus(true);
		CustomGame_Dialogue.Open();
	}

	public void OpenPuzzleMode()
	{
		CloseAllMenus(true);
		PuzzleMode_Dialogue.Open();
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
		CloseAllMenus(true);
		GUI_Controller.instance.CoinDialogue.SetActive(true);
		InfoPanelText.text = "Level Select";
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

		Home_Dialogue.GetComponentInChildren<Button>().interactable = true;
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
		SceneManager.LoadScene("Main");
	}

	public void StartLocalMultiplayer(int difficulty)
	{
		ApplicationModel.GAME_DIFFICULTY = difficulty;
		ApplicationModel.TUTORIAL_MODE = false;
		ApplicationModel.VS_AI = false;
		ApplicationModel.VS_LOCAL_MP = true;
		ApplicationModel.SOLO_PLAY = false;
		ApplicationModel.THEME = 3;
		SceneManager.LoadScene("Main");
		
	}

	public void StartGameAI(int difficulty)
	{
		ApplicationModel.GAME_DIFFICULTY = difficulty;
		ApplicationModel.TUTORIAL_MODE = false;
		ApplicationModel.VS_AI = true;
		ApplicationModel.VS_LOCAL_MP = false;
		ApplicationModel.SOLO_PLAY = false;
		SceneManager.LoadScene("Main");
	}

	public void SoloPlay()
	{
		ApplicationModel.VS_AI = false;
		ApplicationModel.VS_LOCAL_MP = false;
		ApplicationModel.SOLO_PLAY = true;
		ApplicationModel.TUTORIAL_MODE = false;
		//ApplicationModel.theme = 1;
		SceneManager.LoadScene("Main");

	}

	public void OpenMainMenu()
	{
		CloseAllMenus(true);
		GUI_Controller.instance.ToggleSettingsButton(true);
        GUI_Controller.instance.ToggleCurrencyUI(true);
		StartCoroutine(MainMenuIntro());

	}

	private IEnumerator MainMenuIntro()
	{
		yield return new WaitForSeconds(0.55f);
		foreach( GUI_Dialogue_Call Dialogue in MainMenuDialogues)
		{
			Dialogue.Open();
			yield return new WaitForSeconds(0.35f);
		}
		ToggleDialogueInteractions(MainMenuDialogues, true);


	}

	public void OpenSinglePlayer()
	{
		CloseAllMenus(false);

		StartCoroutine(OpenSinglePlayerIntro());
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

	public void StartCustomVsCPU()
	{
		ApplicationModel.SOLO_PLAY = false;
		ApplicationModel.VS_LOCAL_MP = false;
		ApplicationModel.VS_AI = true;
		ApplicationModel.TUTORIAL_MODE = false;
		OpenPlayerNumbers();
		//OpenGridSizes();
	}

	public void StartCustomTargetMode()
	{
		ApplicationModel.SOLO_PLAY = true;
		ApplicationModel.VS_LOCAL_MP = false;
		ApplicationModel.VS_AI = false;
		ApplicationModel.TUTORIAL_MODE = false;
		OpenGridSizes();
	}

	public void StartCustomLocalMP()
	{
		ApplicationModel.SOLO_PLAY = false;
		ApplicationModel.VS_LOCAL_MP = true;
		ApplicationModel.VS_AI = true;
		ApplicationModel.TUTORIAL_MODE = false;
		OpenPlayerHuNumbers();
	}

	public void StartPracticeMode()
	{
		ApplicationModel.SOLO_PLAY = true;
		ApplicationModel.VS_LOCAL_MP = false;
		ApplicationModel.VS_AI = false;
		ApplicationModel.TURNS = 999;
		ApplicationModel.TARGET = 400;
		ApplicationModel.TUTORIAL_MODE = false;
		OpenGridSizes();
	}


	private IEnumerator OpenSinglePlayerIntro()
	{
		yield return new WaitForSeconds(0.45f);
		foreach( GUI_Dialogue_Call Dialogue in SinglePlayerDialogues)
		{
			Dialogue.Open();
			yield return new WaitForSeconds(0.35f);
		}
		ToggleDialogueInteractions(SinglePlayerDialogues, true);
		
	}

	private IEnumerator OpenPlayersNumberIntro()
	{
		yield return new WaitForSeconds(0.45f);
		foreach( GUI_Dialogue_Call Dialogue in PlayerNumberDialogues)
		{
			Dialogue.Open();
			yield return new WaitForSeconds(0.35f);
		}
		ToggleDialogueInteractions(PlayerNumberDialogues, true);
	}

	private IEnumerator OpenPlayersNumberHuIntro()
	{
		yield return new WaitForSeconds(0.45f);
		foreach( GUI_Dialogue_Call Dialogue in PlayerNumberHumanDialogues)
		{
			Dialogue.Open();
			yield return new WaitForSeconds(0.35f);
		}
		ToggleDialogueInteractions(PlayerNumberHumanDialogues, true);
	}

	private IEnumerator OpenPlayersNumberAIIntro()
	{
		yield return new WaitForSeconds(0.45f);

		if(ApplicationModel.HUMAN_PLAYERS == 1)
		{
			Ai_One_MenuButton.Open();
			yield return new WaitForSeconds(0.35f);
			Ai_Two_MenuButton.Open();
			yield return new WaitForSeconds(0.35f);
			Ai_Three_MenuButton.Open();
		} else if (ApplicationModel.HUMAN_PLAYERS == 2)
		{
			Ai_Zero_MenuButton.Open();
			yield return new WaitForSeconds(0.35f);
			Ai_One_MenuButton.Open();
			yield return new WaitForSeconds(0.35f);
			Ai_Two_MenuButton.Open();
		} else if (ApplicationModel.HUMAN_PLAYERS == 3)
		{
			Ai_Zero_MenuButton.Open();
			yield return new WaitForSeconds(0.35f);
			Ai_One_MenuButton.Open();
		} else if (ApplicationModel.HUMAN_PLAYERS == 4)
		{
			OpenGridSizes();
		} 

		ToggleDialogueInteractions(PlayerNumberAIDialogues, true);
	}

	public void OpenGridSizes()
	{
		CloseAllMenus(false);
		StartCoroutine(OpenGridSizeIntro());
		InfoPanelText.text = "Grid Sizes";
		InfoPanel_Dialogue.SetActive(true);
	}

	public void OpenPlayerNumbers()
	{
		CloseAllMenus(false);
		StartCoroutine(OpenPlayersNumberIntro());
		InfoPanelText.text = "Players";
		InfoPanel_Dialogue.SetActive(true);
	}

	public void OpenThemeMenu()
	{
		CloseAllMenus(true);
		ThemeMenu_Dialogue.Open();
	}

	public void OpenPlayerHuNumbers()
	{
		CloseAllMenus(false);
		StartCoroutine(OpenPlayersNumberHuIntro());
		InfoPanelText.text = "Human Players";
		InfoPanel_Dialogue.SetActive(true);
	}

	public void OpenPlayerAiNumbers()
	{
		CloseAllMenus(false);
		StartCoroutine(OpenPlayersNumberAIIntro());
		InfoPanelText.text = "AI Players";
		InfoPanel_Dialogue.SetActive(true);

	}

	private IEnumerator OpenGridSizeIntro()
	{
		yield return new WaitForSeconds(0.45f);
		foreach( GUI_Dialogue_Call Dialogue in GridSizeDialogues)
		{
			Dialogue.Open();
			yield return new WaitForSeconds(0.35f);
		}
		ToggleDialogueInteractions(GridSizeDialogues, true);
	}

	public void StartGameGridSize(int size)
	{
		ApplicationModel.GRID_SIZE = size;

		if(ApplicationModel.VS_AI)
		{
			StartGameAI(1);
		} else if(ApplicationModel.VS_LOCAL_MP) 
		{
			StartLocalMultiplayer(1);
		} else if (ApplicationModel.SOLO_PLAY)
		{
			ApplicationModel.AI_PLAYERS=0;
			ApplicationModel.HUMAN_PLAYERS=1;
			SoloPlay();
		}

	}

	public void SetPlayersNumberAi(int size)
	{
		ApplicationModel.AI_PLAYERS = size;
		if(ApplicationModel.AI_PLAYERS == 0)
		{
			ApplicationModel.VS_AI = false;
		}
		OpenGridSizes();

	}

	public void SetPlayersNumberHu(int size)
	{
		ApplicationModel.HUMAN_PLAYERS = size;
		OpenPlayerAiNumbers();
	}

	public void CloseAllMenus(bool closeInfo)
	{
	// 	foreach( GUI_Dialogue_Call Dialogue in MainMenuDialogues)
	// 	{
	// 		if(Dialogue.isOpen)
	// 			Dialogue.Close();
	// 	}
	// 	ToggleDialogueInteractions(MainMenuDialogues, false);

	// 	foreach( GUI_Dialogue_Call Dialogue in SinglePlayerDialogues)
	// 	{
	// 		if(Dialogue.isOpen)
	// 			Dialogue.Close();
	// 	}
	// 	ToggleDialogueInteractions(SinglePlayerDialogues, false);

	// 	foreach( GUI_Dialogue_Call Dialogue in GridSizeDialogues)
	// 	{
	// 		if(Dialogue.isOpen)
	// 			Dialogue.Close();
	// 	}
	// 	ToggleDialogueInteractions(GridSizeDialogues, false);

	// 	foreach( GUI_Dialogue_Call Dialogue in PlayerNumberDialogues)
	// 	{
	// 		if(Dialogue.isOpen)
	// 			Dialogue.Close();
	// 	}
	// 	ToggleDialogueInteractions(PlayerNumberDialogues, false);

	// 	foreach( GUI_Dialogue_Call Dialogue in PlayerNumberHumanDialogues)
	// 	{
	// 		if(Dialogue.isOpen)
	// 			Dialogue.Close();
	// 	}
	// 	ToggleDialogueInteractions(PlayerNumberHumanDialogues, false);

	// 	foreach( GUI_Dialogue_Call Dialogue in PlayerNumberAIDialogues)
	// 	{
	// 		if(Dialogue.isOpen)
	// 			Dialogue.Close();
	// 	}
	// 	ToggleDialogueInteractions(PlayerNumberAIDialogues, false);

	// 	if(closeInfo)
	// 		InfoPanel_Dialogue.SetActive(false);

	// 	if(PlayGame_MenuButton.isOpen)
	// 		PlayGame_MenuButton.Close();
	// 	if(LevelSelectionDialogue.isOpen)	
	// 		LevelSelectionDialogue.Close();
	// 	if(WorldSelectionDialogue.isOpen)			
	// 		WorldSelectionDialogue.Close();

	// 	fireWorks.SetActive(false);

	// 	// foreach(GUI_Dialogue_Call Dialogue in World1Levels)
	// 	// {
	// 	// 	if(Dialogue.isOpen)
	// 	// 		Dialogue.Close();
	// 	// }

	// 	foreach(GUI_Dialogue_Call Dialogue in World2Levels)
	// 	{
	// 		if(Dialogue.isOpen)
	// 			Dialogue.Close();
	// 	}

	// 	foreach(GUI_Dialogue_Call Dialogue in World3Levels)
	// 	{
	// 		if(Dialogue.isOpen)
	// 			Dialogue.Close();
	// 	}

	// 	if(CustomGame_Dialogue.isOpen)
	// 	{
	// 		CustomGame_Dialogue.Close();
	// 	}

	// 	if(ThemeMenu_Dialogue.isOpen)
	// 	{
	// 		ThemeMenu_Dialogue.Close();
	// 	}

	// 	// foreach(GUI_Dialogue_Call Dialogue in World3Levels)
	// 	// {
	// 	// 	Dialogue.Close();
	// 	// }

	// 	// foreach(GUI_Dialogue_Call Dialogue in World4Levels)
	// 	// {
	// 	// 	Dialogue.Close();
	// 	// }

		

	// 	//close unlockables Dialogue list

	// 	//close multipalyer Dialogue list
	}

	public void Quit()
	{
		Application.Quit();
	}
}
