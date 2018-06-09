using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateAround : MonoBehaviour {
	[SerializeField]
	private Text statusText;
	void Start()
	{
		statusText.text = "Press A Button";
	}
	void Update() 
	{
		transform.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);
	}

	public void OnPress()
	{
		statusText.text = "PRESSED";
	}
	public void OnStay()
	{
		statusText.text = "STAY";
	}
	public void OnLeave()
	{
		statusText.text = "LEAVE";
	}

}
