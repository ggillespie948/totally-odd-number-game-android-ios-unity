using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TMPro.Examples;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Tutorial_Controller : MonoBehaviour {

	
	//Singleton instance
	public static Tutorial_Controller instance = null;


	[SerializeField]
	public bool colValidity;
	[SerializeField]
	public bool rowValidity;



	
	//References to random game objects which are moved during the tutorial process
	[Header("Tutorial 1 (Learn Rules)")]
	public GameObject submitBtn;

	public GameObject swapTilesBtn;

	public GameObject timerBlock;

	[SerializeField]
	private List<TextMeshProUGUI> winObjectiveTexts;

	[SerializeField]
	private List<TextMeshProUGUI> loseObjectiveTexts;
	public GameObject gridContainer;

	[Header("Tutorial 1.5 (Main Menu 1)")]
	public GameObject dailyChallengeBlock;
	public GameObject dailyChallengeParent;
	public GameObject playBtn;
	public GameObject multiPlayerBtn;
	public GameObject beginnerSelection;
	public GameObject intermediateSelection;
	public GameObject tutorial2LevelSelection;
	public GameObject lockedLevelSelection;
	public GameObject lockedLevel1Selection;
	public GameObject returnToContentTransform;
	public GameObject returnCurrencyContent;
	public GameObject level1StarContainer;
	public GameObject rightBtn;
	public GameObject leftBtn;
	public GameObject startGameBtn;
	public GameObject levelDetails;
	public GameObject backButton;

	[Header("Tutorial 2.5 (Main Menu 2)")]
	public GameObject tutorialTileBox;
	public GameObject tutorialNoBtn;
	public GameObject unlockGoodRay;
	public GameObject shopGoodRay;
	public GameObject tutorialShopButton;
	public GameObject returnIAPContent;
	public GameObject coinShop;
	public GameObject energyShop;
	public GameObject boosterShop;

	public GameObject multiplayerBtn;
	public GameObject singleplayerBtn;
	public GameObject settingsButton;
	public GameObject starsButton;

	public GameObject level1LevelSelection;

	public GameObject retryBtn;
	public GameObject continueBtn;



	public Button youWinRetry;
	public Button youWinContinue; 

	public Button youLoseRetry;
	public Button youLoseContinue; 

	
	[SerializeField]
    public int tutorialIndex;

	[SerializeField]
    public TeleType tele;

	[SerializeField]
    public GameObject infoPopup;

	[SerializeField]
	public string[] tutorialDialogueInstructions;


	[SerializeField]
	public GameObject remianingTurnsPanel;

	[SerializeField]
	public GameObject targetScorePanel;

	[SerializeField]
	public GameObject currentScorePanel;

	[SerializeField]
	public GameObject targetStarsPanel;

	public HorizontalLayoutGroup targetLayout; //also acts as a return transform for target UI when highlighting

	[SerializeField]
	private GameObject levelSelecBackBtn;
	
	
	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		if(instance==null)
			instance=this;

			if(submitBtn!=null)
			{
				submitBtn.GetComponent<BoxCollider>().enabled=false;
			}

			if(continueBtn!= null)
			{
				continueBtn.GetComponent<Button>().interactable=false;
			}

			if(retryBtn!=null)
			{
				retryBtn.GetComponent<Button>().interactable=false;
			}
	}
	
	
	// Use this for initialization
	void Start () {
		infoPopup.SetActive(true);
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	
	[SerializeField]
	public List<bool> colValdityChecks = new List<bool>();
	[SerializeField]
	public List<bool> rowValidityChecks = new List<bool>();

	private void LockUI(int vers)
	{
		switch(vers)
		{
			case 0: // single player btn press
			multiplayerBtn.GetComponent<BoxCollider>().enabled=false;
			settingsButton.GetComponent<BoxCollider>().enabled=false;
			starsButton.GetComponent<BoxCollider>().enabled=false;
			break;


		}

	}

	private void UnlockUI(int vers)
	{
		switch(vers)
		{
			case 0: // single player btn press
			multiplayerBtn.GetComponent<BoxCollider>().enabled=true;
			settingsButton.GetComponent<BoxCollider>().enabled=true;
			starsButton.GetComponent<BoxCollider>().enabled=true;
			break;


		}

	}

	public void AddToErrorLog(int total, List<int> values, bool isCol)
	{
		if(tutorialDialogueInstructions[19] != "Oops! ")
			tutorialDialogueInstructions[19] = "Oops! ";


		foreach (int i in values)
		{
			tutorialDialogueInstructions[19]+=i+"+";
		}

		tutorialDialogueInstructions[19] = tutorialDialogueInstructions[19].Remove(tutorialDialogueInstructions[19].Length - 1);

		tutorialDialogueInstructions[19]+="="+total+"! ";

		if(isCol)
		{
			tutorialDialogueInstructions[19]+="This means that the COLUMN has an EVEN total.. try again!";
		} else 
		{
			tutorialDialogueInstructions[19]+="This means that the ROW has an EVEN total.. try again!";
		}

		
		

	}


	public void GenerateErrorMessage() // DELETE??????????????
	{
		colValdityChecks.Clear();
		rowValidityChecks.Clear();

		List<GridCell> invalidCells = new List<GridCell>();


		foreach(GridCell cell in GameMaster.instance.playedTiles)
		{
			if(BoardController.instance.CheckTileValidity(cell, false))
			{
				invalidCells.Add(cell);
			} else 
			{
				invalidCells.Add(null);
			}
		}

	}

	

	/// <summary>
	/// Method which cycles to the next tutorial section or screen
	/// </summary>
	public void OnMouseDown()
	{
		if((tutorialDialogueInstructions[tutorialIndex]=="[unfade]" || tutorialDialogueInstructions[tutorialIndex]=="[close]") )
			AudioManager.instance.Play("pop");

		tutorialIndex++;


		if(submitBtn!=null)
		{
			if(tutorialIndex > 26)
				submitBtn.GetComponent<BoxCollider>().enabled=true;
		}

		if(youWinContinue != null)
		{
			if(tutorialIndex >= 42)
			{
				youWinContinue.interactable=true;
				//youWinRetry.interactable=true;
				youLoseContinue.interactable=true;
				//youLoseRetry.interactable=true;
			}

		}

		if(ApplicationModel.LEVEL_CODE=="B1" && tutorialIndex==25)
		{
			Invoke("LockAllTiles",1f);
		}

		switch(tutorialDialogueInstructions[tutorialIndex])
		{
			//Dialog Instructions   -- this should probably be a Switch(: but it's too late now..
			case "[raise]":
				infoPopup.GetComponent<Animator>().SetTrigger("Raise");
				OnMouseDown();
			break;

			case "[enableInput]":
				if(tutorialIndex==50)
				UnHighlightStartGameBtn();
				GetComponent<BoxCollider2D>().enabled=false;
			break;

			case "[beginTimer]":
				GameMaster.instance.TurnTimer.tutoralProgressFlag=true;
				OnMouseDown();
				return;

			case "[enableInputNext]":
				GetComponent<BoxCollider2D>().enabled=false;
				OnMouseDown();
				return;

			case "[disableInput]":
				GetComponent<BoxCollider2D>().enabled=true;
				return;

			case "[disableInputNext]":
				GetComponent<BoxCollider2D>().enabled=true;
				OnMouseDown();
			return;

			case "[disableInputNext2]":
				Debug.Log("disable input next 2");
				GetComponent<Image>().enabled=false;
				GetComponent<BoxCollider2D>().enabled=false;

				if(ApplicationModel.RETURN_TO_WORLD==-3)
				{
				  tutorialNoBtn.GetComponent<BoxCollider>().enabled=false;

				}
				//OnMouseDown();
			return;

			case "[startgrid]":
				StartGridIntro();
				GUI_Controller.instance.swapTilesBtn.InitlockSubmit();
				OnMouseDown();
				return;
			case "[highlightGrid]":
				gridContainer.transform.position+= new Vector3(0,0,4);
				OnMouseDown();
				return;
			case "[unhighlightGrid]":
				gridContainer.transform.position-= new Vector3(0,0,4);
				OnMouseDown();
				return;

			case "[lower]":
				infoPopup.GetComponent<Animator>().SetTrigger("Lower");
				OnMouseDown();
			return;
			case "[startSolo]":
				GameMaster.instance.TurnTimer.tutoralProgressFlag=true;
				GameMaster.instance.TurnTimer.StartTurn();
				OnMouseDown();
			return;
			case "[return36]":
				tutorialIndex=35;
				OnMouseDown();
			return;
			case "[closeEnd]":
				GetComponent<BoxCollider2D>().enabled=false;
				infoPopup.GetComponent<Animator>().SetTrigger("Close");
				ApplicationModel.TUTORIAL_MODE=false;
				GameMaster.instance.TUTORIAL_MODE=false;
			return;
			case "[highlightTargets]":
				
			return;
			case "[return14]":
			{
				GameMaster.instance.totalTiles=1;
				foreach(GridCell cell in GameMaster.instance.playedTiles)
				{
					if(cell.x==BoardController.instance.GRID_CENTER && cell.y==BoardController.instance.GRID_CENTER )
					{
						Debug.Log("Leave center tile alonnnnnnne :-'(");
					} else 
					{
						if(cell.cellTile!=null)
						{
							//Reset Board Position
							BoardController.instance.gameGrid[cell.x,cell.y]=0;
							BoardController.instance.staticgameGrid[cell.x,cell.y]=0;
							BoardController.instance.lastValidGameGrid[cell.x,cell.y]=0;

							//Add tile bgack to hand
							GameMaster.instance.currentHand.Add(cell.cellTile);

							//Animate Tile Back To Start Location 
							cell.cellTile.GetComponent<GUI_Object>().targetPos = cell.cellTile.startPos;
							GUI_Controller.instance.AnimateTo(cell.cellTile.GetComponent<GUI_Object>(), cell.cellTile.startPos
															+ new Vector3(0, 0, -1), .8f);
							GUI_Controller.instance.RotateObjectBackward(cell.cellTile.gameObject, .8f, 360);

							//Restore tile properties
							cell.cellTile.placed=false;
							cell.cellTile.activated=false;
							cell.cellTile.x=0;
							cell.cellTile.y=0;
							cell.cellTile=null;
						}
					}
				}

				GameMaster.instance.playedTiles.Clear();
				GameMaster.instance.playedTiles.Add(GameMaster.instance.objGameGrid[BoardController.instance.GRID_CENTER,BoardController.instance.GRID_CENTER]);

				tutorialIndex=13;
				OnMouseDown();
				return;

			}  case "[center R]":
				infoPopup.GetComponent<Animator>().SetTrigger("Center R");
				OnMouseDown();
				return;

			 case "[center L]":
				infoPopup.GetComponent<Animator>().SetTrigger("Center L");
				OnMouseDown();
				return;
			 case "[fade]":
				GetComponent<Animator>().SetTrigger("Fade");
				OnMouseDown();
				return;
			 case "[unfade]":
			 Debug.Log("UNFADE");
			 	GetComponent<Image>().enabled=true;
				GetComponent<Animator>().SetTrigger("Unfade");
				OnMouseDown();
				return;
			 case "[highlightSubmit]":
				Invoke("HighlightSubmit",1f);
				OnMouseDown();
				return;
			case "[unHighlightTimer]":
				timerBlock.transform.SetParent(submitBtn.transform.transform.parent);
				GUI_Controller.instance.swapTilesBtn.UnlockSubmit();
				OnMouseDown();
				return;
			case "[highlightTimer]":
				timerBlock.transform.SetParent(this.transform);
				OnMouseDown();
				return;
			case "[unhighlightSubmit]":
				Invoke("UnhighlightSubmit",.75f);
				OnMouseDown();
				return;

			 case "[close]":
				infoPopup.GetComponent<Animator>().SetTrigger("Close");
				GetComponent<BoxCollider2D>().enabled=false;
				return;
			 case "[dealtiles]":
				GameMaster.instance.DealPlayerTiles(1);
				OnMouseDown();
				return;

			case "[gameComplete]":
				infoPopup.GetComponent<Animator>().SetTrigger("Lower L");

				if(GameMaster.instance.playerWin)
				{
					foreach(TextMeshProUGUI txt in winObjectiveTexts)
					{
						//txt.gameObject.transform.position-=new Vector3(0,0,50);
					}
				} else 
				{
					foreach(TextMeshProUGUI txt in loseObjectiveTexts)
					{
						//txt.gameObject.transform.position-=new Vector3(0,0,50);					
					}
				}

				youWinContinue.interactable=false;
				youWinRetry.interactable=false;
				youLoseContinue.interactable=false;
				youLoseRetry.interactable=false;

				OnMouseDown();
			return;

			case "[welcomeMessage]": 
				infoPopup.GetComponent<Animator>().SetTrigger("Raise");
				tutorialDialogueInstructions[3]="Welcome <font=\"FredokaOne-Regular SDF\" material=\"FredokaOne-Regular SDF YELLOW GLOW\">" + AccountInfo.Instance.Info.PlayerProfile.DisplayName + "</font>! Let's play another few games so that you can complete your <font=\"FredokaOne-Regular SDF\" material=\"FredokaOne-Regular SDF YELLOW GLOW\">Daily Challenge!";
				playBtn.GetComponent<BoxCollider>().enabled=false;
				LockUI(0);
				OnMouseDown();
				MenuController.instance.CurrencyUI.layout.enabled=false;
				MenuController.instance.NavBar.layout.enabled=false;
			break;
			case "[highlightPlayBtn]":
				GetComponent<BoxCollider2D>().enabled=false; 
				
				Invoke("HighlightPlayBtn", .25f);
				//LockUI(0);
				
			break;
			case "[unhighlightPlayBtn]": 
				playBtn.transform.position+=new Vector3(0,0,50);
				UnlockUI(0);
				OnMouseDown();
			break;
			case "[highlightDailyChallenge]": 
			
				dailyChallengeBlock.transform.SetParent(this.transform);
				OnMouseDown();
			break;
			case "[unhighlightDailyChallenge]": 
				dailyChallengeBlock.transform.SetParent(dailyChallengeParent.transform);
				OnMouseDown();
			break;
			case "[highlightBeginner]":
				Invoke("HighlightBeginner",1.5f);
				return;
			break;
			case "[unhighlightBeginner]":
				Debug.Log("un highlght beginner");
				beginnerSelection.transform.SetParent(intermediateSelection.transform.parent.transform);
				OnMouseDown();

				if(MenuController.instance.NavBar.challengeModeDialogue.GetComponent<ChallengeModeController>().tutFlag2 == false)
				{
					MenuController.instance.NavBar.challengeModeDialogue.GetComponent<ChallengeModeController>().SelectWorld(0);
				}
				levelSelecBackBtn.GetComponent<GraphicRaycaster>().enabled=false;
				backButton.GetComponent<BoxCollider>().enabled=false;
				//MenuController.instance.OpenC
			break;
			case "Drag an odd-numbered tile into the center to begin!":
				Invoke("SkipDelay", 3.4f);
				tele.label01=tutorialDialogueInstructions[tutorialIndex];
				tele.GetComponent<TextMeshProUGUI>().text=tutorialDialogueInstructions[tutorialIndex];
				tele.autoPlay=true;
				StartCoroutine(tele.Start());
				GetComponent<BoxCollider2D>().enabled=false;
			break;
			// case "Let's play another game!":
			// 	//Invoke("SkipDelay2", 1f);
			// break;
			case "[closeNext]":
				infoPopup.GetComponent<Animator>().SetTrigger("Close");
				//GetComponent<BoxCollider2D>().enabled=false;
				OnMouseDown();
			return;
			case "[closeNextInput]":
				infoPopup.GetComponent<Animator>().SetTrigger("Close");
				GetComponent<BoxCollider2D>().enabled=false;
				OnMouseDown();
			return;
			case "[coinsInfo]":
				GetComponent<BoxCollider2D>().enabled=false;
				MenuController.instance.CurrencyUI.showCoinInfoPanel();
				GUI_Controller.instance.CoinDialogue.transform.SetParent(this.transform);
				Invoke("EnableInput", 2f);
			break;
			case "[energyInfo]":
				GetComponent<BoxCollider2D>().enabled=false;
				//MenuController.instance.CurrencyUI.StopAllCoroutines();
				//StartCoroutine(MenuController.instance.CurrencyUI.ResetInformationPanelAfterDelay(MenuController.instance.CurrencyUI.coinInfoPanel, .1f));
				MenuController.instance.CurrencyUI.showEnergyInfoPanel();
				GUI_Controller.instance.LivesDialogue.transform.SetParent(GUI_Controller.instance.CoinDialogue.transform.parent.transform);
				GUI_Controller.instance.LivesDialogue.transform.SetParent(this.transform);
				Invoke("EnableInput", 2f);
			break;
			case "[starsInfo]":
				GetComponent<BoxCollider2D>().enabled=false;
				//MenuController.instance.CurrencyUI.StopAllCoroutines();
				//MenuController.instance.CurrencyUI.ResetInformationPanelAfterDelay(MenuController.instance.CurrencyUI.starInfoPanel, .1f);
				MenuController.instance.CurrencyUI.showStarInfoPanel();
				GUI_Controller.instance.StarDialogue.transform.SetParent(GUI_Controller.instance.LivesDialogue.transform.parent.transform);
				GUI_Controller.instance.StarDialogue.transform.SetParent(this.transform);
				Invoke("EnableInput", 2f);
				
			break;
			case "[closeStarInfo]":
				//MenuController.instance.CurrencyUI.StopAllCoroutines();
				StartCoroutine(MenuController.instance.CurrencyUI.ResetInformationPanelAfterDelay(MenuController.instance.CurrencyUI.starInfoPanel, .1f));
				GUI_Controller.instance.StarDialogue.transform.SetParent(returnCurrencyContent.transform);
				//Invoke("OnMouseDown", 1.5f);
			break;
			case "[resetCoins]":
				//MenuController.instance.CurrencyUI.ResetCoins();
				OnMouseDown();
			break;
			case "[highlightLevel4]":
				level1StarContainer.SetActive(false);
				lockedLevelSelection.transform.SetParent(this.transform);
				lockedLevelSelection.GetComponentInChildren<GraphicRaycaster>().enabled=false;
				lockedLevel1Selection.GetComponentInChildren<GraphicRaycaster>().enabled=false;
				OnMouseDown();
			break;
			case "[highlightLevel2]":
				tutorial2LevelSelection.transform.SetParent(this.transform);
				lockedLevelSelection.transform.SetParent(returnToContentTransform.transform);
				OnMouseDown();
			break;
			case "[unhighlightLevel2]":
				level1StarContainer.SetActive(true);
				tutorial2LevelSelection.transform.SetParent(returnToContentTransform.transform);
				OnMouseDown();
			break;
			
			case "[highlightRightBtn]":
				
				//UnHighlightStartGameBtn();
				GetComponent<Image>().enabled =true;
				rightBtn.transform.SetParent(this.transform);
				OnMouseDown();
			break;
			case "[highlightStartGameBtn]":
				Debug.Log("HighlightStartGameBtn");
				rightBtn.transform.SetParent(leftBtn.transform.parent);
				levelDetails.transform.SetParent(leftBtn.transform.parent);
				GetComponent<Image>().enabled=true;
				HighlightStartGameBtn();
				OnMouseDown();
			break;
			case "[highlightLevelDetails]":
				//rightBtn.transform.SetParent(leftBtn.transform.parent);
				levelDetails.transform.SetParent(this.transform);
				OnMouseDown();
			break;
			case "[highlightEnergyPanel]":
				
				Debug.Log("highlight Enegry Panel");
				GUI_Controller.instance.CoinDialogue.transform.SetParent(returnCurrencyContent.transform);
				GUI_Controller.instance.StarDialogue.transform.SetParent(returnCurrencyContent.transform);
				GUI_Controller.instance.LivesDialogue.transform.SetParent(this.transform);
				OnMouseDown();
			break;
			case "[highlightStarPanel]":
				GUI_Controller.instance.CoinDialogue.transform.SetParent(returnCurrencyContent.transform);
				GUI_Controller.instance.LivesDialogue.transform.SetParent(returnCurrencyContent.transform);
				GUI_Controller.instance.StarDialogue.transform.SetParent(this.transform);
				OnMouseDown();
			break;
			case "[highlightCoinPanel]":
				GUI_Controller.instance.StarDialogue.transform.SetParent(returnCurrencyContent.transform);
				GUI_Controller.instance.LivesDialogue.transform.SetParent(returnCurrencyContent.transform);
				GUI_Controller.instance.CoinDialogue.transform.SetParent(this.transform);
				OnMouseDown();
			break;
			case "[enableInput2]":
				GUI_Controller.instance.StarDialogue.transform.SetParent(returnCurrencyContent.transform);
				GUI_Controller.instance.LivesDialogue.transform.SetParent(returnCurrencyContent.transform);
				GUI_Controller.instance.CoinDialogue.transform.SetParent(this.transform);
				
				OnMouseDown();
			break;
			case "[highlightTileBox]":
				tutorialTileBox.transform.SetParent(this.transform);
				tutorialTileBox.GetComponentInChildren<Button>().interactable=false;
				OnMouseDown();
			break;
			case "[creditAccount1500]":
				GUI_Controller.instance.CoinDialogue.transform.SetParent(this.transform);
				GUI_Controller.instance.CurrencyUI.AddCurrency(1500);
				AccountInfo.AddInGameCurrency(1500);
				AudioManager.instance.Play("coinLoop");
				tutorialTileBox.GetComponentInChildren<Button>().interactable=true;
				Invoke("StopCoinLoop", 3f);
				OnMouseDown();
			break;
			case "[highlightGameShop]":
				MenuController.instance.NavBar.goodrayTutFlag=true;
				GUI_Controller.instance.CoinDialogue.transform.SetParent(returnCurrencyContent.transform);
				tutorialShopButton.transform.SetParent(this.transform);
				if(shopGoodRay!=null)
					shopGoodRay.SetActive(true);
				OnMouseDown();
			break;
			case "[highlightFullGamePass]":
				Invoke("HighlightFullGamePass", 1.5f);
				//OnMouseDown();
			break;
			case "[highlightEnergyShop]":
				MenuController.instance.InAppPurchasesController.fullGameLevelPack.gameObject.transform.SetParent(returnIAPContent.transform);
				//MenuController.instance.GoToEnergyStore();
				MenuController.instance.InAppPurchasesController.scrollRect.SnapToPositionVertical(MenuController.instance.InAppPurchasesController.energyStoreMarker, MenuController.instance.InAppPurchasesController.scrollRectContent, new Vector3(0,-.5f,0));
				Invoke("HighlightEnergyShop", .75f);
				Debug.LogError("Smooth Scroll Down!");
				//OnMouseDown();
			break;
			case "[highlightBoosterShop]":
				energyShop.transform.SetParent(returnIAPContent.transform);
				boosterShop.transform.SetParent(this.transform);
				Debug.LogError("Smooth Scroll Down!");
				MenuController.instance.GoToCoinStore();
				OnMouseDown();
			break;
			case "[highlightCoinShop]":
				boosterShop.transform.SetParent(returnIAPContent.transform);
				coinShop.transform.SetParent(this.transform);
				MenuController.instance.GoToCoinStore();
				OnMouseDown();
			break;
			case "[unhighlightCoinShop]":
				coinShop.transform.SetParent(returnIAPContent.transform);
				OnMouseDown();
			break;
			case "[socialPanel]":
				energyShop.transform.SetParent(returnIAPContent.transform);
				MenuController.instance.InAppPurchasesController.UnlockEnergyShopButtons();
				Invoke("SocialPanel", .65f);
				//OnMouseDown();
			break;
			case "[referalPanel]":
				Invoke("ReferalPanel", 1.65f);
				//OnMouseDown();
			break;
			case "[endTutorial]":
				energyShop.transform.SetParent(returnIAPContent.transform);
				tutorialShopButton.transform.SetParent(MenuController.instance.NavBar.btn_1.transform);
				MenuController.instance.OpenMainMenu();
				tutorialTileBox.SetActive(false);
				tutorialTileBox.SetActive(true);
				tutorialTileBox.transform.SetSiblingIndex(1);
				OnMouseDown();
			break;
			case "[closeFinish]":
				ApplicationModel.TUTORIAL_MODE=false;
				this.gameObject.SetActive(false);
			break;
			case "[HighlightTargetStars]":
					HighlightTargetStars();
			break;
			case "[HighlightTargetScores]":
					HighlightTargetScorePanels();
			break;
			case "[HighlightRemainingTurns]":
					HighlightRemainingTurnsPanel();
			break;
			case "[UnhighlightRemainingTurns]":
					UnhighlightRemainingTurnsPanel();
			break;
			default: 
				//ELSE the intruction is a text panel update
				tele.label01=tutorialDialogueInstructions[tutorialIndex];
				tele.GetComponent<TextMeshProUGUI>().text=tutorialDialogueInstructions[tutorialIndex];
				tele.autoPlay=true;
				StartCoroutine(tele.Start());
				GetComponent<BoxCollider2D>().enabled=false;
			break;
			
		}

		
	}

	private void EnableInput()
	{
		GetComponent<BoxCollider2D>().enabled=true;
	}

	private void HighlightBeginner()
	{
		beginnerSelection.transform.SetParent(this.transform);
		OnMouseDown();

	}

	public void UnhighlightPlayBtn()
	{
		Debug.Log("UNHIGHLIGHT FOR FUCK SAKE MAN");
		startGameBtn.transform.SetParent(leftBtn.transform.parent);
		startGameBtn.transform.position=new Vector3(startGameBtn.transform.position.x,startGameBtn.transform.position.y,backButton.transform.position.z);
	}

	private void HighlightTargetStars()
	{
		targetLayout.enabled=false;
		targetStarsPanel.transform.SetParent(this.transform);
		OnMouseDown();
	}

	private void HighlightTargetScorePanels()
	{
		targetStarsPanel.transform.SetParent(targetLayout.transform);
		currentScorePanel.transform.SetParent(this.transform);
		targetScorePanel.transform.SetParent(this.transform);
		OnMouseDown();
	}

	private void HighlightRemainingTurnsPanel()
	{
		currentScorePanel.transform.SetParent(targetLayout.transform);
		targetScorePanel.transform.SetParent(targetLayout.transform);
		remianingTurnsPanel.transform.SetParent(this.transform);
		OnMouseDown();
	}

	private void UnhighlightRemainingTurnsPanel()
	{
		remianingTurnsPanel.transform.SetParent(targetLayout.transform);
		OnMouseDown();
	}



	private void SocialPanel()
	{
		MenuController.instance.NavBar.PressSocialButton();
		OnMouseDown();

	}

	private void ReferalPanel()
	{
		MenuController.instance.NavBar.PressReferalButton();
		OnMouseDown();

	}

	private void HighlightFullGamePass()
	{
		MenuController.instance.InAppPurchasesController.fullGameLevelPack.gameObject.transform.SetParent(this.transform);
		MenuController.instance.InAppPurchasesController.fullGameLevelPack.gameObject.GetComponent<Button>().enabled=false;
		OnMouseDown();
	}

	public void TestFunc()
	{
		MenuController.instance.tutorialController.gameObject.SetActive(true);
		MenuController.instance.NavBar.PressShopButton();
		tutorialIndex=68;
		OnMouseDown();
	}

	private void HighlightEnergyShop()
	{
		MenuController.instance.InAppPurchasesController.LockEnergyShopButtons();
		MenuController.instance.InAppPurchasesController.fullGameLevelPack.gameObject.GetComponent<Button>().enabled=true;
		energyShop.transform.SetParent(this.transform);
		OnMouseDown();
	}

	

	public void SkipDelay2()
	{
		OnMouseDown();
	}

	private void StopCoinLoop()
	{
		AudioManager.instance.Stop("coinLoop");
	}

	private void LockAllTiles()
	{
		Scene scene = SceneManager.GetActiveScene();
		if(scene.name != "Main")
		{
			return;

		}
		foreach(GridCell cell in GameMaster.instance.playedTiles)
		{
			Destroy(cell.cellTile.collider);
		}

	}

	private void SkipDelay()
	{
		if(tutorialIndex>=10)
		return;

		OnMouseDown();
	}

	private void HighlightPlayBtn()
	{
		startGameBtn.GetComponent<BoxCollider>().enabled=false;
		playBtn.GetComponent<BoxCollider>().enabled=true;
		playBtn.transform.position-=new Vector3(0,0,50);
		MenuController.instance.NavBar.LockNavBar();
		OnMouseDown();
	}

	private void HighlightStartGameBtn()
	{
		backButton.GetComponent<BoxCollider>().enabled=false;
		startGameBtn.GetComponent<BoxCollider>().enabled=true;
		startGameBtn.transform.position-=new Vector3(0,0,50);
	}

	private void UnHighlightStartGameBtn()
	{
		startGameBtn.transform.localPosition = new Vector3(293,-614,-135);
		startGameBtn.transform.SetParent(leftBtn.transform.parent);
		
	}

	private void HighlightSubmit()
	{
		submitBtn.transform.position-=new Vector3(0,0,50);
	}

	private void HighlightTimer()
	{
		timerBlock.transform.position-=new Vector3(0,0,50);
	} 

	private void UnhighlightSubmit()
	{
		submitBtn.transform.position+=new Vector3(0,0,50);
	}

	public void StartGridIntro()
	{
		GameMaster.instance.gameObject.SetActive(true);
		GameMaster.instance.enabled=true;
		GUI_Controller.instance.gameObject.SetActive(true);
		StartCoroutine(GUI_Controller.instance.GridIntroAnim());
	}

	public void IncrementTutorial()
	{
		OnMouseDown();
	}

	public void IncrementTutorialAfterDelay()
	{
		Debug.LogError("INCREMNT TUT AFTER DELAY");
		Invoke("OnMouseDown", 1.5f);
	}
}
