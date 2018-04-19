using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderButtons : MonoBehaviour {

	public float step = 0.1f;

	public void Increase(){
		Slider slider = gameObject.GetComponent<Slider> ();
		if (slider.value + step > slider.maxValue)
			slider.value = slider.maxValue;
		else
			slider.value += step;
	}

	public void Decrease(){
		Slider slider = gameObject.GetComponent<Slider> ();
		if (slider.value - step < slider.minValue)
			slider.value = slider.minValue;
		else
			slider.value -= step;
	}
}
