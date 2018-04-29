using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThemeController : MonoBehaviour {

	[SerializeField]
	private TileSpawner tileSpawner1;
	[SerializeField]
	private TileSpawner tileSpawner2;

	public Theme[] themes;
	private Theme currentTheme;

	public Button[] buttons;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		
	}

	public void EquipTheme(int themeNo)
	{
		UnequipThemes();
		buttons[themeNo].GetComponentInChildren<TextMeshProUGUI>().text="Equipped";
		currentTheme = themes[themeNo];
        Debug.LogWarning("Loading Theme: " + currentTheme.name);
        RenderSettings.skybox = currentTheme.Skybox;
		ApplicationModel.THEME=themeNo;
	}

	private void UnequipThemes()
	{
		foreach(Button btn in buttons)
		{
			btn.GetComponentInChildren<TextMeshProUGUI>().text="Equip";
		}
	}


}
