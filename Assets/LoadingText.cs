using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingText : MonoBehaviour {

	public bool active=true;
	private TextMeshProUGUI ui_text;
	private string text;
	private int elipses;
	private int phraseCount;
	private int loopCount;

	[SerializeField]
	private string[] phrases;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		ui_text=GetComponent<TextMeshProUGUI>();
		elipses=0;
		loopCount=0;
		text=phrases[0];
		phraseCount=0;
		ui_text.text=text;
		StartCoroutine(AddEllipsis());
	}

	public IEnumerator AddEllipsis()
	{
		while(active)
		{
			if(elipses<3)
			{
				ui_text.text+=".";
				elipses++;
			} else {
				loopCount++;

				if(loopCount>1)
				{
					if(phraseCount<phrases.Length-1)
					{
						phraseCount++;
					} else{
						phraseCount=0;
					}
					text=phrases[phraseCount];
					ui_text.text=text;
					elipses=0;
					loopCount=0;
				} else{
					text=phrases[phraseCount];
					ui_text.text=text;
					elipses=0;
				}

			}
		yield return new WaitForSeconds(.75f);
		}

		
	}

	
}
