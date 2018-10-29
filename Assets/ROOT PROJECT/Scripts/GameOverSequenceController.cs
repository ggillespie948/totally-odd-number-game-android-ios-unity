using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TMPro.Examples;

public class GameOverSequenceController : MonoBehaviour {

	// Use this for initialization
	[SerializeField]
    private GameObject gameOverSequencePanel;
    [SerializeField]
    private TeleType tele;
    
	[SerializeField]
	private GameObject goodRay;
	
	[SerializeField]
    private GameObject playerWinTxT;
    [SerializeField]
    private GameObject playerLoseTxT;

	[SerializeField]
    private TextMeshProUGUI targetText1;

	[SerializeField]
    private TextMeshProUGUI targetText2;

	[SerializeField]
    private TextMeshProUGUI targetText3;

	private bool scoreTallying=false;

	

	[SerializeField]
	private GameObject[] playerGameOverSection; // root ->score->name
												//  	->image->border

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		GetComponent<Animator>().Rebind(); //Weird untested code
		SetResolutionDefault();
		gameOverSequencePanel.SetActive(true);
		StartCoroutine(InitGameOverSections());
		
		

	}

	private void SetResolutionDefault()
	{
		// if(ApplicationModel.SOLO_PLAY)
		// {
		// 	GetComponent<Animator>().SetBool("solo", true);
		// } else 
		// {
		// 	GetComponent<Animator>().SetBool("solo", false);
		// }

		if(Screen.height > 2400)
		{
            if(Screen.width < 1130)
            {
                Debug.Log("Set Xtra Slim Resolution Default"); //iphone X
                GetComponent<Animator>().SetTrigger("slim");

            } else if(Screen.height > 2600)
            {
                Debug.Log("Set Xtra Slim Resolution Default"); // s8 s9
                GetComponent<Animator>().SetTrigger("slim");
            } else 
            {
                Debug.Log("Set Default Resolution Default");  //s6 / 27
			    GetComponent<Animator>().SetTrigger("default");

            }

		} else if(Screen.width > 1500)
		{
			Debug.Log("Set Wide Resolution Default");
			GetComponent<Animator>().SetTrigger("wide");

		} else 
		{
			Debug.Log("Set Default Resolution Default");
			GetComponent<Animator>().SetTrigger("default");

		}

	}


	public IEnumerator InitGameOverSections()
	{
		int playercount=ApplicationModel.HUMAN_PLAYERS+ApplicationModel.AI_PLAYERS;
		int playerIndex=0;
		foreach(PlayerCard card in GUI_Controller.instance.PlayerCards)
		{
			if(playerIndex<playercount)
			{
				if(playerIndex==0)
				{
					if(ApplicationModel.SOLO_PLAY)
					{
						GetComponent<IntroPanelController>().soloGameOverSection.GetComponentInChildren<TextMeshProUGUI>().enabled=true;
						GetComponent<IntroPanelController>().soloGameOverSection.GetComponentInChildren<TextMeshProUGUI>().text="0";
						GetComponent<IntroPanelController>().soloGameOverSection.GetComponentInChildren<TextMeshProUGUI>().transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text=GUI_Controller.instance.PlayerCards[playerIndex].ActiveNameTag.text;
						for(int u=0; u<3; u++)
						{
							GetComponent<IntroPanelController>().soloIntroTiles[u].enabled=false;
						}

					} else 
					{
						GetComponent<IntroPanelController>().player1MultiSection.GetComponentInChildren<TextMeshProUGUI>().enabled=true;
						GetComponent<IntroPanelController>().player1MultiSection.GetComponentInChildren<TextMeshProUGUI>().text="0";
						GetComponent<IntroPanelController>().player1MultiSection.GetComponentInChildren<TextMeshProUGUI>().transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text=GUI_Controller.instance.PlayerCards[playerIndex].ActiveNameTag.text;
					}

				} else 
				{

					playerGameOverSection[playerIndex].GetComponentInChildren<TextMeshProUGUI>().enabled=true;
					playerGameOverSection[playerIndex].GetComponentInChildren<TextMeshProUGUI>().text="0";
					playerGameOverSection[playerIndex].GetComponentInChildren<TextMeshProUGUI>().transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text=GUI_Controller.instance.PlayerCards[playerIndex].ActiveNameTag.text;
				}
			} else
			{
				playerGameOverSection[playerIndex].SetActive(false);
			}
			playerIndex++;
		}

		if(GameMaster.instance.soloPlay)
		{
			targetText1.gameObject.SetActive(true);
			targetText1.text="1 Star Target "+ApplicationModel.TARGET.ToString();
			targetText2.gameObject.SetActive(true);
			targetText2.text="2 Star Target "+ApplicationModel.TARGET2.ToString();
			targetText3.gameObject.SetActive(true);
			targetText3.text="3 Star Target "+ApplicationModel.TARGET3.ToString();
		}

		yield return new WaitForSeconds(3f);
		AudioManager.instance.Play("drumLoop");
		scoreTallying=true;
		for(int i=0;i<ApplicationModel.HUMAN_PLAYERS+ApplicationModel.AI_PLAYERS;i++)
		{
			StartCoroutine(GUI_Controller.instance.UpdateUIScore(0, GameMaster.instance.playerScores[i], playerGameOverSection[i].GetComponentInChildren<TextMeshProUGUI>(), ""));
			yield return new WaitForSeconds(.5f);
		}

		yield return new WaitForSeconds(4.5f);
	}

	/// <summary>
	///  this function is called when the GUI controller method UpdateScore has completed updateing a UI element 
	///  but only when GameMaster.gameOver and this.ScoreTallying are true
	/// </summary>
	public void GameResultCallBack()
	{
		if(scoreTallying)
		{
			AudioManager.instance.Stop("drumLoop");
			AudioManager.instance.Play("drumFinale");
			LoadWinLoseTxt();
			scoreTallying=false;
		}
	}

	
	[SerializeField]
	private GameObject[] playerCrowns;
	public void LoadWinLoseTxt()
    {
        if(GameMaster.instance.playerWin)
        {
			if(!ApplicationModel.SOLO_PLAY)
				playerCrowns[0].SetActive(true);

            playerWinTxT.SetActive(true);
        } else
        {
            playerLoseTxT.SetActive(true);
        }

        Invoke("fadeOut", 3f);
		Invoke("GenerateBonusAwardText", 7.5f);
    }

    private void GenerateBonusAwardText()
    {
		Destroy(playerWinTxT);
		Destroy(playerLoseTxT);
		foreach(GameObject g in playerGameOverSection)
			g.SetActive(false);
		
        tele.gameObject.SetActive(true);
		int currencyTotal  = tele.GenerateGameOverBonuses();
		
		goodRay.transform.SetParent(this.gameObject.transform);

		//callback afetr currency has been added
		Invoke("DisablePreviousGUI", 7.5f);
		StartCoroutine(GameMaster.instance.ActivateGameOverDialogue(7.5f, GameMaster.instance.playerWin, currencyTotal));
		
    }

	private void DisablePreviousGUI()
	{
		tele.gameObject.SetActive(false);
		tele.activateObjectOnComplete.SetActive(false);
		goodRay.SetActive(false);
	}
	
	public void fadeOut()
	{
		GetComponent<Animator>().SetTrigger("fadeOut");
		
		if(ApplicationModel.TUTORIAL_MODE)
		{
			Debug.LogWarning("Tutorial mode = true;");			 
			{
				if(ApplicationModel.LEVEL_CODE=="B1")
				{
					if(Tutorial_Controller.instance.tutorialIndex < 5)
					{
						Debug.LogWarning(" b1 Tutorial mode = false; tutorial index < 5");
						ApplicationModel.TUTORIAL_MODE=false;
						ApplicationModel.RETURN_TO_WORLD=0;
					} else 
					{
						Debug.LogWarning(" b1 Tutorial mode = true; tutorial index > 5");
						ApplicationModel.TUTORIAL_MODE=true;
						ApplicationModel.RETURN_TO_WORLD=-2;
					}

				} else if(ApplicationModel.LEVEL_CODE =="B2")
				{
					
					Debug.LogWarning(" b2 Tutorial mode = true; tutorial index > 5");
					ApplicationModel.TUTORIAL_MODE=false;
					ApplicationModel.RETURN_TO_WORLD=-1;
					
				}

				if(ApplicationModel.LEVEL_CODE == "B3")
				{
					ApplicationModel.TUTORIAL_MODE=false;
					ApplicationModel.RETURN_TO_WORLD=0;
				}

			}
			

		} else 
		{
			Debug.Log("Tutoriakl mode = false  RTW: " + ApplicationModel.RETURN_TO_WORLD);
			if(ApplicationModel.RETURN_TO_WORLD==-3)
			{
				ApplicationModel.RETURN_TO_WORLD=-1;
				ApplicationModel.TUTORIAL_MODE=false;
			} else 
			{
				if(ApplicationModel.LEVEL_CODE=="B1" || ApplicationModel.LEVEL_CODE=="B2")
				{
					ApplicationModel.RETURN_TO_WORLD=0;
				}
			}

			
		}

		if(ApplicationModel.LEVEL_CODE == "B3")
			{
				ApplicationModel.TUTORIAL_MODE=false;
				ApplicationModel.RETURN_TO_WORLD=0;
			}

		


	}

	
}
