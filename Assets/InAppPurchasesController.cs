using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using TMPro;
using UnityEngine.UI;

public class InAppPurchasesController : MonoBehaviour , IStoreListener {

	
 	[Header("IAP Scroll View Panel")]
	[SerializeField]
	public ScrollRect scrollRect;
	[SerializeField]
	public RectTransform scrollRectContent;
	public RectTransform coinStoreMarker;
	 // Items list, configurable via inspector
    [SerializeField]
	public List<CatalogItem> Catalog;

	// The Unity Purchasing system
    private static IStoreController m_StoreController;

	[Header("Coin Bundles")]
	[SerializeField]
	public List<ShopItem> coinBundles;

	[Header("Energy Passes")]
	[SerializeField]
	public List<ShopItem> energyPasses;

	[Header("Energy Refill")]
	[SerializeField]
	public ShopItem energyRefill;

	[Header("Full Game Level Pack")]
	[SerializeField]
	public ShopItem fullGameLevelPack;

	[SerializeField]
	public GameObject yesBtn;
	[SerializeField]
	public GameObject noBtn;

	public void LoadStore()
	{

		//Load Energy Refill
		energyRefill.itemData=Database.Instance.EnergyShopRefill;
		energyRefill.item=Database.Instance.CatalogEnergyRefill;
		energyRefill.LoadItem();

		fullGameLevelPack.itemData=Database.Instance.FullGameLevelPack;
		fullGameLevelPack.item=Database.Instance.CatalogFullGameLevelPack;
		fullGameLevelPack.LoadItem();

		//Load Energy Passes
		for(int i=0; i<Database.Instance.EnergyShopPasses.Count;i++)
		{
			//Debug.LogWarning("Loading pass object");
			energyPasses[i].itemData=Database.Instance.EnergyShopPasses[i];
			energyPasses[i].item=Database.Instance.CatalogEnergyPasses[i];
			energyPasses[i].LoadItem();
		}



		//Load Coin Bundles
		for(int i=0; i<Database.Instance.CoinBundles.Count;i++)
		{
			//Debug.LogWarning("Loading pass object");
			coinBundles[i].itemData=Database.Instance.CoinBundles[i];
			coinBundles[i].item=Database.Instance.CatalogCoinBundles[i];
			coinBundles[i].LoadItem();
		}

		//Load Full Game Level Pack

		Catalog=Database.Instance.CatalogEnergyPasses;
		Catalog.AddRange(Database.Instance.CatalogCoinBundles);
		Catalog.Add(Database.Instance.CatalogFullGameLevelPack);
		InitializePurchasing();
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

	    // This is invoked manually on Start to initialize UnityIAP
    public void InitializePurchasing() {
        // If IAP is already initialized, return gently
        if (IsInitialized) return;

        //Create a builder for IAP service
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.GooglePlay));

        // Register each item from the catalog
        foreach (var item in Catalog) {
            builder.AddProduct(item.ItemId, ProductType.Consumable);
        }

        // Trigger IAP service initialization
        UnityPurchasing.Initialize(this, builder);
    }

    // We are initialized when StoreController and Extensions are set and we are logged in
    public bool IsInitialized {
        get {
            return m_StoreController != null && Catalog != null;
        }
    }

    // This is automatically invoked automatically when IAP service is initialized
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
        m_StoreController = controller;
    }

    // This is automatically invoked automatically when IAP service failed to initialized
    public void OnInitializeFailed(InitializationFailureReason error) {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    // This is automatically invoked automatically when purchase failed
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    // This is invoked automatically when succesful purchase is ready to be processed
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e) {
        // NOTE: this code does not account for purchases that were pending and are
        // delivered on application start.
        // Production code should account for such case:
        // More: https://docs.unity3d.com/ScriptReference/Purchasing.PurchaseProcessingResult.Pending.html

        if (!IsInitialized) {
            return PurchaseProcessingResult.Complete;
        }

        // Test edge case where product is unknown
        if (e.purchasedProduct == null) {
            Debug.LogWarning("Attempted to process purchasewith unknown product. Ignoring");
            return PurchaseProcessingResult.Complete;
        }

        // Test edge case where purchase has no receipt
        if (string.IsNullOrEmpty(e.purchasedProduct.receipt)) {
            Debug.LogWarning("Attempted to process purchase with no receipt: ignoring");
            return PurchaseProcessingResult.Complete;
        }

        Debug.Log("Processing transaction: " + e.purchasedProduct.transactionID);

        // Deserialize receipt
        var googleReceipt = GooglePurchase.FromJson(e.purchasedProduct.receipt);

        // Invoke receipt validation
        // This will not only validate a receipt, but will also grant player corresponding items
        // only if receipt is valid.
        PlayFabClientAPI.ValidateGooglePlayPurchase(new ValidateGooglePlayPurchaseRequest() {
            // Pass in currency code in ISO format
            CurrencyCode = e.purchasedProduct.metadata.isoCurrencyCode,
            // Convert and set Purchase price
            PurchasePrice = (uint)(e.purchasedProduct.metadata.localizedPrice * 100),
            // Pass in the receipt
            ReceiptJson = googleReceipt.PayloadData.json,
            // Pass in the signature
            Signature = googleReceipt.PayloadData.signature
        }, result => PurchaseComplete() ,
           error => Debug.Log("Validation failed: " + error.GenerateErrorReport())
        );

		
		

        return PurchaseProcessingResult.Complete;
    }

	private static void PurchaseComplete()
	{
		switch(MenuController.instance.NavBar.activeShopItem.GetComponent<ShopItem>().item.ItemClass)
		{
			case "Coin Pack":
				MenuController.instance.CoinPurchaseComplete();
			break;

			case "Life Refill":
				MenuController.instance.LifePurchaseComplete();
			break;

			case "Level Pack":
			MenuController.instance.LevelPurchaseComplete();
			break;
		}

	}

    // This is invvoked manually to initiate purchase
    public void BuyProductID(string productId) {
        // If IAP service has not been initialized, fail hard
        if (!IsInitialized) throw new Exception("IAP Service is not initialized!");

        // Pass in the product id to initiate purchase
        m_StoreController.InitiatePurchase(productId);
    }


	} // End or IAP Purchases Class

	// The following classes are used to deserialize JSON results provided by IAP Service
	// Please, note that Json fields are case-sensetive and should remain fields to support Unity Deserialization via JsonUtilities
	public class JsonData {
		// Json Fields, ! Case-sensetive

		public string orderId;
		public string packageName;
		public string productId;
		public long purchaseTime;
		public int purchaseState;
		public string purchaseToken;
	}

	public class PayloadData {
		public JsonData JsonData;

		// Json Fields, ! Case-sensetive
		public string signature;
		public string json;

		public static PayloadData FromJson(string json) {
			var payload = JsonUtility.FromJson<PayloadData>(json);
			payload.JsonData = JsonUtility.FromJson<JsonData>(payload.json);
			return payload;
		}
	}

	public class GooglePurchase {
		public PayloadData PayloadData;

		// Json Fields, ! Case-sensetive
		public string Store;
		public string TransactionID;
		public string Payload;

		public static GooglePurchase FromJson(string json) {
			var purchase = JsonUtility.FromJson<GooglePurchase>(json);
			purchase.PayloadData = PayloadData.FromJson(purchase.Payload);
			return purchase;
		}
	}
