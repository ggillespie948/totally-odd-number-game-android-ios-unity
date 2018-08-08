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
		gameOverSequencePanel.SetActive(true);
		StartCoroutine(InitGameOverSections());
	}


	public IEnumerator InitGameOverSections()
	{
		int playercount=ApplicationModel.HUMAN_PLAYERS+ApplicationModel.AI_PLAYERS;
		int playerIndex=0;
		foreach(PlayerCard card in GUI_Controller.instance.PlayerCards)
		{
			if(playerIndex<playercount)
			{
				playerGameOverSection[playerIndex].GetComponentInChildren<TextMeshProUGUI>().enabled=true;
				playerGameOverSection[playerIndex].GetComponentInChildren<TextMeshProUGUI>().text="0";
				playerGameOverSection[playerIndex].GetComponentInChildren<TextMeshProUGUI>().transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text=GUI_Controller.instance.PlayerCards[playerIndex].ActiveNameTag.text;
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
			LoadWinLoseTxt();
			scoreTallying=false;
		}
	}

	public void LoadWinLoseTxt()
    {
        if(GameMaster.instance.playerWin)
        {
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
		if(GameMaster.instance.soloPlay)
		{
			targetText1.gameObject.SetActive(false);
			targetText2.gameObject.SetActive(false);
			targetText3.gameObject.SetActive(false);
		}

	}

	
}
