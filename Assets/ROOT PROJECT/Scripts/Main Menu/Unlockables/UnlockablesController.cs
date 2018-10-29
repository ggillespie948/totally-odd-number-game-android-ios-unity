using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;


public class UnlockablesController : MonoBehaviour {

	[Header("Store Items")]
	[SerializeField]
	public List<TileBox> tileBlocks;

	[SerializeField]
	public List<GridBox> gridBlocks;
	

	[Header("Unlockables Header")]
	[SerializeField]
	private TextMeshProUGUI shopTitle; 
	[SerializeField]
	public TextMeshProUGUI collectionSubTitle;
	public TextMeshProUGUI themeCollectionSubTitle;


	[Header("Navigation Buttons")]
	[SerializeField]
	private Button btn_Game; 
	[SerializeField]
	private Button btn_Tiles;
	[SerializeField]
	private Button btn_Background;
	[SerializeField]
	private GameObject currentTabBG;


	[Header("Tile Your Collection Layout Groups")]
	[SerializeField]
	private GridLayoutGroup yourCollectionLayout;
	[SerializeField]
	public GameObject yourCollectionLayoutTile;
	[SerializeField]
	public RectTransform tileSkinContent;
	public GameObject stillToCollectLayoutTile;

	[Header("Theme Your Collection Layout Groups")]
	[SerializeField]
	public GameObject yourCollectionLayoutTheme;
	[SerializeField]
	public RectTransform themeContent;
	public GameObject stillToCollectLayoutTheme;

	[SerializeField]
	public ScrollRect scrollRect;
	


	[SerializeField]
	private GameObject backgroundPannel;
	public int currentShopNo =0;
	[SerializeField]
	public int tileSkinCount=0;
	[SerializeField]
	public int gridSkinCount=1;
	[SerializeField]
	private int avatarCount=0;


	[SerializeField]
	public GameObject yesBtn;
	[SerializeField]
	public GameObject noBtn;


	[SerializeField]
	public Color bluePurchase;
	[SerializeField]
	public Color greenPurchase;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		stillToCollectStartPos=stillToCollectLayoutTile.transform.parent.localPosition;
	}

	public void CorrectCollectionLayout()
	{
		Invoke("correctCollectionEnable",.55f);
		
	}

	private void correctCollectionEnable()
	{
		yourCollectionLayout.enabled=false;
		yourCollectionLayout.enabled=true;
	}

	public void PreviousShop()
	{
		currentShopNo--;
		if(currentShopNo < 0)
			currentShopNo=1;

		CloseAllScrollRects();
		switch(currentShopNo)
		{
			case 0:
			//load scroll rect shop
			yourCollectionLayoutTile.transform.parent.gameObject.SetActive(true);
			stillToCollectLayoutTile.transform.parent.gameObject.SetActive(true);

			//Set header text
			shopTitle.text="Tile Skins";

			//set collectionString
			collectionSubTitle.text="Your Colleciton " + (tileSkinCount+1)+"/15";

			break;

			case 1:
			//load scroll rect shop
			yourCollectionLayoutTheme.transform.parent.gameObject.SetActive(true);
			stillToCollectLayoutTheme.transform.parent.gameObject.SetActive(true);

			//Set header text
			shopTitle.text="Themes";

			//set collectionString
			themeCollectionSubTitle.text="Your Collection: " + (gridSkinCount+1)+"/6";
			break;

		}

	}
	
	public void NextShop()
	{
		currentShopNo++;
		if(currentShopNo > 1)
			currentShopNo=0;

		CloseAllScrollRects();
		switch(currentShopNo)
		{
			case 0:
			//load scroll rect shop
			yourCollectionLayoutTile.transform.parent.gameObject.SetActive(true);
			stillToCollectLayoutTile.transform.parent.gameObject.SetActive(true);

			//Set header text
			shopTitle.text="Tile Skins";

			//set collectionString
			collectionSubTitle.text="Your Colleciton " + (tileSkinCount+1)+"/15";

			break;

			case 1:
			//load scroll rect shop
			yourCollectionLayoutTheme.transform.parent.gameObject.SetActive(true);
			stillToCollectLayoutTheme.transform.parent.gameObject.SetActive(true);

			//Set header text
			shopTitle.text="Themes";

			//set collectionString
			themeCollectionSubTitle.text="Your Collection: " + (gridSkinCount+1)+"/6";
			break;

		}

	}

	private void CloseAllScrollRects()
	{
		Debug.LogWarning("Check performance of this vs binary switch");
		yourCollectionLayoutTheme.transform.parent.gameObject.SetActive(false);
		stillToCollectLayoutTheme.transform.parent.gameObject.SetActive(false);
		yourCollectionLayoutTile.transform.parent.gameObject.SetActive(false);
		stillToCollectLayoutTile.transform.parent.gameObject.SetActive(false);
	}

	public void ColourBg(GameObject Bg)
	{
		if(currentTabBG==null)
		{
			currentTabBG=Bg;
		}
		currentTabBG.GetComponent<Image>().color=Color.white;
		currentTabBG.GetComponentInChildren<Image>().color=Color.white;
		currentTabBG=Bg;
		currentTabBG.GetComponent<Image>().color=Color.green;
		currentTabBG.GetComponentInChildren<Image>().color=Color.green;

	}

	public void LoadTileStore()
	{
		tileSkinCount=0;
		for(int i=0; i<Database.Instance.TileSkinShopItems.Count;i++)
		{
			if(tileBlocks[i].itemData.Prefab == null)
			{
				tileBlocks[i].itemData=Database.Instance.TileSkinShopItems[i];
				tileBlocks[i].item=Database.Instance.CatalogTileSkins[i];
				tileBlocks[i].LoadItem();
				foreach(ItemInstance inst in AccountInfo.Instance.inv_items)
				{
					if(inst.ItemId==tileBlocks[i].itemID)
					{
						tileSkinCount++;
						tileBlocks[i].UnlockItem();
					}
				}


			}
		}

		// if(ApplicationModel.RETURN_TO_WORLD != -1 && ApplicationModel.RETURN_TO_WORLD != -2 && ApplicationModel.RETURN_TO_WORLD != -3) //tutorial level returns
		// 	Invoke("CheckForItemUnlock", 2.5f);

		AdjustTileStoreCollectionOffset();
		//collectionSubTitle.text="Your Colleciton " + (tileSkinCount+1)+"/15";
		
	}

	private Vector3  stillToCollectStartPos;

	/// </summary>
	public void AdjustTileStoreCollectionOffset()
	{
		if(yourCollectionLayoutTile.transform.childCount > 12)
		{
			float yOffset = (yourCollectionLayoutTile.transform.childCount/3)*297;
			stillToCollectLayoutTile.transform.parent.localPosition-=new Vector3(0f,yOffset,0f);

		} else if(yourCollectionLayoutTile.transform.childCount > 9)
		{
			float yOffset = (yourCollectionLayoutTile.transform.childCount/3)*267;
			stillToCollectLayoutTile.transform.parent.localPosition-=new Vector3(0f,yOffset,0f);

		}else if(yourCollectionLayoutTile.transform.childCount > 6)
		{
			float yOffset = (yourCollectionLayoutTile.transform.childCount/3)*247;
			stillToCollectLayoutTile.transform.parent.localPosition-=new Vector3(0f,yOffset,0f);

		} else if (yourCollectionLayoutTile.transform.childCount > 3) 
		{
			float yOffset = (yourCollectionLayoutTile.transform.childCount/3)*157;
			stillToCollectLayoutTile.transform.parent.localPosition-=new Vector3(0f,yOffset,0f);
		} else 
		{
			//3 or less tile skins
		}

		collectionSubTitle.text="Your Colleciton " + (yourCollectionLayoutTile.transform.childCount)+"/15";

	}

	public void IncrementTileCollection()
	{
		if((yourCollectionLayoutTile.transform.childCount+1) >15)
		{

		} else if((yourCollectionLayoutTile.transform.childCount+1) > 9)
		{
			float yOffset = 207;
			stillToCollectLayoutTile.transform.parent.localPosition-=new Vector3(0f,yOffset,0f);

		}else if((yourCollectionLayoutTile.transform.childCount+1) > 6)
		{
			float yOffset = 187;
			stillToCollectLayoutTile.transform.parent.localPosition-=new Vector3(0f,yOffset,0f);

		} else if((yourCollectionLayoutTile.transform.childCount+1) < 3)
		{
			//float yOffset = ;
			//stillToCollectLayoutTile.transform.parent.localPosition-=new Vector3(0f,yOffset,0f);
		}

	}

	public void LoadGridStore()
	{
		gridSkinCount=0;

		for(int i=0; i<Database.Instance.GridSkinShopItems.Count;i++)
		{
			if(gridBlocks[i].itemData.Prefab == null)
			{
				gridBlocks[i].itemData=Database.Instance.GridSkinShopItems[i];
				gridBlocks[i].item=Database.Instance.CatalogGridSkins[i];
				gridBlocks[i].LoadItem();
				foreach(ItemInstance inst in AccountInfo.Instance.inv_items)
				{
					if(inst.ItemId==gridBlocks[i].itemID)
					{
						gridSkinCount++;
						gridBlocks[i].UnlockItem();
					}
				}

			}
		}

		AdjustThemeStoreCollectionOffset();

		
	}

	/// <summary>
	/// this method dynamically repositons then "your collection" and "still to collect
	/// store items based on the number of child elements".true it also updated the colleciton
	/// label
	/// </summary>
	public void AdjustThemeStoreCollectionOffset()
	{
		float yOffset;

		if(yourCollectionLayoutTheme.transform.childCount==1)
		{
			yOffset = (yourCollectionLayoutTheme.transform.childCount)*350;

		} else 
		{
			 yOffset = (yourCollectionLayoutTheme.transform.childCount/2)*350;
		}

		stillToCollectLayoutTheme.transform.parent.localPosition-=new Vector3(0f,yOffset,0f);
		themeCollectionSubTitle.text="Your Colleciton " + (gridSkinCount+1)+"/6";


	}

	public void CheckForItemUnlock()
	{
		//Check for Tile Skin Unlock
		for(int i=0; i<Database.Instance.TileSkinShopItems.Count;i++)
		{
			if(tileBlocks[i].itemData.Prefab != null)
			{
				if(AccountInfo.tileUnlockString[i]=='0' && tileBlocks[i].itemData.StarReq <= AccountInfo.TotalStars())
				{
					Debug.LogError("Unlock tile skin");
					ItemUnlockAnimation(tileBlocks[i].GetComponent<RectTransform>(), tileBlocks[i]); //temp - chnage this val to item related info
				}
			}
		}

		//Check for Grid Skin Unlock
		// for(int i=0; i<Database.Instance.GridSkinShopItems.Count;i++)
		// {
		// 	if(gridBlocks[i].itemData.Prefab != null)
		// 	{
		// 		if(AccountInfo.themeUnlockString[i]=='0' && gridBlocks[i].itemData.StarReq <= AccountInfo.TotalStars())
		// 		{
		// 			Debug.LogError("Unlock grid skin");
		// 			ItemUnlockAnimation(gridBlocks[i].GetComponent<RectTransform>(), gridBlocks[i]); //temp - chnage this val to item related info
		// 		}
		// 	}
		// }

		Debug.LogError("Check for unlocks complete..");


		
		
	}

	public void ItemUnlockAnimation(RectTransform item, TileBox tileBox)
	{
		Debug.Log("Unlocking tile skin item..");

		
		MenuController.instance.NavBar.PressUnlockablesButton();
		//scrollRect.SnapToPositionVertical(item, tileSkinContent, new Vector3(0,5,0));
		MenuController.instance.NavBar.ScrollSnap.GetComponent<ScrollRect>().enabled=false;

		if(ApplicationModel.RETURN_TO_WORLD==-1 && ApplicationModel.TUTORIAL_MODE==false && tileBox.itemData.Index==1)
		{
			//MenuController.instance.NavBar.ScrollSnap.enabled=false;
			MenuController.instance.CurrencyUI.layout.enabled=false;
			ApplicationModel.RETURN_TO_WORLD=-3;
			ApplicationModel.TUTORIAL_MODE=true;
			Debug.Log("start menu tutorial 3");
			Invoke("StartTutorialSequence3", 3.1f);
		}
		
		tileBox.RevealAnimation();

		


		
	}

	public void TESTItemUnlockAnimation()
	{
		Debug.Log("Unlocking tile skin item..");
		MenuController.instance.NavBar.PressUnlockablesButton();
		tileBlocks[10].RevealAnimation();
		//Invoke("test2", .35f);

		if(ApplicationModel.TUTORIAL_MODE && ApplicationModel.RETURN_TO_WORLD==-3)
		{
			Invoke("StartTutorialSequence3", 3.1f);

		}

		
	}

	public void StartTut3AfterDelay()
	{
		MenuController.instance.CurrencyUI.layout.enabled=false;
		ApplicationModel.RETURN_TO_WORLD=-3;
		ApplicationModel.TUTORIAL_MODE=true;
		Debug.Log("start menu tutorial 3 after delay");
			Invoke("StartTutorialSequence3", 2.8f);

	}

	private void StartTutorialSequence3()
	{
		MenuController.instance.tutorialController.gameObject.SetActive(true);
		MenuController.instance.tutorialController.tutorialIndex=52;
		MenuController.instance.tutorialController.OnMouseDown();
		MenuController.instance.NavBar.ScrollSnap.enabled=true;
	}

	public void ItemUnlockAnimation(RectTransform item, GridBox gridBox)
	{
		Debug.Log("Unlocking grid skin item..");
		CloseAllScrollRects();
		scrollRect.gameObject.SetActive(true);
		shopTitle.text="Themes";
		themeCollectionSubTitle.text="Your Collection: " + gridSkinCount+"/10";
		MenuController.instance.NavBar.PressUnlockablesButton();
		//scrollRect.SnapToPositionVertical(item, tileSkinContent, new Vector3(0,-1,0));
		gridBox.RevealAnimation();
	}

	public void LockShopButtons()
	{
		yesBtn.GetComponent<BoxCollider>().enabled=false;
		noBtn.GetComponent<BoxCollider>().enabled=false;
	}

	public void UnlockShopButtons()
	{
		yesBtn.GetComponent<BoxCollider>().enabled=true;
		noBtn.GetComponent<BoxCollider>().enabled=true;
	}




}
