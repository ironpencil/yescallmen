﻿using UnityEngine;
using System.Collections;

public class TitleScreenManager : MonoBehaviour {

    public UIButton ClearSaveDataScript;

    public UIPanel IntroPanel;

    public static TitleScreenManager instance;

    public UITweener ClickTextLabelTween;

    public UISlider VolumeSlider;

	// Use this for initialization
	void Start () {
        instance = this;

        ClearSaveDataScript.isEnabled = Globals.GetInstance().DoesSaveDataExist();

        try
        {
            if (Globals.GetInstance().AudioSource1.isPlaying ||
                Globals.GetInstance().AudioSource2.isPlaying)
            {
                Globals.GetInstance().AudioSource1.Stop();
                Globals.GetInstance().AudioSource2.Stop();
            }
        }
        catch { }

        //ClearSaveDataScript.enabled = Globals.GetInstance().DoesSaveDataExist();
	}
	
	// Update is called once per frame
	void Update () {

        ClearSaveDataScript.isEnabled = Globals.GetInstance().DoesSaveDataExist();
	}

    public void WipeToTitle()
    {
        BarWipe.instance.TransitionPanel.alpha = 1.0f;
        NGUITools.SetActive(IntroPanel.gameObject, false);

        BarWipe.instance.DoWipe(true);

        ClickTextLabelTween.PlayForward();

        StartCoroutine(StartMusic());
    }

    private IEnumerator StartMusic()
    {
        yield return new WaitForSeconds(1.0f);

        if (!Globals.GetInstance().AudioSource1.isPlaying &&
            !Globals.GetInstance().AudioSource2.isPlaying)
        {
            AudioListener.volume = Globals.GetInstance().InitialAudioVolume;
            Globals.GetInstance().AudioSource1.Play();
            Globals.GetInstance().AudioSource2.Play();
            NGUITools.SetActive(VolumeSlider.gameObject, true);
        }
    }

    public void StartGame()
    {
        StartCoroutine(DoStartGame());
    }

    private IEnumerator DoStartGame()
    {
        BarWipe.instance.DoWipe(false);

        yield return new WaitForSeconds(1.0f);

        Globals.GetInstance().LastScene = Globals.GameScene.Title;
        Application.LoadLevel("outofbattle");
    }
}
