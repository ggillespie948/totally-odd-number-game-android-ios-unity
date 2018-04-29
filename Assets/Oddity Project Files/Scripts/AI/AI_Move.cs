using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Move {
    
    // Combined total of the tiles
    public int total;
    // Number of tiles in move
    public int tileCount;
    // Tile objects, set to 0 by default
    public int tile1;
    public int tile2;
    public int tile3;
    public List<int> tileValues = new List<int>();
    //Odd/Even bool
    public bool isEven;
    // Variables to actually make the move
    public int moveValue;
    public AI_MatchCriteria Criteria;
    public AI_Move_Correction Correction;
       

    public AI_Move(int _Total, int _Tiles, int _Tile1, int _Tile2, int _Tile3, bool _isEven)
    {
        total = _Total;
        tileCount = _Tiles;

        tile1 = _Tile1;
        tile2 = _Tile2;
        tile3 = _Tile3;

        if(_Tile1 != 0) {tileValues.Add(_Tile1);}
        if(_Tile2 != 0) {tileValues.Add(_Tile2);}
        if(_Tile3 != 0) {tileValues.Add(_Tile3);}
        
        // tileValues.Add(_Tile2);
        // tileValues.Add(_Tile3);
        isEven = _isEven;

    }
    
}
