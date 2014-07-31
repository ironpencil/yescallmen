using UnityEngine;
using System.Collections;

public class TVTone : MonoBehaviour {

    public UIWidget AlphaFollower;
    public AudioSource ToneSource;
    public float VolumeFactor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        ToneSource.volume = AlphaFollower.alpha * VolumeFactor * Globals.GetInstance().InitialAudioVolume;
	}

    public void StartTone()
    {
        ToneSource.Play();
    }
}
