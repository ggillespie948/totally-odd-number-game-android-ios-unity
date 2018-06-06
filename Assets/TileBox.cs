using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class TileBox : MonoBehaviour {

	public Button purchaseBtn; 
	public Button equipBtn;
	public Button equippedBtn;
	public Button starReqBtn;
	public Image itemIcon;
	public TextMeshProUGUI itemName;
	public string itemID;
	public bool equipped;

	public bool unlocked=false;
	

	public ShopItemTile itemData;

	[SerializeField]
	public CatalogItem item;

	

	public void LoadItem()
	{
		itemIcon.sprite = itemData.Icon.sprite;
		itemName.text = itemData.Name;
		itemID = itemData.ItemID;
		starReqBtn.GetComponentInChildren<TextMeshProUGUI>().text = itemData.StarReq.ToString();
		purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text = itemData.Cost.ToString();
		this.gameObject.SetActive(true);
	}

	public void UnlockItem()
	{
		starReqBtn.gameObject.SetActive(false);
		purchaseBtn.gameObject.SetActive(false);
		unlocked=true;
	}

	public void EquipTileSkin()
	{
		if(unlocked)
		{
			ApplicationModel.TILESKIN=itemData.Index;
			for(int i=0; i<Database.Instance.CatalogTileSkins.Count;i++)
			{
				GUI_Controller.instance.NavBar.unlockablesPannel.GetComponent<UnlockablesController>().tileBlocks[i].GetComponent<Image>().color=Color.white;
				GUI_Controller.instance.NavBar.unlockablesPannel.GetComponent<UnlockablesController>().tileBlocks[i].equipBtn.gameObject.SetActive(true);
			}
			equipBtn.gameObject.SetActive(false);
			GetComponent<Image>().color=Color.green;
		}

	}

	public void PurchaseWithCoins()
	{
		uint price = 0;
		if(!item.VirtualCurrencyPrices.TryGetValue(AccountInfo.COINS_CODE, out price))
		{
			Debug.Log("No Coin Code Found");
		}
		PurchaseItemRequest request = new PurchaseItemRequest()
		{
			ItemId = itemID,
			VirtualCurrency = AccountInfo.COINS_CODE,
			Price = (int)price
		};

		PlayFabClientAPI.PurchaseItem(request, OnBoughtItem, AccountInfo.OnAPIError);
	}

	public void OnBoughtItem(PurchaseItemResult result)
	{
		AccountInfo.GetAccountInfo();

	}
}
