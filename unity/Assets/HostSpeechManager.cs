using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HostSpeechManager : MonoBehaviour {

    public List<Sprite> speakingSprites;

    private UISprite spriteScript;

    public float BaseAnimationDelay = 0.15f;

    public float AnimationDelayRangeOffset = 0.05f;

    public float AnimationStartDelay = 0.05f;

    private bool animationReady = true;

	// Use this for initialization
	void Start () {
        spriteScript = gameObject.GetComponent<UISprite>();
        
	}
	
	// Update is called once per frame
	void Update () {

        //if (GameMessageManager.gameMessageManager.AlwaysDoHostMumble ||
        //    (GameMessageManager.gameMessageManager.CurrentSpeaker == GameMessageManager.Speaker.Host &&
        //    !GameMessageManager.gameMessageManager.IsFinished))
        //{
        //    if (animationReady)
        //    {
        if (ShouldAnimate())
        {
            StartCoroutine(AnimateHost());
        }
        //    }
        //}
	}

    private bool ShouldAnimate()
    {
        if (forceAnimate ||
            GameMessageManager.gameMessageManager.AlwaysDoHostMumble ||
            (GameMessageManager.gameMessageManager.CurrentSpeaker == GameMessageManager.Speaker.Host &&
            !GameMessageManager.gameMessageManager.IsFinished))
        {
            return animationReady;
        }

        return false;
    }

    public IEnumerator AnimateHost()
    {
        animationReady = false;

        float randomDelay = Random.Range(AnimationStartDelay - AnimationDelayRangeOffset,
            AnimationStartDelay + AnimationDelayRangeOffset);

        //don't start animating immediately
        yield return new WaitForSeconds(randomDelay);

        //start at the second sprite, because we assume we were already at the beginning sprite after 1 cycle
        for (int i = 1; i < speakingSprites.Count; i++)
        {
            spriteScript.spriteName = speakingSprites[i].name;

            randomDelay = Random.Range(BaseAnimationDelay - AnimationDelayRangeOffset,
                BaseAnimationDelay + AnimationDelayRangeOffset);

            yield return new WaitForSeconds(randomDelay);
        }

        //did 1 animation cycle, now set it back to beginning
        spriteScript.spriteName = speakingSprites[0].name;

        animationReady = true;
    }

    private bool forceAnimate = false;

    public void ForceAnimate(float duration)
    {
        ForceAnimate(duration, AnimationStartDelay, BaseAnimationDelay, AnimationDelayRangeOffset);
    }

    public void ForceAnimate(float duration, float beginDelay, float animateDelay, float delayRange)
    {
        //can't start a new forced animation while one is already playing
        if (forceAnimate) { return; }

        //cache the original animation timing values so we don't lose them
        cachedStartDelay = AnimationStartDelay;
        cachedAnimateDelay = BaseAnimationDelay;
        cachedDelayRange = AnimationDelayRangeOffset;

        //update animation timing values
        AnimationStartDelay = beginDelay;
        BaseAnimationDelay = animateDelay;
        AnimationDelayRangeOffset = delayRange;

        forceAnimate = true;

        StartCoroutine(ResetForceAnimate(duration));
    }

    private IEnumerator ResetForceAnimate(float delay)
    {
        yield return new WaitForSeconds(delay);

        forceAnimate = false;

        //restore cached animation timing values
        AnimationStartDelay = cachedStartDelay;
        BaseAnimationDelay = cachedAnimateDelay;
        AnimationDelayRangeOffset = cachedDelayRange;
    }

    private float cachedStartDelay = 0.0f;
    private float cachedAnimateDelay = 0.0f;
    private float cachedDelayRange = 0.0f;

}
