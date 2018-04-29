using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reload : MonoBehaviour {

	void OnMouseDown()
    {
        GameMaster.instance.ReloadScene();

    }
}
