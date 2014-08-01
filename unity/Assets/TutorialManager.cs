using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour {

    public static TutorialManager instance;

    public UITweener MessageBoxTween;

    public UILabel MessageLabel;

    public Collider BlockingCollider;

    private bool startStateMachineOnClose = false;

    public bool CanPlaySpite = true;
    public bool CanPlayFatigue = true;
    public bool CanPlayConfusion = true;
    public bool CanPlayAllSpite = true;
    public bool CanEndTurn = true;

	// Use this for initialization
	void Start () {
        instance = this;

        MessageBoxTween.gameObject.transform.localScale = Vector3.zero;

        if (Globals.GetInstance().DoIntroTutorial)
        {
            startStateMachineOnClose = true;
            StartCoroutine(ShowIntroTutorial());
        } else if (Globals.GetInstance().PlayerLevel == 1)
        {
            //show refresher tutorial
            Show(refreshTutorial, 0.5f);
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
        Show(text, 0.0f);
    }

    public void Show(string text, float delay)
    {
        StartCoroutine(DoShowDialog(text, delay));
    }

    private IEnumerator DoShowDialog(string text, float delay)
    {
        BlockingCollider.enabled = true;

        yield return new WaitForSeconds(delay);

        NGUITools.SetActive(MessageBoxTween.gameObject, true);

        MessageLabel.text = text;
        MessageBoxTween.PlayForward();

        SFXManager.instance.PlaySound(SFXManager.instance.WindowOpen, 1.0f);
        
    }

    public void Close()
    {
        BlockingCollider.enabled = false;
        MessageBoxTween.PlayReverse();

        SFXManager.instance.PlaySound(SFXManager.instance.WindowClose, 1.0f);

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
        Show(endBattleTutorial, 1.0f);
    }

    public void ShowLevelingTutorial()
    {
        Show(endShowTutorial, 1.0f);
    }

    public IEnumerator ShowIntroTutorial()
    {
        //only allow the player to start by playing a Spite card
        CanPlaySpite = true;
        CanPlayAllSpite = false;
        CanPlayFatigue = false;
        CanPlayConfusion = false;
        CanEndTurn = false;
        
        Show(newDeckIntroSpite, 0.5f);

        while (!ContinueTutorial)
        {
            yield return new WaitForSeconds(0.1f);
        }
        ContinueTutorial = false;

        //now let the player only Play All Spite
        CanPlaySpite = false;        
        CanPlayAllSpite = true;

        Show(newDeckIntroAllSpite, 1.0f);

        while (!ContinueTutorial)
        {
            yield return new WaitForSeconds(0.1f);
        }
        ContinueTutorial = false;

        //now let the player only play Fatigue argument
        CanPlayAllSpite = false;
        CanPlayFatigue = true;

        Show(newDeckIntroArgument, 1.0f);

        while (!ContinueTutorial)
        {
            yield return new WaitForSeconds(0.1f);
        }
        ContinueTutorial = false;

        //now let the player only play a Spite card again
        CanPlayFatigue = false;
        CanPlaySpite = true;

        Show(newDeckIntroSpite2, 1.2f);

        while (!ContinueTutorial)
        {
            yield return new WaitForSeconds(0.1f);
        }
        ContinueTutorial = false;

        //now let the player only play Circumcision
        CanPlaySpite = false;
        CanPlayConfusion = true;

        Show(newDeckIntroConfusion, 1.0f);

        while (!ContinueTutorial)
        {
            yield return new WaitForSeconds(0.1f);
        }
        ContinueTutorial = false;

        //now let the player end turn
        CanEndTurn = true;
        CanPlayConfusion = false;

        Show(newDeckIntroEndTurn, 2.0f);

        while (!ContinueTutorial)
        {
            yield return new WaitForSeconds(0.1f);
        }
        ContinueTutorial = false;

        //now let the player do everything
        CanPlaySpite = true;
        CanPlayAllSpite = true;
        CanPlayFatigue = true;
        CanPlayConfusion = true;
        CanEndTurn = true;

        Globals.GetInstance().DoIntroTutorial = false;

    }


    public bool ContinueTutorial = false;

#region tutorial text

    private string newDeckIntroSpite = @"Welcome to #YesCallMen!

Click cards to play them to your ~anime~ play mat.

Play Spite cards to generate Spite for your arguments.

Try playing a Spite card now.";   

    private string newDeckIntroAllSpite = @"Good job!

You can play as many Spite cards as you want each turn.

They do a little damage too, so play them
even if you don't need the Spite!

Try playing out the rest of your Spite cards
by hitting the Play All Spite button.";

    private string newDeckIntroArgument = @"Great, now you have 3 Spite!

Argument cards use Spite.

They do damage and often have other effects.

Divorce Laws says +1 Card, which means
you get to draw a card after playing it!

Don't worry about +1 Action for now.

Try playing Divorce Laws.";

    private string newDeckIntroSpite2 = @"Good!

Damage increases the Caller's Shame, which is
displayed in Call Info in the upper right.

Max out a Caller's Shame to make them hang up,
but do it before they max out your Anger!

Play the new Spite card you drew.";

    private string newDeckIntroConfusion = @"Great!

You can play as many Arguments as you want each turn
as long as you have the Spite to pay for them.

Circumcision reveals the top card of the deck.
If it is an Argument card, you get to play it for free!

Try playing Circumcision.";

    private string newDeckIntroEndTurn = @"Excellent!

Circumcision revealed an Argument and played it for free!

When you end your turn, everything gets discarded
and you draw a new hand.

When the draw pile is empty, the discard is shuffled to refill it.

Try clicking End Turn now. Good luck!";

    private string endShowTutorial = @"As you finish shows, you will get access to higher level versions of Spite and Argument cards.

Higher level Spite cards provide more Spite.

Higher level Argument cards use more Spite, but do more damage and may be stronger in other ways too!

Some card effects may even level up cards you already have!

New Card Type
Special - Play for free";

    private string endBattleTutorial = @"After a call, you can select a card to add to your deck.

Action cards use an Action instead of Spite. You start
with 1 Action each turn, but some cards give you more.

New Effects
Trash - Removes a card from your deck [b]permanently[/b]!
Gain - Adds a new card to your deck [b]permanently[/b]!

Your deck saves between calls, shows, and game sessions!

Try adding a new card to your deck by clicking it.";

    private string credits = @"[b]Iron Pencil is:[/b]

The Contemptuous Mr. Jim South : Design, Code, Art, Sound
www.ironpencil.com

[b]With Special Thanks To:[/b]

The Bewildering Mr. Austin Thresher : 'Casio Lines'
dangermomentum.tumblr.com

The Deplorable Mr. Everdraed : Voice, Title Art

The Enigmatic Mr. Fangz : Title Art

and #SAGameDev on synIRC.net

Most card sounds by www.kenney.nl";


    private string refreshTutorial = @"Welcome back to #YesCallMen!

Click cards to play them.

Card Types
Spite - Play for free
Argument - Uses Spite
Action - Uses Actions

Discard Pile is shuffled to refill Draw Pile when empty.

Do damage to fill the Caller's Shame before they fill your Anger!";
#endregion

}
