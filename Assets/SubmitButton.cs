using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SubmitButton : MonoBehaviour {

	[SerializeField]
	private Renderer rend;
	[SerializeField]
	private TextMeshProUGUI txt;

	public GameObject btnMesh;
	private bool lightEnabled = true;

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if(GameMaster.instance.invalidTilesInplay && lightEnabled)
		{
			DisableEmission();
			lightEnabled=false;
		} else if(!GameMaster.instance.invalidTilesInplay && !lightEnabled){
			EnableEmission();
			lightEnabled=true;
		}
	}

	public void DeactivateAnimation()
	{
		GetComponent<Animator>().Play("ButtonTransition");

	}

	public void ActivateAnimation()
	{
		GetComponent<Animator>().Play("ButtonTransition_IN");

	}

	public void DisableEmission()
	{
		rend.material.DisableEmission();
	}

	public void EnableEmission()
	{
		rend.material.EnableEmission();
		AudioManager.instance.Play("TurnOnLight");

	}
}
