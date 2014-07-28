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

    public void ShowCredits()
    {
        Show(credits);
    }

    public void ShowRewardsTutorial()
    {
        Show(endBattleTutorial);
    }

    public void ShowLevelingTutorial()
    {
        Show(endShowTutorial);
    }

#region tutorial text

    private string endShowTutorial = @"As you finish shows, you will get access to higher level versions of Spite and Argument cards.

Higher level Spite cards provide more Spite, but not damage.

Higher level Argument cards use more Spite, but do more damage and may be stronger in other ways!

Some card effects may even level up cards you already have!

New Card Type
Special - Play for free";

    private string endBattleTutorial = @"After a call, you can select a card to add to your deck.

New Card Type
Action - Uses an Action. You start with 1 Action a turn.

New Effects
Trash - Removes a card from your deck [b]permanently[/b]!
Gain - Adds a new card to your deck [b]permanently[/b]!

Your deck saves between calls, shows, and game sessions!

Try gaining a card by dragging it to the discard pile.";

    private string credits = @"Iron Pencil is Jim South (sighnoceros on SA)


Special Thanks:

Everdraed - Testing and Feedback

and #SAGameDev on synirc.net";


    private string introTutorial = @"Welcome!

Drag cards up to your ~anime~ playmat to play them.

Card Types
Spite - Play for free
Argument - Uses Spite

Discard Pile is shuffled to refill Draw Pile when empty.

Do damage to fill the caller's Shame before they fill your Anger!";
#endregion

}
