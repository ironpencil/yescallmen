using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BarWipe : MonoBehaviour {

    public static BarWipe instance;


    UIPanel transitionPanel;

    TweenScale[] barTweens;
	// Use this for initialization
	void Start () {
        instance = this;

        barTweens = gameObject.GetComponentsInChildren<TweenScale>();

        transitionPanel = gameObject.GetComponent<UIPanel>();

        Debug.Log("Tweens found: " + barTweens.Length);

        
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void DoWipe(bool wipeIn)
    {
        transitionPanel.alpha = 1.0f;

        foreach (TweenScale tween in barTweens)
        {
            if (wipeIn)
            {
                //tween.transform.localScale = Vector3.one;
                tween.PlayForward();                
            }
            else
            {
                //tween.transform.localScale = Vector3.zero;
                tween.PlayReverse();
            }
        }
    }


}
