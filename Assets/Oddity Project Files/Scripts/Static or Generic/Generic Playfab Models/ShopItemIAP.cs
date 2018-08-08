using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShopItemIAP {

	[SerializeField]
	private int index;
	[SerializeField]
	private string name;
	[SerializeField]
	private string description;
	[SerializeField]
	private int cost;
	[SerializeField]
	private Image icon;
	[SerializeField]
	private string itemID;

	public ShopItemIAP(int _index, string _name, string _description, int _cost, string _itemID)
	{
		index=_index;
		name=_name;
		cost=_cost;
		description=_description;										
		itemID=_itemID;
	}

	public int Index{
		get {return index;}
		set {index = value;}
	}

	public string Name{
		get {return name;}
		set {name = value;}
	}

	public int Cost{
		get {return cost;}
		set {cost = value;}
	}

	public string Description{
		get {return description;}
		set {description = value;}
	}

	public string ItemID
	{
		get {return itemID;}
		set {itemID = value;}
	}

	

}
