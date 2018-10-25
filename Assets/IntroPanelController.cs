using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IntroPanelController : MonoBehaviour {

	// Use this for initialization
	[SerializeField]
    private GameObject introPanel;
    
	[SerializeField]
    private TextMeshProUGUI targetText1;

	[SerializeField]
    private TextMeshProUGUI targetText2;

	[SerializeField]
    private TextMeshProUGUI targetText3;

	[SerializeField]
    private TextMeshProUGUI vsText;

	[SerializeField]
	private GameObject[] playerGameOverSection; // root ->score->name
												//  	->image->border

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		introPanel.SetActive(true);
		StartCoroutine(InitIntroPlayerSections());
	}


	public IEnumerator InitIntroPlayerSections()
	{
		int playercount=ApplicationModel.HUMAN_PLAYERS+ApplicationModel.AI_PLAYERS;
		for(int i=0; i<(ApplicationModel.HUMAN_PLAYERS+ApplicationModel.AI_PLAYERS); i++)
		{
			playerGameOverSection[i].SetActive(true);
			if(i==0)
			{
				playerGameOverSection[i].GetComponentInChildren<TextMeshProUGUI>().enabled=false;
				if(AccountInfo.Instance!=null)
				{
					if(AccountInfo.Instance.Info.PlayerProfile.DisplayName!=null)
					{
						playerGameOverSection[i].GetComponentInChildren<TextMeshProUGUI>().transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text=AccountInfo.Instance.Info.PlayerProfile.DisplayName;
					}
				}
			} else
			{
				playerGameOverSection[i].GetComponentInChildren<TextMeshProUGUI>().enabled=false;
				playerGameOverSection[i].GetComponentInChildren<TextMeshProUGUI>().transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text="AI_"+i;
				
			}
		}

		for(int i=(ApplicationModel.HUMAN_PLAYERS+ApplicationModel.AI_PLAYERS); i<4; i++ )
		{
			Debug.Log("Deactive player section");
			playerGameOverSection[i].SetActive(false);
		}

		if(ApplicationModel.AI_PLAYERS==0)
		{
			vsText.text="Target Mode";
			targetText1.gameObject.SetActive(true);
			targetText1.text="1 Star Target "+ApplicationModel.TARGET.ToString();
			targetText2.gameObject.SetActive(true);
			targetText2.text="2 Star Target "+ApplicationModel.TARGET2.ToString();
			targetText3.gameObject.SetActive(true);
			targetText3.text="3 Star Target "+ApplicationModel.TARGET3.ToString();
		}


		yield return new WaitForSeconds(4.5f);

		IntroFadeInTrigger();
	}

	private void IntroFadeInTrigger()
	{
		GetComponent<Animator>().SetTrigger("fadeIn");
	}

	/// <summary>
	/// This function is called at the end of the intro panel "fade in" aniamtion via an animation event
	/// </summary>
	private void HideIntroPanel()
	{
		GameMaster.instance.gameObject.SetActive(true);
		GameMaster.instance.enabled=true;
		GUI_Controller.instance.gameObject.SetActive(true);
		StartCoroutine(GUI_Controller.instance.GridIntroAnim());
		
	}


}
