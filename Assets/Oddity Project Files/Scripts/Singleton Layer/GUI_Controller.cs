using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
 //using UnityEditor;
 using EZCameraShake;

public class GUI_Controller : MonoBehaviour, Observable {

    public static GUI_Controller instance = null;
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
    public GameObject statusBar;
    public GameObject buttons;
    [Header("UI Text")]
    public GameObject ScoreText;
    public GameObject physcialGridContainer;
    public AnimationCurve ElasticCurve;
    public AnimationCurve RotationCurve;
    [Header("Portrait Anchors")]
    public GameObject[] InactiveCardPositions;
    [Header("Grid Options")]
    public GameObject GameGrid_5x5;
    public GameObject GameGrid_7x7;
    public GameObject GameGrid_9x9;
    [SerializeField]
    public Vector3 Grid_Scale;
    [SerializeField]
    public GameObject GameOverFX;
    //FX
    public GameObject Tile1ActivateFX;
    public GameObject Tile2ActivateFX;
    public GameObject Tile3ActivateFX;
    public GameObject Tile4ActivateFX;
    public GameObject Tile5ActivateFX;
    public GameObject Tile6ActivateFX;
    public GameObject Tile7ActivateFX;
    public GameObject Tile1ActivateFX_alt;
    public GameObject Tile2ActivateFX_alt;
    public GameObject Tile3ActivateFX_alt;
    public GameObject Tile4ActivateFX_alt;
    public GameObject Tile5ActivateFX_alt;
    public GameObject Tile6ActivateFX_alt;
    public GameObject Tile7ActivateFX_alt;
    public Component[] GridTiles;

    [Header("Game UI objects")]
    public GameObject Rotate_Tiles_Btn;

    [Header("Active Player Cards")]
    public PlayerCard PlayerCard1;
    public PlayerCard PlayerCard2;
    public PlayerCard PlayerCard3;
    public PlayerCard PlayerCard4;

    [Header("3 Player Cards")]
    public PlayerCard PlayerCard5;
    public PlayerCard PlayerCard6;
    public PlayerCard PlayerCard7;

    [Header("4 Player Cards")]
    public PlayerCard PlayerCard8;
    public PlayerCard PlayerCard9;
    public PlayerCard PlayerCard10;
    public PlayerCard PlayerCard11;

    public List<PlayerCard> PlayerCards = new List<PlayerCard>();
    public Light directionalLight;
    private static TextPopup POPUP_TEXT;
    private static TextPopup POPUP_SCORE;
    private static TextPopup POPUP_CASH;

    [Header("GUI Effect Listener")]
    public List<GridTile> TilesScored = new List<GridTile>(); //this is a list of tiles that are involved an a round of player moves, used to activate score trail effect
    public GUI_Object TargetScore_Stone;
    public GUI_Object RemainingTurns_Stone;
    public GUI_Object PlayerCoins_Stone;
    public TextMeshProUGUI remainingTurnText;
    public TextMeshProUGUI remainingTilesText;
    public TextMeshProUGUI targetScoreText;
    public GameObject SoloTargetCard;
    public GameObject SoloScoreCard;

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
    public GUI_Dialogue_Call PauseMenu;
    public GameObject SettingsButton;
    public GameObject SoundButton;
    public GameObject MusicButton;
    public GameObject CoinDialogue;
    public GameObject LivesDialogue;
    public int inactiveCardCount=0;   
    public GameObject Confetti; 

    public GameObject GridCompleteAnim;


    void Awake()
    {
        if(instance == null)
        {
            
        }
        instance = this;

        if(!POPUP_TEXT)
            POPUP_TEXT = Resources.Load<TextPopup>("PopupTextParent");

        if(!POPUP_SCORE)
            POPUP_SCORE = Resources.Load<TextPopup>("PopupScoreParent");

        if(!POPUP_CASH)
            POPUP_CASH = Resources.Load<TextPopup>("PopupCashParent");

        DialogueController = this.GetComponent<GUI_Dialogue_Controller>();

        Time.timeScale = 1f;
       
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
            }
            GUI_Controller.instance.PlayerCards[0].SetQueuePos(1);
        }
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Method originally used to rotate the screen in local multiplayer when game was restrcited to two players
    /// </summary>
    public void RotateScreen()
    {
        // if( ApplicationModel.vsLocalMP )
        // {
        //     //GUI_Controller.instance.RotateScreen();

        //     Debug.Log("Screen oritentation localMP " + Screen.orientation);
        //     if(Screen.orientation == ScreenOrientation.LandscapeLeft)
        //     {
        //         Debug.Log("Rotate");
        //         Screen.orientation = ScreenOrientation.LandscapeRight;

        //     } else if(Screen.orientation == ScreenOrientation.LandscapeRight) {
        //         Debug.Log("Rotate2");
        //         Screen.orientation = ScreenOrientation.LandscapeLeft;
        //     } else {
        //         Debug.Log("flip");
        //         Screen.orientation = ScreenOrientation.LandscapeRight;
        //     }
        // }
    }

    public void ToggleAllSoloUI()
    {
        RemainingTurns_Stone.gameObject.SetActive(!RemainingTurns_Stone.gameObject.activeSelf);
        ToggleActionButtons();
    }

    public void ToggleSettingsButton(bool state)
    {
        SettingsButton.SetActive(state);
    }

    public void ToggleCurrencyUI(bool state)
    {
        CoinDialogue.SetActive(state);
        LivesDialogue.SetActive(state);
    }

    public void ToggleSettingsOptions()
    {
        Debug.Log("TOGGLE");
        SoundButton.SetActive(!SoundButton.activeSelf);
        MusicButton.SetActive(!MusicButton.activeSelf);
    }

    public void ToggleAllVsAiUI()
    {
        if(PlayerCard1 != null) { PlayerCard1.gameObject.SetActive(PlayerCard1.gameObject.activeSelf); }
        if(PlayerCard2 != null) { PlayerCard2.gameObject.SetActive(PlayerCard1.gameObject.activeSelf); }
        if(PlayerCard3 != null) { PlayerCard3.gameObject.SetActive(PlayerCard1.gameObject.activeSelf); }
        if(PlayerCard4 != null) { PlayerCard4.gameObject.SetActive(PlayerCard1.gameObject.activeSelf); }
        ToggleActionButtons();
    }

    private void ToggleActionButtons()
    {
        // EndTurn_Btn.gameObject.SetActive(!EndTurn_Btn.gameObject.activeSelf);
        // ExchangeTile_Btn.gameObject.SetActive(!ExchangeTile_Btn.gameObject.activeSelf);
        // Menu_Btn.gameObject.SetActive(!Menu_Btn.gameObject.activeSelf);
    }

    //IEnumerators For Animation        //temp - could refactor into scale object ?
    public IEnumerator GridIntroAnim()
    {

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
        foreach(GridCell cell in GameMaster.instance.objGameGrid)
        {
            if(cell != null){cell.gameObject.GetComponent<BoxCollider>().enabled=false;}
        }

    }

    //IEnumerators For Animation
    public IEnumerator GridOutroAnim()
    {

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
        while(startScore < newScore+1)
        {
            text.text = prefix + " " + startScore.ToString();
            startScore+=UnityEngine.Random.Range(1, 10);
            yield return new WaitForSeconds(UnityEngine.Random.Range(.005f, .05f));
        }

        text.text = prefix + " " + newScore.ToString();


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

    public void CashRewardAnim()
    {
        int cashReward = 0;
        for(int i=0; i<GameMaster.instance.starCount; i++)
        {
            StartCoroutine(SpawnCashPopup("+100", Color.green, 
            DialogueController.ActiveDialogue.targetStarController.starFX[i].transform, (i)+1.5f, "")); 
            cashReward+=100;
        }
        if(GameMaster.instance.errorsMade == 0)
        {
            
            StartCoroutine(SpawnCashPopup("+20", Color.green, 
            DialogueController.ActiveDialogue.playerErrorsTxt.transform, 5f, "Error bonus!")); 
            cashReward+=20;
        }
        int res=-1;
        if(AccountInfo.playfabId != null)
        {
            if(AccountInfo.Instance.Info.UserVirtualCurrency.TryGetValue(AccountInfo.COINS_CODE, out res))
            {
                StartCoroutine(UpdateUIScore(res, cashReward+res, PlayerCoins_Stone.GetComponentInChildren<TextMeshProUGUI>()));
                AccountInfo.AddInGameCurrency(cashReward);
            }
        }
    }

    public void PauseGame()
    {
        ActionButtons.SetActive(false);
        PauseMenu.Open();
        Time.timeScale = 0f;
        if(GameMaster.instance.vsAi || ApplicationModel.VS_LOCAL_MP)
        {
            ToggleAllVsAiUI();

        } else 
        {
            ToggleAllSoloUI();

        }
        ToggleSettingsButton(true);
    }

    public void UnpauseGame()
    {
        if(ActionButtons != null)
            ActionButtons.SetActive(true);

        if(GameMaster.instance.vsAi || ApplicationModel.VS_LOCAL_MP)
        {
            ToggleAllVsAiUI();

        } else 
        {
            ToggleAllSoloUI();

        }
        ToggleSettingsButton(false);
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        DisableAllEmissions();
        Invoke("GridFullEffect", 3f);
        Invoke("EndGame", 7.5f);
    }

    public void EndGame()
    {
        if(GameMaster.instance.playerWin)
        {
            AudioManager.instance.Play("YouWinCheer");
            if(GameMaster.instance.starCount > 1)
                GUI_Controller.instance.Confetti.SetActive(true);
        }

        if(GameMaster.instance.vsAi || ApplicationModel.VS_LOCAL_MP)
        {
            ToggleAllVsAiUI();
        } else 
        {
            ToggleAllSoloUI();
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
        Invoke("GridOutro",1.7f); // temp - don't use invoke
        GUI_Controller.instance.ToggleActionButtons();
        if(PlayerCard1 != null){Destroy(PlayerCard1.gameObject);}
        if(PlayerCard2 != null){Destroy(PlayerCard2.gameObject);}
        if(PlayerCard3 != null){Destroy(PlayerCard3.gameObject);}
        if(PlayerCard4 != null){Destroy(PlayerCard4.gameObject);}
        if(statusBar.gameObject!=null){Destroy(statusBar.gameObject);}
        if(SoloTargetCard.gameObject!=null){Destroy(SoloTargetCard.gameObject);}
        Destroy(TimerUI);
        PlayerCoins_Stone.gameObject.SetActive(true);
       

    }

    private void GridOutro()
    {
        StartCoroutine(GridOutroAnim());
    }

    public GridTile[] GetAllTiles()
    {
        return GetComponentsInChildren<GridTile>();
        
    }

     

    public void DisableAllEmissions()
    {
        Debug.LogWarning("Disabling All Tiles emissions");
        GridTiles = GetComponentsInChildren<GridTile>();
        foreach (GridTile tile in GridTiles)
        {
            if(!tile.isFlashing && (tile.placed || tile.placedByAI) )
            {
                tile.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
            }
        }
    }

    public void EnableAllEmissions()
    {
        if(!GameMaster.instance.gameOver)
            StartCoroutine(EnableEM(1.5f));
    }

    /// <summary>
    /// Enables emissions of all tiles after a set delay
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnableEM(float delay)
    {
        Debug.LogWarning("Enabling ALL EM");
        yield return new WaitForSeconds(delay);
        GridTiles = GetComponentsInChildren<GridTile>();
        foreach (GridTile tile in GridTiles)
        {
            tile.isFlashing=false;
            tile.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");  
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


    // public void RotateScreen()
    // {

    //     // Debug.Log("Screen orientation: " + Screen.orientation);
    //     // if(Screen.orientation == ScreenOrientation.LandscapeLeft)
    //     //     {
    //     //         Debug.Log("Rotate");
    //     //         Screen.orientation = ScreenOrientation.LandscapeRight;

    //     //     } else {
    //     //         Debug.Log("Rotate2");
    //     //         Screen.orientation = ScreenOrientation.LandscapeLeft;
    //     //     }
    // }

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

    public void ActivateCell(GridTile tile) 
    {
        if (tile != null && !tile.activated )
        {
            tile.ActiveTileSkin();
            tile.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
            tile.GetComponent<NoGravity>().enabled = false;
            switch(tile.value)
            {
                case 1:
                GameObject FX = new GameObject();
                if(GameMaster.instance.humanTurn)
                {
                    FX = Instantiate(Tile1ActivateFX, tile.transform.position-
                    new Vector3(0,0,1), Tile1ActivateFX.transform.rotation);

                } else 
                {
                    FX = Instantiate(Tile1ActivateFX_alt, tile.transform.position-
                    new Vector3(0,0,1), Tile1ActivateFX_alt.transform.rotation);
                }

                FX.transform.localScale=AdjustFXScale();
                Destroy(FX, 1f);
                break;

                case 2:
                GameObject FX2 = new GameObject();
                if(GameMaster.instance.humanTurn)
                {
                    FX2 = Instantiate(Tile2ActivateFX, tile.transform.position-
                    new Vector3(0,0,1), Tile2ActivateFX.transform.rotation);

                } else 
                {
                    FX2 = Instantiate(Tile2ActivateFX_alt, tile.transform.position-
                    new Vector3(0,0,1), Tile2ActivateFX_alt.transform.rotation);
                }
                FX2.transform.localScale=AdjustFXScale();
                Destroy(FX2, 1f);
                break;

                case 3:
                GameObject FX3 = new GameObject();
                if(GameMaster.instance.humanTurn)
                {
                    FX3 = Instantiate(Tile3ActivateFX, tile.transform.position-
                    new Vector3(0,0,1), Tile3ActivateFX.transform.rotation);

                } else 
                {
                    FX3 = Instantiate(Tile3ActivateFX_alt, tile.transform.position-
                    new Vector3(0,0,1), Tile3ActivateFX_alt.transform.rotation);
                }
                FX3.transform.localScale=AdjustFXScale();
                Destroy(FX3, 1f);
                break;

                case 4:
                GameObject FX4 = new GameObject();
                if(GameMaster.instance.humanTurn)
                {
                    FX4 = Instantiate(Tile4ActivateFX, tile.transform.position-
                    new Vector3(0,0,1), Tile4ActivateFX.transform.rotation);

                } else 
                {
                    FX4 = Instantiate(Tile1ActivateFX_alt, tile.transform.position-
                    new Vector3(0,0,1), Tile1ActivateFX_alt.transform.rotation);
                }
                FX4.transform.localScale=AdjustFXScale();
                Destroy(FX4, 1f);
                break;

                case 5:
                GameObject FX5 = new GameObject();
                if(GameMaster.instance.humanTurn)
                {
                    FX5 = Instantiate(Tile5ActivateFX, tile.transform.position-
                    new Vector3(0,0,1), Tile5ActivateFX.transform.rotation);

                } else 
                {
                    FX5 = Instantiate(Tile5ActivateFX_alt, tile.transform.position-
                    new Vector3(0,0,1), Tile5ActivateFX_alt.transform.rotation);
                }
                FX5.transform.localScale=AdjustFXScale();
                Destroy(FX5, 1f);
                break;

                case 6:
                GameObject FX6 = new GameObject();
                if(GameMaster.instance.humanTurn)
                {
                    FX6 = Instantiate(Tile6ActivateFX, tile.transform.position-
                    new Vector3(0,0,1), Tile6ActivateFX.transform.rotation);

                } else 
                {
                    FX6 = Instantiate(Tile6ActivateFX_alt, tile.transform.position-
                    new Vector3(0,0,1), Tile6ActivateFX_alt.transform.rotation);
                }
                FX6.transform.localScale=AdjustFXScale();
                Destroy(FX6, 1f);
                break;

                case 7:
                GameObject FX7 = new GameObject();
                if(GameMaster.instance.humanTurn)
                {
                    FX7 = Instantiate(Tile7ActivateFX, tile.transform.position-
                    new Vector3(0,0,1), Tile7ActivateFX.transform.rotation);

                } else 
                {
                    FX7 = Instantiate(Tile7ActivateFX_alt, tile.transform.position-
                    new Vector3(0,0,1), Tile7ActivateFX_alt.transform.rotation);
                }
                FX7.transform.localScale=AdjustFXScale();
                Destroy(FX7, 1f);
                break;
            }
            tile.activated = true;
        } 
    }

    public void ScoreTrailEffect()
    {
        //Remove duplicates
        foreach(GridTile tile in TilesScored)
        {
            //perform scoring effect
            if(tile != null)
            {
                tile.isFlashing = true;
                //tile.ScoreEffect.Play(); // temp - decided to remove effect particle colour effect
                StartCoroutine( tile.GetComponent<GUI_Object>().Flash(tile.activeSkin.color, 1.5f, true) );
            }
        }
       TilesScored.Clear();
    }

    /// <summary>
    /// note: referenced from GameOver() using Invoke()
    /// </summary>
    public void GridFullEffect()
    {
        DisableAllEmissions();
        GridTiles = GetComponentsInChildren<GridTile>();
        Debug.LogWarning("Grid Tiles Count:" + GridTiles.Length);
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
            StartCoroutine(tile.GetComponent<GUI_Object>().Flash(tile.activeSkin.color, .75f, false));
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
            return new Vector3(0.2f,0.2f,0.2f);

            case 2:
            return new Vector3(0.4f,0.4f,0.4f);

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

    public void SpawnScoreNotif(int score) // not currently in use
    {
        if (LastPlacedTile == null)
            return;

        GameObject TextObj = Instantiate(ScoreText, NotificationParent.transform.position, ScoreText.transform.rotation);
        TextObj.GetComponent<TextMeshProUGUI>().text = "+" + score.ToString();
        
        TextObj.gameObject.transform.SetParent(this.gameObject.transform);
        TextObj.transform.localScale = new Vector3(1,1,1);
        TextObj.transform.position = NotificationParent.transform.position+new Vector3(0,0,-10);
        
        TextObj.GetComponent<Animator>().enabled = true;
        Destroy(TextObj, 3f);
    }

    public void SpawnTextPopup(string text, Color colour, Transform location, int size)
    {
        if(size>100)
            size=100;

        Debug.Log("Spawning notif");
        if(text=="+0")
            return;

        if(location == null)
            return;

        if(text[0]=='+')
        {
            if(text[1]=='0')
                return;
            TextPopup instance = Instantiate(POPUP_TEXT);
            instance.transform.SetParent(this.GetComponent<Canvas>().transform, false);
            instance.transform.position = location.transform.position - 
                        new Vector3(UnityEngine.Random.Range(0.03f, .1f),UnityEngine.Random.Range(0.05f, .1f),-3);
            
            if(instance.transform.position.x <= .7)
                instance.transform.position += new Vector3(.2f,0f,0f);

            instance.SetText(text);
            instance.SetSize(size);
            instance.SetFont(orangeGlow);
            instance.gameObject.SetActive(true);
        } else {
            TextPopup instance = Instantiate(POPUP_TEXT);
            instance.transform.SetParent(this.GetComponent<Canvas>().transform, false);
            instance.transform.position = location.transform.position + new Vector3(UnityEngine.Random.Range(0.05f, .07f),UnityEngine.Random.Range(0.12f, .15f),-3); 
            if(instance.transform.position.x <= .7)
                instance.transform.position += new Vector3(.2f,0f,0f);

            instance.SetColour(colour);
            instance.SetText(text);
            instance.SetSize(size);
            if(text != "+0")
                instance.gameObject.SetActive(true);
        }

    }

    public IEnumerator SpawnScorePopup(string text, Color colour, Transform location, float delay)
    {
        yield return new WaitForSeconds(delay);
        TextPopup instance = Instantiate(POPUP_SCORE);
            instance.transform.SetParent(this.GetComponent<Canvas>().transform, false);
            instance.transform.position = location.transform.position;
            instance.SetText(text);
    }

    public IEnumerator SpawnCashPopup(string text, Color colour, Transform location, float delay, string popup)
    {
        yield return new WaitForSeconds(delay);
        TextPopup instance = Instantiate(POPUP_CASH);
        instance.transform.SetParent(this.GetComponent<Canvas>().transform, false);
        instance.transform.position = location.transform.position + new Vector3(0,+1,+50);
        instance.SetText(text);

        if(popup != "")
        {
            SpawnTextPopup(popup, colour, location, 28);
        }

    }

    public void ActivateStar(int star)
    {
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

    }

    public virtual void DeleteObserver(Observer ob)
    {

    }

    public virtual void NotifyObservers(MonoBehaviour _class, string _event)
    {

    }


   
}
