using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Theme : MonoBehaviour {

	[SerializeField]
	public string name{get; set;}

	[SerializeField]
	public int themeID;
	[SerializeField]
	public Material GridCell;
	[SerializeField]
	public Material GirdCellAlt;
	[SerializeField]
	public Material GridOutline;
	[SerializeField]
	public Material GridDivider;
	[SerializeField]
	public Material Skybox;
	[SerializeField]
	public float SunLightIntensity;
	[SerializeField]
	public float TileLightIntensity;

	[SerializeField]
	public Image Background;
	[SerializeField]
	public Color GradientVertext1;
	[SerializeField]
	public Color GradientVertext2;

	[Header("Theme Configuration")]
	[SerializeField]
	public bool gradientBgEnabled;
	[SerializeField]
	public bool gridDividerEnabled;
	[SerializeField]
	public bool skyBoxRotationEnabled;
	

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		
	}
}
