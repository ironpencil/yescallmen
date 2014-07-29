using UnityEngine;
using System.Collections;

public class ControlVolume : MonoBehaviour {

    private UISlider slider;

	// Use this for initialization
	void Start () {

        slider = gameObject.GetComponent<UISlider>();
        slider.value = AudioListener.volume;
	}

    public void OnSliderChange()
    {        
        AudioListener.volume = slider.value;
    }

}
