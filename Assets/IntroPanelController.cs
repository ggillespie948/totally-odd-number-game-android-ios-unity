using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IntroPanelController : MonoBehaviour {

	public TextMeshProUGUI playerNameTxt;
	public Image playerPortrait;
	public Image playerPortraitRing;

	public GameObject AI_Container1;
	public TextMeshProUGUI AINameTxt1;
	public Image AIPortrait1;
	public Image AIPortraitRing1;

	public GameObject AI_Container2;
	public TextMeshProUGUI AINameTxt2;
	public Image AIPortrait2;
	public Image AIPortraitRing2;

	
	public GameObject AI_Container3;
	public TextMeshProUGUI AINameTxt3;
	public Image AIPortrait3;
	public Image AIPortraitRing3;

	public TextMeshProUGUI VsText;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		if(AccountInfo.Instance!= null)
			playerNameTxt.text=AccountInfo.Instance.Info.PlayerProfile.DisplayName;
		playerPortraitRing.color=GameMaster.instance.tileSkins[ApplicationModel.TILESKIN].tileSkinCol;

		//AINameTxt1.text=ApplicationModel.AI_NAME_1;
		AIPortraitRing3.color=GameMaster.instance.tileSkins[ApplicationModel.TILESKIN+1].tileSkinCol;

		//AINameTxt2.text=ApplicationModel.AI_NAME_2;
		AIPortraitRing3.color=GameMaster.instance.tileSkins[ApplicationModel.TILESKIN+2].tileSkinCol;

		//AINameTxt3.text=ApplicationModel.AI_NAME_3;
		//AIPortraitRing3.color=GameMaster.instance.tileSkins[ApplicationModel.TILESKIN+3].tileSkinCol;

		if(ApplicationModel.AI_PLAYERS+1 == 4)
		{
			AI_Container1.GetComponent<Animator>().enabled=true;
			AI_Container2.GetComponent<Animator>().enabled=true;
			AI_Container3.GetComponent<Animator>().enabled=true;

		} else if (ApplicationModel.AI_PLAYERS+1 == 3)
		{
			AI_Container3.SetActive(false);
			AI_Container1.GetComponent<Animator>().enabled=true;
			AI_Container2.GetComponent<Animator>().enabled=true;

		} else if (ApplicationModel.AI_PLAYERS+1 == 2)
		{
			AI_Container2.SetActive(false);
			AI_Container3.SetActive(false);
			AI_Container1.GetComponent<Animator>().enabled=true;
			AINameTxt1.fontSize=36;
			AI_Container1.gameObject.transform.localScale=new Vector3(1,1,1);
			
		} else if (ApplicationModel.AI_PLAYERS+1 == 1)
		{
			VsText.text="Target Mode";
			AI_Container1.SetActive(false);
			AI_Container2.SetActive(false);
			AI_Container3.SetActive(false);
		}

		Invoke("HideIntro", 4f);
	}

	public void HideIntro()
	{
		GetComponent<Animator>().SetTrigger("hide");
		AI_Container1.GetComponent<Animator>().SetTrigger("hide");
		AI_Container2.GetComponent<Animator>().SetTrigger("hide");
		AI_Container3.GetComponent<Animator>().SetTrigger("hide");

		GameMaster.instance.enabled=true;
		GUI_Controller.instance.enabled=true;

	}
}
