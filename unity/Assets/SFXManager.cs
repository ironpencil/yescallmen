using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SFXManager : MonoBehaviour {

    public static SFXManager instance;

    public AudioClip PlayCardSound;
    public AudioClip RolloverCardSound;

    public AudioClip ReshuffleSound;

    public List<AudioClip> SlidingSounds;
    public List<AudioClip> DrawingSounds;

	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlaySound(AudioClip sound, float volume)
    {
        Globals.GetInstance().SFXSource.PlayOneShot(sound, volume);
    }

    public void QueueSound(AudioClip sound, float volume, float delay)
    {
        StartCoroutine(PlaySoundAfterDelay(sound, volume, delay));
    }

    public IEnumerator PlaySoundAfterDelay(AudioClip sound, float volume, float delay)
    {
        yield return new WaitForSeconds(delay);

        PlaySound(sound, volume);
    }

    public AudioClip RandomSlidingSound
    {
        get
        {
            int randomIndex = UnityEngine.Random.Range(0, SlidingSounds.Count);
            return SlidingSounds[randomIndex];
        }
    }

    public AudioClip RandomDrawingSound
    {
        get
        {
            int randomIndex = UnityEngine.Random.Range(0, DrawingSounds.Count);
            return DrawingSounds[randomIndex];
        }
    }
}
