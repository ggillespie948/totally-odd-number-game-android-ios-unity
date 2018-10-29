using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RedeemCodeController : MonoBehaviour {

	// UI 
	public TextMeshProUGUI referalcode;
	public GameObject redeemButton;
	public InputField inputField;
	public GameObject copyCodeButton;
	public TextMeshProUGUI referalcodeInfoText;
	
	public string badgeName = "referralBadge";
	
	private bool wasReferralBageFound; 

	public string playFabTitleId;

	public TextMeshProUGUI copyToClipTxt;
	
	public void OnRedeemClicked()
	{
		RedeemReferralCode();
	}

	public void OnCopyCodeClicked()
	{
		UniClipboard.SetText(AccountInfo.playfabId);
		copyToClipTxt.text="Code Copied";
	}

	public void HideReferalInput()
	{
		inputField.gameObject.SetActive(false);
		redeemButton.SetActive(false);
	}

	public void InitiateReferalPanel()
	{
		// set TitleID in the SDK
		PlayFab.PlayFabSettings.TitleId = this.playFabTitleId;
		SearchForReferralBadge();
		if(this.wasReferralBageFound == true)
		{
			HideReferalInput();
			referalcodeInfoText.text="You have already redeemed a referal code. Pass your code on to others to keep the rewards coming.";
		}
		referalcode.text=AccountInfo.playfabId;
	}

	private void SearchForReferralBadge()
	{
		if(AccountInfo.Instance.InventoryContainsItemClass("referralBadge"))
		{
			this.wasReferralBageFound=true;
		} else 
		{
			this.wasReferralBageFound=false;
		}
	}

	void RedeemReferralCode()
	{	
		Debug.Log("REDEEMING...");
		ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest() { 
			FunctionName = "RedeemReferral", 
			GeneratePlayStreamEvent = true,

			FunctionParameter = new { 
				referralCode = this.inputField.text 
			}
		};
		PlayFabClientAPI.ExecuteCloudScript(request, OnRedeemReferralCodeCallback, OnApiCallError);
	}
	
	void OnRedeemReferralCodeCallback(ExecuteCloudScriptResult result) 
	{
		// output any errors that happend within cloud script
		if(result.Error != null)
		{
			Debug.LogError(string.Format("{0} -- {1}", result.Error, result.Error.Message));
			return;
		}

		Debug.Log("Function res:" + result.FunctionResult);//result.FunctionResult


		if(result.Logs.Count == 0)
		{
			referalcodeInfoText.text="You cannot refer yourself. Please enter the referral code of another player.";
			return;
		}

		if(result.FunctionResult.ToString() == "{\"errorDetails\":\"Error: [object Object]\"}")
		{
			referalcodeInfoText.text="Error: Invalid Referral Code";
			return;
		}

		List<ItemInstance> grantedItems = PlayFab.Json.JsonWrapper.DeserializeObject<List<ItemInstance>>(result.FunctionResult.ToString());
		if(grantedItems != null)
		{
			Debug.Log("SUCCESS!...\nYou Just Recieved:");
			string output = string.Empty;
			foreach(var itemInstance in grantedItems)
			{			
				output += string.Format("\t {0} \n", itemInstance.DisplayName); 
			}
			
			AccountInfo.Instance.inv_items.AddRange(grantedItems);
			HideReferalInput();
			referalcodeInfoText.text="You have already redeemed a referal code. Pass your code on to others to keep the rewards coming.";


			Debug.Log(output);
			foreach(var statement in result.Logs)
			{
				Debug.Log(statement.Message);
			}
		}
		else
		{
			Debug.LogError("An error occured when attemtpting to deserialize the granted items.");
		}
	}
	
	void OnApiCallError(PlayFabError err)
	{
		string http = string.Format("HTTP:{0}", err.HttpCode);
		string message = string.Format("ERROR:{0} -- {1}", err.Error, err.ErrorMessage);
		string details = string.Empty;
		
		if(err.ErrorDetails != null)
		{
			foreach(var detail in err.ErrorDetails)
			{
				details += string.Format("{0} \n", detail.ToString());
			}
		}
		
		Debug.LogError(string.Format("{0}\n {1}\n {2}\n", http, message, details));
	}
}

