using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupResDefault : MonoBehaviour {

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		float aspect = Camera.main.aspect;
		GetComponent<Animator>().SetInteger("width", Screen.width);
		GetComponent<Animator>().SetInteger("height", Screen.height);
		GetComponent<Animator>().SetFloat("aspect", aspect);
		
	}
}
