using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class GridBox : MonoBehaviour {

	public Button purchaseBtn; 
	public Button starReqBtn;
	public Image itemIcon;
	public TextMeshProUGUI itemName;
	public string itemID;
	public bool unlocked=false;
	public Vector3 startPos = new Vector3();

	//public GridSkin tileSkin;

	public ShopItemTile itemData;

	[SerializeField]
	public CatalogItem item;
	public GameObject lockedPanel;
	private int starReq;
	bool inspected=false;

	public void InspectItem()
	{
		if(inspected)
			return;

		inspected=true;
		if(starReq > AccountInfo.TotalStars())
		{
			Debug.Log("Not enough stars to inspect");
			GUI_Controller.instance.SpawnTextPopup("This item requires "+starReq+" stars to be unlocked", Color.white, this.transform, 24 );
			return;
		}
		startPos=transform.position;
		GetComponent<GUI_Object>().targetPos= GUI_Controller.instance.confirmPurchasePanelTarget.transform.position;
		this.gameObject.transform.SetParent(GUI_Controller.instance.confirmPurchasePanel.transform);
		GUI_Controller.instance.confirmPurchasePanel.SetActive(true);
		StartCoroutine(GetComponent<GUI_Object>().ScaleUp(1.45f,1f));
		StartCoroutine(GetComponent<GUI_Object>().AnimateTo(GUI_Controller.instance.confirmPurchasePanelTarget.transform.position, .35f));

		MenuController.instance.NavBar.activeTileBox=this.gameObject;

		if(AccountInfo.Instance.InventoryContains(item))
		{
			MenuController.instance.NavBar.gameObject.SetActive(false);
			MenuController.instance.NavBar.shopItemTitle.text=item.Description;
			MenuController.instance.NavBar.priceText.text= "Do you wish to equip this " + item.ItemClass + "?";
			MenuController.instance.NavBar.coinIcon.enabled=false;
		} else 
		{
			MenuController.instance.NavBar.priceText.text= "Do you wish to purchase this " + item.ItemClass + " for " +itemData.Cost + "         ?";
			MenuController.instance.NavBar.coinIcon.enabled=true;
		}

		int itemCount=0;
		List<GridTile> allTiles = itemData.Prefab.AllTiles();
		foreach(GameObject obj in MenuController.instance.NavBar.previewItems)
		{

			if(obj.GetComponent<GridTile>() != null)
			{
				obj.GetComponent<Renderer>().material=allTiles[itemCount].GetComponent<Renderer>().sharedMaterial;
				obj.GetComponent<GridTile>().activeSkin=allTiles[itemCount].activeSkin;
			}
			itemCount++;

		}

		purchaseBtn.gameObject.SetActive(false); //close button animation
	}

	public void UninspectItem()	{
		inspected=false;
		GetComponent<GUI_Object>().targetPos= startPos;
		MenuController.instance.NavBar.activeTileBox=null;
		
		StartCoroutine(GetComponent<GUI_Object>().ScaleUp(1.39f,1f));
		StartCoroutine(GetComponent<GUI_Object>().AnimateTo(startPos, .35f));

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


		if(AccountInfo.Instance.InventoryContains(item))
		{
			purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text=" Owned";
			purchaseBtn.transform.GetChild(0).GetComponentInChildren<Image>().enabled=false;
			unlocked=true;
		} else 
		{
			purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text = itemData.Cost.ToString();
			purchaseBtn.transform.GetChild(0).GetComponentInChildren<Image>().enabled=true;
		}

		if(AccountInfo.TILESKIN == itemData.Index)
		{
			purchaseBtn.GetComponent<Image>().color=Color.green;
			purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text=" Active";
			purchaseBtn.transform.GetChild(0).GetComponentInChildren<Image>().enabled=false;

		}

		if(AccountInfo.tileUnlockString!= null)
		{
			if(starReq > AccountInfo.TotalStars() || AccountInfo.tileUnlockString[itemData.Index] == '0')
			{
				lockedPanel.SetActive(true);
				purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text="Locked";
				itemName.text="?????";
				lockedPanel.GetComponentInChildren<TextMeshProUGUI>().text = itemData.StarReq.ToString();
				purchaseBtn.transform.GetChild(0).GetComponentInChildren<Image>().enabled=false;
			} else 
			{
				lockedPanel.SetActive(false);
			}
		} else
		{
			Debug.Log("TILE UNLOCK STRING NULL");
		}


		

		this.gameObject.SetActive(true);
	}

	public void UnlockItem()
	{
		starReqBtn.gameObject.SetActive(false);
		//purchaseBtn.gameObject.SetActive(false);
		unlocked=true;
	}

	public void RevealAnimation()
	{
		StartCoroutine(revealAfterDelay());
	}

	private IEnumerator revealAfterDelay()
	{
		yield return new WaitForSeconds(1.5f);
		GetComponent<Animator>().SetTrigger("reveal");


		// play sound effect
		AudioManager.instance.Play("Unlock");

		//Update tile unlock string so animation is only played once
		char[] unlockS = AccountInfo.tileUnlockString.ToCharArray();
		unlockS[itemData.Index]='1';
		AccountInfo.tileUnlockString = new string(unlockS);
		AccountInfo.UpdateTileSkinUnlockString();
		
		
		itemIcon.sprite = itemData.Icon.sprite;
		itemName.text = itemData.Name;
		itemID = itemData.ItemID;
		starReq = itemData.StarReq;
		startPos = transform.position;

		Invoke("LoadItem", 2f);

	}

	public void EquipTileSkin()
	{
		if(unlocked)
		{
			ApplicationModel.TILESKIN=itemData.Index;
			AccountInfo.Instance.UpdateTileSkin(itemData.Index);
			for(int i=0; i<Database.Instance.CatalogTileSkins.Count;i++)
			{
				MenuController.instance.NavBar.unlockablesPanel.GetComponent<UnlockablesController>().tileBlocks[i].GetComponent<Image>().color=Color.white;
				MenuController.instance.NavBar.unlockablesPanel.GetComponent<UnlockablesController>().tileBlocks[i].purchaseBtn.GetComponent<Image>().color=Color.blue;

				if(MenuController.instance.NavBar.unlockablesPanel.GetComponent<UnlockablesController>().tileBlocks[i].unlocked)
				{
					MenuController.instance.NavBar.unlockablesPanel.GetComponent<UnlockablesController>().tileBlocks[i].purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text=" Owned";
				} else 
				{
					MenuController.instance.NavBar.unlockablesPanel.GetComponent<UnlockablesController>().tileBlocks[i].purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text = itemData.Cost.ToString();
					MenuController.instance.NavBar.unlockablesPanel.GetComponent<UnlockablesController>().tileBlocks[i].purchaseBtn.transform.GetChild(0).GetComponentInChildren<Image>().enabled=true;
				}

				if(MenuController.instance.NavBar.unlockablesPanel.GetComponent<UnlockablesController>().tileBlocks[i].starReq > AccountInfo.TotalStars())
				{
					MenuController.instance.NavBar.unlockablesPanel.GetComponent<UnlockablesController>().tileBlocks[i].lockedPanel.SetActive(true);
					MenuController.instance.NavBar.unlockablesPanel.GetComponent<UnlockablesController>().tileBlocks[i].purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text="Locked";
					MenuController.instance.NavBar.unlockablesPanel.GetComponent<UnlockablesController>().tileBlocks[i].itemName.text="?????";
					MenuController.instance.NavBar.unlockablesPanel.GetComponent<UnlockablesController>().tileBlocks[i].lockedPanel.GetComponentInChildren<TextMeshProUGUI>().text = MenuController.instance.NavBar.unlockablesPanel.GetComponent<UnlockablesController>().tileBlocks[i].itemData.StarReq.ToString();
					MenuController.instance.NavBar.unlockablesPanel.GetComponent<UnlockablesController>().tileBlocks[i].purchaseBtn.transform.GetChild(0).GetComponentInChildren<Image>().enabled=false;
				} else 
				{
					MenuController.instance.NavBar.unlockablesPanel.GetComponent<UnlockablesController>().tileBlocks[i].lockedPanel.SetActive(false);
				}
			}
			purchaseBtn.GetComponent<Image>().color=Color.green;
			purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text=" Active";
			purchaseBtn.transform.GetChild(0).GetComponentInChildren<Image>().enabled=false;
			//UninspectItem();
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
		
		//TEMP - switch this between different unlcokable item items e.g. tile skin, theme, grid skin
		EquipTileSkin();



		Invoke("StopCoinEmission", 6.5f);
		Invoke("StopCoinAnim", 10f);
		
	}


	private int itemCount=0;
	public IEnumerator ActivatePreviewTiles()
	{
		List<GridTile> allTiles = itemData.Prefab.AllTiles();
		
		foreach(GameObject obj in MenuController.instance.NavBar.previewItems)
		{
			if(obj.GetComponent<GridTile>() != null)
			{
				obj.GetComponent<Renderer>().material=allTiles[itemCount].GetComponent<Renderer>().sharedMaterial;
				itemCount++;
				AudioManager.instance.Play("piano"+itemCount);
				obj.GetComponent<GridTile>().ActiveTileSkin();
				ActivateTile(obj.GetComponent<GridTile>());
				yield return new WaitForSeconds(.5f);
			}

		}

		Invoke("EquipTileSkin", 3f);
	}
	public void ActivateTile(GridTile tile) 
    {
		
        if (tile != null && !tile.activated )
        {
            tile.ActiveTileSkin();
            tile.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
            //tile.GetComponent<NoGravity>().enabled = false;
            switch(tile.value)
            {
                case 1:
                GameObject FX;
                FX = Instantiate(itemData.Prefab.Tile1FX, tile.gameObject.transform.position-
                    new Vector3(0,0,5), itemData.Prefab.Tile1FX.transform.rotation);

                FX.transform.localScale=new Vector3(0.5f,0.5f,0.5f);
                Destroy(FX, 1f);
                break;

                case 2:
                GameObject FX2;
                FX2 = Instantiate(itemData.Prefab.Tile2FX, tile.gameObject.transform.position-
                    new Vector3(0,0,5), itemData.Prefab.Tile2FX.transform.rotation);
                FX2.transform.localScale=new Vector3(0.5f,0.5f,0.5f);
                Destroy(FX2, 1f);
                break;

                case 3:
                GameObject FX3;
                FX3 = Instantiate(itemData.Prefab.Tile3FX, tile.gameObject.transform.position-
                    new Vector3(0,0,5), itemData.Prefab.Tile3FX.transform.rotation);
                FX3.transform.localScale=new Vector3(0.5f,0.5f,0.5f);
                Destroy(FX3, 1f);
                break;

                case 4:
                GameObject FX4;
                FX4 = Instantiate(itemData.Prefab.Tile4FX, tile.gameObject.transform.position-
                    new Vector3(0,0,5), itemData.Prefab.Tile4FX.transform.rotation);
                FX4.transform.localScale=new Vector3(0.5f,0.5f,0.5f);
                Destroy(FX4, 1f);
                break;

                case 5:
                GameObject FX5;
                FX5 = Instantiate(itemData.Prefab.Tile5FX, tile.gameObject.transform.position-
                    new Vector3(0,0,5), itemData.Prefab.Tile5FX.transform.rotation);
                FX5.transform.localScale=new Vector3(0.5f,0.5f,0.5f);
                Destroy(FX5, 1f);
                break;

                case 6:
                GameObject FX6;
                FX6 = Instantiate(itemData.Prefab.Tile6FX, tile.gameObject.transform.position-
                    new Vector3(0,0,5), itemData.Prefab.Tile6FX.transform.rotation);
                FX6.transform.localScale=new Vector3(0.5f,0.5f,0.5f);
                Destroy(FX6, 1f);
                break;

                case 7:
                GameObject FX7;
                FX7 = Instantiate(itemData.Prefab.Tile7FX, tile.gameObject.transform.position-
                    new Vector3(0,0,5), itemData.Prefab.Tile7FX.transform.rotation);
                FX7.transform.localScale=new Vector3(0.5f,0.5f,0.5f);
                Destroy(FX7, 1f);
                break;

                case 8:
                GameObject FX8;
                FX8 = Instantiate(itemData.Prefab.Tile8FX, tile.gameObject.transform.position-
                    new Vector3(0,0,5), itemData.Prefab.Tile8FX.transform.rotation);
                FX8.transform.localScale=new Vector3(0.5f,0.5f,0.5f);
                Destroy(FX8, 1f);
                break;

                case 9:
                GameObject FX9;
                FX9 = Instantiate(itemData.Prefab.Tile9FX, tile.transform.position-
                    new Vector3(0,0,5), itemData.Prefab.Tile9FX.transform.rotation);
                FX9.transform.localScale=new Vector3(0.5f,0.5f,0.5f);
                Destroy(FX9, 1f);
                break;
            }
            tile.activated = true;
        } 
    }

	public void StopCoinEmission()
	{
		MenuController.instance.coinEmitter.EnableEmission(false);
		itemCount=0;
		StartCoroutine(ActivatePreviewTiles());
		
	}

	public void StopCoinAnim()
	{
		
		MenuController.instance.confirmPurchaseBtn.GetComponent<Animator>().SetTrigger("purchaseComplete");

	}
}
