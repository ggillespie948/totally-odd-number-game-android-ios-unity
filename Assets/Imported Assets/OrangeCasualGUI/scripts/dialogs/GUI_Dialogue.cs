using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Dialogue : MonoBehaviour {

	public GameObject dialogRoot;

    protected bool _isOpened = false;

    public void Close() {
         if(_isOpened)
         {
            Animator animator = gameObject.GetComponent<Animator>();
            animator.enabled = true;
            animator.Play("dialog_hide", 0);
            _isOpened = false;
         } else 
         {
             Debug.LogError("Trying to close unopen dialogue");
         }

        if(GameMaster.instance != null) { GameMaster.instance.ResumeGame(); }
		
		
    }
        
    public void CloseComplete() {
        if(dialogRoot)
            dialogRoot.SetActive(false);
        else
            gameObject.SetActive(false);
        Animator animator = gameObject.GetComponent<Animator>();
        animator.enabled = false;
    }
	virtual public void Open() {
        if(_isOpened)
            return;
        if(dialogRoot)
            dialogRoot.SetActive(true);
        else
            gameObject.SetActive(true);
        Animator animator = gameObject.GetComponent<Animator>();
        animator.enabled = true;
        animator.Play("dialog_open", 0);
        _isOpened = true;
    }

    virtual public void OpenComplete() {
        Animator animator = gameObject.GetComponent<Animator>();
        animator.enabled = false;
    }
}
