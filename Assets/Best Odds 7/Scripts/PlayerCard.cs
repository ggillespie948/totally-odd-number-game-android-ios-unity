using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class PlayerCard : MonoBehaviour {

	public GUI_Object ActivePlayerCard;
	public TextMeshProUGUI ActiveNameTag;
	public TextMeshProUGUI ActiveCardScore;

	
	public GUI_Object InactivePlayerCard;
	public TextMeshProUGUI InactiveNameTag;
	public TextMeshProUGUI InactiveCardScore;

	[SerializeField]
	public Transform[] ScoreAnimTransforms;


	public bool Active = false;

	public int queuePos;

	public void SetQueuePos(int pos)
	{
		queuePos=pos;

	}

	public void UpdateCard(int score)
	{
		InactiveCardScore.text = score.ToString();
	}

	public void UpdateName(string name)
	{
		ActiveNameTag.text = name;
	}

	public void ActivateCard()
	{
		InactivePlayerCard.gameObject.SetActive(false);
		ActivePlayerCard.gameObject.SetActive(true);
	}


	public void ToggleCard()
	{
		if(Active)
		{
			InactivePlayerCard.transform.position = GUI_Controller.instance.InactiveCardPositions[GameMaster.MAX_TURN_INDICATOR-2].transform.position;
			//queuePos=GameMaster.MAX_TURN_INDICATOR--;
			InactivePlayerCard.transform.rotation = Quaternion.Euler(0,0,0);
			StartCoroutine(InactivePlayerCard.GetComponent<GUI_Object>().ScaleDown());
			InactivePlayerCard.GetComponent<NoGravity>().enabled = false;
			Active = false;
		} else 
		{
			StartCoroutine(InactivePlayerCard.GetComponent<GUI_Object>().AnimateTo(InactivePlayerCard.GetComponent<GUI_Object>().targetPosMarker.transform.position, 1f));
			queuePos = GameMaster.MAX_TURN_INDICATOR-2;
			InactivePlayerCard.transform.rotation = Quaternion.Euler(0,0,0);
			StartCoroutine(InactivePlayerCard.GetComponent<GUI_Object>().ScaleUp());
			InactivePlayerCard.GetComponent<NoGravity>().enabled = true;
			Active = true;
		}
	}

	public void ShuffleCardPosition()
	{
		// InactivePlayerCard.transform.position = GUI_Controller.instance.InactiveCardPositions[GUI_Controller.instance.inactiveCardCount].transform.position;
		// GUI_Controller.instance.inactiveCardCount++;
		// if(GUI_Controller.instance.inactiveCardCount >= GameMaster.MAX_TURN_INDICATOR-1)
		// 		GUI_Controller.instance.inactiveCardCount=1;
		queuePos--;
		if(queuePos <0)
			queuePos=0;
		InactivePlayerCard.transform.position = GUI_Controller.instance.InactiveCardPositions[queuePos].transform.position;



	}

	public void CardScoreAnim(List<GridTile> tiles)
	{
		tiles = tiles.Distinct().ToList();

		
		int spawnIndex = 0;
		float delay = 0f;
		foreach(GridTile tile in tiles)
		{
			StartCoroutine(GUI_Controller.instance.SpawnScorePopup("+"+tile.value.ToString(), Color.yellow, tile.transform, delay));
			spawnIndex++;
			delay += .1f;
			if(spawnIndex > ScoreAnimTransforms.Length-1)
				spawnIndex=0;

		}
		
	}


}
