using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theme : MonoBehaviour {

	public string name;

	public int themeID;

	public Material GridCell;
	public Material GirdCellAlt;
	public Material Timer;

	public Material Buttons;

	public GridTile Tile1;
	public GridTile Tile2;
	public GridTile Tile3;

	public GridTile Tile4;
	public GridTile Tile5;
	public GridTile Tile6;

	public GridTile Tile7;



	public Material Skybox;

	public float SunLightIntensity;
	public float TileLightIntensity;

	public GameObject Tile1FX;
	public GameObject Tile2FX;
	public GameObject Tile3FX;
	public GameObject Tile4FX;
	public GameObject Tile5FX;
	public GameObject Tile6FX;
	public GameObject Tile7FX;

	public Transform Background;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		if(Background!= null)
			Background.gameObject.SetActive(true);
	}
}
