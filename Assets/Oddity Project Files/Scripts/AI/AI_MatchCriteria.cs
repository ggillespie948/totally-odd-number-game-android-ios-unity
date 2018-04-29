using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_MatchCriteria {

    public int total;

    public int spaces;

    public List<int> xSpaces;

    public int rowcolNo; //temp - on horizontal moves this is col no on vertical its the row no

    public bool isEven;

    public int startCell; //this is the number cell which is used when placing the tile

    public char startDirection;


    public AI_MatchCriteria(int _Total, int _Spaces, List<int> _xSpaces, int _rowNo, bool _isEven, char _Direction)
    {
        total = _Total;
        spaces = _Spaces;
        xSpaces = _xSpaces;
        rowcolNo = _rowNo;
        isEven = _isEven;
        startDirection = _Direction;

    }


}
