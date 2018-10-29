using UnityEngine;
using System.Collections;

public class Filler : MonoBehaviour {

	public float size = 0f;
	public float minWidth = 0f;
	public float maxWidth = 0f;

	public GameObject fillerImage;

	private float _currentSize = 0;
	private RectTransform _fillerRect;

	// Use this for initialization
	void Start () {
		_fillerRect = fillerImage.GetComponent<RectTransform> ();
		_fillerRect.sizeDelta = new Vector2(minWidth, _fillerRect.sizeDelta.y);
	}
	
	// Update is called once per frame
	void Update () {
		if (size > _currentSize) {
			_currentSize += Time.deltaTime*0.6f;
			_fillerRect.sizeDelta = new Vector2(minWidth+(maxWidth - minWidth)*_currentSize, _fillerRect.sizeDelta.y);
		}
	}
}
