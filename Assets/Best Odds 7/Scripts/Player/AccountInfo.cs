using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class AccountInfo : MonoBehaviour {

	private static AccountInfo instance;

	public static AccountInfo Instance
	{
		get {return instance;}
		set { instance = value;}
	}

	[SerializeField]
	public static int[,] worldStars;
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
	

	
	private void Awake()
	{
		if(instance !=this)
			instance = this;

		DontDestroyOnLoad(gameObject);

		worldStars = new int[4,5]; // temp - hard coded the number of worlds/levels
	}

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		Login();
		
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
			print("Ifone");

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
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			//Debug.Log("Mobile device detected");
		}

		if(result.NewlyCreated)
		{
			SetUpAccount();
		} else 
		{
			GetPlayerData(result.PlayFabId);
		}
		
		GetAccountInfo(result.PlayFabId);
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

	public static void OnAPIError(PlayFabError error)
	{
		Debug.LogError(error);
	}

	static void OnAccountInfoSuccess(GetPlayerCombinedInfoResult result)
	{
		Debug.Log("Player Info Retrieved");
		Instance.info = result.InfoResultPayload;
		UpdateUIContent(result.InfoResultPayload);

	}

	public static void UpdateUIContent(GetPlayerCombinedInfoResultPayload info)
	{
		Debug.Log("Updating UI content..");
		int res = -1;	
		if(AccountInfo.playfabId != null)
		{
			if(Instance.Info.UserVirtualCurrency.TryGetValue(AccountInfo.COINS_CODE, out res))
			{
				GUI_Controller.instance.PlayerCoins_Stone.GetComponentInChildren<TextMeshProUGUI>().text =  res.ToString();
			}

			
		}

	}

	public static void DispalyID()
	{
		GUI_Controller.instance.PlayerCoins_Stone.GetComponentInChildren<TextMeshProUGUI>().text = Instance.info.AccountInfo.PlayFabId.ToString();
		
	}

	// public static void SetUpAccount()
	// {
	// 	Debug.Log("Setting Up Account..");
	// 	Dictionary<string, string> data = new Dictionary<string, string>();

	// 	//Initalise a star count for each of the single player levels
	// 	data.Add("B1", "0");
	// 	data.Add("B2", "0");
	// 	data.Add("B3", "0");
	// 	data.Add("B4", "0");
	// 	data.Add("B5", "0");

	// 	data.Add("I1", "0");
	// 	data.Add("I2", "0");
	// 	data.Add("I3", "0");
	// 	data.Add("I4", "0");
	// 	data.Add("I5", "0");

	// 	data.Add("A1", "0");
	// 	data.Add("A2", "0");
	// 	data.Add("A3", "0");
	// 	data.Add("A4", "0");
	// 	data.Add("A5", "0");

	// 	data.Add("M1", "0");
	// 	data.Add("M2", "0");
	// 	data.Add("M3", "0");
	// 	data.Add("M4", "0");
	// 	data.Add("M5", "0");

	// 	UpdateUserDataRequest request = new UpdateUserDataRequest()
	// 	{
	// 		Data = data,
	// 	};

	// 	PlayFabClientAPI.UpdateUserData(request, UpdateDataInfo, OnAPIError);

	// }
	public static void SetUpAccount() {
		PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
			Data = new Dictionary<string, string>() {
				{"B1", "0"},
				{"B2", "0"},
				{"B3", "0"},
				{"B4", "0"},
				{"B5", "0"},
				{"I1", "0"},
				{"I2", "0"},
				{"I3", "0"},
				{"I4", "0"},
				{"I5", "0"},
			}
		}, 
		result => SetUpAccount2(),
		error => {
			Debug.Log("Got error setting initial user data");
			Debug.Log(error.GenerateErrorReport());
		});
	}

	public static void SetUpAccount2()
	{
			PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
			Data = new Dictionary<string, string>() {
				{"A1", "0"},
				{"A2", "0"},
				{"A3", "0"},
				{"A4", "0"},
				{"A5", "0"},
				{"M1", "0"},
				{"M2", "0"},
				{"M3", "0"},
				{"M4", "0"},
				{"M5", "0"}
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
				if(!result.Data.ContainsKey("B1"))
				{
					SetUpAccount();
				}

				int beginnerCounter = 0;
				for(int i=0; i<5; i++)
				{
					worldStars[0,i] = int.Parse(result.Data["B"+(i+1).ToString()].Value);
					beginnerCounter += int.Parse(result.Data["B"+(i+1).ToString()].Value);
				}
				beginnerStars = beginnerCounter;

				int interCounter = 0;
				for(int i=0; i<5; i++)
				{
					worldStars[1,i] = int.Parse(result.Data["I"+(i+1).ToString()].Value);
					interCounter += int.Parse(result.Data["I"+(i+1).ToString()].Value);
				}
				intermediateStars = interCounter;

				int advCounter = 0;
				for(int i=0; i<5; i++)
				{
					worldStars[2,i] = int.Parse(result.Data["A"+(i+1).ToString()].Value);
					advCounter += int.Parse(result.Data["A"+(i+1).ToString()].Value);
				}
				advancedStars = advCounter;


			}
		}, (error) => {
			Debug.Log("Got error retrieving user data:");
			Debug.Log(error.GenerateErrorReport());
		});
	}

	public void UpdatePlayerStarData(int worldNo, int levelNo, string levelCode, int starCount)
	{
		if(worldStars[worldNo,levelNo] < starCount)
		{
			Debug.Log("Updating player star info.." + starCount);
			PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
				Data = new Dictionary<string, string>() {
					{levelCode, starCount.ToString()}
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




}
