using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChallengeModeController : MonoBehaviour {

	[Header("Challenege Mode Dialogues")]
	[SerializeField]
	private NavigationBarController navBar;

	[SerializeField]
	public ParticleSystem selectionFX;

	

	
	[Header("Game Configurations")]
	[SerializeField]
	private Game_Configuration[] beginnerGameConfigurations;
	[SerializeField]
	private Game_Configuration[] noviceGameConfigurations;
	[SerializeField]
	private Game_Configuration[] intermediateGameConfigurations;
	[SerializeField]
	private Game_Configuration[] adeptGameConfigurations;
	[SerializeField]
	private Game_Configuration[] advancedGameConfigurations;
	[SerializeField]
	private Game_Configuration[] expertGameConfigurations;
	[SerializeField]
	private Game_Configuration[] masterGameConfigurations;
	[SerializeField]
	private Game_Configuration[] targetMasterGameConfigurations;
	[SerializeField]
	private Game_Configuration[] grandMasterGameConfigurations;
	[SerializeField]
	private Game_Configuration[] legendGameConfigurations;
	[SerializeField]
	public List<Color> worldColourThemes;

	[Header("Select Level")]
	[SerializeField]
	private Image headerBg;
	[SerializeField]
	private TextMeshProUGUI headerTitle;
	[SerializeField]
	private GameObject worldSelector; //scroll view of 10 level selections

	private List<Game_Configuration[]> gameConfigurations;
	[SerializeField]
	public GameObject worldScrollView;

	[SerializeField]
	public GameObject[] levelHeaders;
	[SerializeField]
	public Game_Configuration[] levelSelections;
	
	
	[SerializeField]
	private GameObject levelSelector;


	[Header("Select World")]
	[SerializeField]
	private List<GameObject> worldBoxes;

	[SerializeField]
	private List<Image> worldCrowns;
	void Awake()
	{
		InitGameConfigs();
	}

	public void CloseAllDialogues()
	{
		for(int i=0; i<levelHeaders.Length; i++)
		{
			if(levelHeaders[i] != null)
				levelHeaders[i].SetActive(false);
		}
		worldScrollView.SetActive(false);
		worldSelector.SetActive(false);
		levelSelector.SetActive(false);
		MenuController.instance.fireWorks.SetActive(false);
	}

	public void CheckWorldForStarImprovement()
	{
		//Debug.Log("Checking for star improvement.. ");
		Component[] configs =worldScrollView.GetComponentsInChildren<Game_Configuration>();
		foreach(Game_Configuration config in configs)
		{
			config.CheckStarReq();
		}
	}


		private bool tutFlag1 = false;
	public void OpenChallenegeMode()
	{
		MenuController.instance.NavBar.challengeModeDialogue.GetComponent<Animator>().SetTrigger("hide");
		CloseAllDialogues();
		selectionFX.Play();
		worldSelector.SetActive(true);
		LoadWorldData();
		//Invoke("HidePlayerPanel", .05f);

		if(ApplicationModel.TUTORIAL_MODE && ApplicationModel.RETURN_TO_WORLD==-2 && !tutFlag1)
		{
			if(Tutorial_Controller.instance.tutorialIndex==16)
				Tutorial_Controller.instance.OnMouseDown();

			Debug.Log("TUTORIAL INDEX ::::::::" + Tutorial_Controller.instance.tutorialIndex);
			tutFlag1=true;
			Tutorial_Controller.instance.OnMouseDown();
			MenuController.instance.CurrencyUI.ResetInformationPanelAfterDelay(MenuController.instance.CurrencyUI.starInfoPanel, .87f);
			Tutorial_Controller.instance.GetComponent<BoxCollider2D>().enabled=false;
			Tutorial_Controller.instance.singleplayerBtn.GetComponent<BoxCollider>().enabled=false;
		}
	}

	private void HidePlayerPanel()
	{
		navBar.playerPanel.SetActive(false);
	}

	public void InitGameConfigs()
	{
		//Debug.LogWarning("Challenge mode controller awake");
		gameConfigurations=new List<Game_Configuration[]>();
		gameConfigurations.Add(beginnerGameConfigurations);
		gameConfigurations.Add(noviceGameConfigurations);
		gameConfigurations.Add(intermediateGameConfigurations);
		gameConfigurations.Add(adeptGameConfigurations);
		gameConfigurations.Add(advancedGameConfigurations);
		gameConfigurations.Add(expertGameConfigurations);
		gameConfigurations.Add(masterGameConfigurations);
		gameConfigurations.Add(targetMasterGameConfigurations);
		gameConfigurations.Add(grandMasterGameConfigurations);
		gameConfigurations.Add(legendGameConfigurations);
		//Debug.LogWarning("game configurations length post awake " + gameConfigurations.Count);
	}

	public  bool tutFlag2=false;
	
	public void SelectWorld(int worldNo)
	{

		if((!AccountInfo.Instance.InventoryContainsItemClass(AccountInfo.ITEM_LEVELPACK)) && worldNo > 3 )
		{
			MenuController.instance.LevelPassRequired();
			return;
		}

		if(!tutFlag2)
			tutFlag2=true;

		CloseAllDialogues();
		navBar.CloseAllDialogues(true);
		AdjustLevelHeader(worldNo);
		levelHeaders[worldNo].SetActive(true);
		

		//tutorial instructions
		if (ApplicationModel.TUTORIAL_MODE && ApplicationModel.RETURN_TO_WORLD == -2)
		{
			Debug.LogError("Press Select World:  TUT INDX:" + Tutorial_Controller.instance.tutorialIndex);
			Tutorial_Controller.instance.OnMouseDown();
			if(Tutorial_Controller.instance.tutorialIndex<26)
				Tutorial_Controller.instance.OnMouseDown();
			tutFlag2=true;
		}

		//Debug.LogWarning("Game confiurations length: " + gameConfigurations[worldNo].Length);		

		for(int i=0; i<10; i++)
		{
			levelSelections[i].LoadConfiguration(gameConfigurations[worldNo][i]);
			levelSelections[i].colour.color=worldColourThemes[worldNo];
		}

		for(int i=0; i<10; i++)
		{
			for(int o=0;o<19;o++ )
			{
				levelSelections[i].startTileCounts[o]=gameConfigurations[worldNo][i].startTileCounts[o];
			}
		}
		CheckWorldForStarImprovement();
		worldScrollView.SetActive(true);

		switch(worldNo)
		{
			case 0:
			
			if(AccountInfo.beginnerStars == 30)
				MenuController.instance.fireWorks.SetActive(true);
			else
				MenuController.instance.fireWorks.SetActive(false);
			
			break;

			case 1:
			
			if(AccountInfo.noviceStars == 30)
				MenuController.instance.fireWorks.SetActive(true);
				else
				MenuController.instance.fireWorks.SetActive(false);
			break;

			case 2:
			
			if(AccountInfo.intermediateStars == 30)
			MenuController.instance.fireWorks.SetActive(true);
			else
				MenuController.instance.fireWorks.SetActive(false);
			break;

			case 3:
			
			if(AccountInfo.adeptStars == 30)
			MenuController.instance.fireWorks.SetActive(true);
			else
				MenuController.instance.fireWorks.SetActive(false);
			break;
			
			case 4:
			
			if(AccountInfo.advancedStars == 30)
			MenuController.instance.fireWorks.SetActive(true);
			else
				MenuController.instance.fireWorks.SetActive(false);
			break;

			case 5:
			
			if(AccountInfo.expertStars == 30)
			MenuController.instance.fireWorks.SetActive(true);
			else
				MenuController.instance.fireWorks.SetActive(false);
			break;

			case 6:

			if(AccountInfo.masterStars == 30)
			MenuController.instance.fireWorks.SetActive(true);
			else
				MenuController.instance.fireWorks.SetActive(false);
			break;

			case 7:
			
			if(AccountInfo.targetMasterStars == 30)
			MenuController.instance.fireWorks.SetActive(true);
			else
				MenuController.instance.fireWorks.SetActive(false);
			break;

			case 8:
			
			if(AccountInfo.grandMasterStars == 30)
			MenuController.instance.fireWorks.SetActive(true);
			else
				MenuController.instance.fireWorks.SetActive(false);
			break;

			case 9:
			
			if(AccountInfo.legendStars == 30)
			MenuController.instance.fireWorks.SetActive(true);
			else
				MenuController.instance.fireWorks.SetActive(false);
			break;

		}
	}

	private void AdjustLevelHeader(int worldNo)
	{
		float aspect = Camera.main.aspect;

		if(aspect < 0.47f)
		{
			//Debug.Log("Set Xtra Slim Resolution Default"); //iphone X
				levelHeaders[worldNo].GetComponent<HorizontalLayoutGroup>().spacing=-200f;
				levelHeaders[worldNo].GetComponent<HorizontalLayoutGroup>().padding.left=0;

		} else if (aspect < 0.51f)
		{
			//Debug.Log("Set  Slim Resolution Default"); //s8 s9
			levelHeaders[worldNo].GetComponent<HorizontalLayoutGroup>().spacing=-190;
				levelHeaders[worldNo].GetComponent<HorizontalLayoutGroup>().padding.left=-10;

		} else if (aspect < 0.65f)
		{
			//Debug.Log("Set Default Resolution Default"); //s6 s7
			levelHeaders[worldNo].GetComponent<HorizontalLayoutGroup>().spacing=-150;
				levelHeaders[worldNo].GetComponent<HorizontalLayoutGroup>().padding.left=-25;

		} else if (aspect > 0.7f)
		{
			//Debug.Log("Set Wide Resolution Default");
			levelHeaders[worldNo].GetComponent<HorizontalLayoutGroup>().spacing=-170;
			levelHeaders[worldNo].GetComponent<HorizontalLayoutGroup>().padding.left=-10;

		} else 
		{
			//Debug.Log("Set Default Resolution Default");
			levelHeaders[worldNo].GetComponent<HorizontalLayoutGroup>().spacing=-150;
			levelHeaders[worldNo].GetComponent<HorizontalLayoutGroup>().padding.left=-25;

		}

	}

	public void LoadWorldData()
	{
		worldBoxes[0].GetComponentInChildren<TextMeshProUGUI>().text="Beginner " + AccountInfo.beginnerStars.ToString() + "/30";
		worldBoxes[1].GetComponentInChildren<TextMeshProUGUI>().text="Novice " + AccountInfo.noviceStars.ToString() + "/30";
		worldBoxes[2].GetComponentInChildren<TextMeshProUGUI>().text="Intermediate " + AccountInfo.intermediateStars.ToString() + "/30";
		worldBoxes[3].GetComponentInChildren<TextMeshProUGUI>().text="Adept " + AccountInfo.adeptStars.ToString() + "/30";
		worldBoxes[4].GetComponentInChildren<TextMeshProUGUI>().text="Advanced " + AccountInfo.advancedStars.ToString() + "/30";
		worldBoxes[5].GetComponentInChildren<TextMeshProUGUI>().text="Expert " + AccountInfo.expertStars.ToString() + "/30";
		worldBoxes[6].GetComponentInChildren<TextMeshProUGUI>().text="Master " + AccountInfo.masterStars.ToString() + "/30";
		worldBoxes[7].GetComponentInChildren<TextMeshProUGUI>().text="Target Master " + AccountInfo.targetMasterStars.ToString() + "/30";
		worldBoxes[8].GetComponentInChildren<TextMeshProUGUI>().text="Grand Master " + AccountInfo.grandMasterStars.ToString() + "/30";
		worldBoxes[9].GetComponentInChildren<TextMeshProUGUI>().text="Legend " + AccountInfo.legendStars.ToString() + "/30";
	}

	
}
