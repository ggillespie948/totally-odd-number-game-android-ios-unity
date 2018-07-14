using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CurrenyBarController : MonoBehaviour {

	[SerializeField]
	private TextMeshProUGUI coinText;
	[SerializeField]
	private TextMeshProUGUI liveText;
	
	[SerializeField]
	private GameObject livesEmitter;
	[SerializeField]
	private GameObject coinEmitter;

	[SerializeField]
	private GameObject coinReceiver;
	[SerializeField]
	private GameObject liveReceiver;

	public int playerCoins;
	public int playerLives;


	public IEnumerator IncreasePlayerUIVal(int startVal, int newVal, TextMeshProUGUI text, char type) //type is used to determine currency lives/coins 
    {
        while(startVal < newVal+1)
        {
            //update score ui
			text.text=startVal.ToString();
            if(startVal-5 >= newVal)
            {
                startVal += UnityEngine.Random.Range(1,5);

            } else {
                startVal++;
            }
            yield return new WaitForSeconds(.03f);
        }

		switch(type)
		{
			case 'c':
			playerCoins=newVal;
			GUI_Controller.instance.LivesDialogue.GetComponentInChildren<Image>().color=Color.white;
			break;

			case 'l':
			playerLives=newVal;
			GUI_Controller.instance.CoinDialogue.GetComponentInChildren<Image>().color=Color.white;
			break;

			default:
			break;
		}

    }

	public IEnumerator DecreasePlayerUIVal(int startVal, int newVal, TextMeshProUGUI text, char type) //type is used to determine currency lives/coins 
    {
		

        while(startVal > newVal-1)
        {
            //update score ui
			text.text=startVal.ToString();
            if(startVal-5 >= newVal)
            {
                startVal -= UnityEngine.Random.Range(1,5);

            } else {
                startVal--;
            }

			switch(type)
			{
				case 'c':
            		yield return new WaitForSeconds(.03f);
				break;

				case 'l':
				yield return new WaitForSeconds(.3f);
				break;
			}


        }

		switch(type)
		{
			case 'c':
			playerCoins=newVal;
			GUI_Controller.instance.PlayerCoins_Stone.GetComponent<Image>().color=Color.white;
			break;

			case 'l':
			livesEmitter.GetComponent<ParticleSystem>().EnableEmission(false);
			yield return new WaitForSeconds(1f);
			GUI_Controller.instance.LivesDialogue.GetComponent<Image>().color=Color.white;
			playerLives=newVal;
			yield return new WaitForSeconds(2f);
			break;

			default:
			break;
		}

    }


	//note: these methods only alter the visual appearance of the currency UI and have no effect on player currency data
	public void AddCurrency(int val)
	{
		GUI_Controller.instance.PlayerCoins_Stone.GetComponent<Image>().color=Color.green;
		StartCoroutine(IncreasePlayerUIVal( playerCoins, playerCoins+val, GUI_Controller.instance.CoinDialogue.GetComponentInChildren<TextMeshProUGUI>(), 'c'));
	}

	public void DecreaseCurrency(int val)
	{
		GUI_Controller.instance.PlayerCoins_Stone.GetComponent<Image>().color=Color.red;
		StartCoroutine(DecreasePlayerUIVal( playerCoins, playerCoins-val, GUI_Controller.instance.CoinDialogue.GetComponentInChildren<TextMeshProUGUI>(), 'c'));
	}

	public void AddLives()
	{
		
		//turn ui red
		//start coroutine decrease value
		//enable/disable particle emitter/receiver
		//reset when emission done 

	}

	public void DecreaseLives(int val)
	{
		GUI_Controller.instance.LivesDialogue.GetComponent<Image>().color=Color.red;
		StartCoroutine(DecreasePlayerUIVal( playerLives, playerLives-val, GUI_Controller.instance.LivesDialogue.GetComponentInChildren<TextMeshProUGUI>(), 'l'));
		livesEmitter.GetComponent<ParticleSystem>().EnableEmission(true);

	}



}
