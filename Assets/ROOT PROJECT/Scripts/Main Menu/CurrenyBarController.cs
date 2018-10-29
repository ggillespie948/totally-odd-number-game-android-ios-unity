using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TMPro.Examples;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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


	[SerializeField]
	private RectTransform rect;
	[SerializeField]
	private RectTransform coinPanel;
	[SerializeField]
	private RectTransform lifePanel;
	[SerializeField]
	private RectTransform starPanel;
	[SerializeField]
	public HorizontalLayoutGroup layout;

	[SerializeField]
	private GameObject headerBG;

	[SerializeField]
	private GameObject headerTitle;
	

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		SetResolutionDefault();
	}

	private void SetResolutionDefault()
	{
		Scene scene = SceneManager.GetActiveScene();
		if(scene.name == "Main")
		{
			float aspect = Camera.main.aspect;

			if(aspect < 0.47f)
			{
				//Debug.Log("Set Xtra Slim Resolution Default"); //iphone X
				layout.padding.left=-60;
				layout.spacing=6;

			} else if (aspect < 0.51f)
			{
				//Debug.Log("Set  Slim Resolution Default"); //s8 s9
				// layout.padding.left=-60;
				// layout.spacing=10f;
				layout.padding.left=-63;
				layout.spacing=15f;

			} else if (aspect < 0.65f)
			{
				//Debug.Log("Set Default Resolution Default"); //s6 s7
				layout.padding.left=-65;
				layout.spacing=40;

			} else if (aspect > 0.7f)
			{
				//Debug.Log("Set Wide Resolution Default");
				headerBG.transform.position-=new Vector3(0,.2f,0);

				headerTitle.transform.position-=new Vector3(0,.2f,0);

				float left = -58;
				float right = -264.15f;

				rect.anchorMin = Vector3.zero;
				rect.anchorMax = Vector3.one;
				rect.anchoredPosition = new Vector2((left - right)/2, 0f);
				rect.sizeDelta = new Vector2(-(left + right), 0);
				rect.localPosition = new Vector2(rect.localPosition.x, 345);

				coinPanel.sizeDelta=new Vector2(120,46);
				lifePanel.sizeDelta=new Vector2(120,46);
				starPanel.sizeDelta=new Vector2(120,46);

				layout.padding.left=0;
				layout.spacing=40f;

			} else 
			{
				//Debug.Log("Set Default Resolution Default");
				layout.padding.left=-65;
				layout.spacing=40;
			}

		} else // else TITLE SCREEN
		{
			float aspect = Camera.main.aspect;

		if(aspect < 0.47f)
		{
			//Debug.Log("Set Xtra Slim Resolution Default"); //iphone X
			layout.padding.left=5;
			layout.spacing=-6.27f;

		} else if (aspect < 0.51f)
		{
			//Debug.Log("Set  Slim Resolution Default"); //s8 s9
			// layout.padding.left=-60;
			// layout.spacing=10f;
			layout.padding.left=5;
			layout.spacing=-6.27f;

		} else if (aspect < 0.65f)
		{
			//Debug.Log("Set Default Resolution Default"); //s6 s7
			layout.padding.left=-22;
			layout.spacing=20;

		} else if (aspect > 0.7f)
		{
			//Debug.Log("Set Wide Resolution Default");
			headerBG.transform.position-=new Vector3(0,.2f,0);

			headerTitle.transform.position-=new Vector3(0,.2f,0);

			float left = -58;
			float right = -264.15f;

			rect.anchorMin = Vector3.zero;
			rect.anchorMax = Vector3.one;
			rect.anchoredPosition = new Vector2((left - right)/2, 0f);
			rect.sizeDelta = new Vector2(-(left + right), 0);
			rect.localPosition = new Vector2(rect.localPosition.x, 345);

			coinPanel.sizeDelta=new Vector2(120,46);
			lifePanel.sizeDelta=new Vector2(120,46);
			starPanel.sizeDelta=new Vector2(120,46);

			layout.padding.left=0;
			layout.spacing=20f;

		} else 
		{
			//Debug.Log("Set Default Resolution Default");
			layout.padding.left=-22;
			layout.spacing=20;
			

		}

		}


		
		
	}


	public void CloseAllInformationPanels()
	{
		

		if(coinInfoPanel.activeSelf)
		{
			StartCoroutine(ResetInformationPanelAfterDelay(coinInfoPanel,0));
		}

		if(energyInfoPanel.activeSelf)
		{
			StartCoroutine(ResetInformationPanelAfterDelay(energyInfoPanel,0));
		}

		if(starInfoPanel.activeSelf)
		{
			StartCoroutine(ResetInformationPanelAfterDelay(starInfoPanel,0));
		}

		
		
	}

	public void showCoinInfoPanel()
	{
		CloseAllInformationPanels();
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
		CloseAllInformationPanels();
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
					energyInfoPanel.GetComponentInChildren<TeleType>().label01 = "<font=\"FredokaOne-Regular SDF\" material=\"FredokaOne-Regular SDF RED GLOW\">Energy</font> is used to play games and regenerates over time.                   Full Energy in <font=\"FredokaOne-Regular SDF\" material=\"FredokaOne-Regular SDF YELLOW GLOW\">"+ CalculateMinutesUntilFullEnergy() + " minutes</font>";
					energyInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = "<font=\"FredokaOne-Regular SDF\" material=\"FredokaOne-Regular SDF RED GLOW\">Energy</font> is used to play games and regenerates over time.                   Full Energy in <font=\"FredokaOne-Regular SDF\" material=\"FredokaOne-Regular SDF YELLOW GLOW\">"+CalculateMinutesUntilFullEnergy()+ " minutes</font>";		
					fullEnergyStoreTxt.text  ="Full Energy in "+CalculateMinutesUntilFullEnergy() + " minutes";
				} else
				{
					energyInfoPanel.GetComponentInChildren<TeleType>().label01 = "<font=\"FredokaOne-Regular SDF\" material=\"FredokaOne-Regular SDF RED GLOW\">Energy</font> is used to play games and regenerates over time.                                 Energy Full.";
					energyInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = "<font=\"FredokaOne-Regular SDF\" material=\"FredokaOne-Regular SDF RED GLOW\">Energy</font> is used to play games and regenerates over time.                             Energy Full.";
					fullEnergyStoreTxt.text="Energy Full";
				}
			}

			energyInfoPanel.GetComponentInChildren<TeleType>().autoPlay=true;
			StartCoroutine(energyInfoPanel.GetComponentInChildren<TeleType>().Start());
			StartCoroutine(ResetInformationPanelAfterDelay(energyInfoPanel,7.5f));
			
			
			
		}
		

	}

	public TextMeshProUGUI fullEnergyStoreTxt;
	
	public decimal CalculateMinutesUntilFullEnergy()
	{
		//Debug.Log("Lives: " + playerLives);
		//Debug.Log("secs: " + secsToFullEnergy);

		int livesUntilFull = (15 - (playerLives+1));
		int secsVal = livesUntilFull*12;
		secsVal+=secsToFullEnergy;

		if( AccountInfo.Instance.InventoryContainsItemClass("Life Pass") )
		{
			fullEnergyStoreTxt.text  =" Unlimited Live Pass Active";
		} else 
		{
			if(playerLives<15)
			{
				fullEnergyStoreTxt.text="Full Energy in " + (secsVal/60) + " minutes";

			} else 
			{
				fullEnergyStoreTxt.text="Energy Full";
			}


		}

		return (secsVal/60);
	}

	public IEnumerator ResetInformationPanelAfterDelay(GameObject panel, float delay)
	{
		yield return new WaitForSeconds(delay);
		if(panel.activeSelf)
			panel.GetComponent<Animator>().SetTrigger("Close");
		yield return new WaitForSeconds(5f);
		panel.SetActive(false);
		
		
	}


	private IEnumerator showStarInfoAfterDelay()
	{
		yield return new WaitForSeconds(5f);
		showStarInfoPanel();
	}

	private IEnumerator showCoinInfoAfterDelay()
	{
		yield return new WaitForSeconds(5f);
		showCoinInfoPanel();
	}

	private IEnumerator showEnergyInfoAfterDelay()
	{
		yield return new WaitForSeconds(.87f);
		showEnergyInfoPanel();

	}

	public void showStarInfoPanel()
	{
		CloseAllInformationPanels();
		if(starInfoPanel!= null)
		{
			starInfoPanel.SetActive(true);
			starInfoPanel.GetComponentInChildren<TeleType>().autoPlay=true;
			StartCoroutine(starInfoPanel.GetComponentInChildren<TeleType>().Start());
			StartCoroutine(ResetInformationPanelAfterDelay(starInfoPanel,7.5f));
		}

		

		

	}

	public void ResetCoins()
	{
		StopAllCoroutines();
		GUI_Controller.instance.CoinDialogue.transform.SetParent(this.transform);
		StartCoroutine(ResetInformationPanelAfterDelay(starInfoPanel,.87f));
	}



	public IEnumerator IncreasePlayerUIVal(int startVal, int newVal, TextMeshProUGUI text, char type) //type is used to determine currency lives/coins 
    {
		Debug.Log("Increasing player UI VAL " + type);
        while(startVal < newVal+1)
        {
            

			switch(type)
			{
				case 'c':
				//update score ui
				text.text=startVal.ToString();
				if(startVal-150 >= newVal)
				{
					startVal += UnityEngine.Random.Range(50,150);

				} else {
					startVal+=10;
				}
				break;

				case 'l':
				//update score ui
				text.text=startVal.ToString();
				if(startVal-15 >= newVal)
				{
					startVal += UnityEngine.Random.Range(5,15);

				} else {
					startVal++;
				}
				break;

				case 's':
				//update score ui
				text.text=startVal.ToString();
				if(startVal-15 >= newVal)
				{
					startVal += UnityEngine.Random.Range(5,15);

				} else {
					startVal++;
				}
				break;

				default:
				break;
			}
            yield return new WaitForSeconds(.01f);
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
					if(startVal==newVal-1)
						livesEmitter.GetComponent<ParticleSystem>().EnableEmission(false);
						
					yield return new WaitForSeconds(.5f);
					break;
				}


			}

			switch(type)
			{
				case 'c':
				playerCoins=newVal;
				GUI_Controller.instance.PlayerCoins_Stone.GetComponent<Image>().color=defaultCol;
				MenuController.instance.coinEmitter.EnableEmission(false);
				AudioManager.instance.Stop("coinLoop");
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

	public void AddLives(int val)
	{
		GUI_Controller.instance.LivesDialogue.GetComponent<Image>().color=Color.red;
		StartCoroutine(IncreasePlayerUIVal( playerLives, playerLives+val, GUI_Controller.instance.CoinDialogue.GetComponentInChildren<TextMeshProUGUI>(), 'l')); 
	}

	public void DecreaseLives(int val)
	{

		if(ApplicationModel.TUTORIAL_MODE && ApplicationModel.RETURN_TO_WORLD==-2)
		{
			Scene scene = SceneManager.GetActiveScene();
			if(scene.name != "Main")
			{
				Debug.LogWarning("Unhighlioght play btn called");
				Tutorial_Controller.instance.UnhighlightPlayBtn();
			}
		}

		if(AccountInfo.Instance.InventoryContainsItemClass("Life Pass"))
		{
			Scene scene = SceneManager.GetActiveScene();
			if(scene.name != "Main")
			{
				GUI_Controller.instance.LivesDialogue.GetComponent<Image>().color=Color.red;
				StartCoroutine(DecreasePlayerUIVal( playerLives, playerLives-val, GUI_Controller.instance.LivesDialogue.GetComponentInChildren<TextMeshProUGUI>(), 'l'));
				livesEmitter.GetComponent<ParticleSystem>().EnableEmission(true);
			} else 
			{
				GameMaster.instance.RetryGame();
			}
			
		} else 
		{
			int lifeTotal=0;
			if(AccountInfo.Instance.Info.UserVirtualCurrency.TryGetValue(AccountInfo.LIVES_CODE, out lifeTotal))
			if(lifeTotal >= val)
			{
				GUI_Controller.instance.LivesDialogue.GetComponent<Image>().color=Color.red;
				StartCoroutine(DecreasePlayerUIVal( playerLives, playerLives-val, GUI_Controller.instance.LivesDialogue.GetComponentInChildren<TextMeshProUGUI>(), 'l'));
				livesEmitter.GetComponent<ParticleSystem>().EnableEmission(true);
				StartCoroutine(LifeDeductionSoundEffect(val, .5f));
				AccountInfo.DeductEnergy(val);
			} else 
			{
				Scene scene = SceneManager.GetActiveScene();
				if(scene.name == "Main")
				{
					Debug.Log("Not enough energy");
					


				} else 
				{
					Debug.Log("Not enough energy");
					MenuController.instance.EnergyEmpty();
				}
			}
		}
	}

	private IEnumerator LifeDeductionSoundEffect(int lives, float delay)
	{
		WaitForSeconds delaybreak = new WaitForSeconds(delay);
		yield return delaybreak;
		AudioManager.instance.Play("pop");

		for(int i=0; i<lives; i++)
		{
			AudioManager.instance.Play("pop");
			yield return delaybreak;
		}

		yield return null;
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
