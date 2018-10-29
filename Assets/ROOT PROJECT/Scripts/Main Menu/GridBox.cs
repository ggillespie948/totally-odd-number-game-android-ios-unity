using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class GridBox : MonoBehaviour {

	public GameObject purchaseBtn; 
	public Image itemIcon;
	public TextMeshProUGUI itemName;
	public string itemID;
	public bool unlocked=false;
	public Vector3 startPos = new Vector3();
	public Theme tileSkin;
	public ShopItemGrid itemData;

	[SerializeField]
	public CatalogItem item;
	public GameObject lockedPanel;
	public GameObject lockedText;
	private int starReq;
	bool inspected=false;

	private int startIndex;

	public void InspectItem()
	{
		if(inspected)
			return;

		AudioManager.instance.Click();

		inspected=true;
		if(starReq > AccountInfo.TotalStars())
		{
			Debug.Log("Not enough stars to inspect");
			GUI_Controller.instance.SpawnTextPopupMenu("This item requires "+starReq+" stars to be unlocked", Color.white, this.transform, 24 );
			inspected=false;
			return;
		}
		startPos=transform.position;
		GetComponent<GUI_Object>().targetPos= GUI_Controller.instance.confirmPurchasePanelTarget.transform.position;

		MenuController.instance.NavBar.inspectedItemParent=this.gameObject.transform.parent.gameObject;
		this.gameObject.transform.SetParent(GUI_Controller.instance.confirmPurchasePanel.transform);

		GUI_Controller.instance.confirmPurchasePanel.SetActive(true);
		StartCoroutine(GetComponent<GUI_Object>().AnimateTo(GUI_Controller.instance.confirmPurchasePanelTarget.transform.position, .35f));

		MenuController.instance.NavBar.activeGridBox=this.gameObject;
		MenuController.instance.NavBar.gameObject.SetActive(false);

		MenuController.instance.NavBar.shopItemTitle.text=itemData.Name;
		MenuController.instance.NavBar.shopItemDescription.text=item.Description;

		if(AccountInfo.Instance.InventoryContains(item) || itemData.Index ==0  )
		{
			MenuController.instance.NavBar.priceText.text= "Do you wish to equip this " + item.ItemClass + "?";
			MenuController.instance.NavBar.coinIcon.enabled=false;
		} else 
		{
			MenuController.instance.NavBar.priceText.text= "Purchase this " + item.ItemClass + " for " +itemData.Cost + "         ?";
			MenuController.instance.NavBar.coinIcon.enabled=true;
		}

		MenuController.instance.navHighlight.SetActive(false);
		
		foreach(GameObject obj in MenuController.instance.NavBar.previewItems)
		{
			obj.SetActive(false);
		}

		purchaseBtn.gameObject.SetActive(false); //close button animation
	}

	public void UninspectItem()	{
		inspected=false;
		GetComponent<GUI_Object>().targetPos= startPos;
		MenuController.instance.NavBar.activeGridBox=null;

		MenuController.instance.NavBar.gameObject.SetActive(true);
		MenuController.instance.navHighlight.SetActive(true);

		//check ownership of item and return to corrosponding layout group at 
		//specific sibling index
		if(AccountInfo.Instance.InventoryContains(item) || itemData.Cost ==0 )
		{
			this.gameObject.transform.SetParent(MenuController.instance.UnlockablesController.yourCollectionLayoutTheme.transform);
			transform.SetSiblingIndex(startIndex);
		} else 
		{
			purchaseBtn.gameObject.SetActive(true);
			this.gameObject.transform.SetParent(MenuController.instance.UnlockablesController.stillToCollectLayoutTheme.transform);
			transform.SetSiblingIndex(startIndex);
		}

		StartCoroutine(GetComponent<GUI_Object>().AnimateTo(startPos, .35f));
		MenuController.instance.NavBar.gameObject.SetActive(true);
		MenuController.instance.navHighlight.SetActive(true);
		//purchaseBtn.gameObject.SetActive(true);
		//MenuController.instance.NavBar.inspectedItemParent=this.gameObject.transform.parent.gameObject;
		GUI_Controller.instance.confirmPurchasePanel.GetComponent<Animator>().SetTrigger("ClosePanel");
		Invoke("CloseInspector", 1.5f);
	}
		
	public void QuickUninspect()
	{
		inspected=false;
		MenuController.instance.NavBar.activeGridBox=null;
		transform.position=startPos;
		purchaseBtn.gameObject.SetActive(true);
		MenuController.instance.navHighlight.SetActive(true);
		//check ownership of item and return to corrosponding layout group at 
		//specific sibling index
		if(AccountInfo.Instance.InventoryContains(item) ||  itemData.Cost ==0 )
		{
			this.gameObject.transform.SetParent(MenuController.instance.UnlockablesController.yourCollectionLayoutTheme.transform);
			transform.SetSiblingIndex(startIndex);
		} else 
		{
			purchaseBtn.gameObject.SetActive(true);
			this.gameObject.transform.SetParent(MenuController.instance.UnlockablesController.stillToCollectLayoutTheme.transform);
			transform.SetSiblingIndex(startIndex);
		}

		MenuController.instance.NavBar.unlockablesPanel.GetComponent<UnlockablesController>().UnlockShopButtons();
		CloseInspector();
	}


	public void CloseInspector()
	{
		GUI_Controller.instance.confirmPurchasePanel.SetActive(false);
	}

	public void LoadItem()
	{
		itemIcon.sprite = itemData.Icon.sprite;
		itemName.text = itemData.Name;
		itemID = itemData.ItemID;
		starReq = itemData.StarReq;
		startPos = transform.position;


		if(AccountInfo.Instance.InventoryContains(item) || itemData.Cost ==0 )
		{
			this.gameObject.transform.SetParent(MenuController.instance.UnlockablesController.yourCollectionLayoutTheme.transform);
			purchaseBtn.gameObject.SetActive(false);
			unlocked=true;
		} else 
		{
			purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text = itemData.Cost.ToString();
			purchaseBtn.transform.GetChild(0).GetComponentInChildren<Image>().enabled=true;
		}

		if(AccountInfo.THEME == itemData.Index)
		{
			purchaseBtn.gameObject.SetActive(true);
			purchaseBtn.GetComponent<Image>().color=Color.green;
			purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text=" Active";
			purchaseBtn.transform.GetChild(0).GetComponentInChildren<Image>().enabled=false;
		}

		if(AccountInfo.themeUnlockString!= null)
		{
			if(starReq > AccountInfo.TotalStars())
			{
				lockedPanel.SetActive(true);
				purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text="Locked";
				itemName.text="?????";
				lockedText.GetComponent<TextMeshProUGUI>().text = itemData.StarReq.ToString();
				purchaseBtn.transform.GetChild(0).GetComponentInChildren<Image>().enabled=false;
			} else 
			{
				lockedPanel.SetActive(false);
				lockedText.SetActive(false);


			}
		} else
		{
			Debug.Log("THEME UNLOCK STRING NULL");
		}


		startIndex=transform.GetSiblingIndex();

		this.gameObject.SetActive(true);
	}

	public void UnlockItem()
	{
		lockedPanel.SetActive(false);
		lockedText.SetActive(false);
		unlocked=true;
	}

	public void RevealAnimation()
	{
		StartCoroutine(revealAfterDelay());
	}

	private IEnumerator revealAfterDelay()
	{
		yield return new WaitForSeconds(.355f);
		MenuController.instance.UnlockablesController.scrollRect.SnapToPositionVertical(GetComponent<RectTransform>(), MenuController.instance.UnlockablesController.tileSkinContent, new Vector3(0,-1,0));
		yield return new WaitForSeconds(.85f);
		GetComponent<Animator>().SetTrigger("gridReveal");


		// play sound effect
		AudioManager.instance.Play("Unlock");

		//Update tile unlock string so animation is only played once
		char[] unlockS = AccountInfo.themeUnlockString.ToCharArray();
		unlockS[itemData.Index]='1';
		AccountInfo.themeUnlockString = new string(unlockS);
		AccountInfo.UpdateThemeString();
		
		itemIcon.sprite = itemData.Icon.sprite;
		itemName.text = itemData.Name;
		itemID = itemData.ItemID;
		starReq = itemData.StarReq;
		startPos = transform.position;

		Invoke("LoadItem", 2f);
	}

	public void EquipGridSkin()
	{
		if(unlocked)
		{
			ApplicationModel.THEME=itemData.Index;
			AccountInfo.Instance.UpdateTheme(itemData.Index);
			for(int i=0; i<Database.Instance.CatalogGridSkins.Count;i++)
			{
				MenuController.instance.UnlockablesController.gridBlocks[i].GetComponent<Image>().color=Color.white;
				MenuController.instance.UnlockablesController.gridBlocks[i].purchaseBtn.GetComponent<Image>().color=MenuController.instance.UnlockablesController.bluePurchase;

				MenuController.instance.UnlockablesController.gridBlocks[i].purchaseBtn.SetActive(false);

				if(!AccountInfo.Instance.InventoryContains(MenuController.instance.UnlockablesController.gridBlocks[i].item))
				{
					if( MenuController.instance.UnlockablesController.gridBlocks[i].itemData.Cost !=0)
					MenuController.instance.UnlockablesController.gridBlocks[i].purchaseBtn.SetActive(true);
				}


				if(MenuController.instance.UnlockablesController.gridBlocks[i].unlocked)
				{
					MenuController.instance.UnlockablesController.gridBlocks[i].purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text=" Owned";
				} else 
				{
					//MenuController.instance.UnlockablesController.gridBlocks[i].purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text = itemData.Cost.ToString();
					MenuController.instance.UnlockablesController.gridBlocks[i].purchaseBtn.transform.GetChild(0).GetComponentInChildren<Image>().enabled=true;
				}

				if(MenuController.instance.UnlockablesController.gridBlocks[i].starReq > AccountInfo.TotalStars())
				{
					MenuController.instance.UnlockablesController.gridBlocks[i].lockedPanel.SetActive(true);
					MenuController.instance.UnlockablesController.gridBlocks[i].purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text="Locked";
					MenuController.instance.UnlockablesController.gridBlocks[i].itemName.text="?????";
					MenuController.instance.UnlockablesController.gridBlocks[i].lockedText.GetComponent<TextMeshProUGUI>().text = MenuController.instance.NavBar.unlockablesPanel.GetComponent<UnlockablesController>().gridBlocks[i].itemData.StarReq.ToString();
					MenuController.instance.UnlockablesController.gridBlocks[i].purchaseBtn.transform.GetChild(0).GetComponentInChildren<Image>().enabled=false;
				} else 
				{
					MenuController.instance.UnlockablesController.gridBlocks[i].lockedPanel.SetActive(false);
					MenuController.instance.UnlockablesController.gridBlocks[i].lockedText.SetActive(false);
				}
			}
			MenuController.instance.UnlockablesController.UnlockShopButtons();
			purchaseBtn.GetComponent<Image>().color=MenuController.instance.UnlockablesController.greenPurchase;
			purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text=" Active";
			purchaseBtn.transform.GetChild(0).GetComponentInChildren<Image>().enabled=false;
			purchaseBtn.SetActive(true);


			UninspectItem();
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
		MenuController.instance.coinEmitter.EnableEmission(true);
		MenuController.instance.coinEmitter.SetEmissionRate(itemData.Cost/100);
		MenuController.instance.CurrencyUI.DecreaseCurrency(itemData.Cost);
		AccountInfo.GetAccountInfo();
		unlocked=true;
		MenuController.instance.UnlockablesController.tileSkinCount++;

		AudioManager.instance.Play("coinLoop");
		

		Invoke("StopCoinEmission", 6.5f);
		Invoke("StopCoinAnim", 10f);
		Invoke("EquipGridSkin", 10f);
		
	}

	

	public void StopCoinEmission()
	{
		MenuController.instance.coinEmitter.EnableEmission(false);
		Debug.LogError("GRID SKIN EQUIPPED! POPUP IMPLEMENT");
		AudioManager.instance.Stop("coinLoop");

	}

	public void StopCoinAnim()
	{
		MenuController.instance.confirmPurchaseBtn.GetComponent<Animator>().SetTrigger("purchaseComplete");

	}
}
