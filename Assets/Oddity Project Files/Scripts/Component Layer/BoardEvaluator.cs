using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoardEvaluator : MonoBehaviour {

	public AI_Player Player; //optional reference back to parent AI player
	private int handCount;
	private bool isPlayer;
	public List<AI_Move> moveCombinations = new List<AI_Move>();
    public List<AI_MatchCriteria> matchCriteria = new List<AI_MatchCriteria>();
    public List<GridTile> currentHand = new List<GridTile>();
    public List<AI_Move> validMove = new List<AI_Move>();
    //Two lists of valid moves and their matching criteria, to be executed by algorithm
    public int[,] gameGrid;
	public bool movesCalculated{ get; private set;}
    public  bool noValidMoves{ get; private set;}

    public bool inEvaluation = false;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        movesCalculated = false;
        noValidMoves = false;
    }

	public void isPlayerbool(bool isplayer)
	{
		if(isplayer)
		{
			isPlayer = true;
		} else 
		{
			isPlayer = false;
		}

	}

	public void GenerateAllMoveCombinations()
	{
		Debug.Log("Generating all move combinations");
		List<int[]> allHands = new List<int[]>();
		handCount = 0;
		int evenCount =0;
		int oddCount =0;
		allHands.Clear();

		evenCount=(GameMaster.instance.EvenTiles.Count);
		oddCount=(GameMaster.instance.OddTiles.Count);

		//Look at both players hands and add tile values to count
		foreach(GridTile tile in GameMaster.instance.Player1Hand)
		{
			switch(tile.value)
			{
				case 1:
				oddCount++;
				break;

				case 2:
				evenCount++;
				break;

				case 3:
				oddCount++;
				break;

                case 4:
				evenCount++;
				break;

				case 5:
				oddCount++;
				break;

				case 6:
				evenCount++;
				break;

				case 7:
				oddCount++;
				break;

			}
		}

		foreach(GridTile tile in GameMaster.instance.Player2Hand)
		{
			switch(tile.value)
			{
				case 1:
				oddCount++;
				break;

				case 2:
				evenCount++;
				break;

				case 3:
				oddCount++;
				break;

                case 4:
				evenCount++;
				break;

				case 5:
				oddCount++;
				break;

                case 6:
				evenCount++;
				break;

				case 7:
				oddCount++;
				break;

				default:
				break;

			}
		}

        foreach(GridTile tile in GameMaster.instance.Player3Hand)
		{
			switch(tile.value)
			{
				case 1:
				oddCount++;
				break;

				case 2:
				evenCount++;
				break;

				case 3:
				oddCount++;
				break;

                case 4:
				evenCount++;
				break;

				case 5:
				oddCount++;
				break;

                case 6:
				evenCount++;
				break;

				case 7:
				oddCount++;
				break;

				default:
				break;

			}
		}

        foreach(GridTile tile in GameMaster.instance.Player4Hand)
		{
			switch(tile.value)
			{
				case 1:
				oddCount++;
				break;

				case 2:
				evenCount++;
				break;

				case 3:
				oddCount++;
				break;

                case 4:
				evenCount++;
				break;

				case 5:
				oddCount++;
				break;

                case 6:
				evenCount++;
				break;

				case 7:
				oddCount++;
				break;

				default:
				break;

			}
		}

		Debug.Log("Odd Count:" + oddCount);
		Debug.Log("Even Count:" + evenCount);




		if(evenCount >= 3 && oddCount>=3)
		{
			int[] TileSet1 = new int[] {1,1,1};
			int[] TileSet2 = new int[] {2,2,2};
			int[] TileSet3 = new int[] {1,2,1};
			int[] TileSet4 = new int[] {2,2,1};
			allHands.Add(TileSet1);
			allHands.Add(TileSet2);
			allHands.Add(TileSet3);
			allHands.Add(TileSet4);
		} else if (evenCount >= 3 && oddCount >=2)
		{
			int[] TileSet2 = new int[] {2,2,2};
			int[] TileSet3 = new int[] {1,2,1};
			int[] TileSet4 = new int[] {2,2,1};
			allHands.Add(TileSet2);
			allHands.Add(TileSet3);
            allHands.Add(TileSet4);
		} else if (evenCount >= 2 && oddCount >=3)
		{
			int[] TileSet2 = new int[] {1,1,1};
			int[] TileSet3 = new int[] {1,2,1};
			int[] TileSet4 = new int[] {2,2,1};
            allHands.Add(TileSet2);
			allHands.Add(TileSet3);
			allHands.Add(TileSet4);
		} else if (evenCount >= 2 && oddCount >=2)
		{
			int[] TileSet3 = new int[] {1,2,1};
			int[] TileSet4 = new int[] {2,2,1};
			allHands.Add(TileSet3);
			allHands.Add(TileSet4);
		} else if (evenCount >= 2 && oddCount >=1)
		{
			int[] TileSet4 = new int[] {2,2,1};
			allHands.Add(TileSet4);

		} else if (evenCount >= 1 && oddCount >=2)
		{
			int[] TileSet4 = new int[] {2,1,1};
			allHands.Add(TileSet4);

		} else if(evenCount >= 1 && oddCount >=1)
		{
			int[] TileSet3 = new int[] {1,2};
			allHands.Add(TileSet3);
		} else if (evenCount >= 1 && oddCount < 1)
		{
			int[] TileSet3 = new int[] {2};
			allHands.Add(TileSet3);
		} else if (evenCount <1 && oddCount >=2)
        {
            int[] TileSet3 = new int[] {1,1};
			allHands.Add(TileSet3);


        } else if (evenCount >=2 && oddCount <1)
        {
            int[] TileSet3 = new int[] {2,2};
			allHands.Add(TileSet3);

        }else if (evenCount <1 && oddCount >=1)
		{
			int[] TileSet3 = new int[] {1};
			allHands.Add(TileSet3);
		} 

		moveCombinations.Clear();
		
		handCount = allHands.Count;

		Debug.Log("HandCount: " + handCount);
		
		foreach(int[] Hand in allHands)
		{
			MoveCombinationFromInts(Hand);
		}


	}

	private void MoveCombinationFromInts(int[] Hand)
	{
		switch (Hand.Length)
        {
            case 0:
                //stop searching for new moves!!!!
				if(isPlayer)
                	Player.noValidMoves = true;
                	break;

            case 1:
                AI_Move T11 = new AI_Move(Hand[0], 1, Hand[0], 0, 0, isEven(Hand[0]));
                moveCombinations.Add(T11);
                break;

            case 2:
                AI_Move T21 = new AI_Move(Hand[0], 1, Hand[0], 0, 0, isEven(Hand[0]));
                AI_Move T22 = new AI_Move(Hand[1], 1, Hand[1], 0, 0, isEven(Hand[1]));
                AI_Move T212 = new AI_Move((Hand[0] + Hand[1]), 2, Hand[0], Hand[1], 0, isEven( (Hand[0] + Hand[1]) ));

                moveCombinations.Add(T21);
                moveCombinations.Add(T22);
                moveCombinations.Add(T212);
                break;

            case 3:
                // 1, 2, 3
                AI_Move T1 = new AI_Move(Hand[0], 1, Hand[0], 0, 0, isEven(Hand[0]));
                AI_Move T2 = new AI_Move(Hand[1], 1, 0, Hand[1], 0, isEven(Hand[1]));
                AI_Move T3 = new AI_Move(Hand[2], 1, 0, 0, Hand[2], isEven(Hand[2]));

                // 1 + 2
                AI_Move T12 = new AI_Move((Hand[0] + Hand[1]),
                    2,
                    Hand[0],
                    Hand[1],
                    0,
                    isEven(Hand[0] + Hand[1]));

                // 1 + 3
                AI_Move T13 = new AI_Move((Hand[0] + Hand[2]),
                    2,
                    Hand[0],
                    0,
                    Hand[2],
                    isEven(Hand[0] + Hand[2]));

                // 2 + 3
                AI_Move T23 = new AI_Move((Hand[1] + Hand[2]),
                    2,
                    0,
                    Hand[1],
                    Hand[2],
                    isEven(Hand[1] + Hand[2]));

                // 1 + 2 + 3
                AI_Move T123 = new AI_Move((Hand[0] + Hand[1] + Hand[2]),
                    3,
                    Hand[0],
                    Hand[1],
                    Hand[2],
                    isEven(Hand[1] + Hand[1] + Hand[2]));


                moveCombinations.Add(T1);
                moveCombinations.Add(T2);
                moveCombinations.Add(T3);
                moveCombinations.Add(T12);
                moveCombinations.Add(T13);
                moveCombinations.Add(T23);
                moveCombinations.Add(T123);
                break;
        }

	}

	public void MoveCombinationsFromHand(List<GridTile> Hand)
	{
		moveCombinations.Clear();
        validMove.Clear();

		currentHand = Hand;

		switch (currentHand.Count)
        {
            case 0:
                //stop searching for new moves!!!!
				if(Player)
                {
                    Debug.Log("EmptyHand");
                	Player.noValidMoves = true;
                }
                break;

            case 1:
                AI_Move T11 = new AI_Move(currentHand[0].value, 1, currentHand[0].value, 0, 0, isEven(currentHand[0].value));
                moveCombinations.Add(T11);
                break;

            case 2:
                AI_Move T21 = new AI_Move(currentHand[0].value, 1, currentHand[0].value, 0, 0, isEven(currentHand[0].value));
                AI_Move T22 = new AI_Move(currentHand[1].value, 1, currentHand[1].value, 0, 0, isEven(currentHand[1].value));
                AI_Move T212 = new AI_Move((currentHand[0].value + currentHand[1].value), 2, currentHand[0].value, currentHand[1].value, 0, isEven( (currentHand[0].value + currentHand[1].value) ));

                moveCombinations.Add(T21);
                moveCombinations.Add(T22);
                moveCombinations.Add(T212);
                break;

            case 3:
                // 1, 2, 3
                AI_Move T1 = new AI_Move(currentHand[0].value, 1, currentHand[0].value, 0, 0, isEven(currentHand[0].value));
                AI_Move T2 = new AI_Move(currentHand[1].value, 1, 0, currentHand[1].value, 0, isEven(currentHand[1].value));
                AI_Move T3 = new AI_Move(currentHand[2].value, 1, 0, 0, currentHand[2].value, isEven(currentHand[2].value));

                // 1 + 2
                AI_Move T12 = new AI_Move((currentHand[0].value + currentHand[1].value),
                    2,
                    currentHand[0].value,
                    currentHand[1].value,
                    0,
                    isEven(currentHand[0].value + currentHand[1].value));

                // 1 + 3
                AI_Move T13 = new AI_Move((currentHand[0].value + currentHand[2].value),
                    2,
                    currentHand[0].value,
                    0,
                    currentHand[2].value,
                    isEven(currentHand[0].value + currentHand[2].value));

                // 2 + 3
                AI_Move T23 = new AI_Move((currentHand[1].value + currentHand[2].value),
                    2,
                    0,
                    currentHand[1].value,
                    currentHand[2].value,
                    isEven(currentHand[1].value + currentHand[2].value));

                // 1 + 2 + 3
                AI_Move T123 = new AI_Move((currentHand[0].value + currentHand[1].value + currentHand[2].value),
                    3,
                    currentHand[0].value,
                    currentHand[1].value,
                    currentHand[2].value,
                    isEven(currentHand[1].value + currentHand[1].value + currentHand[2].value));


                moveCombinations.Add(T1);
                moveCombinations.Add(T2);
                moveCombinations.Add(T3);
                moveCombinations.Add(T12);
                moveCombinations.Add(T13);
                moveCombinations.Add(T23);
                moveCombinations.Add(T123);

                Debug.Log("AI:  Number of move combinations.. " + moveCombinations.Count);
                break;
        }



	}

	

	public void EvaluateBoard()
    {
		movesCalculated=false;
        moveCombinations.Clear();

        //is Player is used to differentiate between AI player move detection and end of game detection
		if(isPlayer)
		{
			MoveCombinationsFromHand(Player.CurrentHand);
		} else  {
			GenerateAllMoveCombinations();
		}
		matchCriteria.Clear();
        validMove.Clear();

        // fetch latest game grid
        gameGrid = BoardController.instance.gameGrid;

        // analyse each row
        for (int i = 0; i < BoardController.instance.GRID_SIZE; i++)
        {
            CalculateMatchCriteria(i);
        }

        // analyse each column
        for (int i = 0; i < BoardController.instance.GRID_SIZE; i++)
        {
            CalculateMatchCriteriaCol(i);
        }

        EvauluateMatchCriteria();

		if(isPlayer)
		{
			Player.PickMove();
		} else 
		{
			movesCalculated = true;
			if(validMove.Count > 0)
			{
				Debug.Log("VALID MOVES FOUND::::::::::: count below");
				Debug.Log(validMove.Count);
			} else {
				Debug.Log("No moves!!!");
				noValidMoves= true;
			}
		}
    }


    /// <summary>
    /// Method which takes a row and calculates all of the possible move match criteria for that row
    /// </summary>
    public void CalculateMatchCriteria(int rowNo) //chaneg this function name
    {

        int cSpaces = 0;

        List<int> xSpaces = new List<int>();
        xSpaces.Clear();

        //traverse gamegrid until a number is found, counting zero's as u go
        for (int x = 0; x < BoardController.instance.GRID_SIZE; x++)
        {
            if (gameGrid[x, rowNo] != 0)
            {
                int tTotal = ScoreTileRow(x, rowNo);

                // so here we have cSpaces, the tTotal (total of all the tiles in a row)
                // so CREATE FIRST match critera () but also STORE the tTiles
                CreateNewMatchCriteria(cSpaces, tTotal, xSpaces, rowNo, isEven(tTotal), 'R');
                break;
            }
            else
            {
                cSpaces++;
                xSpaces.Add(x);
            }

        }
        
    }


    //Override method which is called once the first number set of this row has been found..
    // called via scoreCol()  
    public void CalculateMatchCriteria(int tilePos, int rowNo, int prevtTotal) 
    {

        int cSpaces = 0;

        List<int> xSpaces = new List<int>();
        xSpaces.Clear();

        //traverse gamegrid until a number is found, counting zero's as u go
        for (int x = tilePos; x < BoardController.instance.GRID_SIZE; x++)
        {
            if (gameGrid[x, rowNo] != 0)
            {
                //now create new match criteria for new found number set
                int tTotal = ScoreTileRow(x, rowNo);

                // at this point, you are on one side of one number set and have encountered another, 
                // so a criteria for the full spaces between with odd/even of both totals must be created
                // another criteria for -1 the full criteria with just the first total odd/even
                CreateNewMatchCriteria(cSpaces, tTotal+prevtTotal, xSpaces, rowNo, isEven(tTotal + prevtTotal), 'L'); // critera for the JOIN

                //if there's more than 1 space between numbers also create a match criteria for that (different odd/even rule than joining two odds cols)
                if((cSpaces-1) > 0)
                {
                    CreateNewMatchCriteria(cSpaces-1, prevtTotal, xSpaces, rowNo, isEven(prevtTotal), 'L'); // critera for one short of the join
                }
                break;
            }
            else
            {
                cSpaces++;
                xSpaces.Add(x);
            }

        }

        //if rest of row is just zeros at the very end match criteria for this row
        if(tilePos+cSpaces==BoardController.instance.GRID_SIZE)
        {
            CreateNewMatchCriteria(cSpaces, prevtTotal, xSpaces, rowNo, isEven(prevtTotal), 'L'); //L= start from left
        }
        
    }

    int ScoreTileRow(int tileX, int tileY)
    {
        List<int> xList = new List<int>();
        xList.Clear();

        //traverse right until 0 found,adding nubers to list
        for (int x = tileX; x < BoardController.instance.GRID_SIZE; x++)
        {

            if (gameGrid[x, tileY] == 0)
            {

                //first match criteria could be created here usiong the number of cSpaces with the total of the rowOfTiles!!!

                if(gameGrid[x, tileY] == 0)
                {
                    // Calculate match critera for the opposite side of this number set now
                    int prevtTotal = CalculateListTotal(xList);
                    CalculateMatchCriteria(x, tileY, prevtTotal);

                    // surround LIST
                    List<int> sList = new List<int>();
                    sList.Clear();


                    //Debug.Log("Creating surround criteria...");

                    if(tileX != 0)
                    {
                        sList.Add((tileX-1));
                        sList.Add(x);

                        // if(tileY >= 2)
                        // {
                        //     if(gameGrid[tileX,tileY-2] != 0)
                        //     {
                        //         //traverse down from neighbour to first space, adding total
                        //         for (int yi = tileY-2; yi >= 0; yi--)
                        //         {
                        //             //Debug.LogWarning("Traversing down from surround!!!!!");
                        //             if(gameGrid[tileX,yi] != 0)
                        //             {
                        //                 prevtTotal += gameGrid[tileX,yi];
                        //             } else{
                        //                 //Debug.LogWarning("Break");
                        //                 break;
                        //             }
                        //         }

                        //         //traverse up from neighbour to first space ading total
                        //         for (int yii = tileY; yii < BoardController.instance.GRID_SIZE; yii++)
                        //         {
                        //             //Debug.LogWarning("Traversing up from surround!!!!!");
                        //             if(gameGrid[tileX,yii] != 0)
                        //             {
                        //                 prevtTotal += gameGrid[tileX,yii];
                        //             } else{
                        //                 //Debug.LogWarning("Break");
                        //                 break;
                        //             }
                        //         }

                                
                        //     }
                        // }

                        if(tileX >= 2)
                        {
                            if(gameGrid[tileX-2,tileY] != 0)
                            {
                                //traverse left from neighbour to first space, adding total
                                for (int xi = tileX-2; xi >= 0; xi--)
                                {
                                    //Debug.LogWarning("Traversing left from surround!!!!!");
                                    if(gameGrid[xi,tileY] != 0)
                                    {
                                        prevtTotal += gameGrid[xi,tileY];
                                    } else{
                                        //Debug.LogWarning("Break");
                                        break;
                                    }
                                }

                                //traverse right from neighbour to first space ading total
                                for (int xii = tileX; xii < BoardController.instance.GRID_SIZE; xii++)
                                {
                                    //Debug.LogWarning("Traversing right from surround!!!!!");
                                    if(gameGrid[xii,tileY] != 0)
                                    {
                                        prevtTotal += gameGrid[xii,tileY];
                                    } else{
                                        //Debug.LogWarning("Break");
                                        break;
                                    }
                                }
                            }
                        }

                        //Also create match criteria for the 'surround' - placing a tile at each side of the tile rown
                        CreateNewMatchCriteria(2, prevtTotal+gameGrid[x,tileY], sList, tileY, isEven(prevtTotal), 'S');

                    }



                }


                break;
            }
            else
            {
                xList.Add(gameGrid[x, tileY]);
            }

        }

        //Calculate list total and return
        int rowTot = CalculateListTotal(xList);
        return rowTot;

    }



    // Column Analysis
    //     
    //   CCCC   OOOOO    LL
    //   CC     O   O    LL
    //   CCCC   OOOOO    LLLLLL

    public void CalculateMatchCriteriaCol(int colNo) //chaneg this function name
    {

        int cSpaces = 0;

        List<int> xSpaces = new List<int>();
        xSpaces.Clear();

        //traverse gamegrid until a number is found, counting zero's as u go
        for (int y = 0; y < BoardController.instance.GRID_SIZE; y++)
        {
            if (gameGrid[colNo, y] != 0)
            {
                int tTotal = ScoreTileCol(colNo, y);

                // so here we have cSpaces, the tTotal (total of all the tiles in a row)
                // so CREATE FIRST match critera () but also STORE the tTiles
                CreateNewMatchCriteria(cSpaces, tTotal, xSpaces, colNo, isEven(tTotal), 'D');
                break;
            }
            else
            {
                cSpaces++;
                xSpaces.Add(y);
            }

        }

    }


    public void CalculateMatchCriteriaCol(int tilePos, int colNo, int prevtTotal)
    {

        int cSpaces = 0;

        List<int> xSpaces = new List<int>();
        xSpaces.Clear();

        //traverse gamegrid until a number is found, counting zero's as u go
        for (int y = colNo; y < BoardController.instance.GRID_SIZE; y++)
        {
            if (gameGrid[tilePos, y] != 0)
            {
                //now create new match criteria for new found number set
                int tTotal = ScoreTileCol(tilePos, y);

                // at this point, you are on one side of one number set and have encountered another, 
                // so a criteria for the full spaces between with odd/even of both totals must be created
                // another criteria for -1 the full criteria with just the first total odd/even
                CreateNewMatchCriteria(cSpaces, (tTotal + prevtTotal), xSpaces, tilePos, isEven(tTotal + prevtTotal), 'U'); // critera for the JOIN

                //if there's more than 1 space between numbers also create a match criteria for that (different odd/even rule than joining two odds cols)
                if ((cSpaces - 1) > 0)
                {
                    CreateNewMatchCriteria(cSpaces - 1, prevtTotal, xSpaces, tilePos, isEven(prevtTotal), 'U'); // critera for one short of the join //change to U
                }
                break;
            }
            else
            {
                cSpaces++;
                xSpaces.Add(y);
            }

        }

        //if rest of row is just zeros at the very end match criteria for this row
        if (colNo + cSpaces == BoardController.instance.GRID_SIZE)
        {
            CreateNewMatchCriteria(cSpaces, prevtTotal, xSpaces, tilePos, isEven(prevtTotal), 'U'); //u= 
        }

    }


    int ScoreTileCol(int tileX, int tileY)
    {

        List<int> yList = new List<int>();
        yList.Clear();

        //traverse down until 0 found,adding nubers to list
        for (int y = tileY; y < BoardController.instance.GRID_SIZE; y++)
        {

            if (gameGrid[tileX, y] == 0)
            {

                //first match criteria could be created here usiong the number of cSpaces with the total of the rowOfTiles!!!
                if (gameGrid[tileX, y] == 0)
                {
                    // Calculate match critera for the opposite side of this number set now
                    int prevtTotal = CalculateListTotal(yList);
                    CalculateMatchCriteriaCol(tileX, y, prevtTotal);

                    // surround LIST
                    List<int> sList = new List<int>();
                    sList.Clear();

                    if(tileY != 0)
                    {
                        sList.Add((tileY - 1));
                        sList.Add(y);

                        if(tileY >= 2)
                        {
                            if(gameGrid[tileX,tileY-2] != 0)
                            {
                                //traverse down from neighbour to first space, adding total
                                for (int yi = tileY-2; yi >= 0; yi--)
                                {
                                    //Debug.LogWarning("Traversing down from surround!!!!!");
                                    if(gameGrid[tileX,yi] != 0)
                                    {
                                        prevtTotal += gameGrid[tileX,yi];
                                    } else{
                                        //Debug.LogWarning("Break");
                                        break;
                                    }
                                }

                                //Debug.LogWarning("Transition Col 1");

                                //traverse up from neighbour to first space ading total
                                for (int yii = tileY; yii < BoardController.instance.GRID_SIZE; yii++)
                                {
                                    //Debug.LogWarning("Traversing up from surround!!!!!");
                                    if(gameGrid[tileX,yii] != 0)
                                    {
                                        prevtTotal += gameGrid[tileX,yii];
                                    } else{
                                        //Debug.LogWarning("Break");
                                        break;
                                    }
                                }

                                
                            }
                        }

                        // if(tileX >= 2)
                        // {
                        //     if(gameGrid[tileX-2,tileY] != 0)
                        //     {
                        //         //traverse left from neighbour to first space, adding total
                        //         for (int xi = tileX-2; xi >= 0; xi--)
                        //         {
                        //             //Debug.LogWarning("Traversing left from surround!!!!!");
                        //             if(gameGrid[xi,tileY] != 0)
                        //             {
                        //                 prevtTotal += gameGrid[xi,tileY];
                        //             } else{
                        //                 //Debug.LogWarning("Break");
                        //                 break;
                        //             }
                        //         }

                        //         //Debug.LogWarning("Transition Col 2");

                        //         //traverse right from neighbour to first space ading total
                        //         for (int xii = tileX; xii < BoardController.instance.GRID_SIZE; xii++)
                        //         {
                        //             //Debug.LogWarning("Traversing right from surround!!!!!");
                        //             if(gameGrid[xii,tileY] != 0)
                        //             {
                        //                 prevtTotal += gameGrid[xii,tileY];
                        //             } else{
                        //                 //Debug.LogWarning("Break");
                        //                 break;
                        //             }
                        //         }
                        //     }
                        // }

                        //Also create match criteria for the 'surround' - placing a tile at each side of the tile rown
                        CreateNewMatchCriteria(2, prevtTotal, sList, tileX, isEven(prevtTotal), 'X');
                    }

                }


                break;
            }
            else
            {
                yList.Add(gameGrid[tileX, y]);
            }

        }

        //Calculate list total and return
        int colTot = CalculateListTotal(yList);
        return colTot;

    }







    // Takes cSpaces and tT
    void CreateNewMatchCriteria(int cSpaces, int tTotal, List<int> xSpaces, int rowNo, bool isEven, char Direction)
    {

        if (cSpaces < 1)
            return;



        //Debug.Log("NEW MATCH CRITERIA: tTot:" + tTotal + " cSpc:" + cSpaces + " rowno:" + rowNo + " isEven:" + isEven + " Dir:" + Direction);
        AI_MatchCriteria Criteria = new AI_MatchCriteria(tTotal, cSpaces, xSpaces, rowNo, isEven, Direction);

        matchCriteria.Add(Criteria);

    }

    void EvauluateMatchCriteria()
    {
        //for each MATCH CRITERIA, compare with EACH of the AI moves,
        //if a match is found, then add to VALID MOVE list
        Debug.Log("Evaluating Match Criteria");
        Debug.Log("Moves: " + moveCombinations.Count);
        Debug.Log("Criteria: " + matchCriteria.Count);

        foreach(AI_MatchCriteria Criteria in matchCriteria)
        {
            foreach(AI_Move Move in moveCombinations)
            {
                if(Criteria.isEven != Move.isEven)
                {
                    if(Move.tileCount <= Criteria.spaces)
                    {
                        //Debug.Log("Potential Move Found");
                        TestMove(Move, Criteria);

                    }
                }
            }

        }


        //Even tile check
        // check all the valid moves to check if an EVEN tile is required
        // to make a valid move, if so.. indicate to Game Master to ensure player is dealt an even tile
        bool oddTileFound = false;

        foreach (AI_Move move in validMove)
        {
            if(!oddTileFound)
            {
                foreach(int i in move.tileValues)
                {
                    if(i %2 != 0) {oddTileFound=true; Debug.Log("ODD TILE FOUND IN VALID MOVES");}
                }
            } else 
            {
                break;
            }
        }

        if(oddTileFound)
        {
            GameMaster.instance.evenTileRequired=false;
        } else 
        {
            GameMaster.instance.evenTileRequired=true;
        }

        Debug.Log("Valid AI moves: " + validMove.Count);

		


    }

    void TestMove(AI_Move Move, AI_MatchCriteria Criteria)
    {
        // Reverse moves that were calcualted in reverse (this happens because the board is analysed L>R, U>D)
        if (Criteria.startDirection == 'R')
            Criteria.xSpaces.Reverse();
        else if (Criteria.startDirection == 'D')
            Criteria.xSpaces.Reverse();


        //Identify which direction moves are played in, horizontally or vertically
        switch(Criteria.startDirection)
        {
            case 'L':   case 'R': case 'S':
                //Move is played horitzontally
                TestMoveHorizontal(Move, Criteria);
                break;

            case 'U':   case 'D':   case 'X': 
                //Move is played vertically
                TestMoveVertical(Move, Criteria);
                break;

        }

        // Un-Reverse moves that were calcualted in reverse (this happens because the board is analysed L>R, U>D)
        if (Criteria.startDirection == 'R')
            Criteria.xSpaces.Reverse();
        else if (Criteria.startDirection == 'D')
            Criteria.xSpaces.Reverse();

    }


    void TestMoveHorizontal(AI_Move Move, AI_MatchCriteria Criteria)
    {
        //try and place tiles
        //Debug.LogWarning("Testing new move.");
        int y = Criteria.rowcolNo;

        if(y==5)
        {
            //Debug.LogWarning("testing fifth row..");
            foreach(int t in Move.tileValues)
            {
                //Debug.LogWarning(t);
            }
        }

        int[] tempTileValues = new int[Move.tileValues.Count];
        for(int i=0; i<Move.tileValues.Count;i++)
        {
            tempTileValues[i]=Move.tileValues[i];
        }

        int tileCount = 0;
        int tilesPlayed = 0;
        int currentTileVal = tempTileValues[tileCount];
        foreach (int x in Criteria.xSpaces)
        {

            //if tilesPlayed >= tiles in move then BREAK
            if (tilesPlayed >= Move.tileCount)
            {
                //Debug.LogWarning("Tiles played break;");
                break;
            }

            //loop until non 0 val
            while (currentTileVal == 0)
            {
                tileCount++;
                if(tileCount <= Move.tileValues.Count-1)
                {
                    currentTileVal = tempTileValues[tileCount];
                } else {
                    //Debug.LogWarning("Safety guard in place"); //TEMP?
                    break;
                }
            }

            if (BoardController.instance.gameGrid[x, y] != 0)                               
            {
                //Debug.Log("ERROR ERROR ERROR ERROR:   TRYING TO PLACE TILE ON NON ZERO VALUE");
                return;
            }

            //Debug.LogWarning("PLACING: currentTileVal:" + currentTileVal);
            BoardController.instance.gameGrid[x, y] = currentTileVal;

            tempTileValues[tileCount]=0;
            currentTileVal=0;
            tilesPlayed++;
        }


        //All tiles played....
        // CHECK TILE VALIDITY
        bool isValid = true;
        foreach (int x in Criteria.xSpaces)
        {

            if(BoardController.instance.gameGrid[x, y] != 0)
            {
                //Debug.LogWarning("Checking Tile Validity..");
                if (!BoardController.instance.CheckAITileValidity(x, y))
                {
                    isValid = false;
					if(Criteria.startDirection == 'S') //if move is of type surround
					{
						if (currentHand.Count == 3) {
                            //Debug.LogWarning("Attempting Correction");
							int otherTileVal  = FindUnusedTileValue(Move);
							CorrectSurround(x,y, otherTileVal, Move, Criteria);
						}

					} 
                }



            }
        }

        if (isValid)
        {

            if (Move.Criteria != null)
            {
                AI_Move NewMove = new AI_Move(Move.total, Move.tileCount, Move.tile1, Move.tile2, Move.tile3, Move.isEven);
                NewMove.Criteria = Criteria;
                NewMove.moveValue = Criteria.total + Move.total;

                if (!isEven(Criteria.total + Move.total))
                {
                    validMove.Add(NewMove);
                }

            }
            else
            {
                Move.Criteria = Criteria;
                Move.moveValue = Criteria.total + Move.total;
                

                if (isEven(Criteria.total + Move.total))
                {

                    Debug.Log("Move with even total is valid?");

                } else
                {
                    validMove.Add(Move);

                }
            }

        }
        
        //Return board to original state
        foreach (int x in Criteria.xSpaces)
        {
            BoardController.instance.gameGrid[x, Criteria.rowcolNo] = 0;
        }

    }


    private void CorrectSurround(int x, int y, int tileVal, AI_Move Move, AI_MatchCriteria Criteria)
    {
        bool valid1 = false;
        int addedValue = 0;
        for (int iy = y; iy < BoardController.instance.GRID_SIZE; iy++)
        {
            if (gameGrid[x, iy] == 0)
            {
                //Debug.Log("Attempted correction: Place " + tileVal + " at x:" + x + " y:" + iy);
				gameGrid[x, iy] = tileVal;
				valid1= BoardController.instance.CheckAITileValidity(x,iy);

                if(valid1)
                {
                    foreach(int space in Criteria.xSpaces)
                    {
                        valid1= BoardController.instance.CheckAITileValidity(space,y);

                        if(valid1 == false)
                        {
                            gameGrid[x,iy] = 0;
                            break;
                        } else {
							gameGrid[x,iy] = 0;
                        }
                    }
                }                
                
                if(valid1)
                {

                    AI_Move NewMove = new AI_Move(Move.total + (addedValue+tileVal), 2,
                                         Move.tile1, Move.tile2, Move.tile3, Move.isEven); 

                    AI_Move_Correction correction = new AI_Move_Correction(x,iy,tileVal,'D');

                    NewMove.Criteria = Criteria; 

                    NewMove.Correction = correction;

					if(NewMove.total > 0)
                    {
                        Debug.Log("Adding value more than 0 move..");
                    	validMove.Add(NewMove);                 
                    } else {
                        Debug.Log("Denied..");
                    }

                    //reset board 
                    gameGrid[x,iy] = 0;
                } else {
                    
                    gameGrid[x,iy] = 0;
                    break;

                }
            } else {
                addedValue+= gameGrid[x,iy];
            }
        }

        if(!valid1)
        {
            bool valid2 = false;
            int addedValue2 = 0;

            //traverse downwards from xy board position, looking for zero
            for (int iy = y; iy > 0; iy--)
            {
                if (gameGrid[x, iy] == 0)
                {
                    gameGrid[x,iy] = tileVal;
                    valid2= BoardController.instance.CheckAITileValidity(x,iy);

                    if(valid2)
                    {
                        foreach(int space in Criteria.xSpaces)
                        {
                            valid2= BoardController.instance.CheckAITileValidity(space,y);

                            if(valid2 == false)
                            {
                                gameGrid[x,iy] = 0;
                                //Debug.Log("Correction Failed");
                                break;
                            }
                        }
                    }

                    if(valid2)
                    {

                        AI_Move NewMove = new AI_Move(Move.total + (addedValue2+tileVal), 2,
                                            Move.tile1, Move.tile2, Move.tile3, Move.isEven); 

                        AI_Move_Correction correction = new AI_Move_Correction(x,iy,tileVal,'U'); 

                        NewMove.Criteria = Criteria; 

                        NewMove.Correction = correction; 

                        //ValidMove.Add(NewMove);  
                        if(NewMove.total > 0)
                        {
                            Debug.Log("Adding value more than 0 move..");
                            validMove.Add(NewMove);                 
                        } else {
                            Debug.Log("Denied..");
                        }

                        //reset board 
                        gameGrid[x,iy] = 0;
                    } else {

						//reset board 
						gameGrid[x,iy] = 0;
                        break;

                    }
                } else {
                    addedValue2+= gameGrid[x,iy];
                }
            }
        }

    }

    private void CorrectSurroundVertical(int x, int y, int tileVal, AI_Move Move, AI_MatchCriteria Criteria)
    {
        bool valid1 = false;
        int addedValue = 0;
        for (int ix = x; ix < BoardController.instance.GRID_SIZE; ix++)
        {
            if (gameGrid[ix, y] == 0)
            {
                //Debug.Log("Attempted correction: Place " + tileVal + " at x:" + x + " y:" + iy);
				gameGrid[ix, y] = tileVal;
				valid1= BoardController.instance.CheckAITileValidity(ix,y);

                if(valid1)
                {
                    foreach(int space in Criteria.xSpaces)
                    {
                        valid1= BoardController.instance.CheckAITileValidity(x,space);

                        if(valid1 == false)
                        {
                            gameGrid[ix, y] = 0;
                            break;
                        } else {
							gameGrid[ix, y] = 0;
                        }
                    }
                }                
                
                if(valid1)
                {

                    AI_Move NewMove = new AI_Move(Move.total + (addedValue+tileVal), 2,
                                         Move.tile1, Move.tile2, Move.tile3, Move.isEven); 

                    AI_Move_Correction correction = new AI_Move_Correction(ix,y,tileVal,'R');

                    NewMove.Criteria = Criteria; 

                    NewMove.Correction = correction;

					if(NewMove.total > 0)
                    {
                        Debug.Log("Adding value more than 0 move..");
                    	validMove.Add(NewMove);                 
                    } else {
                        Debug.Log("Denied..");
                    }

                    //reset board 
                    gameGrid[ix, y] = 0;
                } else {
                    
                    gameGrid[ix, y] = 0;
                    break;

                }
            } else {
                addedValue+= gameGrid[ix, y];
            }
        }

        if(!valid1)
        {
            bool valid2 = false;
            int addedValue2 = 0;

            //traverse downwards from xy, looking for zero
            for (int ix = x; ix > 0; ix--)
            {
                if (gameGrid[ix, y] == 0)
                {
                    gameGrid[ix, y] = tileVal;
                    valid2= BoardController.instance.CheckAITileValidity(ix,y);

                    if(valid2)
                    {
                        foreach(int space in Criteria.xSpaces)
                        {
                            valid2= BoardController.instance.CheckAITileValidity(x,space);

                            if(valid2 == false)
                            {
                                gameGrid[ix, y] = 0;
                                //Debug.Log("Correction Failed");
                                break;
                            }
                        }
                    }

                    if(valid2)
                    {

                        AI_Move NewMove = new AI_Move(Move.total + (addedValue2+tileVal), 2,
                                            Move.tile1, Move.tile2, Move.tile3, Move.isEven); 

                        AI_Move_Correction correction = new AI_Move_Correction(ix,y,tileVal,'L'); 

                        NewMove.Criteria = Criteria; 

                        NewMove.Correction = correction; 

                        //ValidMove.Add(NewMove);  
                        if(NewMove.total > 0)
                        {
                            Debug.Log("Adding value more than 0 move..");
                            validMove.Add(NewMove);                 
                        } else {
                            Debug.Log("Denied..");
                        }

                        //reset board 
                        gameGrid[ix, y] = 0;
                    } else {

						//reset board 
						gameGrid[ix, y] = 0;
                        break;

                    }
                } else {
                    addedValue2+= gameGrid[ix, y];
                }
            }
        }

    }

    //Method which finds the unused tile value in any given move, 0 being the default for no unused tile
    private int FindUnusedTileValue(AI_Move Move)
    {
        // navigate down from invalid tile position until 0 found
        if(Move.tile1 == 0)
			if(currentHand[0] != null) { return currentHand [0].value;}

        if(Move.tile2 == 0)
			if(currentHand[1] != null) { return currentHand [1].value;}

        if(Move.tile3 == 0)
			if(currentHand[2] != null) { return currentHand [2].value;}

        Debug.Log("Returning 0 as unused tile value..");
        return 0;
    }

    void TestMoveVertical(AI_Move Move, AI_MatchCriteria Criteria)
    {
        //try and place tiles
        int x = Criteria.rowcolNo;
        
        int[] tempTileValues = new int[Move.tileValues.Count];
        for(int i=0; i<Move.tileValues.Count;i++)
        {
            tempTileValues[i]=Move.tileValues[i];
        }

        int tileCount = 0;
        int tilesPlayed = 0;
        int currentTileVal = tempTileValues[tileCount];
        foreach (int y in Criteria.xSpaces)
        {

            //if tilesPlayed >= tiles in move then BREAK
            if (tilesPlayed >= Move.tileCount)
            {
                //Debug.LogWarning("Tiles played break;");
                break;
            }

            //loop until non 0 val
            while (currentTileVal == 0)
            {
                tileCount++;
                if(tileCount <= Move.tileValues.Count-1)
                {
                    currentTileVal = tempTileValues[tileCount];
                } else {
                    //Debug.LogWarning("Safety guard in place"); //TEMP?
                    break;
                }
            }

            BoardController.instance.gameGrid[x, y] = currentTileVal;
            tempTileValues[tileCount]=0;
            currentTileVal=0;
            tilesPlayed++;
        }

        //All tiles played....
        // CHECK TILE VALIDITY
        bool isValid = true;
        foreach (int y in Criteria.xSpaces)
        {

            if (BoardController.instance.gameGrid[x, y] != 0)
            {
                if (!BoardController.instance.CheckAITileValidity(x, y))
                {
                    isValid = false;
                    if(Criteria.startDirection == 'X') //if move is of type surround
					{
						if (currentHand.Count == 3) {
							int otherTileVal  = FindUnusedTileValue(Move);
							CorrectSurroundVertical(x,y, otherTileVal, Move, Criteria);
						}

					} 
                }
               
            }

        }

        if (isValid)
        {

            if (Move.Criteria != null)
            {
                AI_Move NewMove = new AI_Move(Move.total, Move.tileCount, Move.tile1, Move.tile2, Move.tile3, Move.isEven);
                NewMove.Criteria = Criteria;
                NewMove.moveValue = Criteria.total + Move.total;


                if(isEven(NewMove.moveValue))
                {
                    if(NewMove.moveValue != 0)
                    {
                        Debug.Log("Move with 0 value..");
                    } else {
                        Debug.Log("Move with even move value..");

                        }
                } else
                {
                    validMove.Add(NewMove);
                }


            }
            else
            {
                Move.Criteria = Criteria;
                Move.moveValue = Criteria.total + Move.total;

                if(isEven(Move.moveValue))
                {
                    if(Move.moveValue != 0)
                    {
                        Debug.Log("Move with 0 value..");
                    } else {
                        Debug.Log("Move with even move value..");

                        }
                } else
                {
                    validMove.Add(Move);

                }

            }

        }

        //Return board to original state
        foreach (int y in Criteria.xSpaces)
        {
            BoardController.instance.gameGrid[Criteria.rowcolNo, y] = 0;
        }

    }

	bool isEven(int tot)
    {
        return tot % 2 == 0;
    }

	int CalculateListTotal(List<int> xList)
    {

        int rowTot = 0;
        foreach (int i in xList)
        {
            rowTot = rowTot + i;
        }

        return rowTot;

    }


}
