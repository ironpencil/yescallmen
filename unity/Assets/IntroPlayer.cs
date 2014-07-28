using UnityEngine;
using System.Collections;

public class IntroPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        TitleScreenManager.instance.WipeToTitle();
    }
}
