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

	void Awake()
	{
		InitGameConfigs();
	}

	public void CloseAllDialogues()
	{
		for(int i=0; i<levelHeaders.Length; i++)
		{
			if(levelHeaders[i].activeSelf)
				levelHeaders[i].SetActive(false);
		}
		worldScrollView.SetActive(false);
		worldSelector.SetActive(false);
		levelSelector.SetActive(false);
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

	public void OpenChallenegeMode()
	{
		MenuController.instance.NavBar.challengeModeDialogue.GetComponent<Animator>().SetTrigger("hide");
		CloseAllDialogues();
		selectionFX.Play();
		worldSelector.SetActive(true);
		LoadWorldData();
		//Invoke("HidePlayerPanel", .05f);
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

	public void SelectWorld(int worldNo)
	{

		if((!AccountInfo.Instance.InventoryContainsItemClass(AccountInfo.ITEM_LEVELPACK)) && worldNo > 3 )
		{
			MenuController.instance.LevelPassRequired();
			return;
		}

		CloseAllDialogues();
		navBar.CloseAllDialogues(true);
		levelHeaders[worldNo].SetActive(true);

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
