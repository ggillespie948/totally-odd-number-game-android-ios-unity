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
	private TextMeshProUGUI collectionSubTitle;


	[Header("Navigation Buttons")]
	[SerializeField]
	private Button btn_Game; 
	[SerializeField]
	private Button btn_Tiles;
	[SerializeField]
	private Button btn_Background;
	[SerializeField]
	private GameObject currentTabBG;


	[Header("Unlockables Scroll View Panels")]
	[SerializeField]
	private ScrollRect tileSkinShop;
	[SerializeField]
	private RectTransform tileSkinContent;
	[SerializeField]
	private ScrollRect gridSkinShop; 
	[SerializeField]
	private RectTransform gridSkinShopContent;
	[SerializeField]
	private ScrollRect avatarShop; 


	[SerializeField]
	private GameObject backgroundPannel;
	public int currentShopNo =0;
	[SerializeField]
	private int tileSkinCount=0;
	[SerializeField]
	private int gridSkinCount=1;
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

	public void PreviousShop()
	{
		currentShopNo--;
		if(currentShopNo < 0)
			currentShopNo=2;

		CloseAllScrollRects();
		switch(currentShopNo)
		{
			case 0:
			//load scroll rect shop
			tileSkinShop.gameObject.SetActive(true);

			//Set header text
			shopTitle.text="Tile Skins";

			//set collectionString
			collectionSubTitle.text="Collection: " + tileSkinCount+"/15";

			break;

			case 1:
			//load scroll rect shop
			gridSkinShop.gameObject.SetActive(true);

			//Set header text
			shopTitle.text="Themes";

			//set collectionString
			collectionSubTitle.text="Collection: " + gridSkinCount+"/10";
			break;

			case 2:
			//load scroll rect shop
			//tileSkinShop.gameObject.SetActive(true);

			//Set header text
			shopTitle.text="Player Avatars";

			//set collectionString
			collectionSubTitle.text="Collection: " + avatarCount + "/50";
			break;

		}

	}
	
	public void NextShop()
	{
		currentShopNo++;
		if(currentShopNo > 2)
			currentShopNo=0;

		CloseAllScrollRects();
		switch(currentShopNo)
		{
			case 0:
			//load scroll rect shop
			tileSkinShop.gameObject.SetActive(true);

			//Set header text
			shopTitle.text="Tile Skins";

			//set collectionString
			collectionSubTitle.text="Collection: " + tileSkinCount+"/15";

			break;

			case 1:
			//load scroll rect shop
			gridSkinShop.gameObject.SetActive(true);

			//Set header text
			shopTitle.text="Grid Skins";

			//set collectionString
			collectionSubTitle.text="Collection: " + gridSkinCount+"/10";
			break;

			case 2:
			//load scroll rect shop
			//tileSkinShop.gameObject.SetActive(true);

			//Set header text
			shopTitle.text="Player Avatars";

			//set collectionString
			collectionSubTitle.text="Collection: " + avatarCount + "/50";
			break;

		}

	}

	private void CloseAllScrollRects()
	{
		tileSkinShop.gameObject.SetActive(false);
		gridSkinShop.gameObject.SetActive(false);
		//ava.gameObject.SetActive(false);
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


				if(ApplicationModel.RETURN_TO_WORLD != -1)
					Invoke("CheckForItemUnlock", 2.5f);
			}
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
					ItemUnlockAnimation(tileBlocks[i].GetComponent<RectTransform>(), tileBlocks[i]); //temp - chnage this val to item related info
				}
			}
		}

		//Check for Grid Skin Unlock
		for(int i=0; i<Database.Instance.GridSkinShopItems.Count;i++)
		{
			if(gridBlocks[i].itemData.Prefab != null)
			{
				if(AccountInfo.themeUnlockString[i]=='0' && gridBlocks[i].itemData.StarReq <= AccountInfo.TotalStars())
				{
					ItemUnlockAnimation(gridBlocks[i].GetComponent<RectTransform>(), gridBlocks[i]); //temp - chnage this val to item related info
				}
			}
		}
		
	}

	public void ItemUnlockAnimation(RectTransform item, TileBox tileBox)
	{
		Debug.Log("Unlocking tile skin item..");
		MenuController.instance.NavBar.PressUnlockablesButton();
		tileSkinShop.SnapToPositionVertical(item, tileSkinContent, new Vector3(0,1,0));
		tileBox.RevealAnimation();
	}

	public void ItemUnlockAnimation(RectTransform item, GridBox gridBox)
	{
		Debug.Log("Unlocking grid skin item..");
		CloseAllScrollRects();
		gridSkinShop.gameObject.SetActive(true);
		shopTitle.text="Grid Skins";
		collectionSubTitle.text="Collection: " + gridSkinCount+"/10";
		MenuController.instance.NavBar.PressUnlockablesButton();
		gridSkinShop.SnapToPositionVertical(item, gridSkinShopContent, new Vector3(0,1,0));
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
