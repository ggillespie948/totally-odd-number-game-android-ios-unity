using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;

public class AccountInfo : MonoBehaviour {

	//Singleton Instance
	private static AccountInfo instance;

	public static AccountInfo Instance
	{
		get {return instance;}
		set { instance = value;}
	}

	public GameObject connectionErrorPanel;

	[SerializeField]
	public static string[,] worldStars;
	public static string playfabId;

	public static int beginnerStars;
	public static int noviceStars;
	public static int intermediateStars;
	public static int advancedStars;
	public static int masterStars;
	public static int grandMasterStars;

	public static int totalStars;

	public static string tileUnlockString;

	[SerializeField]
	private GetPlayerCombinedInfoResultPayload info;

	public GetPlayerCombinedInfoResultPayload Info
	{
		get { return info;}
		set { info = value;}
	}

	// STATIC IDENTIFIERS
	public static string COINS_CODE  = "PC";
	public static string GEMS_CODE  = "PG";
	public static string LIVES_CODE  = "PL";
	public static string DATA_STARS  = "DS";


	public static string CATALOG_ITEMS = "Items";
	public static string ITEM_TILESKIN = "Tile Skin";

	public static string ITEM_LIFEPASS = "Life Pass";
	public static string ITEM_LIFEREFILL = "Life Refill";
	public static string ITEM_COINPACK = "Coin Pack";
	public static string ITEM_LEVELPACK = "Level Pack";

	public static int ITEM_COST =1;
	public static int ITEM_PREFAB =3;
	public static int ITEM_ICON =5;
	public static int ITEM_STARREQ =7;
	public static int UNLOCKNO =9;

	public static int TILESKIN =0;
	public static int GRIDSKIN =0;
	public static int THEME =0;

	public List<ItemInstance> inv_items = new List<ItemInstance>();


	//Player Statistics
	[SerializeField]
	public static int WIN_STREAK{get; private set;}
	[SerializeField]
	public static int DAILY_TILES_PLAYED{get; private set;}
	[SerializeField]
	public static int DAILY_WINS{get; private set;} 
	[SerializeField]
	public static int DAILY_STARS{get; private set;}

	// Player Daily Challenge Data
	[SerializeField]
	public static int CURRENT_CHALLENGE_NO{get; private set;}

	[SerializeField]
	public static string DAILY_CHALLENGE_CODE;
	
		

	
	private void Awake()
	{
		if(instance !=this)
			instance = this;

		DontDestroyOnLoad(gameObject);

		worldStars = new string[6,11]; // temp - hard coded the number of worlds/levels
	}

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		//MenuController.instance.loadingPanel.SetActive(true);
		Debug.Log("Account info initial login");
		Login();
		
	}

	public static void RetryLogin()
	{
		Debug.Log("Retrying login..");
		Login();
	}

	public static int TotalStars()
	{
		return (beginnerStars+noviceStars+intermediateStars+advancedStars+masterStars+grandMasterStars);
	}

	public static void Login()
	{
		 PlayFabSettings.TitleId = "FCE"; // Title ID of the Playfab BAAS
		if (Application.platform == RuntimePlatform.Android)
		{
			var request = new LoginWithAndroidDeviceIDRequest 
			{
				TitleId = PlayFabSettings.TitleId,
				AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
				CreateAccount = true
			};
			PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnLoginSuccess, OnLoginFailure);
		}
		else if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			print("iOS Login");
		}
		else
		{
			var request = new LoginWithCustomIDRequest { CustomId = "E4A0DA3093A5FD3A", CreateAccount = true};
			PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
		}
	}

	private static void OnLoginSuccess(LoginResult result)
	{
		Debug.LogWarning("Login Successful");
		if(result.NewlyCreated)
		{
			SetUpAccount();
		} else 
		{
			GetPlayerData(result.PlayFabId);
		}
		MenuController.instance.navHighlight.SetActive(true);
		Instance.LoadLeaderboard("Wins");
		//GetAccountInfo(result.PlayFabId);
		Instance.GetInventory();
		
		playfabId = result.PlayFabId;

		Scene scene = SceneManager.GetActiveScene();
		if(scene.name != "Main")
		{
			AccountInfo.instance.connectionErrorPanel.SetActive(false);
		}
		
		
	}

	private static void OnLoginFailure(PlayFabError error)
	{
		Debug.LogWarning("Something went wrong with Login.  :(");
		Debug.LogError("Debug information:");
		Debug.LogError(error.GenerateErrorReport());

		GUI_Controller.instance.menuDebugTxt.text += " Something went wrong with login..."; // temp - change this to GameMenuMaster
		GUI_Controller.instance.menuDebugTxt.text += " Debug info: ";
		GUI_Controller.instance.menuDebugTxt.text += error.GenerateErrorReport();

		Scene scene = SceneManager.GetActiveScene();
		if(scene.name != "Main")
		{
			AccountInfo.instance.connectionErrorPanel.SetActive(true);
		}
		
	}

	public static void GetPlayerStatistics()
	{
		GetPlayerStatisticsRequest request = new GetPlayerStatisticsRequest()
		{
		};

		PlayFabClientAPI.GetPlayerStatistics(request, OnPlayerStatisticsRequestSuccess, OnAPIError);
	}

	private static void OnPlayerStatisticsRequestSuccess(GetPlayerStatisticsResult result)
	{
		Debug.Log("Player Statistics Retrieved Successfully...");
		foreach(StatisticValue val in result.Statistics)
		{
			switch(val.StatisticName)
			{
				case "DailyStars":
				AccountInfo.WIN_STREAK=val.Value;
				break;

				case "DailyTiles":
				AccountInfo.DAILY_TILES_PLAYED=val.Value;
				break;

				case "DailyWins":
				AccountInfo.DAILY_WINS=val.Value;
				break;

				case "WinStreak":
				AccountInfo.WIN_STREAK=val.Value;
				break;
			}

		}
		Debug.Log("Player Statistics Updated.");
	}

	public static void GetAccountInfoFromID(string playfabId)
	{
		GetPlayerCombinedInfoRequestParams paramInfo = new GetPlayerCombinedInfoRequestParams()
		{
			GetTitleData = true,
			GetUserInventory = true,
			GetUserAccountInfo = true,
			GetUserVirtualCurrency = true,
			GetPlayerProfile = true,
			GetPlayerStatistics = true,
			GetUserData = true,
			GetUserReadOnlyData = true

		};

		GetPlayerCombinedInfoRequest request = new GetPlayerCombinedInfoRequest()
		{
			PlayFabId = playfabId,
			InfoRequestParameters = paramInfo

		};

		PlayFabClientAPI.GetPlayerCombinedInfo(request, OnAccountInfoSuccess, OnAPIError);
	}

	public static void GetAccountInfo()
	{
		GetPlayerCombinedInfoRequestParams paramInfo = new GetPlayerCombinedInfoRequestParams()
		{
			GetTitleData = true,
			GetUserInventory = true,
			GetUserAccountInfo = true,
			GetUserVirtualCurrency = true,
			GetPlayerProfile = true,
			GetPlayerStatistics = true,
			GetUserData = true,
			GetUserReadOnlyData = true

		};

		GetPlayerCombinedInfoRequest request = new GetPlayerCombinedInfoRequest()
		{
			PlayFabId = AccountInfo.playfabId,
			InfoRequestParameters = paramInfo

		};

		PlayFabClientAPI.GetPlayerCombinedInfo(request, OnAccountInfoSuccess, OnAPIError);
	}


	public static void OnAPIError(PlayFabError error)
	{
		Debug.LogError(error);
		Scene scene = SceneManager.GetActiveScene();
		if(scene.name != "Main")
		{
			AccountInfo.instance.connectionErrorPanel.SetActive(true);
		} else{
		}
	}

	static void OnAccountInfoSuccess(GetPlayerCombinedInfoResult result)
	{
		Debug.Log("Player Info Retrieved");
		if(Instance != null)
		{
			Instance.info = result.InfoResultPayload;
			UpdateUIContent(result.InfoResultPayload);
		} else {
			
		}

		GetDailyChallenge();

	}

	public static void UpdateUIContent(GetPlayerCombinedInfoResultPayload info)
	{
		Debug.Log("Updating UI content..");
		int res = -1;	
		if(AccountInfo.playfabId != null)
		{
			if(Instance.Info.UserVirtualCurrency.TryGetValue(AccountInfo.COINS_CODE, out res))
			{
				GUI_Controller.instance.CoinDialogue.GetComponentInChildren<TextMeshProUGUI>().text =  res.ToString();
				GUI_Controller.instance.CurrencyUI.playerCoins=res;
			}

			if(Instance.Info.UserVirtualCurrency.TryGetValue(AccountInfo.LIVES_CODE, out res))
			{
				GUI_Controller.instance.LivesDialogue.GetComponentInChildren<TextMeshProUGUI>().text =  res.ToString();
				GUI_Controller.instance.CurrencyUI.playerLives=res;
			}

			GUI_Controller.instance.StarDialogue.GetComponentInChildren<TextMeshProUGUI>().text = (beginnerStars+noviceStars+intermediateStars+advancedStars+masterStars+grandMasterStars).ToString();
			GUI_Controller.instance.CurrencyUI.playerStars=(beginnerStars+noviceStars+intermediateStars+advancedStars+masterStars+grandMasterStars);
			
		}

	}

	public static void DispalyID()
	{
		GUI_Controller.instance.PlayerCoins_Stone.GetComponentInChildren<TextMeshProUGUI>().text = Instance.info.AccountInfo.PlayFabId.ToString();
		
	}

	public static void SetUpAccount() {
		PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
			Data = new Dictionary<string, string>() {
				{"B_STARS", "000,000,000,000,000,000,000,000,000,000"},
				{"N_STARS", "000,000,000,000,000,000,000,000,000,000"},
				{"I_STARS", "000,000,000,000,000,000,000,000,000,000"},
				{"A_STARS", "000,000,000,000,000,000,000,000,000,000"},
				{"M_STARS", "000,000,000,000,000,000,000,000,000,000"},
				{"G_STARS", "000,000,000,000,000,000,000,000,000,000"},
				{"Theme", "0"},
				{"TileSkin", "0"},
				{"GridSkin", "0"},
				{"TileUnlockString", "0000000000000000"},
				{"CURRENT_CHALLENGE_VAL", "0"}
			}
		}, 
		result => Debug.Log("Successfully setup user data"),//SetUpAccount2(),
		error => {
			Debug.Log("Got error setting initial user data");
			Debug.Log(error.GenerateErrorReport());
		});
	}

	public static void GetTimeToFullEnergy()
	{
		

	}

	static void UpdateDataInfo(UpdateUserDataResult result)
	{
		Debug.Log("Updated Info");
		GetPlayerData(playfabId);
		//List<StatisticUpdate> stats = new List<StatisticUpdate>();

	}

	public static void AddInGameCurrency(int value)
	{
		AddUserVirtualCurrencyRequest request = new AddUserVirtualCurrencyRequest();
		request.VirtualCurrency = "PC";
		request.Amount = value;
		PlayFabClientAPI.AddUserVirtualCurrency(request, OnAddedInGameCurrency, OnAddGameCurrencyError);
	}

	public static void DeductEnergy(int value)
	{
		SubtractUserVirtualCurrencyRequest request = new SubtractUserVirtualCurrencyRequest();
		request.VirtualCurrency = AccountInfo.LIVES_CODE;
		request.Amount = value;
		PlayFabClientAPI.SubtractUserVirtualCurrency(request, OnSubtractedEnergySuccess, OnAddGameCurrencyError);
	}

	public static void OnSubtractedEnergySuccess(ModifyUserVirtualCurrencyResult result)
	{
		Debug.Log("Currency Subtracted Successfully");
		MenuController.instance.StartActiveGameConfiguration();
	}

	public static void OnSubtractedInGameCurrency(ModifyUserVirtualCurrencyResult result)
	{
		Debug.Log("Currency Subtracted Successfully");
	}

	public static void OnAddedInGameCurrency(ModifyUserVirtualCurrencyResult result)
	{
		Debug.Log("Currency Added Successfully!");
	}
		
	public static void OnAddGameCurrencyError(PlayFabError error)
	{
		Debug.LogError("Error adding/subtracting in game currency: " + error.Error + " " + error.ErrorMessage);
	}

	public static void GetPlayerData(string playfabId) {
		PlayFabClientAPI.GetUserData(new GetUserDataRequest() {
			PlayFabId = playfabId,
			Keys = null
		}, result => {
			Debug.Log("User Star Data Retrieved");
			if (result.Data == null)
				Debug.Log("User Data Null");
			else
			{
				if(!result.Data.ContainsKey("B_STARS"))
				{
					SetUpAccount();
				}

				Scene scene = SceneManager.GetActiveScene();
				if(scene.name != "Main")
				{
					//Parse Beginner Stars
					int beginnerCounter = 0;
					string beginnerStarString = result.Data["B_STARS"].Value;
					//tokenize star string data and allocate each token to worldStars[]
					// counting the total number of beginner stars during the proccessing
					string[] tokenResult = beginnerStarString.Split(',');
					int indx=0;
					foreach(string token in tokenResult)
					{
						worldStars[0,indx]=token;
						indx++;
						foreach(char c in token)
						{
							if(c=='1')
								beginnerCounter++;
						}
					}
					beginnerStars = beginnerCounter;

					//Parse Novice Stars
					int noviceCounter = 0;
					string noviceStarString = result.Data["N_STARS"].Value;
					//tokenize star string data and allocate each token to worldStars[]
					// counting the total number of beginner stars during the proccessing
					string[] tokenResultN = noviceStarString.Split(',');
					int indxN=0;
					foreach(string token in tokenResultN)
					{
						worldStars[1,indxN]=token;
						indxN++;
						foreach(char c in token)
						{
							if(c=='1')
								noviceCounter++;
						}
					}
					noviceStars = noviceCounter;

					//Parse Intermediate Stars
					int intermediateCounter = 0;
					string intermediateStarString = result.Data["I_STARS"].Value;
					string[] tokenResult2 = intermediateStarString.Split(',');
					int indx2=0;
					foreach(string token in tokenResult2)
					{
						worldStars[2,indx2]=token;
						indx2++;
						foreach(char c in token)
						{
							if(c=='1')
								intermediateCounter++;
						}
					}
					intermediateStars = intermediateCounter;

					//Parse Advanced Stars
					int advancedCounter = 0;
					string advancedStarString = result.Data["A_STARS"].Value;
					string[] tokenResult3 = advancedStarString.Split(',');
					int indx3=0;
					foreach(string token in tokenResult3)
					{
						worldStars[3,indx3]=token;
						indx3++;
						foreach(char c in token)
						{
							if(c=='1')
								advancedCounter++;
						}
					}
					advancedStars = advancedCounter;

					//Parse Master Stars
					int masterCounter = 0;
					string masterStarString = result.Data["M_STARS"].Value;
					string[] tokenResult4 = masterStarString.Split(',');
					int indx4=0;
					foreach(string token in tokenResult4)
					{
						worldStars[4,indx4]=token;
						indx4++;
						foreach(char c in token)
						{
							if(c=='1')
								masterCounter++;
						}
					}
					masterStars = masterCounter;

					//Parse Grand Master Stars
					int gmasterCounter = 0;
					string gmasterStarString = result.Data["G_STARS"].Value;
					string[] tokenResult5 = gmasterStarString.Split(',');
					int indx5=0;
					foreach(string token in tokenResult5)
					{
						worldStars[5,indx5]=token;
						indx5++;
						foreach(char c in token)
						{
							if(c=='1')
								gmasterCounter++;
						}
					}
					grandMasterStars = gmasterCounter;

					if(result.Data.ContainsKey("TileUnlockString"))
					{
						tileUnlockString=result.Data["TileUnlockString"].Value;
					}
					else
					{
						tileUnlockString="1000000000000000";
						UpdateTileSkinUnlockString();
					}


					Debug.Log("Setting cosmetic values...");
					TILESKIN=int.Parse(result.Data["TileSkin"].Value);
					ApplicationModel.TILESKIN=TILESKIN;
					GRIDSKIN=int.Parse(result.Data["GridSkin"].Value);
					THEME=int.Parse(result.Data["Theme"].Value);

					GetAccountInfo();
				}

				//Account Info Request From Main GAmePlay Scene
				GetAccountInfo();



			}
		}, (error) => {
			Debug.Log("Got error retrieving user data:");
			Debug.Log(error.GenerateErrorReport());
		});
	}

	public void UpdateTheme(int themeID)
	{
		PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
			Data = new Dictionary<string, string>() {
					{"Theme", themeID.ToString()}
				}
			}, 
			result => Debug.Log("Successfully updated player theme data"),
			error => {
				Debug.Log("Got error setting theme data");
				Debug.Log(error.GenerateErrorReport());
			});
	}

	public void UpdateTileSkin(int tileSkinID)
	{
		PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
			Data = new Dictionary<string, string>() {
					{"TileSkin", tileSkinID.ToString()}
				}
			}, 
			result => Debug.Log("Successfully updated player tile skin data"),
			error => {
				Debug.Log("Got error setting tile skin");
				Debug.Log(error.GenerateErrorReport());
			});
	}

	public static void UpdateTileSkinUnlockString()
	{
		PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
			Data = new Dictionary<string, string>() {
					{"TileUnlockString", tileUnlockString}
				}
			}, 
			result => Debug.Log("Successfully updated player tile skin unlock string data"),
			error => {
				Debug.Log("Got error setting tile skin");
				Debug.Log(error.GenerateErrorReport());
			});
	}

	public void UpdateGridSkin(int gridSkinID)
	{
		PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
			Data = new Dictionary<string, string>() {
					{"GridSkin", gridSkinID.ToString()}
				}
			}, 
			result => Debug.Log("Successfully updated player grid skin data"),
			error => {
				Debug.Log("Got error setting grid skin data");
				Debug.Log(error.GenerateErrorReport());
			});
	}

	public void UpdatePlayerStarData(int worldNo, int levelNo, string levelCode, string starString)
	{
		char[] existingStarString = worldStars[worldNo,levelNo].ToCharArray();
		bool improved = false;
		for(int i=0; i<3; i++)
		{
			if(existingStarString[i]=='0'&&starString[i]=='1')
			{
				existingStarString[i]='1';
				improved=true;
			}
		}

		if(improved)
		{
			//PArse new star string
			worldStars[worldNo,levelNo]=new string(existingStarString);
			Debug.Log("Updating player star info.." + new string(existingStarString));
			PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
			Data = new Dictionary<string, string>() {
					{levelCode[0]+"_STARS", GenerateStarString(worldNo)}
				}
			}, 
			result => {Debug.Log("Successfully updated player star data"); GetPlayerData(AccountInfo.playfabId);},
			error => {
				Debug.Log("Got error setting star data");
				Debug.Log(error.GenerateErrorReport());
			});
		} else{
			Debug.Log("Star count not improved");
			 GetAccountInfo();
		}
	}

	public string GenerateStarString(int worldNo)
	{
		return worldStars[worldNo,0]+","+worldStars[worldNo,1]+","+worldStars[worldNo,2]+","+worldStars[worldNo,3]+","+worldStars[worldNo,4]+","+worldStars[worldNo,5]+","+worldStars[worldNo,6]+","+worldStars[worldNo,7]+","+worldStars[worldNo,8]+","+worldStars[worldNo,9];
	}

	public void PlayerEvent_CompletedGame(bool playerWin, int gridSize, int maxTile, int score, bool targetMode)							
	{
		PlayFabClientAPI.WritePlayerEvent(new WriteClientPlayerEventRequest() {
        Body = new Dictionary<string, object>() {
            { "Grid-Size", gridSize },
            { "Max-Tile", maxTile },
			{ "Score", score},
			{ "Player-Win", playerWin},
			{ "Target-Mode", targetMode}
		},
		EventName = "player_completed_game"
		},
		result => UpdatePlayerStatistics(playerWin, GameMaster.instance.PlayerStatistics.p1Tiles),
		error => Debug.LogError(error.GenerateErrorReport()));
	}

	public void UpdatePlayerStatistics(bool playerWin, int tilesPlayed)
	{
		Debug.Log("Player event sent.. updating player statistics");

		GetPlayerStatistics();

		//Player Win Streak, Current & All Time
		int playerWinStreakVal = -1;
		if(playerWin)
		{
			playerWinStreakVal=AccountInfo.WIN_STREAK+1;
		} else
		{
			playerWinStreakVal=0;
		}

		// Win/Lose Increment Daily & All-time
		int winIncrement =0;
		int loseIncrement =0;
		if(playerWin)
			winIncrement++;
		else
			loseIncrement++;

		PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest() {
			Statistics = new List<StatisticUpdate>(){
				new StatisticUpdate{StatisticName = "GamesPlayed", Value=1},
				new StatisticUpdate{StatisticName = "Wins", Value=winIncrement},
				new StatisticUpdate{StatisticName = "DailyWins", Value=winIncrement},
				new StatisticUpdate{StatisticName = "Loses", Value=loseIncrement},
				new StatisticUpdate{StatisticName = "TilesPlayed", Value=tilesPlayed},
				new StatisticUpdate{StatisticName = "DailyTiles", Value=tilesPlayed},
				new StatisticUpdate{StatisticName = "WinStreak", Value=playerWinStreakVal}
			} 
		},
		result => {Debug.Log("Player statistics updated"); },
		error => {Debug.LogWarning("Stats failed to update"); });
	}

	public void LoadLeaderboard(string statisticName)
	{
		PlayFabClientAPI.GetLeaderboardAroundPlayer(new GetLeaderboardAroundPlayerRequest() {
			PlayFabId=playfabId,
			StatisticName=statisticName
		},
		result => {
			Debug.Log("leaderboard retireved");
			MenuController.instance.leaderboardController.LoadLeaderboardResponse(result);

		},
		error => {Debug.Log("leaderboard could not be retrieved.");});

	}

	public int LoadDailyLeaderboardValue(string statisticName)
	{
		int value=-1;
		PlayFabClientAPI.GetLeaderboardAroundPlayer(new GetLeaderboardAroundPlayerRequest() {
			PlayFabId=playfabId,
			StatisticName=statisticName,
			MaxResultsCount=1
		},
		result => {
			Debug.Log("leaderboard retireved");
			foreach(PlayerLeaderboardEntry entry in result.Leaderboard)
			{
				if(entry.PlayFabId == AccountInfo.playfabId)
				{
					value=entry.StatValue;
				}
			}

			Debug.LogError("DailyLeaderBoard: " + statisticName + ": val returned: " + value);
			UpdateChallengeUI(value);

		},
		error => {Debug.Log(" daily leaderboard could not be retrieved.");});

		return value;
	}

	public void GetInventory() {
		Debug.LogWarning("Get player Inventory");
		PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnInventorySuccess, OnAPIError);
	}

	public static void OnInventorySuccess(GetUserInventoryResult result)
	{
		for (int i = 0; i < result.Inventory.Count; i++)
		{
			Instance.inv_items.Add(result.Inventory[i]);
		}

		PlayFab.ClientModels.VirtualCurrencyRechargeTime res;
		if(result.VirtualCurrencyRechargeTimes.TryGetValue(AccountInfo.LIVES_CODE, out res))
		{
			MenuController.instance.CurrencyUI.secsToFullEnergy=res.SecondsToRecharge;
		}
			

		if(ApplicationModel.RETURN_TO_WORLD==-1)
			Database.LoadCatalog();

	}

	public bool InventoryContains(CatalogItem item)
	{
		foreach(ItemInstance inv_item in Instance.inv_items)
		{
			if(inv_item.ItemId == item.ItemId)
			return true;
		}

		return false;
	}

	public void SetDisplayName(string name)
	{
		PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest() {
			DisplayName = name
			}, 
			result => OnNameSetComplete(),
			error => {
				Debug.Log("Got error setting player name");
				Debug.Log(error.GenerateErrorReport());
		});
	}

	public void OnNameSetComplete()
	{
		Debug.Log("Successfully updated player name");
		MenuController.instance.OpenMainMenu();
	}

	public static void GetDailyChallenge()
	{
		Debug.LogError("Daily Challenge Code: " +AccountInfo.instance.info.TitleData["DailyChallengeCode."+AccountInfo.CURRENT_CHALLENGE_NO]);
		DAILY_CHALLENGE_CODE=AccountInfo.instance.info.TitleData["DailyChallengeCode."+AccountInfo.CURRENT_CHALLENGE_NO];

		//Get daily challenge text name
		Scene scene = SceneManager.GetActiveScene();
		if(scene.name != "Main")
		{
			//Retrieve challenge code
			string challengeCode = AccountInfo.instance.info.TitleData["DailyChallengeCode."+CURRENT_CHALLENGE_NO];

			string[] ret = challengeCode.Split('.');

			
			AccountInfo.Instance.LoadDailyLeaderboardValue(ret[0]);
		}

	}

	/// <summary>
	/// Method which is called when the currency value DC reaches 10 (which occurs every 24 hours)
	/// this resets the currency value to 0, generates a new Daily Challenge, updates UI content, updates player Datas
	/// </summary>
	public static void ResetDailyChallenge()
	{
		//Generate New Current Challenge Val and Update player data		
		int newChallengeVal=-1;
		while(newChallengeVal != -1 && newChallengeVal != AccountInfo.CURRENT_CHALLENGE_NO)
		{
			newChallengeVal = Random.Range(0, 10); 
		}
		AccountInfo.UpdateCurrentChallengeVal(newChallengeVal);

		//Retrieve challenge code
		string challengeCode = AccountInfo.instance.info.TitleData["DailyChallengeCode."+newChallengeVal];
		DAILY_CHALLENGE_CODE=AccountInfo.instance.info.TitleData["DailyChallengeCode."+newChallengeVal];
		CURRENT_CHALLENGE_NO=newChallengeVal;
		

		MenuController.instance.UpdateDailyChallengeUI(
			ParseChallengeCodeTitle(challengeCode),
			0, 
			ParseChallengeMaxVal(challengeCode)
		);


	}

	public static void UpdateChallengeUI(int val)
	{
		MenuController.instance.UpdateDailyChallengeUI(
			ParseChallengeCodeTitle(DAILY_CHALLENGE_CODE),
			val, 
			ParseChallengeMaxVal(DAILY_CHALLENGE_CODE)
		);

	}

	private static string ParseChallengeCodeTitle(string challengeCode)
	{
		string[] ret = challengeCode.Split('.');

		switch(ret[0])
		{
			case "DailyWins":
				return "Win " + ret[1] + " Games.";

			case "DailyStars":
				return "Collect " + ret[1] + " Stars.";
			
			case "DailyTiles":
				return "Play " + ret[1] + " Tiles Today.";

			default:
				return "404: Object code unrecognised";

		}

	}

	private static int ParseChallengeMaxVal(string challengeCode)
	{
		string[] ret = challengeCode.Split('.');
		return int.Parse(ret[1]);
	}

	public static void UpdateDailyChallengeValue()
	{
		string challengeCode = AccountInfo.instance.info.TitleData["DailyChallengeCode."+CURRENT_CHALLENGE_NO];
				string[] ret = challengeCode.Split('.');
				MenuController.instance.UpdateDailyChallengeUI(AccountInfo.Instance.LoadDailyLeaderboardValue(ret[0]));
	}

	private static void UpdateCurrentChallengeVal(int challengeNo)
	{
		PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
			Data = new Dictionary<string, string>() {
					{"CURRENT_CHALLENGE_VAL", challengeNo.ToString()}
				}
			}, 
			result => Debug.Log("Successfully updated player CURRENT DAILY CHALLENGE DATA"),
			error => {
				Debug.Log("Got error updated current challenge data");
				Debug.Log(error.GenerateErrorReport());
			});
	}

}
