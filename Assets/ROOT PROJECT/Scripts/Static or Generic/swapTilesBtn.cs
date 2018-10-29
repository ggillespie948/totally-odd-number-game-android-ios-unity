using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swapTilesBtn : MonoBehaviour {

	// Use this for initialization

	[SerializeField]
	private BoxCollider collider;

	void Start () {


		if(ApplicationModel.LEVEL_CODE=="B1" && ApplicationModel.TUTORIAL_MODE)
		{
			Debug.Log("Disablign SWap collider");
			collider.enabled=false;

		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UnlockSubmit()
	{
		Debug.Log("UNLOKCING  SWap collider");
		collider.enabled=true;
	}

	public void InitlockSubmit()
	{
		if(ApplicationModel.LEVEL_CODE=="B1")
		{
			Debug.Log("LOCKING  SWap collider");
			collider.enabled=false;
		}
	}

	public void LockSubmit()
	{
		collider.enabled=false;
		StartCoroutine(unlockSubmitAfterDelay());
	}

	private IEnumerator unlockSubmitAfterDelay()
	{
		yield return new WaitForSeconds(3f);
		collider.enabled=true;

	}
}
