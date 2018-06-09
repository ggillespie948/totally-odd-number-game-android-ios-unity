using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRaycastController : MonoBehaviour {
	
	[Header("Button Interaction Settings")]
	[SerializeField]
	private bool componentBasedSelection = true;



	private bool isPressing;
	private int currentInstance;
	private int stayCheckCooldown;
	private GameObject currentButtonHit;
	void Start () 
	{
		
	}
	
	void Update () 
	{


		if (Input.GetMouseButtonDown(0)) 
		{
			RaycastHit hit;

			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) 
			{
				if(componentBasedSelection)
				{
					if(hit.transform.gameObject.GetComponent<ButtonLogic>() != null)
					{
						currentInstance = hit.transform.gameObject.GetInstanceID();
						currentButtonHit = hit.transform.gameObject;

						isPressing = true;
						hit.transform.gameObject.GetComponent<ButtonLogic>().OnClick();
						stayCheckCooldown = 10;
					}
				}
				else
				{
					if(hit.transform.gameObject.tag == "button3D")
					{
						currentInstance = hit.transform.gameObject.GetInstanceID();
						currentButtonHit = hit.transform.gameObject;

						isPressing = true;
						hit.transform.gameObject.GetComponent<ButtonLogic>().OnClick();
						stayCheckCooldown = 10;
					}
				}
			}
		}
		if (Input.GetMouseButton(0)) 
		{
			if(stayCheckCooldown < 1)
			{
				RaycastHit hit;
				if(isPressing)
				{
					if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) 
					{
						if(componentBasedSelection)
						{
							if(hit.transform.gameObject.GetComponent<ButtonLogic>() != null && currentInstance == hit.transform.gameObject.GetInstanceID())
							{
								hit.transform.gameObject.GetComponent<ButtonLogic>().OnClickStay();
							}
							else
							{
								isPressing = false;
								currentButtonHit.GetComponent<ButtonLogic>().OnClickLeave();
								currentButtonHit.GetComponent<ButtonLogic>().ButtonNoLongerPressed();
								currentButtonHit = null;
							}
						}
						else
						{
							//Tag not Component Check

							if(hit.transform.gameObject.tag == "button3D" && currentInstance == hit.transform.gameObject.GetInstanceID())
							{
								hit.transform.gameObject.GetComponent<ButtonLogic>().OnClickStay();
							}
							else
							{
								isPressing = false;
								currentButtonHit.GetComponent<ButtonLogic>().OnClickLeave();
								currentButtonHit.GetComponent<ButtonLogic>().ButtonNoLongerPressed();
								currentButtonHit = null;
							}
						}
					}
					else
					{
						isPressing = false;
						currentButtonHit.GetComponent<ButtonLogic>().OnClickLeave();
						currentButtonHit.GetComponent<ButtonLogic>().ButtonNoLongerPressed();
						currentButtonHit = null;
					}
				}
			}
			else
			{
				stayCheckCooldown--;
			}

		}
		if (Input.GetMouseButtonUp(0)) 
		{
			RaycastHit hit;
			if(isPressing)
			{
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) 
				{
					if(componentBasedSelection)
					{
						if(hit.transform.gameObject.GetComponent<ButtonLogic>() != null && currentInstance == hit.transform.gameObject.GetInstanceID())
						{
							hit.transform.gameObject.GetComponent<ButtonLogic>().OnClickLeave();
							currentButtonHit.GetComponent<ButtonLogic>().ButtonNoLongerPressed();
							currentButtonHit = null;
						}
					}
					else
					{
						//Tag not Component Check

						if(hit.transform.gameObject.tag == "button3D" && currentInstance == hit.transform.gameObject.GetInstanceID())
						{
							hit.transform.gameObject.GetComponent<ButtonLogic>().OnClickLeave();
							currentButtonHit.GetComponent<ButtonLogic>().ButtonNoLongerPressed();
							currentButtonHit = null;
						}
					}
				}
			}
			isPressing = false;
			if(currentButtonHit != null)
			{
				currentButtonHit.GetComponent<ButtonLogic>().ButtonNoLongerPressed();
				currentButtonHit = null;
			}
		}
	}
}
