using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour {

	public Image portraitRing;
	public Image portraitImage;
	public GUI_Object ActivePlayerCard;
	public TextMeshProUGUI ScoreText;
	public TextMeshProUGUI ActiveNameTag;

	public Animator Anim;
	
	public bool Active = false;
	public int queuePos;

	public AudioClip soundFX;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		Anim = this.GetComponent<Animator>();
	}

	public void SetRingCol(int i)
	{
		if(GameMaster.instance.soloPlay)
		return;

		if(i<GameMaster.instance.playerTileSkins.Count)
		portraitRing.color=GameMaster.instance.tileSkins[GameMaster.instance.playerTileSkins[i]].tileSkinCol;
	}

	public void SetQueuePos(int pos)
	{
		queuePos=pos;
	}

	public void UpdateCard(int score)
	{
		ScoreText.text = score.ToString();
	}

	public void UpdateName(string name)
	{
		ActiveNameTag.text = name;
	}

	public void ToggleCard()
	{
		if(Active)
		{
			//Anim.Play("PlayerCard_Deactivate",0);
			PlayAudio();
			//GetComponentInChildren<Image>().color = Color.red;
			Anim.enabled=false;
			transform.rotation = Quaternion.Euler(0,0,0);
			Active = false;
			Invoke("PlayAudio", .3f);
		} else 
		{
			Anim.enabled=true;
			Anim.Play("PlayerCard_Activate",0);
			PlayAudio();
			//GetComponentInChildren<Image>().color = Color.green;
			Active = true;
			Invoke("PlayAudio", .3f);
		}
	}

	public void PlayAudio()
	{
		AudioManager.instance.Play("CardFlip");
	}

	public void SetCol(Color col)
	{
		ActiveNameTag.color=col;
	}



}
