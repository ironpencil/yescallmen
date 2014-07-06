using UnityEngine;
using System.Collections;

public class HoverOverCard : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //resetDepth = this.gameObject.GetComponent<UISprite>().depth;
	}    
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Play()
    {
        //TweenScale.Begin(gameObject, AnimationDuration, ScaleTo);
        //this.gameObject.GetComponent<UISprite>().depth += depthDelta;
        //GetComponentInParent<UICenterOnChild>().CenterOn(transform);

    }

    public void PlayReverse()
    {
        //TweenScale.Begin(gameObject, AnimationDuration, ScaleFrom);
        //this.gameObject.GetComponent<UISprite>().depth -= depthDelta;
        //GetComponentInParent<UICenterOnChild>().CenterOn(null);
    }
}
