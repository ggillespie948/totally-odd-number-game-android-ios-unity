using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TMPro.Examples;
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
	public int playerStars;

	[SerializeField]
	public GameObject coinInfoPanel;

	[SerializeField]
	public GameObject energyInfoPanel;

	[SerializeField]
	public GameObject starInfoPanel;

	[SerializeField]
	public int secsToFullEnergy=0;

	[SerializeField]
	private Color defaultCol;

	public void showCoinInfoPanel()
	{
		if(coinInfoPanel!= null)
		{
			coinInfoPanel.SetActive(true);
			coinInfoPanel.GetComponentInChildren<TeleType>().autoPlay=true;
			StartCoroutine(coinInfoPanel.GetComponentInChildren<TeleType>().Start());
			StartCoroutine(ResetInformationPanelAfterDelay(coinInfoPanel,6f));
		}

	}

	public void showEnergyInfoPanel()
	{
		if(energyInfoPanel!= null)
		{
			energyInfoPanel.SetActive(true);

			if( AccountInfo.Instance.InventoryContainsItemClass("Life Pass") )
			{
				energyInfoPanel.GetComponentInChildren<TeleType>().label01 = "<font=\"FredokaOne-Regular SDF\" material=\"FredokaOne-Regular SDF RED GLOW\">Energy</font> is used to play games and regenerates over time.                   Unlimited Energy Pass Active.";
				energyInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = "<font=\"FredokaOne-Regular SDF\" material=\"FredokaOne-Regular SDF RED GLOW\">Energy</font> is used to play games and regenerates over time.               Unlimited Energy Pass Active.";

			} else{

				if(playerLives<15)
				{
					energyInfoPanel.GetComponentInChildren<TeleType>().label01 = "<font=\"FredokaOne-Regular SDF\" material=\"FredokaOne-Regular SDF RED GLOW\">Energy</font> is used to play games and regenerates over time.                   Full Energy in <font=\"FredokaOne-Regular SDF\" material=\"FredokaOne-Regular SDF YELLOW GLOW\">"+ ((secsToFullEnergy+((15-playerLives)*192)/60)) + " minutes</font>";
					energyInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = "<font=\"FredokaOne-Regular SDF\" material=\"FredokaOne-Regular SDF RED GLOW\">Energy</font> is used to play games and regenerates over time.                   Full Energy in <font=\"FredokaOne-Regular SDF\" material=\"FredokaOne-Regular SDF YELLOW GLOW\">"+((secsToFullEnergy+((15-playerLives)*192)/60))+ " minutes</font>";		
				} else
				{
					energyInfoPanel.GetComponentInChildren<TeleType>().label01 = "<font=\"FredokaOne-Regular SDF\" material=\"FredokaOne-Regular SDF RED GLOW\">Energy</font> is used to play games and regenerates over time.                                 Energy Full.";
					energyInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = "<font=\"FredokaOne-Regular SDF\" material=\"FredokaOne-Regular SDF RED GLOW\">Energy</font> is used to play games and regenerates over time.                             Energy Full.";
				}
			}

			energyInfoPanel.GetComponentInChildren<TeleType>().autoPlay=true;
			StartCoroutine(energyInfoPanel.GetComponentInChildren<TeleType>().Start());
			StartCoroutine(ResetInformationPanelAfterDelay(energyInfoPanel,7.5f));
		}
		

	}

	public IEnumerator ResetInformationPanelAfterDelay(GameObject panel, float delay)
	{
		yield return new WaitForSeconds(delay);
		panel.SetActive(false);
		
	}

	public void showStarInfoPanel()
	{
		if(starInfoPanel!= null)
		{
			starInfoPanel.SetActive(true);
			starInfoPanel.GetComponentInChildren<TeleType>().autoPlay=true;
			StartCoroutine(starInfoPanel.GetComponentInChildren<TeleType>().Start());
			StartCoroutine(ResetInformationPanelAfterDelay(starInfoPanel,7.5f));
		}

	}


	public IEnumerator IncreasePlayerUIVal(int startVal, int newVal, TextMeshProUGUI text, char type) //type is used to determine currency lives/coins 
    {
		Debug.Log("Increasing player UI VAL " + type);
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
			GUI_Controller.instance.LivesDialogue.GetComponentInChildren<Image>().color=defaultCol;
			break;

			case 'l':
			playerLives=newVal;
			GUI_Controller.instance.CoinDialogue.GetComponentInChildren<Image>().color=defaultCol;
			break;

			case 's':
			Debug.Log("star ding");
			playerStars=newVal;
			GUI_Controller.instance.CoinDialogue.GetComponentInChildren<Image>().color=defaultCol;
			break;

			default:
			break;
		}

    }

	public IEnumerator DecreasePlayerUIVal(int startVal, int newVal, TextMeshProUGUI text, char type) //type is used to determine currency lives/coins 
    {
		if(AccountInfo.Instance.InventoryContainsItemClass("Life Pass") && type =='l')
		{
			yield return new WaitForSeconds(1.5f);
			livesEmitter.GetComponent<ParticleSystem>().EnableEmission(false);
			yield return new WaitForSeconds(.75f);
			MenuController.instance.StartActiveGameConfiguration();
			

		} else 
		{
			while(startVal > newVal-1)
			{
				//update score ui
				text.text=startVal.ToString();
				if(startVal-5 >= newVal)
				{
					startVal -= UnityEngine.Random.Range(5,20);

				} else {
					startVal--;
				}

				switch(type)
				{
					case 'c':
						yield return new WaitForSeconds(.005f);
					break;

					case 'l':
					yield return new WaitForSeconds(.5f);
					break;
				}


			}

			switch(type)
			{
				case 'c':
				playerCoins=newVal;
				GUI_Controller.instance.PlayerCoins_Stone.GetComponent<Image>().color=defaultCol;
				break;

				case 'l':
				livesEmitter.GetComponent<ParticleSystem>().EnableEmission(false);
				yield return new WaitForSeconds(1f);
				GUI_Controller.instance.LivesDialogue.GetComponent<Image>().color=defaultCol;
				playerLives=newVal;
				yield return new WaitForSeconds(2f);
				break;

				default:
				break;
			}

		}
		


    }

	public void AddStar(int val)
	{
		GUI_Controller.instance.StarDialogue.GetComponent<Image>().color=Color.green;
		StartCoroutine(IncreasePlayerUIVal(AccountInfo.TotalStars(), AccountInfo.TotalStars()+val, GUI_Controller.instance.StarDialogue.GetComponentInChildren<TextMeshProUGUI>(), 's'));

	}

	//note: these methods only alter the visual appearance of the currency UI and have no effect on player currency data
	public void AddCurrency(int val)
	{
		GUI_Controller.instance.CoinDialogue.GetComponent<Image>().color=Color.green;
		StartCoroutine(IncreasePlayerUIVal( playerCoins, playerCoins+val, GUI_Controller.instance.CoinDialogue.GetComponentInChildren<TextMeshProUGUI>(), 'c'));
	}

	public void DecreaseCurrency(int val)
	{
		GUI_Controller.instance.CoinDialogue.GetComponent<Image>().color=Color.red;
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
		if(AccountInfo.Instance.InventoryContainsItemClass("Life Pass"))
		{
			GUI_Controller.instance.LivesDialogue.GetComponent<Image>().color=Color.red;
			StartCoroutine(DecreasePlayerUIVal( playerLives, playerLives-val, GUI_Controller.instance.LivesDialogue.GetComponentInChildren<TextMeshProUGUI>(), 'l'));
			livesEmitter.GetComponent<ParticleSystem>().EnableEmission(true);
			
		} else 
		{
			int lifeTotal=0;
			if(AccountInfo.Instance.Info.UserVirtualCurrency.TryGetValue(AccountInfo.LIVES_CODE, out lifeTotal))
			if(lifeTotal >= val)
			{
				GUI_Controller.instance.LivesDialogue.GetComponent<Image>().color=Color.red;
				StartCoroutine(DecreasePlayerUIVal( playerLives, playerLives-val, GUI_Controller.instance.LivesDialogue.GetComponentInChildren<TextMeshProUGUI>(), 'l'));
				livesEmitter.GetComponent<ParticleSystem>().EnableEmission(true);
				AccountInfo.DeductEnergy(val);
			} else 
			{
				Debug.Log("Not enough energy");
				MenuController.instance.EnergyEmpty();
			}
		}
	}

	public ScrollRect shoptemp;
	public RectTransform child;
	public RectTransform contentRect;
	
	public void TestRectD()
	{
		shoptemp.SnapToPositionVertical(child, contentRect, new Vector3(60,1.5f,0));
	}

	public void TestRectU()
	{
		shoptemp.ScrollToTop();
	}

	public void TestRectL()
	{
		shoptemp.ScrollToLeft();
	}

	public void TestRectR()
	{
		shoptemp.ScrollToRight();
	}

	



}
