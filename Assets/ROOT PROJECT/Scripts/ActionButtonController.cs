using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButtonController : MonoBehaviour {

	[SerializeField]
	private GameObject tilesLeftBtn;
	[SerializeField]
	private GameObject submitBtn;
	[SerializeField]
	private GameObject menuBtn;
	[SerializeField]
	private GameObject swapTileBtn;
	
	// Use this for initialization
	void Start () {
		SetResolutionDefault();
	}
	
	public void SetResolutionDefault()
	{

		float aspect = Camera.main.aspect;

		if(aspect < 0.47f)
		{
			//Debug.Log("Set Xtra Slim Resolution Default"); //iphone X
			tilesLeftBtn.transform.localPosition=new Vector3(-3.48f,0.08f,4.4f);
				submitBtn.transform.localPosition=new Vector3(-1.06f,-0.12f,4.4f);
				menuBtn.transform.localPosition=new Vector3(1.47f,0.06f,4.4f);
				swapTileBtn.transform.localPosition=new Vector3(-3.27f,-9.2f,4.4f);
			

		} else if (aspect < 0.51f)
		{
			//Debug.Log("Set  Slim Resolution Default"); //s8 s9
			tilesLeftBtn.transform.localPosition=new Vector3(-3.589f,0.08f,4.4f);
				submitBtn.transform.localPosition=new Vector3(-1.08f,-0.12f,4.4f);
				menuBtn.transform.localPosition=new Vector3(1.54f,0.069f,4.4f);
				swapTileBtn.transform.localPosition=new Vector3(-3.41f,-9.13f,4.4f);
			

		} else if (aspect < 0.65f)
		{
			//Debug.Log("Set Default Resolution Default"); //s6 s7
			

		} else if (aspect > 0.7f)
		{
			//Debug.Log("Set Wide Resolution Default");
			tilesLeftBtn.transform.localPosition=new Vector3(-4.28f,0.08f,4.4f);
			submitBtn.transform.localPosition=new Vector3(-1.06f,-0.12f,4.4f);
			menuBtn.transform.localPosition=new Vector3(2.3f,0.06f,4.4f);
			swapTileBtn.transform.localPosition=new Vector3(-5.06f,-9.2f,4.4f);
			

		} else 
		{
			//Debug.Log("Set Default Resolution Default");
			
			

		}




		if(Screen.height > 2400)
		{
            if(Screen.width < 1130)
            {
                Debug.Log("Set Xtra Slim Resolution Default"); //iphone X
				
                

            } else if(Screen.height > 2600)
            {
                Debug.Log("Set  Slim Resolution Default"); //s8 s9
				
				
            } else 
            {
                Debug.Log("Set Default Resolution Default"); //s6 s7
			    //GetComponent<Animator>().SetTrigger("default");
				

            }

		} else if(Screen.width > 1500)
		{
			Debug.Log("Set Wide Resolution Default");
			

		} else 
		{
			Debug.Log("Set Default Resolution Default");
			
		}

	}


	public void HideTilesLeft()
	{
		tilesLeftBtn.SetActive(false);

	}
}
