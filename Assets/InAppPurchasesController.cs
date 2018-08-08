using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InAppPurchasesController : MonoBehaviour {

	[Header("Coin Bundles")]
	[SerializeField]
	public List<ShopItem> coinBundles;

	[Header("Energy Passes")]
	[SerializeField]
	public List<ShopItem> energyPasses;

	[Header("Energy Refill")]
	[SerializeField]
	public ShopItem energyRefill;

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

		//Load Energy Passes
		for(int i=0; i<Database.Instance.EnergyShopPasses.Count;i++)
		{
			Debug.LogWarning("Loading pass object");
			energyPasses[i].itemData=Database.Instance.EnergyShopPasses[i];
			energyPasses[i].item=Database.Instance.CatalogEnergyPasses[i];
			energyPasses[i].LoadItem();
		}

		//Load Coin Bundles
		// for(int i=0; i<Database.Instance.Co.Count;i++)
		// {
		// 	energyPasses[i].itemData=Database.Instance.EnergyShopPasses[i];
		// 	energyPasses[i].item=Database.Instance.CatalogEnergyPasses[i];
		// 	energyPasses[i].LoadItem();
		// }

		//Load Full Game Level Pack

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
