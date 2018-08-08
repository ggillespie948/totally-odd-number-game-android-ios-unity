using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShopItemTile {

	[SerializeField]
	private int index;
	[SerializeField]
	private string name;
	[SerializeField]
	private string description;
	[SerializeField]
	private int cost;
	[SerializeField]
	private int starReq;
	[SerializeField]
	private Image icon;
	[SerializeField]
	private TileSkin prefab;
	[SerializeField]
	private string itemID;

	public ShopItemTile(int _index, string _name, int _cost, int _starReq, Image _icon, TileSkin _prefab, string _itemID)
	{
		index=_index;
		name=_name;
		cost=_cost;
		starReq=_starReq;
		icon=_icon;
		prefab=_prefab;
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

	public Image Icon{
		get {return icon;}
		set {icon = value;}
	}

	public TileSkin Prefab{
		get {return prefab;}
		set {prefab = value;}
	}

	public int StarReq
	{
		get {return starReq;}
		set {starReq = value;}
	}

	public string ItemID
	{
		get {return itemID;}
		set {itemID = value;}
	}

	

}
