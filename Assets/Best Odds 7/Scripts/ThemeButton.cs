using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeButton : MonoBehaviour {

	public Theme Theme;

	public GameObject MM_Btn1;
	public GameObject MM_Btn2;
	public GameObject MM_Btn3;


	public GameObject MM_Btn1_Active;
	public GameObject MM_Btn2_Active;
	public GameObject MM_Btn3_Active;

	 /// <summary>
	/// OnMouseDown is called when the user has pressed the mouse button while
	/// over the GUIElement or Collider.
	/// </summary>
	void OnMouseUp()
	{
		ApplicationModel.THEME = Theme.themeID;	
		RenderSettings.skybox = Theme.Skybox;	
		GUI_Controller.instance.themeIndicator.transform.position = new Vector3(GUI_Controller.instance.themeIndicator.transform.position.x, this.gameObject.transform.position.y, GUI_Controller.instance.themeIndicator.transform.position.z);
		MenuController.instance.LoadMenuTheme(Theme);

		//Disable Previous MM tiles
		if(MenuController.instance.selectedTheme != null)
			MenuController.instance.selectedTheme.Deactivate();

		//Activate Curretn MM tiles
		Activate();
		MenuController.instance.selectedTheme = this;

	}


	public void Activate()
	{
		MM_Btn1.SetActive(false);
		MM_Btn2.SetActive(false);
		MM_Btn3.SetActive(false);

		MM_Btn1_Active.SetActive(true);
		MM_Btn2_Active.SetActive(true);
		MM_Btn3_Active.SetActive(true);

	}

	public void Deactivate()
	{
		MM_Btn1.SetActive(true);
		MM_Btn2.SetActive(true);
		MM_Btn3.SetActive(true);

		MM_Btn1_Active.SetActive(false);
		MM_Btn2_Active.SetActive(false);
		MM_Btn3_Active.SetActive(false);

	}
}
