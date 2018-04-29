using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class end_turn : MonoBehaviour {

	void OnMouseUp()
    {
        BoardController.instance.testFunc();
    }
}
