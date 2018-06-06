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
	
	[Header("Navigation Buttons")]
	[SerializeField]
	private Button btn_Game; 
	[SerializeField]
	private Button btn_Tiles;
	[SerializeField]
	private Button btn_Background;

	[SerializeField]
	private GameObject currentTabBG;

	[Header("Unlockables Panels")]
	[SerializeField]
	private GameObject gamePanel; 
	[SerializeField]
	private GameObject tilesPanel;
	[SerializeField]
	private GameObject backgroundPannel;

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

	public void CloseAllTabs()
	{
		gamePanel.SetActive(false);
		tilesPanel.SetActive(false);
		backgroundPannel.SetActive(false);
	}

	public void PressTilesTab()
	{
		CloseAllTabs();
		tilesPanel.SetActive(true);
		

	}

	public void PressBackgroundTab()
	{
		CloseAllTabs();
		backgroundPannel.SetActive(true);

	}

	public void PressGameTab()
	{
		CloseAllTabs();
		gamePanel.SetActive(true);

	}

	public void LoadTileStore()
	{
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
						tileBlocks[i].UnlockItem();
					}
				}
			}
		}
	}




}
