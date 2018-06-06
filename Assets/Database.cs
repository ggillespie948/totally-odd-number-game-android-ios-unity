using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class Database : MonoBehaviour {

	private static Database instance;
	
	public static Database Instance
	{
		get { return instance;}
		set { instance = value;}
	}

	[SerializeField]
	private List<ShopItemTile> tileSkinShopItems;
	public List<ShopItemTile> TileSkinShopItems
	{
		get { return tileSkinShopItems;}
		set { tileSkinShopItems = value;}
	}

	[SerializeField]
	private List<CatalogItem> catalogTileSkins;
	public List<CatalogItem> CatalogTileSkins
	{
		get { return catalogTileSkins;}
		set { catalogTileSkins = value;}
	}

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		if(instance !=this)
			instance = this;

		catalogTileSkins = new List<CatalogItem>();
		tileSkinShopItems = new List<ShopItemTile>();
		DontDestroyOnLoad(gameObject);
	}

	public static void GetPlayerInventory()
	{
		//GetPlayerI


	}

	public static void UpdateDatabase()
	{
		GetCatalogItemsRequest request  = new GetCatalogItemsRequest()
		{
			CatalogVersion = AccountInfo.CATALOG_ITEMS
		};

		PlayFabClientAPI.GetCatalogItems(request, UpdateDatabaseSuccess,AccountInfo.OnAPIError);
	}

	public static void UpdateDatabaseSuccess(GetCatalogItemsResult result)
	{
		for(int i=0; i<result.Catalog.Count; i++)
		{
			if(result.Catalog[i].ItemClass == AccountInfo.ITEM_TILESKIN)
			{
				Instance.catalogTileSkins.Add(result.Catalog[i]);
				Instance.TileSkinShopItems.Add(CreateTileShopItem(result.Catalog[i], i));
			}
		}

		GUI_Controller.instance.NavBar.unlockablesPannel.GetComponent<UnlockablesController>().LoadTileStore();
	}

	public static ShopItemTile CreateTileShopItem(CatalogItem item, int i)
	{
		Debug.Log("Creating shop tile item.. " + item.ItemId);
		ShopItemTile ts = new ShopItemTile(
			i,
			item.DisplayName,
		 	int.Parse(GetCatalogCustomData(AccountInfo.ITEM_COST, item)),
			int.Parse(GetCatalogCustomData(AccountInfo.ITEM_STARREQ, item)),
		 	Resources.Load(GetCatalogCustomData(AccountInfo.ITEM_ICON, item), typeof(Image)) as Image,
		 	Resources.Load(GetCatalogCustomData(AccountInfo.ITEM_PREFAB, item), typeof(TileSkin)) as TileSkin,
			item.ItemId
		);

		return ts;

	}

	public static string GetCatalogCustomData(int index, CatalogItem item)
	{
		string cDataTemp = item.CustomData.Trim();
		cDataTemp = cDataTemp.TrimStart('{');
		cDataTemp = cDataTemp.TrimEnd('}');
		string [] newCData;
		newCData = cDataTemp.Split(',',':');

		for(int i=0; i<newCData.Length; i++)
		{
			if(index==i)
			{
				newCData[i] = newCData[i].Trim();
				newCData[i] = newCData[i].TrimStart('"');
				newCData[i] = newCData[i].TrimEnd('"');
				newCData[i] = newCData[i].Trim();
				Debug.Log(newCData[i]);
				return newCData[i];
				
			}

		}

		Debug.Log("Could not find index in catalog data");
		return "ERROR";
	} 
	

	
}
