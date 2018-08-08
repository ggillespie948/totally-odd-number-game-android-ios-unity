using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class AI_Player : MonoBehaviour {

    //Two lists of valid moves and their matching criteria, to be executed by algorithm
    public int[,] gameGrid;

    //Thread object for caluclation
    Thread _thread;

    public bool movesCalculated = false;
    public bool noValidMoves = false;

    private BoardEvaluator BoardEvaluator;

    public List<GridTile> CurrentHand = new List<GridTile>();

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        BoardEvaluator=GetComponent<BoardEvaluator>();
        BoardEvaluator.Player = this;
    }

    //Calculate tuples from hand
    public void Initalise()
    {
		Debug.Log ("Calculating tile tuples..");
        movesCalculated = false;
        noValidMoves = false;

        //Load current hand from game master
        LoadHand();

        // if(GameMaster.instance.totalTiles ==0)
        // {
        //     PlayFirstTile();    
        // }



        Debug.Log("AI Player Begin Board Evaluation");
        // _thread = new Thread(BoardEvaluator.EvaluateBoard);  
        // _thread.Start();
        //Loop which waits for the thread to complete board evaluation
        StartCoroutine("WaitLoop");

        BoardEvaluator.isPlayerbool(true);
        BoardEvaluator.EvaluateBoard();
    }


    private void LoadHand()
	{
		bool handFound = false;
        int tryCount = 0;
        while(handFound == false)                           // temp - refactor into loadHand()?
        {
            switch(GameMaster.instance.turnIndicator)
            {
                case 1:
                    //Debug.Log("P1 hand loaded");
                    CurrentHand = GameMaster.instance.Player1Hand;
                    break;

                case 2:
                    //Debug.Log("P2 hand loaded");
                    CurrentHand = GameMaster.instance.Player2Hand;
                    break;

                case 3:
                    //Debug.Log("P3 hand loaded");
                    CurrentHand = GameMaster.instance.Player3Hand;
                    break;

                case 4:
                    //Debug.Log("P4 hand loaded");
                    CurrentHand = GameMaster.instance.Player4Hand;
                    break;
            }

            if(CurrentHand.Count == 0)
            {
                Debug.Log("null hand");

                if(movesCalculated && noValidMoves)
                    GameMaster.instance.EndTurn();

            } else
            {
                handFound = true;
            }

            if(tryCount > 200 )  // temp - thread racing causing the need for this?
            {
                Debug.Log("U TRIEDDDDDD"); //TEMP
                handFound = true;
            }

            tryCount++;

        }

	}


    public IEnumerator WaitLoop()
    {

        //Wait for board evaluator thread to finish execution
        while (movesCalculated == false)
        {
            Debug.Log("AI PLAYA Waiting....");
            yield return new WaitForSeconds(1);
        }


        if (noValidMoves == false)
        {
            Debug.Log("Moves found:" + BoardEvaluator.validMove.Count);
            int RandSelect = Mathf.RoundToInt(BoardEvaluator.validMove.Count * (ApplicationModel.GAME_DIFFICULTY/100));

            if(RandSelect<=0)
                RandSelect = 1;

            MakeMove(BoardEvaluator.validMove[RandSelect-1]);
        }


        if (CurrentHand.Count > 0 && noValidMoves == false)
        {
            Debug.Log("Recalculating Possible Moves..");
            Initalise();
            yield break;
        }



        if (CurrentHand.Count == 0 || noValidMoves == true)
        {
			Debug.Log ("Exit point");

            //delay before ending turn to allow for tiles that are in animation to be activated properly
            yield return new WaitForSeconds(1.5f);

            
            foreach(GridCell cell in GameMaster.instance.playedTiles)
            {
                BoardController.instance.CheckAITileValidity(cell.x, cell.y);
            }

            if(CurrentHand.Count > 2 && GameMaster.instance.totalTiles>0)
            {
                GameMaster.instance.HandExchange();
                yield return null;
            }

            BoardController.instance.CheckBoardValidity(true, true);
            
            yield break;
        } 

               
    }


    public void PickMove()
    {
        Debug.Log("Picking move..");

        if(BoardEvaluator.validMove.Count < 1)
        {

            movesCalculated = true;
            noValidMoves = true;
           
            return;
        }

        // map a random int to the range of the length of the valid move set
        movesCalculated = true;
    }


    void MakeMove(AI_Move Move)
    {
        Debug.Log("Making move");

        AI_MatchCriteria Criteria = Move.Criteria;

        // Un-Reverse moves that were calcualted in reverse (this happens because the board is analysed L>R, U>D)
        if (Criteria.startDirection == 'R')
            Criteria.xSpaces.Reverse();
        else if (Criteria.startDirection == 'D')
            Criteria.xSpaces.Reverse();

        //Identify which direction moves are played in, horizontally or vertically
        switch (Criteria.startDirection)
        {
            case 'L': //left
            case 'R': //right
            case 'S': //surround
                //Move is played horitzontally
                MakeMoveHorizontal(Move);
                break;

            case 'U': //up
            case 'D': //down
            case 'X': // surround (vert)
                //Move is played vertically
                MakeMoveVertical(Move);
                break;

        }


    }

    void MakeMoveHorizontal(AI_Move Move)
    {
        Debug.Log("This move consists of : " + Move.tileCount + " moves!!!!! ");
        //temp
		if (Move.Correction != null) {
			Debug.Log ("HORIZONTAL CORRECTION BEING PLAYED");
			Debug.Log ("X: " + Move.Correction.tileX);
			Debug.Log ("Y: " + Move.Correction.tileY);
			Debug.Log ("Val: " + Move.Correction.tileValue);
			Debug.Log ("Dir: " + Move.Correction.correctionDir);
		} else {
			Debug.Log ("correction null");
		}

        // Debug.Log("Move.Criteria.rowColNo :" + Move.Criteria.rowcolNo);
        // Debug.Log("Move.Criteria.Spaces :" + Move.Criteria.spaces);
        // Debug.Log("Move.Criteria.startDir :" + Move.Criteria.startDirection);
        // Debug.Log("Move.Criteria.isEven" + Move.Criteria.isEven);
        // Debug.Log("Move.MoveValue" + Move.moveValue);
        // Debug.Log("Move.tileValues" + Move.tileValues);
        



        //try and place tiles
        AI_MatchCriteria Criteria = Move.Criteria;
        int y = Criteria.rowcolNo;

        int tileCount = 0;
        int tilesPlayed = 0;

        // IS THIS NEEDED? IN BOTH MOVE METHODS? TEMP
        int tempMoveCountCheck = 0;

        foreach(int i in Move.tileValues)
        {
            if(i != 0)
            {
                tempMoveCountCheck++;
            }
        }

        if(tempMoveCountCheck > Move.tileCount)
        {
            Move.tileCount = tempMoveCountCheck;
        }
        //


        bool tilePlayed = false;
        int currentTileVal = Move.tileValues[tileCount];

        foreach (int x in Criteria.xSpaces)
        {
			if (tilesPlayed == Move.tileCount && Move.Correction == null)
            {
                movesCalculated = true;
                break;
            }

            //loop until non 0 val
            while (currentTileVal == 0)
            {
                tileCount++;
                
                if(tileCount <= Move.tileValues.Count-1)
                {
                    currentTileVal = Move.tileValues[tileCount];
                } else {
                    Debug.Log("Safety guard in place"); //TEMP?
                    break;
                }
            }
            //Debugging: move specifications
            // Debug.Log("TILES PLAYED: " + tilesPlayed);
            // Debug.Log("MOVE TILES: " + Move.Tiles);
            // Debug.Log("xSPACELENGTH: " + Criteria.xSpaces.Count);
            // Debug.Log("Move Tile " + currentTileVal + " to: x:" + x + " y:" + y);
            // Debug.Log("Move val:" + Move.MoveValue);
            // Debug.Log("MOVE DIR: " + Criteria.startDirection);
            

            //Place tile on to REAL game grid 
            BoardController.instance.gameGrid[x, y] = currentTileVal;
            tilesPlayed++;

            List<GridTile> RemovalList = new List<GridTile>();
            RemovalList.Clear();

            tilePlayed = false; //temp ?

            foreach (GridTile tile in CurrentHand) //GameMaster.instance.Player1Hand)
            {
                //Debug.Log("tile: " + tile.value + " | (p)currentTile: " + currentTileVal + " | tileplayed: " + tilePlayed);
                if ((tile.value == currentTileVal) && tilePlayed == false)
                {
                    tile.placed = true;
                    tile.placedByAI = true;
                    tile.x=x;
                    tile.y=y;
                    GameMaster.instance.playedTiles.Add(GameMaster.instance.objGameGrid[x, y]);
                    tile.GetComponent<GUI_Object>().targetPos = GameMaster.instance.objGameGrid[x, y].transform.position + new Vector3(0, 0, -1);
                    GUI_Controller.instance.AnimateTo(tile.GetComponent<GUI_Object>(), GameMaster.instance.objGameGrid[x, y].transform.position + new Vector3(0, 0, -1), .8f);
                    GUI_Controller.instance.RotateObjectBackward(tile.gameObject, .8f, 360);
                    GameMaster.instance.objGameGrid[x,y].cellTile = tile;
                    GameMaster.instance.totalTiles++;
                    tilePlayed = true;
                    RemovalList.Add(tile);
                    
                    currentTileVal = 0;
                    break;
                }

            }
            //Debug.Log("Removal list count: " + RemovalList.Count);
            //Debug.Log("HandCount count: " + CurrentHand.Count);

            foreach (GridTile i in RemovalList)
            {
                CurrentHand.Remove(i);    //GameMaster.instance.Player1Hand.Remove(i);
                Debug.Log("Tile removed - new hand count: " + CurrentHand.Count);
            }

            if (tilePlayed == false)
            {
                Debug.Log("TILE NOT FOUND IN PLAYERS HAND");
            }

        }

        if(Move.Correction != null)
        {
            Debug.Log("Correction timeeeeeee");
			List<GridTile> RemovalList = new List<GridTile>();
			RemovalList.Clear();

            foreach (GridTile tile in CurrentHand) 
            {
                if (Move.Correction.tileValue == tile.value)
                {
                    tile.placed = true;
                    tile.placedByAI = true;
                    tile.x=Move.Correction.tileX;
                    tile.y=Move.Correction.tileY;
                    BoardController.instance.gameGrid[Move.Correction.tileX, Move.Correction.tileY] = Move.Correction.tileValue;
                    GameMaster.instance.playedTiles.Add(GameMaster.instance.objGameGrid[Move.Correction.tileX, Move.Correction.tileY]);
                    tile.GetComponent<GUI_Object>().targetPos = GameMaster.instance.objGameGrid[Move.Correction.tileX, Move.Correction.tileY].transform.position + new Vector3(0, 0, -1);
                    GUI_Controller.instance.AnimateTo(tile.GetComponent<GUI_Object>(), GameMaster.instance.objGameGrid[Move.Correction.tileX, Move.Correction.tileY].transform.position + new Vector3(0, 0, -1), .8f);
                    GUI_Controller.instance.RotateObjectBackward(tile.gameObject, .8f, 360);
                    GameMaster.instance.objGameGrid[Move.Correction.tileX, Move.Correction.tileY].cellTile = tile;
                    GameMaster.instance.totalTiles++;
					RemovalList.Add(tile);
                    break;
                }

            }

            Debug.Log("Removing correction tilesssss");

			foreach (GridTile i in RemovalList)
			{
				CurrentHand.Remove(i);    //GameMaster.instance.Player1Hand.Remove(i);
			}
            //GameMaster.instance.StateMachine.SetLastValidState(2);
			noValidMoves = true;
		    
			//Debug.Log (CurrentHand.Count);

        }

        movesCalculated = true;
        GameMaster.instance.StateMachine.SetLastValidState(2); //temp ... having to pass in this int = code stinks
    }

    public bool TileWaitComplete = false;

    IEnumerator TransformTileDelay(float waitTime)
    {
        TileWaitComplete = true;
        yield return new WaitForSeconds(waitTime); //temp make this rand so delays all seem different
        TileWaitComplete = false;
    }

    void MakeMoveVertical(AI_Move Move)
    {
        Debug.Log("This move consists of : " + Move.tileCount + " moves!!!!! ");

        //temp
		if (Move.Correction != null) {
			Debug.Log ("VERTICAL CORRECTION BEING PLAYED");
			Debug.Log ("X: " + Move.Correction.tileX);
			Debug.Log ("Y: " + Move.Correction.tileY);
			Debug.Log ("Val: " + Move.Correction.tileValue);
			Debug.Log ("Dir: " + Move.Correction.correctionDir);
		} else {
			Debug.Log ("correction null");
		}

        //try and place tiles
        AI_MatchCriteria Criteria = Move.Criteria;
        int x = Criteria.rowcolNo;

        int tileCount = 0;
        int tilesPlayed = 0;

        // IS THIS NEEDED? IN BOTH MOVE METHODS? TEMP
        int tempMoveCountCheck = 0;

        foreach(int i in Move.tileValues)
        {
            if(i != 0)
            {
                tempMoveCountCheck++;
            }
        }

        if(tempMoveCountCheck > Move.tileCount)
        {
            Move.tileCount = tempMoveCountCheck;
        }
        //

        bool tilePlayed = false;
        int currentTileVal = Move.tileValues[tileCount];

        foreach (int y in Criteria.xSpaces)
        {
            if (tilesPlayed == Move.tileCount && Move.Correction == null)
            {
                movesCalculated = true;
                break;
            }

            //loop until non 0 val************
            while (currentTileVal == 0)
            {
                tileCount++;
                
                if(tileCount <= Move.tileValues.Count-1)
                {
                    currentTileVal = Move.tileValues[tileCount];
                } else {
                    Debug.Log("Safety guard in place"); //TEMP?
                    break;
                }
            }

            //Debug.Log("TILES PLAYED: " + tilesPlayed);
            //Debug.Log("MOVE TILES: " + Move.Tiles);
            //Debug.Log("xSPACELENGTH: " + Criteria.xSpaces.Count);
            //Debug.Log("Move Tile " + currentTileVal + " to: x:" + x + " y:" + y);
            //Debug.Log("Move val:" + Move.MoveValue);
            //Debug.Log("MOVE DIR: " + Criteria.startDirection);

            //Place tile on to REAL game grid 
            BoardController.instance.gameGrid[x, y] = currentTileVal;
            tilesPlayed++;

            List<GridTile> RemovalList = new List<GridTile>();
            RemovalList.Clear();

            tilePlayed = false; //temp ?

            foreach (GridTile tile in CurrentHand) //GameMaster.instance.Player1Hand)
            {
                //Debug.Log("tile: " + tile.value + " | (p)currentTile: " + currentTileVal + " | tileplayed: " + tilePlayed);
                if ((tile.value == currentTileVal) && tilePlayed == false)
                {
                    tile.placed = true;
                    tile.placedByAI = true;
                    tile.x=x;
                    tile.y=y;
                    GameMaster.instance.playedTiles.Add(GameMaster.instance.objGameGrid[x, y]); 
                    tile.GetComponent<GUI_Object>().targetPos = GameMaster.instance.objGameGrid[x, y].transform.position + new Vector3(0, 0, -1);
                    GUI_Controller.instance.AnimateTo(tile.GetComponent<GUI_Object>(), GameMaster.instance.objGameGrid[x, y].transform.position
                                                     + new Vector3(0, 0, -1), .8f);
                    GUI_Controller.instance.RotateObjectBackward(tile.gameObject, .8f, 360);
                    GameMaster.instance.objGameGrid[x,y].cellTile = tile; //assign tile to grid cell object
                    GameMaster.instance.totalTiles++;
                    tilePlayed = true;
                    RemovalList.Add(tile);
                    
                    tile.GetComponent<NoGravity>().enabled = false;
                    currentTileVal = 0;
                    break;
                }

            }

            foreach (GridTile i in RemovalList)
            {
                CurrentHand.Remove(i);    //GameMaster.instance.Player1Hand.Remove(i);
                //Debug.Log("PLAYER 1 HAND COUNT: " + CurrentHand.Count);
            }

            if (tilePlayed == false)
            {
                Debug.Log("TILE NOT FOUND IN PLAYERS HAND");
            }

            //break;

        }

        if(Move.Correction != null)
        {
            Debug.Log("Correction timeeeeeee vERTICAL");
			List<GridTile> RemovalList = new List<GridTile>();
			RemovalList.Clear();

            foreach (GridTile tile in CurrentHand) 
            {
                if (Move.Correction.tileValue == tile.value)
                {
                    tile.placed = true;
                    tile.placedByAI = true;
                    tile.x=Move.Correction.tileX;
                    tile.y=Move.Correction.tileY;
                    BoardController.instance.gameGrid[Move.Correction.tileX, Move.Correction.tileY] = Move.Correction.tileValue;
                    GameMaster.instance.playedTiles.Add(GameMaster.instance.objGameGrid[Move.Correction.tileX, Move.Correction.tileY]);
                    tile.GetComponent<GUI_Object>().targetPos = GameMaster.instance.objGameGrid[Move.Correction.tileX, Move.Correction.tileY].transform.position + new Vector3(0, 0, -1);
                    GUI_Controller.instance.AnimateTo(tile.GetComponent<GUI_Object>(), GameMaster.instance.objGameGrid[Move.Correction.tileX, Move.Correction.tileY].transform.position + new Vector3(0, 0, -1), .8f);
                    GUI_Controller.instance.RotateObjectBackward(tile.gameObject, .8f, 360);
                    GameMaster.instance.objGameGrid[Move.Correction.tileX, Move.Correction.tileY].cellTile = tile;
                    GameMaster.instance.totalTiles++;
					RemovalList.Add(tile);
                    break;
                }

            }

            Debug.Log("Removing correction tilesssss");

			foreach (GridTile i in RemovalList)
			{
				CurrentHand.Remove(i);    //GameMaster.instance.Player1Hand.Remove(i);
			}
            //GameMaster.instance.StateMachine.SetLastValidState(2);
			noValidMoves = true;
		    
			Debug.Log ("Byeeeee");
			Debug.Log (CurrentHand.Count);

        }

        movesCalculated = true;
        GameMaster.instance.StateMachine.SetLastValidState(2); //temp ... having to pass in this int = code stinks
    }


    bool isEven(int tot)
    {
        return tot % 2 == 0;
    }






}
