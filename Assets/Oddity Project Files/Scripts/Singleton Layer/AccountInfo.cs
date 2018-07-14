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

	[SerializeField]
	public static string[,] worldStars;
	public static string playfabId;

	public static int beginnerStars;
	public static int intermediateStars;
	public static int advancedStars;

	[SerializeField]
	private GetPlayerCombinedInfoResultPayload info;

	public GetPlayerCombinedInfoResultPayload Info
	{
		get { return info;}
		set { info = value;}
	}

	public static string COINS_CODE  = "PC";
	public static string GEMS_CODE  = "PG";
	public static string LIVES_CODE  = "PL";
	public static string DATA_STARS  = "DS";
	public static string CATALOG_ITEMS = "Items";
	public static string ITEM_TILESKIN = "TileSkin";
	public static int ITEM_COST =1;
	public static int ITEM_PREFAB =3;
	public static int ITEM_ICON =5;
	public static int ITEM_STARREQ =7;

	public List<ItemInstance> inv_items = new List<ItemInstance>();
	
	

	
	private void Awake()
	{
		if(instance !=this)
			instance = this;

		DontDestroyOnLoad(gameObject);

		worldStars = new string[4,11]; // temp - hard coded the number of worlds/levels
	}

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		Login();
		
	}

	public static int TotalStars()
	{
		Debug.LogWarning("Total stars: " + (beginnerStars+intermediateStars+advancedStars));
		return beginnerStars+intermediateStars+advancedStars;
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
		GetAccountInfo(result.PlayFabId);
		Instance.GetInventory();
		Database.UpdateDatabase();
		playfabId = result.PlayFabId;
	}

	private static void OnLoginFailure(PlayFabError error)
	{
		Debug.LogWarning("Something went wrong with Login.  :(");
		Debug.LogError("Debug information:");
		Debug.LogError(error.GenerateErrorReport());

		GUI_Controller.instance.menuDebugTxt.text += " Something went wrong with login..."; // temp - change this to GameMenuMaster
		GUI_Controller.instance.menuDebugTxt.text += " Debug info: ";
		GUI_Controller.instance.menuDebugTxt.text += error.GenerateErrorReport();
	}

	public static void GetAccountInfo(string playfabId)
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
	}

	static void OnAccountInfoSuccess(GetPlayerCombinedInfoResult result)
	{
		Debug.Log("Player Info Retrieved");
		if(Instance != null)
		{
			Instance.info = result.InfoResultPayload;
			UpdateUIContent(result.InfoResultPayload);
		}

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

			GUI_Controller.instance.StarDialogue.GetComponentInChildren<TextMeshProUGUI>().text = (beginnerStars+intermediateStars+advancedStars).ToString();

			
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
				{"I_STARS", "000,000,000,000,000,000,000,000,000,000"},
				{"A_STARS", "000,000,000,000,000,000,000,000,000,000"},
				{"M_STARS", "000,000,000,000,000,000,000,000,000,000"},
			}
		}, 
		result => Debug.Log("Successfully setup user data"),//SetUpAccount2(),
		error => {
			Debug.Log("Got error setting initial user data");
			Debug.Log(error.GenerateErrorReport());
		});
	}

	public static void SetUpAccount2()
	{
			PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
			Data = new Dictionary<string, string>() {
				{"A1", "000"},
				{"A2", "000"},
				{"A3", "000"},
				{"A4", "000"},
				{"A5", "000"},
				{"M1", "000"},
				{"M2", "000"},
				{"M3", "000"},
				{"M4", "000"},
				{"M5", "000"}
			}
		}, 
		result => Debug.Log("Successfully setup user data"),
		error => {
			Debug.Log("Got error setting initial user data");
			Debug.Log(error.GenerateErrorReport());
		});

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

	public static void OnAddedInGameCurrency(ModifyUserVirtualCurrencyResult result)
	{
		Debug.Log("Currency Updated!");
	}
		
	public static void OnAddGameCurrencyError(PlayFabError error)
	{
		Debug.LogError("Error adding in game currency: " + error.Error + " " + error.ErrorMessage);
	}

	private static void GetPlayerData(string playfabId) {
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

					//Parse Intermediate Stars
					int intermediateCounter = 0;
					string intermediateStarString = result.Data["I_STARS"].Value;
					string[] tokenResult2 = intermediateStarString.Split(',');
					int indx2=0;
					foreach(string token in tokenResult2)
					{
						worldStars[1,indx2]=token;
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
						worldStars[2,indx3]=token;
						indx3++;
						foreach(char c in token)
						{
							if(c=='1')
								advancedCounter++;
						}
					}
					advancedStars = advancedCounter;
					
				}
			}
		}, (error) => {
			Debug.Log("Got error retrieving user data:");
			Debug.Log(error.GenerateErrorReport());
		});
	}

	public void UpdatePlayerStarData(int worldNo, int levelNo, string levelCode, string starString)
	{
		char[] sString = worldStars[worldNo,levelNo].ToCharArray();
		bool improved = false;
		for(int i=0; i<3; i++)
		{
			if(sString[i]=='0'&&starString[i]=='1')
			{
				sString[i]='1';
				improved=true;
			}
		}

		if(improved)
		{
			//PArse new star string
			worldStars[worldNo,levelNo]=starString;
			Debug.Log("Updating player star info.." + new string(sString));
			PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
			Data = new Dictionary<string, string>() {
					{levelCode[0]+"_STARS", GenerateStarString(worldNo)}
				}
			}, 
			result => Debug.Log("Successfully updated player star data"),
			error => {
				Debug.Log("Got error setting star data");
				Debug.Log(error.GenerateErrorReport());
			});
		} else{
			Debug.Log("Star count not improved");
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
		result => Debug.Log("Success"),
		error => Debug.LogError(error.GenerateErrorReport()));
	}

	public void GetInventory() {
		PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnInventorySuccess, OnAPIError);
	}

	public static void OnInventorySuccess(GetUserInventoryResult result)
	{
		for (int i = 0; i < result.Inventory.Count; i++)
		{
			Instance.inv_items.Add(result.Inventory[i]);
		}
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

	

}
