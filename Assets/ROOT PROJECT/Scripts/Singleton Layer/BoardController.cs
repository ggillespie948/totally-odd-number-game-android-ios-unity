using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour, Observable {

    //Singleton instance
    public static BoardController instance = null;

    //GameGrids
    public int[,] staticgameGrid; //updated every round
    public int[,] lastValidGameGrid; //updated after each valid board state
    public int[,] gameGrid; //updated after each move
    public List<Observer> observerList;
    public int GRID_SIZE = 7;
    public int GRID_CENTER;
    public bool boardFull = false;
    
    public bool firstEvenTileRequired=false;

    // Use this for initialization
    void Awake () {
        if (instance == null)
        {
        } 
        instance = this;

        GRID_SIZE = ApplicationModel.GRID_SIZE;
        observerList = new List<Observer>();
    }

    public void InitBoard()
    {
        GRID_CENTER = GRID_SIZE-1;
        GRID_CENTER = GRID_CENTER/2;



        //Init Game Grid with 0's
        gameGrid = new int[GRID_SIZE, GRID_SIZE];
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int o = 0; o < GRID_SIZE; o++)
            {
                gameGrid[i, o] = 0;
            }

        }


        lastValidGameGrid = new int[GRID_SIZE, GRID_SIZE];
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int o = 0; o < GRID_SIZE; o++)
            {
                lastValidGameGrid[i, o] = 0;
            }

        }

        staticgameGrid = new int[GRID_SIZE, GRID_SIZE];
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int o = 0; o < GRID_SIZE; o++)
            {
                staticgameGrid[i, o] = 0;
            }

        }

        //save last valid state before change
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int o = 0; o < GRID_SIZE; o++)
            {
                staticgameGrid[i, o] = gameGrid[i, o];
            }

        }

    }

    public void GenerateBoardTiles()
    {
        //Destory all existing tiles
        GUI_Controller.instance.DestroyAllTiles();

        for (int o = 0; o < GRID_SIZE; o++)
        {
            for (int i = 0; i < GRID_SIZE; i++)
            {
                gameGrid[i,o] = GameMaster.instance.StateMachine.BOARD_STATE.gameGrid[i,o];
                
                
            }

        }


    }

    public bool CheckMoveValidity(GridCell cell)
    {
        if(GameMaster.instance.totalTiles == 0 && gameGrid[GRID_CENTER,GRID_CENTER] == 0)
        {
            // first tile must be played in centre
            GUI_Controller.instance.SpawnErrorPopup("Place first tile in center", Color.red, GameMaster.instance.objGameGrid[1,GRID_CENTER].transform, 20);
            GameMaster.instance.StateMachine.RevertToLastValidState(false);

            //animate tile back to start
            if(GameMaster.instance.selectedTile!= null)
            {
                GameMaster.instance.selectedTile.GetComponent<NoGravity>().enabled=true;
                GameMaster.instance.selectedTile.DeafultTileSkin();
                GameMaster.instance.selectedTile.GetComponent<GUI_Object>().targetPos = GameMaster.instance.selectedTile.startPos;
                GUI_Controller.instance.AnimateTo(GameMaster.instance.selectedTile.GetComponent<GUI_Object>(), GameMaster.instance.selectedTile.startPos
                                                + new Vector3(0, 0, -1), .8f);
                GUI_Controller.instance.RotateObjectBackward(GameMaster.instance.selectedTile.gameObject, .8f, 360);
                GameMaster.instance.selectedTile.placed=false;
                
            }

            GameMaster.instance.totalTiles=0;

            return false;
        } else if(GameMaster.instance.totalTiles==0 && gameGrid[GRID_CENTER,GRID_CENTER] != 0 && cell.cellTile.value % 2 != 0)
        {
            AudioManager.instance.Play("play11");
            Debug.LogError("Play tile 11? ");
            


        } else if(GameMaster.instance.totalTiles==0 && gameGrid[GRID_CENTER,GRID_CENTER] != 0 && cell.cellTile.value % 2 == 0)
        {
            GUI_Controller.instance.SpawnErrorPopup("Not odd!", Color.red, GameMaster.instance.objGameGrid[GRID_CENTER,GRID_CENTER].transform, 28);
            //GameMaster.instance.totalTiles--;
            gameGrid[GRID_CENTER,GRID_CENTER]=0;
            
            //animate tile back to start
            if(GameMaster.instance.selectedTile!= null)
            {
                GameMaster.instance.selectedTile.GetComponent<NoGravity>().enabled=true;
                GameMaster.instance.selectedTile.DeafultTileSkin();
                GameMaster.instance.selectedTile.placed=false;
                GameMaster.instance.selectedTile.GetComponent<GUI_Object>().targetPos = GameMaster.instance.selectedTile.startPos;
                GUI_Controller.instance.AnimateTo(GameMaster.instance.selectedTile.GetComponent<GUI_Object>(), GameMaster.instance.selectedTile.startPos
                                                + new Vector3(0, 0, -1), .8f);
                GUI_Controller.instance.RotateObjectBackward(GameMaster.instance.selectedTile.gameObject, .8f, 360);
                
            }
            GameMaster.instance.StateMachine.RevertToLastValidState(false);
            Debug.LogError("Send played tile back to star pos");
            return false;
        }

        GameMaster.instance.staticStateBroken = true;

        bool validity = true;

        List<int> xList = new List<int>();

        List<int> yList = new List<int>();

        //check row has neighbours
        int MT;
        if (cell.y != 0)
        {
            MT = gameGrid[cell.x, cell.y - 1];
        }
        else
        {
            MT = 0;
        }

        int MB;
        if (cell.y != GRID_SIZE-1)
        {
            MB = gameGrid[cell.x, cell.y + 1];
        }
        else
        {
            MB = 0;
        }

        int LM;
        if (cell.x != 0)
        {
            LM = gameGrid[cell.x - 1, cell.y];
        }
        else
        {
            LM = 0;
        }

        int RM;
        if (cell.x != GRID_SIZE-1)
        {
            RM = gameGrid[cell.x + 1, cell.y];
        }
        else
        {
            RM = 0;
        }
        if ((MT == 0) && (MB == 0) && (LM == 0) && (RM == 0) && (GameMaster.instance.totalTiles > 0)) //ADD PIECES > 1
        {
            validity = false; //no neighbours
        }


        //Check the cell has neighbours which already existed on the game grid
        int TM;
        if (cell.y != 0)
        {
            TM = staticgameGrid[cell.x, cell.y - 1];
        }
        else
        {
            TM = 0;
        }

        int BM;
        if (cell.y != GRID_SIZE-1)
        {
            BM = staticgameGrid[cell.x, cell.y + 1];
        }
        else
        {
            BM = 0;
        }

        int ML;
        if (cell.x != 0)
        {
            ML = staticgameGrid[cell.x - 1, cell.y];
        }
        else
        {
            ML = 0;
        }

        int MR;
        if (cell.x != GRID_SIZE-1)
        {
            MR = staticgameGrid[cell.x + 1, cell.y];
        }
        else
        {
            MR = 0;
        }
        if ((TM == 0) && (BM == 0) && (ML == 0) && (MR == 0) && (GameMaster.instance.totalTiles > 0)) //ADD PIECES > 1
        {
            if(validity==true && GameMaster.instance.TUTORIAL_MODE)
            {
                Debug.Log("NO EXISTING NEIGHBOURS but its tutorial mode oooo ye");

            } else 
            {
            //Debug.Log("NO EXISTING NEIGHBOURS");
                validity = false; //no  existing neighbours
            }
            

        }

        //start at placed item,
        yList.Add(gameGrid[cell.x, cell.y]); //add placed cell

        //traverse upwards until 0 found, adding numers to list
        for (int y = cell.y - 1; y >= 0; y--)
        {

            if (gameGrid[cell.x, y] == 0)
            {
                break;
            }
            else
            {
                yList.Add(gameGrid[cell.x, y]);
            }
        }
        //traverse downards until 0 found, add numbers to list,
        for (int y = cell.y + 1; y < GRID_SIZE; y++)
        {

            if (gameGrid[cell.x, y] == 0)
            {
                break;
            }
            else
            {
                yList.Add(gameGrid[cell.x, y]);
            }
        }

        int colTot = 0;
        foreach (int i in yList)
        {
            colTot = colTot + i;
        }
        bool isEven = colTot % 2 == 0;
        if (isEven == true)
        {
            if (yList.Count > 1)
            {
                //Debug.Log("EVEN COL TOTAL");

                if(ApplicationModel.TUTORIAL_MODE)
                {
                    //col tot, yList
                    Tutorial_Controller.instance.AddToErrorLog(colTot, yList, true);
                }

                validity = false;
            }
        }

        xList.Add(gameGrid[cell.x, cell.y]);
        //traverse left until 0 found,adding nubers to list
        for (int x = cell.x - 1; x >= 0; x--)
        {
            if (gameGrid[x, cell.y] == 0)
            {
                break;
            }
            else
            {
                xList.Add(gameGrid[x, cell.y]);
            }

        }

        //traverse right until 0 found,adding nubers to list
        for (int x = cell.x + 1; x < GRID_SIZE; x++)
        {
            if (gameGrid[x, cell.y] == 0)
            {
                break;
            }
            else
            {
                xList.Add(gameGrid[x, cell.y]);
            }

        }

        int rowTot = 0;
        foreach (int i in xList)
        {
            rowTot = rowTot + i;
        }
        bool isEven2 = rowTot % 2 == 0;
        if (isEven2 == true)
        {
            if (xList.Count > 1)
            {
                //Debug.Log("EVEN ROW TOTAL");
                if(ApplicationModel.TUTORIAL_MODE)
                {
                    //col tot, yList
                    Tutorial_Controller.instance.AddToErrorLog(rowTot, xList, false);
                }
                validity = false; //MOVE NOT VALID

            }

        }

        if (validity == false)
        {
            GameMaster.instance.invalidTilesInplay = true;
        }

        // ##### temp - refacto this using current hand
        switch (GameMaster.instance.turnIndicator)
        {
            case 1:
                //remove placed tile form player hand
                GameMaster.instance.Player1Hand.Remove(cell.cellTile);

                //save last valid state before change
                if ((validity == true) && (GameMaster.instance.invalidTilesInplay == false))
                {
                    GameMaster.instance.StateMachine.SetLastValidState(GameMaster.instance.turnIndicator);

                }
                break;

            case 2:
                GameMaster.instance.Player2Hand.Remove(cell.cellTile);
                if ((validity == true) && (GameMaster.instance.invalidTilesInplay == false))
                {
                    GameMaster.instance.StateMachine.SetLastValidState(GameMaster.instance.turnIndicator);
                }
                break;

            case 3:
                GameMaster.instance.Player3Hand.Remove(cell.cellTile);
                if ((validity == true) && (GameMaster.instance.invalidTilesInplay == false))
                {
                    GameMaster.instance.StateMachine.SetLastValidState(GameMaster.instance.turnIndicator);
                }
                break;

            case 4:
                GameMaster.instance.Player4Hand.Remove(cell.cellTile);
                if ((validity == true) && (GameMaster.instance.invalidTilesInplay == false))
                {
                    GameMaster.instance.StateMachine.SetLastValidState(GameMaster.instance.turnIndicator);
                }
                break;
        }

        colTot = 0;
        rowTot = 0;

        
        GameMaster.instance.totalTiles++;
        //GameMaster.instance.lblHandPrint2.text = "total tiles: " + GameMaster.instance.totalTiles;

        if (GameMaster.instance.invalidTilesInplay == true)
        {
            //check the row and cols of every placed tile this turn,
            bool valFlag = true;
            foreach (GridCell Cell in GameMaster.instance.playedTiles)
            {

                if (CheckTileValidity(Cell, false) == false)
                {
                    valFlag = false;
                    //InvalidTiles.Add(Cell);
                }
            }

            //If all are valid
            if (valFlag == true)
            {
                GameMaster.instance.invalidTilesInplay = false;
                GameMaster.instance.StateMachine.SetLastValidState(GameMaster.instance.turnIndicator);
            }
        } else 
        {
            // for(int i=0; i<GameMaster.instance.playedTiles.Count; i++)
            // {
            //     GameMaster.instance.playedTiles[i].cellTile.ActiveTileSkin();
            // }
        }
        
        return validity;
    }

    public int GetNotificationLocation()
    {
        List<int> locs = new List<int>();
        locs.Clear();
        locs.Add(0);
        locs.Add(0);
        locs.Add(0);
        locs.Add(0);
        if(GRID_SIZE==5)
        {
            for(int i=0; i<4; i++)
            {
                for(int u=0; u<4; u++)
                {
                        Debug.LogError("i:" + i + "     u:" + u + " GG:"  + gameGrid[i,u]);
                        Debug.LogError("i:" + i + "     u+1:" + u + " GG:"  + gameGrid[i,u+1]);
                    if(gameGrid[i,u] != 0 || gameGrid[i,u+1] != 0)
                    {
                        locs[i]+=1;
                    }
                }
                Debug.LogError("_______" );
            }
        } else if(GRID_SIZE==7)
        {
            locs.Add(0);
            locs.Add(0);

            for(int i=1; i<5; i++)
            {
                for(int u=0; u<5; u++)
                {
                    if(gameGrid[i,u] != 0 || gameGrid[i,u+1] != 0)
                        locs[i]+=1;
                }
            }
        } else if(GRID_SIZE==9)
        {
            locs.Add(0);
            locs.Add(0);

            for(int i=1; i<7; i++)
            {
                for(int u=0; u<7; u++)
                {
                    if(gameGrid[i,u] != 0 || gameGrid[i,u+1] != 0)
                    {
                        Debug.Log("game grid:" + gameGrid[i,u]);
                        locs[i]+=1;
                    }
                }
            }
        }

        int lowestLoc=0;
        int lowestCount=9999;
        int index=0;
        foreach(int count in locs)
        {

            Debug.LogError("loc index:"+index+ "  value:" +count);
            if(count<lowestCount)
            {
                lowestLoc=index;
                lowestCount=count;
            }

            index++;
        }

        Debug.LogError("lowest loc:" +lowestLoc);


        return lowestLoc;
    }

    /// <summary>
    ///  Method which ensures tiles are connect to existing neighbours on the LAST VALID grid (this ensures all tiles are connected)
    ///  but must stillbe used in conjsution with STATIC neighbour
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool CheckForExistingNeighbours(int x, int y)
    {
        bool hasNeighbours = true;
        
        // if(lastValidGameGrid[x,y]==0)
        // {
        //     Debug.Log("Checking for neighbour of empty cell :S X:Y:" + x + y);
        //     return false;
        // }

        int TM;
        if (y != 0)
        {
            //TM = staticgameGrid[x, y - 1];
            TM = lastValidGameGrid[x, y - 1];
        }
        else
        {
            TM = 0;
        }

        int BM;
        if (y != GRID_SIZE-1)
        {
            BM = lastValidGameGrid[x, y + 1];
        }
        else
        {
            BM = 0;
        }

        int ML;
        if (x != 0)
        {
            ML = lastValidGameGrid[x - 1, y];
        }
        else
        {
            ML = 0;
        }

        int MR;
        if (x != GRID_SIZE-1)
        {
            MR = lastValidGameGrid[x + 1, y];
        }
        else
        {
            MR = 0;
        }
        if ((TM == 0) && (BM == 0) && (ML == 0) && (MR == 0) && (GameMaster.instance.totalTiles > 1)) //ADD PIECES > 1
        {
            //Debug.Log("NO NEEEEEEBS :(");
            hasNeighbours = false; //no  existing neighbours
        }

        return hasNeighbours;

    }

    /// <summary>
    /// This metho is used to determine if a tile is connected to a tile on the last static grid, this ensures that
    /// at least one tile each round is connected to tiles from the previous round of playing
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool CheckForExistingStaticNeighbours(int x, int y)
    {
        bool hasNeighbours = true;
        
        int TM;
        if (y != 0)
        {
            //TM = staticgameGrid[x, y - 1];
            TM = staticgameGrid[x, y - 1];
        }
        else
        {
            TM = 0;
        }

        int BM;
        if (y != GRID_SIZE-1)
        {
            BM = staticgameGrid[x, y + 1];
        }
        else
        {
            BM = 0;
        }

        int ML;
        if (x != 0)
        {
            ML = staticgameGrid[x - 1, y];
        }
        else
        {
            ML = 0;
        }

        int MR;
        if (x != GRID_SIZE-1)
        {
            MR = staticgameGrid[x + 1, y];
        }
        else
        {
            MR = 0;
        }
        if ((TM == 0) && (BM == 0) && (ML == 0) && (MR == 0) && (GameMaster.instance.totalTiles > 1)) //ADD PIECES > 1
        {
            //Debug.Log("NO NEEEEEEBS :(");
            hasNeighbours = false; //no  existing neighbours
        }

        return hasNeighbours;

    }

    public bool ExistingRowCheck(GridCell cell, GridCell cell1)
    {
        List<int> xList = new List<int>();
        List<int> yList = new List<int>();

        xList.Clear();
        yList.Clear();

        //traverse left until 0 found,adding nubers to list
        for (int x = cell.x - 1; x >= 0; x--)
        {
            if (gameGrid[x, cell.y] == 0)
            {
                break;
            }
            else if (cell.y == cell1.y && x == cell1.x)
            {
                return true;
            }

        }

        //traverse right until 0 found,adding nubers to list
        for (int x = cell.x + 1; x < GRID_SIZE; x++)
        {
            if (gameGrid[x, cell.y] == 0)
            {
                break;
            }
            else if (cell.y == cell1.y && x == cell1.x)
            {
                return true;
            }

        }

        return false;



    }

    //Returns true if the two given grid cells are connected via the column they share (no gaps betwen them)
    public bool ExistingColCheck(GridCell cell, GridCell cell1)
    {
        List<int> xList = new List<int>();
        List<int> yList = new List<int>();

        xList.Clear();
        yList.Clear();

        //traverse upwards until 0 or cell2 found
        for (int y = cell.y - 1; y >= 0; y--)
        {
            if (gameGrid[cell.x, y] == 0)
            {
                break;

            }
            else if (cell.x == cell1.x && y == cell1.y)
            {
                return true;
            }

        }
        //traverse downards until 0 found, add numbers to list,
        for (int y = cell.y + 1; y < GRID_SIZE; y++)
        {
            if (gameGrid[cell.x, y] == 0)
            {
                break;
            }
            else if (cell.x == cell1.x && y == cell1.y)
            {
                return true;
            }
        }


        return false;
    }

    public int ScoreTileRow(GridCell cell)
    {

        List<int> xList = new List<int>();

        // 
        xList.Clear();
        xList.Add(gameGrid[cell.x, cell.y]);

        //Add tile to GUI list for score effect
        //GUI_Controller.instance.TilesScored.Add(GameMaster.instance.objGameGrid[cell.x,cell.y].cellTile);


        //traverse left until 0 found,adding nubers to list
        for (int x = cell.x - 1; x >= 0; x--)
        {

            if (gameGrid[x, cell.y] == 0)
            {
                break;
            }
            else
            {
                xList.Add(gameGrid[x, cell.y]);
                //GUI_Controller.instance.TilesScored.Add(GameMaster.instance.objGameGrid[x,cell.y].cellTile);
            }

        }

        //traverse right until 0 found,adding nubers to list
        for (int x = cell.x + 1; x < GRID_SIZE; x++)
        {


            if (gameGrid[x, cell.y] == 0)
            {
                break;
            }
            else
            {
                xList.Add(gameGrid[x, cell.y]);
                //GUI_Controller.instance.TilesScored.Add(GameMaster.instance.objGameGrid[x,cell.y].cellTile);
            }

        }

        //Calculate Row Total
        int rowTot = 0;
        foreach (int i in xList)
        {
            rowTot = rowTot + i;
        }

        if (xList.Count == 1)
        {
            rowTot = 0;
        }

        return rowTot;


    }

    public void FindEffectedTiles(GridCell cell)
    {
        List<int> xList = new List<int>();

        // 
        xList.Clear();
        xList.Add(gameGrid[cell.x, cell.y]);

        //Add tile to GUI list for score effect
        GUI_Controller.instance.TilesScored.Add(GameMaster.instance.objGameGrid[cell.x,cell.y].cellTile);


        //traverse left until 0 found,adding nubers to list
        for (int x = cell.x - 1; x >= 0; x--)
        {

            if (gameGrid[x, cell.y] == 0)
            {
                break;
            }
            else
            {
                xList.Add(gameGrid[x, cell.y]);
                GUI_Controller.instance.TilesScored.Add(GameMaster.instance.objGameGrid[x,cell.y].cellTile);
            }

        }

        //traverse right until 0 found,adding nubers to list
        for (int x = cell.x + 1; x < GRID_SIZE; x++)
        {


            if (gameGrid[x, cell.y] == 0)
            {
                break;
            }
            else
            {
                xList.Add(gameGrid[x, cell.y]);
                GUI_Controller.instance.TilesScored.Add(GameMaster.instance.objGameGrid[x,cell.y].cellTile);
            }

        }


        List<int> yList = new List<int>();

        yList.Clear();

        //start at placed item,
        yList.Add(gameGrid[cell.x, cell.y]); //add placed cell


        //Add tile to GUI list for score effect
        GUI_Controller.instance.TilesScored.Add(GameMaster.instance.objGameGrid[cell.x,cell.y].cellTile);


        //traverse upwards until 0 found, adding numers to list
        for (int y = cell.y - 1; y >= 0; y--)
        {

            if (gameGrid[cell.x, y] == 0)
            {
                break;
            }
            else
            {
                yList.Add(gameGrid[cell.x, y]);
                GUI_Controller.instance.TilesScored.Add(GameMaster.instance.objGameGrid[cell.x,y].cellTile);
            }
        }
        //traverse downards until 0 found, add numbers to list,
        for (int y = cell.y + 1; y < GRID_SIZE; y++)
        {

            if (gameGrid[cell.x, y] == 0)
            {
                break;
            }
            else
            {
                yList.Add(gameGrid[cell.x, y]);
                GUI_Controller.instance.TilesScored.Add(GameMaster.instance.objGameGrid[cell.x,y].cellTile);
            }
        }


    }

    public int ScoreTileCol(GridCell cell)
    {

        List<int> yList = new List<int>();

        yList.Clear();

        //start at placed item,
        yList.Add(gameGrid[cell.x, cell.y]); //add placed cell


        //Add tile to GUI list for score effect
        //GUI_Controller.instance.TilesScored.Add(GameMaster.instance.objGameGrid[cell.x,cell.y].cellTile);


        //traverse upwards until 0 found, adding numers to list
        for (int y = cell.y - 1; y >= 0; y--)
        {

            if (gameGrid[cell.x, y] == 0)
            {
                break;
            }
            else
            {
                yList.Add(gameGrid[cell.x, y]);
                //GUI_Controller.instance.TilesScored.Add(GameMaster.instance.objGameGrid[cell.x,y].cellTile);
            }
        }
        //traverse downards until 0 found, add numbers to list,
        for (int y = cell.y + 1; y < GRID_SIZE; y++)
        {

            if (gameGrid[cell.x, y] == 0)
            {
                break;
            }
            else
            {
                yList.Add(gameGrid[cell.x, y]);
               // GUI_Controller.instance.TilesScored.Add(GameMaster.instance.objGameGrid[cell.x,y].cellTile);
            }
        }

        //Calculate Column Total
        int colTot = 0;
        foreach (int i in yList)
        {
            colTot = colTot + i;
        }

        if (yList.Count == 1)
        {
            colTot = 0;
        }

        return colTot;


    }

    
    public void testFunc()
    {
        //Stop user ending turn on AI turn
        if(GameMaster.instance.vsAi && !GameMaster.instance.humanTurn)
            return;

        StartCoroutine(UnlockSubmitButton());
        GUI_Controller.instance.Submit_Button.GetComponent<BoxCollider>().enabled=false;

        GameMaster.instance.LockCurrentHand();
       


        CheckBoardValidity(true, false); 


        

        
    }

    private IEnumerator UnlockSubmitButton()
    {
        yield return new WaitForSeconds(4.5f);
        GUI_Controller.instance.Submit_Button.GetComponent<SubmitButton>().ResetGreen();
        GUI_Controller.instance.Submit_Button.GetComponent<BoxCollider>().enabled=true;
    }

    
    public bool CheckBoardValidity(bool endTurn, bool isAI) //temp
    {

        List<GridCell> InvalidTiles = new List<GridCell>();
        InvalidTiles.Clear();
        GUI_Controller.instance.TilesScored.Clear();

        //Debug.LogWarning("Played tiles count: " + GameMaster.instance.playedTiles.Count);

        bool staticNeighbourCheck = false;

        foreach (GridCell Cell in GameMaster.instance.playedTiles)
        {

            if (CheckTileValidity(Cell, endTurn) == false)
            {
                //Debug.Log("CELL ADDED TO INVALID TILES LIST!! X:" + Cell.x + " Y:" + Cell.y);
                InvalidTiles.Add(Cell);

            } else
            {
                
            }

            if(GameMaster.instance.totalTiles>3)
            {
                if(!staticNeighbourCheck && CheckForExistingStaticNeighbours(Cell.x, Cell.y))
                {
                    staticNeighbourCheck=true;
                    //Debug.LogWarning("Tile Has Static Neebs");
                }
            } else 
            {
                staticNeighbourCheck=true;
            }

            //GUI_Controller.instance.ScoreTextSpawnLoc = Cell.gameObject.transform.position; //- new Vector3 (0,0,15);

        }

        if(GameMaster.instance.playedTiles.Count == 0)
            staticNeighbourCheck=true;

        //Debug.LogWarning("static need check: " + staticNeighbourCheck);

        //If all are valid, end turn
        if (InvalidTiles.Count == 0 && endTurn && staticNeighbourCheck)
        {
            foreach(GridCell cell in GameMaster.instance.playedTiles)
            {
                BoardController.instance.FindEffectedTiles(cell);
            }


            //foreach temp tile, check if they exist in the same column opr row, and score accordingly
            int colScore = 0;
            int rowScore = 0;

            GameMaster.instance.playerPlayedTiles[GameMaster.instance.turnIndicator-1] += GameMaster.instance.playedTiles.Count; // temp - move this to GameMaster method?

            switch (GameMaster.instance.playedTiles.Count)
            {
                case 1:

                    //SCORE THAT TILE
                    colScore = ScoreTileCol(GameMaster.instance.playedTiles[0]);
                    rowScore = ScoreTileRow(GameMaster.instance.playedTiles[0]);

                    break;

                case 2:

                    if (ExistingColCheck(GameMaster.instance.playedTiles[0], GameMaster.instance.playedTiles[1]))
                    {
                        colScore = ScoreTileCol(GameMaster.instance.playedTiles[0]);
                    }
                    else
                    {
                        colScore = ScoreTileCol(GameMaster.instance.playedTiles[0]);
                        colScore += ScoreTileCol(GameMaster.instance.playedTiles[1]);
                    }

                    if (ExistingRowCheck(GameMaster.instance.playedTiles[0], GameMaster.instance.playedTiles[1]))
                    {
                        rowScore = ScoreTileRow(GameMaster.instance.playedTiles[0]);
                    }
                    else
                    {
                        rowScore = ScoreTileRow(GameMaster.instance.playedTiles[0]);
                        rowScore += ScoreTileRow(GameMaster.instance.playedTiles[1]);
                    }
                    
                    break;

                case 3:

                    //Do first and second tiles share a col?
                    if (ExistingColCheck(GameMaster.instance.playedTiles[0], GameMaster.instance.playedTiles[1]))
                    {
                        //First and second share, does 3rd?
                        if (ExistingColCheck(GameMaster.instance.playedTiles[0], GameMaster.instance.playedTiles[2]))
                        {
                            colScore = ScoreTileCol(GameMaster.instance.playedTiles[0]);

                        }
                        else
                        {
                            //Score (first+second), score 3rd
                            colScore = ScoreTileCol(GameMaster.instance.playedTiles[0]);
                            colScore += ScoreTileCol(GameMaster.instance.playedTiles[2]);

                        }

                    }
                    // 1 and 2 don't match... what about 1 and 3?
                    else if (ExistingColCheck(GameMaster.instance.playedTiles[0], GameMaster.instance.playedTiles[2]))
                    {
                        //1 and 3 match... and we know 1 and 2 don't match
                        colScore = ScoreTileCol(GameMaster.instance.playedTiles[0]); //add 1+3
                        colScore += ScoreTileCol(GameMaster.instance.playedTiles[1]); //add 2

                    }
                    // so 1 and 2 don't match, 1 and 3 don't match.... what about 2 and 3?
                    else if (ExistingColCheck(GameMaster.instance.playedTiles[1], GameMaster.instance.playedTiles[2]))
                    {
                        colScore = ScoreTileCol(GameMaster.instance.playedTiles[0]); //add 1
                        colScore += ScoreTileCol(GameMaster.instance.playedTiles[1]); //add 2+3

                    }
                    else
                    {
                        //none match
                        colScore = ScoreTileCol(GameMaster.instance.playedTiles[0]);
                        colScore += ScoreTileCol(GameMaster.instance.playedTiles[1]);
                        colScore += ScoreTileCol(GameMaster.instance.playedTiles[2]);
                    }

                    //Do first and second tiles share a row?
                    if (ExistingRowCheck(GameMaster.instance.playedTiles[0], GameMaster.instance.playedTiles[1]))
                    {
                        //First and second share, does 3rd?
                        if (ExistingRowCheck(GameMaster.instance.playedTiles[0], GameMaster.instance.playedTiles[2]))
                        {
                            rowScore = ScoreTileRow(GameMaster.instance.playedTiles[0]);

                        }
                        else
                        {
                            //Score (first+second), score 3rd
                            rowScore = ScoreTileRow(GameMaster.instance.playedTiles[0]);
                            rowScore += ScoreTileRow(GameMaster.instance.playedTiles[2]);

                        }

                    }
                    // 1 and 2 don't match... what about 1 and 3?
                    else if (ExistingRowCheck(GameMaster.instance.playedTiles[0], GameMaster.instance.playedTiles[2]))
                    {
                        //1 and 3 match... we know 1 and 2 don't match
                        rowScore = ScoreTileRow(GameMaster.instance.playedTiles[0]); //add 1+3
                        rowScore += ScoreTileRow(GameMaster.instance.playedTiles[1]); //add 2

                    }
                    // so 1 and 2 don't match, 1 and 3 don't match.... what about 2 and 3?
                    else if (ExistingRowCheck(GameMaster.instance.playedTiles[1], GameMaster.instance.playedTiles[2]))
                    {
                        rowScore = ScoreTileRow(GameMaster.instance.playedTiles[0]); //add 1
                        rowScore += ScoreTileRow(GameMaster.instance.playedTiles[1]); //add 2+3

                    }
                    else
                    {
                        //none match
                        rowScore = ScoreTileRow(GameMaster.instance.playedTiles[0]);
                        rowScore += ScoreTileRow(GameMaster.instance.playedTiles[1]);
                        rowScore += ScoreTileRow(GameMaster.instance.playedTiles[2]);
                    }
                    break;
            }
            
            if(GameMaster.instance.playedTiles.Count > 0)
                GUI_Controller.instance.DisableAllEmissions();

            if(GameMaster.instance.totalTiles==1 && gameGrid[GRID_CENTER,GRID_CENTER] != 0 && gameGrid[GRID_CENTER,GRID_CENTER] % 2 != 0)
            {
                //Debug.LogError("valid central tile? ");
                colScore=gameGrid[GRID_CENTER,GRID_CENTER];
            }
             
             
            //SCORE EFFECT AND PROCESS SCORE + NOTIFICATION POP UP //  
            GUI_Controller.instance.TilesScoredEffect(rowScore+colScore);
            ///


            //Debug.Log("turnindicator: " + GameMaster.instance.turnIndicator);
            //Debug.Log("exact score: " + GameMaster.instance.PlayerStatistics.exactScore);
            //Debug.Log("colScore rowScore" + (colScore+rowScore));
            
            
            if(GameMaster.instance.turnIndicator==1 && GameMaster.instance.PlayerStatistics.exactScore!=0 && ((rowScore+colScore)==GameMaster.instance.PlayerStatistics.exactScore))
            {
                //Debug.LogWarning("EXACT TURNSCORE OBJECTIVE MET");
                GameMaster.instance.PlayerStatistics.turnScoreExactMet=true;
            }

            //Update Player SCore
            if(GameMaster.instance.totalTiles==1 && GameMaster.instance.playedTiles.Count >0)
            {
                GameMaster.instance.UpdatePlayerScore(GameMaster.instance.playedTiles[0].cellTile.value, GameMaster.instance.turnIndicator);
            } else {
                GameMaster.instance.UpdatePlayerScore((rowScore + colScore), GameMaster.instance.turnIndicator);

                //if(GameMaster.instance.playedTiles.Count >0)
                    //NotifyObservers(this, "Event.Score."+(rowScore+colScore).ToString());

            }
            
            if(isAI && !GameMaster.instance.humanTurn)
            {
                foreach(GridCell cell in GameMaster.instance.playedTiles)
                {
                    //GUI_Controller.instance.ActivateTile(cell.cellTile);
                }
            }

            //save last valid state before change
            GameMaster.instance.StateMachine.SetLastValidState(GameMaster.instance.turnIndicator);

            if (endTurn)
            {
                GameMaster.instance.LockCurrentHand();
                if(!GameMaster.instance.soloPlay)
                StartCoroutine(GameMaster.instance.TurnTimer.refill());

                GUI_Controller.instance.Submit_Button.GetComponent<SubmitButton>().GlowGreen();
                
                //GameMaster.instance.TurnTimer.enabled=false;
                foreach(GridTile tile in GUI_Controller.instance.GetAllTiles())
                {
                    if(tile.placed==true)
                    {
                        tile.locked=true;
                        if(tile.collider != null)
                        tile.GetComponent<BoxCollider2D>().enabled=false;
                    }
                    
                }
                if(GameMaster.instance.humanTurn)
                {
                    StartCoroutine(GameMaster.instance.EndTurnDelay(3f));
                } else 
                {
                    StartCoroutine(GameMaster.instance.EndTurnDelay(1f));
                }
            }

        }
        else // else END TURN = FALSE  or INVALID TILES IN PLAY in play
        {
            //Debug.LogError("end turn false or invalid tiles in play");
            foreach(GridCell cell in InvalidTiles)
            {
               //Debug.LogError("INCREMENTING PLAYER ERRORS, DEACTIVATING ACTIVATED CELL SKINS AND ENABLE NO GRAV");
                GameMaster.instance.playerErrors[GameMaster.instance.turnIndicator-1]++;
                GameMaster.instance.errorsMade++;
                if(cell.cellTile != null && cell.cellTile.activated)
                {
                    GUI_Controller.instance.DeactivateTileSkin(cell.cellTile);
                    cell.cellTile.gameObject.GetComponent<NoGravity>().enabled = true;
                }
            }

            //if there are unvalid tiles, return grid to last valdi state, return invalid nodes to starpos
            if(GameMaster.instance.TurnTimer.timeLeft > 0.01)
            {
                //GameMaster.instance.StateMachine.RevertToLastValidState(false);
            } else
            {
                 //Debug.LogError("TIME UP INVALID TILES");
                foreach(GridCell cell in InvalidTiles)
                {
                    //Debug.LogError("INCREMENTING PLAYER ERRORS, RESET BOARD POS, ANIM BACK TO HAND");
                    GameMaster.instance.playerErrors[GameMaster.instance.turnIndicator-1]++;
                    GameMaster.instance.errorsMade++;
                }
            }

            GameMaster.instance.UnlockCurrentHand();

        }

        

        if(GameMaster.instance.totalTiles==1 &&  gameGrid[GRID_CENTER,GRID_CENTER] % 2 != 0)
        {
            //Debug.LogWarning("Check board validity: true");
                GameMaster.instance.invalidTilesInplay=true;
                GameMaster.instance.StateMachine.SetLastValidState(GameMaster.instance.turnIndicator);
                return true;
        }

        if(InvalidTiles.Count == 0)
        {
            //Debug.LogWarning("Check board validity: true");
            GameMaster.instance.invalidTilesInplay=false;
            
            GameMaster.instance.StateMachine.SetLastValidState(GameMaster.instance.turnIndicator);
            return true;
        }
        else
        {
            //NotifyObservers(this, "")

            if(GameMaster.instance.TurnTimer.timeLeft < 0.01)
            {
                GUI_Controller.instance.DisableGridBoxColliders();

                //Debug.LogError("TIME UP INVALID TILES in play AGAIN??????????????");
                foreach(GridCell cell in GameMaster.instance.playedTiles)
                {
                    GUI_Controller.instance.SpawnErrorPopup("Not odd!", Color.red, GameMaster.instance.objGameGrid[GRID_CENTER,GRID_CENTER].transform, 28);
                    if(cell.cellTile != null)
                    {
                        //Debug.LogError("RE-RESET BOARD, ADD TO HAND, DEFAULT TILE SKINS, NO GRAC, PLACED, START POSS, CELL TILE");
                        
                        //Reset Board Position
                        BoardController.instance.gameGrid[cell.cellTile.x,cell.cellTile.y]=0;
                        BoardController.instance.staticgameGrid[cell.cellTile.x,cell.cellTile.y]=0;
                        BoardController.instance.lastValidGameGrid[cell.cellTile.x,cell.cellTile.y]=0;

                        //Add tile bgack to hand
                        GameMaster.instance.currentHand.Add(cell.cellTile);

                        //Animate Tile Back To Start Location 
                        //cell.cellTile.transform.position=cell.cellTile.startPos;
                        cell.cellTile.GetComponent<NoGravity>().enabled=true;
                        cell.cellTile.DeafultTileSkin();

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

                        //NotifyObservers(this, "Event.TimesUp");
                        
                    }
                }

                if(GameMaster.instance.selectedTile!= null)
                {
                    GameMaster.instance.selectedTile.GetComponent<NoGravity>().enabled=true;
                    GameMaster.instance.selectedTile.DeafultTileSkin();

                    GameMaster.instance.selectedTile.GetComponent<GUI_Object>().targetPos = GameMaster.instance.selectedTile.startPos;
                    GUI_Controller.instance.AnimateTo(GameMaster.instance.selectedTile.GetComponent<GUI_Object>(), GameMaster.instance.selectedTile.startPos
                                                    + new Vector3(0, 0, -1), .8f);
                    GUI_Controller.instance.RotateObjectBackward(GameMaster.instance.selectedTile.gameObject, .8f, 360);
                    
                    GameMaster.instance.selectedTile.GetComponent<BoxCollider2D>().enabled=false;
                }

                
                            

                GameMaster.instance.ToggleBoxColliders(false, false);
                //GameMaster.instance.HidePlayerTiles();
                //Debug.LogError("END TURN DELAY!!!!! . (DISABLED)");
                if(GameMaster.instance.humanTurn)
                {
                    StartCoroutine(GameMaster.instance.EndTurnDelay(3f));
                } else 
                {
                    StartCoroutine(GameMaster.instance.EndTurnDelay(1f));
                }

            }

            if(endTurn)
            {
                GUI_Controller.instance.Submit_Button.GetComponent<SubmitButton>().GlowRed();
                GUI_Controller.instance.SpawnErrorPopup("Errors+1", Color.red, GameMaster.instance.objGameGrid[GRID_CENTER,GRID_CENTER].transform, 22);
            }
            else
            {
                //GUI_Controller.instance.Submit_Button.GetComponent<SubmitButton>().ResetGreen();
            }
            

            //INVALID TILES IN PLAY, TIME > 0.01
            return false;

        }
        
    }

    public void EventScore(int score)
    {
        NotifyObservers(this, "Event.Score."+(score).ToString());

    }

    
    public int FindNotificationPosition()
    {
        int[] rowTotals = new int[GRID_SIZE];

        foreach(GridTile tile in GUI_Controller.instance.TilesScored)
        {
            if(rowTotals.Length>=tile.y)
            rowTotals[tile.y]+=1;
        }


        int lowestIndex =0;
        int lowestIndexCount =9999;
        List<int> equalRowtots = new List<int>();
        equalRowtots.Clear();
        for(int i=1; i<GRID_SIZE-1; i++)
        {
            if(rowTotals[i]<lowestIndexCount)
            {
                lowestIndex=i;
                lowestIndexCount=rowTotals[i];
            } else if(rowTotals[i] == lowestIndexCount)
            {
                equalRowtots.Add(i);
            }
        }

        //if equal number of rows have same activated tile count,
        //pick a random index 
        if(equalRowtots.Count>1)
        {
            //int rng = Random.Range(0, equalRowtots.Count);
            //Debug.LogWarning("equal row tot counts:  " + equalRowtots.Count);
            //presetLoc=rng;
            int mdl = equalRowtots.Count/2;
           // Debug.LogWarning("Returning MDL: " + mdl);
            return equalRowtots[mdl];
        } 

        //else there was only one lowest index count
        return lowestIndex;

    }

    public bool CheckTileValidity(GridCell cell, bool endTurn)
    {
               
        //Set Validity Flags All to TRUE
        bool validity = true;
        bool colValidity = true;
        bool rowValidity = true;

        List<int> xList = new List<int>();
        List<int> yList = new List<int>();

        xList.Clear();
        yList.Clear();

        //Check at least one item in the column has an existing game grid neighbour
        bool neighbourFound = false;

        //EXISTING NEIGHBOURS CHECK
        //MIDDLE TOP, MIDDLE BOTTOM, LEFT MIDDLE, RIGHT MIDDLE
        
        int MT;
        if (cell.y != 0) //boundary check = 0 top of game grid
        {
            MT = gameGrid[cell.x, cell.y - 1];
        }
        else
        {
            MT = 0;
        }

        int MB;
        if (cell.y != GRID_SIZE-1)
        {
            MB = gameGrid[cell.x, cell.y + 1];
        }
        else
        {
            MB = 0;
        }

        int LM;
        if (cell.x != 0)
        {
            LM = gameGrid[cell.x - 1, cell.y];
        }
        else
        {
            LM = 0;
        }

        int RM;
        if (cell.x != GRID_SIZE-1)
        {
            RM = gameGrid[cell.x + 1, cell.y];
        }
        else
        {
            RM = 0;
        }
        if ((MT == 0) && (MB == 0) && (LM == 0) && (RM == 0) && (GameMaster.instance.totalTiles > 0)) //ADD PIECES > 1
        {
            if(GameMaster.instance.totalTiles > 1)
            {
                validity = false; //no neighbours

                //GUI_Controller.instance.SpawnTextPopup("Not connected!", Color.red, cell.cellTile.transform, 23);

                GameMaster.instance.errorsMade++;
                GameMaster.instance.playerErrors[GameMaster.instance.turnIndicator-1]++;
            }
            //Debug.Log("No neighbours");
        }


        if(validity)
        {

            //start at placed item,
            yList.Add(gameGrid[cell.x, cell.y]); //add placed cell


            //traverse upwards until 0 found, adding numers to list
            if(MT != 0)
            {
                for (int y = cell.y - 1; y >= 0; y--)
                {

                    if (neighbourFound == false)
                    {
                        neighbourFound = CheckForExistingNeighbours(cell.x, y);
                        
                    }

                    if (gameGrid[cell.x, y] == 0)
                    {
                        break;
                    }
                    else
                    {
                        yList.Add(gameGrid[cell.x, y]);
                    }
                }
            }

            //traverse downards until 0 found, add numbers to list,
            if(MB != 0)
            {
                for (int y = cell.y + 1; y < GRID_SIZE; y++)
                {

                    if (neighbourFound == false)
                    {
                        neighbourFound = CheckForExistingNeighbours(cell.x, y);
                    }

                    if (gameGrid[cell.x, y] == 0)
                    {
                        break;
                    }
                    else
                    {
                        yList.Add(gameGrid[cell.x, y]);
                    }
                }
            }

            int colTot = 0;
            foreach (int i in yList)
            {
                colTot = colTot + i;
            }
            bool isEven = colTot % 2 == 0;
            if (isEven == true)
            {
                if (yList.Count > 1)
                {
                    ////Debug.Log("Even col tot (TV)");
                    validity = false;
                    colValidity = false;

                }

            }


            xList.Add(gameGrid[cell.x, cell.y]);
            //traverse left until 0 found,adding nubers to list
            if(LM != 0)
            {
                for (int x = cell.x - 1; x >= 0; x--)
                {

                    if (neighbourFound == false)
                    {
                        neighbourFound = CheckForExistingNeighbours(x, cell.y);
                    }
                    if (gameGrid[x, cell.y] == 0)
                    {
                        break;
                    }
                    else
                    {
                        xList.Add(gameGrid[x, cell.y]);
                    }

                }
            }

            //traverse right until 0 found,adding nubers to list
            if(RM != 0)
            {
                for (int x = cell.x + 1; x < GRID_SIZE; x++)
                {
                    if (neighbourFound == false)
                    {
                        neighbourFound = CheckForExistingNeighbours(x, cell.y);
                    }

                    if (gameGrid[x, cell.y] == 0)
                    {
                        break;
                    }
                    else
                    {
                        xList.Add(gameGrid[x, cell.y]);
                    }

                }
            }

            int rowTot = 0;
            foreach (int i in xList)
            {
                rowTot = rowTot + i;
            }
            bool isEven2 = rowTot % 2 == 0;

            if (isEven2 == true)
            {
                if (xList.Count > 1)
                {
                    validity = false; //MOVE NOT VALID
                    rowValidity = false;

                }

            }

            if (neighbourFound == false)
            {
                validity = false;
            }
        }


        //Update Score

        if (GameMaster.instance.totalTiles == 1)
        {
            validity = true;
            
            //Debug.Log("total tiles = 1");
        }

        if (validity == true)
        {
            //GUI_Controller.instance.ActivateTile(cell.cellTile);
            

        } else
        {

            if(rowValidity == true && colValidity == true)
            {
                ////Debug.Log("ROW VALDITIY: " + rowValidity);
                ////Debug.Log("COL VALDITIY: " + colValidity);
            } else
            {
                // GUI_Controller.instance.SpawnTextPopup("Not Odd!", Color.red, cell.cellTile.transform);
                GUI_Controller.instance.DeactivateTileSkin(cell.cellTile);
            }



        }
        
        //PLAY SOUND FX BASED ON VALIDITY OF MOVE
        if(!endTurn)
        {
            switch(GameMaster.instance.turnIndicator)
            {
                case 1:
                    int playstr = 3-GameMaster.instance.Player1Hand.Count;


                    if (playstr == 0 && GameMaster.instance.invalidTilesInplay == true)
                        break;

                    if(playstr != 3)
                        AudioManager.instance.Play("play" + playstr.ToString());
                    break;

                case 2:
                    int playstr1 = 3 - GameMaster.instance.Player2Hand.Count;


                    if (playstr1 == 0 && GameMaster.instance.invalidTilesInplay == true)
                        break;

                    if(playstr1 != 3)
                        AudioManager.instance.Play("play" + playstr1.ToString());
                    break;


                case 3:
                    int playstr2 = 3 - GameMaster.instance.Player3Hand.Count;


                    if (playstr2 == 0 && GameMaster.instance.invalidTilesInplay == true)
                        break;

                    if(playstr2 != 3)
                        AudioManager.instance.Play("play" + playstr2.ToString());
                    break;


                case 4:
                    int playstr3 = 3 - GameMaster.instance.Player4Hand.Count;


                    if (playstr3 == 0 && GameMaster.instance.invalidTilesInplay == true)
                        break;
                    
                    if(playstr3 != 3)
                        AudioManager.instance.Play("play" + playstr3.ToString());
                    break;
            }
        }

        if(ApplicationModel.TUTORIAL_MODE)
        {
            Tutorial_Controller.instance.rowValidityChecks.Add(rowValidity);
            Tutorial_Controller.instance.colValdityChecks.Add(colValidity);
        }

        return validity;
    }



    ///////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////

    //              AI_BOARD_CONTROLLER_FUNCTIONS

    ///////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////

    public bool CheckAITileValidity(int x, int y)
    {

        // Unused tles always valid
        if(gameGrid[x,y]==0)
            return true;

        //Set Validity Flags All to TRUE
        bool validity = true;
        bool colValidity = true;
        bool rowValidity = true;

        List<int> xList = new List<int>();
        List<int> yList = new List<int>();

        xList.Clear();
        yList.Clear();

        //EXISTING NEIGHBOURS CHECK
        //MIDDLE TOP, MIDDLE BOTTOM, LEFT MIDDLE, RIGHT MIDDLE

        

        //Check at least one item in the column has an existing game grid neighbour
        bool neighbourFound = false;

        //start at placed item,
        yList.Add(gameGrid[x, y]); //add placed cell

        //traverse upwards until 0 found, adding numers to list
        for (int _y = y - 1; _y >= 0; _y--)
        {

            if (neighbourFound == false)
            {
                neighbourFound = CheckForExistingNeighbours(x, _y);
            }

            if (gameGrid[x, _y] == 0)
            {
                break;
            }
            else
            {
                yList.Add(gameGrid[x, _y]);
            }
        }
        //traverse downards until 0 found, add numbers to list,
        for (int _y = y + 1; _y < GRID_SIZE; _y++)
        {

            if (neighbourFound == false)
            {
                neighbourFound = CheckForExistingNeighbours(x, _y);
            }

            if (gameGrid[x, _y] == 0)
            {
                break;
            }
            else
            {
                yList.Add(gameGrid[x, _y]);
            }
        }

        int colTot = 0;
        foreach (int i in yList)
        {
            colTot = colTot + i;
        }
        bool isEven = colTot % 2 == 0;
        if (isEven == true)
        {
            if (yList.Count > 1)
            {
                
                validity = false;
                colValidity=false;

            }

        }


        xList.Add(gameGrid[x, y]);                                      

        //traverse left until 0 found,adding nubers to list
        for (int _x = x - 1; _x >= 0; _x--)
        {
            if (neighbourFound == false)
            {
                neighbourFound = CheckForExistingNeighbours(_x, y);
            }

            if (gameGrid[_x, y] == 0)
            {
                break;
            }
            else
            {
                xList.Add(gameGrid[_x, y]);
            }

        }

        //traverse right until 0 found,adding nubers to list
        for (int _x = x + 1; _x < GRID_SIZE; _x++)
        {
            if (neighbourFound == false)
            {
                neighbourFound = CheckForExistingNeighbours(_x, y);
            }

            if (gameGrid[_x, y] == 0)
            {
                break;
            }
            else
            {
                xList.Add(gameGrid[_x, y]);
            }

        }

        int rowTot = 0;
        foreach (int i in xList)
        {
            rowTot = rowTot + i;
        }
        bool isEven2 = rowTot % 2 == 0;

        if (isEven2 == true)
        {
            if (xList.Count > 1)
            {
                validity = false; //MOVE NOT VALID
                rowValidity=false;

            }

        }


        //neighbourFound = true;

        if (neighbourFound == false)
        {
            validity = false;
            //Debug.Log("AI Valid: last neeb check mf failed :(((( ");
        }

        // if(validity==true && ((y==2 ||y==4) && x==1) && gameGrid[1,4] != 0)
        // {
        //     //Update Score
        //     Debug.LogWarning("Check AI TileVAlidity:");   
        //     Debug.LogWarning("GridTile Value:" + gameGrid[x,y]);           
        //     Debug.LogWarning("x: " + x);
        //     Debug.LogWarning("y: " + y);
        //     Debug.LogWarning("rowTot: " + rowTot);
        //     Debug.LogWarning("colTot: " + colTot);
        //     Debug.LogWarning("rowVal: " + rowValidity);
        //     Debug.LogWarning("colVal: " + colValidity);
        //     // Debug.LogWarning("-------------  X-LIST  -----------------");
        //     // Debug.LogWarning("xList.Count: " + xList.Count);
        //     // foreach(int z in xList)
        //     // {
        //     //     Debug.LogWarning(z);
        //     // }
        //     // Debug.LogWarning("-------------  Y-LIST  -----------------");
        //     // Debug.LogWarning("yList.Count: " + yList.Count);
        //     // foreach(int v in yList)
        //     // {
        //     //     Debug.LogWarning(v);
        //     // }
        //     // Debug.LogWarning("-----------------------------");
        //     //Debug.LogWarning("0:5  - " + gameGrid[0,5]);
        //     //Debug.LogWarning("6:5  - " + gameGrid[6,5]);

        // }

        return validity;
    }

    public virtual void AddObserver(Observer ob)
    {
        observerList.Add(ob);

    }

    public virtual void DeleteObserver(Observer ob)
    {
        observerList.Remove(ob);

    }

    public virtual void NotifyObservers(MonoBehaviour _class, string _event)
    {
        foreach (var Observer in observerList)
        {
            Observer.OnNotification(_class, _event);
        }

    }




}
