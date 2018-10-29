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

	public Material redGlow;
	public Material greenGlow;

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{

		if(ApplicationModel.WORLD_NO==0 || ApplicationModel.LEVEL_CODE=="B1" || ApplicationModel.LEVEL_CODE=="B2" ) // OR INVENTORY CONTAINS BOOSTER
		{
			if(GameMaster.instance.invalidTilesInplay && lightEnabled)
			{
				DisableEmission();
				lightEnabled=false;
			} else if(!GameMaster.instance.invalidTilesInplay && !lightEnabled)
			{
				EnableEmission();
				lightEnabled=true;
			}

			
		}


		// IF account info has player booster 1
		// if(GameMaster.instance.invalidTilesInplay && lightEnabled)
		// {
		// 	DisableEmission();
		// 	lightEnabled=false;
		// } else if(!GameMaster.instance.invalidTilesInplay && !lightEnabled){
		// 	EnableEmission();
		// 	lightEnabled=true;
		// }
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

	public void GlowRed()
	{
		rend.material.EnableKeyword("_EMISSION"); 
		rend.material=redGlow;
		rend.material.EnableEmission();
		Color color = Color.red * 0.5f;
		rend.material.SetColor ("_EmissionColor", Color.red);
		if(GameMaster.instance.humanTurn)
			AudioManager.instance.Play("TurnOnLight");
		rend.material.SetEmissionRate(.5f);

	}

	public void GlowGreen()
	{
		//Debug.Log("Glow Green");
		//rend.material.EnableKeyword("_EMISSION"); 
		rend.material=greenGlow;
		rend.material.SetEmissionRate(1f);
		rend.material.EnableEmission();
		// if(GameMaster.instance.humanTurn)
		// 	AudioManager.instance.Play("TurnOnLight");

	}

	public void ResetGreen()
	{
		//Debug.Log("Reset Green");
		rend.material=greenGlow;
		Color color = Color.green * 0.5f;
		DisableEmission();

	}
}
