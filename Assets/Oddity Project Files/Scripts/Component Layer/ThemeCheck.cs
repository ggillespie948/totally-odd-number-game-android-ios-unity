using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeCheck : MonoBehaviour {

	// Use this for initialization
	public string ThemeFrom;

	void Start () {
		if(GameMaster.instance.themes[ApplicationModel.THEME].name != ThemeFrom)
			Destroy(this.gameObject);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
