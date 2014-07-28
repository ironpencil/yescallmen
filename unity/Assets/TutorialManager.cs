using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour {

    public static TutorialManager instance;

    public UITweener MessageBoxTween;

    public UILabel MessageLabel;

    public Collider BlockingCollider;

    private bool startStateMachineOnClose = false;

	// Use this for initialization
	void Start () {
        instance = this;

        MessageBoxTween.gameObject.transform.localScale = Vector3.zero;

        if (Globals.GetInstance().PlayerLevel == 1)
        {
            //show intro tutorial
            Show(introTutorial);
            startStateMachineOnClose = true;
        }
        else
        {
            //start State Machine
            TurnManager.turnManager.StartStateMachine();
        }
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init()
    {
        NGUITools.SetActive(gameObject, true);
    }

    public void Show(string text)
    {
        NGUITools.SetActive(MessageBoxTween.gameObject, true);        

        MessageLabel.text = text;
        MessageBoxTween.PlayForward();
        BlockingCollider.enabled = true;
    }

    public void Close()
    {
        BlockingCollider.enabled = false;
        MessageBoxTween.PlayReverse();

        if (startStateMachineOnClose)
        {
            TurnManager.turnManager.StartStateMachine();
            startStateMachineOnClose = false;
        }

        StartCoroutine(DeactivateMessagebox());
    }

    public IEnumerator DeactivateMessagebox ()
    {
        yield return new WaitForSeconds(MessageBoxTween.duration);
        yield return new WaitForSeconds(0.1f);

        NGUITools.SetActive(MessageBoxTween.gameObject, false);
    }

#region tutorial text









    private string introTutorial = @"Welcome!

Drag cards up to your ~anime~ playmat to play them.

Card Types
Spite - Play for free
Argument - Uses Spite

Discard Pile is shuffled to refill Draw Pile when empty.

Do damage to fill the caller's Shame before they fill your Anger!";
#endregion

}
