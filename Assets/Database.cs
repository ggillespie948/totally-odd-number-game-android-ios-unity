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

	[Header("Tile Skins")]
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
	//------------------------------------------

	[Header("Grid Skins")]
	[SerializeField]
	private List<ShopItemGrid> gridSkinShopItems;
	public List<ShopItemGrid> GridSkinShopItems
	{
		get { return gridSkinShopItems;}
		set { gridSkinShopItems = value;}
	}

	[SerializeField]
	private List<CatalogItem> catalogGridSkins;
	public List<CatalogItem> CatalogGridSkins
	{
		get { return catalogGridSkins;}
		set { catalogGridSkins = value;}
	}
	//------------------------------------------

	[Header("Coin Bundles")]
	[SerializeField]
	private List<ShopItemIAP> coinBundles;
	public List<ShopItemIAP> CoinBundles
	{
		get { return coinBundles;}
		set { coinBundles = value;}
	}

	[SerializeField]
	private List<CatalogItem> catalogCoinBundles;
	public List<CatalogItem> CatalogCoinBundles
	{
		get { return catalogCoinBundles;}
		set { catalogCoinBundles = value;}
	}
	//------------------------------------------
	[Header("Energy Passes")]
	[SerializeField]
	private List<ShopItemIAP> energyShopPasses;
	public List<ShopItemIAP> EnergyShopPasses
	{
		get { return energyShopPasses;}
		set { energyShopPasses = value;}
	}

	[SerializeField]
	private List<CatalogItem> catalogEnergyPasses;
	public List<CatalogItem> CatalogEnergyPasses
	{
		get { return catalogEnergyPasses;}
		set { catalogEnergyPasses = value;}
	}
	//------------------------------------------

	[Header("Energy Refill")]
	[SerializeField]
	private ShopItemIAP energyShopRefill;
	public ShopItemIAP EnergyShopRefill
	{
		get { return energyShopRefill;}
		set { energyShopRefill = value;}
	}
	[SerializeField]
	private CatalogItem catalogEnergyRefill;
	public CatalogItem CatalogEnergyRefill
	{
		get { return catalogEnergyRefill;}
		set { catalogEnergyRefill = value;}
	}
	//------------------------------------------


	[Header("Full Game Level Pack")]
	[SerializeField]
	private ShopItemIAP fullGameLevelPack;
	public ShopItemIAP FullGameLevelPack
	{
		get { return fullGameLevelPack;}
		set { fullGameLevelPack = value;}
	}
	[SerializeField]
	private CatalogItem catalogFullGameLevelPack;
	public CatalogItem CatalogFullGameLevelPack
	{
		get { return catalogFullGameLevelPack;}
		set { catalogFullGameLevelPack = value;}
	}
	//------------------------------------------
	// }

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

	public static void LoadCatalog()
	{
		GetCatalogItemsRequest request  = new GetCatalogItemsRequest()
		{
			CatalogVersion = AccountInfo.CATALOG_ITEMS
		};

		PlayFabClientAPI.GetCatalogItems(request, UpdateDatabaseSuccess,AccountInfo.OnAPIError);
	}


	/// <summary>
	/// This method iterates through every catalog item returned from the catalog and creates the relevant shop item
	/// based on the item class of each.false This filters results from BOTH the UNLOCKABLES STORES  and IN-APP PURCHASES
	/// </summary>
	/// <param name="result"></param>
	public static void UpdateDatabaseSuccess(GetCatalogItemsResult result)
	{
		for(int i=0; i<result.Catalog.Count; i++)
		{
			if(result.Catalog[i].ItemClass == AccountInfo.ITEM_TILESKIN)
			{
				Instance.catalogTileSkins.Add(result.Catalog[i]);
				Instance.TileSkinShopItems.Add(CreateTileShopItem(result.Catalog[i],i));
			} else if(result.Catalog[i].ItemClass == AccountInfo.ITEM_GRIDSKIN)
			{
				Instance.catalogGridSkins.Add(result.Catalog[i]);
				Instance.GridSkinShopItems.Add(CreateGridShopItem(result.Catalog[i]));
			} else if(result.Catalog[i].ItemClass == AccountInfo.ITEM_LIFEREFILL)
			{
				Instance.catalogEnergyRefill = result.Catalog[i];
				Instance.EnergyShopRefill = CreateIAPShopItem(result.Catalog[i], 0);

			} else if(result.Catalog[i].ItemClass == AccountInfo.ITEM_LIFEPASS)
			{
				//Debug.LogWarning("Life pass found, backend not implemented.");
				Instance.catalogEnergyPasses.Add(result.Catalog[i]);
				Instance.energyShopPasses.Add(CreateIAPShopItem(result.Catalog[i], i));
			} else if(result.Catalog[i].ItemClass == AccountInfo.ITEM_COINPACK)
			{
				//Debug.LogWarning("Adding coin bundle..");
				Instance.catalogCoinBundles.Add(result.Catalog[i]);
				Instance.coinBundles.Add(CreateIAPShopItem(result.Catalog[i], i));
			} else if(result.Catalog[i].ItemClass == AccountInfo.ITEM_LEVELPACK)
			{
				//Debug.LogWarning("Adding Full game level pack");
				Instance.catalogFullGameLevelPack = result.Catalog[i];
				Instance.FullGameLevelPack = CreateIAPShopItem(result.Catalog[i], 0);
			} else 
			{
				//Debug.LogWarning("Unknown catalog item found: " + result.Catalog[i].ItemClass);
			}
		}
		MenuController.instance.NavBar.unlockablesPanel.GetComponent<UnlockablesController>().LoadTileStore();
		MenuController.instance.NavBar.unlockablesPanel.GetComponent<UnlockablesController>().LoadGridStore();
		MenuController.instance.InAppPurchasesController.LoadStore();
	}

	public static ShopItemTile CreateTileShopItem(CatalogItem item, int i)
	{
		//Debug.Log("Creating shop tile item.. " + item.ItemId);
		ShopItemTile ts = new ShopItemTile(
			int.Parse(GetCatalogCustomData(AccountInfo.UNLOCKNO, item)),
			item.DisplayName,
		 	int.Parse(GetCatalogCustomData(AccountInfo.ITEM_COST, item)),
			int.Parse(GetCatalogCustomData(AccountInfo.ITEM_STARREQ, item)),
		 	Resources.Load(GetCatalogCustomData(AccountInfo.ITEM_ICON, item), typeof(Image)) as Image,
		 	Resources.Load(GetCatalogCustomData(AccountInfo.ITEM_PREFAB, item), typeof(TileSkin)) as TileSkin,
			item.ItemId
		);

		return ts;
	}

	public static ShopItemGrid CreateGridShopItem(CatalogItem item)
	{
		//Debug.Log("Creating grid skin shop item.. " + item.ItemId);
		ShopItemGrid ts = new ShopItemGrid(
			int.Parse(GetCatalogCustomData(AccountInfo.UNLOCKNO, item)),
			item.DisplayName,
		 	int.Parse(GetCatalogCustomData(AccountInfo.ITEM_COST, item)),
			int.Parse(GetCatalogCustomData(AccountInfo.ITEM_STARREQ, item)),
		 	Resources.Load(GetCatalogCustomData(AccountInfo.ITEM_ICON, item), typeof(Image)) as Image,
		 	Resources.Load(GetCatalogCustomData(AccountInfo.ITEM_PREFAB, item), typeof(Theme)) as Theme,
			item.ItemId
		);

		return ts;
	}

	public static ShopItemIAP CreateIAPShopItem(CatalogItem item, int i)
	{
		//Debug.Log("Creating shop item.. " + item.ItemId);
		ShopItemIAP er = new ShopItemIAP(
			i,
			item.DisplayName,
			item.Description,
		 	int.Parse(GetCatalogCustomData(AccountInfo.ITEM_COST, item)),
			item.ItemId
		);


		return er;
	}

	public static ShopItemIAP CreateEnergyPassItem(CatalogItem item, int i)
	{
		//Debug.Log("Creating shop energy pass item.. " + item.ItemId);
		ShopItemIAP ep = new ShopItemIAP(
			i,
			item.DisplayName,
			item.Description,
		 	int.Parse(GetCatalogCustomData(AccountInfo.ITEM_COST, item)),
			item.ItemId
		);

		return ep;
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
				return newCData[i];
				
			}

		}

		//Debug.Log("Could not find index in catalog data");
		return "ERROR";
	} 
	

	
}
