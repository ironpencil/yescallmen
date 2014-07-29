using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HostClicker : MonoBehaviour {

    public List<AudioClip> clips;

    public AudioSource source;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    bool canPlaySound = true;

    void OnClick()
    {
        if (canPlaySound && GameMessageManager.gameMessageManager.IsFinished)
        {
            canPlaySound = false;
            StartCoroutine(PlayHostSounds());
        }
    }

    public float ClipDelayOffset = -0.1f;

    private IEnumerator PlayHostSounds()
    {
        foreach (AudioClip clip in clips)
        {
            source.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length + ClipDelayOffset);
        }

        canPlaySound = true;
    }


}
