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
    private LevelChanger levelChanger;
    
    [SerializeField]
    public bool TUTORIAL_MODE {get; set;}

    [Header("Game Architecture")]
    public StateMachine StateMachine;
    public BoardEvaluator BoardEvaluator;
    public  TutorialController TutorialController;
    public GridCell[,] objGameGrid;

    public PlayerStatistics PlayerStatistics;

    public TurnTimer TurnTimer;

    List<Observer> observers = new List<Observer>();

    [Header("Game Configuration Constants")]
    private static AI_Player AI_PLAYER;
    public static int MAX_TURN_INDICATOR;
    //[Header("Game Options")]
    // private static int START_ONE_TILES;    private int START_TWO_TILES;    private int START_THREE_TILES;
    // private  int START_FOUR_TILES;    private int START_FIVE_TILES;    private static int START_SIX_TILES;
    // private static int START_SEVEN_TILES; private static int START_EIGHT_TILES; private static int START_NINE_TILES;
    private static int[] START_TILE_COUNTS;
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
    public bool humanTurn{get; set;}
    public int turnIndicator {get; private set;}
    public bool invalidTilesInplay {get; set;}


    [Header("Tile Variables")]
    private int totalRemainingTiles;
    // private static GridTile[] tile1;    private static GridTile[] tile2;    private static GridTile[] tile3;
    // private static GridTile[] tile4;    private static GridTile[] tile5;    private static GridTile[] tile6;  
    // private static GridTile[] tile7;    private static GridTile[] tile8;    private static GridTile[] tile9;
    [SerializeField]
    public List<GridTile[]> tileModels = new List<GridTile[]>();

    [SerializeField]
    public GameObject tileModel3d;


    [SerializeField]
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
    public List<int> playerBestTurnActivateTiles{get; set;}
    public List<int> playerPlayedTiles{get; set;}
    public List<int> playerSwaps{get; set;}


    //Spawn Location
    [Header("Other")]
    [SerializeField]
    private List<GameObject> spawnLocations = new List<GameObject>();
    public Theme[] themes;
    public TileSkin[] tileSkins;

    public float averageTurnTime; //not currently implemented
    public static float TILE_SCALE{get; set;}
    public static float GRID_SCALE{get; set;}
    public static float GRID_TILE_LIGHT_RANGE{get; set;}
    public int starCount{get; set;}
    public int errorsMade{get; set;} 

    public bool playerWin {get; set;}
    public bool gridFull {get; set;}

    public bool evenTileRequired{get; set;}

    public bool turnOver {get; private set;}

    


    [Header("Debugging")]
    [SerializeField]
    private GameObject lblGrid;   
    [SerializeField]
    private GameObject lblLVGrid; 
    [SerializeField]
    private TextMeshProUGUI lblHandPrint1;   
    [SerializeField]
    public TextMeshProUGUI lblHandPrint2; 

    void Awake()
    {
        StateMachine = GetComponent<StateMachine>();
        PlayerStatistics = GetComponent<PlayerStatistics>();

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
        //init player error counters
        playerPlayedTiles = new List<int>();
        //init player exchange counts
        playerSwaps = new List<int>();

        turnLimit=20; //default
        soloPlay = ApplicationModel.SOLO_PLAY;
        vsAi = ApplicationModel.VS_AI;
        turnLimit = ApplicationModel.TURNS;
        targetScore_3Star = ApplicationModel.TARGET;

        objGameGrid = new GridCell[9, 9];

        
        MAX_TURN_INDICATOR = ApplicationModel.AI_PLAYERS + ApplicationModel.HUMAN_PLAYERS;
        Debug.LogError("MAX TURN INDICATOR: " + MAX_TURN_INDICATOR);
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
            GUI_Controller.instance.PlayerCard1.gameObject.SetActive(true);
            GUI_Controller.instance.PlayerCard1.GetComponent<PlayerCard>().portraitImage.enabled=false;
            GUI_Controller.instance.PlayerCard1.GetComponent<PlayerCard>().portraitRing.enabled=false;

            GUI_Controller.instance.PlayerCard2.gameObject.SetActive(true);
            GUI_Controller.instance.PlayerCard2.GetComponent<PlayerCard>().portraitImage.enabled=false;
            GUI_Controller.instance.PlayerCard2.GetComponent<PlayerCard>().portraitRing.enabled=false;
            
            GUI_Controller.instance.PlayerCard3.gameObject.SetActive(true);
            GUI_Controller.instance.PlayerCard3.GetComponent<PlayerCard>().portraitImage.enabled=false;
            GUI_Controller.instance.PlayerCard3.GetComponent<PlayerCard>().portraitRing.enabled=false;

            GUI_Controller.instance.PlayerCard4.gameObject.SetActive(true);
            GUI_Controller.instance.PlayerCard4.GetComponent<PlayerCard>().portraitImage.enabled=false;
            GUI_Controller.instance.PlayerCard4.GetComponent<PlayerCard>().portraitRing.enabled=false;

        } else if (MAX_TURN_INDICATOR ==3)
        {
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

        if(!soloPlay) // vs mode
        {
            GUI_Controller.instance.PlayerCard1.gameObject.GetComponentInParent<HorizontalLayoutGroup>().enabled=true; 
            GUI_Controller.instance.remainingTilesText.gameObject.transform.parent.gameObject.SetActive(true);      
        } else //target mode
        {
            GUI_Controller.instance.PlayerCard1.gameObject.GetComponentInParent<HorizontalLayoutGroup>().enabled=false; 
            GUI_Controller.instance.remainingTilesText.gameObject.transform.parent.gameObject.SetActive(false);
        }

        Time.timeScale = 1f;
        errorsMade = 0;
        averageTurnTime = 10f; //temp

        // START_ONE_TILES = ApplicationModel.ONE_TILES;                     
        // START_TWO_TILES = ApplicationModel.TWO_TILES;
        // START_THREE_TILES = ApplicationModel.THREE_TILES;
        // START_FOUR_TILES = ApplicationModel.FOUR_TILES;                     
        // START_FIVE_TILES = ApplicationModel.FIVE_TILES;
        // START_SIX_TILES = ApplicationModel.SIX_TILES;
        // START_SEVEN_TILES = ApplicationModel.SEVEN_TILES;
        // START_EIGHT_TILES = ApplicationModel.EIGHT_TILES;
        // START_NINE_TILES = ApplicationModel.NINE_TILES;

        START_TILE_COUNTS = ApplicationModel.START_TILE_COUNTS;

        if(BoardController.instance.GRID_SIZE == 9)
        {
            GUI_Controller.instance.physcialGridContainer = GUI_Controller.instance.GameGrid_9x9;
            GUI_Controller.instance.physicalGridBG = GUI_Controller.instance.GameGridBG_9x9;
            GUI_Controller.instance.physicalGridDivider = GUI_Controller.instance.GameGrid_9x9_MainBG;
            //GUI_Controller.instance.GameGrid_9x9.SetActive(true);
            GRID_SCALE = .8f;
            TILE_SCALE=0.33f;
            GRID_TILE_LIGHT_RANGE = 2.5f;
        }
        else if(BoardController.instance.GRID_SIZE == 5)
        {
            GUI_Controller.instance.physcialGridContainer = GUI_Controller.instance.GameGrid_5x5;
            GUI_Controller.instance.physicalGridBG = GUI_Controller.instance.GameGridBG_5x5;
            GUI_Controller.instance.physicalGridDivider = GUI_Controller.instance.GameGrid_5x5_MainBG;
            //GUI_Controller.instance.GameGrid_5x5.SetActive(true);
            GRID_SCALE = 1.3f;
            TILE_SCALE=0.55f;
        } else if(BoardController.instance.GRID_SIZE == 7)
        {
            GUI_Controller.instance.physcialGridContainer = GUI_Controller.instance.GameGrid_7x7;
            GUI_Controller.instance.physicalGridBG = GUI_Controller.instance.GameGridBG_7x7;
            GUI_Controller.instance.physicalGridDivider = GUI_Controller.instance.GameGrid_7x7_MainBG;
            //GUI_Controller.instance.GameGrid_7x7.SetActive(true);
            GRID_SCALE = 1f;
            TILE_SCALE = .45f;
        } else {
            GUI_Controller.instance.physcialGridContainer = GUI_Controller.instance.GameGrid_5x5;
            //GUI_Controller.instance.GameGrid_5x5.SetActive(true);
            GRID_SCALE = 1.3f;
            TILE_SCALE=0.55f;

        }

        GUI_Controller.instance.Grid_Scale = new Vector3(GRID_SCALE,GRID_SCALE,GRID_SCALE);

        if(soloPlay)        //Refactor solo play as child of game master temp
        {
            GUI_Controller.instance.SoloTargetCard.gameObject.transform.parent.gameObject.SetActive(true);
            //Destroy(GUI_Controller.instance.TimerUI.gameObject);
        } else {
            GUI_Controller.instance.SoloTargetCard.gameObject.transform.parent.gameObject.SetActive(false);
            GUI_Controller.instance.SoloTargetCard.gameObject.SetActive(false);
            GUI_Controller.instance.SoloScoreCard.gameObject.SetActive(false);
        }

        SetTheme();
        
        
    }

    void SetTheme()
    {
        Theme currentTheme = themes[ApplicationModel.THEME];
        Theme altTheme = themes[2];

        GUI_Controller.instance.directionalLight.intensity=currentTheme.SunLightIntensity;
        RenderSettings.skybox = currentTheme.Skybox;

        //Enable / Disabled sky box rotation
        if(!currentTheme.skyBoxRotationEnabled)
            GUI_Controller.instance.gameObject.GetComponent<SkyboxRotator>().enabled=false;

        
        //enable/dsiabled gradient BG which masks skybox
        if(!currentTheme.gradientBgEnabled)
        {
            GUI_Controller.instance.UI_GRADIENT_BG.SetActive(false);
        } else 
        {
            GUI_Controller.instance.UI_GRADIENT_BG.SetActive(true);
            GUI_Controller.instance.UI_GRADIENT_BG.GetComponent<UnityEngine.UI.Extensions.Gradient>().vertex1=currentTheme.GradientVertext1;
            GUI_Controller.instance.UI_GRADIENT_BG.GetComponent<UnityEngine.UI.Extensions.Gradient>().vertex2=currentTheme.GradientVertext2;
        }

        //Enable / Disable Visible Cell Dividers
        if(!currentTheme.gridDividerEnabled)
        {
            GUI_Controller.instance.physicalGridDivider.gameObject.SetActive(false);
        } else 
        {
            GUI_Controller.instance.physicalGridDivider.gameObject.SetActive(true);
            GUI_Controller.instance.physicalGridDivider.GetComponent<Renderer>().material=currentTheme.GridDivider;
        }

        //Initialise player tile models
        // tile1=new GridTile[MAX_TURN_INDICATOR];
        // tile2=new GridTile[MAX_TURN_INDICATOR];
        // tile3=new GridTile[MAX_TURN_INDICATOR];
        // tile4=new GridTile[MAX_TURN_INDICATOR];
        // tile5=new GridTile[MAX_TURN_INDICATOR];
        // tile6=new GridTile[MAX_TURN_INDICATOR];
        // tile7=new GridTile[MAX_TURN_INDICATOR];
        // tile8=new GridTile[MAX_TURN_INDICATOR];
        // tile9=new GridTile[MAX_TURN_INDICATOR];

        Debug.Log("default skins len" +  tileSkins[ApplicationModel.TILESKIN].defaultSkins.Length);
        
        GUI_Controller.instance.tileFX = new List<GameObject[]>();
        //Initialise the material / activate FX stores
        for(int i=0; i<MAX_TURN_INDICATOR; i++)
        {
            GUI_Controller.instance.tileFX.Add(new GameObject[20]);
        }

        for(int playerIndex=0; playerIndex<MAX_TURN_INDICATOR;playerIndex++)
        {
            Debug.LogError("assigning PLAYER tile skin data");
            for(int tileValIndex=0; tileValIndex<ApplicationModel.MAX_TILE-1; tileValIndex++)
            {
                Debug.LogError("assign tile skin");
                GUI_Controller.instance.tileFX[playerIndex][tileValIndex]=tileSkins[ApplicationModel.TILESKIN+playerIndex].activateFX[tileValIndex];
            }
        }


        // GUI_Controller.instance.Tile1ActivateFX=new GameObject[MAX_TURN_INDICATOR];
        // GUI_Controller.instance.Tile2ActivateFX=new GameObject[MAX_TURN_INDICATOR];
        // GUI_Controller.instance.Tile3ActivateFX=new GameObject[MAX_TURN_INDICATOR];
        // GUI_Controller.instance.Tile4ActivateFX=new GameObject[MAX_TURN_INDICATOR];
        // GUI_Controller.instance.Tile5ActivateFX=new GameObject[MAX_TURN_INDICATOR];
        // GUI_Controller.instance.Tile6ActivateFX=new GameObject[MAX_TURN_INDICATOR];
        // GUI_Controller.instance.Tile7ActivateFX=new GameObject[MAX_TURN_INDICATOR];
        // GUI_Controller.instance.Tile8ActivateFX=new GameObject[MAX_TURN_INDICATOR];
        // GUI_Controller.instance.Tile9ActivateFX=new GameObject[MAX_TURN_INDICATOR];

        if(ApplicationModel.MIRROR_TILESKIN)
        {
            for(int i=0; i<MAX_TURN_INDICATOR;i++)
            {
                // tile1[i]=tileSkins[ApplicationModel.TILESKIN].Tile1;
                // tile2[i]=tileSkins[ApplicationModel.TILESKIN].Tile2;
                // tile3[i]=tileSkins[ApplicationModel.TILESKIN].Tile3;
                // tile4[i]=tileSkins[ApplicationModel.TILESKIN].Tile4;
                // tile5[i]=tileSkins[ApplicationModel.TILESKIN].Tile5;
                // tile6[i]=tileSkins[ApplicationModel.TILESKIN].Tile6;
                // tile7[i]=tileSkins[ApplicationModel.TILESKIN].Tile7;
                // tile8[i]=tileSkins[ApplicationModel.TILESKIN].Tile8;
                // tile9[i]=tileSkins[ApplicationModel.TILESKIN].Tile9;

                // GUI_Controller.instance.Tile1ActivateFX[i]=tileSkins[ApplicationModel.TILESKIN].Tile1FX;
                // GUI_Controller.instance.Tile2ActivateFX[i]=tileSkins[ApplicationModel.TILESKIN].Tile2FX;
                // GUI_Controller.instance.Tile3ActivateFX[i]=tileSkins[ApplicationModel.TILESKIN].Tile3FX;
                // GUI_Controller.instance.Tile4ActivateFX[i]=tileSkins[ApplicationModel.TILESKIN].Tile4FX;
                // GUI_Controller.instance.Tile5ActivateFX[i]=tileSkins[ApplicationModel.TILESKIN].Tile5FX;
                // GUI_Controller.instance.Tile6ActivateFX[i]=tileSkins[ApplicationModel.TILESKIN].Tile6FX;
                // GUI_Controller.instance.Tile7ActivateFX[i]=tileSkins[ApplicationModel.TILESKIN].Tile7FX;
                // GUI_Controller.instance.Tile8ActivateFX[i]=tileSkins[ApplicationModel.TILESKIN].Tile8FX;
                // GUI_Controller.instance.Tile9ActivateFX[i]=tileSkins[ApplicationModel.TILESKIN].Tile9FX;
            }

        } else 
        {
            // tile1[0]=tileSkins[ApplicationModel.TILESKIN].Tile1;
            // tile2[0]=tileSkins[ApplicationModel.TILESKIN].Tile2;
            // tile3[0]=tileSkins[ApplicationModel.TILESKIN].Tile3;
            // tile4[0]=tileSkins[ApplicationModel.TILESKIN].Tile4;
            // tile5[0]=tileSkins[ApplicationModel.TILESKIN].Tile5;
            // tile6[0]=tileSkins[ApplicationModel.TILESKIN].Tile6;
            // tile7[0]=tileSkins[ApplicationModel.TILESKIN].Tile7;
            // tile8[0]=tileSkins[ApplicationModel.TILESKIN].Tile8;
            // tile9[0]=tileSkins[ApplicationModel.TILESKIN].Tile9;

            // GUI_Controller.instance.Tile1ActivateFX[0]=tileSkins[ApplicationModel.TILESKIN].Tile1FX;
            // GUI_Controller.instance.Tile2ActivateFX[0]=tileSkins[ApplicationModel.TILESKIN].Tile2FX;
            // GUI_Controller.instance.Tile3ActivateFX[0]=tileSkins[ApplicationModel.TILESKIN].Tile3FX;
            // GUI_Controller.instance.Tile4ActivateFX[0]=tileSkins[ApplicationModel.TILESKIN].Tile4FX;
            // GUI_Controller.instance.Tile5ActivateFX[0]=tileSkins[ApplicationModel.TILESKIN].Tile5FX;
            // GUI_Controller.instance.Tile6ActivateFX[0]=tileSkins[ApplicationModel.TILESKIN].Tile6FX;
            // GUI_Controller.instance.Tile7ActivateFX[0]=tileSkins[ApplicationModel.TILESKIN].Tile7FX;
            // GUI_Controller.instance.Tile8ActivateFX[0]=tileSkins[ApplicationModel.TILESKIN].Tile8FX;
            // GUI_Controller.instance.Tile9ActivateFX[0]=tileSkins[ApplicationModel.TILESKIN].Tile9FX;

            for(int i=1; i<MAX_TURN_INDICATOR;i++)
            {
                // tile1[i]=tileSkins[ApplicationModel.TILESKIN+i].Tile1;
                // tile2[i]=tileSkins[ApplicationModel.TILESKIN+i].Tile2;
                // tile3[i]=tileSkins[ApplicationModel.TILESKIN+i].Tile3;
                // tile4[i]=tileSkins[ApplicationModel.TILESKIN+i].Tile4;
                // tile5[i]=tileSkins[ApplicationModel.TILESKIN+i].Tile5;
                // tile6[i]=tileSkins[ApplicationModel.TILESKIN+i].Tile6;
                // tile7[i]=tileSkins[ApplicationModel.TILESKIN+i].Tile7;
                // tile8[i]=tileSkins[ApplicationModel.TILESKIN+i].Tile8;
                // tile9[i]=tileSkins[ApplicationModel.TILESKIN+i].Tile9;

                // GUI_Controller.instance.Tile1ActivateFX[i]=tileSkins[ApplicationModel.TILESKIN+i].Tile1FX;
                // GUI_Controller.instance.Tile2ActivateFX[i]=tileSkins[ApplicationModel.TILESKIN+i].Tile2FX;
                // GUI_Controller.instance.Tile3ActivateFX[i]=tileSkins[ApplicationModel.TILESKIN+i].Tile3FX;
                // GUI_Controller.instance.Tile4ActivateFX[i]=tileSkins[ApplicationModel.TILESKIN+i].Tile4FX;
                // GUI_Controller.instance.Tile5ActivateFX[i]=tileSkins[ApplicationModel.TILESKIN+i].Tile5FX;
                // GUI_Controller.instance.Tile6ActivateFX[i]=tileSkins[ApplicationModel.TILESKIN+i].Tile6FX;
                // GUI_Controller.instance.Tile7ActivateFX[i]=tileSkins[ApplicationModel.TILESKIN+i].Tile7FX;
                // GUI_Controller.instance.Tile8ActivateFX[i]=tileSkins[ApplicationModel.TILESKIN+i].Tile8FX;
                // GUI_Controller.instance.Tile9ActivateFX[i]=tileSkins[ApplicationModel.TILESKIN+i].Tile9FX;

            }

            
        }

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

        foreach(GameObject obj in GUI_Controller.instance.physicalGridBG)
        {
            obj.GetComponent<Renderer>().material=currentTheme.GridOutline;
        }
        
        // tileModels.Add(tile1);
        // tileModels.Add(tile2);
        // tileModels.Add(tile3);
        // tileModels.Add(tile4);
        // tileModels.Add(tile5);
        // tileModels.Add(tile6);
        // tileModels.Add(tile7);
        // tileModels.Add(tile8);
        // tileModels.Add(tile9);
    }

    public void ExitToTitle()
    {
        Time.timeScale = 1f;
        levelChanger.FadeToLevel("TitleScreen");
    }
    
    public void InitGame()
    {
        StopAllCoroutines();
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

        for(int i=0; i<MAX_TURN_INDICATOR; i++)
        {
            playerPlayedTiles.Add(0);
        }

        for(int i=0; i<MAX_TURN_INDICATOR; i++)
        {
            playerSwaps.Add(0);
        }

        if(soloPlay) 
        {
            targetScore_3Star = (ApplicationModel.TARGET3);  
            targetScore1_1Star = (ApplicationModel.TARGET);
            targetScore2_2Star = (ApplicationModel.TARGET2);
            GUI_Controller.instance.remainingTilesIndicatorText.text="Target Stars";
        }

        turnLimit = ApplicationModel.GRID_SIZE*3;
        
        if(ApplicationModel.GRID_SIZE==9)
            turnLimit=30;

        turnCounter = 0;
        BoardController.instance.InitBoard();

        EvenTiles = new List<int>();
        OddTiles = new List<int>();

        Debug.Log("star ar len boy; "+START_TILE_COUNTS.Length);

        for(int i =1; i<19; i++)
        {
            if(ApplicationModel.START_TILE_COUNTS[i-1] != 0)
                ApplicationModel.MAX_TILE=i+1;

            if(i % 2 ==0)
            {
                for(int o=0; o< ApplicationModel.START_TILE_COUNTS[i-1];o++)
                {
                    Debug.Log("Adding Even Tile:"  + i);
                    EvenTiles.Add(i);
                }
            } else 
            {
                for(int o=0; o< ApplicationModel.START_TILE_COUNTS[i-1]; o++)
                {
                    Debug.Log("Adding Odd Tile:"  + i);
                    OddTiles.Add(i);
                }
            }
        }

        //TurnTimer.instance.gameObject.SetActive(false);
        for(int i=0; i<MAX_TURN_INDICATOR; i++)
        {
            turnIndicator = i;
            humanTurn = turnIdentifier[turnIndicator]; 
            if(humanTurn)
                if(i < GUI_Controller.instance.PlayerCards.Count-1)
                {
                    GUI_Controller.instance.PlayerCards[i].UpdateName("Player " + (i+1));
                    if(AccountInfo.Instance != null) {GUI_Controller.instance.PlayerCards[i].UpdateName(AccountInfo.Instance.Info.PlayerProfile.DisplayName);}
                }
            else
                if(i < GUI_Controller.instance.PlayerCards.Count-1) {GUI_Controller.instance.PlayerCards[i].UpdateName("AI " + (i+1));}
        }

        if(MAX_TURN_INDICATOR == 4)
        {
            for(int i=0; i<4; i++)
            {
                if(GameMaster.instance.tileSkins.Length>ApplicationModel.TILESKIN+i)
                    GUI_Controller.instance.PlayerCards[i].SetCol(GameMaster.instance.tileSkins[ApplicationModel.TILESKIN+i].tileSkinCol);

            }
        }


        turnIndicator = MAX_TURN_INDICATOR; 
        GUI_Controller.instance.EnableGridBoxColliders();
        if(!TUTORIAL_MODE)
        {
        } else 
        {
            TutorialController.StartTutorial();
        }
        StartCoroutine(Debuger());
        EndTurn();

        if(soloPlay)
            TurnTimer.StartTurn();
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
            int tileCount = 0;
            foreach(GridTile tile in GUI_Controller.instance.GetAllTiles())
            {
                if(tile.placed && tile.activated)
                tileCount++;
            }
            if(tileCount >= BoardController.instance.GRID_SIZE*BoardController.instance.GRID_SIZE)
            {
                BoardController.instance.boardFull=true;
            } else {
                //GUI_Controller.instance.SpawnTextPopup("No Moves!", Color.red, GUI_Controller.instance.gameObject.transform, 38);
                GUI_Controller.instance.NoMovesAnim.SetActive(true);
            }

            GameOver();
        } else {
            Debug.Log("GG = LVG");
            StateMachine.LastValidGameGrid(); 
        }
    }

    public IEnumerator EndTurnDelay(float delay)
    {
        turnOver=true;
        yield return new WaitForSeconds(delay);
        EndTurn();

    }

    public void EndTurn()
    {
        StopCoroutine("WaitingForBoardEvaluator");
        if(GameMaster.instance.isGridComplete())
        {
            GUI_Controller.instance.GridCompleteAnim.SetActive(true);
            gridFull=true;
            GameOver();
        }
        if(gameOver) {return;}
        if(!soloPlay) {turnCounter++;}
        if(vsAi){AI_PLAYER.StopAllCoroutines();}
        BoardEvaluator.StopAllCoroutines();
        BoardController.instance.presetLoc=0;
        if(TurnTimer!=null) {TurnTimer.enabled=true;}
        if(TurnTimer != null && !soloPlay)
        {
            TurnTimer.StartTurn();
        }
        StartCoroutine("WaitingForBoardEvaluator");
        HidePlayerTiles();
        SwitchTurnIndicator();
        GUI_Controller.instance.SwitchEndTurnButton();
        LoadCurrentHand(); 
        GUI_Controller.instance.EnableAllEmissions();
        if(totalTiles >  3 && humanTurn) 
        {
            EndGameDetection();
        }

        if(!TUTORIAL_MODE) {ShowPlayerTiles();}
        playedTiles.Clear();
        DealPlayerTiles(turnIndicator);

        turnOver=false; //bool to stop Times Up calling CheckBoardVAlidity() when the player submits just before time hits 0
        
        if(soloPlay){EndTurn_SoloMode();} 

        StateMachine.SetLastValidState(turnIndicator);
        StateMachine.SetLastStaticState();

        if (vsAi && !humanTurn)
        {
            Debug.Log("INVOKING AI:::");
            Invoke("InvokeAI", 3.5f); //temp ?
            ToggleBoxColliders(false);

        } else
        {
            GUI_Controller.instance.EnableGridBoxColliders();
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
        yield return new WaitForSeconds(1f);
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

        if(!humanTurn)
            GUI_Controller.instance.Submit_Button.GetComponent<SubmitButton>().DeactivateAnimation();
        else
            GUI_Controller.instance.Submit_Button.GetComponent<SubmitButton>().ActivateAnimation();


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

        lblHandPrint1.text = "";
        foreach(GridTile tile in Player1Hand)
        {
            lblHandPrint1.text+=tile.value.ToString();
        }

        lblHandPrint2.text = "Total Tiles: " + totalTiles;
        

    }

    public void PauseGame()
    {
        GUI_Controller.instance.PauseGame();
        HidePlayerTiles();
    }

    public void ResumeGame()
    {
        ShowPlayerTiles();
    }

    public void ToggleBoxColliders(bool Toggle) 
    {
        foreach (GridTile tile in currentHand)
        {
            tile.GetComponent<BoxCollider2D>().enabled = Toggle;
            if(!Toggle)
                DisableTileEmission(tile);
        }
    }

    public void ToggleBoxColliders(bool toggle, bool toggleEmission) 
    {
        foreach (GridTile tile in currentHand)
        {
            tile.GetComponent<BoxCollider2D>().enabled = toggle;
            if(toggleEmission)
                DisableTileEmission(tile);
        }
    }

    private void DisableTileEmission(GridTile tile)
    {
        tile.GetComponent<Renderer>().material.SetColor("_EmissionColor", tile.activeSkin.color*0f);
    }

    public void HidePlayerTiles()
    {
        foreach (GridTile tile in currentHand)
        {
            if(tile!=null){tile.gameObject.SetActive(false);}
            if(tile!=null){tile.gameObject.transform.position = tile.GetComponent<GUI_Object>().returnPos;}
        }

        foreach (GridTile tile in Player1Hand)
        {
            if(tile!=null){tile.gameObject.SetActive(false);}
            if(tile!=null){tile.gameObject.transform.position = tile.GetComponent<GUI_Object>().returnPos;}
        }

        foreach (GridTile tile in Player2Hand)
        {
            if(tile!=null){tile.gameObject.SetActive(false);}
            if(tile!=null){tile.gameObject.transform.position = tile.GetComponent<GUI_Object>().returnPos;}
        }

        foreach (GridTile tile in Player3Hand)
        {
            if(tile!=null){tile.gameObject.SetActive(false);}
            if(tile!=null){tile.gameObject.transform.position = tile.GetComponent<GUI_Object>().returnPos;}
        }

        foreach (GridTile tile in Player4Hand)
        {
            if(tile!=null){tile.gameObject.SetActive(false);}
            if(tile!=null){tile.gameObject.transform.position = tile.GetComponent<GUI_Object>().returnPos;}
        }
    }

    public void ShowPlayerTiles()
    {
        foreach (GridTile tile in currentHand)
        {
            tile.gameObject.SetActive(true);
            if(humanTurn)
                tile.GetComponent<BoxCollider2D>().enabled=true;
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

                //Debug.Log("current rng tile val: " + tileVal);
                if(evenTileRequired && EvenTiles.Count> 0)
                {
                    while(tileVal %2 !=0)
                    {
                        tileVal = Random.Range(1, ApplicationModel.MAX_TILE);
                    }

                    //Debug.Log("NEW tile val:" + tileVal);
                    evenTileRequired=false;
                } else if(EvenTiles.Count == 0)
                {
                    //Debug.Log("Even tile required but count = 0!");
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
                        Debug.LogWarning("TILE DEALING SAFETY MEASURE REACHED");
                        if (tileVal == ApplicationModel.MAX_TILE-1)
                            tileVal = 1;
                        else
                            tileVal++;

                    }

                    //Debug.Log("Tile val:" + tileVal);

                    // Spawn Grid Tile, Assign Materials and Value
                    if(tileVal % 2 == 0)
                    {
                        //Debug.Log("Even val:" + tileVal);
                        if(EvenTiles.Count >0)
                        {
                            //Debug.Log("Enough even tiles.. val:" + tileVal);
                            if(EvenTiles.Remove(tileVal))
                            {
                                //Debug.Log("Even Tile Removed! val:" + tileVal);
                                successFlag=true;
                                GameObject t;
                                t = Instantiate(tileModel3d, transform.position, Quaternion.Euler(0, 0, 0));
                                t.AddComponent<GridTile>();
                                GridTile tile = t.GetComponent<GridTile>();
                                tile.value=tileVal;
                                t.GetComponent<Renderer>().material=GameMaster.instance.tileSkins[ApplicationModel.TILESKIN+(turnIndicator-1)].defaultSkins[tileVal-1];
                                tile.defaultSkin=GameMaster.instance.tileSkins[ApplicationModel.TILESKIN+(turnIndicator-1)].defaultSkins[tileVal-1];
                                tile.activeSkin=GameMaster.instance.tileSkins[ApplicationModel.TILESKIN+(turnIndicator-1)].activeSkins[tileVal-1];
                                t.transform.SetParent(GUI_Controller.instance.gameObject.transform); //tempppppppp?
                                t.transform.localScale = new Vector3(TILE_SCALE, TILE_SCALE, TILE_SCALE);
                                tile.AddObserver(TutorialController);
                                tile.placedBy = turnIndicator;
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
                                t.GetComponent<GridTile>().startPos=spawnLocations[spawnLocCounter].transform.position;
                                t.GetComponent<GUI_Object>().StartIntroAnim(Random.Range(0,1.5f));
                                currentHand.Add(tile);

                            } 
                            //Debug.Log("Could not remove even val:" + tileVal);

                        } 
                    } else 
                    {
                        //Debug.Log("Odd val:" + tileVal);
                        if(OddTiles.Count >0)
                        {
                            //Debug.Log("Enough odd Tiles..:" + tileVal);
                            if(OddTiles.Remove(tileVal))
                            {
                                //Debug.Log("Tile removed!:" + tileVal);
                                successFlag=true;
                                GameObject t;
                                t = Instantiate(tileModel3d, transform.position, Quaternion.Euler(0, 0, 0));
                                t.AddComponent<GridTile>();
                                GridTile tile = t.GetComponent<GridTile>();
                                tile.value=tileVal;
                                t.GetComponent<Renderer>().material=GameMaster.instance.tileSkins[ApplicationModel.TILESKIN+(turnIndicator-1)].defaultSkins[tileVal-1];
                                tile.defaultSkin=GameMaster.instance.tileSkins[ApplicationModel.TILESKIN+(turnIndicator-1)].defaultSkins[tileVal-1];
                                tile.activeSkin=GameMaster.instance.tileSkins[ApplicationModel.TILESKIN+(turnIndicator-1)].activeSkins[tileVal-1];
                                t.transform.SetParent(GUI_Controller.instance.gameObject.transform); //tempppppppp?
                                t.transform.localScale = new Vector3(TILE_SCALE, TILE_SCALE, TILE_SCALE);
                                tile.AddObserver(TutorialController);
                                tile.placedBy = turnIndicator;
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
                                t.GetComponent<GridTile>().startPos=spawnLocations[spawnLocCounter].transform.position;
                                t.GetComponent<GUI_Object>().StartIntroAnim(Random.Range(0,1.5f));
                                currentHand.Add(tile);

                            }
                        } 
                        //Debug.Log("Could not remove odd val:" + tileVal);
                    }



                    // switch (tileVal)
                    // {
                    //     case 1:
                    //         if (OddTiles.Count > 0)
                    //         {
                    //             if(OddTiles.Remove(1))
                    //             {
                    //                 successFlag = true;
                    //                 GridTile t;
                    //                 t = Instantiate(tile1[turnIndicator-1], transform.position, Quaternion.Euler(0, 0, 0));
                    //                 t.gameObject.transform.SetParent(GUI_Controller.instance.gameObject.transform); //tempppppppp?
                    //                 t.gameObject.transform.localScale = new Vector3(TILE_SCALE, TILE_SCALE, TILE_SCALE);
                    //                 t.AddObserver(TutorialController);
                    //                 t.placedBy = turnIndicator;
                    //                 bool spawnFlag = false;
                    //                 while(spawnFlag == false)
                    //                 {
                    //                     if(CheckSpawnLocation(spawnLocCounter) == false)
                    //                     {
                    //                         spawnLocCounter++;
                    //                         if (spawnLocCounter > 2) 
                    //                             spawnLocCounter = 0;
                    //                     } else
                    //                     {
                    //                         spawnFlag = true;
                    //                     }
                    //                 }
                    //                 t.GetComponent<GUI_Object>().SetAnimationTarget(spawnLocations[spawnLocCounter].transform.position);
                    //                 t.GetComponent<GridTile>().startPos=spawnLocations[spawnLocCounter].transform.position;
                    //                 t.GetComponent<GUI_Object>().StartIntroAnim(Random.Range(0,1.5f));
                    //                 currentHand.Add(t);
                    //             } else
                    //             {
                    //                 break;
                    //             }
                    //         }
                    //         break;

                  
                }
            }
            
        }
        
        totalRemainingTiles = OddTiles.Count + EvenTiles.Count;
        Debug.Log("Total Remaining Tiles: " + totalRemainingTiles);
        if(!soloPlay)
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

        if(TurnTimer != null)
            TurnTimer.enabled=false;

            
        BoardController.instance.StopAllCoroutines();

        //GameMaster.instance.PlayerStatistics.PlayedMostTiles(GUI_Controller.instance.GetAllTiles());
        

        

        GUI_Controller.instance.GameOver();
        
    }

    public IEnumerator ActivateGameOverDialogue(float time, bool playerWin, int currencyTotal)
    {
        Debug.Log("Activate game over.");
        yield return new WaitForSeconds(time);
        GUI_Controller.instance.CurrencyUI.gameObject.SetActive(true);
        GUI_Controller.instance.CurrencyUI.AddCurrency(currencyTotal);
        
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


        yield return new WaitForSeconds(1.25f);
        if(AccountInfo.Instance!= null)
        {
            AccountInfo.GetPlayerData(AccountInfo.playfabId);
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
            if(Tile.GetComponent<GUI_Object>().targetPos == spawnLocations[spawnLoc].transform.position || Tile.GetComponent<GUI_Object>().returnPos == spawnLocations[spawnLoc].transform.position)
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

        playerSwaps[turnIndicator-1]++;

        //BoardController.instance.NotifyObservers(BoardController.instance,"Exchange");
        GUI_Controller.instance.SpawnTextPopup("Exchange!", Color.red, GUI_Controller.instance.exchangePoint.transform, 38, GUI_Controller.instance.GetComponent<NotificationController>().fontPresets[0] );

        foreach(GridCell cell in playedTiles)
        {
            if(cell.cellTile != null)
            {
                BoardController.instance.gameGrid[cell.cellTile.x,cell.cellTile.y]=0;
                BoardController.instance.staticgameGrid[cell.cellTile.x,cell.cellTile.y]=0;
                BoardController.instance.lastValidGameGrid[cell.cellTile.x,cell.cellTile.y]=0;
                currentHand.Add(cell.cellTile);
                cell.cellTile=null;
                
            }
        }

        if(totalRemainingTiles > 0)
        {
            foreach(GridTile tile in currentHand)
            {
                switch(tile.value)
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
                        EvenTiles.Add(6);
                        break;

                    case 7:
                        OddTiles.Add(7);
                        break;
                    
                    case 8:
                        EvenTiles.Add(8);
                        break;

                    case 9:
                        OddTiles.Add(9);
                        break;
                }

                Destroy(tile.gameObject);
            }

            DrawBoardDebug();

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
    /// Method which returns true if every grid cell in the gamegrid is populated by a tile and the board is in a valid state
    /// </summary>
    /// <param name="thread"></param>
    /// <returns></returns>
    public bool isGridComplete()
    {
        foreach(GridCell cell in objGameGrid)
        {
            if(cell != null)
            {
                if(cell.cellTile==null)
                    return false;
            }
        }
        if(BoardController.instance.CheckBoardValidity(false,false))
        {
            return true;
        } else 
        {
            return false;
        }
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
