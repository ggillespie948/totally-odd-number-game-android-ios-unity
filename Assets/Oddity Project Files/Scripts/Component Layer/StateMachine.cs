using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EZCameraShake;

public class StateMachine : MonoBehaviour {

	[Header("Edit Board State")]
    public Grid_Data BOARD_STATE;
    
    [SerializeField]
    List<GridTile> lastValidHand = new List<GridTile>();
    public List<GridTile> lastValidScoreTiles = new List<GridTile>();
	public int lastValidScore;
	
    [SerializeField]
    List<GridCell> lastTempGrid = new List<GridCell>();
    
     void Start()
    {
        // temp - create a new scriptable object instance - write backend for exporting grid data
    }

    public void LoadGameState()
    {
        Debug.Log("LOADING GAME STATE");
        StopAllCoroutines();
        GameMaster.instance.InitGame();
        for (int i = 0; i < BoardController.instance.GRID_SIZE; i++)
        {
            for (int o = 0; o < BoardController.instance.GRID_SIZE; o++)
            {
               BoardController.instance.gameGrid[o,i] = BOARD_STATE.gameGrid[o,i];
               BoardController.instance.lastValidGameGrid[o,i] = BOARD_STATE.gameGrid[o,i];
               BoardController.instance.staticgameGrid[o,i] = BOARD_STATE.gameGrid[o,i];
            }
        }
        GameMaster.instance.totalTiles = (BOARD_STATE.OneTileCount+BOARD_STATE.TwoTileCount+BOARD_STATE.ThreeTileCount+
                                            BOARD_STATE.FourTileCount+BOARD_STATE.FiveTileCount+BOARD_STATE.SixTileCount+BOARD_STATE.SevenTileCount);
        BoardController.instance.GenerateBoardTiles();
        GameMaster.instance.Player1Hand.Clear();
        GameMaster.instance.EndTurn();
        GameMaster.instance.totalTiles++;
    }


	public void RevertToLastValidState(bool endTurn)
    {
        Debug.LogWarning("REVERT TO LAST STATE");
        if(endTurn)
        {
            AudioManager.instance.Play("Oops");

        } else {
            AudioManager.instance.Play("Exchange");

        }
        LastValidGameGrid();

        //RESET TEMP GRID TILES
        GameMaster.instance.playedTiles.Clear();
        GameMaster.instance.playedTiles.AddRange(lastTempGrid);
        GUI_Controller.instance.TilesScored.Clear();
        GUI_Controller.instance.TilesScored.AddRange(lastValidScoreTiles);

        // foreach(GridCell Cell in GameMaster.instance.playedTiles)
        // {
        //     if(Cell.cellTile.placed == true)
        //     {
        //         GUI_Controller.instance.ActivateCell(Cell.cellTile);
        //     }
        // }
        LastValidHand();
        GameMaster.instance.invalidTilesInplay = false;
        CameraShaker.Instance.ShakeOnce(2f, .2f, .3f, .5f);
        Debug.Log("LAST STATE REVERTED");
        GameMaster.instance.DrawBoardDebug();

        if(endTurn == true)
        {
            GameMaster.instance.EndTurn();
        }
    }

    public void SetLastStaticState()
    {
        for (int i = 0; i <BoardController.instance.GRID_SIZE; i++)
        {
            for (int o = 0; o < BoardController.instance.GRID_SIZE; o++)
            {
                BoardController.instance.staticgameGrid[i, o] = BoardController.instance.gameGrid[i, o];
            }
        }

    }

    public void SetLastValidState(int player)
    {
        //Debug.Log("Setting Last Valid State");
        for (int i = 0; i <BoardController.instance.GRID_SIZE; i++)
        {
            for (int o = 0; o < BoardController.instance.GRID_SIZE; o++)
            {
                BoardController.instance.lastValidGameGrid[i, o] = BoardController.instance.gameGrid[i, o];
            }
        }

        lastValidHand.Clear();
        lastValidHand.AddRange(GameMaster.instance.currentHand);
        lastTempGrid.Clear();
        lastTempGrid.AddRange(GameMaster.instance.playedTiles);
        lastValidScoreTiles.Clear();
        lastValidScoreTiles.AddRange(GUI_Controller.instance.TilesScored);
        GameMaster.instance.invalidTilesInplay = false;

        if(lastTempGrid.Count == 3)
            lastTempGrid.Clear();

        lastValidScore = GameMaster.instance.playerScores[player-1];
        GameMaster.instance.DrawBoardDebug();
    }

    public void LastValidGameGrid()
    {
        for (int i = 0; i < BoardController.instance.GRID_SIZE; i++)
        {
            for (int o = 0; o < BoardController.instance.GRID_SIZE; o++)
            {
                BoardController.instance.gameGrid[i, o] = BoardController.instance.lastValidGameGrid[i, o];
            }
        }

    }

    void LastValidHand()
    {
        GameMaster.instance.currentHand.Clear();
        GameMaster.instance.currentHand.AddRange(lastValidHand);

        foreach(GridTile Tile in GameMaster.instance.currentHand)
        {
            if(Tile == null)
                return;
            Tile.placed = false;
            //GUI_Controller.instance.AnimateTo(Tile.GetComponent<GUI_Object>(), Tile.GetComponent<GUI_Object>().targetPos, .7f);
            GUI_Controller.instance.DeactivateTileSkin(Tile);
            Tile.gameObject.GetComponent<NoGravity>().enabled = true;
            Tile.GetComponent<GUI_Object>().PutObjectDown();
        }

    }

    
}
