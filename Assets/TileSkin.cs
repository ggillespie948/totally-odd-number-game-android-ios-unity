using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileSkin : MonoBehaviour {

	// Use this for initialization
	[SerializeField]
	public Material[] defaultSkins;

	[SerializeField]
	public Material[] activeSkins;

	[SerializeField]
	public GameObject[] activateFX;


	public GridTile Tile1;
	public GridTile Tile2;
	public GridTile Tile3;

	public GridTile Tile4;
	public GridTile Tile5;
	public GridTile Tile6;

	public GridTile Tile7;

	public GridTile Tile8;

	public GridTile Tile9;

	public GameObject Tile1FX;
	public GameObject Tile2FX;
	public GameObject Tile3FX;
	public GameObject Tile4FX;
	public GameObject Tile5FX;
	public GameObject Tile6FX;
	public GameObject Tile7FX;
	public GameObject Tile8FX;
	public GameObject Tile9FX;
	public Color tileSkinCol;
	public Image icon;

	public List<GridTile> AllTiles()
	{
		List<GridTile> list = new List<GridTile>();
		list.Add(Tile1);
		list.Add(Tile2);
		list.Add(Tile3);
		list.Add(Tile4);
		list.Add(Tile5);
		list.Add(Tile6);
		list.Add(Tile7);
		list.Add(Tile8);
		list.Add(Tile9);
		return list;

	}
}
