using UnityEngine;
using System.Collections;

public class ControlVolume : MonoBehaviour {

    public Sprite MutedSprite;
    public Sprite UnmutedSprite;
    public UISprite Icon;

    private UISlider slider;

    public enum SliderType
    {
        Music,
        SFX
    }

    public SliderType sliderType = SliderType.Music;

	// Use this for initialization
	void Start () {

        slider = gameObject.GetComponent<UISlider>();

        switch (sliderType)
        {
            case SliderType.Music:
                slider.value = Globals.GetInstance().GlobalMusicVolume;
                break;
            case SliderType.SFX:
                slider.value = Globals.GetInstance().GlobalSFXVolume;
                break;
            default:
                break;
        }

        UpdateSprites();
        
	}

    public void OnSliderChange()
    {
        switch (sliderType)
        {
            case SliderType.Music:
                Globals.GetInstance().GlobalMusicVolume = slider.value;
                break;
            case SliderType.SFX:
                Globals.GetInstance().GlobalSFXVolume = slider.value;
                break;
            default:
                break;
        }

        UpdateSprites();
        //AudioListener.volume = slider.value;
    }

    public void UpdateSprites()
    {
        if (Icon != null)
        {
            if (Mathf.Approximately(slider.value, 0.0f))
            {
                if (MutedSprite != null)
                {
                    Icon.spriteName = MutedSprite.name;
                }
            }
            else
            {
                if (UnmutedSprite != null)
                {
                    Icon.spriteName = UnmutedSprite.name;
                }
            }
        }
    }

}
