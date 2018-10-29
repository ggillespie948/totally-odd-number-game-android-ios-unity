using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IntroPanelController : MonoBehaviour {

	
	public GameObject tutorialController;
	
	
	// Use this for initialization
	[SerializeField]
    private GameObject introPanel;
    
	[SerializeField]
    private TextMeshProUGUI targetText1;

	[SerializeField]
    private TextMeshProUGUI targetText2;

	[SerializeField]
    private TextMeshProUGUI targetText3;


	//[SerializeField]
	//public List<int> playerTileSkins = new List<int>();

	[SerializeField]
    private TextMeshProUGUI vsText;

	[SerializeField]
	private List<MeshRenderer> introTiles;

	[SerializeField]
	private GameObject[] playerGameOverSection; // root ->score->name
												//  	->image->border

	[SerializeField]
	public GameObject soloGameOverSection;
	[SerializeField]
	public List<MeshRenderer> soloIntroTiles;
	[SerializeField]
	public GameObject player1MultiSection;

												

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Awake()
	{
		//AccountInfo.Login();
		// AccountInfo.GetPlayerData(AccountInfo.playfabId);
		introPanel.SetActive(true);
		StartCoroutine(InitIntroPlayerSections());
		
	}


	public IEnumerator InitIntroPlayerSections()
	{
		int playercount=ApplicationModel.HUMAN_PLAYERS+ApplicationModel.AI_PLAYERS;

		int humanCount =0;


		for(int i=0; i<(ApplicationModel.HUMAN_PLAYERS+ApplicationModel.AI_PLAYERS); i++)
		{
			playerGameOverSection[i].SetActive(true);
			if(i==0)
			{
				if(ApplicationModel.SOLO_PLAY)
				{
					player1MultiSection.SetActive(false);
					soloGameOverSection.SetActive(true);
					soloGameOverSection.GetComponentInChildren<TextMeshProUGUI>().enabled=false;
					humanCount++;
					if(AccountInfo.Instance!=null)
					{
						if(AccountInfo.Instance.Info.PlayerProfile.DisplayName!=null && AccountInfo.Instance.Info.PlayerProfile.DisplayName!="")
						{
							soloGameOverSection.GetComponentInChildren<TextMeshProUGUI>().transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text=AccountInfo.Instance.Info.PlayerProfile.DisplayName;
						} else 
						{
							soloGameOverSection.GetComponentInChildren<TextMeshProUGUI>().transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text="Player";
						}
					}
					for(int u=0; u<3; u++)
					{
						soloIntroTiles[u].material=GameMaster.instance.tileSkins[ApplicationModel.TILESKIN].defaultSkins[u];
					}

				} else 
				{
					soloGameOverSection.SetActive(false);
					playerGameOverSection[i].GetComponentInChildren<TextMeshProUGUI>().enabled=false;
					humanCount++;
					if(AccountInfo.Instance!=null)
					{
						if(AccountInfo.Instance.Info.PlayerProfile.DisplayName!=null && AccountInfo.Instance.Info.PlayerProfile.DisplayName!="" )
						{
							playerGameOverSection[i].GetComponentInChildren<TextMeshProUGUI>().transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text=AccountInfo.Instance.Info.PlayerProfile.DisplayName;
						} else 
						{
							playerGameOverSection[i].GetComponentInChildren<TextMeshProUGUI>().transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text="Player";
						}
					}
					for(int u=0; u<3; u++)
					{
						introTiles[u].material=GameMaster.instance.tileSkins[ApplicationModel.TILESKIN].defaultSkins[u];
					}
				}

				//playerTileSkins[0]=ApplicationModel.TILESKIN;
			} else
			{
				playerGameOverSection[i].GetComponentInChildren<TextMeshProUGUI>().enabled=false;
				if(ApplicationModel.TUTORIAL_MODE || ApplicationModel.LEVEL_CODE=="B1")
				{
					playerGameOverSection[i].GetComponentInChildren<TextMeshProUGUI>().transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text="Tutorial AI "+i;
				}
				else
				{
					//check human player count, incremnt human counter if counter?human count AI else player eoi
					if(humanCount < ApplicationModel.HUMAN_PLAYERS)
					{
						humanCount++;
						playerGameOverSection[i].GetComponentInChildren<TextMeshProUGUI>().transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text="Player "+(i+1);
					} else 
					{
						playerGameOverSection[i].GetComponentInChildren<TextMeshProUGUI>().transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text="AI "+(i+1);
					}
				}


				if(i==1)
				{
					if(ApplicationModel.TILESKIN != ApplicationModel.OPPONENT_TILESKIN_1)
					{
						for(int u=3; u<6; u++)
						{
							introTiles[u].material=GameMaster.instance.tileSkins[ApplicationModel.OPPONENT_TILESKIN_1].defaultSkins[u-3];
						}
						//playerTileSkins[1]=ApplicationModel.OPPONENT_TILESKIN_1;
					} else 
					{
						for(int u=3; u<6; u++)
						{
							introTiles[u].material=GameMaster.instance.tileSkins[ApplicationModel.OPPONENT_TILESKIN_4].defaultSkins[u-3];
						}
						//playerTileSkins[1]=ApplicationModel.OPPONENT_TILESKIN_4;
					}
				} else if(i==2)
				{
					if(ApplicationModel.TILESKIN != ApplicationModel.OPPONENT_TILESKIN_2)
					{
						for(int u=6; u<9; u++)
						{
							introTiles[u].material=GameMaster.instance.tileSkins[ApplicationModel.OPPONENT_TILESKIN_2].defaultSkins[u-6];
						}
						//playerTileSkins[2]=ApplicationModel.OPPONENT_TILESKIN_2;

					} else 
					{
						for(int u=6; u<9; u++)
						{
							introTiles[u].material=GameMaster.instance.tileSkins[ApplicationModel.OPPONENT_TILESKIN_4].defaultSkins[u-6];
						}
						//playerTileSkins[2]=ApplicationModel.OPPONENT_TILESKIN_4;
					}

				}else if (i==3)
				{

					if(ApplicationModel.TILESKIN != ApplicationModel.OPPONENT_TILESKIN_3)
					{
						for(int u=9; u<12; u++)
						{
							introTiles[u].material=GameMaster.instance.tileSkins[ApplicationModel.OPPONENT_TILESKIN_3].defaultSkins[u-9];
						}
						//playerTileSkins[3]=ApplicationModel.OPPONENT_TILESKIN_3;

					} else 
					{
						for(int u=9; u<12; u++)
						{
							introTiles[u].material=GameMaster.instance.tileSkins[ApplicationModel.OPPONENT_TILESKIN_4].defaultSkins[u-9];
						}
						//playerTileSkins[3]=ApplicationModel.OPPONENT_TILESKIN_4;
					}
				}
			}
		}

		for(int i=(ApplicationModel.HUMAN_PLAYERS+ApplicationModel.AI_PLAYERS); i<4; i++ )
		{
			Debug.Log("Deactive player section");
			playerGameOverSection[i].SetActive(false);
		}

		//Assign preview tile materials
		if(AccountInfo.Instance!= null)
		{
			if(AccountInfo.worldStars!= null)
			{
				if(ApplicationModel.LEVEL_CODE=="B2")
				{
					if(AccountInfo.worldStars[ApplicationModel.WORLD_NO,ApplicationModel.LEVEL_NO]!="000")
					{
						ApplicationModel.TUTORIAL_MODE=false;
						GameMaster.instance.TUTORIAL_MODE=false;
						ApplicationModel.RETURN_TO_WORLD=0;
					}


				} else if (ApplicationModel.LEVEL_CODE=="B1")
				{
					if(AccountInfo.worldStars[ApplicationModel.WORLD_NO,ApplicationModel.LEVEL_NO]!="000")
					{
						ApplicationModel.TUTORIAL_MODE=false;
						GameMaster.instance.TUTORIAL_MODE=false;
						ApplicationModel.RETURN_TO_WORLD=0;
					}

				}
			}
		}
		

		if(ApplicationModel.AI_PLAYERS==0 && !ApplicationModel.CUSTOM_GAME)
		{
			vsText.text="Target Mode";
			targetText1.gameObject.SetActive(true);
			targetText1.text="1 Star Target "+ApplicationModel.TARGET.ToString();
			targetText2.gameObject.SetActive(true);
			targetText2.text="2 Star Target "+ApplicationModel.TARGET2.ToString();
			targetText3.gameObject.SetActive(true);
			targetText3.text="3 Star Target "+ApplicationModel.TARGET3.ToString();
		}


		SetResolutionDefault();
		yield return new WaitForSeconds(4.55f);


		IntroFadeInTrigger();
	}

	private void SetResolutionDefault()
	{
		float aspect = Camera.main.aspect;

		if(aspect < 0.47f)
		{
			//Debug.Log("Set Xtra Slim Resolution Default"); //iphone X
			GetComponent<Animator>().SetTrigger("slim");

		} else if (aspect < 0.51f)
		{
			//Debug.Log("Set  Slim Resolution Default"); //s8 s9
			GetComponent<Animator>().SetTrigger("slim");

		} else if (aspect < 0.65f)
		{
			//Debug.Log("Set Default Resolution Default"); //s6 s7
			GetComponent<Animator>().SetTrigger("default");

		} else if (aspect > 0.7f)
		{
			//Debug.Log("Set Wide Resolution Default");
			GetComponent<Animator>().SetTrigger("wide");

		} else 
		{
			//Debug.Log("Set Default Resolution Default");
			GetComponent<Animator>().SetTrigger("default");

		}

	}

	private void IntroFadeInTrigger()
	{
		GetComponent<Animator>().SetTrigger("fadeIn");
	}

	/// <summary>
	/// This function is called at the end of the intro panel "fade in" aniamtion via an animation event
	/// </summary>
	private void HideIntroPanel()
	{
		for(int i=0; i<12; i++)
		{
			introTiles[i].enabled=false;
		}
		GameMaster.instance.gameObject.SetActive(true);
		GameMaster.instance.enabled=true;
		GUI_Controller.instance.gameObject.SetActive(true);



		if (ApplicationModel.LEVEL_CODE == "B2" || ApplicationModel.LEVEL_CODE == "B1")
		{
			Debug.Log("Setting Tutorial Mode Through LEvel_CODE");
			ApplicationModel.TUTORIAL_MODE=true;
			GameMaster.instance.TUTORIAL_MODE=true;
		} else if(ApplicationModel.LEVEL_CODE == "B3")
		{
			ApplicationModel.RETURN_TO_WORLD=-1;
			ApplicationModel.TUTORIAL_MODE=false;
		}
		else 
		{
			ApplicationModel.TUTORIAL_MODE=false;
		}

		if(ApplicationModel.TUTORIAL_MODE)
		{
			Debug.Log("tut mode true");
			if(AccountInfo.Instance!=null)
			{
				Debug.Log("Account info instance not null");
				if(AccountInfo.worldStars[ApplicationModel.WORLD_NO, ApplicationModel.LEVEL_NO]!= null)
				{
					if(AccountInfo.worldStars[ApplicationModel.WORLD_NO, ApplicationModel.LEVEL_NO] == "000")
					{
						Debug.Log("world star 000");
						tutorialController.SetActive(true);
						if (ApplicationModel.LEVEL_CODE == "B2")
						{
							ApplicationModel.RETURN_TO_WORLD=-3;
							ApplicationModel.TUTORIAL_MODE=true;
							GameMaster.instance.TUTORIAL_MODE=true;
							tutorialController.GetComponent<Tutorial_Controller>().tutorialIndex=55;
							tutorialController.GetComponent<Tutorial_Controller>().OnMouseDown();
						}
					} else 
					{
					//tutorial already compelted
					Debug.Log("Tutorial already completed through world data..  tut false");
					ApplicationModel.TUTORIAL_MODE=false;
					GameMaster.instance.TUTORIAL_MODE=false;
					tutorialController.SetActive(false);
					StartCoroutine(GUI_Controller.instance.GridIntroAnim());
					}
				} else 
				{
					GameMaster.instance.TUTORIAL_MODE=true;
					tutorialController.SetActive(true);
				}
				
			} else 
			{
				Debug.LogError("Account info instance null");
				if (ApplicationModel.LEVEL_CODE == "B2")
				{
					ApplicationModel.RETURN_TO_WORLD=-3;
					ApplicationModel.TUTORIAL_MODE=true;
					GameMaster.instance.TUTORIAL_MODE=true;
					tutorialController.GetComponent<Tutorial_Controller>().tutorialIndex=55;
					tutorialController.GetComponent<Tutorial_Controller>().OnMouseDown();
				}
				GameMaster.instance.TUTORIAL_MODE=true;
				tutorialController.SetActive(true);
			}
			
		} else 
		{
			StartCoroutine(GUI_Controller.instance.GridIntroAnim());
		}


		
	}


}
