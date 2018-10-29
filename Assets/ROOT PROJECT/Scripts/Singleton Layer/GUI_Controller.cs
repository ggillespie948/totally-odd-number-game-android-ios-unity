using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
 //using UnityEditor;
 using EZCameraShake;
 using TMPro.Examples;

public class GUI_Controller : MonoBehaviour, Observable {

    public static GUI_Controller instance = null;
    
    public List<Observer> observerList;
    public int animationCount = 0;
    public TextMeshProUGUI menuDebugTxt;
    public GameObject LastPlacedTile;
    [Header("Popup Fonts")]
    public TMP_FontAsset orangeGlow;
    public GameObject TimerUI;
    public Transform exchangePoint;
    [Header("Grid Options")]
    public GameObject NotificationParent;
    //GUI Gameobjects
    public GameObject buttons;
    [Header("UI Text")]
    public GameObject ScoreText;
    [Header("Active GAme Grid")]
    public GameObject physcialGridContainer;
    public GameObject[] physicalGridBG;
    public GameObject physicalGridDivider;
    
    public AnimationCurve ElasticCurve;
    public AnimationCurve RotationCurve;
    [Header("Portrait Anchors")]
    public GameObject[] InactiveCardPositions;
    [Header("Grid Options")]
    public GameObject UI_GRADIENT_BG;
    public GameObject GameGrid_5x5;
    public GameObject[] GameGridBG_5x5;
    public GameObject GameGrid_5x5_MainBG;
    public GameObject GameGrid_7x7;
    public GameObject[] GameGridBG_7x7;
    public GameObject GameGrid_7x7_MainBG;
    public GameObject GameGrid_9x9;
    public GameObject[] GameGridBG_9x9;

    public GameObject GameGrid_9x9_MainBG;
    [SerializeField]
    public Vector3 Grid_Scale;
    [SerializeField]
    public GameObject GameOverFX;
    //FX
    public GameObject[] Tile1ActivateFX;
    public GameObject[] Tile2ActivateFX;
    public GameObject[] Tile3ActivateFX;
    public GameObject[] Tile4ActivateFX;
    public GameObject[] Tile5ActivateFX;
    public GameObject[] Tile6ActivateFX;
    public GameObject[] Tile7ActivateFX;
    public GameObject[] Tile8ActivateFX;
    public GameObject[] Tile9ActivateFX;

    [SerializeField]
    public List<GameObject[]> tileFX;
    
    public Component[] GridTiles;

    [Header("Game UI objects")]
    public GameObject Rotate_Tiles_Btn;
    public GameObject Submit_Button;

    [Header("Active Player Cards")]
    public PlayerCard PlayerCard1;
    public PlayerCard PlayerCard2;
    public PlayerCard PlayerCard3;
    public PlayerCard PlayerCard4;
    

    public List<PlayerCard> PlayerCards = new List<PlayerCard>();
    public Light directionalLight;
    private static TextPopup POPUP_TEXT;
    private static TextPopup POPUP_SCORE;
    private static TextPopup POPUP_CASH;
    private static TextPopup POPUP_ERROR;

    [Header("GUI Effect Listener")]
    [SerializeField]
    public List<GridTile> TilesScored = new List<GridTile>(); //this is a list of tiles that are involved an a round of player moves, used to activate score trail effect
    public GUI_Object TargetScore_Stone;
    public GUI_Object RemainingTurns_Stone;
    public GUI_Object PlayerCoins_Stone;
    public TextMeshProUGUI remainingTurnText;
    public TextMeshProUGUI remainingTilesText;
    public TextMeshProUGUI remainingTilesIndicatorText;
    public TextMeshProUGUI targetScoreText;
    public GameObject SoloTargetCard;
    public GameObject SoloScoreCard;

    public ActionButtonController actionBtncontroller;

    [Header("GUI Dialogues")]
    public GUI_Dialogue_Controller DialogueController;
    public GameObject TargetStar1;
    public GameObject TargetStar1FX;
    public GameObject TargetStar2;
    public GameObject TargetStar2FX;
    public GameObject TargetStar3;
    public GameObject TargetStar3FX;
    public Sprite ActiveStar;
    public GameObject ActionButtons;
    public GameObject PauseMenu;
    public GameObject SettingsButton;
    public GameObject SoundButton;
    public GameObject MusicButton;
    public GameObject CoinDialogue;
    public GameObject LivesDialogue;
    public GameObject StarDialogue;
    public int inactiveCardCount=0;   
    public GameObject Confetti; 
    public GameObject GridCompleteAnim;
    public GameObject NoMovesAnim;

    public CurrenyBarController CurrencyUI;

    [Header("Pause Menu Objective Text")]
    public TextMeshProUGUI pauseObjective1;
    public TextMeshProUGUI pauseObjective2;
    public TextMeshProUGUI pauseObjective3;

    

    public GameObject EndTurn_Btn;
    public GameObject EndTurn_Flag;

    public GameObject confirmPurchasePanel;
    public GameObject confirmPurchasePanelTarget;

    public GameObject starPanel;


    [Header("Currency Reward Panel")]
    [SerializeField]
    private GameOverSequenceController GameOverPanelController;


    public swapTilesBtn swapTilesBtn;

    


    void Awake()
    {
        if(instance == null)
        {
            
        }
        instance = this;

        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        if(!POPUP_TEXT)
            POPUP_TEXT = Resources.Load<TextPopup>("PopupTextParent");

        if(!POPUP_SCORE)
            POPUP_SCORE = Resources.Load<TextPopup>("PopupTextParent");

        if(!POPUP_CASH)
            POPUP_CASH = Resources.Load<TextPopup>("PopupCashParent");

        if(!POPUP_ERROR)
            POPUP_ERROR = Resources.Load<TextPopup>("PopupTextError");

        DialogueController = this.GetComponent<GUI_Dialogue_Controller>();
        Time.timeScale = 1f;
        observerList = new List<Observer>();
       
    }

    
    void Start()
    {
        if(PlayerCard1 != null){PlayerCards.Add(PlayerCard1);}
        if(PlayerCard2 != null){PlayerCards.Add(PlayerCard2);}
        if(PlayerCard3 != null){PlayerCards.Add(PlayerCard3);}
        if(PlayerCard4 != null){PlayerCards.Add(PlayerCard4);}
        if(PlayerCard1!= null)
        {
            for(int i=0; i<GameMaster.MAX_TURN_INDICATOR; i++)
            {
                GUI_Controller.instance.PlayerCards[i].SetQueuePos(i);
                if(GameMaster.instance.humanTurn)
                    GUI_Controller.instance.PlayerCards[i].UpdateName("Player " + (i+1));
                else
                    GUI_Controller.instance.PlayerCards[i].UpdateName("AI " + (i+1));

                    if(ApplicationModel.TUTORIAL_MODE)
                        GUI_Controller.instance.PlayerCards[1].UpdateName("Tutorial AI");
            }
            GUI_Controller.instance.PlayerCards[0].SetQueuePos(1);
            int indx=0;
            foreach(PlayerCard p in GUI_Controller.instance.PlayerCards)
            {
                p.SetRingCol(indx);
                indx++;
            }
        }
        Time.timeScale = 1f;
    }

    public void SwitchEndTurnButton()
    {
        if(GameMaster.instance.humanTurn)
        {
            //EndTurn_Btn.GetComponentInChildren<TextMeshProUGUI>().text="End Turn";

        } else 
        {
            //EndTurn_Btn.GetComponent<Button>().interactable=false;
            //EndTurn_Btn.GetComponentInChildren<TextMeshProUGUI>().text="Opponent's turn";
        }
    }


    public void ToggleCurrencyUI(bool state)
    {
        CoinDialogue.SetActive(state);
        LivesDialogue.SetActive(state);
    }
    

    public void ToggleAllVsAiUI()
    {
        if(PlayerCard1 != null) { PlayerCard1.gameObject.SetActive(PlayerCard1.gameObject.activeSelf); }
        if(PlayerCard2 != null) { PlayerCard2.gameObject.SetActive(PlayerCard1.gameObject.activeSelf); }
        if(PlayerCard3 != null) { PlayerCard3.gameObject.SetActive(PlayerCard1.gameObject.activeSelf); }
        if(PlayerCard4 != null) { PlayerCard4.gameObject.SetActive(PlayerCard1.gameObject.activeSelf); }
    }

    

    //IEnumerators For Animation        //temp - could refactor into scale object ?
    public IEnumerator GridIntroAnim()
    {
        physcialGridContainer.transform.localPosition = new Vector3 (GameMaster.GRID_X, GameMaster.GRID_Y, 110 );
        physcialGridContainer.SetActive(true);
        float scaleDuration = .87f;                                //animation duration in seconds
        Vector3 actualScale = physcialGridContainer.transform.localScale;             // scale of the object at the begining of the animation
        physcialGridContainer.transform.localScale = new Vector3(4, 4, 1);
        float curveTime = 0.0f;
        float curveAmount = ElasticCurve.Evaluate(curveTime);

        while (curveTime <= 1.0f)
        {
            curveTime += Time.deltaTime * scaleDuration;
            curveAmount = ElasticCurve.Evaluate(curveTime);
            physcialGridContainer.transform.localScale = new Vector3(Grid_Scale.x * curveAmount, Grid_Scale.y * curveAmount, Grid_Scale.z * curveAmount);
            //Move to Animate pos
            yield return null;
        }

        physcialGridContainer.transform.localScale = new Vector3(Grid_Scale.x , Grid_Scale.y , Grid_Scale.z );
        


        GameMaster.instance.InitGame();
    }

    public void EnableGridBoxColliders()
    {
        foreach(GridCell cell in GameMaster.instance.objGameGrid)
        {
            if(cell != null){cell.gameObject.GetComponent<BoxCollider>().enabled=true;}
        }
    }

    public void DisableGridBoxColliders()
    {
        Debug.LogError("Disable Grid Box Colliders");
        foreach(GridCell cell in GameMaster.instance.objGameGrid)
        {
            if(cell != null){cell.gameObject.GetComponent<BoxCollider>().enabled=false;}
        }
    }

    //IEnumerators For Animation
    public IEnumerator GridOutroAnim()
    {
        yield return new WaitForSeconds(1.7f);
        float scaleDuration = .87f;                                //animation duration in seconds
        Vector3 actualScale = physcialGridContainer.transform.localScale;             // scale of the object at the begining of the animation
        physcialGridContainer.transform.localScale = new Vector3(4, 4, 1);
        Vector3 targetScale = new Vector3(.51f, .51f, .51f);     // scale of the object at the end of the animation
        float curveTime = 1.0f;
        float curveAmount = ElasticCurve.Evaluate(curveTime);

        while (curveTime >= 0.0f)
        {
            curveTime -= Time.deltaTime * scaleDuration;
            curveAmount = ElasticCurve.Evaluate(curveTime);
            physcialGridContainer.transform.localScale = new Vector3(targetScale.x * curveAmount, targetScale.y * curveAmount, targetScale.z * curveAmount);
            buttons.transform.localScale = new Vector3(targetScale.x * curveAmount, targetScale.y * curveAmount, targetScale.z * curveAmount);
            //Move to Animate pos
            yield return null;
        }

        DestroyAllTiles();
    }

    public IEnumerator UpdatePlayerScore(int startScore, int newScore, int player)
    {
        while(startScore < newScore+1)
        {
            GUI_Controller.instance.PlayerCards[player-1].UpdateCard(startScore);
            if(startScore-5 >= newScore)
            {
                startScore += UnityEngine.Random.Range(1,5);

            } else {
                startScore++;
            }
            yield return new WaitForSeconds(.03f);
        }
        
    }

    public IEnumerator UpdateUIScore(int startScore, int newScore, TextMeshProUGUI text)
    {
        while(startScore < newScore+1)
        {

            text.text = startScore.ToString();
            startScore+=UnityEngine.Random.Range(1, 5);
            yield return new WaitForSeconds(UnityEngine.Random.Range(.05f, .15f));
        }
        startScore = newScore;
        text.text = startScore.ToString();

    }

    public IEnumerator UpdateUIScore(int startScore, int newScore, TextMeshProUGUI text, string prefix)
    {
        if(ApplicationModel.SOLO_PLAY)
        {
            text=GameOverPanelController.GetComponent<IntroPanelController>().soloGameOverSection.GetComponentInChildren<TextMeshProUGUI>();
        }

        while(startScore < newScore+1)
        {
            text.text = prefix + " " + startScore.ToString();
            startScore+=UnityEngine.Random.Range(1, 10);
            yield return new WaitForSeconds(UnityEngine.Random.Range(.005f, .05f));
        }

        text.text = prefix + " " + newScore.ToString();

        yield return new WaitForSeconds(1.5f);

        if(GameMaster.instance.gameOver)
            GameOverPanelController.GameResultCallBack();

    }

    public IEnumerator RotateObjectForward(float rotTime, GameObject _gameObject)
    {
        float curveTime = 0.0f;
        Quaternion newRot = Quaternion.Euler(_gameObject.transform.rotation.x + 180, _gameObject.transform.rotation.y, _gameObject.transform.rotation.z);
        Quaternion startRot = Quaternion.Euler(_gameObject.transform.rotation.x, _gameObject.transform.rotation.y, _gameObject.transform.rotation.z);

        while (curveTime <= rotTime)
        {
            _gameObject.transform.rotation = Quaternion.Lerp(startRot, newRot, RotationCurve.Evaluate(curveTime / rotTime));
            curveTime += Time.deltaTime;
            yield return null;
        }

        _gameObject.transform.rotation = newRot;

    }

    public int rotation = 0;

    public IEnumerator RotateTiles(float rotTime, Component[] _gameObject, int rotAmount)
    {
        if(_gameObject == null)
            yield return null;

        float curveTime = 0.0f;
        Quaternion newRot = Quaternion.Euler(_gameObject[0].transform.rotation.x, _gameObject[0].transform.rotation.y, _gameObject[0].transform.rotation.z+rotAmount);
        Quaternion startRot = Quaternion.Euler(_gameObject[0].transform.rotation.x, _gameObject[0].transform.rotation.y, rotation);
        
        while (curveTime <= rotTime)
        {
            foreach(GridTile Obj in _gameObject)
            {
                Obj.transform.rotation = Quaternion.Lerp(startRot, newRot, RotationCurve.Evaluate(curveTime / rotTime));
            }
            curveTime += Time.deltaTime;
            yield return null;
        }

        foreach(GridTile Obj in _gameObject)
        {
            Obj.transform.rotation = newRot;
        }

    }

    public void StarRewardAnim(string starString)
    {
        int starReward = 0;
        int i =0;

        if(AccountInfo.Instance==null)
        {
            Debug.LogError("Account info is null: CALL ALWAYS ONLINE");
            return;
        }

        //Debug.LogError("Begining star reward animation");

        char[] sString;        

        if(!ApplicationModel.CUSTOM_GAME)
        {
            sString = AccountInfo.worldStars[ApplicationModel.WORLD_NO, ApplicationModel.LEVEL_NO].ToCharArray();
        } else 
        {
            sString = "111".ToCharArray();
        }
        
        
        //Debug.LogError("sString: (existing) " + AccountInfo.worldStars[ApplicationModel.WORLD_NO, ApplicationModel.LEVEL_NO]);
        //Debug.LogError("starString (from level)" + starString);

		for(int il=0; il<3; il++)
		{
			if(sString[il]=='0'&&starString[il]=='1')
			{
                Debug.Log("Improvement");
                i++;

                switch(il)
                {
                    case 0:
                   // Debug.LogError("Spawn star 1");
                    StartCoroutine(SpawnCashPopup("+1", Color.yellow, 
                    DialogueController.ActiveDialogue.objectiveStar1.transform, (i)+.15f, "")); 
                    starReward+=1;
                    break;

                    case 1:
                    //Debug.LogError("Spawn star 2");
                    StartCoroutine(SpawnCashPopup("+1", Color.yellow, 
                    DialogueController.ActiveDialogue.objectiveStar2.transform, (i)+.15f, "")); 
                    starReward+=1;
                    break;

                    case 2:
                   // Debug.LogError("Spawn star 3");
                    StartCoroutine(SpawnCashPopup("+1", Color.yellow, 
                    DialogueController.ActiveDialogue.objectiveStar3.transform, (i)+.15f, "")); 
                    starReward+=1;
                    break;

                }


				
			} else 
            {
                //Debug.LogError("No improvement");
            }
		}

        if(starReward>0 && !ApplicationModel.CUSTOM_GAME)
        {
            StartCoroutine(currecnyDealyAdd(starReward, 4.6f));
            if(AccountInfo.Instance != null)
            AccountInfo.Instance.PlayerEvent_CompletedGame(GameMaster.instance.playerWin, ApplicationModel.GRID_SIZE, ApplicationModel.MAX_TILE, GameMaster.instance.playerScores[0], ApplicationModel.SOLO_PLAY);
        }
        
        
    }

    private IEnumerator currecnyDealyAdd(int s, float d)
    {
        yield return new WaitForSeconds(d);
        GUI_Controller.instance.StarDialogue.GetComponent<Image>().color=Color.green;
        //CurrencyUI.AddStar(s);
    }

    public GameObject quitBtn;
    public GameObject quitBtnMesh;

    public void PauseGame()
    {
        //ActionButtons.SetActive(false);
        PauseMenu.SetActive(true);
        PauseMenu.GetComponent<Image>().enabled=true;

        if( (ApplicationModel.TUTORIAL_MODE && ApplicationModel.LEVEL_CODE=="B1") || ApplicationModel.LEVEL_CODE=="B2" )
        {
            quitBtn.GetComponent<BoxCollider>().enabled=false;
            quitBtnMesh.GetComponent<Renderer>().material.color=Color.grey;
            quitBtnMesh.GetComponent<Renderer>().material.DisableEmission();
        }

        if(ApplicationModel.FX_ENABLED)
        {
            AudioManager.instance.soundBtnMesh.GetComponent<Renderer>().material=AudioManager.instance.greenMat;
        } else
        {
            AudioManager.instance.soundBtnMesh.GetComponent<Renderer>().material=AudioManager.instance.redMat;
        }

        if(ApplicationModel.MUSIC_ENABLED)
        {
           AudioManager.instance.musicBtnMesh.GetComponent<Renderer>().material=AudioManager.instance.greenMat;
        } else
        {
            AudioManager.instance.musicBtnMesh.GetComponent<Renderer>().material=AudioManager.instance.redMat;
        }

        pauseObjective1.text= GameMaster.instance.PlayerStatistics.GenerateObjectiveText(ApplicationModel.Objective1Code);
        pauseObjective2.text= GameMaster.instance.PlayerStatistics.GenerateObjectiveText(ApplicationModel.Objective2Code);
        pauseObjective3.text= GameMaster.instance.PlayerStatistics.GenerateObjectiveText(ApplicationModel.Objective3Code);
        Time.timeScale = 0f;
        if(GameMaster.instance.vsAi || ApplicationModel.VS_LOCAL_MP)
        {
            ToggleAllVsAiUI();

        } 
    }

    /// <summary>
    /// called via button
    /// </summary>
    public void UnpauseGame()
    {
        PauseMenu.GetComponentInChildren<Animator>().SetTrigger("hide");
        PauseMenu.GetComponent<Image>().enabled=false;
        if(GameMaster.instance.vsAi || ApplicationModel.VS_LOCAL_MP)
        {
            ToggleAllVsAiUI();

        }
        Time.timeScale = 1f;
        GameMaster.instance.ResumeGame();
        Invoke("UnpauseInvoke",1.5f);
    }

    /// <summary>
    /// called via above invoke
    /// </summary>
    private void UnpauseInvoke()
    {
        PauseMenu.SetActive(false);
    }

    public void  GameOver()
    {
        ForceDisableAllEmissions();
        DisableAllEmissions();
        Invoke("GridFullEffect", 3f);
        Invoke("EndGame", 7.5f);
    }

    public void EndGame()
    {
        Debug.LogError("GUI CONTROL GAME OVER!");
        LoadGameOverPanel(); // this also enables win/lose text after a timer

        if(GameMaster.instance.vsAi || ApplicationModel.VS_LOCAL_MP)
        {
            ToggleAllVsAiUI();
        } else 
        {
            //ToggleAllSoloUI();
            actionBtncontroller.HideTilesLeft();
        }

        Time.timeScale = 1.0f;
        GameOverFX.SetActive(true);
        GridTiles = GetComponentsInChildren<GridTile>();
        foreach (GridTile tile in GridTiles)
        {
            tile.GetComponent<Rigidbody2D>().gravityScale = UnityEngine.Random.Range(1f, 4f);
            Destroy(tile.gameObject,3f);
        }
        DestroyAllTiles();
        StartCoroutine(GridOutroAnim());
        if(PlayerCard1 != null){Destroy(PlayerCard1.gameObject);}
        if(PlayerCard2 != null){Destroy(PlayerCard2.gameObject);}
        if(PlayerCard3 != null){Destroy(PlayerCard3.gameObject);}
        if(PlayerCard4 != null){Destroy(PlayerCard4.gameObject);}
        if(SoloTargetCard.gameObject.transform.parent!=null){Destroy(SoloTargetCard.gameObject.transform.parent.gameObject);}
        Destroy(TimerUI);
        
        int res=-1;
        if(AccountInfo.playfabId != null)
        {
            if(AccountInfo.Instance.Info.UserVirtualCurrency.TryGetValue(AccountInfo.COINS_CODE, out res))
			{
				GUI_Controller.instance.CoinDialogue.GetComponentInChildren<TextMeshProUGUI>().text =  res.ToString();
				GUI_Controller.instance.CurrencyUI.playerCoins=res;
			}

			if(AccountInfo.Instance.Info.UserVirtualCurrency.TryGetValue(AccountInfo.LIVES_CODE, out res))
			{
				GUI_Controller.instance.LivesDialogue.GetComponentInChildren<TextMeshProUGUI>().text =  res.ToString();
				GUI_Controller.instance.CurrencyUI.playerLives=res;
			}

			GUI_Controller.instance.StarDialogue.GetComponentInChildren<TextMeshProUGUI>().text = (GUI_Controller.instance.CurrencyUI.playerStars).ToString();
        }
    }

    public void LoadGameOverPanel()
    {
        GameOverPanelController.GetComponent<IntroPanelController>().enabled=false;
        GameOverPanelController.enabled=true;
        GameOverPanelController.GetComponent<Animator>().Rebind();
        GameOverPanelController.gameObject.SetActive(true);
    }

    public GridTile[] GetAllTiles()
    {
        return GetComponentsInChildren<GridTile>();
    }

    public void DisableAllEmissions()
    {
        //Debug.LogWarning("Disabling All Tiles emissions");
        GridTiles = GetComponentsInChildren<GridTile>();
        foreach (GridTile tile in GridTiles)
        {
            if(!tile.isFlashing && (tile.placed || tile.placedByAI) )
            {
                tile.GetComponent<Renderer>().material.DisableEmission();
            }
        }
    }

    public void ForceDisableAllEmissions()
    {
        //Debug.LogWarning("Force Disabling All Tiles emissions");
        GridTiles = GetComponentsInChildren<GridTile>();
        foreach (GridTile tile in GridTiles)
        {
            tile.GetComponent<Renderer>().material.DisableEmission();

        }
    }

    public void EnableAllEmissions()
    {
        if(!GameMaster.instance.gameOver)
            StartCoroutine(EnableEM(.5f));
    }

    /// <summary>
    /// Enables emissions of all tiles after a set delay
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnableEM(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(GameMaster.instance.gameOver)
            yield return null;

        GridTiles = GetComponentsInChildren<GridTile>();
        foreach (GridTile tile in GridTiles)
        {
            if(!tile.isFlashing && tile.activated)
            {
                StartCoroutine(tile.GetComponent<GUI_Object>().GlowToEmission(0,.6f,2f, false));
            }
            tile.isFlashing=false;
            if(!GameMaster.instance.gameOver || !GameMaster.instance.gridFull)
                tile.GetComponent<Renderer>().material.EnableEmission();
            
            tile.GetComponent<Renderer>().material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        }

    }

    public void DestroyAllTiles()
    {
        GridTiles = GetComponentsInParent<GridTile>();
        foreach (GridTile tile in GridTiles)
        {
            tile.gameObject.SetActive(false);
        }

        GridTiles = GetComponentsInChildren<GridTile>();
        foreach (GridTile tile in GridTiles)
        {
            Destroy(tile.gameObject,2f);
        }

    } 

    private IEnumerator Rotation(float rotTime, GameObject _gameObject, int rotAmount)
        {
            // Initialize the time variables
            float currentTime = 0f;
            float duration = rotTime;
 
            // Figure out the current angle/axis
            Quaternion sourceOrientation = _gameObject.transform.rotation;
            float sourceAngle;
            Vector3 sourceAxis;
            sourceOrientation.ToAngleAxis(out sourceAngle, out sourceAxis);
   
            // Calculate a new target orientation
            float targetAngle = (float)rotAmount; //(UnityEngine.Random.value - 0.5f) * 3600f + sourceAngle; // Source +/- 1800
            Vector3 targetAxis = UnityEngine.Random.onUnitSphere;
 
            while (currentTime < duration)
            {
                // Might as well wait first, especially on the first iteration where there'd be nothing to do otherwise.
                yield return null;
 
                currentTime += Time.deltaTime;
                float progress = currentTime / duration; // From 0 to 1 over the course of the transformation.
 
                // Interpolate to get the current angle/axis between the source and target.
                float currentAngle = Mathf.Lerp(sourceAngle, targetAngle, progress);
                Vector3 currentAxis = Vector3.Slerp(sourceAxis, targetAxis, progress);
               
                // Assign the current rotation
                _gameObject.transform.rotation = Quaternion.AngleAxis(currentAngle, currentAxis);
            }
        }

    public void ActivateTile(GridTile tile) 
    {
        if (tile != null)
        {
            //AudioManager.instance.Play("play11");
            tile.ActiveTileSkin();

            if(tileFX[GameMaster.instance.turnIndicator-1][tile.value-1] == null)
            {
                Debug.LogError("AWOOGA AWOOGA ABANDON SHIP AWOOGA AWOOGA THIS IS NOT A DIRLL");
                Debug.LogError("tile.value" + tile.value);
                Debug.LogError("turn indicator" + GameMaster.instance.turnIndicator);
                Debug.LogError("Tile FX length:" + tileFX.Count);
                Debug.LogError("Tile FX (turn indicator-1) length:" + tileFX[GameMaster.instance.turnIndicator-1].Length);


            } else 
            {
             GameObject FX;
                FX = Instantiate(tileFX[GameMaster.instance.turnIndicator-1]
                [tile.value-1],
                 GameMaster.instance.objGameGrid[tile.x,tile.y].transform.position-
                    new Vector3(0,0,5),
                     Quaternion.Euler(0,0,0));

                FX.transform.localScale=AdjustFXScale();
                Destroy(FX, 1f);
            }

            
           
            tile.activated = true;
        } 
    }

    public void TilesScoredEffect(int score)
    {
        //Remove duplicates
        TilesScored = TilesScored.Distinct().ToList();

        NotifyObservers(this, "Event.ScoreSound."+score);

        if(TilesScored.Count < 8)
            TileScoreFlash(score);
        else if(TilesScored.Count < 15)
            StartCoroutine(TileScoreFlip(score));
        else
            StartCoroutine(TileScoreChain(score));

    }

    private void TileScoreFlash(int score)
    {
        
        for(int i=0; i<TilesScored.Count; i++)
        {
            if(TilesScored[i] != null)
            {
                if(!TilesScored[i].activated){GUI_Controller.instance.ActivateTile(TilesScored[i]);}
                TilesScored[i].isFlashing = true;
                TilesScored[i].GetComponent<Renderer>().material.EnableEmission();
                TilesScored[i].transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
                TilesScored[i].GetComponent<NoGravity>().enabled = false;
                StartCoroutine(TilesScored[i].GetComponent<GUI_Object>().Flash(TilesScored[i].GetComponent<Renderer>().material.color,2.5f, true));
            }
        }

       //TilesScored.Clear();
       GUI_Controller.instance.SpawnTextPopup("+"+(+score), Color.gray, 
                GUI_Controller.instance.transform, (score+10));
        if(GameMaster.instance.playedTiles.Count >0)
            BoardController.instance.EventScore(score);
        GameMaster.instance.playedTiles.Clear();
    }

    private IEnumerator TileScoreChain(int score)
    {
        float time = 1f/TilesScored.Count;
        if(time>.2f)
            time=.2f;
        WaitForSeconds wait = new WaitForSeconds( time ) ;
        for(int i=0; i<TilesScored.Count; i++)
        {
            if(TilesScored[i] != null)
            {
                TilesScored[i].isFlashing = true;
                if(!TilesScored[i].activated){GUI_Controller.instance.ActivateTile(TilesScored[i]);}
                TilesScored[i].GetComponent<Renderer>().material.EnableEmission();
                TilesScored[i].GetComponent<Animator>().enabled=true;
                TilesScored[i].GetComponent<Animator>().SetTrigger("TileScored");
                yield return wait;
            }
        }

       //TilesScored.Clear();
       GUI_Controller.instance.SpawnTextPopup("+"+(+score), Color.gray, 
                GUI_Controller.instance.transform, (score+10));
        if(GameMaster.instance.playedTiles.Count >0)
            BoardController.instance.EventScore(score);
        GameMaster.instance.playedTiles.Clear();
    }

    private IEnumerator TileScoreFlip(int score)
    {
        float time = 1f/TilesScored.Count;
        if(time>.2f)
            time=.2f;
        WaitForSeconds wait = new WaitForSeconds( time ) ;
        for(int i=0; i<TilesScored.Count; i++)
        {
            if(TilesScored[i] != null)
            {
                TilesScored[i].isFlashing = true;
                if(!TilesScored[i].activated){GUI_Controller.instance.ActivateTile(TilesScored[i]);}
                TilesScored[i].GetComponent<Renderer>().material.EnableEmission();
                TilesScored[i].GetComponent<Animator>().enabled=true;
                TilesScored[i].GetComponent<Animator>().SetTrigger("TileScored_Flip");
                yield return wait;
            }
        }

       //ilesScored.Clear();
       GUI_Controller.instance.SpawnTextPopup("+"+(+score), Color.gray, 
                GUI_Controller.instance.transform, (score+10));
        if(GameMaster.instance.playedTiles.Count >0)
            BoardController.instance.EventScore(score);
        GameMaster.instance.playedTiles.Clear();
        
    }

    /// <summary>
    /// note: referenced from GameOver() using Invoke()
    /// </summary>
    public void GridFullEffect()
    {
        DisableAllEmissions();
        GridTiles = GetComponentsInChildren<GridTile>();
      //  Debug.LogWarning("Grid Tiles Count:" + GridTiles.Length);
        GridTiles.Shuffle();
        StartCoroutine(RandomEnableEmissions());
        AudioManager.instance.Play("GridFull");

    }

    private IEnumerator RandomEnableEmissions()
    {
        Debug.LogWarning("Random Enable Emissions");
        GridTiles = GetComponentsInChildren<GridTile>();
        foreach (GridTile tile in GridTiles)
        {
            //StartCoroutine(tile.GetComponent<GUI_Object>().Flash(tile.activeSkin.color, 1.5f, true));
            tile.GetComponent<Renderer>().material.EnableEmission();
            tile.GetComponent<Renderer>().material.SetEmissionRate(.8f);
            if(BoardController.instance.GRID_SIZE==5)
                yield return new WaitForSeconds(.12f);
            else if (BoardController.instance.GRID_SIZE==7)
                yield return new WaitForSeconds(.06f);
            else if (BoardController.instance.GRID_SIZE==9)
                yield return new WaitForSeconds(.03f);
        }

    }

    private Vector3 AdjustFXScale() //metghod which chnages the scale of the tile place effect based on whther it is the first second or third tile played
    {
        switch(GameMaster.instance.playedTiles.Count)
        {
            case 0:
            return new Vector3(0,0,0);

            case 1:
            return new Vector3(0.4f,0.4f,0.4f);

            case 2:
            return new Vector3(0.5f,0.5f,0.5f);

            case 3:
            return new Vector3(0.6f,0.6f,0.6f);
        }
        return new Vector3(0,0,0);
    }

    public void DeactivateTileSkin(GridTile tile) //surely a mehtod for TILE???
    {
        if (tile != null)
        {
            tile.DeafultTileSkin();
            tile.activated=false;
        }
    }

    public void AnimateTo(GUI_Object gui_obj, Vector3 pos, float animTime)
    {
        animationCount++;
        StartCoroutine(gui_obj.AnimateTo(pos, animTime));
    }


    public void RotateObjectBackward(GameObject gUI_Object, float animTime, int rotAmount)
    {
        StartCoroutine(Rotation(animTime, gUI_Object, rotAmount));
    }

    public void SwitchTurnIndicator()
    {
        AudioManager.instance.Play("StatusRotateStone");
        //Deactive All Cards
        foreach(PlayerCard card in PlayerCards)
        {
             if(card.Active)
                 card.ToggleCard();
        }

        if(GameMaster.instance.humanTurn)
        {
            Submit_Button.GetComponent<Animator>().SetTrigger("PlayerTurn");
        } else{
            //Submit_Button.GetComponent<Animator>().SetTrigger("Opponent");
        }

        //Activate Current Card
        switch(GameMaster.instance.turnIndicator)
        {
            case 1:
            PlayerCard1.ToggleCard();
            break;

            case 2:
            PlayerCard2.ToggleCard();
            break;

            case 3:
            PlayerCard3.ToggleCard();
            break;

            case 4:
            PlayerCard4.ToggleCard();
            break;
        }


    }

    public void SpawnTextPopupMenu(string text, Color colour, Transform location, int size)
    {
        TextPopup instance = Instantiate(POPUP_ERROR);
        instance.transform.SetParent(this.GetComponent<Canvas>().transform, false);

        instance.transform.position = instance.transform.position;
        instance.popupText.alignment=TMPro.TextAlignmentOptions.Center;
        instance.popupText.fontSize=28;
        instance.SetColour(colour);
        instance.SetText(text);
    }

    public void SpawnErrorPopup(string text, Color colour, Transform location, int size)
    {
        TextPopup instance = Instantiate(POPUP_ERROR);
        instance.transform.SetParent(this.GetComponent<Canvas>().transform, false);
        instance.transform.position = location.position;
        instance.popupText.alignment=TMPro.TextAlignmentOptions.Center;
        instance.popupText.fontSize=28;
        instance.SetColour(colour);
        instance.SetText(text);
    }

    public int notifPosition;

    public void SpawnTextPopup(string text, Color colour, Transform location, int size)
    {
        if(size>80)
            size=80;

        int notificationLocation;

        notificationLocation = BoardController.instance.FindNotificationPosition();//Mathf.RoundToInt(UnityEngine.Random.Range(0,4)); // finds area with least lit tiles for notifcation location


        //Debug.Log("Spawning notif");
        if(text=="+0")
            return;

        if(location == null)
            return;

        if(text[0]=='+')
        {
            if(text[1]=='0')
                return;

            //Spawn Score Notifcaiton
            
            TextPopup instance = Instantiate(POPUP_SCORE);
            instance.transform.SetParent(this.GetComponent<Canvas>().transform, false);
            //location.transform.position;
                        //new Vector3(UnityEngine.Random.Range(0.03f, .05f),UnityEngine.Random.Range(0.05f, .05f),-3);

            int Xindex =0;
            switch(ApplicationModel.GRID_SIZE)
            {
                case 5:
                Xindex=2;
                break;

                case 7:
                Xindex=2;
                break;

                case 9:
                Xindex=5;
                break;
            }
            
            instance.transform.position =GameMaster.instance.objGameGrid[Xindex,notificationLocation].transform.position;

            if(size<45)
            size=45;

            instance.SetText(text);
            instance.SetSize(size);
            instance.SetFont(orangeGlow);
            instance.gameObject.SetActive(true);

        } else {
            //Spawn Text Notification
            //Debug.Log("Spawning notif text size: " + size);


            TextPopup instance = Instantiate(POPUP_TEXT);
            instance.transform.SetParent(this.GetComponent<Canvas>().transform, false);

            instance.transform.position = instance.transform.position =GameMaster.instance.objGameGrid[2,notificationLocation].transform.position;
                     //- new Vector3(0,GameMaster.instance.objGameGrid[2,notificationLocation].transform.position.y+.5f,
                     //GameMaster.instance.objGameGrid[2,notificationLocation].transform.position.z - 35); //+ new Vector3(0,UnityEngine.Random.Range(0.12f, .15f),0); 

            instance.SetColour(colour);
            instance.SetText(text);
            instance.SetSize(size);
            if(text != "+0")
                instance.gameObject.SetActive(true);
        }

    }

    /// <summary>
    /// OVERLOAD of method above including custom TM material
    /// Spawn text popup with a custom TM PRo font material preset (used for in game text notifcaitons (not score!))
    /// </summary>
    /// <param name="text"></param>
    /// <param name="colour"></param>
    /// <param name="location"></param>
    /// <param name="size"></param>
    /// <param name="fontMaterial"></param>
    public void SpawnTextPopup(string text, Color colour, Transform location, int size, Material fontMaterial)
    {
        if(size>85)
            size=85;

        
        int notificationLocation;

        notificationLocation = BoardController.instance.FindNotificationPosition();//Mathf.RoundToInt(UnityEngine.Random.Range(0,4)); // finds area with least lit tiles for notifcation location
        

        if(notificationLocation==ApplicationModel.GRID_SIZE-1)
        {
            notificationLocation++;
        } else
        {
            notificationLocation--;
        }




        Debug.Log("Spawning notif");
        if(text=="+0")
            return;

        if(location == null)
            return;

        if(text[0]=='+')
        {
            if(text[1]=='0')
                return;

            //Spawn Score Notifcaiton
            
            TextPopup instance = Instantiate(POPUP_SCORE);
            instance.transform.SetParent(this.GetComponent<Canvas>().transform, false);
            //location.transform.position;
            //new Vector3(UnityEngine.Random.Range(0.03f, .05f),UnityEngine.Random.Range(0.05f, .05f),-3);

            int Xindex =0;

            //adjust notification X pos based on grid value
            switch(ApplicationModel.GRID_SIZE)
            {
                case 5:
                Xindex=1;
                break;

                case 7:
                Xindex=4;
                break;

                case 9:
                Xindex=5;
                break;
            }

            
            
            instance.transform.position =GameMaster.instance.objGameGrid[Xindex,notificationLocation].transform.position;

            if(size<45)
            size=45;

            instance.SetText(text);
            instance.SetSize(size);
            instance.SetFont(orangeGlow);
            
            instance.gameObject.SetActive(true);
        } else {
            //Spawn Text Notification
            TextPopup instance = Instantiate(POPUP_TEXT);
            instance.transform.SetParent(this.GetComponent<Canvas>().transform, false);


            int Xindex =0;

            //adjust notification X pos based on grid value
            switch(ApplicationModel.GRID_SIZE)
            {
                case 5:
                Xindex=2;
                break;

                case 7:
                Xindex=2;
                break;

                case 9:
                Xindex=5;
                break;
            }

            Debug.LogWarning("TextPopup Xindex" + Xindex);

            instance.transform.position =GameMaster.instance.objGameGrid[Xindex,notificationLocation].transform.position - new Vector3(0, .25f, 0);
            instance.popupText.enableVertexGradient=false;

            instance.SetColour(colour);
            instance.SetText(text);
            instance.SetSize(size);
            instance.SetFontMaterial(fontMaterial);
            if(text != "+0")
                instance.gameObject.SetActive(true);
        }

    }

    /// <summary>
    /// This method originally spawned cash popup but now shows new stars being added 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="colour"></param>
    /// <param name="location"></param>
    /// <param name="delay"></param>
    /// <param name="popup"></param>
    /// <returns></returns>
    public IEnumerator SpawnCashPopup(string text, Color colour, Transform location, float delay, string popup)
    {
        yield return new WaitForSeconds(delay);
        TextPopup instance = Instantiate(POPUP_CASH);
        instance.transform.SetParent(this.GetComponent<Canvas>().transform, false);
        instance.transform.position = location.transform.position;// + new Vector3(0,0,20);
        instance.SetText(text);

        if(popup != "")
        {
            SpawnTextPopup(popup, colour, location, 28);
        }

    }

    [SerializeField]
    public TextPopup menuTextPopup;
    public void SpawnMenuTextPopUp(string text, Color colour, Vector3 position, int size, Material fontMat)
    {
        menuTextPopup.transform.position=position;
        menuTextPopup.SetColour(colour);
        menuTextPopup.SetText(text);
        menuTextPopup.SetSize(size);
        menuTextPopup.SetFontMaterial(fontMat);
        menuTextPopup.enabled=true;
        menuTextPopup.animator.SetTrigger("");

    }

    public void ActivateStar(int star)
    {
        AudioManager.instance.Play("star");

        switch(star)
        {
            case 1:
                Debug.Log("Star 1 activated");
                GameMaster.instance.starCount = 1;
                TargetStar1FX.SetActive(true);
            break;

            case 2:
                Debug.Log("Star 2 activated");
                GameMaster.instance.starCount = 2;
                TargetStar2FX.SetActive(true);
            break;

            case 3:
                Debug.Log("Star 3 activated");
                GameMaster.instance.starCount = 3;
                TargetStar3FX.SetActive(true);
            break;
        }
    }

    public void GameOverNext()
    {
        DialogueController.ActiveDialogue.NextPlayer(GameMaster.instance.playerScores, GameMaster.instance.playerBestScores, GameMaster.instance.playerErrors, GameMaster.instance.targetScore_3Star);
    }

    public void GameOverPrevious()
    {
        DialogueController.ActiveDialogue.PreviousPlayer(GameMaster.instance.playerScores, GameMaster.instance.playerBestScores, GameMaster.instance.playerErrors, GameMaster.instance.targetScore_3Star);
    }



    public virtual void AddObserver(Observer ob)
    {
        observerList.Add(ob);

    }

    public virtual void DeleteObserver(Observer ob)
    {

    }

    public virtual void NotifyObservers(MonoBehaviour _class, string _event)
    {
        foreach (var Observer in observerList)
        {
            Observer.OnNotification(_class, _event);
        }

    }


   
}
