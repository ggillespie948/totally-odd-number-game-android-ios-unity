using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputDialog : MonoBehaviour {

	[SerializeField]
	private InputField input;

	public void SubmitNameDialog()
	{
		AccountInfo.Instance.SetDisplayName(input.text);
	}
	
}
