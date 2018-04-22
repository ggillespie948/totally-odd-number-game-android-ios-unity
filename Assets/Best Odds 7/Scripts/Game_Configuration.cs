using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

	[Header("Game Configuration Settings")]
	public bool challengeMode;
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

	public GameObject tile1;
	public GameObject tile2;
	public GameObject tile3;
	public GameObject tile4;
	public GameObject tile5;
	public GameObject tile6;
	public GameObject tile7;
	private List<GameObject> tiles = new List<GameObject>();


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
				if(starFx != null && worldNo >= 0)
				{
					Debug.Log("wordl no  " + worldNo);
					Debug.Log("level no  " + levelNo);
					Debug.Log("wordlstars level,world:  " + AccountInfo.worldStars[worldNo,levelNo]);
					starFx.ea=AccountInfo.worldStars[worldNo,levelNo];
					starFx.enabled=true;
				}
			}
		}
	}

	public void InitaliseLevelSelection()
	{
		titleTxt.text = levelTitle;
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

		if(challengeMode && worldNo >= 0)
		{
			starFx.Reset();
			starFx.ea=AccountInfo.worldStars[worldNo,levelNo];
		}
		
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
		ApplicationModel.RETURN_TO_WORLD=worldNo;
		ApplicationModel.HUMAN_PLAYERS=human_players;
		ApplicationModel.THEME=theme;

		if(targetScore ==0)
		{
			MenuController.instance.StartGameAI(ai_difficulty);
		} else {
			ApplicationModel.TARGET = targetScore;
			MenuController.instance.SoloPlay();
		}
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
		if(human_players < 3)
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
				oneTiles=gridSize*(8-maxTile);
				twoTiles=gridSize*(8-maxTile);
				threeTiles=gridSize*(8-maxTile);
				fourTiles=0;
				fiveTiles=0;
				sixTiles=0;
				sevenTiles=0;
			break;

			case 4:
				oneTiles=gridSize*(8-maxTile);
				twoTiles=gridSize*(8-maxTile);
				threeTiles=gridSize*(8-maxTile);
				fourTiles=gridSize*(8-maxTile);
				fiveTiles=0;
				sixTiles=0;
				sevenTiles=0;
			break;

			case 5:
				oneTiles=gridSize*(8-maxTile);
				twoTiles=gridSize*(8-maxTile);
				threeTiles=gridSize*(8-maxTile);
				fourTiles=gridSize*(8-maxTile);
				fiveTiles=gridSize*(8-maxTile);
				sixTiles=0;
				sevenTiles=0;
			break;

			case 6:
				oneTiles=gridSize*(8-maxTile);
				twoTiles=gridSize*(8-maxTile);
				threeTiles=gridSize*(8-maxTile);
				fourTiles=gridSize*(8-maxTile);
				fiveTiles=gridSize*(8-maxTile);
				sixTiles=gridSize*(8-maxTile);
				sevenTiles=0;
			break;

			case 7:
				oneTiles=gridSize*(8-maxTile);
				twoTiles=gridSize*(8-maxTile);
				threeTiles=gridSize*(8-maxTile);
				fourTiles=gridSize*(8-maxTile);
				fiveTiles=gridSize*(8-maxTile);
				sixTiles=gridSize*(8-maxTile);
				sevenTiles=gridSize*(8-maxTile);
			break;
		}

	}
}

