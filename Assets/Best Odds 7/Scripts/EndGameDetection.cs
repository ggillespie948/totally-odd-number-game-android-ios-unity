 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameDetection : MonoBehaviour {

    List<GridCell> RemainingCells = new List<GridCell>();

    List<GridCell> ValidCellEven = new List<GridCell>(); //valid remianing cells which require even tiles
    List<GridCell> ValidCellOdd = new List<GridCell>();  //valid remaining cells 

    //AI_MatchCriteria

    private void CheckForEndGame()
    {
        RemainingCells.Clear();
        ValidCellEven.Clear();
        ValidCellOdd.Clear();

        //Get all empty cells in the board
        foreach(GridCell cell in GameMaster.instance.objGameGrid)
        {
            if(cell.cellTile == null)
            {
                RemainingCells.Add(cell);
            }
        }

        foreach (GridCell cell in RemainingCells)
        {
            if(RowRequiresEvenTile(cell) == ColRequiresEvenTile(cell))
            {
                ValidCellEven.Add(cell);
            }
            
        }




    }

    private bool RowRequiresEvenTile(GridCell cell)
    {
        return false;

    }

    private bool ColRequiresEvenTile(GridCell cell)
    {
        return false;

    }

	
}
