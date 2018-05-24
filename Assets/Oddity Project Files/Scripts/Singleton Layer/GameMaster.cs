using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;

public class GameMaster : MonoBehaviour{

    public static GameMaster instance = null;     // Singleton instance

    [SerializeField]
    public bool TUTORIAL_MODE {get; set;}

    [Header("Game Architecture")]
    public StateMachine StateMachine;
    public BoardEvaluator BoardEvaluator;
    public  TutorialController TutorialController;
    public GridCell[,] objGameGrid;

    public TurnTimer TurnTimer;

    List<Observer> observers = new List<Observer>();

    [Header("Game Configuration Constants")]
    private static AI_Player AI_PLAYER;
    public static int MAX_TURN_INDICATOR;
    //[Header("Game Options")]
    private static int START_ONE_TILES;    private int START_TWO_TILES;    private int START_THREE_TILES;
    private  int START_FOUR_TILES;    private int START_FIVE_TILES;    private static int START_SIX_TILES;
    private static int START_SEVEN_TILES;
    private int turnLimit{get; set;}
    public bool vsAi{get; private set;} 
    public bool soloPlay{get; private set;}
    public int targetScore_3Star{get; private set;}
    public int targetScore2_2Star{get; private set;} 
    public int targetScore1_1Star{get; private set;} 


    [Header("Active Game Variables")]
    private int turnCounter;
    public bool gameOver{get; private set;}
    public bool staticStateBroken;
    public GridTile selectedTile;    public GridCell activeCell;
    public List<GridTile> currentHand = new List<GridTile>();
    public List<bool> turnIdentifier = new List<bool>(); //method of telling if its a human or ai turn...
    public bool humanTurn{get; private set;}
    public int turnIndicator {get; private set;}
    public bool invalidTilesInplay {get; set;}


    [Header("Tile Variables")]
    private int totalRemainingTiles;
    private static GridTile tile1;    private static GridTile tile2;    private static GridTile tile3;
    private static GridTile tile4;    private static GridTile tile5;    private static GridTile tile6;  private static GridTile tile7;
    private static GridTile tile1_alt;    private static GridTile tile2_alt;    private static GridTile tile3_alt;
    private static GridTile tile4_alt;    private static GridTile tile5_alt;    private static GridTile tile6_alt;  private static GridTile tile7_alt;
    private List<GridTile> tileModels = new List<GridTile>();

    public int totalTiles{get; set;}
    public List<int> EvenTiles{get; private set;}
    public List<int> OddTiles{get; private set;}
    public List<GridCell> playedTiles = new List<GridCell>(); //stores the tiles which have currently been played, reset each round


    [Header("Player Variables")]
    public List<GridTile> Player1Hand = new List<GridTile>();    public List<GridTile> Player2Hand = new List<GridTile>();
    public List<GridTile> Player3Hand = new List<GridTile>();    public List<GridTile> Player4Hand = new List<GridTile>();
    public List<int> playerScores{get; set;}
    public List<int> playerBestScores{get; set;}
    public List<int> playerErrors{get; set;}

    //Spawn Location
    [Header("Other")]
    [SerializeField]
    private List<GameObject> spawnLocations = new List<GameObject>();
    public Theme[] themes;
    public float averageTurnTime; //not currently implemented
    public static float TILE_SCALE{get; set;}
    public static float GRID_SCALE{get; set;}
    public static float GRID_TILE_LIGHT_RANGE{get; set;}
    public int starCount{get; set;}
    public int errorsMade{get; set;} 

    public bool playerWin {get; set;}


    [Header("Debugging")]
    [SerializeField]
    private GameObject lblGrid;   
    [SerializeField]
    private GameObject lblLVGrid; 

    void Awake()
    {
        StateMachine = GetComponent<StateMachine>();

        TUTORIAL_MODE = ApplicationModel.TUTORIAL_MODE;
        if(TUTORIAL_MODE)
            TutorialController = GetComponent<TutorialController>();

        if(instance==null)
            instance = this;

        //init playerscores
        playerScores = new List<int>();
        //Init player best scores
        playerBestScores = new List<int>();
        //init player error counters
        playerErrors = new List<int>();

        turnLimit=20; //default
        soloPlay = ApplicationModel.SOLO_PLAY;
        vsAi = ApplicationModel.VS_AI;
        turnLimit = ApplicationModel.TURNS;
        targetScore_3Star = ApplicationModel.TARGET;

        objGameGrid = new GridCell[9, 9];

        MAX_TURN_INDICATOR = ApplicationModel.AI_PLAYERS + ApplicationModel.HUMAN_PLAYERS;
        for(int i=0; i<ApplicationModel.HUMAN_PLAYERS; i++)
        {
            turnIdentifier.Add(true);
        }

        for(int i=0; i<ApplicationModel.AI_PLAYERS; i++)
        {
            turnIdentifier.Add(false);
        }
    }

    void Start()
    {
        if(MAX_TURN_INDICATOR ==4)
        {
            GUI_Controller.instance.PlayerCard1=GUI_Controller.instance.PlayerCard8;
            GUI_Controller.instance.PlayerCard2=GUI_Controller.instance.PlayerCard9;
            GUI_Controller.instance.PlayerCard3=GUI_Controller.instance.PlayerCard10;
            GUI_Controller.instance.PlayerCard4=GUI_Controller.instance.PlayerCard11;
            GUI_Controller.instance.PlayerCard1.gameObject.SetActive(true);
            GUI_Controller.instance.PlayerCard2.gameObject.SetActive(true);
            GUI_Controller.instance.PlayerCard3.gameObject.SetActive(true);
            GUI_Controller.instance.PlayerCard4.gameObject.SetActive(true);

        } else if (MAX_TURN_INDICATOR ==3)
        {
            GUI_Controller.instance.PlayerCard1=GUI_Controller.instance.PlayerCard5;
            GUI_Controller.instance.PlayerCard2=GUI_Controller.instance.PlayerCard6;
            GUI_Controller.instance.PlayerCard3=GUI_Controller.instance.PlayerCard7;
            GUI_Controller.instance.PlayerCard4=null;
            GUI_Controller.instance.PlayerCard1.gameObject.SetActive(true);
            GUI_Controller.instance.PlayerCard2.gameObject.SetActive(true);
            GUI_Controller.instance.PlayerCard3.gameObject.SetActive(true);

        } else if(MAX_TURN_INDICATOR ==2)
        {
            GUI_Controller.instance.PlayerCard1.gameObject.SetActive(true);
            GUI_Controller.instance.PlayerCard2.gameObject.SetActive(true);
            GUI_Controller.instance.PlayerCard3=null;
            GUI_Controller.instance.PlayerCard4=null;
        }       

        Time.timeScale = 1f;
        errorsMade = 0;
        averageTurnTime = 10f; //temp

        START_ONE_TILES = ApplicationModel.ONE_TILES;                     
        START_TWO_TILES = ApplicationModel.TWO_TILES;
        START_THREE_TILES = ApplicationModel.THREE_TILES;
        START_FOUR_TILES = ApplicationModel.FOUR_TILES;                     
        START_FIVE_TILES = ApplicationModel.FIVE_TILES;
        START_SIX_TILES = ApplicationModel.SIX_TILES;
        START_SEVEN_TILES = ApplicationModel.SEVEN_TILES;

        if(BoardController.instance.GRID_SIZE == 9)
        {
            GUI_Controller.instance.physcialGridContainer = GUI_Controller.instance.GameGrid_9x9;
            GUI_Controller.instance.GameGrid_9x9.SetActive(true);
            GRID_SCALE = .8f;
            TILE_SCALE=0.33f;
            GRID_TILE_LIGHT_RANGE = 2.5f;
        }
        else if(BoardController.instance.GRID_SIZE == 5)
        {
            GUI_Controller.instance.physcialGridContainer = GUI_Controller.instance.GameGrid_5x5;
            GUI_Controller.instance.GameGrid_5x5.SetActive(true);
            GRID_SCALE = 1.3f;
            TILE_SCALE=0.55f;
        } else if(BoardController.instance.GRID_SIZE == 7)
        {
            GUI_Controller.instance.physcialGridContainer = GUI_Controller.instance.GameGrid_7x7;
            GUI_Controller.instance.GameGrid_7x7.SetActive(true);
            GRID_SCALE = 1f;
            TILE_SCALE = .45f;
        } else {
            GUI_Controller.instance.physcialGridContainer = GUI_Controller.instance.GameGrid_5x5;
            GUI_Controller.instance.GameGrid_5x5.SetActive(true);
            GRID_SCALE = 1.3f;
            TILE_SCALE=0.55f;

        }

        GUI_Controller.instance.Grid_Scale = new Vector3(GRID_SCALE,GRID_SCALE,GRID_SCALE);

        if(soloPlay)        //Refactor solo play as child of game master temp
        {
            GUI_Controller.instance.PlayerCard2.gameObject.SetActive(false);
            GUI_Controller.instance.RemainingTurns_Stone.gameObject.SetActive(true);
            Destroy(GUI_Controller.instance.TimerUI.gameObject);
        } else {
            GUI_Controller.instance.RemainingTurns_Stone.gameObject.SetActive(false);
            //GUI_Controller.instance.TargetScore_Stone.gameObject.SetActive(false);
            GUI_Controller.instance.SoloTargetCard.gameObject.SetActive(false);
            GUI_Controller.instance.SoloScoreCard.gameObject.SetActive(false);
        }

        if(!ApplicationModel.VS_LOCAL_MP)
            Destroy(GUI_Controller.instance.Rotate_Tiles_Btn.gameObject);

        SetTheme();

        StartCoroutine(GUI_Controller.instance.GridIntroAnim());
    }

    void SetTheme()
    {
        Theme currentTheme = themes[ApplicationModel.THEME];
        Theme altTheme = themes[2];
        Debug.Log("Loading Theme: " + currentTheme.name);

        GUI_Controller.instance.directionalLight.intensity=currentTheme.SunLightIntensity;
        RenderSettings.skybox = currentTheme.Skybox;
        tile1=currentTheme.Tile1;
        tile2=currentTheme.Tile2;
        tile3=currentTheme.Tile3;
        tile4=currentTheme.Tile4;
        tile5=currentTheme.Tile5;
        tile6=currentTheme.Tile6;
        tile7=currentTheme.Tile7;
        //load oppoent alt theme
        tile1_alt=altTheme.Tile1;
        tile2_alt=altTheme.Tile2;
        tile3_alt=altTheme.Tile3;
        tile4_alt=altTheme.Tile4;
        tile5_alt=altTheme.Tile5;
        tile6_alt=altTheme.Tile6;
        tile7_alt=altTheme.Tile7;

        GUI_Controller.instance.Tile1ActivateFX=currentTheme.Tile1FX;
        GUI_Controller.instance.Tile2ActivateFX=currentTheme.Tile2FX;
        GUI_Controller.instance.Tile3ActivateFX=currentTheme.Tile3FX;
        GUI_Controller.instance.Tile4ActivateFX=currentTheme.Tile4FX;
        GUI_Controller.instance.Tile5ActivateFX=currentTheme.Tile5FX;
        GUI_Controller.instance.Tile6ActivateFX=currentTheme.Tile6FX;
        GUI_Controller.instance.Tile7ActivateFX=currentTheme.Tile7FX;

        GUI_Controller.instance.Tile1ActivateFX_alt=altTheme.Tile1FX;
        GUI_Controller.instance.Tile2ActivateFX_alt=altTheme.Tile2FX;
        GUI_Controller.instance.Tile3ActivateFX_alt=altTheme.Tile3FX;
        GUI_Controller.instance.Tile4ActivateFX_alt=altTheme.Tile4FX;
        GUI_Controller.instance.Tile5ActivateFX_alt=altTheme.Tile5FX;
        GUI_Controller.instance.Tile6ActivateFX_alt=altTheme.Tile6FX;
        GUI_Controller.instance.Tile7ActivateFX_alt=altTheme.Tile7FX;


        
        //assign materials
        GridCell[] GridCells =
            GUI_Controller.instance.physcialGridContainer
            .GetComponentsInChildren<GridCell>();

        if(currentTheme.GirdCellAlt != null)
        {
            foreach(GridCell cell in GridCells)
            {
                if(cell.x %2 == 0)
                {
                    if(cell.y %2 == 0)
                    {
                        cell.GetComponent<Renderer>().material = currentTheme.GirdCellAlt;
                    } else 
                    {
                        cell.GetComponent<Renderer>().material = currentTheme.GridCell;
                    }
                } else 
                {
                    if(cell.y %2 == 0)
                    {
                        cell.GetComponent<Renderer>().material = currentTheme.GridCell;
                    } else 
                    {
                        cell.GetComponent<Renderer>().material = currentTheme.GirdCellAlt;
                    }
                }
                int centre = BoardController.instance.GRID_SIZE-1;
                centre = (centre/2);
                if(cell.x == centre && cell.y ==centre)
                    cell.GetComponent<Renderer>().material.color = cell.highlightColour;
            }
        } else {
            foreach(GridCell cell in GridCells)
            {
                cell.GetComponent<Renderer>().material = currentTheme.GridCell;
            }
        }
        

        tileModels.Add(tile1);
        tileModels.Add(tile2);
        tileModels.Add(tile3);
        tileModels.Add(tile4);
        tileModels.Add(tile5);
        tileModels.Add(tile6);
        tileModels.Add(tile7);
    }

    public void ExitToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScreen");
    }
    
    public void InitGame()
    {
        StopAllCoroutines();
        Debug.Log("Init game");
        if(vsAi)
            AI_PLAYER=GetComponent<AI_Player>();
            
        if(!ApplicationModel.VS_LOCAL_MP)
            Destroy(GUI_Controller.instance.Rotate_Tiles_Btn);

        staticStateBroken = false;
        totalTiles = 0;


        for(int i=0; i<MAX_TURN_INDICATOR; i++)
        {
            playerScores.Add(0);
        }


        for(int i=0; i<MAX_TURN_INDICATOR; i++)
        {
            playerBestScores.Add(0);
        }


        for(int i=0; i<MAX_TURN_INDICATOR; i++)
        {
            playerErrors.Add(0);
        }

        if(soloPlay) 
        {
            targetScore_3Star = (ApplicationModel.TARGET);  
            targetScore1_1Star = (ApplicationModel.TARGET)-5*ApplicationModel.GRID_SIZE;
            targetScore2_2Star = (ApplicationModel.TARGET)-2*ApplicationModel.GRID_SIZE; 
        }

        turnLimit = ApplicationModel.GRID_SIZE*3;
        
        if(ApplicationModel.GRID_SIZE==9)
            turnLimit=30;

        turnCounter = 0;
        BoardController.instance.InitBoard();

        EvenTiles = new List<int>();
        OddTiles = new List<int>();

        for(int i = 0; i<START_ONE_TILES; i++)
        {
            OddTiles.Add(1);
        }

        for(int i = 0; i<START_TWO_TILES; i++)
        {
            EvenTiles.Add(2);
        }

        for(int i = 0; i<START_THREE_TILES; i++)
        {
            OddTiles.Add(3);
        }

        for(int i = 0; i<START_FOUR_TILES; i++)
        {
            EvenTiles.Add(4);
        }

        for(int i = 0; i<START_FIVE_TILES; i++)
        {
            OddTiles.Add(5);
        }

        for(int i = 0; i<START_SIX_TILES; i++)
        {
            EvenTiles.Add(6);
        }

        for(int i = 0; i<START_SEVEN_TILES; i++)
        {
            OddTiles.Add(7);
        }

        //TurnTimer.instance.gameObject.SetActive(false);
        for(int i=0; i<MAX_TURN_INDICATOR; i++)
        {
            turnIndicator = i;
            humanTurn = turnIdentifier[turnIndicator]; 
            if(humanTurn)
                if(i < GUI_Controller.instance.PlayerCards.Count-1) {GUI_Controller.instance.PlayerCards[i].UpdateName("Player " + (i+1));}
            else
                if(i < GUI_Controller.instance.PlayerCards.Count-1) {GUI_Controller.instance.PlayerCards[i].UpdateName("AI " + (i+1));}
        }
        turnIndicator = MAX_TURN_INDICATOR; 
        GUI_Controller.instance.EnableGridBoxColliders();
        if(!TUTORIAL_MODE)
        {
        } else 
        {
            TutorialController.StartTutorial();
        }
            EndTurn();
    }

    public IEnumerator WaitingForBoardEvaluator()
    {
        while(BoardEvaluator.movesCalculated == false)
        {
            //Debug.Log("Master Waiting For Evaluatoion...");
            yield return new WaitForSeconds(1f);
        }

        if(BoardEvaluator.noValidMoves == true)
        {
            Debug.LogWarning("al tile length: " + GUI_Controller.instance.GetAllTiles().Length + "G*G: " + BoardController.instance.GRID_SIZE*BoardController.instance.GRID_SIZE);
            if(GUI_Controller.instance.GetAllTiles().Length >= BoardController.instance.GRID_SIZE*BoardController.instance.GRID_SIZE)
            {
                GUI_Controller.instance.GridCompleteAnim.SetActive(true);

                BoardController.instance.boardFull=true;
            } else {
                GUI_Controller.instance.SpawnTextPopup("No Moves!", Color.red, GUI_Controller.instance.gameObject.transform, 38);
            }

            GameOver();
        } else {
            Debug.Log("GG = LVG");
            StateMachine.LastValidGameGrid(); 
        }
    }

    public void EndTurn()
    {
        Debug.Log("End turn");
        StopCoroutine("WaitingForBoardEvaluator");
        if(gameOver) {return;}
        if(!soloPlay) {turnCounter++;}
        if(vsAi){AI_PLAYER.StopAllCoroutines();}
        BoardEvaluator.StopAllCoroutines();
        if(TurnTimer != null)
        {
            TurnTimer.StartTurn();
        }
        StartCoroutine("WaitingForBoardEvaluator");
        HidePlayerTiles();
        SwitchTurnIndicator();
        LoadCurrentHand(); 
        GUI_Controller.instance.EnableAllEmissions();
        if(totalTiles >  3 && humanTurn) 
        {
            EndGameDetection();
        }

        if(!TUTORIAL_MODE) {ShowPlayerTiles();}
        playedTiles.Clear();
        DealPlayerTiles(turnIndicator);
        
        if(soloPlay){EndTurn_SoloMode();} 

        StateMachine.SetLastValidState(turnIndicator);

        if (vsAi && !humanTurn)
        {
            Debug.Log("INVOKING AI:::");
            Invoke("InvokeAI", 3.5f); //temp ?
            ToggleBoxColliders(false);

        } else
        {
            ToggleBoxColliders(true);
        }
    }

    private void EndTurn_SoloMode()
    {
        Debug.Log("End Turn Solo Mode");
        turnIndicator=1;
        currentHand = Player1Hand;
        turnCounter++;
        GUI_Controller.instance.remainingTurnText.text = (turnLimit - turnCounter).ToString();

            if(playerScores[0] > targetScore1_1Star && starCount < 1)
            {
                GUI_Controller.instance.ActivateStar(1);
                GUI_Controller.instance.targetScoreText.text = targetScore2_2Star.ToString();
            } 
            
            if (playerScores[0] > targetScore2_2Star && starCount < 2)
            {
                GUI_Controller.instance.ActivateStar(2);
                GUI_Controller.instance.targetScoreText.text = targetScore_3Star.ToString();
            }
            
            if (playerScores[0] > targetScore_3Star && starCount < 3)
            {
                GUI_Controller.instance.ActivateStar(3);
            } else if(starCount >2){
                GUI_Controller.instance.targetScoreText.text = targetScore_3Star.ToString();
            } else if (starCount == 0){
                GUI_Controller.instance.targetScoreText.text = targetScore1_1Star.ToString();
            }
            if(turnCounter > turnLimit)
                GameOver();

    }

    public void LoadCurrentHand()
    {
        switch (turnIndicator)
        {
            case 1:
                currentHand = Player1Hand;
                break;

            case 2:
                currentHand = Player2Hand;
                break;
            
            case 3:
                currentHand = Player3Hand;
                break;

            case 4:
                currentHand = Player4Hand;
                break;
        }
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator Debuger()
    {
        yield return new WaitForSeconds(3f);
        DrawBoardDebug();
    }



    private void InvokeAI()
    {
        AI_PLAYER.Initalise();
    }

    private void SwitchTurnIndicator()
    {
        turnIndicator++;
        if(turnIndicator > MAX_TURN_INDICATOR)
            turnIndicator = 1;

        humanTurn = turnIdentifier[turnIndicator-1];

        if(!soloPlay) 
            GUI_Controller.instance.SwitchTurnIndicator();
    }

    
    public void DrawBoardDebug()
    {
        // DEBUGGING DISPLAYS
        lblGrid.GetComponent<TextMeshProUGUI>().text = "";
        for (int i = 0; i < BoardController.instance.GRID_SIZE; i++)
        {
            for (int o = 0; o < BoardController.instance.GRID_SIZE; o++)
            {
                lblGrid.GetComponent<TextMeshProUGUI>().text += BoardController.instance.gameGrid[o, i];
            }
            lblGrid.GetComponent<TextMeshProUGUI>().text += "\n";
        }

        lblLVGrid.GetComponent<TextMeshProUGUI>().text = "";
        for (int i = 0; i < BoardController.instance.GRID_SIZE; i++)
        {
            for (int o = 0; o < BoardController.instance.GRID_SIZE; o++)
            {
                lblLVGrid.GetComponent<TextMeshProUGUI>().text += BoardController.instance.lastValidGameGrid[o, i];
            }
            lblLVGrid.GetComponent<TextMeshProUGUI>().text += "\n";
        }

    }

    public void PauseGame()
    {
        GUI_Controller.instance.PauseGame();
        HidePlayerTiles();
        AccountInfo.Login();
    }

    public void ResumeGame()
    {
        GUI_Controller.instance.UnpauseGame();
        ShowPlayerTiles();
    }

    private void ToggleBoxColliders(bool Toggle) 
    {
        foreach (GridTile tile in currentHand)
        {
            tile.GetComponent<BoxCollider2D>().enabled = Toggle;
            if(!Toggle)
                DisableTileEmission(tile);
        }
    }

    private void DisableTileEmission(GridTile tile)
    {
        tile.GetComponent<Renderer>().material.SetColor("_EmissionColor", tile.activeSkin.color*0f);
    }

    void HidePlayerTiles()
    {
        foreach (GridTile tile in currentHand)
        {
            if(tile!=null){tile.gameObject.SetActive(false);}
            if(tile!=null){tile.gameObject.transform.position = tile.GetComponent<GUI_Object>().targetPos;}
        }

        foreach (GridTile tile in Player1Hand)
        {
            if(tile!=null){tile.gameObject.SetActive(false);}
            if(tile!=null){tile.gameObject.transform.position = tile.GetComponent<GUI_Object>().targetPos;}
        }

        foreach (GridTile tile in Player2Hand)
        {
            if(tile!=null){tile.gameObject.SetActive(false);}
            if(tile!=null){tile.gameObject.transform.position = tile.GetComponent<GUI_Object>().targetPos;}
        }

        foreach (GridTile tile in Player3Hand)
        {
            if(tile!=null){tile.gameObject.SetActive(false);}
            if(tile!=null){tile.gameObject.transform.position = tile.GetComponent<GUI_Object>().targetPos;}
        }

        foreach (GridTile tile in Player4Hand)
        {
            if(tile!=null){tile.gameObject.SetActive(false);}
            if(tile!=null){tile.gameObject.transform.position = tile.GetComponent<GUI_Object>().targetPos;}
        }
    }

    public void ShowPlayerTiles()
    {
        foreach (GridTile tile in currentHand)
        {
            tile.gameObject.SetActive(true);
        }
    }

    public void DealPlayerTiles(int player)
    {
        Debug.Log("Dealing player tiles");
        totalRemainingTiles = OddTiles.Count + EvenTiles.Count;

        //Deal Tiles x 3
        int tilesNeeded = 3 - currentHand.Count;

        if (tilesNeeded > totalRemainingTiles)
        {
            tilesNeeded = totalRemainingTiles;
        }


        if (tilesNeeded > 0)
        {
            Debug.Log("Tiles Needed");
            int spawnLocCounter = currentHand.Count;
            for (int i = 0; i < tilesNeeded; i++)
            {
                int tileVal = Random.Range(1, ApplicationModel.MAX_TILE); //temp

                // This ensures that the player always has an odd tile to place to start the grid
                if(turnCounter==1 && i==0)
                {
                    while(tileVal %2 ==0)
                    {
                        tileVal = Random.Range(1, ApplicationModel.MAX_TILE);
                    }
                }

                bool successFlag = false;
                int safeCount = 0; //temp

                while (successFlag == false)
                {
                    if ((OddTiles.Count < 1) && (EvenTiles.Count < 1))
                    {
                        //No more tiles - most legit form of game over!! temp
                        GameOver();
                        return;
                    }

                    safeCount++; //this code SMELLS
                    if (safeCount > 20)
                    {
                        if (tileVal == ApplicationModel.MAX_TILE-1)
                            tileVal = 1;
                        else
                            tileVal++;
                    }

                    switch (tileVal)
                    {
                        case 1:
                            if (OddTiles.Count > 0)
                            {
                                if(OddTiles.Remove(1))
                                {
                                    successFlag = true;
                                    GridTile t;
                                    if(humanTurn)
                                    {
                                        t = Instantiate(tile1, transform.position, Quaternion.Euler(0, 0, 0));
                                    }
                                    else
                                    {
                                        t = Instantiate(tile1_alt, transform.position, Quaternion.Euler(0, 0, 0)); 
                                    }
                                    t.gameObject.transform.SetParent(GUI_Controller.instance.gameObject.transform); //tempppppppp?
                                    t.gameObject.transform.localScale = new Vector3(TILE_SCALE, TILE_SCALE, TILE_SCALE);
                                    t.AddObserver(TutorialController);
                                    bool spawnFlag = false;
                                    while(spawnFlag == false)
                                    {
                                        if(CheckSpawnLocation(spawnLocCounter) == false)
                                        {
                                            spawnLocCounter++;
                                            if (spawnLocCounter > 2) 
                                                spawnLocCounter = 0;
                                        } else
                                        {
                                            spawnFlag = true;
                                        }
                                    }
                                    t.GetComponent<GUI_Object>().SetAnimationTarget(spawnLocations[spawnLocCounter].transform.position);
                                    t.GetComponent<GUI_Object>().StartIntroAnim(Random.Range(0,1.5f));
                                    currentHand.Add(t);
                                } else
                                {
                                    break;
                                }
                            }
                            break;

                        case 2:
                            if (EvenTiles.Count > 0)
                            {
                                if(EvenTiles.Remove(2))
                                {
                                    successFlag = true;
                                    GridTile t;
                                    if(humanTurn)
                                    {
                                        t = Instantiate(tile2, transform.position, Quaternion.Euler(0, 0, 0));
                                    }
                                    else
                                    {
                                        t = Instantiate(tile2_alt, transform.position, Quaternion.Euler(0, 0, 0)); 
                                    }
                                    t.gameObject.transform.SetParent(GUI_Controller.instance.gameObject.transform); //tempppppppp?
                                    t.gameObject.transform.localScale = new Vector3(TILE_SCALE, TILE_SCALE, TILE_SCALE);
                                    t.AddObserver(TutorialController);
                                    bool spawnFlag = false;
                                    while(spawnFlag == false)
                                    {
                                        if(CheckSpawnLocation(spawnLocCounter) == false)
                                        {
                                            spawnLocCounter++;
                                            if (spawnLocCounter > 2) 
                                                spawnLocCounter = 0;
                                        } else
                                        {
                                            spawnFlag = true;
                                        }
                                    }
                                    t.GetComponent<GUI_Object>().SetAnimationTarget(spawnLocations[spawnLocCounter].transform.position);
                                    t.GetComponent<GUI_Object>().StartIntroAnim(Random.Range(0,1.5f));
                                    currentHand.Add(t);
                                } else
                                {
                                    break;
                                }
                            }
                            break;
                        case 3:
                            if (OddTiles.Count > 0)
                            {
                                if(OddTiles.Remove(3))
                                {
                                    successFlag = true;
                                    GridTile t;
                                    if(humanTurn)
                                    {
                                        t = Instantiate(tile3, transform.position, Quaternion.Euler(0, 0, 0));
                                    }
                                    else
                                    {
                                        t = Instantiate(tile3_alt, transform.position, Quaternion.Euler(0, 0, 0)); 
                                    }
                                    t.gameObject.transform.SetParent(GUI_Controller.instance.gameObject.transform); //tempppppppp?
                                    t.gameObject.transform.localScale = new Vector3(TILE_SCALE, TILE_SCALE, TILE_SCALE);
                                    t.AddObserver(TutorialController);
                                    bool spawnFlag = false;
                                    while(spawnFlag == false)
                                    {
                                        if(CheckSpawnLocation(spawnLocCounter) == false)
                                        {
                                            spawnLocCounter++;
                                            if (spawnLocCounter > 2)
                                                spawnLocCounter = 0;
                                        } else
                                        {
                                            spawnFlag = true;
                                        }
                                    }
                                    t.GetComponent<GUI_Object>().SetAnimationTarget(spawnLocations[spawnLocCounter].transform.position);
                                    t.GetComponent<GUI_Object>().StartIntroAnim(Random.Range(0,1.5f));
                                    currentHand.Add(t);
                                } else
                                {
                                    break;
                                }
                            }
                        break;

                        case 4:
                            if (EvenTiles.Count > 0)
                            {
                                if(EvenTiles.Remove(4))
                                {
                                    successFlag = true;
                                    GridTile t;
                                    if(humanTurn)
                                    {
                                        t = Instantiate(tile4, transform.position, Quaternion.Euler(0, 0, 0));
                                    }
                                    else
                                    {
                                        t = Instantiate(tile4_alt, transform.position, Quaternion.Euler(0, 0, 0)); 
                                    }
                                    t.gameObject.transform.SetParent(GUI_Controller.instance.gameObject.transform); //tempppppppp?
                                    t.gameObject.transform.localScale = new Vector3(TILE_SCALE, TILE_SCALE, TILE_SCALE);
                                    t.AddObserver(TutorialController);
                                    bool spawnFlag = false;
                                    while(spawnFlag == false)
                                    {
                                        if(CheckSpawnLocation(spawnLocCounter) == false)
                                        {
                                            spawnLocCounter++;
                                            if (spawnLocCounter > 2) 
                                                spawnLocCounter = 0;
                                        } else
                                        {
                                            spawnFlag = true;
                                        }
                                    }
                                    t.GetComponent<GUI_Object>().SetAnimationTarget(spawnLocations[spawnLocCounter].transform.position);
                                    t.GetComponent<GUI_Object>().StartIntroAnim(Random.Range(0,1.5f));
                                    currentHand.Add(t);
                                } else
                                {
                                    break;
                                }
                            }
                             break;

                            case 5:
                                if (OddTiles.Count > 0)
                                {
                                    if(OddTiles.Remove(5))
                                    {
                                        successFlag = true;
                                        GridTile t;
                                        if(humanTurn)
                                        {
                                            t = Instantiate(tile5, transform.position, Quaternion.Euler(0, 0, 0));
                                        }
                                        else
                                        {
                                            t = Instantiate(tile5_alt, transform.position, Quaternion.Euler(0, 0, 0)); 
                                        }
                                        t.gameObject.transform.SetParent(GUI_Controller.instance.gameObject.transform); //tempppppppp?
                                        t.gameObject.transform.localScale = new Vector3(TILE_SCALE, TILE_SCALE, TILE_SCALE);
                                        t.AddObserver(TutorialController);
                                        bool spawnFlag = false;
                                        while(spawnFlag == false)
                                        {
                                            if(CheckSpawnLocation(spawnLocCounter) == false)
                                            {
                                                spawnLocCounter++;
                                                if (spawnLocCounter > 2) 
                                                    spawnLocCounter = 0;
                                            } else
                                            {
                                                spawnFlag = true;
                                            }
                                        }
                                        t.GetComponent<GUI_Object>().SetAnimationTarget(spawnLocations[spawnLocCounter].transform.position);
                                        t.GetComponent<GUI_Object>().StartIntroAnim(Random.Range(0,1.5f));
                                        currentHand.Add(t);
                                    } else
                                    {
                                        break;
                                    }
                                }
                            break;

                            case 6:
                                if (EvenTiles.Count > 0)
                                {
                                    if(EvenTiles.Remove(6))
                                    {
                                        successFlag = true;
                                        GridTile t;
                                        if(humanTurn)
                                        {
                                            t = Instantiate(tile6, transform.position, Quaternion.Euler(0, 0, 0));
                                        }
                                        else
                                        {
                                            t = Instantiate(tile6_alt, transform.position, Quaternion.Euler(0, 0, 0)); 
                                        }
                                        t.gameObject.transform.SetParent(GUI_Controller.instance.gameObject.transform); //tempppppppp?
                                        t.gameObject.transform.localScale = new Vector3(TILE_SCALE, TILE_SCALE, TILE_SCALE);
                                        t.AddObserver(TutorialController);
                                        bool spawnFlag = false;
                                        while(spawnFlag == false)
                                        {
                                            if(CheckSpawnLocation(spawnLocCounter) == false)
                                            {
                                                spawnLocCounter++;
                                                if (spawnLocCounter > 2) 
                                                    spawnLocCounter = 0;
                                            } else
                                            {
                                                spawnFlag = true;
                                            }
                                        }
                                        t.GetComponent<GUI_Object>().SetAnimationTarget(spawnLocations[spawnLocCounter].transform.position);
                                        t.GetComponent<GUI_Object>().StartIntroAnim(Random.Range(0,1.5f));
                                        currentHand.Add(t);
                                    } else
                                    {
                                        break;
                                    }
                                }
                            break;

                            case 7:
                                if (OddTiles.Count > 0)
                                {
                                    if(OddTiles.Remove(7))
                                    {
                                        successFlag = true;
                                        GridTile t;
                                        if(humanTurn)
                                        {
                                            t = Instantiate(tile7, transform.position, Quaternion.Euler(0, 0, 0));
                                        }
                                        else
                                        {
                                            t = Instantiate(tile7_alt, transform.position, Quaternion.Euler(0, 0, 0)); 
                                        }
                                        t.gameObject.transform.SetParent(GUI_Controller.instance.gameObject.transform); //tempppppppp?
                                        t.gameObject.transform.localScale = new Vector3(TILE_SCALE, TILE_SCALE, TILE_SCALE);
                                        t.AddObserver(TutorialController);
                                        bool spawnFlag = false;
                                        while(spawnFlag == false)
                                        {
                                            if(CheckSpawnLocation(spawnLocCounter) == false)
                                            {
                                                spawnLocCounter++;
                                                if (spawnLocCounter > 2) 
                                                    spawnLocCounter = 0;
                                            } else
                                            {
                                                spawnFlag = true;
                                            }
                                        }
                                        t.GetComponent<GUI_Object>().SetAnimationTarget(spawnLocations[spawnLocCounter].transform.position);
                                        t.GetComponent<GUI_Object>().StartIntroAnim(Random.Range(0,1.5f));
                                        currentHand.Add(t);
                                    } else
                                    {
                                        break;
                                    }
                                }
                            break;                                              

                            default:
                            Debug.Log("Default logged.");
                            break;
                    }
                }
            }
            
        }

        GUI_Controller.instance.remainingTilesText.text = totalRemainingTiles.ToString();

        ToggleBoxColliders(true);
    }

    public void GameOver()
    {
        gameOver = true;
        HidePlayerTiles();

        playerWin = true;

        if(vsAi)
        {
            foreach(int score in playerScores)
            {
                if(score> playerScores[0])
                    playerWin = false;
            }

        } else if(soloPlay)
        {
            if(playerScores[0] >= targetScore1_1Star)
                starCount=1;
            if(playerScores[0]>= targetScore2_2Star)
                starCount=2;
            if(playerScores[0]>=targetScore_3Star)
                starCount=3;

            if(starCount>0)
                playerWin = true;
            else
                playerWin = false;
        } 

        
        
         TurnTimer.StopAllCoroutines();
        BoardController.instance.StopAllCoroutines();

        StartCoroutine(ActivateGameOverDialogue(9f, playerWin));

        GUI_Controller.instance.GameOver();
        
    }

    public IEnumerator ActivateGameOverDialogue(float time, bool playerWin)
    {
        Debug.Log("Activate game over.");
        yield return new WaitForSeconds(time);

        if(ApplicationModel.VS_LOCAL_MP)
        {

        } else if(vsAi)
        {
            if(playerWin)
            {
                GUI_Controller.instance.DialogueController.LoadDialogue("Win", playerScores, playerBestScores, playerErrors, targetScore_3Star);
            } else {
                GUI_Controller.instance.DialogueController.LoadDialogue("Lose", playerScores, playerBestScores, playerErrors, targetScore_3Star);
                AudioManager.instance.Play("YouLose");
            }
        } else if(soloPlay)
        {
            if(playerWin)
            {
                GUI_Controller.instance.DialogueController.LoadDialogue("Win", playerScores, playerBestScores, playerErrors, targetScore_3Star);
            } else {
                GUI_Controller.instance.DialogueController.LoadDialogue("Lose", playerScores, playerBestScores, playerErrors, targetScore_3Star);
                AudioManager.instance.Play("YouLose");
            }

        } else {
            //*-GUI_Controller.instance.DialogueController.LoadDialogue("Win", player1Score, player2Score, averageTurnTime, targetScore);
            GUI_Controller.instance.DialogueController.LoadDialogue("Win", playerScores, playerBestScores, playerErrors, targetScore_3Star); //temp - make a local mp game over screen with summary data
        }

    }

    /// <summary>
    /// Check Spawn Location is called to ensure that grid tiles are always reset to their spawn location when loaded
    /// </summary>
    /// <param name="spawnLoc"></param>
    /// <returns></returns>
    public bool CheckSpawnLocation(int spawnLoc)
    {
        foreach (GridTile Tile in currentHand)
        {
            if(Tile.GetComponent<GUI_Object>().targetPos == spawnLocations[spawnLoc].transform.position)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    ///  Method which updates a given players score, spawns the on-screen popup and updates the target fill bar in solo-mode
    /// </summary>
    /// <param name="val"></param>
    /// <param name="player"></param>
    public void UpdatePlayerScore(int val, int player)
    {
        int newScore = playerScores[player - 1] + val;
        if(soloPlay)
        {
            StartCoroutine(GUI_Controller.instance.UpdateUIScore(playerScores[player - 1], newScore, 
            GUI_Controller.instance.SoloScoreCard.GetComponentInChildren<TextMeshProUGUI>()));
        } else {
            StartCoroutine(GUI_Controller.instance.UpdatePlayerScore(playerScores[player - 1], newScore, turnIndicator));
        }
        playerScores[player - 1] = newScore;
        if(val > playerBestScores[player-1]){playerBestScores[player-1] = val;}

    }


    /// <summary>
    /// Method which returns whether or not the current player has any tiles in their hand
    /// </summary>
    /// <returns></returns>
    public bool CheckRemainingTiles()
    {
        if(currentHand.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Returns the players current hand to the respective tile lists, then ends their turn
    /// </summary>
    public void HandExchange()
    {
        // if(vsAi && turnIndicator == 2)
        //     return;
        if(gameOver)
            return;


        GUI_Controller.instance.SpawnTextPopup("Exchange!", Color.gray, GUI_Controller.instance.exchangePoint.transform, 38);



        StateMachine.RevertToLastValidState(false);

        if(totalRemainingTiles > 0)
        {
            foreach(GridTile Tile in currentHand)
            {
                switch(Tile.value)
                {
                    case 1:
                        OddTiles.Add(1);
                        break;

                    case 2:
                        EvenTiles.Add(2);
                        break;

                    case 3:
                        OddTiles.Add(3);
                        break;

                    case 4:
                        EvenTiles.Add(4);
                        break;

                    case 5:
                        OddTiles.Add(5);
                        break;

                    case 6:
                        OddTiles.Add(6);
                        break;

                    case 7:
                        OddTiles.Add(7);
                        break;
                }
            }

            HidePlayerTiles();
            currentHand.Clear();
            EndTurn();
        } else
        {
            return;
        }
    }

    /// <summary>
    /// Method which is called from hand exchange button - Ensure's the board is in a valid state then calls HandExchange()
    /// </summary>
    public void HandExchangeBtn()
    {
        if(GameMaster.instance.vsAi && !GameMaster.instance.humanTurn)
            return;

        BoardController.instance.CheckBoardValidity(false, false);
        HandExchange();
    }

    /// <summary>
    /// Instantiates a new thread, starting the board evaluator (a thread is used because this operation is too processor intensive to be ran on the main thread) 
    /// </summary>
    public void EndGameDetection()
    {
        Debug.LogWarning("Begin End Game Detection..");
        BoardEvaluator.isPlayerbool(false);
        // // _thread = new Thread(BoardEvaluator.EvaluateBoard);
        // // _thread.Start();
        BoardEvaluator.EvaluateBoard();
    }

}
