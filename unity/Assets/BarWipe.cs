using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BarWipe : MonoBehaviour {

    public static BarWipe instance;

    UIPanel transitionPanel;

    public UIPanel TransitionPanel { get { return transitionPanel; } }

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

        float wipeDuration = 0.0f;
        if (gameObject.collider != null)
        {
            gameObject.collider.enabled = true;
        }

        foreach (TweenScale tween in barTweens)
        {
            wipeDuration = tween.duration;
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

        StartCoroutine(DisableCollider(wipeDuration));
    }

    private IEnumerator DisableCollider(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (gameObject.collider != null)
        {
            gameObject.collider.enabled = false;
        }
    }

    public void DoWipe(bool wipeIn, float duration)
    {
        transitionPanel.alpha = 1.0f;

        if (gameObject.collider != null)
        {
            collider.enabled = true;
        }

        foreach (TweenScale tween in barTweens)
        {
            tween.delay = 0.0f;
            tween.duration = duration;

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

        StartCoroutine(DisableCollider(duration));
    }


}
