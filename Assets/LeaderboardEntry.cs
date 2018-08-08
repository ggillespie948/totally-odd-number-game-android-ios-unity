using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LeaderboardEntry : MonoBehaviour {

	[SerializeField]
	public TextMeshProUGUI rankTxt;

	[SerializeField]
	public TextMeshProUGUI nameTxt;

	[SerializeField]
	public TextMeshProUGUI valueTxt;

	private Color startCol;

	[SerializeField]
	private Image entryBg;
	

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		GetBg();
		
	}

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		startCol=nameTxt.color;
	}

	public void HighlightPlayer()
	{
		if(entryBg!= null)
		{
			entryBg.color=Color.HSVToRGB(0.3989f,0.2919f,0.8196f);
		} else{
			GetBg();
		}
	}

	public void UnhighlightPlayer()
	{
		if(entryBg!= null)
		{
			entryBg.color=Color.white;
		} else 
		{
			GetBg();
		}
	}

	private void GetBg()
	{
		entryBg=GetComponent<Image>();
	}
}
