using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBar : MonoBehaviour {

	public float minWidth = 65f;
	public float maxWidth = 350f;

	public GameObject fillerImage;

	public float _currentSize = 0f;
	private RectTransform _fillerRect;

	public float size;

	public bool star1 = false;
	public bool star2 = false;
	public bool star3 = false;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
        _fillerRect = fillerImage.GetComponent<RectTransform> ();
		_fillerRect.sizeDelta = new Vector2(minWidth, _fillerRect.sizeDelta.y);
		_currentSize = 0;
		star1 = false;
		star2 = false;
		star3 = false;
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		// if(size>2.20f)
		// 	return;

		// if(size >= 1.23f && !star1)
		// {
		// 	star1=true;
		// 	GUI_Controller.instance.ActivateStar(1);
		// }

		// if(size >= 1.56f && !star2)
		// {
		// 	star2=true;
		// 	GUI_Controller.instance.ActivateStar(2);

		// }

		// if(GameMaster.instance.player1Score >= GameMaster.instance.targetScore && !star3)
		// {
		// 	star3=true;
		// 	GUI_Controller.instance.ActivateStar(3);

		// }


		// float playerScoreF= GameMaster.instance.player1Score+0.1f;
		// size = (playerScoreF / GameMaster.instance.targetScore)*2;

		// if (_currentSize < size) {
		// 	_currentSize += Time.deltaTime*0.15f;
		// 	_fillerRect.sizeDelta = new Vector2(minWidth+(maxWidth - minWidth)*_currentSize, _fillerRect.sizeDelta.y);
		// }
	}
}
