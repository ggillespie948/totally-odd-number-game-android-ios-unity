using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTilesOnTouch : MonoBehaviour {

	void Start()
    {
        Destroy(this.gameObject, 3.5f);
    }
}
