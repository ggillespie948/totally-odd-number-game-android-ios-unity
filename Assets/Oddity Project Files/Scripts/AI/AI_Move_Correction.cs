using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Move_Correction {

	public int tileX;

	public int tileY;

	public int tileValue;

	public char correctionDir; //direction of the correctiong e.g. L = left, traverse the tileXY left until tileValue can be placed
	//L = Left, R = right, U = Up, D = Down

	public AI_Move_Correction (int _tileX, int _tileY, int _tileValue, char _correctionDir)
	{
		if(_correctionDir == 'U' || _correctionDir == 'D') //Up or down =  a correction to a horizontal paly
		{
			//Debug.Log("New Horizontal Correction Created.");


		} else if (_correctionDir == 'L' || _correctionDir == 'R')  //Left or right =  a correction to a vertical paly
		{
			//Debug.Log("New Vertical Correction Created.");
		}


		tileX = _tileX;
		tileY = _tileY;
		tileValue = _tileValue;
		correctionDir = _correctionDir;
	}
	
}
