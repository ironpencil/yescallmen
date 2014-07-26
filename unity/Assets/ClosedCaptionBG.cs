using UnityEngine;
using System.Collections;

public class ClosedCaptionBG : MonoBehaviour {

    public UILabel captionLabel;
    public UISprite spriteScript;

	// Use this for initialization
	void Start () {
        spriteScript = gameObject.GetComponent<UISprite>();
	}
	
	// Update is called once per frame
	void Update () {

        if (captionLabel.text.Length == 0)
        {
            spriteScript.alpha = 0.0f;
        }
        else
        {
            spriteScript.alpha = 1.0f;
        }
	}
}
