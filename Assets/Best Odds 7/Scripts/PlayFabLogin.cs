using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

    public class PlayFabLogin : MonoBehaviour
    {
        // public void Start()
        // {
        //     PlayFabSettings.TitleId = "FCE"; // Please change this value to your own titleId from PlayFab Game Manager

		// 	if (Application.platform == RuntimePlatform.Android)
		// 	{
		// 	    var request = new LoginWithAndroidDeviceIDRequest 
		// 		{
		// 			TitleId = PlayFabSettings.TitleId,
		// 			AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
		// 			CreateAccount = true
		// 		};
		// 		PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnLoginSuccess, OnLoginFailure);
		// 	}
		// 	else if(Application.platform == RuntimePlatform.IPhonePlayer)
		// 	{
		// 		print("Ifone");

		// 	}
		// 	else
		// 	{
		// 		var request = new LoginWithCustomIDRequest { CustomId = "E4A0DA3093A5FD3A", CreateAccount = true};
		// 		PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
		// 	}
			

        // }

        // private void OnLoginSuccess(LoginResult result)
        // {
		// 	Debug.LogWarning("Login Successful");
		// 	if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		// 	{
		// 	}
        //     	AccountInfo.Instance.GetAccountInfo(result.PlayFabId);
		// 		AccountInfo.Instance.SetUpAccount(); // temp - resets the account statistics each login currently
        // }

        // private void OnLoginFailure(PlayFabError error)
        // {
        //     Debug.LogWarning("Something went wrong with Login.  :(");
        //     Debug.LogError("Debug information:");
        //     Debug.LogError(error.GenerateErrorReport());

		// 	GUI_Controller.instance.menuDebugTxt.text += " Something went wrong with login..."; // temp - change this to GameMenuMaster
		// 	GUI_Controller.instance.menuDebugTxt.text += " Debug info: ";
		// 	GUI_Controller.instance.menuDebugTxt.text += error.GenerateErrorReport();
        // }
    }