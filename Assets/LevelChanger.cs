using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {

	public Animator animator;
	private string levelToLoad;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		animator=GetComponent<Animator>();
	}

	public void FadeToLevel(string levelIndex)
	{
		levelToLoad=levelIndex;
		animator.SetTrigger("FadeOut");
	}

	public void OnFadeComplete()
	{
		SceneManager.LoadScene(levelToLoad);
	}
	
}
