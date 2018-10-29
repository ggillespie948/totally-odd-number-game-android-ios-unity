using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeHand : MonoBehaviour {

	void OnMouseDown()
    {
        GameMaster.instance.HandExchange();
    }
}
