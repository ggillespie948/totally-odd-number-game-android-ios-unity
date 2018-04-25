using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Grid_Data_State", menuName = "Top Odds!/Grid")]
public class Grid_Data : ScriptableObject
{
    private const int defaultGridSize = 9;

    [Range(3, 9)]
    public int gridSize = defaultGridSize;

    public CellRow[] cells = new CellRow[defaultGridSize];

    public int[,] gameGrid = new int[defaultGridSize,defaultGridSize];

    //[SerializeField]
    public int OneTileCount;
    public int TwoTileCount;
    public int ThreeTileCount;
    public int FourTileCount;
    public int FiveTileCount;
    public int SixTileCount;
    public int SevenTileCount;



    public int[,] GetCells()
    {
        int[,] ret = new int[gridSize, gridSize];

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                ret[i, j] = cells[i].row[j];
            }
        }

        return ret;
    }

    /// <summary>
    /// Just an example, you can remove this.
    /// </summary>
    public int GetCountActiveCells()
    {
        int count = 0;
        OneTileCount = 0;
        TwoTileCount = 0;
        ThreeTileCount = 0;
        FourTileCount=0;
        FiveTileCount=0;
        SixTileCount=0;
        SevenTileCount=0;

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (cells[i].row[j]!= 0) count++;

                gameGrid[i,j]=cells[j].row[i];

                switch(cells[i].row[j])
                {
                    case 1:
                    OneTileCount++;
                    break;

                    case 2:
                    TwoTileCount++;
                    break;

                    case 3:
                    ThreeTileCount++;
                    break;

                    case 4:
                    FourTileCount++;
                    break;

                    case 5:
                    FiveTileCount++;
                    break;

                    case 6:
                    SixTileCount++;
                    break;

                    case 7:
                    SevenTileCount++;
                    break;

                } 
            }
        }

        Debug.Log("COUNTS: 1: 2: 3: " + OneTileCount + " " + TwoTileCount + " " +ThreeTileCount);

        return count;
    }


    [System.Serializable]
    public class CellRow
    {
        public int[] row = new int[defaultGridSize];
        
    }
}
