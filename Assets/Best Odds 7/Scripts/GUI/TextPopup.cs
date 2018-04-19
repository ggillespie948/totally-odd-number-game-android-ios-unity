using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPopup : MonoBehaviour {

	public Animator animator;
	public TextMeshProUGUI popupText;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		//Get the animation object of text
		AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
		Destroy(gameObject, clipInfo[0].clip.length - 0.2f);
	}

	public void SetText(string text)
	{
		popupText.text = text;

	}

	public void SetColour(Color colour)
	{
		popupText.color = colour;

	}

	public void SetSize(int size)
	{
		if(size<30)
			size=30;
			
		popupText.fontSize = size;
	}

	public void SetFont(TMP_FontAsset font)
	{
		popupText.font = font;

	}	
}
