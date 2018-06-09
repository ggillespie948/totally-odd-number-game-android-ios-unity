using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ButtonLogic : MonoBehaviour {

	[Header("Target Method")]
	[SerializeField]
	private UnityEvent MethodsToCallMouseDown;
	[SerializeField]
	private UnityEvent MethodsToCallMouseStay;
	[SerializeField]
	private UnityEvent MethodsToCallMouseUp;



	[Header("Button Animation Properties")]
	[SerializeField]
	private GameObject buttonMeshObject;
	[SerializeField]
	private float downTravelSpeed = 1f;
	[SerializeField]
	private float upTravelSpeed = 0.01f;
	[SerializeField]
	private float buttonTravelDistance = 50f;
	private float currentDestination = 0;
	private Vector3 startPos;




	// Use this for initialization
	void Start () 
	{
		if(buttonMeshObject != null)
		{
			startPos = buttonMeshObject.transform.localPosition;
		}

		//gameObject.tag = "button3D"; //This is required if the tag based solution is selected. Can be disabled once it has been set, it's simply just to ensure that the user (you), is aware of this tag's importance.

	}
	



	public void OnClick()
	{
		MethodsToCallMouseDown.Invoke();
		if(buttonMeshObject != null)
		{
			StartCoroutine("AnimateButtonDown");
		}
	}
	public void OnClickStay()
	{
		MethodsToCallMouseStay.Invoke();
	}
	public void OnClickLeave()
	{
		MethodsToCallMouseUp.Invoke();

	}
	public void ButtonNoLongerPressed()
	{
		Debug.Log("Check");
		if(buttonMeshObject != null)
		{
			StartCoroutine("AnimateButtonUp");
		}
	}


	IEnumerator AnimateButtonDown() {
		StopCoroutine("AnimateButtonUp");
		float travelled = 0;
		buttonMeshObject.transform.localPosition = startPos;
		while (travelled < buttonTravelDistance) {
			travelled += downTravelSpeed;
			buttonMeshObject.transform.localPosition -= new Vector3(0,1,0) * downTravelSpeed;
			yield return null;
		}
	}
	IEnumerator AnimateButtonUp() {
		float travelled = 0;
		while (travelled < buttonTravelDistance) {
			travelled += upTravelSpeed;
			buttonMeshObject.transform.localPosition += new Vector3(0,1,0) * upTravelSpeed;
			yield return null;
		}
		buttonMeshObject.transform.localPosition = startPos;
	}
}
