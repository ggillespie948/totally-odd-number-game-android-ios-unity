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

	public Vector3 startPos = new Vector3();
	

	public ShopItemTile itemData;

	[SerializeField]
	public CatalogItem item;

	public Image border;
	public GameObject lockedPanel;
	
	private int starReq;

	public void InspectItem()
	{
		if(starReq > AccountInfo.TotalStars())
		{
			GUI_Controller.instance.SpawnTextPopup("This item requires "+starReq+" stars to be unlocked", Color.white, this.transform, 24 );
			return;
		}
		startPos=transform.position;
		GetComponent<GUI_Object>().targetPos= GUI_Controller.instance.confirmPurchasePanelTarget.transform.position;
		this.gameObject.transform.SetParent(GUI_Controller.instance.confirmPurchasePanel.transform);
		GUI_Controller.instance.confirmPurchasePanel.SetActive(true);
		StartCoroutine(GetComponent<GUI_Object>().ScaleUp(1.45f,1f));
		StartCoroutine(GetComponent<GUI_Object>().AnimateTo(GUI_Controller.instance.confirmPurchasePanelTarget.transform.position, .5f));

		MenuController.instance.NavBar.activeTileBox=this.gameObject;
		MenuController.instance.NavBar.gameObject.SetActive(false);
		MenuController.instance.NavBar.shopItemTitle.text=item.Description;
		MenuController.instance.NavBar.priceText.text= "Do you wish to purchase this " + item.ItemClass + " for " +itemData.Cost + "         ?";

		int itemCount=0;
		List<GridTile> allTiles = itemData.Prefab.AllTiles();
		foreach(GameObject obj in MenuController.instance.NavBar.previewItems)
		{

			if(obj.GetComponent<GridTile>() != null)
			{
				obj.GetComponent<Renderer>().material=allTiles[itemCount].GetComponent<Renderer>().sharedMaterial;
			}
			itemCount++;

		}

		purchaseBtn.gameObject.SetActive(false); //close button animation
	}

	public void UninspectItem()	{
		GetComponent<GUI_Object>().targetPos= startPos;
		MenuController.instance.NavBar.activeTileBox=null;
		
		StartCoroutine(GetComponent<GUI_Object>().ScaleUp(1.39f,1f));
		StartCoroutine(GetComponent<GUI_Object>().AnimateTo(startPos, 1f));

		MenuController.instance.NavBar.gameObject.SetActive(true);
		purchaseBtn.gameObject.SetActive(true);
		this.gameObject.transform.SetParent(MenuController.instance.NavBar.tileSkinParent.transform);
		GUI_Controller.instance.confirmPurchasePanel.GetComponent<Animator>().SetTrigger("ClosePanel");
		Invoke("CloseInspector", 1.5f);
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

		if(starReq > AccountInfo.TotalStars())
		{
			lockedPanel.SetActive(true);
			purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text="Locked";
			itemName.text="?????";
		} else 
		{
			lockedPanel.SetActive(false);
		}

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
				MenuController.instance.NavBar.unlockablesPannel.GetComponent<UnlockablesController>().tileBlocks[i].GetComponent<Image>().color=Color.white;
				MenuController.instance.NavBar.unlockablesPannel.GetComponent<UnlockablesController>().tileBlocks[i].equipBtn.gameObject.SetActive(true);
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
