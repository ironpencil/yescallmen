using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HostClicker : MonoBehaviour {

    public List<AudioClip> clips;

    public float AnimationDuration = 2.0f;

    public float ForcedBeginDelay = 0.2f;
    public float ForcedAnimDelay = 1.0f;
    public float ForcedAnimRange = 0.0f;

    public float StartTimer = 3.0f;

    public bool CanEnableAlwaysMumble = true;

    public HostSpeechManager speechManager;

	// Use this for initialization
	void Start () {

        StartCoroutine(BeginStartTimer());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private IEnumerator BeginStartTimer()
    {
        //will just decrease timer until it is less than zero
        while (StartTimer > 0.0f)
        {
            yield return null;
            StartTimer = StartTimer - Time.deltaTime;
        }
    }

    bool canPlaySound = true;

    void OnClick()
    {
        if (!(StartTimer > 0.0f))
        {
            if (canPlaySound && GameMessageManager.gameMessageManager.IsFinished)
            {
                canPlaySound = false;
                StartCoroutine(PlayHostSounds());
            }
        }
    }

    void OnDoubleClick()
    {
        if (CanEnableAlwaysMumble)
        {
            if (!(StartTimer > 0.0f))
            {
                if (GameMessageManager.gameMessageManager.AlwaysDoHostMumble)
                {
                    StartCoroutine(DisableAlwaysMumble());
                }
                else
                {
                    StartCoroutine(EnableAlwaysMumble());
                }
            }
        }
    }

    private IEnumerator EnableAlwaysMumble()
    {
        //if we're toggling on, wait for any current sounds to finish

        while (!canPlaySound)
        {
            yield return null;
        }

        canPlaySound = false; //disable clicking to play single shot sounds

        GameMessageManager.gameMessageManager.AlwaysDoHostMumble = true;
    }

    private IEnumerator DisableAlwaysMumble()
    {
        //if we're toggling off, turn it off immediately
        GameMessageManager.gameMessageManager.AlwaysDoHostMumble = false;

        //enable clicking to play single shot sounds
        canPlaySound = true;

        yield return null;
    }


    public float ClipDelayOffset = -0.1f;

    private IEnumerator PlayHostSounds()
    {
        if (speechManager.CanForceNewAnimation)
        {
            speechManager.ForceAnimate(AnimationDuration, ForcedBeginDelay, ForcedAnimDelay, ForcedAnimRange);

            foreach (AudioClip clip in clips)
            {
                Globals.GetInstance().SFXSource.PlayOneShot(clip);
                yield return new WaitForSeconds(clip.length + ClipDelayOffset);
            }

        }

        canPlaySound = true;
    }


}
