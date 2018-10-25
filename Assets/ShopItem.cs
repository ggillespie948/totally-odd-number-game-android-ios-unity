using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class ShopItem : MonoBehaviour {

	public Image itemIcon;
	public TextMeshProUGUI itemName;
	public TextMeshProUGUI itemCost;
	public string description;
	public string itemID;
	public bool unlocked=false;
	public Vector3 startPos = new Vector3();
	public ShopItemIAP itemData;
	[SerializeField]
	public CatalogItem item;
	bool inspected=false;
	
	[SerializeField]
	public GameObject purchaseBG;

	[SerializeField]
	public static decimal GBP {get; private set;}

	[SerializeField]
	public static decimal USD {get; private set;}

	[SerializeField]
	public static decimal EUR {get; private set;}

	public void InspectItem()
	{
		if(inspected)
			return;

		inspected=true;
		
		startPos=transform.position;
		GetComponent<GUI_Object>().targetPos= GUI_Controller.instance.confirmPurchasePanelTarget.transform.position;

		MenuController.instance.NavBar.inspectedItemParent=this.gameObject.transform.parent.gameObject;
		this.gameObject.transform.SetParent(GUI_Controller.instance.confirmPurchasePanel.transform);
		GUI_Controller.instance.confirmPurchasePanel.SetActive(true);

		StartCoroutine(GetComponent<GUI_Object>().AnimateTo(GUI_Controller.instance.confirmPurchasePanelTarget.transform.position, .35f));

		MenuController.instance.NavBar.activeShopItem=this.gameObject;
		MenuController.instance.NavBar.gameObject.SetActive(false);

		MenuController.instance.NavBar.shopItemTitle.text=itemData.Name;
		MenuController.instance.NavBar.shopItemDescription.text=item.Description;

		MenuController.instance.navHighlight.SetActive(false);

		if(AccountInfo.Instance.InventoryContains(item))
		{
			MenuController.instance.NavBar.priceText.alignment=TMPro.TextAlignmentOptions.Center;
			MenuController.instance.NavBar.priceText.text="You have already purchased this item.";
		} else 
		{
			MenuController.instance.NavBar.priceText.alignment=TMPro.TextAlignmentOptions.Left;

			if(item.ItemClass == AccountInfo.ITEM_LIFEPASS || item.ItemClass == AccountInfo.ITEM_COINPACK || item.ItemClass ==  AccountInfo.ITEM_LEVELPACK)
			{
				MenuController.instance.NavBar.priceText.alignment=TMPro.TextAlignmentOptions.Center;
				MenuController.instance.NavBar.priceText.text= "Do you wish to purchase this " + item.ItemClass + " for £" + itemData.GBP.ToString("#.##")+"?";
				MenuController.instance.NavBar.coinIcon.enabled=false;
			} else{
				MenuController.instance.NavBar.priceText.alignment=TMPro.TextAlignmentOptions.Center;
				MenuController.instance.NavBar.priceText.text= "Do you wish to purchase this " + item.ItemClass + " for " + itemData.Cost + " coins?";
				MenuController.instance.NavBar.coinIcon.enabled=false;
			}
		}

		foreach(GameObject obj in MenuController.instance.NavBar.previewItems)
		{
			obj.SetActive(false);
		}

		//activate coin BG / Life BG / Level Pack BG
		if(purchaseBG!=null)
			purchaseBG.SetActive(true);

	}

	public void UninspectItem()	{
		inspected=false;
		GetComponent<GUI_Object>().targetPos= startPos;
		MenuController.instance.NavBar.activeShopItem=null;
		
		//StartCoroutine(GetComponent<GUI_Object>().ScaleUp(1.39f,1f));
		StartCoroutine(GetComponent<GUI_Object>().AnimateTo(startPos, .35f));

		MenuController.instance.NavBar.gameObject.SetActive(true);

		this.gameObject.transform.SetParent(MenuController.instance.NavBar.inspectedItemParent.transform);
		GUI_Controller.instance.confirmPurchasePanel.GetComponent<Animator>().SetTrigger("ClosePanel");

		MenuController.instance.NavBar.unlockablesPanel.GetComponent<UnlockablesController>().UnlockShopButtons();
		MenuController.instance.navHighlight.SetActive(true);
		Invoke("CloseInspector", 1.5f);

		if(purchaseBG!=null)
			purchaseBG.SetActive(false);
	}

	public void QuickUninspect()
	{
		inspected=false;
		GetComponent<GUI_Object>().targetPos= startPos;
		MenuController.instance.NavBar.activeShopItem=null;
		transform.position=startPos;
		MenuController.instance.NavBar.gameObject.SetActive(true);
		this.gameObject.transform.SetParent(MenuController.instance.NavBar.inspectedItemParent.transform);
		GUI_Controller.instance.confirmPurchasePanel.GetComponent<Animator>().SetTrigger("ClosePanel");
		MenuController.instance.NavBar.unlockablesPanel.GetComponent<UnlockablesController>().UnlockShopButtons();
		MenuController.instance.navHighlight.SetActive(true);
		CloseInspector();

		if(purchaseBG!=null)
			purchaseBG.SetActive(false);

	}


	public void CloseInspector()
	{
		foreach(GameObject obj in MenuController.instance.NavBar.previewItems)
		{
			obj.SetActive(true);
		}
		GUI_Controller.instance.confirmPurchasePanel.SetActive(false);
	}

	public void LoadItem()
	{
		//itemIcon.sprite = itemData.Icon.sprite;
		itemName.text = itemData.Name;
		itemID = itemData.ItemID;
		itemCost.text=itemData.Cost.ToString();
		startPos = transform.position;
		description=itemData.Description;
		this.gameObject.SetActive(true);


		if(item.ItemClass == AccountInfo.ITEM_LIFEPASS || item.ItemClass == AccountInfo.ITEM_COINPACK || item.ItemClass == AccountInfo.ITEM_LEVELPACK)
		{
			ShopItem.GBP=(decimal)itemData.Cost/100;
			itemData.GBP=(decimal)itemData.Cost/100;
			itemCost.text="£"+ShopItem.GBP.ToString("#.##");
		}
		//if class==life pass || coin bundle.. cost = <float>cost/100 string that and pace currecny infront of it 

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

		//play purchase animation
		PlayFabClientAPI.PurchaseItem(request, OnBoughtItem, AccountInfo.OnAPIError);
	}

	public void OnBoughtItem(PurchaseItemResult result)
	{
		MenuController.instance.coinEmitter.EnableEmission(true);
		MenuController.instance.coinEmitter.SetEmissionRate(itemData.Cost/100);
		MenuController.instance.CurrencyUI.DecreaseCurrency(itemData.Cost);
		AccountInfo.GetAccountInfo();
		unlocked=true;
		

		Invoke("StopCoinEmission", 3.25f);
		Invoke("StopCoinAnim", 6f);
		Invoke("UninspectItem", 7.25f);
	}

	private int itemCount=0;
	public void StopCoinEmission()
	{
		MenuController.instance.coinEmitter.EnableEmission(false);
		itemCount=0;
	}

	public void StopCoinAnim()
	{
		MenuController.instance.confirmPurchaseBtn.GetComponent<Animator>().SetTrigger("purchaseComplete");
		MenuController.instance.InAppPurchasesController.UnlockShopButtons();
	}
}
