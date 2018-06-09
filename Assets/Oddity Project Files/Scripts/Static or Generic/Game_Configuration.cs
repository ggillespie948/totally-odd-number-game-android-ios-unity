using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game_Configuration : MonoBehaviour {

	[Header("GUI")]
	public int theme;
	
	[Header("Game Selection Itmes")]
	public TextMeshProUGUI titleTxt;
	public TextMeshProUGUI gridSizeTxt;
	public TextMeshProUGUI aiPlayersTxt;
	public TextMeshProUGUI humanPlayersTxt;
	public TextMeshProUGUI aiDifficultyTxt;
	public TextMeshProUGUI turnTimeTxt;
	public starFxController starFx;

	public TextMeshProUGUI objectiveText1;
	public TextMeshProUGUI objectiveText2;
	public TextMeshProUGUI objectiveText3;

	public GameObject objectivePanel;
	public GameObject configPanel;

	public GameObject objStar1;
	public GameObject objStar2;
	public GameObject objStar3;

	[Header("Game Configuration Settings")]
	public bool challengeMode;
	public bool puzzleMode;
	public string levelTitle;
	public string levelCode;
	public int levelNo;
	public int worldNo;
	public int gridSize;
	public int ai_opponents;
	public int ai_difficulty;
	public int human_players =1;
	public int turnTime;
	public int maxTile;
	public int oneTiles;
	public int twoTiles;
	public int threeTiles;
	public int fourTiles;
	public int fiveTiles;
	public int sixTiles;
	public int sevenTiles;
	public int targetScore;

	public int targetScore2;
	public int targetScore3;

	public int rewardCoins;

	public int rewardLives;

	public string rewardSpecial;

	public GameObject tile1;
	public GameObject tile2;
	public GameObject tile3;
	public GameObject tile4;
	public GameObject tile5;
	public GameObject tile6;
	public GameObject tile7;

	[Header("Game Objective Items")]
	public string objective1Code;
	public string objective2Code;
	public string objective3Code;
	private List<GameObject> tiles = new List<GameObject>();
	[Header("Star Requirement (optional)")]
	public int starRequirement = 0;



	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		tiles.Add(tile1);
		tiles.Add(tile2);
		tiles.Add(tile3);
		tiles.Add(tile4);
		tiles.Add(tile5);
		tiles.Add(tile6);
		tiles.Add(tile7);

		if(AccountInfo.playfabId != null)
		{
			if(challengeMode)
			{
				if(worldNo >= 0)
				{
					Debug.Log("wordl no  " + worldNo);
					Debug.Log("level no  " + levelNo);
					Debug.LogWarning("wordlstars level,world:  " + AccountInfo.worldStars[worldNo,levelNo]);
					
					if(AccountInfo.worldStars[worldNo, levelNo][0]=='1')
						if(objStar1 != null){objStar1.SetActive(true);}
					else
						if(objStar1 != null){objStar1.SetActive(false);}

					if(AccountInfo.worldStars[worldNo, levelNo][1]=='1')
						if(objStar2 != null){objStar2.SetActive(true);}
					else
						if(objStar2 != null){objStar2.SetActive(false);}

					if(AccountInfo.worldStars[worldNo, levelNo][2]=='1')
						if(objStar3 != null){objStar3.SetActive(true);}
					else
						if(objStar3 != null){objStar3.SetActive(false);}

					if(starRequirement>(AccountInfo.beginnerStars+AccountInfo.intermediateStars+AccountInfo.advancedStars))
					{
						//Image[] imgs = GetComponentsInChildren<Image>();
						Button[] btns = GetComponentsInChildren<Button>();
						GetComponentInChildren<TextMeshProUGUI>().enableAutoSizing=true;
						GetComponentInChildren<TextMeshProUGUI>().text="Requires " + starRequirement.ToString();
						// foreach(Image img in imgs)
						// {
						// 	img.gameObject.SetActive(true);
						// 	img.CrossFadeAlpha(0.35f, 1f, false);
						// }

						foreach(Button btn in btns)
						{
							btn.interactable=false;
						}
					}
				}
			}
		}
	}

	public void InitaliseLevelSelection()
	{
		objectivePanel.SetActive(false);
		configPanel.SetActive(true);
		
		gridSizeTxt.text = "Grid: "+gridSize+"x"+gridSize;

		if(challengeMode)
			if(humanPlayersTxt != null ){humanPlayersTxt.gameObject.SetActive(false);}
		

		if(humanPlayersTxt != null ){humanPlayersTxt.text="Players: " + human_players;}

		if(targetScore == 0)
		{
			aiPlayersTxt.text = "AI Opponents: " +ai_opponents;
			aiDifficultyTxt.text = "AI Difficulty: " + ai_difficulty + "/100";
			turnTimeTxt.text = "Turn Time: "+turnTime+"s";
		} else {
			aiPlayersTxt.text = "Target: " +targetScore;
			aiDifficultyTxt.text = "";
			turnTimeTxt.text = "";
		}
		
		if(oneTiles == 0)
			tile1.SetActive(false);
		else
			tile1.SetActive(true);

		if(twoTiles == 0)
			tile2.SetActive(false);
		else
			tile2.SetActive(true);

		if(threeTiles == 0)
			tile3.SetActive(false);
		else
			tile3.SetActive(true);

		if(fourTiles == 0)
			tile4.SetActive(false);
		else
			tile4.SetActive(true);

		if(fiveTiles == 0)
			tile5.SetActive(false);
		else
			tile5.SetActive(true);

		if(sixTiles == 0)
			tile6.SetActive(false);
		else
			tile6.SetActive(true);

		if(sevenTiles == 0)
			tile7.SetActive(false);
		else
			tile7.SetActive(true);

		
		
	}

	public void InitaliseLevelObjectives()
	{
		objectivePanel.SetActive(true);
		configPanel.SetActive(false);
		titleTxt.text = levelTitle;
		objectiveText1.text=GenerateObjectiveText(objective1Code);
		objectiveText2.text=GenerateObjectiveText(objective2Code);
		objectiveText3.text=GenerateObjectiveText(objective3Code);

		if(AccountInfo.worldStars[worldNo, levelNo][0]=='1')
		objStar1.SetActive(true);
		else
		objStar1.SetActive(false);

		if(AccountInfo.worldStars[worldNo, levelNo][1]=='1')
		objStar2.SetActive(true);
		else
		objStar2.SetActive(false);

		if(AccountInfo.worldStars[worldNo, levelNo][2]=='1')
		objStar3.SetActive(true);
		else
		objStar3.SetActive(false);
	}

	public void StartLevel()
	{
		ApplicationModel.LEVEL_NO = levelNo;
		ApplicationModel.WORLD_NO = worldNo;
		ApplicationModel.LEVEL_CODE = levelCode;
		ApplicationModel.ONE_TILES = oneTiles;
		ApplicationModel.TWO_TILES = twoTiles;
		ApplicationModel.THREE_TILES = threeTiles;
		ApplicationModel.FOUR_TILES = fourTiles;
		ApplicationModel.FIVE_TILES = fiveTiles;
		ApplicationModel.SIX_TILES = sixTiles;
		ApplicationModel.SEVEN_TILES = sevenTiles;
		ApplicationModel.AI_PLAYERS=ai_opponents;
		ApplicationModel.HUMAN_PLAYERS=1;
		ApplicationModel.GRID_SIZE=gridSize;
		ApplicationModel.TURN_TIME=turnTime;
		ApplicationModel.TUTORIAL_MODE=false;
		ApplicationModel.HUMAN_PLAYERS=human_players;
		ApplicationModel.Objective1Code=objective1Code;
		ApplicationModel.Objective2Code=objective2Code;
		ApplicationModel.Objective3Code=objective3Code;

		if(challengeMode)
			ApplicationModel.RETURN_TO_WORLD=worldNo;
		else
			ApplicationModel.RETURN_TO_WORLD=-1;

		if(targetScore ==0)
		{
			MenuController.instance.StartGameAI(ai_difficulty);
		} else {
			ApplicationModel.TARGET = targetScore;
			ApplicationModel.TARGET2 = targetScore2;
			ApplicationModel.TARGET3 = targetScore3;
			MenuController.instance.SoloPlay();
		}
	}

	public void LoadConfiguration(Game_Configuration config)
	{
		if(challengeMode)
			GUI_Controller.instance.NavBar.CloseAllDialogues(true);

		levelCode = config.levelCode;
		ai_difficulty = config.ai_difficulty;
		levelTitle = config.levelTitle;
		levelNo = config.levelNo;
		worldNo = config.worldNo;
		gridSize = config.gridSize;
		human_players = config.human_players;
		ai_opponents = config.ai_opponents;
		turnTime = config.turnTime;
		maxTile = config.maxTile;
		oneTiles = config.oneTiles;
		twoTiles = config.twoTiles;
		threeTiles = config.threeTiles;
		fourTiles = config.fourTiles;
		fiveTiles = config.fiveTiles;
		sixTiles = config.sixTiles;
		sevenTiles = config.sevenTiles;
		targetScore = config.targetScore;
		targetScore2 = config.targetScore2;
		targetScore3 = config.targetScore3;
		objective1Code = config.objective1Code;
		objective2Code = config.objective2Code;
		objective3Code = config.objective3Code;
		gameObject.SetActive(true);
		InitaliseLevelObjectives();

	}

	/// <summary>
	/// Method which increments the grid size of a game config dialogue
	/// </summary>
	public void IncrementGridSize()
	{
		if(gridSize <= 7)
			gridSize+=2;
		InitaliseLevelSelection();
	}

	/// <summary>
	/// Method which decrements the grid size of a game config dialogue
	/// </summary>
	public void DecrementGridSize()
	{
		if(gridSize >= 7)
			gridSize-=2;
		InitaliseLevelSelection();
	}

	/// <summary>
	/// Method which increments the grid size of a game config dialogue
	/// </summary>
	public void IncrementTurnTime()
	{
		turnTime+=5;
		InitaliseLevelSelection();
	}

	/// <summary>
	/// Method which decrements the grid size of a game config dialogue
	/// </summary>
	public void DecrementTurnTime()
	{
		if(turnTime >= 10)
			turnTime-=5;
		InitaliseLevelSelection();
	}
	
	/// <summary>
	/// Method which decrements the number of ai_players of a game configuration dialogue
	/// </summary>
	public void IncrementAiPlayers()
	{
		if(ai_opponents <= 3)
		{
			if(human_players+ai_opponents+1 <=4)
				ai_opponents++;
		}
		InitaliseLevelSelection();
	}

	/// <summary>
	/// Method which decrements the number of ai_players of a game configuration dialogue
	/// </summary>
	public void DecrementAiPlayers()
	{
		if(ai_opponents >= 1)
			ai_opponents--;
		InitaliseLevelSelection();
	}

	/// <summary>
	/// Method which decrements the number of players of a game configuration dialogue
	/// </summary>
	public void IncrementPlayers()
	{
		if(human_players <= 3)
		{
			human_players++;
			if(human_players+ai_opponents >4)
				ai_opponents--;
		}
		InitaliseLevelSelection();
	}

	public void DecrementPlayers()
	{
		if(human_players >= 2)
		{
			human_players--;
		}
		InitaliseLevelSelection();
	}

	/// <summary>
	/// Method which incrmenets the tile set of a game configuration dialogue
	/// </summary>
	public void IncrementDifficulty()
	{
		if(ai_difficulty<=90)
		{
			ai_difficulty+=10;
		}
		InitaliseLevelSelection();
	}


	/// <summary>
	///  Method whcih decrements the tile set of a game config dialogue
	/// </summary>
	public void DecrementDifficulty()
	{
		if(ai_difficulty>=20)
		{
			ai_difficulty-=10;
		}
		InitaliseLevelSelection();
	}

	/// <summary>
	/// Method which incrmenets the tile set of a game configuration dialogue
	/// </summary>
	public void IncrementTiles()
	{
		if(maxTile <= 6)
			maxTile++;
		GenerateTileCounts();
		InitaliseLevelSelection();
	}


	/// <summary>
	///  Method whcih decrements the tile set of a game config dialogue
	/// </summary>
	public void DecrementTiles()
	{
		if(maxTile >= 4)
			maxTile--;
		GenerateTileCounts();
		InitaliseLevelSelection();
	}

	/// <summary>
	/// Method which generates default tile counts based on the value of maxTile
	/// Primarily used when adjusting custom-game game config values
	/// </summary>
	private void GenerateTileCounts()
	{
		switch(maxTile)
		{
			case 3:
				oneTiles=gridSize*(8-maxTile)+9;
				twoTiles=gridSize*(8-maxTile)+9;
				threeTiles=gridSize*(8-maxTile)+9;
				fourTiles=0;
				fiveTiles=0;
				sixTiles=0;
				sevenTiles=0;
			break;

			case 4:
				oneTiles=gridSize*(8-maxTile)+9;
				twoTiles=gridSize*(8-maxTile)+9;
				threeTiles=gridSize*(8-maxTile)+9;
				fourTiles=gridSize*(8-maxTile)+9;
				fiveTiles=0;
				sixTiles=0;
				sevenTiles=0;
			break;

			case 5:
				oneTiles=gridSize*(8-maxTile)+9;
				twoTiles=gridSize*(8-maxTile)+9;
				threeTiles=gridSize*(8-maxTile)+9;
				fourTiles=gridSize*(8-maxTile)+9;
				fiveTiles=gridSize*(8-maxTile)+9;
				sixTiles=0;
				sevenTiles=0;
			break;

			case 6:
				oneTiles=gridSize*(8-maxTile)+9;
				twoTiles=gridSize*(8-maxTile)+9;
				threeTiles=gridSize*(8-maxTile)+9;
				fourTiles=gridSize*(8-maxTile)+9;
				fiveTiles=gridSize*(8-maxTile)+9;
				sixTiles=gridSize*(8-maxTile)+9;
				sevenTiles=0;
			break;

			case 7:
				oneTiles=gridSize*(8-maxTile)+9;
				twoTiles=gridSize*(8-maxTile)+9;
				threeTiles=gridSize*(8-maxTile)+9;
				fourTiles=gridSize*(8-maxTile)+9;
				fiveTiles=gridSize*(8-maxTile)+9;
				sixTiles=gridSize*(8-maxTile)+9;
				sevenTiles=gridSize*(8-maxTile)+9;
			break;
		}

	}

	public void ReturnToWorldSelection()
	{
		GUI_Controller.instance.NavBar.challengeModeDialogue.GetComponent<ChallengeModeController>().SelectWorld(worldNo);
	}

	public string GenerateObjectiveText(string objCode)
	{
		string[] ret = objCode.Split('.');

		switch(ret[0])
		{
			case "Win":
				return "Win the game";

			case "WinBy":
				return "Win by "+ret[1]+" points";

			case "Fill":
				return "Fill the game grid completely";

			case "BestTurnScore":
				return "Finish with the best turn score";

			case "TurnScore":
				return "Score " + ret[1] + " or more in a single turn";

			case "TurnScoreExact":
				return "Score exactly " + ret[1] + " in a single turn";

			case "MostTiles":
				return "Play the most tiles in the game";

			case "PlayTiles":
				return "Play " + ret[1] + " or more tiles in the game";

			case "Score":
				return "Score " + ret[1] + " or more";

			case "Errors":
				return "Finish with " + ret[1] + " errors or less";

			case "ErrorsWin":
				return "Win with  " + ret[1] + " errors or less";

			case "ErrorsMore":
				return "Win with  " + ret[1] + " errors or more";

			case "Odd":
				return "Win the game with an odd score";

			case "Even":
				return "Win the game with an even score";

			case "Activate":
				return "Activate " + ret[0] + " tiles in a single turn";

			case "RunnerUp":
				return "Finish 2nd or better in the game";

			default:
				return "404: Object code unrecognised";

		}

	}
}

